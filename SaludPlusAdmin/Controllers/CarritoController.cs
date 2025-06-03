using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaludPlusAdmin.Models;

namespace SaludPlusAdmin.Controllers
{
    public class CarritoController : Controller
    {
        private SaludPlusDBEntities1 db = new SaludPlusDBEntities1();
        // ID del empleado usado para ventas en línea
        int empleadoId = 11;
        // Mostrar productos disponibles
        public ActionResult CompraCliente()
        {
            var productos = db.Productos.Where(p => p.StockActual > 0).ToList();
            return View(productos);
        }

        // Mostrar el carrito
        public ActionResult Carrito()
        {
            var carrito = Session["Carrito"] as List<CartItem>;
            if (carrito == null)
            {
                carrito = new List<CartItem>();
            }
            return View(carrito);
        }

        [HttpPost]
        public ActionResult Agregar(int id, int cantidad)
        {
            var producto = db.Productos.Find(id);
            if (producto == null || cantidad <= 0)
            {
                return Json(new { success = false });
            }

            var carrito = Session["Carrito"] as List<CartItem> ?? new List<CartItem>();
            var itemExistente = carrito.FirstOrDefault(p => p.ProductoID == id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CartItem
                {
                    ProductoID = producto.ProductoID,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = cantidad
                });
            }

            Session["Carrito"] = carrito;

            // Devuelve la nueva cantidad total
            return Json(new
            {
                success = true,
                totalItems = carrito.Sum(x => x.Cantidad)
            });
        }

        // Eliminar producto del carrito
        public ActionResult Eliminar(int id)
        {
            var carrito = Session["Carrito"] as List<CartItem>;
            if (carrito != null)
            {
                var item = carrito.FirstOrDefault(p => p.ProductoID == id);
                if (item != null)
                {
                    carrito.Remove(item);
                    Session["Carrito"] = carrito;
                }
            }
            return RedirectToAction("Carrito");
        }

        // Finalizar compra
        [HttpPost]
        public ActionResult FinalizarCompra()
        {
            var carrito = Session["Carrito"] as List<CartItem>;
            if (carrito == null || !carrito.Any())
            {
                return RedirectToAction("Carrito");
            }

            // Suponiendo que el cliente está autenticado (puedes ajustar esto)
            int clienteId = 1; // cliente autenticado

            // Crear venta
            Ventas nuevaVenta = new Ventas
            {
                Fecha = DateTime.Now,
                TipoVenta = "Online",
                MetodoPago = "Tarjeta",
                CostoEnvio = 0,
                Total = carrito.Sum(x => x.Precio * x.Cantidad),
                Estado = "Completado",
                ClienteID = clienteId,
                EmpleadoID = empleadoId
            };

            db.Ventas.Add(nuevaVenta);
            db.SaveChanges();

            // Guardar detalle de venta
            foreach (var item in carrito)
            {
                db.DetalleVenta.Add(new DetalleVenta
                {
                    VentaID = nuevaVenta.VentaID,
                    ProductoID = item.ProductoID,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio
                });

                // Reducir stock del producto
                var producto = db.Productos.Find(item.ProductoID);
                if (producto != null)
                {
                    producto.StockActual -= item.Cantidad;
                }
            }

            db.SaveChanges();
            Session["Carrito"] = null;

            TempData["Mensaje"] = "¡Compra realizada con éxito!";
            return RedirectToAction("CompraCliente");



        }

        //actualizar
        [HttpPost]
        public ActionResult ActualizarCantidad(int id, int cantidad)
        {
            var carrito = Session["Carrito"] as List<CartItem>;
            if (carrito == null)
            {
                return HttpNotFound("Carrito no encontrado.");
            }

            var item = carrito.FirstOrDefault(p => p.ProductoID == id);
            if (item == null)
            {
                return HttpNotFound("Producto no encontrado en el carrito.");
            }

            if (cantidad <= 0)
            {
                carrito.Remove(item);
            }
            else
            {
                item.Cantidad = cantidad;
            }

            Session["Carrito"] = carrito;
            return RedirectToAction("Carrito");
        }
    }
}