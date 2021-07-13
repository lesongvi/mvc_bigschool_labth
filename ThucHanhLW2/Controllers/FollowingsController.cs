using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThucHanhLW2.Models;
using ThucHanhLW2.ViewModels;

namespace ThucHanhLW2.Controllers
{
    public class FollowingsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FollowingsController ()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Followings
        public ActionResult Index()
        {
            var followings = _dbContext.Followings.Include(f => f.Followee).Include(f => f.Follower);
            return View(followings.ToList());
        }

        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();

            var followings = _dbContext.Followings
                .Where(a => a.FollowerId == userId)
                .Select(u => u.Followee)
                .ToList();

            var vm = new CourseViewModel
            {
                Followings = followings,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(vm);
        }

        [Authorize]
        public ActionResult Feed ()
        {
            var userId = User.Identity.GetUserId();

            List<Course> allCourses = new List<Course>();

            var followings = _dbContext.Followings
                .Where(a => a.FollowerId == userId)
                .Select(u => u.FolloweeId)
                .ToList();

            foreach (string id in followings)
            {
                allCourses.AddRange(_dbContext.Courses
                    .Where(c => c.LecturerId == id)
                    .Include("Category")
                    .Include("Lecturer")
                    .ToList());
            }

            return View(allCourses);
        }


        [Authorize]
        public ActionResult Delete(string id)
        {
            var userId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Following following = _dbContext.Followings
                .Include(f => f.Followee)
                .Include(f => f.Follower)
                .Where(f => f.FollowerId == userId)
                .Where(f => f.FolloweeId == id)
                .FirstOrDefault();

            if (following == null) return HttpNotFound();

            return View(following);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _dbContext.Followings.Single(c => c.FolloweeId == id && c.FollowerId == userId);

            _dbContext.Followings.Remove(following);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
