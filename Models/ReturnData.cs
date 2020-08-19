using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class ReturnData:IDisposable
    {
        public Boolean isList { get; set; }
        public bool isError { get; set; }
        public string message { get; set; }
        public object data { get; set; }

        public void Dispose()
        {
        }
    }


}
