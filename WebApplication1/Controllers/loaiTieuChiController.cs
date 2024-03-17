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
    public class loaiTieuChiController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: loaiTieuChi
        public ActionResult Index()
        {
            return View(db.loaiTieuChis.ToList());
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

        // GET: loaiTieuChi/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: loaiTieuChi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iD,ten,tongDiem")] loaiTieuChi loaiTieuChi)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(loaiTieuChi).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(loaiTieuChi);
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
