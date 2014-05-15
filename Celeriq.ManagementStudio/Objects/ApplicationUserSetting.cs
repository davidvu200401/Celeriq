#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Celeriq.ManagementStudio.Objects
{
    [Serializable]
    public class ApplicationUserSetting
    {
        public ApplicationUserSetting()
        {
            this.WindowState = FormWindowState.Normal;
        }

        [XmlElement]
        [DataMember]
        public System.Drawing.Size WindowSize;

        [XmlElement]
        [DataMember]
        public FormWindowState WindowState;

        [XmlElement]
        [DataMember]
        public System.Drawing.Point WindowLocation;

        public void Load()
        {
            var path = (new FileInfo(Application.ExecutablePath)).Directory.FullName;
            var fileName = Path.Combine(path, "usersettings.xml");
            if (File.Exists(fileName))
            {
                var q = Celeriq.Common.Extensions.FromXml(File.ReadAllText(fileName), typeof (ApplicationUserSetting)) as ApplicationUserSetting;
                if (q != null)
                {
                    this.WindowSize = q.WindowSize;
                    this.WindowState = q.WindowState;
                    this.WindowLocation = q.WindowLocation;
                }
            }
        }

        public void Save()
        {
            try
            {
                var path = (new FileInfo(Application.ExecutablePath)).Directory.FullName;
                var fileName = Path.Combine(path, "usersettings.xml");
                var xml = Celeriq.Common.Extensions.ToXml(this);
                File.WriteAllText(fileName, xml);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}