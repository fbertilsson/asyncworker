using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AsyncWorkerProj1;
using NUnit.Framework;

namespace AsyncWorkerProj1Test
{
    [TestFixture]
    public class TestAsyncWorker
    {
        private AsyncWorker m_Worker;

        [SetUp]
        public void SetUp()
        {
            m_Worker = new AsyncWorker();
        }

        [TearDown]
        public void TearDown()
        {
            m_Worker = null;
        }

        [Ignore]
        [Test]
        public void Run_WhenFinished_FinishedCalllbackIsCalled()
        {
            int runDummy = 0;
            int whileDummy = 0;
            int finishedCounter = 0;
            m_Worker.Run(x => runDummy++, x => whileDummy++, x => finishedCounter++);
            //Thread.Sleep(1000);
            Assert.AreEqual(1, finishedCounter);
        }

        [Test]
        public void Cancel_WhenQueueEmpty_ReturnsFalse()
        {
            bool isCanceled = m_Worker.Cancel(1);

            Assert.IsFalse(isCanceled);
        }

        [Test]
        public void Cancel_WhenTicketNotInQueue_ReturnsFalse()
        {
            int runDummy = 0;
            int whileDummy = 0;
            int finishedCounter = 0;
            long ticket = m_Worker.Run(x => runDummy++, x => whileDummy++, x => finishedCounter++);
            bool isCanceled = m_Worker.Cancel(ticket - 1);

            Assert.IsFalse(isCanceled);
        }

        [Test]
        public void Cancel_WhenInQueue_ReturnsTrue()
        {
            int whileDummy = 0;
            int finishedCounter = 0;
            long ticket = m_Worker.Run(x => Thread.Sleep(500), x => whileDummy++, x => finishedCounter++);
            bool isCanceled = m_Worker.Cancel(ticket);

            Assert.IsTrue(isCanceled);
        }
    }
}
