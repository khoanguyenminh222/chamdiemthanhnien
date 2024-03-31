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
            int dmDonvi = (int)Session["dm_DonVi"];
            var yearNow = DateTime.Now.Year;
            List<int> listYear = new List<int>();
            for (int ls = yearNow; ls >= 2010; ls--)
            {
                listYear.Add(ls);
            }
            ViewBag.listYear = new SelectList(listYear);
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var loaiTieuChi = db.loaiTieuChis.Where(l => l.nam == year).ToList();
            if ((bool)Session["cumTruong"] == true) { 
                if((int)Session["donvi"] == 2)
                {
                    var lsdm_donvi = (from dm_donVi in db.dm_donVi
                                      join giaoChiTieuchoDV in db.giaoChiTieuchoDVs on dm_donVi.iD equals giaoChiTieuchoDV.fk_dmDonViChiDoan
                                      join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                      join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                      join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                                      select new dataBangDiem
                                      {
                                          dm_DonVi = dm_donVi,
                                          loaiTieuChi = loaiTieuChi1,
                                          giaoChiTieuchoDV = giaoChiTieuchoDV,
                                      }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViThanhDoan == dmDonvi)
                              .OrderBy(d => d.giaoChiTieuchoDV.fk_dmDonViChiDoan);
                    ViewBag.lsdm_donvi = lsdm_donvi.ToList();
                }
                else if((int)Session["donvi"] == 3)
                {
                    var lsdm_donvi = (from dm_donVi in db.dm_donVi
                                      join giaoChiTieuchoDV in db.giaoChiTieuchoDVs on dm_donVi.iD equals giaoChiTieuchoDV.fk_dmDonViChiDoan
                                      join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                      join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                      join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                                      select new dataBangDiem
                                      {
                                          dm_DonVi = dm_donVi,
                                          loaiTieuChi = loaiTieuChi1,
                                          giaoChiTieuchoDV = giaoChiTieuchoDV,
                                      }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViTinhDoan == dmDonvi)
                              .OrderBy(d => d.giaoChiTieuchoDV.fk_dmDonViChiDoan);
                    ViewBag.lsdm_donvi = lsdm_donvi.ToList();
                }
            }
            else
            {
                var lsdm_donvi = (from dm_donVi in db.dm_donVi
                                  join giaoChiTieuchoDV in db.giaoChiTieuchoDVs on dm_donVi.iD equals giaoChiTieuchoDV.fk_dmDonViChiDoan
                                  join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                  join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                  join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                                  select new dataBangDiem
                                  {
                                      dm_DonVi = dm_donVi,
                                      loaiTieuChi = loaiTieuChi1,
                                      giaoChiTieuchoDV = giaoChiTieuchoDV,
                                  }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViTinhDoan == dmDonvi)
                              .OrderBy(d => d.giaoChiTieuchoDV.fk_dmDonViChiDoan);
                ViewBag.lsdm_donvi = lsdm_donvi.ToList();
            }
            return View(loaiTieuChi);
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

        // GET: loaiTieuChi/AddDonVi/5
        public ActionResult AddDonVi(int? id)
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
            var listQuanHeDonVi = db.quanHeDonVis.Where(x => x.tinhDoan == (int)dmDonvi || x.thanhDoan==(int)dmDonvi).ToList();
            List<dm_donVi> dm = new List<dm_donVi>();
            foreach (var i in listQuanHeDonVi)
            {
                dm.Add(db.dm_donVi.Where(x => x.iD == i.chiDoan).FirstOrDefault());
            }
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);
            var giaoChiTieu = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                               join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                               join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                               join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                               select new dataBangDiem
                               {
                                   giaoChiTieuchoDV = giaoChiTieuchoDV,
                                   loaiTieuChi = loaiTieuChi1,
                               }).Where(l => l.loaiTieuChi.iD == id).DistinctBy(g => g.giaoChiTieuchoDV.fk_dmDonViChiDoan)
                               .ToList();

            ViewBag.giaoChiTieu = giaoChiTieu;
            tieuChi_nguoiDung tieuChi_nguoiDung = new tieuChi_nguoiDung()
            {
                dm_DonVi = dm,
                loaiTieuChi = loaiTieuChi,
            };
            return View(tieuChi_nguoiDung);
        }

        // POST: loaiTieuChi/AddDonVi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDonVi(tieuChi_nguoiDung tieuChi_NguoiDung)
        {
            Console.WriteLine(tieuChi_NguoiDung);
            // lấy ra chi đoàn
            var lsChiDoan = tieuChi_NguoiDung.dm_DonVi.Where(x => x.Ischecked == true).ToList();

            var nhomChiTieu = db.nhomChiTieux.Where(n => n.fk_loaiTieuChi == tieuChi_NguoiDung.loaiTieuChi.iD).ToList();
            var giaoChiTieuchoDV = db.giaoChiTieuchoDVs.ToList();
            foreach (var nhom in nhomChiTieu)
            {
                var chiTieu = db.chiTieux.Where(c => c.fk_loaiChiTieu == nhom.iD).ToList();

                foreach (var chi in chiTieu)
                {
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
                            var chitiet = db.chiTietChiTieux.Where(x => x.fk_loaiChiTieu == chi.iD).ToList();
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
                }
            }
            return RedirectToAction("Index");
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
            var listQuanHeDonVi = db.quanHeDonVis.Where(x => x.tinhDoan == (int)dmDonvi).ToList();
            List<dm_donVi> dm = new List<dm_donVi>();
            foreach (var i in listQuanHeDonVi)
            {
                dm.Add(db.dm_donVi.Where(x => x.iD == i.chiDoan).FirstOrDefault());
            }
            loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(id);
            if ((bool)Session["cumTruong"] == true)
            {
                if ((int)Session["donvi"] == 2)
                {
                    var giaoChiTieu = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                       join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                       join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                       join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                                       select new dataBangDiem
                                       {
                                           giaoChiTieuchoDV = giaoChiTieuchoDV,
                                           loaiTieuChi = loaiTieuChi1,
                                       }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViThanhDoan == (int)dmDonvi)
                               .Where(l => l.loaiTieuChi.iD == id).DistinctBy(g => g.giaoChiTieuchoDV.fk_dmDonViChiDoan)
                               .ToList();

                    ViewBag.giaoChiTieu = giaoChiTieu;
                }
                if ((int)Session["donvi"] == 3)
                {
                    var giaoChiTieu = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                       join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                       join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                       join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                                       select new dataBangDiem
                                       {
                                           giaoChiTieuchoDV = giaoChiTieuchoDV,
                                           loaiTieuChi = loaiTieuChi1,
                                       }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViTinhDoan == (int)dmDonvi)
                               .Where(l => l.loaiTieuChi.iD == id).DistinctBy(g => g.giaoChiTieuchoDV.fk_dmDonViChiDoan)
                               .ToList();

                    ViewBag.giaoChiTieu = giaoChiTieu;
                }
            }
            else
            {
                var giaoChiTieu = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                   join chiTieu in db.chiTieux on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                   join nhomChiTieu in db.nhomChiTieux on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                   join loaiTieuChi1 in db.loaiTieuChis on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi1.iD
                                   select new dataBangDiem
                                   {
                                       giaoChiTieuchoDV = giaoChiTieuchoDV,
                                       loaiTieuChi = loaiTieuChi1,
                                   }).Where(g => g.giaoChiTieuchoDV.fk_dmDonViTinhDoan == (int)dmDonvi)
                               .Where(l => l.loaiTieuChi.iD == id).DistinctBy(g => g.giaoChiTieuchoDV.fk_dmDonViChiDoan)
                               .ToList();

                ViewBag.giaoChiTieu = giaoChiTieu;
            }
                    
            tieuChi_nguoiDung tieuChi_nguoiDung = new tieuChi_nguoiDung()
            {
                dm_DonVi = dm,
                loaiTieuChi = loaiTieuChi,
            };
            return View(tieuChi_nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tieuChi_nguoiDung tieuChi_NguoiDung)
        {
            Console.WriteLine(tieuChi_NguoiDung);
            ///////////////////////////////////////////////////
            //lấy ra chi đoàn
            //TH1 already==true, checked true thì mới lưu
            var lsChiDoan1 = tieuChi_NguoiDung.dm_DonVi.ToList();

            ////loaiTieuChi loaiTieuChi = db.loaiTieuChis.Find(loaiTieuChi.iD);
            var nhomChiTieu = db.nhomChiTieux.Where(n => n.fk_loaiTieuChi == tieuChi_NguoiDung.loaiTieuChi.iD).ToList();
            var giaoChiTieuchoDV = db.giaoChiTieuchoDVs.ToList();
            foreach (var nhom in nhomChiTieu)
            {
                var chiTieu = db.chiTieux.Where(c => c.fk_loaiChiTieu == nhom.iD).ToList();
                foreach (var giao in giaoChiTieuchoDV)
                {
                    //kiểm tra chỉ tiêu và chi đoàn có nằm trong bảng giaoChiTieuchoDV, nếu có thì không tạo mới
                    foreach (var chi in chiTieu)
                    {
                        if (chi.iD == giao.fk_chiTieu)
                        {
                            foreach (var l in lsChiDoan1)
                            {
                                if (l.Already == true)
                                {
                                    var giaochitieu = db.giaoChiTieuchoDVs.Where(x=>x.fk_dmDonViChiDoan==l.iD).ToList();
                                        if(giaochitieu.Count>0)
                                        //if (giao.fk_dmDonViChiDoan != l.iD)
                                        {

                                    }
                                    else
                                    {
                                        //không có trong bảng giao chỉ tiêu=> tạo mới
                                        Console.WriteLine(l.iD);
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
                                            var chitiet = db.chiTietChiTieux.Where(x => x.fk_loaiChiTieu == chi.iD).ToList();
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
                                }
                                else
                                {
                                    if (l.Ischecked == true)
                                    {
                                        if (giao.fk_dmDonViChiDoan == l.iD)
                                        {
                                            // có trong bảng giao chỉ tiêu không tạo mới
                                        }
                                        else
                                        {
                                            //không có trong bảng giao chỉ tiêu=> tạo mới
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
                                                var chitiet = db.chiTietChiTieux.Where(x => x.fk_loaiChiTieu == chi.iD).ToList();
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
                                    }
                                    Console.WriteLine(l.iD);
                                    if (giao.fk_dmDonViChiDoan == l.iD)
                                    {
                                        var removegiaochitieu1 = db.giaoChiTieuchoDVs.Where(g => g.fk_chiTieu == chi.iD).ToList();
                                        Console.WriteLine(removegiaochitieu1);
                                        foreach(var r in removegiaochitieu1)
                                        {
                                            if (r.fk_dmDonViChiDoan == l.iD)
                                            {
                                                var removeBangdiem1 = db.bangDiems.Where(x => x.fk_giaoChiTieu == r.id).FirstOrDefault();
                                                db.bangDiems.Remove(removeBangdiem1);
                                                db.SaveChanges();
                                                db.giaoChiTieuchoDVs.Remove(r);
                                                db.SaveChanges();
                                            }
                                            
                                        }
                                    }

                                }
                            }
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
