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
    public class bangDiemController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: bangDiem
        public ActionResult Index()
        {
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            var dmDonvi = Session["dm_DonVi"];

            var getThanhDoan = db.quanHeDonVis.Where(q => q.donViCha == (int)dmDonvi).FirstOrDefault();
            var getChiDoan = db.quanHeDonVis.Where(q => q.donViCha == getThanhDoan.donViCon).FirstOrDefault();
            if(getChiDoan == null)
            {
                getChiDoan = getThanhDoan;
            }

            var dataChiTieu = (from chTietChiTieu in db.chiTietChiTieux
                            join chiTieu in db.chiTieux
                                on chTietChiTieu.fk_loaiChiTieu equals chiTieu.iD
                            join giaoChiTieu in db.giaoChiTieuchoDVs
                                on chiTieu.iD equals giaoChiTieu.fk_chiTieu
                            join bangdiem in db.bangDiems
                                on giaoChiTieu.id equals bangdiem.fk_giaoChiTieu
                            join nhomChiTeu in db.nhomChiTieux
                                on chiTieu.fk_loaiChiTieu equals nhomChiTeu.iD
                            join loaiTieuChi in db.loaiTieuChis
                                on nhomChiTeu.fk_loaiTieuChi equals loaiTieuChi.iD
                            join dm_donVi in db.dm_donVi
                                on giaoChiTieu.fk_dmDonVi equals dm_donVi.iD
                            join nguoiDung in db.nguoiDungs 
                                on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                            join donVi in db.donVis
                                on nguoiDung.fk_donVi equals donVi.iD
                            select new dataBangDiem()
                            {
                                bangDiem = bangdiem,
                                giaoChiTieuchoDV = giaoChiTieu,
                                loaiTieuChi = loaiTieuChi,
                                nhomChiTieu = nhomChiTeu,
                                chiTieu = chiTieu,
                                chiTietChiTieu = chTietChiTieu,
                                dm_DonVi = dm_donVi,
                                nguoiDung = nguoiDung,
                                donVi = donVi,
                            }).Where(g => g.giaoChiTieuchoDV.fk_dmDonVi == getChiDoan.donViCon || g.giaoChiTieuchoDV.fk_dmDonVi == getChiDoan.donViCha || g.giaoChiTieuchoDV.fk_dmDonVi == (int)dmDonvi)
                            .OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                            .ThenBy(o => o.chiTieu.iD).DistinctBy(x=>x.chiTietChiTieu.iD);

            var dataDiem = (from chTietChiTieu in db.chiTietChiTieux
                            join chiTieu in db.chiTieux
                                on chTietChiTieu.fk_loaiChiTieu equals chiTieu.iD
                            join giaoChiTieu in db.giaoChiTieuchoDVs
                                on chiTieu.iD equals giaoChiTieu.fk_chiTieu
                            join bangdiem in db.bangDiems
                                on giaoChiTieu.id equals bangdiem.fk_giaoChiTieu
                            join nhomChiTeu in db.nhomChiTieux
                                on chiTieu.fk_loaiChiTieu equals nhomChiTeu.iD
                            join loaiTieuChi in db.loaiTieuChis
                                on nhomChiTeu.fk_loaiTieuChi equals loaiTieuChi.iD
                            join dm_donVi in db.dm_donVi
                                on giaoChiTieu.fk_dmDonVi equals dm_donVi.iD
                            join nguoiDung in db.nguoiDungs
                                on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                            join donVi in db.donVis
                                on nguoiDung.fk_donVi equals donVi.iD
                            select new dataBangDiem()
                            {
                                bangDiem = bangdiem,
                                giaoChiTieuchoDV = giaoChiTieu,
                                loaiTieuChi = loaiTieuChi,
                                nhomChiTieu = nhomChiTeu,
                                chiTieu = chiTieu,
                                chiTietChiTieu = chTietChiTieu,
                                dm_DonVi = dm_donVi,
                                nguoiDung = nguoiDung,
                                donVi = donVi,
                            }).Where(g => g.giaoChiTieuchoDV.fk_dmDonVi == getChiDoan.donViCon || g.giaoChiTieuchoDV.fk_dmDonVi ==getChiDoan.donViCha || g.giaoChiTieuchoDV.fk_dmDonVi == (int)dmDonvi)
                               .OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                            .ThenBy(o => o.chiTieu.iD).ThenBy(g => g.giaoChiTieuchoDV.fk_dmDonVi);
                            
            ViewBag.dataChiTieu = dataChiTieu.ToList();
            ViewBag.dataDiem = dataDiem.ToList();
            return View();
        }

        // GET: bangDiem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bangDiem bangDiem = db.bangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // GET: bangDiem/Create
        public ActionResult Create()
        {
            ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id");
            return View();
        }

        // POST: bangDiem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fk_giaoChiTieu,diem,ycDanhGiaKQ,ycMinhChung,thoiGian,banPhuTrach")] bangDiem bangDiem)
        {
            if (ModelState.IsValid)
            {
                db.bangDiems.Add(bangDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id", bangDiem.fk_giaoChiTieu);
            return View(bangDiem);
        }

        // GET: bangDiem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bangDiem bangDiem = db.bangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id", bangDiem.fk_giaoChiTieu);
            return View(bangDiem);
        }

        // POST: bangDiem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fk_giaoChiTieu,diem,ycDanhGiaKQ,ycMinhChung,thoiGian,banPhuTrach")] bangDiem bangDiem)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(bangDiem).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id", bangDiem.fk_giaoChiTieu);
            return View(bangDiem);
        }

        // GET: bangDiem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bangDiem bangDiem = db.bangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // POST: bangDiem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bangDiem bangDiem = db.bangDiems.Find(id);
            db.bangDiems.Remove(bangDiem);
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
