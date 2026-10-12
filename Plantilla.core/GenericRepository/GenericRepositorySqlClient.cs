using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plantilla.core.Manager;

namespace Portal.core.GenericRepository
{
    public class GenericRepositorySqlClient<TEntity> : IGenericRepositorySqlClient<TEntity> where TEntity : class, new()
    {
        protected string _cnn;
        protected GenericReflection<TEntity> _reflectionDataTable;

        public GenericRepositorySqlClient(string cnn)
        {
            string connectionString = Manager.GetConnectionString();
            _cnn = cnn; /*connectionString;*/
            _reflectionDataTable = new GenericReflection<TEntity>();
        }

        public IList<TEntity> ExecuteProcedureSQL(string sqlProcedure, List<SqlParameter> sqlParameters)
        {
            return ExecuteProcedureSqlClient(sqlProcedure, sqlParameters, null);
        }

        public IList<TEntity> ExecuteQuerySQL(string sqlQueryString)
        {
            try
            {
                DataTable dt = new DataTable();

                using (var conexion = new SqlConnection(_cnn))
                {
                    SqlCommand cmd = new SqlCommand(sqlQueryString, conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        dt.Load(reader);

                    conexion.Close();
                }

                return _reflectionDataTable.ToListReflection(dt);
            }
            catch (Exception ex) { throw new Exception("GenericRepository - ExecuteQuerySQL", ex); }
        }

        public IList<TEntity> ExecuteProcedureParamsObjectSQL(string sqlProcedure, params object[] sqlParameters)
        {
            return ExecuteProcedureSqlClient(sqlProcedure, new List<SqlParameter>(), sqlParameters);
        }

        private IList<TEntity> ExecuteProcedureSqlClient(string sqlProcedure, List<SqlParameter> sqlParameters, params object[] sqlObjectParameters)
        {
            SqlConnection conn = new SqlConnection(_cnn);
            DataSet dt = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = sqlProcedure;

                da.SelectCommand = cmd;

                if (sqlParameters.Count() > 0)
                {
                    cmd.Parameters.Clear();

                    foreach (SqlParameter param in sqlParameters)
                        cmd.Parameters.Add(param);
                }

                if (sqlObjectParameters != null)
                {
                    SqlCommandBuilder.DeriveParameters(cmd);

                    for (var i = 1; i <= sqlObjectParameters.Length; i++)
                        cmd.Parameters[i].Value = sqlObjectParameters[i - 1];
                }

                da.Fill(dt);

                return _reflectionDataTable.ToListReflection(dt.Tables[0]);
            }
            catch (Exception ex) { throw new Exception("GenericRepository - ExecuteProcedureSQL", ex); }
            finally { conn.Close(); }
        }
    }
}
