using AMS.Models;
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

        public IActionResult Register()
        {
            return View();
        }

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

        public IActionResult RegisterFaculty()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFaculty(Models.User userModel)
        {
            try
            {
                //create the user
                await AuthProvider.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                //log in the new user
                var fbAuthLink = await AuthProvider
                                .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(userModel);
            }

        }

        public IActionResult SignIn()
        {
            return View();
        }

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
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(25),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        IssuedUtc = DateTime.UtcNow,
                        // The time at which the authentication ticket was issued.

                        RedirectUri = Url.Action("Index", "Home")
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    HttpContext.Session.SetString("_UserToken", token);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(userModel);
            }

        }

        public IActionResult SignInFaculty()
        {
            return View();
        }
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
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(25),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        IssuedUtc = DateTime.UtcNow,
                        // The time at which the authentication ticket was issued.

                        RedirectUri = Url.Action("Index", "Home")
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    HttpContext.Session.SetString("_UserToken", token);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(userModel);
            }

        }

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
