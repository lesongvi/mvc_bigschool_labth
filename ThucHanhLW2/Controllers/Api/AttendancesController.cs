using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using ThucHanhLW2.Models;
using ThucHanhLW2.DTOs;

namespace ThucHanhLW2.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _dbContext;
        public AttendancesController ()
        {
            _dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var userId = User.Identity.GetUserId();

            if (_dbContext.Courses.Any(a => a.LecturerId == userId && a.Id == attendanceDto.CourseId))
                return BadRequest("It's your course!");
            else if (_dbContext.Attendances.Any(a => a.AttendeeId == userId && a.CourseId == attendanceDto.CourseId))
                return BadRequest("The Attendance already exists!");

            var attendance = new Attendance
            {
                CourseId = attendanceDto.CourseId,
                AttendeeId = userId
            };

            _dbContext.Attendances.Add(attendance);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IEnumerable<Attendance> AttendingList()
        {
            var userId = User.Identity.GetUserId();

            return _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .ToList();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var attend = _dbContext.Attendances.Single(c => c.CourseId == id && c.AttendeeId == userId);

            _dbContext.Attendances.Remove(attend);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
