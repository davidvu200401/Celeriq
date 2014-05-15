using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Celeriq.Common
{
    [Serializable]
    [DataContract()]
    public class PagingInfo
    {
        [DataMember]
        protected int _pageOffset = 1;
        [DataMember]
        protected int _recordsPerPage = 10;

        public PagingInfo()
        {
            this.PageOffset = 1;
            this.RecordsPerPage = 10;
            this.SortAsc = true;
        }

        [DataMember]
        public virtual string Keyword { get; set; }
        [DataMember]
        public virtual string SortField { get; set; }
        [DataMember]
        public virtual bool SortAsc { get; set; }
        [DataMember]
        public virtual int TotalItemCount { get; set; }
        
        [DataMember]
        public virtual int PageOffset
        {
            get
            {
                if (_pageOffset < 1) return 1;
                return _pageOffset;
            }
            set
            {
                _pageOffset = value;
                if (_pageOffset < 1) _pageOffset = 1;
                //if (_pageOffset > this.PageCount) _pageOffset = this.PageCount;
            }
        }

        [DataMember]
        public virtual int RecordsPerPage
        {
            get { return _recordsPerPage; }
            set
            {
                _recordsPerPage = value;
                if (_recordsPerPage < 1) _recordsPerPage = 1;
            }
        }

        public int PageCount
        {
            get
            {
                if (this.TotalItemCount == 0) return 0;
                var retval = (this.TotalItemCount / this.RecordsPerPage);
                if (this.TotalItemCount % this.RecordsPerPage != 0) retval++;
                return retval;
            }
        }

        public int StartItemIndex
        {
            get { return ((this.PageOffset - 1) * this.RecordsPerPage); }
        }

    }
}