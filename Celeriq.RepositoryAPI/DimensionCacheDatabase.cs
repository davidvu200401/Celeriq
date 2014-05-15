using Celeriq.Common;
using Celeriq.DataCore.EFDAL;
using Celeriq.DataCore.EFDAL.Entity;
using Celeriq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.RepositoryAPI
{
    internal class DimensionCacheDatabase : DimensionCacheBase
    {
        public DimensionCacheDatabase(int repositoryId, Guid repositoryKey, DimensionDefinition dimensionDefinition, int index)
            : base(repositoryId, repositoryKey, dimensionDefinition, index)
        {
        }

        private DataCore.EFDAL.Entity.DimensionStore CreateDimension(int repositoryId, Guid repositoryKey, DataCoreEntities context, DimensionDefinition dimensionDefinition)
        {
            try
            {
                var dItem = context.DimensionStore.FirstOrDefault(x => x.RepositoryDefinition.UniqueKey == repositoryKey && x.DIdx == dimensionDefinition.DIdx);
                if (dItem == null)
                {
                    dItem = new DataCore.EFDAL.Entity.DimensionStore();
                    dItem.RepositoryId = repositoryId;
                    dItem.DIdx = dimensionDefinition.DIdx;
                    dItem.Name = dimensionDefinition.Name;
                    context.AddItem(dItem);
                    context.SaveChanges();
                }
                return dItem;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        public override void WriteItem(RefinementItem refinementItem)
        {
            try
            {
                using (var context = new DataCoreEntities())
                {
                    var item = context.DimensionStore.FirstOrDefault(x => x.RepositoryDefinition.UniqueKey == _repositoryKey && x.DIdx == this.DimensionDefinition.DIdx);
                    if (item == null) item = this.CreateDimension(_repositoryId, _repositoryKey, context, this.DimensionDefinition);

                    context.AddItem(new DimensionData
                    {
                        Data = refinementItem.ObjectToBin(),
                        DimensionsionStoreId = item.DimensionStoreId,
                    });
                    context.SaveChanges();
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
