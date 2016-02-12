using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NeedleWork2016.Core.Error
{
    //class to store error list 
    [Serializable]
    [XmlRoot("ListOfErrors")]
    public class ListOfErrors
    {
        [XmlArray("ErrorList"), XmlArrayItem(typeof(Error), ElementName = "Error")]
        public List<Error> ErrorList { get; set; }

        //Appeal by error code
        public Error this[int i]
        {
            get { return GetErrorByCode(i); }
        }

        //Return error list
        private Error GetErrorByCode(int code)
        {
            for (int i = 0; i < ErrorList.Capacity; i++)
            {
                if (ErrorList[i].Code == code)
                {
                    return ErrorList[i];
                }               
            }
            return GetErrorByCode(1111);
        }        
    }
}
