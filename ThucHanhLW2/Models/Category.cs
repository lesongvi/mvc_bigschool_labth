using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThucHanhLW2.Models
{

    public class Category
    {
        public byte Id { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Loại")]
        public string Name { get; set; }
    }
}