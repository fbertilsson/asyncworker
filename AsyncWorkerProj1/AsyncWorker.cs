using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using log4net;

namespace AsyncWorkerProj1
{
    internal class AsyncStatus : IAsyncStatus
    {
        public bool IsDone()
        {
            return true;
        }

        public int ProgressNumber()
        {
            return 0;
        }
    }

    /// <summary>
    /// The internal representation of an item of work for <see cref="FifoExecutionQueue"/>.
    /// </summary>
    internal class InternalWorkItem
    {
        public long Ticket { get; set; }

        /// <summary>
        /// The callback to be called when the item is up for processing.
        /// </summary>
        public WaitCallback BeginCallback { get; set; }

        /// <summary>
        /// The callback to be called while not finished processing.
        /// </summary>
        //public WaitCallback QueryStatusCallback { get; set; }
        public QueryStatusCallbackDelegate QueryStatusCallback { get; set; }

        /// <summary>
        /// The callback to be called when the item is finished processing.
        /// </summary>
        public WaitCallback FinishedCallback { get; set; }

        public object State { get; set; }
        public ExecutionContext Context { get; set; }

        private static readonly ContextCallback BeginContextCallback =
            s =>
            {
                var item = (InternalWorkItem)s;
                item.BeginCallback(item.State);
            };

        private static readonly ContextCallback FinishedContextCallback =
            s =>
            {
                var item = (InternalWorkItem)s;
                item.FinishedCallback(item.State);
            };

        public void Execute()
        {
            if (Context != null)
                ExecutionContext.Run(Context, BeginContextCallback, this);
            else BeginCallback(State);

            // While TODO what???
            //if (Context != null)
            //    ExecutionContext.Run(Context, WhileContextCallback, this);
            //else WhileCallback(State);
            IAsyncStatus status;
            do
            {
                status = QueryStatusCallback(Ticket);
                if (!status.IsDone())
                {
                    Thread.Sleep(200);
                }
            } while (!status.IsDone());

            if (Context != null)
                ExecutionContext.Run(Context, FinishedContextCallback, this);
            else BeginCallback(State);

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AsyncWorker : IAsyncWorker
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long m_Ticket;
        private readonly FifoExecutionQueue m_Queue;
        private readonly Queue<WorkItem> m_WorkItems = new Queue<WorkItem>();

        public AsyncWorker()
        {
            m_Queue = new FifoExecutionQueue();
        }

        private IAsyncStatus Slask(long ticket)
        {
            return new AsyncStatus();
        }

        public long Run(WaitCallback beginCallback, WaitCallback whileCallback, WaitCallback finishedCallback)
        {
            long ticket = m_Queue.Enqueue(beginCallback, /* whileCallback */ Slask, finishedCallback, null);
            return ticket; 
                //Interlocked.Increment(ref m_Ticket);
        }

        public bool Cancel(long ticket)
        {
            return m_WorkItems.Any(workItem => ticket == workItem.Ticket);
        }
    }
}
