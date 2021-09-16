using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLiteORM.Handler;
using SQLiteORM.Testing;

namespace SQLiteORM
{
    public class ORM<T> where T : new()
    {
        ISQLiteHandler<T> handler { get; set; }

        public ORM(string table, string connectionString)
        {
            handler = new SQLiteHandler<T>(@"Data Source=C:\Users\Admin7\source\repos\MyPortofolio\Portofilio\User.db;Cache=Shared");
            handler.TableName = "UserSession";

        }

        public Task<List<T>> GetAllItems()
        {
            return handler.GetAllItems();
        }

        public Task AddItems(T item)
        {
            return handler.AddItem(item);
        }

        public Task DeleteItems<Param>(Param id)
        {
            return handler.DeleteItem(id);
        }

        public Task<T> GetItems<Param>(Param id)
        {
            return handler.GetItem(id);
        }

        public Task UpdateItems<Param>(Param id, T item)
        {
            return handler.UpdateItem(id, item);
        }
    }
}
