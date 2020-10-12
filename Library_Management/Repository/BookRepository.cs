using Library_Management.Data;
using Library_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Library_Management.Repository
{


    public class BookRepository
    {
        private readonly LibraryContext _context = null;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }


        public async Task<int> AddNewBook(BookModel model)
        {

       

            var newBook = new Books()
            {


                Title = model.Title,
                Author = model.Author,
                Category = model.Category,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                Language = model.Language,
                TotalPages = model.TotalPages.HasValue ? model.TotalPages.Value : 0,
                UpdatedOn = DateTime.UtcNow,
                CoverImageURL = model.CoverImageUrl,
                PdfBookUrl = model.PdfBookUrl


            };

            newBook.bookGallery = new List<BookGallery>();
            foreach (var item in model.Gallery)
            {
                newBook.bookGallery.Add(new BookGallery()
                {

                    Name = item.Name,
                    URL = item.URL

                });
            }
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return newBook.Id;

        }
        public async Task<List<BookModel>> GetAllBooks()
        {
            var books = new List<BookModel>();
            var allbooks = await _context.Books.ToListAsync();
            if (allbooks.Any() == true)
            {
                foreach (var item in allbooks)
                {
                    books.Add(new BookModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Author = item.Author,
                        Description = item.Description,
                        TotalPages = item.TotalPages,
                        Language = item.Language,
                        Category = item.Category,
                        CoverImageUrl = item.CoverImageURL


                    });
                }
            }

            return books;
        }



        public async Task<BookModel> GetBooksById(int id)
        {
            return await _context.Books.Where(x => x.Id == id)
                  .Select(book => new BookModel()
                  {
                      Author = book.Author,
                      Category = book.Category,
                      Description = book.Description,
                      Id = book.Id,
                      Language = book.Language,
                      Title = book.Title,
                      TotalPages = book.TotalPages,
                      CoverImageUrl = book.CoverImageURL,
                      Gallery = book.bookGallery.Select(g => new GalleryModel()
                      {
                          Id = g.Id,
                          Name = g.Name,
                          URL = g.URL
                      }).ToList(),
                      PdfBookUrl = book.PdfBookUrl

                  }).FirstOrDefaultAsync();
            
        }





        //public async Task<bool> UpdateBook(int id, BookModel model)
        //{
        //    var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);


        //    if (book != null)
        //    {
        //        book.Title = model.Title;
        //        book.Author = model.Author;
        //       // book.CreatedOn = DateTime.UtcNow;
        //        book.Description = model.Description;
        //        book.Language = model.Language;
        //        book.TotalPages = model.TotalPages.HasValue ? model.TotalPages.Value : 0;
        //        book.UpdatedOn = DateTime.UtcNow;
        //        book.CoverImageURL = model.CoverImageUrl;
        //        book.PdfBookUrl = model.PdfBookUrl;

        //    }
        //    await _context.SaveChangesAsync();
        //    return true;

        //}


       


        public async Task<bool> DeleteB(int id)
        {
            
          var DelB =  await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
          _context.Books.Remove(DelB);
           await _context.SaveChangesAsync();
            return true;

        }
    }
}
