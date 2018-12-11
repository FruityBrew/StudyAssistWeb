using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyAssist.Core;
using StudyAssist.Core.Interfaces;
using StudyAssist.Domain.Interfaces;
using StudyAssist.Infrastructure.Data;
using StudyAssistWebApp.Models;

namespace StudyAssistWebApp.Controllers
{
    public class HomeController : Controller
    {
        //private IRepository<IProblem> _problemRepository;

        private UnitOfWork _uw;


        public HomeController(
            IRepository<IProblem> problemRepository, 
            IRepository<ICategory> categoryRepository)
        {
            _uw = new UnitOfWork(problemRepository, categoryRepository);
        }

        public IActionResult Index()
        {
            //IResult<IEnumerable<IProblem>> result =
            //    _uw.Problems.GetList();


            ICategory cat = new Category();
            cat.Name = "Тест";

             var res = _uw.Categories.Add(cat);

            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
