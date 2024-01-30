using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

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

        public DataTable GetLiteData(string query)
        {
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand comm = new SQLiteCommand(query, GetConn());

                SQLiteDataAdapter da = new SQLiteDataAdapter(comm);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
            return dt;
        }

    }

}
