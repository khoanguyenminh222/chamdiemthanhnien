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
        public ActionResult Index()
        {
            if (Session["dm_DonVi"] == null)
            {
                return RedirectToAction("Login", "nguoiDung");
            }
            var chiTieux = db.chiTieux.Include(c => c.nhomChiTieu);
            return View(chiTieux.ToList());
        }

        // GET: chiTieu/Details/5
        public ActionResult Details(int? id)
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

        // GET: chiTieu/Create
        public ActionResult Create()
        {
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten");
            return View();
        }

        // POST: chiTieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iD,ten,fk_loaiChiTieu")] chiTieu chiTieu)
        {
            if (ModelState.IsValid)
            {
                db.chiTieux.Add(chiTieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten", chiTieu.fk_loaiChiTieu);
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
            List<chiTietChiTieu> chiTietChiTieu = db.chiTietChiTieux.Where(c=>c.fk_loaiChiTieu==id).ToList();
            if (chiTieu == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten", chiTieu.fk_loaiChiTieu);
            chiTieu_chiTiet chiTieu_ChiTiet = new chiTieu_chiTiet
            {
                chiTieu = chiTieu,
                chiTietChiTieu = chiTietChiTieu
            };
            return View(chiTieu_ChiTiet);
        }

        // POST: chiTieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,ten,fk_loaiChiTieu")] chiTieu chiTieu)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(chiTieu).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten", chiTieu.fk_loaiChiTieu);
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
