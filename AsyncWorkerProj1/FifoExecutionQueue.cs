using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using log4net;

namespace AsyncWorkerProj1
{
    #region FifoExecutionQueue

    public delegate IAsyncStatus QueryStatusCallbackDelegate(long ticket);

    /// <summary>
    /// The internal representation of an item of work for <see cref="FifoExecutionQueue"/>.
    /// </summary>
    internal class WorkItem
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
                var item = (WorkItem)s;
                item.BeginCallback(item.State);
            };

        private static readonly ContextCallback FinishedContextCallback =
            s =>
            {
                var item = (WorkItem)s;
                item.FinishedCallback(item.Ticket);
            };

        public void Execute()
        {
            if (Context != null)
                ExecutionContext.Run(Context.CreateCopy(), BeginContextCallback, this);
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
                    Thread.Sleep(200); // TODO Optimize. Consider replacing with a wait to avoid waiting an extra 199 ms.
                }
            } while (!status.IsDone());

            // Commenting out: Can only use the context once in ExecutionContext.Run
            if (Context != null)
                ExecutionContext.Run(Context.CreateCopy(), FinishedContextCallback, this);
            else FinishedCallback(Ticket);
            //FinishedCallback(Ticket);

        }
    }

    /// <summary>
    /// A queue that processes work items in sequence. Work items can be delivered asynchronously as this
    /// class is thread safe.
    /// <remarks>
    /// This is probably a temporary class since more a more standardized implementation of the 
    /// Active Object pattern is available in .NET 4.0.
    /// </remarks>
    /// </summary>
    public class FifoExecutionQueue : IDisposable
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private bool m_DelegateQueuedOrRunning;
        private readonly WorkItem m_EndMarker = new WorkItem();
        private bool m_HasEndMarkerArrived;
        private long m_Ticket = long.MinValue;
        private readonly Queue<WorkItem> m_WorkItems = new Queue<WorkItem>();

        /// <summary>
        /// Enqueues a new item of work for processing in turn asynchronously.
        /// </summary>
        /// <param name="beginCallback">A callback representing the code to be run</param>
        /// <param name="queryStatusCallback">A callback representing the code to be called regularly, updating progress</param>
        /// <param name="finishedCallback">A callback representing the code to be called when the work is finished</param>
        /// <param name="state">An object to be supplied to the callback as an argument when run. TODO: Check ICloneable or document that the object must not mutate</param>
        /// <returns>A ticket for the work item created. TODO not used yet. </returns>
        public long Enqueue(WaitCallback beginCallback,
            QueryStatusCallbackDelegate queryStatusCallback, 
            WaitCallback finishedCallback, 
            object state)
        {
            var ticket = Interlocked.Increment(ref m_Ticket);
            var item = new WorkItem
            {
                BeginCallback = beginCallback,
                QueryStatusCallback = queryStatusCallback,
                FinishedCallback = finishedCallback,
                State = state,
                Context = ExecutionContext.Capture(),
                Ticket = ticket
            };

            lock (m_WorkItems)
            {
                if (m_IsDisposed)
                {
                    Log.Debug("Is disposed - not queueing work item.");
                    return long.MinValue;
                }

                m_WorkItems.Enqueue(item);
                if (!m_DelegateQueuedOrRunning)
                {
                    m_DelegateQueuedOrRunning = true;
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
                }
                return ticket;
            }
        }

        private void ProcessQueuedItems(object ignored)
        {
            while (!m_HasEndMarkerArrived)
            {
                WorkItem item;
                lock (m_WorkItems)
                {
                    if (m_WorkItems.Count == 0)
                    {
                        m_DelegateQueuedOrRunning = false;
                        break;
                    }
                    item = m_WorkItems.Dequeue();
                    if (item == m_EndMarker)
                    {
                        m_HasEndMarkerArrived = true;
                        continue;
                    }
                }

                try
                {
                    item.Execute();
                }
                catch (Exception e)
                {
                    Log.Fatal("Exception while processing work.", e);
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems,
                      null);
                    throw;
                }
            }
        }

        #region IDisposable

        private bool m_IsDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            Log.DebugFormat("Enter, isDisposing: {0}", isDisposing);
            lock (m_WorkItems)
            {
                if (!m_IsDisposed)
                {
                    m_IsDisposed = true;
                    if (isDisposing)
                    {
                        // Cleanup managed objects by calling their Dispose(), make threads exit, unregister events etc:
                        m_WorkItems.Enqueue(m_EndMarker);
                        if (!m_DelegateQueuedOrRunning)
                        {
                            m_DelegateQueuedOrRunning = true;
                            ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
                        }
                    }

                }

                // Cleanup native objects (if any):

            }

            Log.Debug("Exit");
        }

        #endregion IDisposable

    #endregion FifoExecutionQueue}
    }
}
