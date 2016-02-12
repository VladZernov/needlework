using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using System.IO;
using NeedleWork2016.Core;

namespace NeedleWork2016.Controllers
{
    public class LocalizationController : Controller
    {
        public object JsonRequestBehavior { get; private set; }

        [HttpPost]
        public string GetLocalizationData(string lang)
        {
            try
            {
                string path = "..//Resources//Home//Index." + lang + ".json";
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                r.Close();
                return json;
            }
            catch (Exception ex)
            {
                return new Core.Error.Error(ex.HResult, ex.Source, ex.Message).ToJson();
            }
        }

        //сюда приежает объект
        public void Update(string name, string json)
        {
            ResourceReader.WriteJson(json, "..//Resources//Home//Index.en.json");
            //return "";
        }
    }
}
