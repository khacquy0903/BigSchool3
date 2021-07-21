using BigSchool2.Models;
using BigSchool2.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool2.Controllers
{
    public class CoursesController : Controller
    {
        
        private readonly ApplicationDbContext dbContext;
        public CoursesController()
        {
            dbContext = new ApplicationDbContext();
        }

        //public ActionResult Create()
        //{
        //    var viewModel = new CourseViewModel
        //    {
        //        Categories = dbContext.Categories.ToList()
        //    };
        //    return View(viewModel);
        //}

        // GET: Courses
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CoursesViewModel
            {
                Categories = dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult Create(CourseViewModel viewModel)
        //{
           
        //    var course = new Course
        //    {
        //        LecturerId = User.Identity.GetUserId(),
        //        DateTime = viewModel.GetDateTime(),
        //        CategoryId = viewModel.Category,
        //        Place = viewModel.Place
        //    };

        //    dbContext.Courses.Add(course);
        //    dbContext.SaveChanges();

        //    return RedirectToAction("Index", "Home");
        //}


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (CoursesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place
            };

            dbContext.Courses.Add(course);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var courses = dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }


        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = dbContext.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();

            return View(courses);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = dbContext.Courses.Single(c => c.ID == id && c.LecturerId == userId);
            var viewModel = new CoursesViewModel
            {
                Categories = dbContext.Categories.ToList(),
                Date = course.DateTime.ToString("dd/M/yyyy"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading = "Edit Course",
                Id = course.ID
            };
            return View("Create", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CoursesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var course = dbContext.Courses.Single(c => c.ID == viewModel.Id && c.LecturerId == userId);

            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();

            dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}