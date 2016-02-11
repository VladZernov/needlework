using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;


namespace NeedleWork2016.Core
{
    public class PDFdata
    {
       /// <summary>
       /// This method convers base64 string to byte array
       /// </summary>
       /// <param name="imageData"></param>
       /// <returns></returns>
        public byte[] StringToCode(string imageData)
        {
            byte[] data = Convert.FromBase64String(imageData);

            return data;
        }
        
    }
}
