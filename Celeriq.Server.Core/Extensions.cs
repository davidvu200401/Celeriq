using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Utilities;

namespace Celeriq.Server.Core
{
    internal static class Extensions
    {
        //public static void SaveRepository(this Celeriq.Common.RepositorySchema repository)
        //{
        //    try
        //    {
        //        using (var context = new Celeriq.DataCore.EFDAL.DataCoreEntities())
        //        {
        //            var item = context.RepositoryDefinition.FirstOrDefault(x => x.UniqueKey == repository.ID);
        //            if (item != null)
        //            {
        //                item.DimensionDataList.Clear();
        //                item.RepositoryLogList.Clear();
        //                item.RepositoryStatList.Clear();
        //                item.RespositoryDataList.Clear();
        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                item = new Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition();
        //                item.UniqueKey = repository.ID;
        //                context.AddItem(item);
        //            }

        //            item.Name = repository.Name;
        //            item.ItemCount = 0;
        //            item.MemorySize = 0;
        //            item.ItemCount = 0;
        //            item.VersionHash = repository.VersionHash;
        //            item.DefinitionData = repository.ToXml().Zip();
        //            var c = context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex);
        //        throw;
        //    }
        //}
    }
}
