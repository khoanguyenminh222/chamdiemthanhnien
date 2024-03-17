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
    public class chiTieuChamDiemController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: chiTieuChamDiem
        public ActionResult Index()
        {
            var chiTieuChamDiem = (from bangdiem in db.bangDiems
                            join giaoChiTieu in db.giaoChiTieuchoDVs
                                on bangdiem.fk_giaoChiTieu equals giaoChiTieu.id
                            join chiTieu in db.chiTieux
                                on giaoChiTieu.fk_chiTieu equals chiTieu.iD
                            join chiTietChiTieu in db.chiTietChiTieux
                                on chiTieu.iD equals chiTietChiTieu.fk_loaiChiTieu
                            
                            select new dataBangDiem()
                            {
                                bangDiem = bangdiem,
                                chiTieu = chiTieu,
                                chiTietChiTieu = chiTietChiTieu,
                            });
            var chiTieux = db.chiTieux.Include(c => c.nhomChiTieu);
            return View(chiTieux.ToList());
        }

        // GET: chiTieuChamDiem/Details/5
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

        // GET: chiTieuChamDiem/Create
        public ActionResult Create()
        {
            ViewBag.fk_loaiChiTieu = new SelectList(db.nhomChiTieux, "iD", "ten");
            return View();
        }

        // POST: chiTieuChamDiem/Create
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

        // GET: chiTieuChamDiem/Edit/5
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

        // POST: chiTieuChamDiem/Edit/5
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

        // GET: chiTieuChamDiem/Delete/5
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

        // POST: chiTieuChamDiem/Delete/5
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
