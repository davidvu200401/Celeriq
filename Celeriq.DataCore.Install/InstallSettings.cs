//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Celeriq.DataCore.Install
{
	/// <summary />
	internal class InstallSettings
	{
		/// <summary />
		public string PrimaryServer { get; set; }

		/// <summary />
		public string PrimaryDatabase { get; set; }

		/// <summary />
		public string PrimaryUserName { get; set; }

		/// <summary />
		public string PrimaryPassword { get; set; }

		/// <summary />
		public bool PrimaryUseIntegratedSecurity { get; set; }

		/// <summary />
		public string CloudServer { get; set; }

		/// <summary />
		public string CloudDatabase { get; set; }

		/// <summary />
		public string CloudUserName { get; set; }

		/// <summary />
		public string CloudPassword { get; set; }

		/// <summary />
		public InstallSettings()
		{
			this.IsLoaded = false;
		}

		/// <summary />
		public bool IsLoaded { get; private set; }

		/// <summary />
		public bool Load()
		{
			var fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			fi = new FileInfo(Path.Combine(fi.DirectoryName, "installsettings.xml"));
			if (!fi.Exists) return false;

			var document = new XmlDocument();
			document.Load(fi.FullName);

			if (document.DocumentElement.Name == "a")
			{
				this.PrimaryServer = XmlHelper.GetNodeValue(document.DocumentElement, "server", string.Empty);
				this.PrimaryUseIntegratedSecurity = XmlHelper.GetNodeValue(document.DocumentElement, "useintegratedsecurity", false);
				this.PrimaryUserName = XmlHelper.GetNodeValue(document.DocumentElement, "username", string.Empty);
				this.PrimaryPassword = XmlHelper.GetNodeValue(document.DocumentElement, "password", string.Empty);

				var v = XmlHelper.GetNodeValue(document.DocumentElement, "username-encrypted", string.Empty).Decrypt();
				if (!string.IsNullOrEmpty(v))
					this.PrimaryUserName = v;

				v = XmlHelper.GetNodeValue(document.DocumentElement, "password-encrypted", string.Empty).Decrypt();
				if (!string.IsNullOrEmpty(v))
					this.PrimaryPassword = v;

				this.PrimaryDatabase = XmlHelper.GetNodeValue(document.DocumentElement, "database", string.Empty);
			}
			else
			{
				var node = document.DocumentElement.SelectSingleNode("primary");
				this.PrimaryServer = XmlHelper.GetNodeValue(node, "server", string.Empty);
				this.PrimaryUseIntegratedSecurity = XmlHelper.GetNodeValue(node, "useintegratedsecurity", false);
				this.PrimaryUserName = XmlHelper.GetNodeValue(node, "username", string.Empty);
				this.PrimaryPassword = XmlHelper.GetNodeValue(node, "password", string.Empty);

				var v = XmlHelper.GetNodeValue(node, "username-encrypted", string.Empty).Decrypt();
				if (!string.IsNullOrEmpty(v))
					this.PrimaryUserName = v;

				v = XmlHelper.GetNodeValue(node, "password-encrypted", string.Empty).Decrypt();
				if (!string.IsNullOrEmpty(v))
					this.PrimaryPassword = v;

				this.PrimaryDatabase = XmlHelper.GetNodeValue(node, "database", string.Empty);

				node = document.DocumentElement.SelectSingleNode("cloud");
				this.CloudServer = XmlHelper.GetNodeValue(node, "server", string.Empty);
				this.CloudUserName = XmlHelper.GetNodeValue(node, "username", string.Empty);
				this.CloudPassword = XmlHelper.GetNodeValue(node, "password", string.Empty);

				v = XmlHelper.GetNodeValue(node, "username-encrypted", string.Empty).Decrypt();
				if (!string.IsNullOrEmpty(v))
					this.CloudUserName = v;

				v = XmlHelper.GetNodeValue(node, "password-encrypted", string.Empty).Decrypt();
				if (!string.IsNullOrEmpty(v))
					this.CloudPassword = v;

				this.CloudDatabase = XmlHelper.GetNodeValue(node, "database", string.Empty);
			}

			this.IsLoaded = true;
			return true;

		}

		public void Kill()
		{
			var fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			fi = new FileInfo(Path.Combine(fi.DirectoryName, "installsettings.xml"));
			if (fi.Exists) fi.Delete();
			System.Threading.Thread.Sleep(500);
		}

		/// <summary />
		public bool Save()
		{
			var fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			fi = new FileInfo(Path.Combine(fi.DirectoryName, "installsettings.xml"));
			if (fi.Exists) fi.Delete();
			System.Threading.Thread.Sleep(500);

			var document = new XmlDocument();
			document.LoadXml("<settings></settings>");

			var node = XmlHelper.AddElement(document.DocumentElement, "primary", string.Empty) as XmlElement;
			XmlHelper.AddElement(node, "server", this.PrimaryServer);
			XmlHelper.AddElement(node, "useintegratedsecurity", this.PrimaryUseIntegratedSecurity.ToString().ToLower());
			XmlHelper.AddElement(node, "username-encrypted", (this.PrimaryUserName + string.Empty).Encrypt());
			XmlHelper.AddElement(node, "password-encrypted", (this.PrimaryPassword + string.Empty).Encrypt());
			XmlHelper.AddElement(node, "database", this.PrimaryDatabase);

			node = XmlHelper.AddElement(document.DocumentElement, "cloud", string.Empty) as XmlElement;
			XmlHelper.AddElement(node, "server", this.CloudServer);
			XmlHelper.AddElement(node, "username-encrypted", (this.CloudUserName + string.Empty).Encrypt());
			XmlHelper.AddElement(node, "password-encrypted", (this.CloudPassword + string.Empty).Encrypt());
			XmlHelper.AddElement(node, "database", this.CloudDatabase);

			document.Save(fi.FullName);

			return true;

		}

		/// <summary>
		/// 
		/// </summary>
		public string GetPrimaryConnectionString()
		{
			if (this.PrimaryUseIntegratedSecurity)
			{
				return "server=" + this.PrimaryServer + ";Initial Catalog=" + this.PrimaryDatabase + ";integrated Security=SSPI;Connect Timeout=604800;";
			}
			else
			{
				return "server=" + this.PrimaryServer + ";Initial Catalog=" + this.PrimaryDatabase + ";user id=" + this.PrimaryUserName + ";password=" + this.PrimaryPassword + ";Connect Timeout=604800;";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string GetCloudConnectionString()
		{
			return "server=" + this.CloudServer + ";Initial Catalog=" + this.CloudDatabase + ";user id=" + this.CloudUserName + ";password=" + this.CloudPassword + ";Connect Timeout=604800;";
		}

	}
}
