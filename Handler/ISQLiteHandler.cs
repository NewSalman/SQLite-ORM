using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM.Handler
{
    public interface ISQLiteHandler
    {
        string ConnectionString { get; set; }
        string TableName { get; set; }
        DataSet items { get;set }
        Task GetItem<Type>(Type id);
        Task GetAllItems();
        Task AddItem<Type>(Type item);
        Task UpdateItem<Type, IType>(Type id, IType item);
        Task DeleteItem<Type>(Type id);
    }
}
