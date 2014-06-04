using System;
using System.Collections.Generic;
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
                throw new Exception("Could not get reader lock: " +
                    "LockID=" + m_Lock.LockID +
                    ((m_Lock.ObjectId == Guid.Empty) ? string.Empty : ", ObjectID=" + m_Lock.ObjectId) +
                    ", CurrentReadCount=" + m_Lock.CurrentReadCount +
                    ", WaitingReadCount=" + m_Lock.WaitingReadCount + 
                    ", WaitingWriteCount=" + m_Lock.WaitingWriteCount +
                    ", IsWriteLockHeld=" + m_Lock.IsWriteLockHeld +
                    ", HoldingThread=" + m_Lock.HoldingThreadId +
                    ", TraceInfo=" + string.Join("|", m_Lock.TraceInfo.ToList()) +
                    ", WriteHeldTime=" + m_Lock.WriteHeldTime);
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
                    lock (m_Lock)
                    {
                        m_Lock.ExitReadLock();
                    }
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
            : this(rwl, traceInfo, Guid.Empty)
        {
        }

        /// <summary />
        public AcquireWriterLock(CeleriqLock rwl, string traceInfo, Guid callerObject)
        {
            m_Lock = rwl;
            if (!m_Lock.TryEnterWriteLock(TimeOut))
            {
                _inError = true;

                throw new Exception("Could not get writer lock: " +
                    "LockID=" + m_Lock.LockID +
                    ((m_Lock.ObjectId == Guid.Empty) ? string.Empty : ", ObjectID=" + m_Lock.ObjectId) +
                    (callerObject == Guid.Empty ? string.Empty : ", CallerID=" + callerObject) +
                    ", CurrentReadCount=" + m_Lock.CurrentReadCount +
                    ", WaitingReadCount=" + m_Lock.WaitingReadCount + 
                    ", WaitingWriteCount=" + m_Lock.WaitingWriteCount +
                    ", IsWriteLockHeld=" + m_Lock.IsWriteLockHeld +
                    ", HoldingThread=" + m_Lock.HoldingThreadId +
                    ", TraceInfo=" + string.Join("|", m_Lock.TraceInfo.ToList()) +
                    ", WriteHeldTime=" + m_Lock.WriteHeldTime);
            }

            lock (m_Lock)
            {
                //Save stack history of trace info
                m_Lock.TraceInfo.Push(traceInfo);
                m_Lock.WriteLockHeldTime = DateTime.Now;
                m_Lock.HoldingThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
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
            try
            {
                if (!m_Disposed && m_Lock != null && disposing)
                {
                    lock (m_Lock)
                    {
                        if (!_inError)
                        {
                            m_Lock.WriteLockHeldTime = null;
                            if (m_Lock.TraceInfo.Count > 0) m_Lock.TraceInfo.Pop();
                            m_Lock.HoldingThreadId = null;
                        }
                        m_Lock.ExitWriteLock();
                    }
                }
                m_Disposed = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }

    public class CeleriqLock : System.Threading.ReaderWriterLockSlim
    {
        public CeleriqLock(LockRecursionPolicy recursionPolicy, Guid objectId)
            : base(recursionPolicy)
        {
            this.LockID = Guid.NewGuid();
            this.ObjectId = objectId;
            this.TraceInfo = new Stack<string>();
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
        public Stack<string> TraceInfo { get; internal set; }
        public Guid LockID { get; private set; }
        public Guid ObjectId { get; private set; }
        public int? HoldingThreadId { get; internal set; }
    }

}