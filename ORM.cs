﻿using System;
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

        public Task DeleteItems<Param>(Param value, string coloumName)
        {
            return handler.DeleteItem(value, coloumName);
        }

        public Task<T> GetItems<Param>(Param value, string coloumName)
        {
            return handler.GetItem(value, coloumName);
        }

        public Task UpdateItems<Param>(Param value, string coloumName,T item)
        {
            return handler.UpdateItem(value, coloumName, item);
        }

        public void Dispose()
        {
            handler.Dispose();
        }
    }
}
