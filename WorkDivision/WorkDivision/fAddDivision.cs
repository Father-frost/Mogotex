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
        liteDB liteDB = new liteDB();
        
        //TODO: Выбор категорий по выбору Модели
        //
        public fAddDivision()
        {
            InitializeComponent();
        }

    
        public string id_rec { get; set; }  //Свойство для идентификатора записи в БД
        public string id_model { get; set; }  //id модели в справочнике моделей
        public string dt { get; set; } //Дата с датапикера на главной форме


        //Сохранить изменения
        private void button1_Click(object sender, EventArgs e)
        {
            if (id_rec == "")  //Если  id записи не передан (новая запись) 
            {
                
                string querySQLite = @"INSERT INTO Division (id_model,dt,mm,yy) VALUES (@id_model,@dt,@mm,@yy)";
                m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
                m_sqlCmd.Parameters.AddWithValue("@id_model", cbModel.SelectedValue);
                m_sqlCmd.Parameters.AddWithValue("@dt", dt);
                m_sqlCmd.Parameters.AddWithValue("@mm", Convert.ToDateTime(dt).Month.ToString());
                m_sqlCmd.Parameters.AddWithValue("@yy", Convert.ToDateTime(dt).Year.ToString());
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

        private void fAddDivision_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
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
                    cbModel.SelectedValue = id_model;
                }
                else
                {
                    cbModel.SelectedIndex = -1;
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




        public async void GetModelParams(string id_model)
        {
            try
            {
                string query = @"SELECT dm.Name,dm.KMOD,dm.EI,dp.Name as pName,dpc.category, dpg.GRP FROM DirModels as dm
                               LEFT JOIN DirProducts as dp on dp.id=dm.id_product
                               LEFT JOIN DirProdCat as dpc on dpc.id=dm.id_cat 
                               LEFT JOIN DirProdGRP as dpg on dpg.id=dm.id_grp WHERE dm.id =" + id_model;

                m_sqlCmd = new SQLiteCommand(query, dblite);

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    lbKMOD.Text = Convert.ToString(sqlReader["KMOD"]);
                    tbEI.Text = Convert.ToString(sqlReader["EI"]);
                    tbProd.Text = Convert.ToString(sqlReader["pName"]);
                    tbProdCat.Text = Convert.ToString(sqlReader["category"]);
                    tbProdGRP.Text = Convert.ToString(sqlReader["GRP"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.06.08", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

        //Изменение значений в связанных полях при выборе Модели
        private void cbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Если что-то выбрано
            if (cbModel.SelectedIndex > 0)
            {
                if (cbModel.SelectedValue == null)  //id модели
                {
                    tbProd.Text = string.Empty;
                    tbProdCat.Text = string.Empty;
                    tbProdGRP.Text = string.Empty;

                }
                else
                {
                    //MessageBox.Show(cbModel.SelectedValue.ToString());
                    GetModelParams(cbModel.SelectedValue.ToString());
                }
            }
        }
    }
}
