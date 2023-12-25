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
using System.Net.Security;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

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
            //dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            //dblite.Open();
            //string querySQLite = @"UPDATE DirOpers SET Name=@Name, UCH=@UCH, PER=@PER, NST=@NST, KOEF=@KOEF, NVR=@NVR WHERE id=" + id_rec;
            //m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
            //m_sqlCmd.Parameters.AddWithValue("@Name", tbOperName.Text);
            //m_sqlCmd.Parameters.AddWithValue("@UCH", tbMatRate.Text);
            //m_sqlCmd.Parameters.AddWithValue("@PER", tbTarif.Text);
            //m_sqlCmd.Parameters.AddWithValue("@NST", tbWorkersCnt.Text);
            //m_sqlCmd.Parameters.AddWithValue("@KOEF", tbNVR.Text.Replace(",", "."));
            //m_sqlCmd.Parameters.AddWithValue("@NVR", tbCost.Text.Replace(",","."));
            //try
            //{
            //    m_sqlCmd.Prepare();
            //    m_sqlCmd.ExecuteNonQuery();
            //    this.Close();

            //}
            //catch (SQLiteException ex)
            //{
            //    MessageBox.Show(ex.Message, "Ошибка 5.21.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        public string id_rec { get; set; }
        public string number { get; set; }
        public string rank { get; set; }

        private void fAddOper_Load(object sender, EventArgs e)
        {
            
            dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            dblite.Open();
            tbMatRate.Text = "";
            cbSelectMat.Text = "";
            //Если выбрана операция, загружаем параметры
            if (id_rec != "")
            {
                
                cbRank.Text = rank;        //Выбираем пустое значение для списка разрядов 
                lbNumber.Text = number;
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

        //Загрузчик параметров операции
        public async void GetOperByDivision()
        {
            string matRate = "";
            int parent=1;
            try
            {
                string query = @"SELECT i.id,d.UCH,i.id_oper,d.Name,i.rank,i.MatRate,dt.TAR_VR,i.NVRforOper,i.workers_cnt,parent
                                FROM inDivision as i 
                                LEFT JOIN DirOpers as d on d.id=i.id_oper
                                LEFT JOIN DirTarif as dt on dt.rank=i.rank

                                WHERE i.id =" + id_rec;

                m_sqlCmd = new SQLiteCommand(query, dblite);
                //m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    lbUCH.Text = Convert.ToString(sqlReader["UCH"]);                //Номер участка
                    tbOperName.Text = Convert.ToString(sqlReader["Name"]);          //Название операции
                    cbRank.Text = Convert.ToString(sqlReader["rank"]);              //Разряд
                    tbTarif.Text = Convert.ToString(sqlReader["TAR_VR"]);           //Тарифная ставка
                    tbWorkersCnt.Text = Convert.ToString(sqlReader["workers_cnt"]); //Кол-во работников
                    tbNVR.Text = Convert.ToString(sqlReader["NVRforOper"]);                //Норма времени
                    matRate = Convert.ToString(sqlReader["MatRate"]);               //Расход ткани
                    tbCost.Text = "";                                               //Расценка на 1м
                    tbNVRbyItem.Text = "";                                          //Норма времени на 1ед
                    tbSumItem.Text = "";                                            //Стоимость 1ед
                    parent = Convert.ToInt32(sqlReader["parent"]);                //Родитель
                }
                //Если участок контроля или настилания
                //Заполнение выпадающего списка видами ткани из БД 
                if ((Convert.ToInt32(lbUCH.Text) <= 2) && (Convert.ToInt32(lbUCH.Text) > 0))
                {
                    if (Convert.ToInt32(lbUCH.Text) == 1)
                    {
                        query = @"SELECT NORMVR,VIDTK FROM DirNormControl ORDER BY VIDTK";
                    }
                    else if (Convert.ToInt32(lbUCH.Text) == 2)
                    {
                        query = @"SELECT NORMVR,VIDTK FROM DirNormNastil ORDER BY VIDTK";
                    }
                    try
                    {
                        SQLiteCommand m_sqlCmd = new SQLiteCommand(query, dblite);
                        SQLiteDataAdapter da = new SQLiteDataAdapter(m_sqlCmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        cbSelectMat.Select();
                        cbSelectMat.DataSource = dt.AsDataView();
                        cbSelectMat.DisplayMember = "VIDTK";
                        cbSelectMat.ValueMember = "NORMVR";
                        cbSelectMat.SelectedIndex = -1;   //Выбираем пустое значение для списка материалов
                        tbMatRate.Text = matRate;
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
                else
                {
                    cbSelectMat.Enabled = false;
                    tbMatRate.Enabled = false;
                    tbMatRate.Text = "";
                    cbRank.Select();
                }

                //Заполнение списка Разрядов 
                query = @"SELECT rank,TAR_VR FROM DirTarif ORDER BY rank";
                    
                try
                {
                    SQLiteCommand m_sqlCmd = new SQLiteCommand(query, dblite);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(m_sqlCmd);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];

                    //cbRank.Select();
                    cbRank.DataSource = dt.AsDataView();
                    cbRank.DisplayMember = "rank";
                    cbRank.ValueMember = "TAR_VR";
                    if (Convert.ToInt32(rank) == 0)
                    {
                        cbRank.SelectedIndex = -1;
                        tbTarif.Text = "0";
                    }
                    else
                    {
                        cbRank.Text = rank;
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

                //Если родительская операция, скрываем поля 
                if (parent == 1)
                {
                    lbUCH.Enabled = false;
                    //tbOperName.Enabled = false;
                    cbRank.Enabled = false;
                    tbWorkersCnt.Enabled = false;
                    tbNVR.Enabled = false;
                }
                else
                {
                    lbUCH.Enabled = true;
                    //tbOperName.Enabled = true;
                    cbRank.Enabled = true;
                    tbWorkersCnt.Enabled = true;
                    tbNVR.Enabled = true;
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

        private void cbSelectMat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSelectMat.SelectedValue != null)
            {
                tbMatRate.Text = cbSelectMat.SelectedValue.ToString();
                tbMatRate.Focus();
            }

        }

        private void cbRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRank.SelectedValue == null)
            {
                tbTarif.Text = "";
            }
            else
            {
                tbTarif.Text = cbRank.SelectedValue.ToString();
            }
        }

        private void cbSelectMat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbMatRate.Focus();
            }
        }

        private void cbRank_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbNVR.Focus();
            }
        }

        private void tbMatRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbRank.Focus();
            }
        }

        private void tbNVR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbWorkersCnt.Focus();
            }
        }

        private void tbWorkersCnt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btCalculate.Focus();
            }
        }

        private void btCalculate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnNext.Focus();
            }
        }

        //Настройка ввода для Нормы времени
        private void tbNVR_KeyPress(object sender, KeyPressEventArgs e)
        {
            // конвертирование запятой в точку
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // ограничение ввода
            if (e.KeyChar < '0' | e.KeyChar > '9' && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Escape && e.KeyChar != (char)Keys.Delete && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // запрет таких значений как 03.xx 09999.xx После 0 должа быть точка. 
            //if (tbNVR.Text == "0")
            //{
            //    if (e.KeyChar != ',' & e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            //    {
            //        e.Handled = true;
            //    }
            //}

            //Ввод только 1 разделителя
            if (e.KeyChar == ',')
            {
                if (tbNVR.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
            }

            // запрет на ввод таких значений как "97.9800", "0.33333."
            // Должно быть "8.300", "5.009"
            if (tbNVR.Text.IndexOf(',') > 0)
            {
                if (tbNVR.Text.Substring(tbNVR.Text.IndexOf(',')).Length > 2)
                {
                    if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void btCalculate_Click(object sender, EventArgs e)
        {
            double.TryParse(tbNVR.Text, out double NVRforOper);             //Норма времени
            double.TryParse(tbTarif.Text, out double tarif);         //Тарифная ставка
            double.TryParse(tbMatRate.Text, out double MatRate);     //расход ткани
            double.TryParse(tbWorkersCnt.Text, out double wcount);   //кол-во рабоников

            tbCost.Text = Convert.ToString(Math.Round(NVRforOper * tarif,5));   //Расценка

            if (Convert.ToInt32(lbUCH.Text) > 2)
            {
                tbNVRbyItem.Text = NVRforOper.ToString();          //Норма времени на ед.
                tbSumItem.Text = Convert.ToString(Convert.ToDouble(tbCost.Text)*wcount);
            }
            else 
            {
                tbNVRbyItem.Text = Convert.ToString(Math.Round(NVRforOper * MatRate,2)*wcount);   //Норма времени на ед.
                tbSumItem.Text = Convert.ToString(Math.Round(Convert.ToDouble(tbCost.Text) * MatRate * wcount,5));   //Норма времени на ед.

            }

            //ЗАПИСЬ В БД
            string querySQLite = @"UPDATE inDivision SET rank=@rank, MatRate=@MatRate, NVRforOper=@NVRforOper, workers_cnt=@workers_cnt WHERE id=" + id_rec;
            m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
            m_sqlCmd.Parameters.AddWithValue("@rank", rank);
            m_sqlCmd.Parameters.AddWithValue("@NVRforOper", NVRforOper);
            m_sqlCmd.Parameters.AddWithValue("@workers_cnt", wcount);
            m_sqlCmd.Parameters.AddWithValue("@MatRate", MatRate);
           // m_sqlCmd.Parameters.AddWithValue("@NST", tbNST.Text);
           // m_sqlCmd.Parameters.AddWithValue("@KOEF", tbKOEF.Text.Replace(",", "."));
            //m_sqlCmd.Parameters.AddWithValue("@NVR", tbNVR.Text.Replace(",", "."));
        
            try
            {
                m_sqlCmd.Prepare();
                m_sqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.21.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void tbMatRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            // конвертирование запятой в точку
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // ограничение ввода
            if (e.KeyChar < '0' | e.KeyChar > '9' && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Escape && e.KeyChar != (char)Keys.Delete && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // запрет таких значений как 03.xx 09999.xx После 0 должа быть точка. 
            //if (tbMatRate.Text == "0")
            //{
            //    if (e.KeyChar != ',' & e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            //    {
            //        e.Handled = true;
            //    }
            //}

            //Ввод только 1 разделителя
            if (e.KeyChar == ',')
            {
                if (tbMatRate.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
            }

            // запрет на ввод таких значений как "97.9800", "0.33333."
            // Должно быть "8.300", "5.009"
            if (tbMatRate.Text.IndexOf(',') > 0)
            {
                if (tbMatRate.Text.Substring(tbMatRate.Text.IndexOf(',')).Length > 2)
                {
                    if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void tbWorkersCnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // конвертирование запятой в точку
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // ограничение ввода
            if (e.KeyChar < '0' | e.KeyChar > '9' && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Escape && e.KeyChar != (char)Keys.Delete && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // запрет таких значений как 03.xx 09999.xx После 0 должа быть точка. 
            //if (tbWorkersCnt.Text == "0")
            //{
            //    if (e.KeyChar != ',' & e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            //    {
            //        e.Handled = true;
            //    }
            //}

            //Ввод только 1 разделителя
            if (e.KeyChar == ',')
            {
                if (tbWorkersCnt.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
            }

            // запрет на ввод таких значений как "97.9800", "0.33333."
            // Должно быть "8.300", "5.009"
            if (tbWorkersCnt.Text.IndexOf(',') > 0)
            {
                if (tbWorkersCnt.Text.Substring(tbWorkersCnt.Text.IndexOf(',')).Length > 2)
                {
                    if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
