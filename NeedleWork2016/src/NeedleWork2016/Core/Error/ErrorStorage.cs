using System;
using System.IO;
using System.Xml.Serialization;

namespace NeedleWork2016.Core.Error
{
    //class for serializing error list
    [Serializable]
    public class ErrorStorage
    {
        public static ListOfErrors GetListOfErrors()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(ListOfErrors));
            TextReader textReader = new StreamReader(@"./error/Error.xml");
            object obj = deserializer.Deserialize(textReader);
            textReader.Close();
            return (ListOfErrors)obj;
        }

    }
}
