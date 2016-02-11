using System;
using System.Xml.Serialization;
using System.Web.Script.Serialization;

namespace NeedleWork2016.Core.Error
{
    //Initialization error data
    [Serializable]
    public class Error
    {
        private int hResult;
        private string source;
        private string message;

        [XmlAttribute("Status")]
        public string Stutus { get; set; }
        [XmlAttribute("Code")]
        public int Code { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Definition")]
        public string Definition { get; set; }


        public Error(string _status,int _code, string _name, string _definition)
        {
            this.Stutus = _status;
            this.Code = _code;
            this.Name = _name;
            this.Definition = _definition;
        }

        public Error() { }

        public Error(int hResult, string source, string message)
        {
            this.hResult = hResult;
            this.source = source;
            this.message = message;
        }

        //serialization data to JSON
        public string ToJson()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(this);
        }
    }
}
