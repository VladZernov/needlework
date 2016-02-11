using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeedleWork2016.Repository;
using NeedleWork2016.Entities;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Mvc;
using System.IO;
using Newtonsoft.Json;

namespace NeedleWork2016.Controllers
{
    public class LocalizationController : Controller 
    {
        public class Page
        {
            public string Title { get; set; }
            public string FirstBlockName { get; set; }
            public string FirstBlockText { get; set; }
            public string SecondBlockName { get; set; }
            public string SecondBlockText { get; set; }
            public string ThirdBlockName { get; set; }
            public string ThirdBlockText { get; set; }
        }

        [HttpGet]
        public string GetLocalizationData()
        {
            string path = "..//Resources//Home//Index.en.json";
            List<Page> myDeserializedObjList = new List<Page>();
            string json;

            using (StreamReader r = new StreamReader(path))
            {
                try
                {
                    json = r.ReadToEnd();
                    myDeserializedObjList = (List<Page>)JsonConvert.DeserializeObject(json, typeof(List<Page>));
                    return json;
                }
                catch (Exception ex)
                { return "error"; }
                finally { }
                
            }
        }
    }
}
