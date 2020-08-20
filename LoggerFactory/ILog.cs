using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFactory
{
   public interface ILog
    {
       static void LogException(string message);
    }
}
