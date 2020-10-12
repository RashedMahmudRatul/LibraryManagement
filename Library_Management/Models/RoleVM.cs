using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management.Models
{
    public class RoleVM
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
