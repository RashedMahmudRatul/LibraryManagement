using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management.Data
{
    public class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
       // public string Language { get; set; }
        public int TotalPages { get; set; }
        public string CoverImageURL { get; set; }
        public string PdfBookUrl { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        //public  Language Language { get; set; }



        public ICollection<BookGallery> bookGallery { get; set; }


    }
}
