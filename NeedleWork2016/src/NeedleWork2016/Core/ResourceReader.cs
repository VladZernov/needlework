using System.IO;
using System.Web.Script.Serialization;

namespace NeedleWork2016.Core
{
    //Serialize resources
    public class ResourceReader
    {
        public static string Lang { get; set; }

        private static string ReadJson(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }

        public static object ParseJson<T>(string path)
        {           
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(ReadJson(path + "." + Lang + ".json"));
        }

        public static string GetTemplate(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }

    }
}
