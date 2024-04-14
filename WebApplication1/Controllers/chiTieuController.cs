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
    public class chiTieuController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: chiTieu
        public ActionResult Index(int? year, int? loaiTieuChi)
        {
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            var distinctYears = db.loaiTieuChis.Select(l => l.nam).Distinct().ToList();

            // Chọn năm mặc định là năm hiện tại nếu chưa có giá trị được chọn hoặc giá trị được chọn không có trong danh sách các năm khả dụng
            if (year == null || !distinctYears.Contains(year.Value))
            {
                year = DateTime.Now.Year;
            }

            // Đưa danh sách năm vào ViewBag
            ViewBag.listYear = new SelectList(distinctYears, year);

            // Lấy danh sách các loại tiêu chí từ cơ sở dữ liệu
            var loaiTieuChiList = db.loaiTieuChis.Where(l => l.nam == year).ToList();

            // Đưa danh sách các loại tiêu chí vào ViewBag
            ViewBag.loaiTieuChiList = new SelectList(loaiTieuChiList, "ID", "Ten", loaiTieuChi);

            // Lọc dữ liệu theo năm nếu có giá trị được chọn
            var chiTieux = db.chiTieux.Include(c => c.nhomChiTieu);
            if (year != null)
            {
                chiTieux = chiTieux.Where(c => c.nhomChiTieu.loaiTieuChi.nam == year);
            }

            // Lọc dữ liệu theo loại tiêu chí nếu có giá trị được chọn
            if (loaiTieuChi != null)
            {
                chiTieux = chiTieux.Where(c => c.nhomChiTieu.loaiTieuChi.iD == loaiTieuChi);
            }
            return View(chiTieux.ToList());
        }

        // GET: chiTieu/Create
        public ActionResult Create()
        {
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            var years = db.loaiTieuChis.Select(l => l.nam).Distinct().ToList();
            var yearsList = years.Select(year => new SelectListItem
            {
                Text = year.ToString(),
                Value = year.ToString()
            });
            // Truyền danh sách các năm vào ViewBag
            ViewBag.YearsList = new SelectList(yearsList, "Value", "Text");
            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten");
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten");
            return View();
        }

        // POST: chiTieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD, fk_loaiChiTieu")] chiTieu chiTieu, List<chiTieu> chiTieuList)
        {
            if (ModelState.IsValid)
            {
                foreach (var n in chiTieuList)
                {
                    n.fk_loaiChiTieu = chiTieu.fk_loaiChiTieu;
                    db.chiTieux.Add(n);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var years = db.loaiTieuChis.Select(l => l.nam).Distinct().ToList();
            var yearsList = years.Select(year => new SelectListItem
            {
                Text = year.ToString(),
                Value = year.ToString()
            });
            // Truyền danh sách các năm vào ViewBag
            ViewBag.YearsList = new SelectList(yearsList, "Value", "Text");
            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten");
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten");
            return View(chiTieu);
        }

        // GET: chiTieu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chiTieu chiTieu = db.chiTieux.Find(id);
            if (chiTieu == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten", chiTieu.fk_loaiChiTieu);
            return View(chiTieu);
        }

        // POST: chiTieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,ten,fk_loaiChiTieu,ycDanhGiaKQ")] chiTieu chiTieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTieu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten", chiTieu.fk_loaiChiTieu);
            return View(chiTieu);
        }

        // GET: chiTieu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chiTieu chiTieu = db.chiTieux.Find(id);
            if (chiTieu == null)
            {
                return HttpNotFound();
            }
            return View(chiTieu);
        }

        // POST: chiTieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            chiTieu chiTieu = db.chiTieux.Find(id);
            db.chiTieux.Remove(chiTieu);
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
