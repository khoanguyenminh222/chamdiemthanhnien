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
        public ActionResult Index()
        {
            var nhomChiTieux = db.nhomChiTieux.Include(n => n.loaiTieuChi);
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
            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten");
            return View();
        }

        // POST: nhomChiTieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD,ten,tongDiem,fk_loaiTieuChi")] nhomChiTieu nhomChiTieu)
        {
            if (ModelState.IsValid)
            {
                db.nhomChiTieux.Add(nhomChiTieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten", nhomChiTieu.fk_loaiTieuChi);
            return View(nhomChiTieu);
        }

        // GET: nhomChiTieu/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten", nhomChiTieu.fk_loaiTieuChi);
            return View(nhomChiTieu);
        }

        // POST: nhomChiTieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,ten,tongDiem,fk_loaiTieuChi")] nhomChiTieu nhomChiTieu)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(nhomChiTieu).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.fk_loaiTieuChi = new SelectList(db.loaiTieuChis, "iD", "ten", nhomChiTieu.fk_loaiTieuChi);
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
