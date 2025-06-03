using SaludPlusAdmin.Models;
using System.Linq;
using System.Web.Mvc;

namespace SaludPlusAdmin.Controllers
{
    public class AuthController : Controller
    {
        private SaludPlusDBEntities1 db = new SaludPlusDBEntities1();

        // GET: Auth/Index
        public ActionResult Index()
        {
            return View(); // Busca la vista Views/Auth/Index.cshtml
        }

        // POST: Auth/Index
        [HttpPost]
        public ActionResult Index(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Usuario
                    .FirstOrDefault(c => c.Nombre == model.Nombre && c.Contraseña == model.Contraseña);

                if (usuario != null)
                {
                    Session["Id"] = usuario.Id;
                    Session["Nombre"] = usuario.Nombre;
                    Session["Contraseña"] = usuario.Contraseña;
                    Session["Rol"] = usuario.Rol;

                    // Redirigir según el rol
                    if (usuario.Rol == "Administrador")
                    {
                        return RedirectToAction("Index", "Home"); // Vista para admin
                    }
                    else if (usuario.Rol == "Vendedor")
                    {
                        return RedirectToAction("Index", "Home"); // Vista para admin
                    }
                    else if (usuario.Rol == "Cliente")
                    {
                        return RedirectToAction("CompraCliente", "Carrito"); // Vista para clientes
                    }

                    // Si el rol es desconocido
                    ViewBag.Error = "Rol de usuario no reconocido.";
                    return View(model);
                }

                ViewBag.Error = "Credenciales incorrectas";
            }

            return View(model); // Vuelve a mostrar el formulario con el mensaje de error
        }
        // GET: Auth/Logout
        public ActionResult Logout()
        {
            // Limpia todas las variables de sesión
            Session.Clear();
            Session.Abandon(); // Termina la sesión

            // Redirige al inicio de sesión (o a la página principal)
            return RedirectToAction("Index", "Auth");
        }
    }
}