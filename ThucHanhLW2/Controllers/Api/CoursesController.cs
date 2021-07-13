using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThucHanhLW2.Models;

namespace ThucHanhLW2.Controllers.Api
{
    public class CoursesController : ApiController
    {
        public ApplicationDbContext _dbContext { get; set; }
        public CoursesController ()
        {
            _dbContext = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel (int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.SingleOrDefault(c => c.Id == id && c.LecturerId == userId);

            if (course == null)
                return BadRequest("Bạn không phải giảng viên của khóa học này");
            else if (course.IsCanceled)
                return BadRequest("Khóa học đã bị hủy trước đó");

            course.IsCanceled = true;
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
