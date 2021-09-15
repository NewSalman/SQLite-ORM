using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLiteORM.Handler;
using SQLiteORM.Testing;

namespace SQLiteORM
{
    public class SQLiteORM
    {
        ISQLiteHandler handler { get; set; }
        
        public SQLiteORM(string table, string connectionString)
        {

            handler = new SQLiteHandler(@"Data Source=C:\Users\Admin7\source\repos\MyPortofolio\Portofilio\User.db;Cache=Shared");
            handler.TableName = "UserSession";

        }

        public Task GetItem<T>(T id)
        {
            throw new NotImplementedException();
        }


        //read entire data an make coloum name as proprty using dynamic
        //for future dev msg
        public Task<List<T>> GetAllItems<T>() where T:new() 
        {
            return handler.GetAllItems<T>();
        }

        public Task AddItem<T>(T item)
        {
            return handler.AddItem(item);
        }

        public Task DeleteItem<T>(T id)
        {
            return handler.DeleteItem(id);
        }

        public Task<T> GetItem<U, T>(U id) where T : new()
        {
            return handler.GetItem<U, T>(id);
        }
    }
}
