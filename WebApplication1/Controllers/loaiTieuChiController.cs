using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class loaiTieuChiController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: loaiTieuChi
        public ActionResult Index()
        {
            var tieuChi_loaiTieuChi = (from loaiTieuChi in db.loaiTieuChis
                                       join nhomChiTieu in db.nhomChiTieux
                                        on loaiTieuChi.iD equals nhomChiTieu.fk_loaiTieuChi
                                       join chiTieu in db.chiTieux
                                        on nhomChiTieu.iD equals chiTieu.fk_loaiChiTieu
                                      select new dataBangDiem
                                      {
                                          chiTieu = chiTieu,
                                          loaiTieuChi = loaiTieuChi,
                                      }).OrderBy(l=>l.loaiTieuChi.iD).DistinctBy(l=>l.loaiTieuChi.iD);
            var tieuChi_giaoChiTieu = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                       join chiTieu in db.chiTieux
                                        on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                       join nhomChiTieu in db.nhomChiTieux
                                        on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                       join loaiTieuChi in db.loaiTieuChis
                                        on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi.iD
                                       join dm_donVi in db.dm_donVi
                                        on giaoChiTieuchoDV.fk_dmDonVi equals dm_donVi.iD
                                       join nguoiDung in db.nguoiDungs
                                        on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                                       join donVi in db.donVis
                                        on nguoiDung.fk_donVi equals donVi.iD
                                       select new dataBangDiem
                                       {
                                           giaoChiTieuchoDV = giaoChiTieuchoDV,
                                           loaiTieuChi = loaiTieuChi,
                                           chiTieu = chiTieu,
                                           dm_DonVi = dm_donVi,
                                           nguoiDung = nguoiDung,
                                           donVi = donVi,
                                       }).DistinctBy(c=>c.giaoChiTieuchoDV.fk_chiTieu);
  
            ViewBag.tieuChi_loaiTieuChi = tieuChi_loaiTieuChi.ToList();
            ViewBag.tieuChi_giaoChiTieu = tieuChi_giaoChiTieu.ToList();
            return View();
        }

        // GET: loaiTieuChi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);
            if (loaiTieuChi == null)
            {
                return HttpNotFound();
            }
            return View(loaiTieuChi);
        }

        // GET: loaiTieuChi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: loaiTieuChi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD,ten,tongDiem")] loaiTieuChi loaiTieuChi)
        {
            if (ModelState.IsValid)
            {
                db.loaiTieuChis.Add(loaiTieuChi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiTieuChi);
        }

        // GET: loaiTieuChi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);
            if (loaiTieuChi == null)
            {
                return HttpNotFound();
            }
            var nguoiDungList = (from dm_donVi in db.dm_donVi
                                 join nguoiDung in db.nguoiDungs
                                    on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                                 join donVi in db.donVis
                                    on nguoiDung.fk_donVi equals donVi.iD
                                 select new
                                 {
                                     iD = dm_donVi.iD,
                                     tenNguoiDung = nguoiDung.ten + "/ "+ donVi.ten,
                                 }).ToList();
            ViewBag.nguoiDungList = new SelectList(nguoiDungList, "iD", "tenNguoiDung");
            return View(loaiTieuChi);
        }

        // POST: loaiTieuChi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,ten,tongDiem")] loaiTieuChi loaiTieuChi,
                                  int dm_DonVi)
        {
            //loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(loaiTieuChi.iD);
            var nhomChiTieu = db.nhomChiTieux.Where(n => n.fk_loaiTieuChi == loaiTieuChi.iD).ToList();
            var giaoChiTieuchoDV = db.giaoChiTieuchoDVs.ToList();
            foreach (var nhom in nhomChiTieu)
            {
                var chiTieu = db.chiTieux.Where(c => c.fk_loaiChiTieu == nhom.iD).ToList();
                //kiểm tra chỉ tiêu có nằm trong bảng giaoChiTieuchoDV, nếu có thì không tạo mới
                foreach (var chi in chiTieu)
                {
                    foreach (var giaoChiTieu in giaoChiTieuchoDV)
                    {
                        if (chi.iD == giaoChiTieu.fk_chiTieu)
                        {
                            //id bằng nghĩa là có trong bảng giaoChiTieu rồi
                            //update fk_dm đơn vị thôi
                            giaoChiTieu.fk_dmDonVi = dm_DonVi;
                            db.SaveChanges();
                            break;
                        }
                        else
                        {
                            // tạo nhiều giaoChiTieuchoDV với nhiều chỉ tiêu
                            
                            giaoChiTieuchoDV newGiao = new giaoChiTieuchoDV();
                            newGiao.fk_chiTieu = chi.iD;
                            newGiao.fk_dmDonVi = dm_DonVi;
                            db.giaoChiTieuchoDVs.Add(newGiao);
                            db.SaveChanges();

                            //tạo bảng điểm
                            bangDiem newBang = new bangDiem();
                            newBang.fk_giaoChiTieu = newGiao.id;
                            db.bangDiems.Add(newBang);
                            db.SaveChanges();
                            break;
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: loaiTieuChi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);
            if (loaiTieuChi == null)
            {
                return HttpNotFound();
            }
            return View(loaiTieuChi);
        }

        // POST: loaiTieuChi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);
            db.loaiTieuChis.Remove(loaiTieuChi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
