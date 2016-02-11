using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NeedleWork2016.Core
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Result
    {
        Success,
        Error,
        Exeption
    }

    public class ManipulationResult
    {

        private Result _result { get;  set; }
        //hide for null
        private string _errorMessage { get;  set; }
        //hide for null
        private int _exeptionNumber { get;  set; }
        //hide for null
        private string _exeptionMessage { get; set; }

        public Result Status
        {
            get
            {
                return _result;
            }
            set
            {
                value = _result;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                value = _errorMessage;
            }
        }

        public int ExeptionNumber
        {
            get
            {
                return _exeptionNumber;
            }
            set
            {
                value = _exeptionNumber;
            }
        }

        public string ExeptionMessage
        {
            get
            {
                return _exeptionMessage;
            }
            set
            {
                value = _exeptionMessage;
            }
        }

        public ManipulationResult(Result result)
        {
            _result = result;
        }

        public ManipulationResult(Result result, string error) : this(result)
        {
            _errorMessage = error;
        }

        public ManipulationResult (Result result, Exception ex) : this(result)
        {
            _exeptionMessage = ex.Message;
            _exeptionNumber = ex.HResult;
        }
}
}
