using System.Web.Http;
using ThucHanhLW2.DTOs;
using ThucHanhLW2.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Data.Entity;
using ThucHanhLW2.ViewModels;
using System.Collections.Generic;

namespace ThucHanhLW2.Controllers.Api
{
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext _dbContext;

        public FollowingsController ()
        {
            _dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow (FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();
            if (followingDto.FolloweeId == userId)
                return BadRequest("You can't follow yourself");
            else if (_dbContext.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId))
                return BadRequest("Following already exists!");
            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = followingDto.FolloweeId
            };

            _dbContext.Followings.Add(following);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IEnumerable<Following> ListFollow()
        {
            var userId = User.Identity.GetUserId();

            return _dbContext.Followings.Where(f => f.FollowerId == userId).ToList();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _dbContext.Followings.Single(c => c.FolloweeId == id && c.FollowerId == userId);

            _dbContext.Followings.Remove(following);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
