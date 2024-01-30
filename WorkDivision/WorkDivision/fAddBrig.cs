using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WorkDivision
{
	public partial class fAddBrig : Form
    {
        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        private liteDB liteDB;
        SQLiteCommand m_sqlCmd = null;
        

        public fAddBrig()
        {
            InitializeComponent();
            liteDB = new liteDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id_rec == "")  //Если  id записи не передан (новая запись) 
            {
                string querySQLite = @"INSERT INTO DirBrigs (KODBR, Name, Numk) VALUES (@KODBR, @Name, @Numk)";
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@KODBR", tbKODBR.Text);
                m_sqlCmd.Parameters.AddWithValue("@Name", tbName.Text);
                m_sqlCmd.Parameters.AddWithValue("@Numk", tbNumk.Text);
            }
            else
            {
                string querySQLite = @"UPDATE DirBrigs SET KODBR=@KODBR, Name=@Name, Numk=@Numk WHERE id=" + id_rec;
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@KODBR", tbKODBR.Text);
                m_sqlCmd.Parameters.AddWithValue("@Name", tbName.Text);
                m_sqlCmd.Parameters.AddWithValue("@Numk", tbNumk.Text);
            }
            try
            {
                m_sqlCmd.Prepare();
                m_sqlCmd.ExecuteNonQuery();
                this.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.21.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string id_rec { get; set; }

        private void fAddBrig_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
            dblite.Open();
            //Если новая запись
            if (id_rec != "")
                {
                    GetBrig();
                }
                else
                {
                    tbKODBR.Text = "";
                    tbName.Text = "";
                    tbNumk.Text = "";
                }
        }

        public async void GetBrig()
        {
            try
            {
                string query = @"SELECT * FROM DirBrigs WHERE id =" + id_rec;

                m_sqlCmd = new SQLiteCommand(query, dblite);

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {

                    tbKODBR.Text = Convert.ToString(sqlReader["KODBR"]);
                    tbName.Text = Convert.ToString(sqlReader["Name"]);
                    tbNumk.Text = Convert.ToString(sqlReader["Numk"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

    }
}
