using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Models
{
    public class OrderDetails
    {
        [Required(ErrorMessage ="This Area is Required!")]
        public string AdresBasligi { get; set; }
        [Required(ErrorMessage = "This Area is Required!")]
        public string Adres { get; set; }
        [Required(ErrorMessage = "This Area is Required!")]
        public string Sehir { get; set; }
        [Required(ErrorMessage = "This Area is Required!")]
        public string Semt { get; set; }
        [Required(ErrorMessage = "This Area is Required!")]
        public string Telefon { get; set; }
    }
}
