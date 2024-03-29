//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#region Copyright (c) 2006-2013 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2013 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Celeriq.DataCore.Install
{
	/// <summary>
	/// The database installer class
	/// </summary>
	[RunInstaller(true)]
	public partial class DatabaseInstaller : Installer
	{
		#region Members
		private string[] PARAMKEYS_UPGRADE = new string[] { "upgrade" };
		private string[] PARAMKEYS_CREATE = new string[] { "create" };
		private string[] PARAMKEYS_MASTERDB = new string[] { "master", "masterdb" };
		private string[] PARAMKEYS_APPDB = new string[] { "applicationdb", "connectionstring" };
		private string[] PARAMKEYS_NEWNAME = new string[] { "newdb", "newdatabase" };
		private string PARAMKEYS_SCRIPT = "script";
		private string PARAMKEYS_SCRIPTFILE = "scriptfile";
		private string PARAMKEYS_SCRIPTFILEACTION = "scriptfileaction";
		private string PARAMKEYS_DBVERSION = "dbversion";
		private string PARAMKEYS_VERSIONWARN = "versioningwarnings";
		private string PARAMKEYS_SHOWSQL = "showsql";
		private string PARAMKEYS_NOTRAN = "notran";
		private string PARAMKEYS_NONORMALIZE = "nonormalize";
		private string PARAMKEYS_USEHASH = "usehashes";
		#endregion

		#region Constructor
		/// <summary>
		/// The default constructor
		/// </summary>
		public DatabaseInstaller()
		{
			InitializeComponent();
		}
		#endregion

		#region Install

		/// <summary>
		/// Performs an install of a database
		/// </summary>
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);

			var commandParams = GetCommandLineParameters();

			var paramUICount = 0;
			var setup = new InstallSetup();
			if (commandParams.Count > 0)
			{
				if (commandParams.ContainsKey(PARAMKEYS_SHOWSQL))
				{
					setup.ShowSql = true;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_NOTRAN))
				{
					setup.UseTransaction = false;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_NONORMALIZE))
				{
					setup.Normalize = false;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_USEHASH))
				{
					setup.UseHashes = true;
					paramUICount++;
				}

				if (commandParams.ContainsKey(PARAMKEYS_VERSIONWARN))
				{
					if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "accept")
					{
						setup.AcceptVersionWarnings = true;
					}
					else
					{
						throw new Exception("The /versioningwarnings parameter must be set to 'accept'.");
					}
					paramUICount++;
				}

				setup.ConnectionString = GetAppDbString(commandParams);
				setup.MasterConnectionString = GetMasterDbConnectionString(commandParams);
				if (GetUpgradeDbSetting(commandParams, setup.IsUpgrade))
					setup.InstallStatus = InstallStatusConstants.Upgrade;
				if (commandParams.Any(x => PARAMKEYS_CREATE.Contains(x.Key)))
					setup.InstallStatus = InstallStatusConstants.Create;

				if (commandParams.Any(x => PARAMKEYS_UPGRADE.Contains(x.Key)) && commandParams.Any(x => PARAMKEYS_CREATE.Contains(x.Key)))
					throw new Exception("You cannot specify both the create and update action.");
				if (commandParams.Count(x => PARAMKEYS_NEWNAME.Contains(x.Key)) > 1)
					throw new Exception("The new database name was specified more than once.");
				if (commandParams.Count(x => PARAMKEYS_MASTERDB.Contains(x.Key)) > 1)
					throw new Exception("The master database connection string was specified more than once.");
				if (commandParams.Count(x => PARAMKEYS_APPDB.Contains(x.Key)) > 1)
					throw new Exception("The connection string was specified more than once.");

				//Determine if calling as a script generator
				if (commandParams.ContainsKey(PARAMKEYS_SCRIPT))
				{
					var scriptAction = commandParams[PARAMKEYS_SCRIPT].ToLower();
					switch (scriptAction)
					{
						case "versioned":
						case "unversioned":
						case "create":
							break;
						default:
							throw new Exception("The script action must be 'create', 'versioned', or 'unversioned'.");
					}

					if (!commandParams.ContainsKey(PARAMKEYS_SCRIPTFILE))
						throw new Exception("The '" + PARAMKEYS_SCRIPTFILE + "' parameter must be set for script generation.");

					var dumpFile = commandParams[PARAMKEYS_SCRIPTFILE];
					if (!IsValidFileName(dumpFile))
						throw new Exception("The '" + PARAMKEYS_SCRIPTFILE + "' is not valid.");

					var fileCreate = true;
					if (commandParams.ContainsKey(PARAMKEYS_SCRIPTFILEACTION) &&
							(commandParams[PARAMKEYS_SCRIPTFILEACTION] + string.Empty) == "append")
						fileCreate = false;

					if (File.Exists(dumpFile) && fileCreate)
					{
						File.Delete(dumpFile);
						System.Threading.Thread.Sleep(500);
					}

					switch (scriptAction)
					{
						case "versioned":
							if (!commandParams.ContainsKey(PARAMKEYS_DBVERSION))
								throw new Exception("Generation of versioned scripts requires a '" + PARAMKEYS_DBVERSION + "' parameter.");

							if (!GeneratedVersion.IsValid(commandParams[PARAMKEYS_DBVERSION]))
								throw new Exception("The '" + PARAMKEYS_DBVERSION + "' parameter is not valid.");

							Console.WriteLine("Generate Script Started");
							setup.InstallStatus = InstallStatusConstants.Upgrade;
							setup.Version = new GeneratedVersion(commandParams[PARAMKEYS_DBVERSION]);
							File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
							Console.WriteLine("Generated Create Script");
							break;
						case "unversioned":
							if (commandParams.ContainsKey(PARAMKEYS_DBVERSION))
								throw new Exception("Generation of unversioned scripts cannot use a '" + PARAMKEYS_DBVERSION + "' parameter.");

							Console.WriteLine("Generate Script Started");
							setup.InstallStatus = InstallStatusConstants.Upgrade;
							setup.Version = UpgradeInstaller._def_Version;
							File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
							Console.WriteLine("Generated Create Script");
							break;
						case "create":
							Console.WriteLine("Generate Script Started");
							setup.InstallStatus = InstallStatusConstants.Create;
							setup.Version = new GeneratedVersion(-1, -1, -1, -1, -1);
							File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
							Console.WriteLine("Generated Create Script");
							break;
					}

					return;
				}

				//If we processed all parameters and they were UI then we need to show UI
				if (paramUICount < commandParams.Count)
				{
					setup.NewDatabaseName = commandParams.Where(x => PARAMKEYS_NEWNAME.Contains(x.Key)).Select(x => x.Value).FirstOrDefault();
					Install(setup);
					return;
				}
			}

			UIInstall(setup);

		}

		private bool IsValidFileName(string fileName)
		{
			try
			{
				new System.IO.FileInfo(fileName);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// Performs an install of a database
		/// </summary>
		public void Install(InstallSetup setup)
		{
			if (setup.InstallStatus == InstallStatusConstants.Create)
			{
				//Conection cannot reference an existing database
				if (SqlServers.TestConnectionString(setup.ConnectionString))
					throw new Exception("The connection string references an existing database.");

				//The new database name must be specified
				if (string.IsNullOrEmpty(setup.NewDatabaseName))
					throw new Exception("A new database name was not specified.");

				//The connection string and the new database name must be the same
				var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(setup.ConnectionString);
				if (builder.InitialCatalog.ToLower() != setup.NewDatabaseName.ToLower())
					throw new Exception("A new database name does not match the specified connection string.");

				SqlServers.CreateDatabase(setup);
			}
			else if (setup.InstallStatus == InstallStatusConstants.Upgrade)
			{
				//The connection string must reference an existing database
				if (!SqlServers.TestConnectionString(setup.ConnectionString))
					throw new Exception("The connection string does not reference a valid database.");
			}

			UpgradeInstaller.UpgradeDatabase(setup);
		}

		/// <summary>
		/// Returns the upgrade script for the specified database
		/// </summary>
		public string GetScript(InstallSetup setup)
		{
			if (string.IsNullOrEmpty(setup.ConnectionString) && setup.Version == null)
				throw new Exception("The connection string must be set.");
			if (setup.SkipSections == null)
				setup.SkipSections = new List<string>();
			return UpgradeInstaller.GetScript(setup);
		}

		#endregion

		#region Uninstall

		/// <summary>
		/// 
		/// </summary>
		/// <param name="savedState"></param>
		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			base.Uninstall(savedState);
		}

		#endregion

		#region NeedsUpdate

		/// <summary>
		/// Determines if the specified database needs to be upgraded
		/// </summary>
		public virtual bool NeedsUpdate(string connectionString)
		{
			return UpgradeInstaller.NeedsUpdate(connectionString);
		}

		/// <summary>
		/// Determines the current version of the specified database
		/// </summary>
		public virtual string VersionInstalled(string connectionString)
		{
			return UpgradeInstaller.VersionInstalled(connectionString);
		}

		/// <summary>
		/// The database version to which this installer will upgrade a database
		/// </summary>
		public virtual string VersionLatest()
		{
			return UpgradeInstaller.VersionLatest();
		}

		/// <summary>
		/// Determines if the specified database has ever been versioned by the framework
		/// </summary>
		/// <param name="connectionString"></param>
		public virtual bool IsVersioned(string connectionString)
		{
			return UpgradeInstaller.IsVersioned(connectionString);
		}

		#endregion

		#region Helpers

		private bool GetUpgradeDbSetting(Dictionary<string, string> commandParams, bool defaultValue)
		{
			bool retVal = defaultValue;
			foreach (string s in PARAMKEYS_UPGRADE)
			{
				if (commandParams.ContainsKey(s))
				{
					retVal = true;
					break;
				}
			}
			return retVal;
		}

		private string GetMasterDbConnectionString(Dictionary<string, string> commandParams)
		{
			string retVal = string.Empty;
			foreach (string s in PARAMKEYS_MASTERDB)
			{
				if (commandParams.ContainsKey(s))
				{
					retVal = commandParams[s];
					break;
				}
			}
			return retVal;
		}

		private string GetAppDbString(Dictionary<string, string> commandParams)
		{
			string retVal = string.Empty;
			foreach (string s in PARAMKEYS_APPDB)
			{
				if (commandParams.ContainsKey(s))
				{
					retVal = commandParams[s];
					break;
				}
			}
			return retVal;
		}

		private Dictionary<string, string> GetCommandLineParameters()
		{
			var retVal = new Dictionary<string, string>();
			var args = Environment.GetCommandLineArgs();

			var loopcount = 0;
			foreach (var arg in args)
			{
				var regEx = new Regex(@"[-/](\w+)(:(.*))?");
				var regExMatch = regEx.Match(arg);
				if (regExMatch.Success)
				{
					retVal.Add(regExMatch.Groups[1].Value.ToLower(), regExMatch.Groups[3].Value);
				}
				else
				{
					//var tmpKey = Guid.NewGuid().ToString();
					//if (loopcount == 0)
					//  tmpKey = EXENAME_KEY;
					//else if (loopcount == 1)
					//  tmpKey = DLLNAME_KEY;
				}
				loopcount++;
			}

			return retVal;
		}

		private bool IdentifyDatabaseConnectionString(InstallSetup setup)
		{
			var F = new IdentifyDatabaseForm(setup);
			if (F.ShowDialog() == DialogResult.OK)
			{
				this.Action = F.Action;
				this.Settings = F.Settings;
				return true;
			}
			return false;
		}

		/// <summary />
		private void UIInstall(InstallSetup setup)
		{
			if (IdentifyDatabaseConnectionString(setup))
			{
				setup.ConnectionString = this.Settings.GetPrimaryConnectionString();
				setup.InstallStatus = InstallStatusConstants.Upgrade;

				if (this.Action == ActionTypeConstants.Create)
				{
					setup.InstallStatus = InstallStatusConstants.Create;
					UpgradeInstaller.UpgradeDatabase(setup);
				}
				else if (this.Action == ActionTypeConstants.Upgrade)
				{
					UpgradeInstaller.UpgradeDatabase(setup);
				}
				else if (this.Action == ActionTypeConstants.AzureCopy)
				{
					UpgradeInstaller.AzureCopyDatabase(this.Settings);
				}
			}
		}

		#endregion

		internal InstallSettings Settings { get; private set; }

		/// <summary>
		/// The action to take
		/// </summary>
		internal ActionTypeConstants Action { get; private set; }

	}

	/// <summary />
	public enum InstallStatusConstants
	{
		/// <summary />
		Create,
		/// <summary />
		Upgrade
	}

	/// <summary />
	public class InstallSetup
	{
		/// <summary />
		public InstallSetup()
		{
			this.SkipSections = new List<string>();
			this.InstallStatus = InstallStatusConstants.Upgrade;
			this.UseTransaction = true;
			this.Normalize = true;
			this.UseHashes = false;
			this.SuppressUI = false;
		}

		/// <summary>
		/// The connection information to the SQL Server master database
		/// </summary>
		public string MasterConnectionString { get; set; }

		/// <summary>
		/// The connection string to the newly created database
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary />
		public GeneratedVersion Version { get; set; }

		/// <summary />
		public bool UseHashes { get; set; }

		/// <summary />
		internal bool SuppressUI { get; set; }

		/// <summary>
		/// Determines if this is a database upgrade
		/// </summary>
		internal bool IsUpgrade
		{
			get { return this.InstallStatus == InstallStatusConstants.Upgrade; }
		}

		/// <summary />
		public bool UseTransaction { get; set; }

		/// <summary />
		public bool Normalize { get; set; }

		/// <summary>
		/// The transaction to use for this action. If null, one will be created.
		/// </summary>
		public System.Data.SqlClient.SqlTransaction Transaction { get; set; }

		/// <summary />
		public List<string> SkipSections { get; set; }

		/// <summary />
		public string NewDatabaseName { get; set; }

		/// <summary />
		public bool ShowSql { get; set; }

		internal bool NewInstall
		{
			get { return this.InstallStatus == InstallStatusConstants.Create; }
		}

		/// <summary />
		public InstallStatusConstants InstallStatus { get; set; }

		/// <summary />
		public bool AcceptVersionWarnings { get; set; }
	}

}
#pragma warning restore 0168