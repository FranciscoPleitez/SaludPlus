using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaludPlusAdmin.Models
{
	public class CartItem
	{
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}