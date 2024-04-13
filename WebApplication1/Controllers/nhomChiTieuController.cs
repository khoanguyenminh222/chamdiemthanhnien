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
    public class nhomChiTieuController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: nhomChiTieu
        public ActionResult Index(int? year)
        {
            var distinctYears = db.loaiTieuChis.Select(l => l.nam).Distinct().ToList();

            // Chọn năm mặc định là năm hiện tại nếu chưa có giá trị được chọn
            if (year == null || !distinctYears.Contains(year.Value))
            {
                year = DateTime.Now.Year;
            }

            // Đưa danh sách năm vào ViewBag
            ViewBag.listYear = new SelectList(distinctYears, year);
            var loaiTieuChi = db.loaiTieuChis.Where(l => l.nam == year).ToList();
            var nhomChiTieux = db.nhomChiTieux.Include(n => n.loaiTieuChi).Where(n => n.loaiTieuChi.nam == year).OrderBy(l => l.loaiTieuChi.iD).ToList();
            return View(nhomChiTieux.ToList());
        }

        // GET: nhomChiTieu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nhomChiTieu nhomChiTieu = db.nhomChiTieux.Find(id);
            if (nhomChiTieu == null)
            {
                return HttpNotFound();
            }
            return View(nhomChiTieu);
        }

        // GET: nhomChiTieu/Create
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
            return View();
        }

        // POST: nhomChiTieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD,fk_loaiTieuChi")] nhomChiTieu nhomChiTieu, List<nhomChiTieu> nhomChiTieuList)
        {
            if (ModelState.IsValid)
            {
                foreach (var n in nhomChiTieuList)
                {
                    n.fk_loaiTieuChi = nhomChiTieu.fk_loaiTieuChi;
                    db.nhomChiTieux.Add(n);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhomChiTieu);
        }
        public ActionResult GetLoaiTieuChi(int selectedYear)
        {
            var loaiTieuChis = db.loaiTieuChis.Where(l => l.nam == selectedYear)
                .Select(l => new { iD = l.iD, ten = l.ten })
                .ToList();
            return Json(loaiTieuChis, JsonRequestBehavior.AllowGet);
        }
        // GET: nhomChiTieu/Edit/5
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
            nhomChiTieu nhomChiTieu = db.nhomChiTieux.Find(id);
            if (nhomChiTieu == null)
            {
                return HttpNotFound();
            }
            var years = db.loaiTieuChis.Select(l => l.nam).Distinct().ToList();
            var yearsList = years.Select(year => new SelectListItem
            {
                Text = year.ToString(),
                Value = year.ToString()
            });
            // Truyền danh sách các năm vào ViewBag
            ViewBag.YearsList = new SelectList(yearsList, "Value", "Text");
            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis.ToList(), "iD", "ten");
            return View(nhomChiTieu);
        }

        // POST: nhomChiTieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,ten,tongDiem,fk_loaiTieuChi")] nhomChiTieu nhomChiTieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhomChiTieu).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
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
            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten", nhomChiTieu.fk_loaiTieuChi);
            return View(nhomChiTieu);
        }

        // GET: nhomChiTieu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nhomChiTieu nhomChiTieu = db.nhomChiTieux.Find(id);
            if (nhomChiTieu == null)
            {
                return HttpNotFound();
            }
            return View(nhomChiTieu);
        }

        // POST: nhomChiTieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            nhomChiTieu nhomChiTieu = db.nhomChiTieux.Find(id);
            db.nhomChiTieux.Remove(nhomChiTieu);
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
