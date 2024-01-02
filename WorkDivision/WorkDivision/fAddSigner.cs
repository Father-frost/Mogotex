using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Xml;

namespace WorkDivision
{
    public partial class fAddSigner : Form
    {
        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        SQLiteCommand m_sqlCmd = null;
        liteDB liteDB = new liteDB();

        public fAddSigner()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id_rec == "")  //Если  id записи не передан (новая запись) 
            {
                string querySQLite = @"INSERT INTO DirSigners (Post, FIO, ord) VALUES (@Post, @FIO, @ord)";
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@Post", tbPost.Text);
                m_sqlCmd.Parameters.AddWithValue("@FIO", tbFIO.Text);
                m_sqlCmd.Parameters.AddWithValue("@ord", tbOrder.Text);
            }
            else
            {
                string querySQLite = @"UPDATE DirSigners SET Post=@Post, FIO=@FIO, ord=@ord WHERE id=" + id_rec;
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@Post", tbPost.Text);
                m_sqlCmd.Parameters.AddWithValue("@FIO", tbFIO.Text);
                m_sqlCmd.Parameters.AddWithValue("@ord", tbOrder.Text);
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

        private void fAddSigner_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
            dblite.Open();
            //Если выбран существующий интервал, загружаем параметры
            if (id_rec != "")
                {
                    //MessageBox.Show(id_rec.ToString());
                    GetSigner();
                }
                else
                {
                    tbPost.Text = "";
                    tbFIO.Text = "";
                    tbOrder.Text = "";
                }
        }

        public async void GetSigner()
        {
            try
            {
                string query = @"SELECT * FROM DirSigners WHERE id =" + id_rec;

                m_sqlCmd = new SQLiteCommand(query, dblite);
                //m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {

                    tbPost.Text = Convert.ToString(sqlReader["post"]);
                    tbFIO.Text = Convert.ToString(sqlReader["FIO"]);
                    tbOrder.Text = Convert.ToString(sqlReader["ord"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.03.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

    }
}
