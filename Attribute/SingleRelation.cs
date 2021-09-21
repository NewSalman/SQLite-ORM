using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteORM.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class SingleRelation : System.Attribute
    {
    }
}
