using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Entity
{
    public class Review
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This Area is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This Area is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Area is Required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "This Area is Required")]
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
