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
