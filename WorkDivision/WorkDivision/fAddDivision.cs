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
using static System.Net.WebRequestMethods;
using System.Collections;

namespace WorkDivision
{
    public partial class fAddDivision : Form
    {
        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        SQLiteCommand m_sqlCmd = null;
        

        public fAddDivision()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            dblite.Open();
            if (id_rec == "")  //Если  id записи не передан (новая запись) 
            {
                
                string querySQLite = @"INSERT INTO Division (id_model) VALUES (@id_model)";
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@id_model", cbModel.SelectedValue);
            }
            else
            {
                string querySQLite = @"UPDATE Division SET  id_model=@id_model WHERE id=" + id_rec;
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@id_model", cbModel.SelectedValue);
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

        private void fAddModel_Load(object sender, EventArgs e)
        {
            dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            dblite.Open();
            string query = @"SELECT id,Name FROM DirModels ORDER by Name";
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand m_sqlCmd = new SQLiteCommand(query, dblite);
                SQLiteDataAdapter da = new SQLiteDataAdapter(m_sqlCmd);
                DataSet ds = new DataSet();

                da.Fill(ds);
                dt = ds.Tables[0];

                cbModel.Select();
                cbModel.DataSource = dt.AsDataView();
                cbModel.DisplayMember = "Name";
                cbModel.ValueMember = "id";

                //Если выбран существующий интервал, загружаем параметры
                if (id_rec != "")
                {
                    GetDivision();
                }
                else
                {
                    cbModel.Text = "";
                    tbProd.Text = "";
                    tbProdCat.Text = "";
                    tbProdGRP.Text = "";
                    tbEI.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.21.03", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

        public async void GetDivision()
        {
            try
            {
                string query = @"SELECT dm.Name,dm.KMOD,dm.EI,dp.Name as pName,dpc.category, dpg.GRP FROM Division as div
                               LEFT JOIN DirModels as dm on div.id_model=dm.id 
                               LEFT JOIN DirProducts as dp on dp.id=dm.id_product
                               LEFT JOIN DirProdCat as dpc on dpc.id=dm.id_cat 
                               LEFT JOIN DirProdGRP as dpg on dpg.id=dm.id_grp WHERE div.id =" + id_rec;

                m_sqlCmd = new SQLiteCommand(query, dblite);

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    cbModel.Text = Convert.ToString(sqlReader["Name"]);
                    lbKMOD.Text = Convert.ToString(sqlReader["KMOD"]);
                    tbEI.Text = Convert.ToString(sqlReader["EI"]);
                    tbProd.Text = Convert.ToString(sqlReader["pName"]);
                    tbProdCat.Text = Convert.ToString(sqlReader["category"]);
                    tbProdGRP.Text = Convert.ToString(sqlReader["GRP"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.06.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

        //TODO: Put in a separate class
        private void cbModel_TextUpdate(object sender, EventArgs e)
        {
            DataTable DF = new DataTable();
            DF.Clear();
            string query = @"SELECT Name FROM DirModels WHERE 
                                        Name like '%" + cbModel.Text + @"%'";

            if (this.cbModel.Text != "")
            {
                string st = cbModel.Text;
                cbModel.Items.Clear();
                DF = GetMedData(query);
                if (DF.Rows.Count > 0)
                {
                    for (int i = 0; i < DF.Rows.Count; i++)
                    {
                        cbModel.Items.Add(DF.Rows[i].ItemArray[0].ToString());
                        cbModel.SelectionStart = cbModel.Text.Length;
                    }
                    cbModel.SelectionStart = cbModel.Text.Length;
                    cbModel.DroppedDown = true;
                    cbModel.Text = st;
                }
                else
                {
                    cbModel.Text = st;
                    cbModel.SelectionStart = cbModel.Text.Length;
                    cbModel.Items.Clear();
                }
            }
            cbModel.SelectionStart = cbModel.Text.Length;
        }

        //TODO: Put in a separate class
        public DataTable GetMedData(string query)
        {
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand comm = new SQLiteCommand(query, dblite);
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
