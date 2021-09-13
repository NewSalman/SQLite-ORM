using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM.Handler
{
    public class SQLiteHandler : ISQLiteHandler
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        private SqlConnection _conn { get; set; }
        public DataSet items { get; set; }

        public SQLiteHandler()
        {
            _conn = new SqlConnection(ConnectionString);
            items = new DataSet();
            Init();
        }

        private void Init()
        {
            string QueryString = $"Select * From {TableName}";
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.SelectCommand = new SqlCommand(QueryString, _conn);
                adapter.Fill(items);
            }
        }

        public Task AddItem<Type>(Type item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteItem<Type>(Type id)
        {
            throw new NotImplementedException();
        }

        public Task GetAllItems()
        {
            throw new NotImplementedException();
        }

        public Task GetItem<Type>(Type id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateItem<Type, IType>(Type id, IType item)
        {
            throw new NotImplementedException();
        }
    }
}
