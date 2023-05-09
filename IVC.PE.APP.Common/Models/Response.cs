using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Common.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Object Result { get; set; }
    }
}
