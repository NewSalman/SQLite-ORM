using Microsoft.Data.Sqlite;
using SQLiteORM.Testing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM.Handler
{
    public interface ISQLiteHandler<T> : IDisposable
    {
        //string ConnectionString { get; set; }
        SqliteConnection _conn { get; set; }
        string TableName { get; set; }
        Task<T> GetItem<Param>(Param id);
        Task<List<T>> GetAllItems();
        Task AddItem(T item);
        Task UpdateItem<Param>(Param primaryKey, T item);
        Task DeleteItem<Param>(Param id);
    }
}
