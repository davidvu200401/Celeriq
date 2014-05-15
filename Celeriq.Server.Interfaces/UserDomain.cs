using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Celeriq.Common;
using Celeriq.Utilities;
using Celeriq.DataCore.EFDAL;

namespace Celeriq.Server.Interfaces
{
    /// <summary>
    /// Handles all the user permission
    /// </summary>
    public static class UserDomain
    {
        private static List<SystemCredentials> _userList = null;
        private static object _locker = new object();

        public static StorageTypeConstants Storage { get; set; }

        /// <summary>
        /// The user list
        /// </summary>
        public static List<SystemCredentials> UserList
        {
            get
            {
                try
                {
                    lock (_locker)
                    {
                        if (_userList == null)
                            InitUsers();
                        return _userList;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Add a user to the system
        /// </summary>
        public static bool AddUser(SystemCredentials user)
        {
            lock (_locker)
            {
                if (_userList == null)
                    throw new Exception("Object not initialized!");

                if (string.IsNullOrEmpty(user.UserName))
                {
                    throw new Exception("The user name must be set.");
                }

                if (_userList.Count(x => x.UserName == user.UserName) == 0)
                {
                    if (!SecurityHelper.IsValidPassword(user.Password))
                    {
                        throw new Exception("The password does not meet length or complexity requirements.");
                    }

                    _userList.Add(user);
                    SaveUserFile();
                    return true;
                }
                else
                {
                    throw new Exception("The user already exists.");
                }
            }
        }

        /// <summary>
        /// Delete a user from the system
        /// </summary>
        public static bool DeleteUser(SystemCredentials user)
        {
            lock (_locker)
            {
                if (_userList == null)
                    throw new Exception("Object not initialized!");

                try
                {
                    if (user.UserName == "root")
                    {
                        throw new Exception("The root user cannot be deleted.");
                    }

                    if (_userList == null) return false;
                    var q = _userList.FirstOrDefault(x => x.UserName == user.UserName);
                    if (q != null)
                    {
                        _userList.Remove(q);
                        SaveUserFile();
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete a user from the system
        /// </summary>
        public static bool UpdateUser(SystemCredentials user)
        {
            lock (_locker)
            {
                if (_userList == null)
                    throw new Exception("Object not initialized!");

                try
                {
                    if (_userList == null) return false;
                    var q = _userList.FirstOrDefault(x => x.UserName == user.UserName);
                    if (q != null)
                    {
                        _userList.Remove(q);
                        _userList.Add(user);
                        SaveUserFile();
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Loads the user file off disk
        /// </summary>
        public static void InitUsers()
        {
            try
            {
                lock (_locker)
                {
                    if (_userList != null) return;
                    _userList = new List<SystemCredentials>();

                    if (Storage == StorageTypeConstants.Database)
                    {
                        #region Database
                        using (var context = new DataCoreEntities())
                        {
                            var list = context.UserAccount.ToList();
                            foreach (var item in list)
                            {
                                _userList.Add(new SystemCredentials() { UserName = item.UserName, Password = item.Password, UserId = item.UniqueKey });
                            }
                        }
                        #endregion
                    }
                    else if (Storage == StorageTypeConstants.File)
                    {
                        #region File
                        var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
                        var section = config.Sections["UserConfiguration"] as UserConfigurationSection;
                        _userList = new List<SystemCredentials>();
                        if (section == null)
                        {
                            var newUser = new SystemCredentials() { UserName = "root", Password = "password", UserId = Guid.NewGuid() };
                            _userList.Add(newUser);

                            section = new UserConfigurationSection();
                            config.Sections.Add("UserConfiguration", section);
                            section.UserConfiguration.Add(new UserConfigurationElement { UserName = newUser.UserName, Password = newUser.Password, UserId = newUser.UserId.ToString() });
                            config.Save();
                        }
                        else
                        {
                            foreach (var item in section.UserConfiguration.Cast<UserConfigurationElement>())
                            {
                                _userList.Add(new SystemCredentials() { UserName = item.UserName, Password = item.Password, UserId = new Guid(item.UserId) });
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Saves the user file to disk
        /// </summary>
        private static void SaveUserFile()
        {
            try
            {
                lock (_locker)
                {
                    if (_userList == null) return;

                    if (Storage == StorageTypeConstants.Database)
                    {
                        #region Database
                        using (var context = new DataCoreEntities())
                        {
                            //Save all existing users
                            foreach (var user in _userList)
                            {
                                var item = context.UserAccount.FirstOrDefault(x => x.UniqueKey == user.UserId);
                                if (item == null)
                                {
                                    item = new DataCore.EFDAL.Entity.UserAccount()
                                    {
                                        UserName = user.UserName,
                                        Password = user.Password,
                                        UniqueKey = user.UserId
                                    };
                                }
                            }

                            //Remove any that are now missing
                            var list = context.UserAccount.ToList();
                            foreach (var item in list)
                            {
                                if (!_userList.Any(x => x.UserId == item.UniqueKey))
                                {
                                    context.DeleteItem(item);
                                }
                            }

                            context.SaveChanges();
                        }
                        #endregion
                    }
                    else if (Storage == StorageTypeConstants.File)
                    {
                        #region File
                        var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
                        var section = config.Sections["UserConfiguration"] as UserConfigurationSection;
                        if (section == null)
                        {
                            section = new UserConfigurationSection();
                            config.Sections.Add("UserConfiguration", section);
                        }

                        foreach (var user in _userList)
                        {
                            section.UserConfiguration.Add(new UserConfigurationElement { UserName = user.UserName, Password = user.Password, UserId = user.UserId.ToString() });
                        }
                        config.Save();

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

    }
}