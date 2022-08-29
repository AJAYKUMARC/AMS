using Firebase.Auth;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class BaseController : Controller
    {
        public string APIKey = "AIzaSyA2xZlUvq_97_Z3UGnqUGH0JSvarqE-Y4c";

        public FirebaseConfig Config { get; set; }
        public FirebaseAuthProvider Auth { get; set; }

        public BaseController()
        {
            Auth = new FirebaseAuthProvider(
                                 new FirebaseConfig(APIKey));
            Config = new FirebaseConfig(APIKey);
        }
    }
}
