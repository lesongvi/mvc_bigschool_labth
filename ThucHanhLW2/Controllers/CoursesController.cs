using System;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanhLW2.Models;
using ThucHanhLW2.ViewModels;
using System.Data.Entity;

namespace ThucHanhLW2.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult Index ()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course" 
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    LecturerId = User.Identity.GetUserId(),
                    DateTime = vm.GetDateTime(),
                    CategoryId = vm.Category,
                    Place = vm.Place
                };
                _dbContext.Courses.Add(course);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            } else
            {
                vm.Categories = _dbContext.Categories.ToList();
                return View("Create", vm);
            }
        }

        [Authorize]
        public ActionResult Attending ()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include("Lecturer")
                .Include("Category")
                .ToList();

            var vm = new CourseViewModel
            {
                UpComingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(vm);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();

            var mineCourses = _dbContext.Courses
                .Where(a => a.LecturerId == userId)
                .Where(a => a.DateTime >= DateTime.Now)
                .Include("Lecturer")
                .Include("Category")
                .ToList();

            return View(mineCourses);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var course = _dbContext.Courses
                .Single(c => c.Id == id && c.LecturerId == userId);

            var vm = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = course.DateTime.ToString("dd/MM/yyyy"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading = "Edit",
                Id = course.Id
            };

            return View("Create", vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View(viewModel);
            }
            var userId = User.Identity.GetUserId();

            var course = _dbContext.Courses
                .Single(c => c.Id == viewModel.Id && c.LecturerId == userId);

            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}