using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace WorkDivision
{
    class liteDB
    {
        //public SqlConnectionStringBuilder MedConn;
        //Конструктор
        public liteDB()
        {
        }
        public SQLiteConnection GetConn()
        {
           return new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
        }

    }

}
