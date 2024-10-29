using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DAL
{
    public abstract class SqlDbRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private string _connectionString;
        public SqlDbRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDbConnection GetOpenConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public TEntity GetSingle(int aSingleId)
        {
            throw new NotImplementedException();
        }
    }
}
