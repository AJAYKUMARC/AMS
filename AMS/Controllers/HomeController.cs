﻿using AMS.Models;
using AMS.Services;
using AMS.ViewModels;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AMS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDbOperations dbOperations;
        public HomeController(IDbOperations dbOperations)
        {
            this.dbOperations = dbOperations;
        }

        /// <summary>
        /// This page is shown after login of a faculty or student.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        /// <summary>
        /// Shows Registration page for the Student
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Process the Registration of a Student
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser(Models.User userModel)
        {
            try
            {
                var studentList = await dbOperations.GetAllData<Student>("Student");
                if (studentList != null && studentList.Any(x => x.Email?.ToLower() == userModel.Email.ToLower()))
                {
                    //User Already Exist
                }
                //create the user
                var regResult = await AuthProvider.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                //log in the new user

                var student = new Student
                {
                    Email = userModel.Email,
                    Name = userModel.UserName
                };

                var result = await dbOperations.SaveData(student, "Student");

                return RedirectToAction("SignIn");
            }

            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(userModel);
            }

        }

        /// <summary>
        /// Shows login page for faculty
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterFaculty()
        {
            return View();
        }

        /// <summary>
        /// Process the Registration of a faculty
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterFaculty(Models.User userModel)
        {
            try
            {
                var facultyList = await dbOperations.GetAllData<Faculty>("Faculty");
                if (facultyList != null && facultyList.Any(x => x.Email?.ToLower() == userModel.Email.ToLower()))
                {
                    //User Already Exist
                }
                //create the user
                var regResult = await AuthProvider.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                //log in the new user

                var faculty = new Faculty
                {
                    Email = userModel.Email,
                    Name = userModel.UserName
                };

                var result = await dbOperations.SaveData(faculty, "Faculty");

                return RedirectToAction("SignIn");

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(userModel);
            }

        }

        /// <summary>
        /// Shows Login page for a student
        /// </summary>
        /// <returns></returns>
        public IActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Process the Login of a studnet
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignInUser(Models.User userModel)
        {
            try
            {
                //log in the user
                var fbAuthLink = await AuthProvider
                                .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //saving the token in a session variable
                if (token != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userModel.Email),
                        new Claim("_UserToken", token),
                        new Claim(ClaimTypes.Role, "Student"),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(25),
                        IssuedUtc = DateTime.UtcNow,
                        RedirectUri = Url.Action("Index", "Home")
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    var studentList = await dbOperations.GetAllData<Student>("Student");
                    var userName = string.Empty;
                    if (studentList != null && studentList.Any(x => x.Email?.ToLower() == userModel.Email.ToLower()))
                    {
                        userName = studentList.FirstOrDefault(x => x.Email?.ToLower() == userModel.Email.ToLower()).Name;
                    }
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("UserName", userName ?? "");
                    HttpContext.Session.SetString("UserType", "Studnet");
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("SignIn");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(userModel);
            }

        }

        /// <summary>
        /// Shows the login page of a faculty
        /// </summary>
        /// <returns></returns>
        public IActionResult SignInFaculty()
        {
            return View();
        }

        /// <summary>
        /// Process the Login of a faculty
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignInFaculty(Models.User userModel)
        {
            try
            {
                //log in the user
                var fbAuthLink = await AuthProvider
                                .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //saving the token in a session variable
                if (token != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userModel.Email),
                        new Claim("_UserToken", token),
                        new Claim(ClaimTypes.Role, "Faculty"),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(25),
                        IsPersistent = true,
                        IssuedUtc = DateTime.UtcNow,
                        RedirectUri = Url.Action("Index", "Home")
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    var facultyList = await dbOperations.GetAllData<Faculty>("Faculty");
                    var userName = string.Empty;
                    if (facultyList != null && facultyList.Any(x => x.Email?.ToLower() == userModel.Email.ToLower()))
                    {
                        userName = facultyList.FirstOrDefault(x => x.Email?.ToLower() == userModel.Email.ToLower()).Name;
                    }
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("UserName", userName);
                    HttpContext.Session.SetString("UserType", "Faculty");
                    return RedirectToAction("Index");
                }
                else
                {
                    //Need to show Fail Message
                    return RedirectToAction("SignIn");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                //Need to show Fail Message
                return RedirectToAction("SignIn");
            }

        }

        /// <summary>
        /// Process the logout of a user (student/faculty)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("_UserToken");

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }
    }
}
