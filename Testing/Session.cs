using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteORM.Testing
{
    public class Session
    {
        public int id { get; set; }
        public string Username { get; set; }
        public DateTime LoginAt { get; set; }
    }
}
