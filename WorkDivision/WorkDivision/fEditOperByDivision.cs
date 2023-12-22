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
using System.Globalization;

namespace WorkDivision
{
    public partial class fEditOperByDivision : Form
    {
        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        SQLiteCommand m_sqlCmd = null;
        

        public fEditOperByDivision()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            dblite.Open();
            string querySQLite = @"UPDATE DirOpers SET Name=@Name, UCH=@UCH, PER=@PER, NST=@NST, KOEF=@KOEF, NVR=@NVR WHERE id=" + id_rec;
            m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
            m_sqlCmd.Parameters.AddWithValue("@Name", tbOperName.Text);
            m_sqlCmd.Parameters.AddWithValue("@UCH", tbMatRate.Text);
            m_sqlCmd.Parameters.AddWithValue("@PER", tbTarif.Text);
            m_sqlCmd.Parameters.AddWithValue("@NST", tbWorkersCnt.Text);
            m_sqlCmd.Parameters.AddWithValue("@KOEF", tbNVR.Text.Replace(",", "."));
            m_sqlCmd.Parameters.AddWithValue("@NVR", tbCost.Text.Replace(",","."));
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

        private void fAddOper_Load(object sender, EventArgs e)
        {
            dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            dblite.Open();
            //Если выбран существующий интервал, загружаем параметры
            if (id_rec != "")
            {
                    //MessageBox.Show(id_rec.ToString());
                GetOperByDivision();
            }
            else
            {
                tbOperName.Text = "";
                tbMatRate.Text = "";
                tbTarif.Text = "";
                tbWorkersCnt.Text = "";
                tbNVR.Text = "";
                tbCost.Text = "";
            }
        }

        public async void GetOperByDivision()
        {
            try
            {
                //dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
                //dblite.Open();
                string query = @"SELECT i.id,d.UCH,d.Name,'' as NMAT,i.rank,i.NVR,i.workers_cnt
                                FROM inDivision as i 
                                LEFT JOIN DirOpers as d on d.id=i.id_oper
                                WHERE i.id =" + id_rec;

                m_sqlCmd = new SQLiteCommand(query, dblite);
                //m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    tbOperName.Text = Convert.ToString(sqlReader["Name"]);
                    tbMatRate.Text = "";
                    tbTarif.Text = "";
                    tbWorkersCnt.Text = "";
                    tbNVR.Text = Convert.ToString(sqlReader["NVR"]);
                    tbCost.Text = "";

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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
