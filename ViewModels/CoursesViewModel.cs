using BigSchool2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigSchool2.ViewModels
{
    public class CoursesViewModel
    {
        public  int Id { get; set; }
        [Required (ErrorMessage = "The Place field is required.")]
        public string Place { get; set; }
        [Required]
        [FutureDate (ErrorMessage ="The field Date is invalid")]
        public string Date { get; set; }
        [Required]
        [ValidTime(ErrorMessage ="The field Time is invalid")]
        public string Time { get; set; }
        [Required]
        public byte Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string Heading { get; set; }
        public string Action
        {
            get { return (Id != 0) ? "Update" : "Create"; }
        }
        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }

        public IEnumerable<Course> UpcommingCourses { get; set; }
        public bool ShowAction { get; set; }
    }
}