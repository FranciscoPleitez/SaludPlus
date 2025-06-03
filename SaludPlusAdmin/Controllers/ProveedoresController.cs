using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaludPlusAdmin.Models;

namespace SaludPlusAdmin.Controllers
{
    public class ProveedoresController : Controller
    {
        private SaludPlusDBEntities1 db = new SaludPlusDBEntities1();

        // GET: Proveedores
        public ActionResult Index()
        {
            return View(db.Proveedores.ToList()); // Cargar todos los proveedores
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var proveedor = db.Proveedores.Find(id);
            if (proveedor == null) return HttpNotFound();
            return View(proveedor);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProveedorID,Nombre,CUIT,Direccion,Telefono,Email,CondicionesPago,TiempoEstimadoEntrega,Activo")] Proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Proveedores.Add(proveedor);  // Agregar proveedor a la base de datos
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var proveedor = db.Proveedores.Find(id);
            if (proveedor == null) return HttpNotFound();
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProveedorID,Nombre,CUIT,Direccion,Telefono,Email,CondicionesPago,TiempoEstimadoEntrega,Activo")] Proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;  // Actualizar proveedor
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var proveedor = db.Proveedores.Find(id);
            if (proveedor == null) return HttpNotFound();
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var proveedor = db.Proveedores.Find(id);
            db.Proveedores.Remove(proveedor);  // Eliminar proveedor
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