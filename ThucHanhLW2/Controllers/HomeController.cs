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
            IEnumerable<Course> upComingCourses = _dbContext.Courses
                .Include("Lecturer")
                .Include("Category")
                .Where(c => c.DateTime > DateTime.Now)
                .Where(ucc => ucc.IsCanceled == false);

            var vm = new CourseViewModel
            {
                UpComingCourses = upComingCourses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var course = _dbContext.Courses
                .Include("Category")
                .Include("Lecturer")
                .Single(c => c.Id == id);

            return View(course);
        }
    }
}