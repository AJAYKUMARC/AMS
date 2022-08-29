using AMS.Models;
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

        public bool RegisterFacultyWithCourse(Course_Section_Faculty data)
        {
            if (data == null)
            {
                return false;
            }
            return true;
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

        public bool RegisterStudentWithCourse(Student_Course_Registration data)
        {
            if (data == null)
            {
                return false;
            }
            return true;
        }
        #endregion StudentCourseRegistration
    }
}
