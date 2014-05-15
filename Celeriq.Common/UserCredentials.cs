using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Celeriq.Common
{
    [Serializable]
    [DataContract()]
    public class UserCredentials : ICloneable
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }

        public override string ToString()
        {
            return this.UserName;
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            var retval = new UserCredentials();
            retval.Password = this.Password;
            retval.UserName = this.UserName;
            return retval;
        }

        #endregion
    }
}