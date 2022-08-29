using FireSharp.Interfaces;

namespace AMS.ViewModels
{
    public class AMSFirebaseConfig : IFirebaseConfig
    {
        public string BasePath { get; set; }
        public string Host { get; set; }
        public string AuthSecret { get; set; }
        public TimeSpan? RequestTimeout { get; set; }
        public ISerializer Serializer { get; set; }
    }
}
