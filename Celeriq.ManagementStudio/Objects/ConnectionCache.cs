#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Celeriq.Common;

namespace Celeriq.ManagementStudio.Objects
{
    internal class ConnectionCache
    {
        public ConnectionCache()
        {
            this.Connections = new List<string>();

            if (File.Exists(this.FileName))
            {
                try
                {
                    var document = new XmlDocument();
                    document.Load(this.FileName);
                    foreach (XmlNode node in document.DocumentElement.ChildNodes)
                    {
                        this.Connections.Add(node.InnerText);
                    }

                }
                catch (Exception ex)
                {
                    //Do Nothing - cannot load file
                }
            }
        }

        public List<string> Connections { get; private set; }

        private string FileName
        {
            get { return Path.Combine(System.Windows.Forms.Application.StartupPath, "connection.cache"); }
        }

        public void Save()
        {
            if (File.Exists(this.FileName))
                File.Delete(this.FileName);

            var document = new XmlDocument();
            document.LoadXml("<root></root>");

            foreach (var s in this.Connections.Distinct().Where(x => x != string.Empty))
            {
                XmlHelper.AddElement(document.DocumentElement, "server", s);
            }

            document.Save(this.FileName);
        }

        public static void CreateRepository(string server, string xmlDefinition, UserCredentials credentials)
        {
            var repository = new Celeriq.Common.RepositorySchema();
            repository.LoadXml(xmlDefinition);
            CreateRepository(server, xmlDefinition, "[" + repository.Name + "]", credentials);
        }

        public static void CreateRepository(string server, string xmlDefinition, string newName, UserCredentials credentials)
        {
            var repository = new Celeriq.Common.RepositorySchema();
            repository.LoadXml(xmlDefinition);
            repository.ID = Guid.NewGuid();
            repository.CreatedDate = DateTime.Now;
            repository.Name = newName;
            using (var factory = SystemCoreInteractDomain.GetFactory(server))
            {
                var s = factory.CreateChannel();
                s.SaveRepository(repository, credentials);
            }
        }



    }
}