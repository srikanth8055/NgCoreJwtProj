using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFactory
{
   public interface ILog
    {
        void LogException(string message);
    }
}
