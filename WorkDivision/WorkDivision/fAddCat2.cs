using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WorkDivision
{
	public partial class fAddCat2 : Form
    {
        private SQLiteConnection _dblite;
        private SQLiteDataReader _sqlReader;
        SQLiteCommand m_sqlCmd = null;
        liteDB liteDB = new liteDB();

        public fAddCat2()
        {
            InitializeComponent();
        }

        public string id_rec { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id_rec == "")  //Если  id записи не передан (новая запись) 
            {
                string querySQLite = @"INSERT INTO DirProdGRP (GRP) VALUES (@GRP)";
                m_sqlCmd = new SQLiteCommand(querySQLite, _dblite);
                m_sqlCmd.Parameters.AddWithValue("@GRP", tbName.Text);
            }
            else
            {
                string querySQLite = @"UPDATE DirProdGRP SET GRP=@GRP WHERE id=" + id_rec;
                m_sqlCmd = new SQLiteCommand(querySQLite, _dblite);
                m_sqlCmd.Parameters.AddWithValue("@GRP", tbName.Text);
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

        private void fAddCat2_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            _dblite = liteDB.GetConn();
            _dblite.Open();
            //Если выбран существующий интервал, загружаем параметры
            if (id_rec != "")
                {
                    GetCat2();
                }
                else
                {
                    tbName.Text = "";
                }
        }

        public async void GetCat2()
        {
            try
            {
                string query = @"SELECT * FROM DirProdGRP WHERE id =" + id_rec;
                m_sqlCmd = new SQLiteCommand(query, _dblite);
                _sqlReader = m_sqlCmd.ExecuteReader();
                while (await _sqlReader.ReadAsync())
                {
                    tbName.Text = Convert.ToString(_sqlReader["GRP"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.03.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
        }

    }
}
