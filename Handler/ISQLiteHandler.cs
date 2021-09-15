using Microsoft.Data.Sqlite;
using SQLiteORM.Testing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM.Handler
{
    public interface ISQLiteHandler : IDisposable
    {
        //string ConnectionString { get; set; }
        SqliteConnection _conn { get; set; }
        string TableName { get; set; }
        Task<T> GetItem<U, T>(U id) where T : new();
        Task<List<T>> GetAllItems<T>() where T : new();
        Task AddItem<Type>(Type item);
        Task UpdateItem<Type, IType>(Type id, IType item);
        Task DeleteItem<Type>(Type id);
    }
}
