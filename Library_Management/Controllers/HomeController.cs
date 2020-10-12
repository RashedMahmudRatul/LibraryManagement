using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Library_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Library_Management.Repository;

namespace Library_Management.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BookRepository _bookRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        private readonly ILogger<HomeController> _logger;

       
        

        public HomeController(ILogger<HomeController> logger, BookRepository bookRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
       [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index(int id)
        {
            var detailsdata = await _bookRepository.GetBooksById(id);
            return View(detailsdata);

            //return View();
        }

        public ViewResult AboutUs()
        {
            return View();
        }

        public ViewResult ContactUs()
        {
            return View();
        }


        [Authorize(Roles = "Manager")]
         public IActionResult ManagerPage()
         {
         return View();
         }

      [Authorize]
      public IActionResult UserPage()
      {
            
           return View();
      }

      //  public IActionResult Privacy()
      //  {
      //      return View();
      //  }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
