using SQLiteORM.Testing;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GenericFuntionLib;
using System.IO;
using SQLiteORM.Attribute;

namespace SQLiteORM.Handler
{
    public class SQLiteHandler<T> : ISQLiteHandler<T> where T:new()
    {
        //public string ConnectionString { get; set; } = @"Data Source=C:\Users\Admin7\source\repos\MyPortofolio\Portofilio\User.db;";
        public string TableName { get; set; } = "Users";

        public SqliteConnection _conn { get; set; }
        //private List<User> Result = new List<User>();


        public SQLiteHandler(string ConnectionString)
        {
            _conn = new SqliteConnection(ConnectionString);
            _conn.Open();
        }


        /// <summary>
        ///  Insert item into table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">Item object</param>
        /// <returns>Task</returns>
        /// <note>id not passed if int using attribute indetifier if guid or string have to manual input note for nex dev</note>
        public Task AddItem(T item)
        {
            PropertyInfo[] props = item.GetType().GetProperties();
            string AddStmt = $"INSERT INTO {TableName} Values ";
            StringBuilder sb = new StringBuilder("(");

            for(int i = 0; i < props.Length; i++)
            {
                var attribInfo = props[i].GetCustomAttribute(typeof(ManyRelation));
                if(attribInfo != null)
                {
                    if(i == props.Length - 1)
                    {
                        sb.ToString().TrimEnd(',');
                        break;
                    }
                    continue;
                }
                
                
                if (i == props.Length - 1)
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
                
                var attribInfo = props[i].GetCustomAttribute(typeof(ManyRelation));
                if (attribInfo != null)
                {
                    continue;
                }
                object value = Generic.GetValue(item, props[i].Name);
                if(value.GetType() == typeof(bool))
                {
                    cmmd.Parameters.AddWithValue($"${props[i].Name}", (bool)value ? 1 : 0);
                    continue;
                }
                cmmd.Parameters.AddWithValue($"${props[i].Name}", value);
                Console.WriteLine(Generic.GetValue(item, props[i].Name));
            }

            int added = cmmd.ExecuteNonQuery();

            Console.WriteLine("item added = {0}", added);

            return Task.CompletedTask;
        }


        /// <summary>
        /// Delete Item from table
        /// </summary>
        /// <typeparam name="Param"></typeparam>
        /// <param name="id">Int or guid or string</param>
        /// <returns>Task</returns>
        public Task DeleteItem<Param>(Param value, string coloumName)
        {
            string Deletestmt = $"Delete From {TableName} Where {coloumName} = $id";
            using(var commd = _conn.CreateCommand())
            {
                commd.CommandText = Deletestmt;
                commd.Parameters.AddWithValue($"${coloumName}", value);

                Console.WriteLine("item deleted = {0}", commd.ExecuteNonQuery());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get All items set set into item obj
        /// </summary>
        /// <returns>List of T / items </returns>
        public Task<List<T>> GetAllItems()
        {
            var commd = _conn.CreateCommand();
            commd.CommandText = $"Select * From {TableName}";
            List<T>  items = new List<T>();
            using (SqliteDataReader reader = commd.ExecuteReader())
            {
                while (reader.Read())
                {
                    try
                    {
                        items.Add(SeriliazeItem(reader));
                    }
                    catch (SqliteException sx)
                    {
                        Console.WriteLine(sx.Message);
                        continue;
                    }
                }
                
                
            }
            return Task.FromResult(items);
        }

        public Task<T> GetItem<Param>(Param Value, string ColoumName)
        {
            T item = new T();
            PropertyInfo[] props = item.GetType().GetProperties(); 
            string QueryStmt = $"Select  ";
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < props.Length; i++)
            {
                if(i == props.Length - 1)
                {
                    sb.Append($"{props[i].Name} ");
                    break;
                }
                sb.Append($"{props[i].Name},");
            }

            QueryStmt += sb.ToString() + $"From {TableName} Where {ColoumName} = ${ColoumName}";

            using (var commd = _conn.CreateCommand())
            {
                commd.CommandText = QueryStmt;
                commd.Parameters.AddWithValue($"${ColoumName}", Value);
                
                

                    try
                    {
                        using (SqliteDataReader reader = commd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                item = SeriliazeItem(reader);
                            }
                        }
                    }
                    catch (SqliteException sx)
                    {
                    Console.WriteLine(sx.Message);
                    }
            }

            return Task.FromResult(item);
        }

        public Task UpdateItem<Param>(Param coloumValue, string columnName, T item)
        {
            //init required variable
            string UpdateStmt = $"Update {TableName} Set ";
            PropertyInfo[] props = item.GetType().GetProperties();

            // adding column and value param to Update statement
            StringBuilder sb = new StringBuilder();

            for (int x = 0; x < props.Length; x++)
            {
                if (x == props.Length - 1)
                {
                    sb.Append($"{props[x].Name} = ${props[x].Name}");
                    break;
                }
                sb.Append($"{props[x].Name} = ${props[x].Name},");
            }

            UpdateStmt += sb.ToString();

            UpdateStmt +=$" Where {columnName} = {coloumValue}";

            Console.WriteLine(UpdateStmt);
            //run update the item
            using (var cmmd = _conn.CreateCommand())
            {
                cmmd.CommandText = UpdateStmt;

                for (int x = 0; x < props.Length; x++)
                {
                    cmmd.Parameters.AddWithValue($"${props[x].Name}", Generic.GetValue(item, props[x].Name));
                }

                int affected = cmmd.ExecuteNonQuery();

                Console.WriteLine($"rows affected = {affected} rows");
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _conn.Close();
        }

        private T SeriliazeItem(SqliteDataReader reader)
        {
            T item = new T();
            PropertyInfo[] props = item.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (reader[props[i].Name].GetType() == typeof(int))
                    {
                        Generic.SetValue(item, props[i].Name, int.Parse(reader[props[i].Name].ToString()));
                    }
                    else if (reader[props[i].Name].GetType() == typeof(DateTime))
                    {
                        Generic.SetValue(item, props[i].Name, DateTime.Parse(reader[props[i].Name].ToString()));
                    }
                    else if (reader[props[i].Name].GetType() == typeof(string))
                    {
                        Generic.SetValue(item, props[i].Name, reader[props[i].Name]);
                    }
                    else if (reader[props[i].Name].GetType() == typeof(Int64))
                    {
                        Generic.SetValue(item, props[i].Name, int.Parse(reader[props[i].Name].ToString()));
                    }
                    else
                    {
                        throw new FormatException();
                    }
                } catch(ArgumentOutOfRangeException)
                {
                    continue;
                }
            }
            return item;
        }

        private string AddParameter(string AppendString, bool withColoumnName)
        {
            T item = new T();
            PropertyInfo[] props = item.GetType().GetProperties();
            
            StringBuilder sb = new StringBuilder();

            if(withColoumnName)
            {
                for(int i = 0; i < props.Length; i++)
                {
                    if(i == props.Length - 1)
                    {
                        sb.Append($"{props[i].Name} = ${props[i].Name},");
                    }
                    sb.Append($"{props[i].Name} = ${props[i].Name},");
                }
                return AppendString += sb.ToString();
            }
            sb.Append("(");

            for (int i = 0; i < props.Length; i++)
            {
                if (i == props.Length - 1)
                {
                    sb.Append($"${props[i].Name}");
                    break;
                }
                sb.Append($"${props[i].Name},");
            }
            //add closing bracket on end of interation
            sb.Append(")");
            return AppendString += sb.ToString();
        }
    }
}
