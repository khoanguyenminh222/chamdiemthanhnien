using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class loaiTieuChiController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: loaiTieuChi
        public ActionResult Index(int? year)
        {
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            var yearNow = DateTime.Now.Year;
            List<int> listYear = new List<int>();
            for (int ls = yearNow; ls>=2010; ls--)
            {
                listYear.Add(ls);
            }
            ViewBag.listYear = new SelectList(listYear);
            if(year == null)
            {
                year = DateTime.Now.Year;
            }
            var tieuChi_loaiTieuChi = (from loaiTieuChi in db.loaiTieuChis
                                       
                                       select new dataBangDiem
                                       {
                                           loaiTieuChi = loaiTieuChi,
                                       }).Where(l=>l.loaiTieuChi.nam == year)
                                       .OrderBy(l => l.loaiTieuChi.iD).DistinctBy(l => l.loaiTieuChi.iD);
            var dmDonvi = Session["dm_DonVi"];
            
            
            
            //var tieuChi_giaoChiTieu = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
            //                           join chiTieu in db.chiTieux
            //                            on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
            //                           join nhomChiTieu in db.nhomChiTieux
            //                            on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
            //                           join loaiTieuChi in db.loaiTieuChis
            //                            on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi.iD
            //                           join dm_donVi in db.dm_donVi
            //                            on giaoChiTieuchoDV.fk_dmDonViChiDoan equals dm_donVi.iD
            //                           join nguoiDung in db.nguoiDungs
            //                            on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
            //                           join donVi in db.donVis
            //                            on nguoiDung.fk_donVi equals donVi.iD
            //                           select new dataBangDiem
            //                           {
            //                               giaoChiTieuchoDV = giaoChiTieuchoDV,
            //                               loaiTieuChi = loaiTieuChi,
            //                               chiTieu = chiTieu,
            //                               dm_DonVi = dm_donVi,
            //                               nguoiDung = nguoiDung,
            //                               donVi = donVi,
            //                           }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViTinhDoan == (int)dmDonvi)
            //                           .DistinctBy(c => c.giaoChiTieuchoDV.fk_chiTieu);
            
            
            ViewBag.tieuChi_loaiTieuChi = tieuChi_loaiTieuChi.ToList();
            //ViewBag.tieuChi_giaoChiTieu = tieuChi_giaoChiTieu.ToList();
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
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dmDonvi = Session["dm_DonVi"];


            //var data = (from dm_donVi in db.dm_donVi
            //            from loaiTieuChi in db.loaiTieuChis
            //            from nguoiDung in db.nguoiDungs
            //            join donVi in db.donVis
            //               on nguoiDung.fk_donVi equals donVi.iD
            //            join quanHeDonVi in db.quanHeDonVis
            //               on dm_donVi.iD equals quanHeDonVi.chiDoan
            //            where(loaiTieuChi.iD == id)
            //            where (quanHeDonVi.tinhDoan == (int)dmDonvi)
            //            select new tieuChi_nguoiDung()
            //            {
            //                dm_DonVi = dm_donVi
            //                nguoiDung = nguoiDung,
            //                donVi = donVi,
            //                loaiTieuChi = loaiTieuChi,
            //            });
            var listQuanHeDonVi = db.quanHeDonVis.Where(x => x.tinhDoan == (int)dmDonvi).ToList();
            List<dm_donVi> dm = new List<dm_donVi>();
            foreach (var i in listQuanHeDonVi)
            {
                dm.Add(db.dm_donVi.Where(x=>x.iD==i.chiDoan).FirstOrDefault());
            }
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);

            

            tieuChi_nguoiDung tieuChi_nguoiDung = new tieuChi_nguoiDung()
            {
                dm_DonVi = dm,
                loaiTieuChi = loaiTieuChi,
            };
            return View(tieuChi_nguoiDung);
        }

        // POST: loaiTieuChi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tieuChi_nguoiDung tieuChi_NguoiDung)
        {
            Console.WriteLine(tieuChi_NguoiDung);
            // lấy ra chi đoàn
            var lsChiDoan = tieuChi_NguoiDung.dm_DonVi.Where(x=>x.Checked==true).ToList();
            ////loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(loaiTieuChi.iD);
            var nhomChiTieu = db.nhomChiTieux.Where(n => n.fk_loaiTieuChi == tieuChi_NguoiDung.loaiTieuChi.iD).ToList();
            var giaoChiTieuchoDV = db.giaoChiTieuchoDVs.ToList();
            foreach (var nhom in nhomChiTieu)
            {
                var chiTieu = db.chiTieux.Where(c => c.fk_loaiChiTieu == nhom.iD).ToList();
                //kiểm tra chỉ tiêu có nằm trong bảng giaoChiTieuchoDV, nếu có thì không tạo mới
                foreach (var chi in chiTieu)
                {
                    //if (giaoChiTieuchoDV.Count == 0)
                    //{
                    // tạo nhiều giaoChiTieuchoDV với nhiều chỉ tiêu

                    foreach (var l in lsChiDoan)
                    {
                        var quanhedv = db.quanHeDonVis.Where(q => q.chiDoan == l.iD).ToList();
                        foreach (var h in quanhedv)
                        {
                            giaoChiTieuchoDV newGiao = new giaoChiTieuchoDV();
                            newGiao.fk_chiTieu = chi.iD;
                            newGiao.fk_dmDonViChiDoan = l.iD;
                            newGiao.fk_dmDonViThanhDoan = h.thanhDoan;
                            newGiao.fk_dmDonViTinhDoan = h.tinhDoan;
                            db.giaoChiTieuchoDVs.Add(newGiao);
                            db.SaveChanges();
                            var chitiet = db.chiTietChiTieux.Where(x=>x.fk_loaiChiTieu == chi.iD).ToList();
                            var maxDiem = 0;
                            foreach (var ct in chitiet)
                            {
                                if (maxDiem < ct.diem)
                                {
                                    maxDiem = ct.diem;
                                }
                            }
                            //tạo bảng điểm
                            bangDiem newBang = new bangDiem();
                            newBang.fk_giaoChiTieu = newGiao.id;
                            newBang.diemCoDinh = maxDiem;
                            newBang.diemChiDoan = 0;
                            newBang.diemThanhDoan = 0;
                            newBang.diemTinhDoan = 0;
                            newBang.trangThai = 0;
                            db.bangDiems.Add(newBang);
                            db.SaveChanges();
                        }
                    }


                    //        //else
                    //        //{
                    //        //    foreach (var giaoChiTieu in giaoChiTieuchoDV)
                    //        //    {
                    //        //        if (chi.iD == giaoChiTieu.fk_chiTieu)
                    //        //        {
                    //        //            //id bằng nghĩa là có trong bảng giaoChiTieu rồi
                    //        //            //update fk_dm đơn vị thôi
                    //        //            giaoChiTieu.fk_dmDonVi = dm_DonVi;
                    //        //            db.SaveChanges();
                    //        //            break;
                    //        //        }
                    //        //        else
                    //        //        {
                    //        //            // tạo nhiều giaoChiTieuchoDV với nhiều chỉ tiêu

                    //        //            giaoChiTieuchoDV newGiao = new giaoChiTieuchoDV();
                    //        //            newGiao.fk_chiTieu = chi.iD;
                    //        //            newGiao.fk_dmDonVi = dm_DonVi;
                    //        //            db.giaoChiTieuchoDVs.Add(newGiao);
                    //        //            db.SaveChanges();

                    //        //            //tạo bảng điểm
                    //        //            bangDiem newBang = new bangDiem();
                    //        //            newBang.fk_giaoChiTieu = newGiao.id;
                    //        //            db.bangDiems.Add(newBang);
                    //        //            db.SaveChanges();
                    //        //            break;
                    //        //        }
                    //        //    }
                    //        //}

                    //    }
                    //}
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
