using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanhLW2.Models;
using ThucHanhLW2.ViewModels;

namespace ThucHanhLW2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;
        public HomeController ()
        {
            _dbContext = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var upComingCourses = _dbContext.Courses
                .Include("Lecturer")
                .Include("Category")
                .Where(c => c.DateTime > DateTime.Now);

            var vm = new CourseViewModel
            {
                UpComingCourses = upComingCourses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}