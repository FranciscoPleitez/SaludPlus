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
    public class ProductosController : Controller
    {
        private SaludPlusDBEntities1 db = new SaludPlusDBEntities1();

        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categorias).Include(p => p.Proveedores).ToList();
            return View(productos);
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Productos productos = db.Productos.Find(id);
            if (productos == null)
                return HttpNotFound();

            return View(productos);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaID", "Nombre");
            ViewBag.Proveedores = new SelectList(db.Proveedores, "ProveedorID", "Nombre");
            return View(new Productos());
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Productos producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaID", "Nombre", producto.CategoriaID);
            ViewBag.Proveedores = new SelectList(db.Proveedores, "ProveedorID", "Nombre", producto.ProveedorID);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Productos producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaID", "Nombre", producto.CategoriaID);
            ViewBag.Proveedores = new SelectList(db.Proveedores, "ProveedorID", "Nombre", producto.ProveedorID);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Productos producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaID", "Nombre", producto.CategoriaID);
            ViewBag.Proveedores = new SelectList(db.Proveedores, "ProveedorID", "Nombre", producto.ProveedorID);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}