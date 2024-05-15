using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WorkDivision
{
	public partial class fEditPieceWork : Form
    {
        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        SQLiteCommand m_sqlCmd = null;
        liteDB liteDB = new liteDB();

        public fEditPieceWork()
        {
            InitializeComponent();           
        }
        public string id_rec { get; set; }
        public string rank { get; set; }
        public string ord { get; set; }


        private void fAddOper_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
            dblite.Open();
            tbMatRate.Text = "";
            cbSelectMat.Text = "";
            //Если выбрана операция, загружаем параметры
            if (id_rec != "")
            {
                
                //cbRank.Text = rank;        //Выбираем пустое значение для списка разрядов 
                GetOperByDivision(id_rec);
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
        public async void GetOperByDivision(string id_rec)
        {
            string matRate = "";
            int parent=0;
            int id_oper = 0;
            try
            {
                string query = @"SELECT i.id,d.UCH,i.id_oper,d.Name,d.PER,i.rank,i.MatRate,dt.TAR_VR,i.NVRforOper,i.workers_cnt,parent,
                                IFNULL(ROUND(i.NVRforOper * dt.TAR_VR,5),0) as Cost,
                                CASE WHEN d.UCH between 1 and 2 THEN IFNULL(ROUND(i.NVRforOper * i.MatRate * i.workers_cnt,2),0) ELSE NVRforOper END as NVRbyItem,
                                CASE WHEN d.UCH between 1 and 2 THEN IFNULL(ROUND(ROUND(i.NVRforOper * dt.TAR_VR,5) * i.MatRate * i.workers_cnt,5),0) 
                                    ELSE IFNULL(ROUND(i.NVRforOper * dt.TAR_VR * i.workers_cnt,5),0) END as SumItem
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
                    id_oper = Convert.ToInt32(sqlReader["id_oper"]);                //id операции
                    tbOperName.Text = Convert.ToString(sqlReader["Name"]);          //Название операции
                    rank = Convert.ToString(sqlReader["rank"]);              //Разряд
                    tbTarif.Text = Convert.ToString(sqlReader["TAR_VR"]);           //Тарифная ставка
                    tbWorkersCnt.Text = Convert.ToString(sqlReader["workers_cnt"]); //Кол-во работников
                    tbNVR.Text = Convert.ToString(sqlReader["NVRforOper"]);                //Норма времени
                    matRate = Convert.ToString(sqlReader["MatRate"]);               //Расход ткани
                    tbCost.Text = Convert.ToString(sqlReader["cost"]);              //Расценка на 1м
                    tbNVRbyItem.Text = Convert.ToString(sqlReader["NVRbyItem"]);     //Норма времени на 1ед
                    tbSumItem.Text = Convert.ToString(sqlReader["SumItem"]);       //Стоимость 1ед
                    parent = Convert.ToInt32(sqlReader["parent"]);                //Родитель
                    ord = Convert.ToString(sqlReader["PER"]);                      //очередь в списке операций
                    //lbNumber.Text = number.ToString();     //порядковый номер операции в разделении
                }
                //Если участок контроля или настилания
                //Заполнение выпадающего списка видами ткани из БД 
                if ((Convert.ToInt32(lbUCH.Text) <= 2) && (Convert.ToInt32(lbUCH.Text) > 0))
                {
                    //Если участок 1 или 2, включаем выбор ткани и ввод расхода
                    cbSelectMat.Enabled = true;
                    tbMatRate.Enabled = true;
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
                        tbMatRate.Text = matRate;         //Расход материала из БД
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
                if (parent == id_oper)
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

        //Нажатие Enter на списке материалов
        private void cbSelectMat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbMatRate.Focus();
            }
        }

        //Нажатие Enter на списке разрядов
        private void cbRank_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbNVR.Focus();
            }
        }

        //Нажатие Enter на поле Расход материала
        private void tbMatRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbRank.Focus();
            }
        }
        //Нажатие Enter на поле Норма времени
        private void tbNVR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbWorkersCnt.Focus();
            }
        }

        //Нажатие Enter на поле Кол-во работников
        private void tbWorkersCnt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btCalculate.Focus();
            }
        }

        //Нажатие Enter на кнопке Пересчитать
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

            //Ввод только 1 разделителя
            if (e.KeyChar == ',')
            {
                if (tbNVR.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
            }

        }

        private void btCalculate_Click(object sender, EventArgs e)
        {
            double.TryParse(tbNVR.Text, out double NVRforOper);             //Норма времени
            double.TryParse(tbTarif.Text, out double tarif);         //Тарифная ставка
            double.TryParse(tbMatRate.Text, out double MatRate);     //расход ткани
            int.TryParse(tbWorkersCnt.Text, out int wcount);   //кол-во рабоников

            tbCost.Text = Convert.ToString(Math.Round(NVRforOper * tarif,5));   //Расценка

            if (Convert.ToInt32(lbUCH.Text) > 2)
            {
                tbNVRbyItem.Text = NVRforOper.ToString();          //Норма времени на ед.
                tbSumItem.Text = Convert.ToString(Convert.ToDouble(tbCost.Text)*wcount);
            }
            else 
            {
                tbNVRbyItem.Text = Convert.ToString(Math.Round(NVRforOper * MatRate*wcount,2));   //Норма времени на ед.
                tbSumItem.Text = Convert.ToString(Math.Round(Convert.ToDouble(tbCost.Text) * MatRate * wcount,5));   //Норма времени на ед.

            }

            //ЗАПИСЬ В БД
            string querySQLite = @"UPDATE inDivision SET rank=@rank, MatRate=@MatRate, NVRforOper=@NVRforOper, workers_cnt=@workers_cnt WHERE id=" + id_rec;
            m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
            m_sqlCmd.Parameters.AddWithValue("@rank", cbRank.Text);
            m_sqlCmd.Parameters.AddWithValue("@NVRforOper", NVRforOper);
            m_sqlCmd.Parameters.AddWithValue("@workers_cnt", wcount);
            m_sqlCmd.Parameters.AddWithValue("@MatRate", MatRate);
        
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

            //Ввод только 1 разделителя
            if (e.KeyChar == ',')
            {
                if (tbMatRate.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void tbWorkersCnt_KeyPress(object sender, KeyPressEventArgs e)
        {

            // ограничение ввода
            if ((e.KeyChar < '0' | e.KeyChar > '9' && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Escape && e.KeyChar != (char)Keys.Delete) || (e.KeyChar == '.'))
            {
                e.Handled = true;
            }

        }

        //Следующая операция
        private void btnNext_Click(object sender, EventArgs e)
        {
            string query = @"SELECT i.id FROM inDivision as i
                            LEFT JOIN DirOpers as d on d.id=i.id_oper
                            WHERE i.id >" + id_rec + @" AND i.id_division="+Division.id+@" ORDER BY i.id ASC LIMIT 1";
            m_sqlCmd = new SQLiteCommand(query, dblite);

            //ID записи в разделении
            id_rec = Convert.ToString(m_sqlCmd.ExecuteScalar());
            if (id_rec == "")
            {
                this.Close();
            }
            else
            {
                GetOperByDivision(id_rec);
            }
        }

        //Предыдущая операция
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            string query = @"SELECT i.id FROM inDivision as i
                            LEFT JOIN DirOpers as d on d.id=i.id_oper
                            WHERE i.id < " + id_rec + @" AND i.id_division=" + Division.id+@" ORDER BY i.id DESC LIMIT 1";
            m_sqlCmd = new SQLiteCommand(query, dblite);

            //ID записи в разделении
            id_rec = Convert.ToString(m_sqlCmd.ExecuteScalar());
            if (id_rec == "")
            {
                this.Close();
            }
            else
            {
                GetOperByDivision(id_rec);
            }
        }
    }
}
