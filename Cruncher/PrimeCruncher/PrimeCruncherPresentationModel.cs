using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using log4net;
using AsyncWorkerProj1;

namespace CruncherModule.PrimeCruncher
{
    /// <summary>
    /// 
    /// </summary>
    public class PrimeCruncherPresentationModel : NotificationObject
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PrimeCruncherPresentationModel));

        public DelegateCommand EnqueueCommand { get; private set; }

        private int m_Number;
        public int Number
        {
            get { return m_Number; }
            set
            {
                m_Number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        public ObservableCollection<PrimeCalcCommand> QueuedItems { get; private set; }
        public ICollectionView QueuedItemsView { get { return CollectionViewSource.GetDefaultView(QueuedItems); } }

        public IPrimeCruncherView View { get; set; }

        private readonly FifoExecutionQueue m_Queue;
        private Dictionary<long, Timer> m_TicketToTimerMap = new Dictionary<long, Timer>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="queue"></param>
        public PrimeCruncherPresentationModel(IPrimeCruncherView view, FifoExecutionQueue queue)
        {
            Log.Debug("Enter");

            View = view;
            View.Model = this;

            m_Queue = queue;

            EnqueueCommand = new DelegateCommand(OnEnqueue);
            QueuedItems = new ObservableCollection<PrimeCalcCommand>();

            m_Number = 123000;

            Log.Debug("Exit");
        }

        private void OnEnqueue()
        {
            var primeCalcCommand = new PrimeCalcCommand(m_Number);
            primeCalcCommand.Ticket = m_Queue.Enqueue(primeCalcCommand.OnBegin, primeCalcCommand.OnQueryStatus, OnFinished, new object());
            QueuedItems.Add(primeCalcCommand);
        }

        private void OnFinished(object ticket)
        {
            var ticketLong = (long) ticket;
            var command = QueuedItems.FirstOrDefault(c => c.Ticket == ticketLong);
            if (command != null)
            {
                m_TicketToTimerMap.Add(ticketLong, new Timer(RemoveCommand, command, 5000, Timeout.Infinite));          
            }
        }

        private void RemoveCommand(object o)
        {
            var primeCalcCommand = (PrimeCalcCommand) o;
            View.Dispatcher.BeginInvoke(new Action(() => QueuedItems.Remove(primeCalcCommand)));
            m_TicketToTimerMap.Remove(primeCalcCommand.Ticket);
        }
    }
}
