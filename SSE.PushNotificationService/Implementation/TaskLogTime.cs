using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowService_POC.Contract;

namespace WindowService_POC.Implementation
{
    public class TaskLogTime : ITaskLogTime
    {
        public async Task DoWork(CancellationToken cancellationToken)
        {
            await Execute();
        }

        public async Task Execute()
        {
            try
            {
                var path = System.IO.Directory.GetCurrentDirectory() + "\\Logs\\PushNotification";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var logfile = path + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                using (StreamWriter _testData = new StreamWriter(logfile, true))
                {
                    _testData.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ": Service is recall"); // Write the file.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
