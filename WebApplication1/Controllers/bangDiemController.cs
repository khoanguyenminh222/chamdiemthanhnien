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
    public class bangDiemController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();

        // GET: bangDiem
        public ActionResult Index()
        {

            var bangDiem = (from chiTieu in db.chiTieux
                            join giaoChiTieu in db.giaoChiTieuchoDVs
                                on chiTieu.iD equals giaoChiTieu.fk_chiTieu
                            join bangdiem in db.bangDiems
                                on giaoChiTieu.id equals bangdiem.fk_giaoChiTieu
                            join chiTietChiTieu in db.chiTietChiTieux
                                on chiTieu.iD equals chiTietChiTieu.fk_loaiChiTieu
                            join nhomChiTeu in db.nhomChiTieux
                                on chiTieu.fk_loaiChiTieu equals nhomChiTeu.iD
                            join loaiTieuChi in db.loaiTieuChis
                                on nhomChiTeu.fk_loaiTieuChi equals loaiTieuChi.iD
                            select new dataBangDiem()
                            {
                                bangDiem = bangdiem,
                                loaiTieuChi = loaiTieuChi,
                                nhomChiTieu = nhomChiTeu,
                                chiTieu = chiTieu,
                                chiTietChiTieu = chiTietChiTieu,
                            }).OrderBy(o => o.chiTieu.iD); ;
            ViewBag.bangdiem = bangDiem.ToList();
            return View();
        }

        // GET: bangDiem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bangDiem bangDiem = db.bangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // GET: bangDiem/Create
        public ActionResult Create()
        {
            ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id");
            return View();
        }

        // POST: bangDiem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fk_giaoChiTieu,diem,ycDanhGiaKQ,ycMinhChung,thoiGian,banPhuTrach")] bangDiem bangDiem)
        {
            if (ModelState.IsValid)
            {
                db.bangDiems.Add(bangDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id", bangDiem.fk_giaoChiTieu);
            return View(bangDiem);
        }

        // GET: bangDiem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bangDiem bangDiem = db.bangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id", bangDiem.fk_giaoChiTieu);
            return View(bangDiem);
        }

        // POST: bangDiem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fk_giaoChiTieu,diem,ycDanhGiaKQ,ycMinhChung,thoiGian,banPhuTrach")] bangDiem bangDiem)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(bangDiem).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.fk_giaoChiTieu = new SelectList(db.giaoChiTieuchoDVs, "id", "id", bangDiem.fk_giaoChiTieu);
            return View(bangDiem);
        }

        // GET: bangDiem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bangDiem bangDiem = db.bangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // POST: bangDiem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bangDiem bangDiem = db.bangDiems.Find(id);
            db.bangDiems.Remove(bangDiem);
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
