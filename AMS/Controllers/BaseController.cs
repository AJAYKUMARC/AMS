using AMS.ViewModels;
using Firebase.Auth;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    public class BaseController : Controller
    {
        public string APIKey = "AIzaSyA2xZlUvq_97_Z3UGnqUGH0JSvarqE-Y4c";
        public string APISecret = "zlotpxJesn9ktlahVwq8AEPY5dflLI18gPmI1xeX";
        public string BasePath = "https://attendancemngtsys-default-rtdb.firebaseio.com";

        public IFirebaseClient FireSharpClient { get; set; }
        public FirebaseConfig AuthConfig { get; set; }
        public FirebaseAuthProvider AuthProvider { get; set; }
        public IFirebaseConfig FireSharpConfig { get; set; }
        public BaseController()
        {
            FireSharpConfig = new AMSFirebaseConfig
            {
                AuthSecret = APISecret,
                BasePath = BasePath
            };
            FireSharpClient = new FireSharp.FirebaseClient(FireSharpConfig);
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
            AuthConfig = new FirebaseConfig(APIKey);
        }
    }
}
