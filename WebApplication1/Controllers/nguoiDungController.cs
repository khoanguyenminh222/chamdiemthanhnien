using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class nguoiDungController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: nguoiDung
        public ActionResult Index()
        {
            if (Session["dm_DonVi"]== null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            int dm_DonVi = (int)Session["dm_DonVi"];
            var dm_DonViCon = db.quanHeDonVis.Where(q => q.donViCha == dm_DonVi).Select(q => q.donViCon).SingleOrDefault();
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
                                       join bangDiem in db.bangDiems
                                        on giaoChiTieuchoDV.id equals bangDiem.fk_giaoChiTieu
                                       select new dataBangDiem
                                       {
                                           giaoChiTieuchoDV = giaoChiTieuchoDV,
                                           bangDiem = bangDiem,
                                           loaiTieuChi = loaiTieuChi,
                                           chiTieu = chiTieu,
                                           dm_DonVi = dm_donVi,
                                           nguoiDung = nguoiDung,
                                           donVi = donVi,
                                       }).Where(g=>g.giaoChiTieuchoDV.fk_dmDonVi == dm_DonVi)
                                       .DistinctBy(l=>l.loaiTieuChi.iD)
                                       .OrderBy(l=>l.loaiTieuChi.iD);
            var searchDonViCon = (from dm_donVi in db.dm_donVi
                                  join nguoiDung in db.nguoiDungs
                                    on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                                  join donVi in db.donVis
                                    on nguoiDung.fk_donVi equals donVi.iD
                                  join quanHeDonVi in db.quanHeDonVis
                                    on dm_donVi.iD equals quanHeDonVi.donViCon
                                  select new dataBangDiem
                                  {
                                      dm_DonVi = dm_donVi,
                                      nguoiDung = nguoiDung,
                                      donVi = donVi,
                                      quanHeDonVi = quanHeDonVi,
                                  }).Where(q => q.quanHeDonVi.donViCha == dm_DonVi);
            ViewBag.searchDonViCon = searchDonViCon.ToList();


            List<int> countLoaiTieuChi1 = new List<int>();
            if (dm_DonViCon != null)
            {
                foreach (var l in db.loaiTieuChis)
                {
                    var count = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                 join chiTieu in db.chiTieux
                                     on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                 join nhomChiTieu in db.nhomChiTieux
                                     on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                 join loaiTieuChi in db.loaiTieuChis
                                     on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi.iD
                                 where (loaiTieuChi.iD == l.iD)
                                 where (giaoChiTieuchoDV.fk_dmDonVi == dm_DonViCon)
                                 select giaoChiTieuchoDV).Count();
                    countLoaiTieuChi1.Add(count);

                }
            }
            else
            {
                foreach (var l in db.loaiTieuChis)
                {
                    var count = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                 join chiTieu in db.chiTieux
                                     on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                                 join nhomChiTieu in db.nhomChiTieux
                                     on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                                 join loaiTieuChi in db.loaiTieuChis
                                     on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi.iD
                                 where (loaiTieuChi.iD == l.iD)
                                 where (giaoChiTieuchoDV.fk_dmDonVi == dm_DonVi)
                                 select giaoChiTieuchoDV).Count();
                    countLoaiTieuChi1.Add(count);
                }
            }    
            
            List<int> countLoaiTieuChi2 = new List<int>();
            foreach (var l in db.loaiTieuChis)
            {
                var count = (from giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                             join chiTieu in db.chiTieux
                                 on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                             join nhomChiTieu in db.nhomChiTieux
                                 on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                             join loaiTieuChi in db.loaiTieuChis
                                 on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi.iD
                             where (loaiTieuChi.iD == l.iD)
                             where (giaoChiTieuchoDV.fk_dmDonVi == dm_DonVi)
                             select giaoChiTieuchoDV).Count();
                countLoaiTieuChi2.Add(count);

            }
            ViewBag.countLoaiTieuChi1 = countLoaiTieuChi1;
            ViewBag.countLoaiTieuChi2 = countLoaiTieuChi2;
            ViewBag.tieuChi_giaoChiTieu = tieuChi_giaoChiTieu.ToList();
            return View();
        }

        // get loại tiêu chí của người dùng
        public ActionResult chamDiemLoaiTieuChi(int? id)
        {
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            Session["idLoaiTieuChi"] = id;
            var dataChiTieu = (from bangdiem in db.bangDiems
                            join giaoChiTieuchoDV in db.giaoChiTieuchoDVs
                                on bangdiem.fk_giaoChiTieu equals giaoChiTieuchoDV.id
                            join dm_donVi in db.dm_donVi
                                on giaoChiTieuchoDV.fk_dmDonVi equals dm_donVi.iD
                            join nguoiDung in db.nguoiDungs
                                on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                            join donVi in db.donVis
                                on nguoiDung.fk_donVi equals donVi.iD
                            join chiTieu in db.chiTieux
                                on giaoChiTieuchoDV.fk_chiTieu equals chiTieu.iD
                            join chiTietChiTieu in db.chiTietChiTieux
                                on chiTieu.iD equals chiTietChiTieu.fk_loaiChiTieu
                            join nhomChiTieu in db.nhomChiTieux
                                on chiTieu.fk_loaiChiTieu equals nhomChiTieu.iD
                            join loaiTieuChi in db.loaiTieuChis
                                on nhomChiTieu.fk_loaiTieuChi equals loaiTieuChi.iD
                            select new dataBangDiem()
                            {
                                bangDiem = bangdiem,
                                giaoChiTieuchoDV = giaoChiTieuchoDV,
                                loaiTieuChi = loaiTieuChi,
                                nhomChiTieu = nhomChiTieu,
                                chiTieu = chiTieu,
                                chiTietChiTieu = chiTietChiTieu,
                                dm_DonVi = dm_donVi,
                                nguoiDung = nguoiDung,
                                donVi = donVi,
                            }).Where(l=>l.loaiTieuChi.iD == id).OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                            .ThenBy(o => o.chiTieu.iD);


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
                            }).Where(l => l.loaiTieuChi.iD == id).OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                            .ThenBy(o => o.chiTieu.iD).ThenBy(b=>b.bangDiem.fk_giaoChiTieu);
            ViewBag.dataChiTieu = dataChiTieu.ToList();
            ViewBag.dataDiem = dataDiem.ToList();
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string ten, string matkhau)
        {
            if (ModelState.IsValid)
            {

                var data = (from taiKhoan in db.taiKhoans
                            join dmDV in db.dm_donVi
                                 on taiKhoan.fk_dmDonVi equals dmDV.iD
                            join nguoiDung in db.nguoiDungs
                                 on dmDV.fk_nguoiQuanLy equals nguoiDung.iD
                            join donVi in db.donVis
                                 on nguoiDung.fk_donVi equals donVi.iD
                            where taiKhoan.ten == ten
                            where taiKhoan.matKhau == matkhau
                            select new dataNguoiDung()
                            {
                                taiKhoan = taiKhoan,
                                dm_DonVi = dmDV,
                                donVi = donVi,
                                nguoiDung = nguoiDung,
                            });
                if (data.Count() > 0)
                {
                    Session["dm_DonVi"] = data.FirstOrDefault().taiKhoan.fk_dmDonVi;
                    Session["name"] = data.FirstOrDefault().taiKhoan.ten;
                    Session["donvi"] = data.FirstOrDefault().nguoiDung.fk_donVi;
                    if ((int)Session["donvi"] == 4)
                    {
                        return RedirectToAction("Index", "bangDiem");
                    }
                    return RedirectToAction("Index", "nguoiDung");
                }
                else if (data.Count() <= 0)
                {
                    ViewBag.fail = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chamDiem([Bind(Include = "id,fk_giaoChiTieu,diem,ycDanhGiaKQ,ycMinhChung,thoiGian,banPhuTrach")] bangDiem bangDiem,
                                      int iD_chiTieu, string submit)
        {
            var id = Session["idLoaiTieuChi"];
            switch (submit)
            {
                case "submit":
                    //update bảng điểm
                    bangDiem bdExisted = db.bangDiems.Find(bangDiem.id);
                    bdExisted.diem = bangDiem.diem;
                    bdExisted.ycDanhGiaKQ = bangDiem.ycDanhGiaKQ;
                    bdExisted.ycMinhChung = bangDiem.ycMinhChung;
                    DateTime date = DateTime.Today;
                    bdExisted.thoiGian = date;
                    db.SaveChanges();

                    //kiếm đơn vị cha
                    var donVi = Session["dm_DonVi"];
                    var quanHeDonVi = db.quanHeDonVis.Where(q => q.donViCon == (int)donVi).FirstOrDefault();
                    if (quanHeDonVi == null)
                    {
                        // nếu như không có đơn vị cha thì cập nhật điẻm
                        bangDiem bd = db.bangDiems.Find(bangDiem.id);
                        bd.diem = bangDiem.diem;
                        bd.ycDanhGiaKQ = bangDiem.ycDanhGiaKQ;
                        bd.ycMinhChung = bangDiem.ycMinhChung;
                        bd.thoiGian = date;
                        db.SaveChanges();
                    }
                    else
                    {
                        //tạo 1 giao chỉ tiêu cho đơn vị cha
                        giaoChiTieuchoDV giaoChiTieuchoDV = new giaoChiTieuchoDV();
                        giaoChiTieuchoDV.fk_chiTieu = iD_chiTieu;
                        giaoChiTieuchoDV.fk_dmDonVi = quanHeDonVi.donViCha;
                        db.giaoChiTieuchoDVs.Add(giaoChiTieuchoDV);
                        db.SaveChanges();

                        //tạo bảng điểm
                        bangDiem bangDiem1 = new bangDiem();
                        bangDiem1.fk_giaoChiTieu = giaoChiTieuchoDV.id;
                        bangDiem1.ycDanhGiaKQ = bangDiem.ycDanhGiaKQ;
                        bangDiem1.ycMinhChung = bangDiem.ycMinhChung;
                        bangDiem1.thoiGian = date;
                        db.bangDiems.Add(bangDiem1);
                        db.SaveChanges();
                    }
                    return RedirectToAction("chamDiemLoaiTieuChi", new { id = id });
                case "edit":
                    //update bảng điểm của chi đoàn
                    bangDiem bdExisted1 = db.bangDiems.Find(bangDiem.id);
                    bdExisted1.diem = bangDiem.diem;
                    bdExisted1.ycDanhGiaKQ = bangDiem.ycDanhGiaKQ;
                    bdExisted1.ycMinhChung = bangDiem.ycMinhChung;
                    DateTime date1 = DateTime.Today;
                    bdExisted1.thoiGian = date1;
                    db.SaveChanges();

                    //update bảng điểm của các đon vị cha
                    var giaoChiTieuList = db.giaoChiTieuchoDVs.Where(g => g.fk_chiTieu == iD_chiTieu).ToList();
                    foreach(var bd in db.bangDiems)
                    {
                        foreach(var giao in giaoChiTieuList)
                        {
                            if (bd.fk_giaoChiTieu == giao.id)
                            {
                                bd.ycDanhGiaKQ = bangDiem.ycDanhGiaKQ;
                                bd.ycMinhChung = bangDiem.ycMinhChung;
                                bd.thoiGian = date1;
                                db.Entry(bd).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
                            }
                        }
                    }
                    db.SaveChanges();

                    return RedirectToAction("chamDiemLoaiTieuChi", new {id = id });
            }
            return RedirectToAction("Index");
        }
    }
}