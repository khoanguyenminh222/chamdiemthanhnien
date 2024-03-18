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
                            }).OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
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
                            }).OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                            .ThenBy(o => o.chiTieu.iD);
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
                Console.WriteLine(data.ToList());
                if (data.Count() > 0)
                {
                    Session["dm_DonVi"] = data.FirstOrDefault().taiKhoan.fk_dmDonVi;
                    Session["name"] = data.FirstOrDefault().taiKhoan.ten;
                    Session["donvi"] = data.FirstOrDefault().nguoiDung.fk_donVi;
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
                                      int iD_chiTieu)
        {
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
            if (quanHeDonVi==null)
            {
               
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
                db.bangDiems.Add(bangDiem1);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}