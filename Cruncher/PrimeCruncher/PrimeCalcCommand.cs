using System;
using System.Collections.Generic;
using System.Threading;
using AsyncWorkerProj1;
using Microsoft.Practices.Prism.ViewModel;

namespace CruncherModule.PrimeCruncher
{
    class PrimeAsyncStatus : IAsyncStatus
    {
        public PrimeAsyncStatus(bool isDone, int progressNumber)
        {
            m_IsDone = isDone;
            m_ProgressNumber = progressNumber;
        }

        private readonly bool m_IsDone;
        public bool IsDone()
        {
            return m_IsDone;
        }

        private readonly int m_ProgressNumber;
        public int ProgressNumber()
        {
            return m_ProgressNumber;
        }
    }

    /// <summary>
    /// Calcluates all primary numbers up to a given max value.
    /// </summary>
    public class PrimeCalcCommand : NotificationObject
    {
        public long Ticket { get; set; }
        public bool IsDone
        {
            get { return m_IsDone; }
            private set
            {
                if (value == m_IsDone) return;
                m_IsDone = value;
                RaisePropertyChanged(() => IsDone);
            }
        }

        public int PercentDone
        {
            get
            {
                return m_MaxValue <= 0 ? 
                    0 : 
                    Math.Min(100, 100 * m_Candidate / m_MaxValue);
            }
        }

        public int MaxValue { get { return m_MaxValue; } }

        private readonly int m_MaxValue;

        private readonly List<int> m_Primes;
        private int m_Candidate;
        private bool m_IsDone;

        public PrimeCalcCommand(int maxValue)
        {
            IsDone = false;
            m_MaxValue = maxValue;
            m_Primes = new List<int>();
        }

        public void OnBegin(object dummy)
        {
            if (m_MaxValue < 2) return;
            m_Primes.Add(2);
            if (m_MaxValue < 3) return;

            m_Candidate = 3;
            for (m_Candidate = 3; m_Candidate <= m_MaxValue; m_Candidate++)
            {
                var isDivisible = false;
                for (int divisor = 2; divisor <= m_Candidate / 2; divisor++)
                {
                    double dividend = m_Candidate /  (double)divisor;
                    if (Math.Floor(dividend).Equals(dividend))
                    {
                        isDivisible = true;
                        break;
                    }
                    //Thread.Sleep(100);
                }
                
                if (! isDivisible)
                {
                    m_Primes.Add(m_Candidate);
                }
                RaisePropertyChanged(() => PercentDone);
            }

            IsDone = true;
        }
        
        public IAsyncStatus OnQueryStatus(long ticket)
        {
            return new PrimeAsyncStatus(IsDone, m_Candidate);
        }
    }
}
