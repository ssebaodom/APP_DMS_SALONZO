using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowService_POC.Contract
{
    public interface ITaskLogTime
    {
        Task DoWork(CancellationToken cancellationToken);
        Task Execute();
    }
}
