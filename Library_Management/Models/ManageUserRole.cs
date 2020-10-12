using Library_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management.Models
{
    public class ManageUserRole
    {
        public ApplicationUser AppUser { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
