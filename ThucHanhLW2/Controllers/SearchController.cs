using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ThucHanhLW2.Models;
using ThucHanhLW2.ViewModels;

namespace ThucHanhLW2.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string query, string Category, string startDate)
        {
            var courses = db.Courses
                .Include(c => c.Category)
                .Include(c => c.Lecturer);
            if (!String.IsNullOrEmpty(query))
                courses = courses
                    .Where(c => c.Lecturer.Name.ToLower()
                        .Contains(query.ToLower())
                    );
            if (!String.IsNullOrEmpty(Category))
                courses = courses
                    .Where(c => c.CategoryId.ToString() == Category);
            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime dTime;
                DateTime.TryParseExact(startDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dTime);
                courses = courses
                    .Where(c => c.DateTime >= dTime);
            }

            var vm = new CourseViewModel
            {
                UpComingCourses = courses,
                Categories = db.Categories.ToList(),
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
