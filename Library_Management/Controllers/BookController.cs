using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Library_Management.Data;
using Library_Management.Models;
using Library_Management.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library_Management.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;

       

        public BookController(BookRepository bookRepository, IWebHostEnvironment webHostEnvironment)
        {
           
            _bookRepository = bookRepository;
            _webHostEnvironment = webHostEnvironment;
            
        }

        //public List<BookModel> GetAllBooks()
        // {
        //     return _bookRepository.GetAllBooks();
        // }

        public async Task<ViewResult> GetAllBooks(string sname)
        {
            var data = await _bookRepository.GetAllBooks();
            return View(data);
        }


        //[Route("book-details/{id}")]
        public async Task<ViewResult> GetBook(int id  /*string bookname*/)
        {
            var detailsdata = await _bookRepository.GetBooksById(id);
            return View(detailsdata);
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult AddNewBook(bool isSuccess = false, int bookId = 0)
        {

            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Manager")]
        public async Task<IActionResult> AddNewBook(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {

                if (bookModel.CoverPhoto!=null)
                {
                    string folder = "books/cover/";
                   bookModel.CoverImageUrl =  await UploadImage(folder, bookModel.CoverPhoto);

                }

                if (bookModel.GalleryFiles != null)
                {
                    string folder = "books/Gallery/";
                    bookModel.Gallery = new List<GalleryModel>();
                    foreach (var item in bookModel.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = item.FileName,
                            URL = await UploadImage(folder, item)
                        };
                        bookModel.Gallery.Add(gallery);
                    }
                }


                var ext = Path.GetExtension(bookModel.PdfBook.FileName);

                if (ext != "pdf")
                {
                    ViewBag.msg = "File Extension Is InValid - Only Upload PDF File";
                }
                if (bookModel.PdfBook != null)
                      {
                        string folder = "books/bookpdf/";
                        bookModel.PdfBookUrl = await UploadImage(folder, bookModel.PdfBook);

                       }

                int id = await _bookRepository.AddNewBook(bookModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewBook), new { isSuccess = true, bookId = id });
                }
            }
            return View();
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            //string folder = "books/cover/";

            folderPath += Guid.NewGuid().ToString() + " " + file.FileName;
          
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ViewResult> Delete(Books books, int id)
        {
            var detailsdata = await _bookRepository.GetBooksById(id);

            return View(detailsdata);
        }
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteConf(int id)
        {
            await _bookRepository.DeleteB(id);
            return RedirectToAction("GetAllBooks");
        }


      




        //public async Task<ViewResult> Edit(int id)
        //{
        //    var data = await _bookRepository.UpdateBook(Books);
        //    return View(data);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(BookModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _bookRepository.UpdateBook(model.Id, model);
        //        return RedirectToAction("GetAllBooks");
        //    }
        //    return View();

        //}




    }
}
