using AMS.Models;
using Firebase.Auth;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class AMSController : BaseController
    {
        public AMSController()
        {

        }

        #region Section
        public IActionResult Section()
        {
            return View();
        }

        public IActionResult SaveSection(Section section)
        {
            return View();
        }

        public IList<Section> GetSection()
        {
            return new List<Section>();
        }
        #endregion Section

        #region Course
        public IActionResult Course()
        {
            return View();
        }

        public IActionResult SaveCourse(Course course)
        {
            try
            {
                var data = course;
                PushResponse response = FireSharpClient.Push("Courses/", course);
                course.Id = response.Result.name;
                SetResponse setResponse = FireSharpClient.Set("Courses/" + course.Id, course);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Added Succesfully");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }

        public IList<Course> GetCourse()
        {
            return new List<Course>();
        }
        #endregion Course

        #region Faculty
        public IActionResult Faculty()
        {
            return View();
        }

        public IActionResult SaveFaculty(Faculty faculty)
        {
            return View();
        }

        public IList<Faculty> GetFaculty()
        {
            return new List<Faculty>();
        }
        #endregion Faculty

        #region Student
        public IActionResult Student()
        {
            return View();
        }

        public IActionResult SaveStudent(Student student)
        {
            return View();
        }

        public IList<Student> GetStudent()
        {
            return new List<Student>();
        }
        #endregion Student

        #region FacultyRegistration
        public IActionResult FacultyRegistration()
        {
            return View();
        }

        public List<Course_Section_Faculty> GetFacultyRegistration()
        {
            return new List<Course_Section_Faculty>();
        }

        public IActionResult RegisterFacultyWithCourse(Course_Section_Faculty data)
        {
            if (data == null)
            {
                return View();
            }
            return View();
        }
        #endregion FacultyRegistration

        #region StudentCourseRegistration

        public IActionResult StudentRegistration()
        {
            return View();
        }

        public List<Course_Section_Faculty> GetStudentCourseRegistration()
        {
            return new List<Course_Section_Faculty>();
        }

        public IActionResult RegisterStudentWithCourse(Student_Course_Registration data)
        {
            if (data == null)
            {
                return View();
            }
            return View();
        }
        #endregion StudentCourseRegistration
    }
}
