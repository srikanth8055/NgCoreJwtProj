using System;
using System.IO;
using System.Text;

namespace LoggerFactory
{
    public class Log
    {
         public Log()
        {
        }
        //private static readonly Lazy<Log> instance = new Lazy<Log>(() => new Log());

        //public static Log GetInstance
        //{
        //    get
        //    {
        //        return instance.Value;
        //    }
        //}

        public static void LogException(string message)
        {
            string fileName = string.Format("{0}_{1}.log", "Exception", DateTime.Now.ToShortDateString());
            string logFilePath = string.Format($"{AppDomain.CurrentDomain.BaseDirectory}/Logs/{fileName}");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.Write(sb.ToString());
                writer.Flush();
            }
        }
    }
}
