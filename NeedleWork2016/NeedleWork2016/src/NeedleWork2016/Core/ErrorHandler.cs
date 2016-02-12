using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.Core
{
    public class ErrorHandler
    {
        public static string HandleException(Exception ex)
        {
            string msg = "";
            if (ex.HResult == -2146233088)
                msg = "Same element is already exist in database";
            return msg;
        }
    }
}
