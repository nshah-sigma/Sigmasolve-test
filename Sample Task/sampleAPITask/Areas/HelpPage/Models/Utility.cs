using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sampleAPITask.Areas.HelpPage.Models
{
    public class Utility
    {
        public class JsonResponse
        {
            public JsonResponse()
            {
                HttpStatusCode = 0;
                Message = "";
                data = new object();
            }
            public int HttpStatusCode { get; set; }
            public string Message { get; set; }
            public object data { get; set; }
        }
    }
}