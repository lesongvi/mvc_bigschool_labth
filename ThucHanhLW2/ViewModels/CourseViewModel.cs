using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThucHanhLW2.Models;

namespace ThucHanhLW2.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public IEnumerable<Course> UpComingCourses { get; set; }
        public bool ShowAction { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        [FutureDate]
        public string Date { get; set; }
        [Required]
        [ValidTime]
        public string Time { get; set; }
        [Required]
        public byte Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ApplicationUser> Followings { get; set; }
        public string Heading { get; set; }
        public string Action
        {
            get
            {
                return (Id != 0) ? "Update" : "Create";
            }
        }
        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}