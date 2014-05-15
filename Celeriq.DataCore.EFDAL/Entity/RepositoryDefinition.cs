using System;
using System.Linq.Expressions;
namespace Celeriq.DataCore.EFDAL.Entity
{
    partial class RepositoryDefinition
    {
        public static int UpdateData(Expression<Func<Celeriq.DataCore.EFDAL.RepositoryDefinitionQuery, long>> select, Expression<Func<Celeriq.DataCore.EFDAL.RepositoryDefinitionQuery, bool>> where, long newValue)
        {
            return BusinessObjectQuery<Celeriq.DataCore.EFDAL.Entity.RepositoryDefinition, Celeriq.DataCore.EFDAL.RepositoryDefinitionQuery, long>.UpdateData(select, where, newValue, "RepositoryDefinition", GetDatabaseFieldName, true);
        }

    }
}
