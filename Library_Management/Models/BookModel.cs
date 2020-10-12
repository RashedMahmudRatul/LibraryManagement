using Library_Management.Migrations.Library;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management.Models
{
    public class BookModel
    {

        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required(ErrorMessage = "Please Enter The Title Of your Book")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please Enter The Author Name of your Book")]
        public string Author { get; set; }
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Please Enter Some Descriotion of your Book")]
        public string Description { get; set; }
        public string Category { get; set; }


        public string Language { get; set; }
        [Required(ErrorMessage = "Please Enter The Number of pages Title Of your Book")]
        public int? TotalPages { get; set; }

        [Display(Name = "Choose the Cover Photo")]
        [Required]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }


        [Display(Name = "Choose the Gallery Photo")]
        [Required]
        public IFormFileCollection GalleryFiles { get; set; }

        public List<GalleryModel> Gallery { get; set; }

        [Display(Name = "Choose the PDF")]
        [Required]
        [DataType(DataType.Upload)]

        public IFormFile PdfBook { get; set; }
        public string PdfBookUrl { get; set; }



    }
}
