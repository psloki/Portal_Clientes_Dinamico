using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Portal.core.GenericRepository
{
    public interface IGenericReflection<TEntity>
    {
        IList<TEntity> ToListReflection(DataTable table);
        TEntity CreateItemFromRow(DataRow row, IList<PropertyInfo> properties);
    }
}
