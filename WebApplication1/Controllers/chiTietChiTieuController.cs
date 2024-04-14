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
    public class chiTietChiTieuController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: chiTietChiTieu
        public ActionResult Index(int? year, int? loaiTieuChi, int? nhomChiTieu)
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

            var nhomChiTieuList = db.nhomChiTieux.Where(n => n.fk_loaiTieuChi == loaiTieuChi).ToList();

            // Đưa danh sách các loại tiêu chí vào ViewBag
            ViewBag.nhomChiTieuList = new SelectList(nhomChiTieuList, "ID", "Ten", nhomChiTieu);
            var chiTietChiTieux = db.chiTietChiTieux.Include(c => c.chiTieu)
                                             .Where(c => c.chiTieu.nhomChiTieu.fk_loaiTieuChi == loaiTieuChi
                                                      && c.chiTieu.fk_loaiChiTieu == nhomChiTieu);
            return View(chiTietChiTieux.ToList());
        }

        // GET: chiTietChiTieu/Create
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
            ViewBag.fk_nhomChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten");
            ViewBag.fk_loaiChiTieu = new SelectList(db.chiTieux, "iD", "ten");
            return View();
        }

        // POST: chiTietChiTieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD,fk_loaiChiTieu")] chiTietChiTieu chiTietChiTieu, List<chiTietChiTieu> chiTietChiTieuList)
        {
            if (ModelState.IsValid)
            {
                foreach (var n in chiTietChiTieuList)
                {
                    n.fk_loaiChiTieu = chiTietChiTieu.fk_loaiChiTieu;
                    db.chiTietChiTieux.Add(n);
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
            ViewBag.fk_nhomChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten");
            ViewBag.fk_loaiChiTieu = new SelectList(db.chiTieux, "iD", "ten", chiTietChiTieu.fk_loaiChiTieu);
            return View(chiTietChiTieu);
        }

        // GET: chiTietChiTieu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chiTietChiTieu chiTietChiTieu = db.chiTietChiTieux.Find(id);
            if (chiTietChiTieu == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_loaiChiTieu = new SelectList(db.chiTieux, "iD", "ten", chiTietChiTieu.fk_loaiChiTieu);
            return View(chiTietChiTieu);
        }

        // POST: chiTietChiTieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,noiDung,diem,fk_loaiChiTieu")] chiTietChiTieu chiTietChiTieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietChiTieu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fk_loaiChiTieu = new SelectList(db.chiTieux, "iD", "ten", chiTietChiTieu.fk_loaiChiTieu);
            return View(chiTietChiTieu);
        }

        // GET: chiTietChiTieu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chiTietChiTieu chiTietChiTieu = db.chiTietChiTieux.Find(id);
            if (chiTietChiTieu == null)
            {
                return HttpNotFound();
            }
            return View(chiTietChiTieu);
        }

        // POST: chiTietChiTieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            chiTietChiTieu chiTietChiTieu = db.chiTietChiTieux.Find(id);
            db.chiTietChiTieux.Remove(chiTietChiTieu);
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
