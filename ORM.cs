using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLiteORM.Handler;
using SQLiteORM.Testing;
using System.IO;

namespace SQLiteORM
{
    public class ORM<T> where T : new()
    {
        ISQLiteHandler<T> handler { get; set; }

        public ORM(string table, string connectionString) 
        {
            handler = new SQLiteHandler<T>(connectionString);
            handler.TableName = table;
        }

        public ORM(string table, string dbname, string Password = "")
        {
            string path = Path.Combine(Environment.CurrentDirectory, dbname);
            Console.WriteLine(path);
            handler = new SQLiteHandler<T>($"Data Source={path};");
            handler.TableName = table;
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
