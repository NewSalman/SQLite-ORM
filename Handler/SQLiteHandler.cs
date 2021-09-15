using SQLiteORM.Testing;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GenericFuntionLib;

namespace SQLiteORM.Handler
{
    public class SQLiteHandler : ISQLiteHandler
    {
        //public string ConnectionString { get; set; } = @"Data Source=C:\Users\Admin7\source\repos\MyPortofolio\Portofilio\User.db;";
        public string TableName { get; set; } = "Users";

        public SqliteConnection _conn { get; set; }
        //private List<User> Result = new List<User>();


        public SQLiteHandler(string ConnectionString)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            _conn = new SqliteConnection(ConnectionString);
            _conn.Open();


        }


        /// <summary>
        ///  Insert item into table
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="item">Item object</param>
        /// <returns>Task</returns>
        /// <note>id not passed if int using attribute indetifier if guid or string have to manual input note for nex dev</note>
        public Task AddItem<Type>(Type item)
        {
            PropertyInfo[] props = item.GetType().GetProperties();
            string AddStmt = $"INSERT INTO {TableName} Values ";
            StringBuilder sb = new StringBuilder("(");

            for(int i = 0; i < props.Length; i++)
            {
                if(i == props.Length - 1)
                {
                    sb.Append($"${props[i].Name}");
                    break;
                }
                sb.Append($"${props[i].Name},");
            }
            //add closing bracket on end of interation
            sb.Append(")");

            //append to stmt
            AddStmt += sb.ToString();

            Console.WriteLine(AddStmt);

            var cmmd = _conn.CreateCommand();
            cmmd.CommandText = AddStmt;

            for(int i = 0; i < props.Length; i++)
            {
                cmmd.Parameters.AddWithValue($"${props[i].Name}", Generic.GetValue(item, props[i].Name));
                Console.WriteLine(Generic.GetValue(item, props[i].Name));
            }

            int added = cmmd.ExecuteNonQuery();

            Console.WriteLine("item added = {0}", added);

            return Task.CompletedTask;
        }


        /// <summary>
        /// Delete Item from table
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="id">Int or guid or string</param>
        /// <returns>Task</returns>
        public Task DeleteItem<Type>(Type id)
        {
            string Deletestmt = $"Delete From {TableName} Where id = $id";
            using(var commd = _conn.CreateCommand())
            {
                commd.CommandText = Deletestmt;
                commd.Parameters.AddWithValue("$id", id);

                Console.WriteLine("item deleted = {0}", commd.ExecuteNonQuery());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get All items set set into item obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of T / items </returns>
        public Task<List<T>> GetAllItems<T>() where T:new()
        {
            var commd = _conn.CreateCommand();
            commd.CommandText = $"Select * From {TableName}";
            List<T>  items = new List<T>();
            using (SqliteDataReader reader = commd.ExecuteReader())
            {
                while (reader.Read())
                {
                    T item = new T();
                    items.Add(SeriliazeItem(reader, item));
                }
            }
            return Task.FromResult(items);
        }

        public Task<T> GetItem<U, T>(U id) where T : new()
        {
            string QueryStmt = $"Select id,Username,LoginAt From {TableName} Where id = $id";
            T item = new T();
            using (var commd = _conn.CreateCommand())
            {
                commd.CommandText = QueryStmt;
                commd.Parameters.AddWithValue("$id", id);
                
                using (SqliteDataReader reader = commd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        SeriliazeItem(reader, item);
                    }
                }
                
            }

            return Task.FromResult(item);
        }

        public Task UpdateItem<Type, IType>(Type id, IType item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _conn.Close();
        }

        private T SeriliazeItem<T>(SqliteDataReader reader, T item)
        {
            PropertyInfo[] props = item.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                Generic.SetValut(item, props[i].Name, int.Parse(reader[props[i].Name]));
            }

            return item;
        }
    }
}
