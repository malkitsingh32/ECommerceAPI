using Application.Abstraction.DataBase;
using System.Data;
using System.Data.SqlClient;

namespace Persistence.DataBase
{
    public class ParameterManager : IParameterManager
    {

        public IDataParameter Get(object value) => Get("Id", value);
        public IDataParameter Get(string paramName, object value)
        {
            return Get(paramName, value, ParameterDirection.Input, DbType.String);
        }
        public IDataParameter Get(string paramName, object value, ParameterDirection direction)
        {
            return Get(paramName, value, direction, DbType.String);
        }
        public IDataParameter Get(string paramName, object value, ParameterDirection direction, DbType type)
        {
            IDataParameter param = new SqlParameter
            {
                ParameterName = paramName,
                Value = value,
                Direction = direction,
                DbType = type
            };
            return (SqlParameter)param;
        }
        public IDataParameter GetNew(string paramName, object value, ParameterDirection direction, SqlDbType type)
        {
            var param = new SqlParameter
            {
                ParameterName = paramName,
                Value = value,
                Direction = direction,
                SqlDbType = type
            };
            return param;
        }
    }
}
