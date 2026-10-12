using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.core.GenericRepository
{
    public interface IGenericRepositorySqlClient<TEntity>
    {
        IList<TEntity> ExecuteQuerySQL(string sqlQueryString);
        IList<TEntity> ExecuteProcedureSQL(string sqlProcedure, List<SqlParameter> sqlParameters);
        IList<TEntity> ExecuteProcedureParamsObjectSQL(string sqlProcedure, params object[] sqlParameters);
    }
}
