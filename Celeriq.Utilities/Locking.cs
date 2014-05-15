using System;
using System.Linq;
using System.Threading;

namespace Celeriq.Utilities
{
    /// <summary />
    public class AcquireReaderLock : IDisposable
    {
        private CeleriqLock m_Lock = null;
        private bool m_Disposed = false;
        private const int TimeOut = 10000;

        /// <summary />
        public AcquireReaderLock(CeleriqLock rwl)
        {
            m_Lock = rwl;
            if (!m_Lock.TryEnterReadLock(TimeOut))
            {
                throw new Exception("Could not get reader lock. " +
                    "ThreadID: " + rwl.ID +
                    ((rwl.ObjectId == Guid.Empty) ? string.Empty : ", ObjectID: " + rwl.ObjectId) +
                    ", CurrentReadCount: " + m_Lock.CurrentReadCount +
                    ", WaitingReadCount: " + m_Lock.WaitingReadCount + 
                    ", WaitingWriteCount: " + m_Lock.WaitingWriteCount +
                    ", IsWriteLockHeld: " + m_Lock.IsWriteLockHeld +
                    ", HoldingThread: " + m_Lock.HoldingThreadId +
                    ", TraceInfo: " + m_Lock.TraceInfo +
                    ", WriteHeldTime: " + m_Lock.WriteHeldTime);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary />
        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing && m_Lock != null)
                {
                    m_Lock.ExitReadLock();
                }
            }
            m_Disposed = true;
        }
    }

    /// <summary />
    public class AcquireWriterLock : IDisposable
    {
        private CeleriqLock m_Lock = null;
        private bool m_Disposed = false;
        private const int TimeOut = 10000;
        private bool _inError = false;

        /// <summary />
        public AcquireWriterLock(CeleriqLock rwl, string traceInfo)
        {
            m_Lock = rwl;
            if (!m_Lock.TryEnterWriteLock(TimeOut))
            {
                _inError = true;
                throw new Exception("Could not get writer lock. " +
                    "ThreadID: " + rwl.ID +
                    ((rwl.ObjectId == Guid.Empty) ? string.Empty : ", ObjectID: " + rwl.ObjectId) + 
                    ", CurrentReadCount: " + m_Lock.CurrentReadCount +
                    ", WaitingReadCount: " + m_Lock.WaitingReadCount + 
                    ", WaitingWriteCount: " + m_Lock.WaitingWriteCount +
                    ", IsWriteLockHeld: " + m_Lock.IsWriteLockHeld +
                    ", HoldingThread: " + m_Lock.HoldingThreadId +
                    ", TraceInfo: " + m_Lock.TraceInfo +
                    ", WriteHeldTime: " + m_Lock.WriteHeldTime);
            }
            rwl.TraceInfo = traceInfo;
            rwl.WriteLockHeldTime = DateTime.Now;
            rwl.HoldingThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary />
        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed && disposing)
            {
                if (!_inError)
                {
                    m_Lock.WriteLockHeldTime = null;
                    m_Lock.TraceInfo = null;
                    m_Lock.HoldingThreadId = null;
                }
                m_Lock.ExitWriteLock();
            }
            m_Disposed = true;
        }
    }

    public class CeleriqLock : System.Threading.ReaderWriterLockSlim
    {
        public CeleriqLock(LockRecursionPolicy recursionPolicy, Guid objectId)
            : base(recursionPolicy)
        {
            this.ID = Guid.NewGuid();
            this.ObjectId = objectId;
        }

        public CeleriqLock(LockRecursionPolicy recursionPolicy)
            : this(recursionPolicy, Guid.Empty)
        {
        }

        public bool AnyLocks()
        {
            return (this.CurrentReadCount == 0) && this.IsWriteLockHeld;
        }

        public int WriteHeldTime
        {
            get
            {
                var retval = -1;
                if (this.WriteLockHeldTime != null)
                    retval = (int)DateTime.Now.Subtract(this.WriteLockHeldTime.Value).TotalMilliseconds;
                return retval;
            }
        }

        public DateTime? WriteLockHeldTime { get; internal set; }
        public string TraceInfo { get; internal set; }
        public Guid ID { get; private set; }
        public Guid ObjectId { get; private set; }
        public int? HoldingThreadId { get; internal set; }
    }

}