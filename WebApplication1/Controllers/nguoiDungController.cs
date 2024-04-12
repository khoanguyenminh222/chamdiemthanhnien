using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebApplication1.Models;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace WebApplication1.Controllers
{
    public class nguoiDungController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: nguoiDung
        public ActionResult Index(int? nguoidung, int? year)
        {

            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }

            var dmDonvi = Session["dm_DonVi"];

            Console.WriteLine(nguoidung);
            if (nguoidung == null)
            {
                var donvi = db.quanHeDonVis.Where(x => x.tinhDoan == (int)dmDonvi || x.thanhDoan == (int)dmDonvi).FirstOrDefault();
                nguoidung = donvi?.chiDoan;
            }
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
            TempData["year"] = year;
            Console.WriteLine(TempData["year"]);
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
                                on giaoChiTieu.fk_dmDonViChiDoan equals dm_donVi.iD
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
                            })
                            .Where(l => l.loaiTieuChi.nam == year)
                            .Where(g => g.giaoChiTieuchoDV.fk_dmDonViChiDoan == nguoidung || g.giaoChiTieuchoDV.fk_dmDonViChiDoan == (int)dmDonvi)

                            .OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                            .ThenBy(o => o.chiTieu.iD)
                            .ThenBy(b => b.bangDiem.fk_giaoChiTieu)
                            .ToList();

            var donviCon = (from dm_donVi in db.dm_donVi
                            join nguoiDung in db.nguoiDungs on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                            join quanHeDonVi in db.quanHeDonVis on dm_donVi.iD equals quanHeDonVi.chiDoan
                            where (quanHeDonVi.tinhDoan == (int)dmDonvi || quanHeDonVi.thanhDoan == (int)dmDonvi)
                            select new
                            {
                                id = dm_donVi.iD,
                                ten = nguoiDung.ten + "/" + nguoiDung.donVi.ten,
                            }).ToList();
            string myMessage = TempData["message"] as string;
            listdataBangDiem listdataBangDiem = new listdataBangDiem()
            {
                MyMessage = myMessage,
                dataBangDiems = dataDiem
            };
            var listnguoiDung = new SelectList(donviCon, "id", "ten");

            ViewBag.listnguoiDung = listnguoiDung;

            return View(listdataBangDiem);
        }

        //post index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chamDiem(listdataBangDiem data, string submit)
        {
            switch (submit)
            {
                case "update":
                    if ((int)Session["donvi"] == 1)
                    {
                        foreach (var item in data.dataBangDiems)
                        {

                            var updateBangDiem = db.bangDiems.Find(item.bangDiem.id);
                            if (updateBangDiem.trangThai != 3)
                            {
                                updateBangDiem.diemChiDoan = item.bangDiem.diemChiDoan;
                                updateBangDiem.ycMinhChung = item.bangDiem.ycMinhChung;
                                updateBangDiem.banPhuTrach = item.bangDiem.banPhuTrach;
                                if (item.bangDiem.HinhAnhFile != null && item.bangDiem.HinhAnhFile.ContentLength > 0)
                                {
                                    // Đọc dữ liệu file thành byte array
                                    using (var binaryReader = new BinaryReader(item.bangDiem.HinhAnhFile.InputStream))
                                    {
                                        updateBangDiem.hinhAnh = binaryReader.ReadBytes(item.bangDiem.HinhAnhFile.ContentLength);
                                    }
                                }
                                db.SaveChanges();
                                TempData["message"] = "update";
                            }
                            if (TempData["message"] == null)
                            {
                                TempData["message"] = "update1";
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    if ((int)Session["donvi"] == 2)
                    {
                        foreach (var item in data.dataBangDiems)
                        {
                            var updateBangDiem = db.bangDiems.Find(item.bangDiem.id);
                            if (updateBangDiem.trangThai != 3)
                            {
                                updateBangDiem.diemThanhDoan = item.bangDiem.diemThanhDoan;
                                db.SaveChanges();
                                TempData["message"] = "update";
                            }
                        }
                        if (TempData["message"] == null)
                        {
                            TempData["message"] = "update1";
                        }
                        return RedirectToAction("Index");
                    }
                    if ((int)Session["donvi"] == 3)
                    {
                        foreach (var item in data.dataBangDiems)
                        {
                            var updateBangDiem = db.bangDiems.Find(item.bangDiem.id);
                            if (updateBangDiem.trangThai != 3)
                            {
                                updateBangDiem.diemTinhDoan = item.bangDiem.diemTinhDoan;
                                updateBangDiem.yKienPhanHoi = item.bangDiem.yKienPhanHoi;
                                db.SaveChanges();
                                TempData["message"] = "update";
                            }
                        }
                        if (TempData["message"] == null)
                        {
                            TempData["message"] = "update1";
                        }
                        return RedirectToAction("Index");
                    }
                    TempData["message"] = "update1";
                    return RedirectToAction("Index");
                case "send":
                    if ((int)Session["donvi"] == 1)
                    {
                        foreach (var item in data.dataBangDiems)
                        {
                            var updateBangDiem = db.bangDiems.Find(item.bangDiem.id);
                            if (updateBangDiem.trangThai == 0 || updateBangDiem.trangThai == 1)
                            {
                                updateBangDiem.diemChiDoan = item.bangDiem.diemChiDoan;
                                updateBangDiem.ycMinhChung = item.bangDiem.ycMinhChung;
                                DateTime date = DateTime.Today;
                                updateBangDiem.thoiGian = date.Date;
                                updateBangDiem.banPhuTrach = item.bangDiem.banPhuTrach;
                                if (item.bangDiem.HinhAnhFile != null && item.bangDiem.HinhAnhFile.ContentLength > 0)
                                {
                                    // Đọc dữ liệu file thành byte array
                                    using (var binaryReader = new BinaryReader(item.bangDiem.HinhAnhFile.InputStream))
                                    {
                                        updateBangDiem.hinhAnh = binaryReader.ReadBytes(item.bangDiem.HinhAnhFile.ContentLength);
                                    }
                                }
                                updateBangDiem.trangThai = 1;
                                db.SaveChanges();
                                TempData["message"] = "send";
                            }
                        }
                        if (TempData["message"] == null)
                        {
                            TempData["message"] = "send1";
                        }
                        return RedirectToAction("Index");
                    }
                    else if ((int)Session["donvi"] == 2)
                    {
                        foreach (var item in data.dataBangDiems)
                        {
                            var updateBangDiem = db.bangDiems.Find(item.bangDiem.id);
                            if (updateBangDiem.trangThai == 1 || updateBangDiem.trangThai == 2)
                            {
                                updateBangDiem.diemThanhDoan = item.bangDiem.diemThanhDoan;
                                DateTime date = DateTime.Today;
                                updateBangDiem.thoiGian = date.Date;
                                
                                updateBangDiem.trangThai = 2;
                                db.SaveChanges();
                                TempData["message"] = "send";
                            }
                        }
                        if (TempData["message"] == null)
                        {
                            TempData["message"] = "send1";
                        }
                        return RedirectToAction("Index");
                    }
                    else if ((int)Session["donvi"] == 3)
                    {
                        foreach (var item in data.dataBangDiems)
                        {
                            var updateBangDiem = db.bangDiems.Find(item.bangDiem.id);
                            if (updateBangDiem.trangThai == 2 || updateBangDiem.trangThai == 3)
                            {
                                updateBangDiem.diemTinhDoan = item.bangDiem.diemTinhDoan;
                                DateTime date = DateTime.Today;
                                updateBangDiem.thoiGian = date.Date;
                                updateBangDiem.yKienPhanHoi = item.bangDiem.yKienPhanHoi;
                                updateBangDiem.trangThai = 3;
                                db.SaveChanges();

                                TempData["message"] = "send";
                            }
                        }
                        if (TempData["message"] == null)
                        {
                            TempData["message"] = "send1";
                        }
                        return RedirectToAction("Index");
                    }
                    TempData["message"] = "send1";
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
                    Session["ngDungTen"] = data.FirstOrDefault().nguoiDung.ten;
                    Session["name"] = data.FirstOrDefault().taiKhoan.ten;
                    Session["donvi"] = data.FirstOrDefault().nguoiDung.fk_donVi;
                    Session["cumTruong"] = data.FirstOrDefault().dm_DonVi.cumTruong;
                    if ((int)Session["donvi"] == 3 || (bool)Session["cumTruong"] == true)
                    {
                        return RedirectToAction("Index", "loaiTieuChi");
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

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult chamDiem([Bind(Include = "id,fk_giaoChiTieu,diem,ycMinhChung,thoiGian,banPhuTrach")] bangDiem bangDiem,
        //                              int iD_chiTieu, string submit, HttpPostedFileBase hinhAnh)
        //{
        //var id = Session["idLoaiTieuChi"];
        //switch (submit)
        //{
        //    case "submit":
        //        //update bảng điểm
        //        bangDiem bdExisted = db.bangDiems.Find(bangDiem.id);
        //        bdExisted.diem = bangDiem.diem;
        //        bdExisted.ycMinhChung = bangDiem.ycMinhChung;
        //        if (hinhAnh != null)
        //        {
        //            bangDiem.hinhAnh = new byte[hinhAnh.ContentLength];
        //            hinhAnh.InputStream.Read(bangDiem.hinhAnh, 0, hinhAnh.ContentLength);
        //            Console.WriteLine(bangDiem.hinhAnh);
        //            bdExisted.hinhAnh = bangDiem.hinhAnh;
        //        }
        //        DateTime date = DateTime.Today;
        //        bdExisted.thoiGian = date;
        //        bdExisted.banPhuTrach = bangDiem.banPhuTrach;
        //        db.SaveChanges();

        //        //kiếm đơn vị cha
        //        var donVi = Session["dm_DonVi"];
        //        var quanHeDonVi = db.quanHeDonVis.Where(q => q.donViCon == (int)donVi).FirstOrDefault();
        //        if (quanHeDonVi == null)
        //        {
        //            // nếu như không có đơn vị cha thì cập nhật điẻm
        //            bangDiem bd = db.bangDiems.Find(bangDiem.id);
        //            bd.diem = bangDiem.diem;
        //            bd.ycMinhChung = bangDiem.ycMinhChung;
        //            if (hinhAnh != null)
        //            {

        //                Console.WriteLine(bangDiem.hinhAnh);
        //                bd.hinhAnh = bangDiem.hinhAnh;
        //            }
        //            bd.thoiGian = date;
        //            bd.banPhuTrach = bangDiem.banPhuTrach;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            //tạo 1 giao chỉ tiêu cho đơn vị cha
        //            giaoChiTieuchoDV giaoChiTieuchoDV = new giaoChiTieuchoDV();
        //            giaoChiTieuchoDV.fk_chiTieu = iD_chiTieu;
        //            giaoChiTieuchoDV.fk_dmDonVi = quanHeDonVi.donViCha;
        //            db.giaoChiTieuchoDVs.Add(giaoChiTieuchoDV);
        //            db.SaveChanges();

        //            //tạo bảng điểm
        //            bangDiem bangDiem1 = new bangDiem();
        //            bangDiem1.fk_giaoChiTieu = giaoChiTieuchoDV.id;
        //            bangDiem1.ycMinhChung = bangDiem.ycMinhChung;
        //            if (hinhAnh != null)
        //            {

        //                Console.WriteLine(bangDiem.hinhAnh);
        //                bangDiem1.hinhAnh = bangDiem.hinhAnh;
        //            }
        //            bangDiem1.thoiGian = date;
        //            bangDiem1.banPhuTrach = bangDiem.banPhuTrach;
        //            db.bangDiems.Add(bangDiem1);
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("chamDiemLoaiTieuChi", new { id = id });
        //    case "edit":
        //        //update bảng điểm của chi đoàn
        //        bangDiem bdExisted1 = db.bangDiems.Find(bangDiem.id);
        //        bdExisted1.diem = bangDiem.diem;
        //        bdExisted1.ycMinhChung = bangDiem.ycMinhChung;
        //        if (hinhAnh != null)
        //        {
        //            bangDiem.hinhAnh = new byte[hinhAnh.ContentLength];
        //            hinhAnh.InputStream.Read(bangDiem.hinhAnh, 0, hinhAnh.ContentLength);
        //            Console.WriteLine(bangDiem.hinhAnh);
        //            bdExisted1.hinhAnh = bangDiem.hinhAnh;
        //        }
        //        DateTime date1 = DateTime.Today;
        //        bdExisted1.thoiGian = date1;
        //        bdExisted1.banPhuTrach = bangDiem.banPhuTrach;
        //        db.SaveChanges();

        //        //update bảng điểm của các đon vị cha
        //        var giaoChiTieuList = db.giaoChiTieuchoDVs.Where(g => g.fk_chiTieu == iD_chiTieu).ToList();
        //        foreach(var bd in db.bangDiems)
        //        {
        //            foreach(var giao in giaoChiTieuList)
        //            {
        //                if (bd.fk_giaoChiTieu == giao.id)
        //                {
        //                    bd.ycMinhChung = bangDiem.ycMinhChung;
        //                    if (hinhAnh != null)
        //                    {

        //                        Console.WriteLine(bangDiem.hinhAnh);
        //                        bd.hinhAnh = bangDiem.hinhAnh;
        //                    }
        //                    bd.thoiGian = date1;
        //                    bd.banPhuTrach = bangDiem.banPhuTrach;
        //                    db.Entry(bd).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
        //                }
        //            }
        //        }
        //        db.SaveChanges();

        //        return RedirectToAction("chamDiemLoaiTieuChi", new {id = id });
        //}
        //return RedirectToAction("Index");
        //}
    }
}