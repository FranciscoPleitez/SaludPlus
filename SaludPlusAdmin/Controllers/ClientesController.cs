using SaludPlusAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaludPlusAdmin.Controllers
{
    public class ClientesController : Controller
    {
        private SaludPlusDBEntities1 db = new SaludPlusDBEntities1();

        // GET: CompraCliente
        public ActionResult CompraCliente()
{
    var productos = db.Productos.Where(p => p.Activo == true).ToList();
    return View("~/Views/Carrito/CompraCliente.cshtml", productos); // Ruta absoluta
}

        // POST: Agregar al carrito
        [HttpPost]
        public ActionResult AgregarAlCarrito(int id)
        {
            var producto = db.Productos.Find(id);
            if (producto == null)
                return HttpNotFound();

            List<CartItem> carrito = Session["Carrito"] as List<CartItem> ?? new List<CartItem>();

            var itemExistente = carrito.FirstOrDefault(p => p.ProductoID == id);
            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                carrito.Add(new CartItem
                {
                    ProductoID = producto.ProductoID,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1
                });
            }

            Session["Carrito"] = carrito;
            return Json(new { success = true });
        }
    }
}