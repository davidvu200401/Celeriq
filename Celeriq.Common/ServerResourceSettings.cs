using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Celeriq.Common
{
    [Serializable]
    public class ServerResourceSettings
    {
        private int _autoDataUnloadTime = 0;
        private int _maxRunningRepositories = 0;
        private long _maxMemory = 0;

        [XmlElement]
        [DataMember]
        public int MaxRunningRepositories
        {
            get { return _maxRunningRepositories; }
            set
            {
                _maxRunningRepositories = value;
                if (_maxRunningRepositories < 0) _maxRunningRepositories = 0;
            }
        }

        [XmlElement]
        [DataMember]
        public long MaxMemory
        {
            get { return _maxMemory; }
            set
            {
                _maxMemory = value;
                if (_maxMemory < 0) _maxMemory = 0;
            }
        }

        [XmlElement]
        [DataMember]
        public int AutoDataUnloadTime
        {
            get { return _autoDataUnloadTime; }
            set
            {
                _autoDataUnloadTime = value;
                if (_autoDataUnloadTime < 0) _autoDataUnloadTime = 0;
            }
        }

    }
}