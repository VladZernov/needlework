using System.IO;
using System.Web.Script.Serialization;
using NeedleWork2016.ViewModels.LocalizationViewModels;

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
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (System.Exception)
            {
                return "";
            }
            
        }

        public static LayoutLocalizationViewModel GetLayoutLocalizationViewModel()
        {
            return (LayoutLocalizationViewModel)ParseJson<LayoutLocalizationViewModel>("../Resources/Layout");
        }

        public static void WriteJson(string json, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(json);
            }

        }
        

    }
}
