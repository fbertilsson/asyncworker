using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AsyncWorkerProj1
{
    interface IAsyncWorker
    {
        long Run(WaitCallback beginCallback, WaitCallback whileCallback, WaitCallback finishedCallback);

        bool Cancel(long ticket);
    }
}
