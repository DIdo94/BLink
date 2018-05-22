using System.Collections.Generic;

namespace BLink.Core.Constants
{
    public static class AppConstants
    {
        public static IDictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();

        public static string BlinkConnectionKey = "BlinkDbConnection";

        public static string ApiUrl = "http://localhost:5000";

        public static string DataFilesPath = "D://Files";

        public static string MainPhotoFormat = "main_photo.png";
    }
}
