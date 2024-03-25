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
        public ActionResult Index()
        {
            var chiTietChiTieux = db.chiTietChiTieux.Include(c => c.chiTieu);
            return View(chiTietChiTieux.ToList());
        }

        // GET: chiTietChiTieu/Create
        public ActionResult Create()
        {
            ViewBag.fk_loaiChiTieu = new SelectList(db.chiTieux, "iD", "ten");
            return View();
        }

        // POST: chiTietChiTieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD,noiDung,diem,fk_loaiChiTieu")] chiTietChiTieu chiTietChiTieu)
        {
            if (ModelState.IsValid)
            {
                db.chiTietChiTieux.Add(chiTietChiTieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
