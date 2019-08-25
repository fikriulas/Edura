using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Bu Alan Gereklidir.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu Alan Gereklidir.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu Alan Gereklidir."),EmailAddress]
        public string Email { get; set; }
    }
}
