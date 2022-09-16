using AMS.Models;
using AMS.Services;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace AMS.Controllers
{
    [Authorize]
    public class AMSController : BaseController
    {
        private readonly IDbOperations dbOperations;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AMSController(IDbOperations dbOperations, IHttpContextAccessor httpContextAccessor)
        {
            this.dbOperations = dbOperations;
            this.httpContextAccessor = httpContextAccessor;
        }

        #region Section
        public IActionResult Section()
        {
            return View();
        }

        public async Task<Section> SaveSection(Section section)
        {
            try
            {
                var result = await dbOperations.SaveData(section, "Section");
                if (result == null)
                {

                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Section>> GetSection()
        {
            try
            {
                var result = await dbOperations.GetAllData<Section>("Section");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Section

        #region Course
        public IActionResult Course()
        {
            return View();
        }

        public async Task<Course?> SaveCourse(Course course)
        {
            try
            {
                var result = await dbOperations.SaveData(course, "Course");
                if (result == null)
                {

                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Course>> GetCourse()
        {
            try
            {
                var result = await dbOperations.GetAllData<Course>("Course");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion Course

        #region Faculty
        public IActionResult Faculty()
        {
            return View();
        }

        public async Task<Faculty> SaveFaculty(Faculty faculty)
        {
            try
            {

                var result = await dbOperations.SaveData(faculty, "Faculty");
                if (result == null)
                {

                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Faculty>> GetFaculty()
        {
            try
            {
                var result = await dbOperations.GetAllData<Faculty>("Faculty");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Faculty

        #region Student
        public IActionResult Student()
        {
            return View();
        }

        public async Task<IActionResult> SaveStudent(Student student)
        {
            try
            {
                var result = await dbOperations.SaveData(student, "Student");
                if (result == null)
                {

                }
                return View("Student");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudent()
        {
            try
            {
                var result = await dbOperations.GetAllData<Student>("Student");
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Student

        #region FacultyRegistration
        public IActionResult FacultyRegistration()
        {
            return View();
        }

        public async Task<IActionResult> GetFacultyRegistration()
        {
            try
            {
                var result = await dbOperations.GetAllData<Course_Section_Faculty>("Course_Section_Faculty");
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> RegisterFacultyWithCourse(Course_Section_Faculty data)
        {
            try
            {
                var result = await dbOperations.SaveData(data, "Course_Section_Faculty");
                if (result == null)
                {

                }
                return RedirectToAction("GetFacultyRegistration");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion FacultyRegistration

        #region StudentCourseRegistration
        public IActionResult StudentRegistration()
        {
            return View();
        }

        public async Task<IActionResult> GetStudentCourseRegistration()
        {
            try
            {
                var result = await dbOperations.GetAllData<Student_Course_Registration>("Course_Section_Faculty");
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> RegisterStudentWithCourse(Student_Course_Registration data)
        {
            try
            {
                var result = await dbOperations.SaveData(data, "Student_Course_Registration");
                if (result == null)
                {

                }
                return RedirectToAction("GetStudentCourseRegistration");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion StudentCourseRegistration

        #region Attendance
        public async Task<Students_Attendance> ShowAttendance()
        {
            return new Students_Attendance();
        }

        public async Task<Students_Attendance> MarkAttendance(string uid)
        {
            return new Students_Attendance();
        }

        public async Task<Students_Attendance> UpdateAttendance()
        {
            return new Students_Attendance();
        }
        #endregion Attendance

        #region QRCode
        [HttpGet]
        public IActionResult CreateQRCode()
        {
            var queryParameter = Guid.NewGuid().ToString("N");
            var url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/AMS/MarkAttendance?uid=" + queryParameter;
            string WebUri = new Uri(url).ToString();
            string UriPayload = WebUri.ToString();
            QRCodeGenerator QrGenerator = new();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(UriPayload, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] BitmapArray = QrBitmap.BitmapToByteArray();
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            ViewBag.QrCodeUri = QrUri;
            return View();
        }
        #endregion QRCode

        public IActionResult MyProfile()
        {
            return View();
        }

    }
}
