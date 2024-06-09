using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace WorkDivision
{
    public partial class fAddPieceWork : Form
    {
        private SQLiteConnection _dblite;
        private SQLiteDataReader _sqlReader;
        SQLiteCommand m_sqlCmd = null;
        liteDB liteDB = new liteDB();

        public fAddPieceWork()
        {
            InitializeComponent();
        }
        public string id_rec { get; set; }


        private void fEditPieceWork_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            _dblite = liteDB.GetConn();
            _dblite.Open();
            tbMatRate.Text = "";

            string query = @"SELECT ID,tab_nom ||' . ' || FIO as tnFIO FROM DirWorkers ORDER BY FIO";

            try
            {
                SQLiteCommand m_sqlCmd = new SQLiteCommand(query, _dblite);
                SQLiteDataAdapter da = new SQLiteDataAdapter(m_sqlCmd);
                DataSet ds = new DataSet();

                da.Fill(ds);
                DataTable dt = ds.Tables[0];

                cbSelectWorker.DataSource = dt.AsDataView();
                cbSelectWorker.DisplayMember = "tnFIO";
                cbSelectWorker.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.27.03", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            //Если выбрана запись, загружаем параметры
            if (id_rec != "")
            {
                cbSelectOper.Visible = false;
                tbOper.Visible = true;
                tbRank.Text = "";
                tbMatRate.Text = "";
                tbTarif.Text = "";
                tbWorkersCnt.Text = "";
                tbNVR.Text = "";
                tbCost.Text = "";
                tbNVRbyItem.Text = "";
                tbSumItem.Text = "";
                tbCardnum.Text = "";
                tbCount.Text = "";
                lblInDiv.Text = "";
                GetPieceWork(id_rec);
            }
            else
            {
                cbSelectOper.Visible = true;
                tbOper.Visible = false;
                LoadOpersByDivision(Division.id);  //Операции по разделению
                tbRank.Text = "";
                tbMatRate.Text = "";
                tbTarif.Text = "";
                tbWorkersCnt.Text = "";
                tbNVR.Text = "";
                tbCost.Text = "";
                tbNVRbyItem.Text = "";
                tbSumItem.Text = "";
                tbCardnum.Text = "";
                tbCount.Text = "";
                lblInDiv.Text = "";
            }
        }

        //Загрузить операции по разделению
        public void LoadOpersByDivision(string id_div)
        {
            int counter = 0;
            string st_cnt = string.Empty;

            string query = @"SELECT i.id,d.Name
                                FROM inDivision as i 
                                LEFT JOIN DirOpers as d on d.id=i.id_oper
                                WHERE id_division='" + id_div + @"' ORDER BY i.id";

            try
            {
                SQLiteCommand m_sqlCmd = new SQLiteCommand(query, _dblite);
                SQLiteDataAdapter da = new SQLiteDataAdapter(m_sqlCmd);
                DataSet ds = new DataSet();

                da.Fill(ds);
                DataTable dt = ds.Tables[0];


                cbSelectOper.DisplayMember = "Name";
                cbSelectOper.ValueMember = "ID";
                cbSelectOper.DataSource = dt.AsDataView();
                cbSelectOper.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.27.03", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbSelectOper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbSelectOper.SelectedIndex > -1) && (id_rec==""))
            {
                GetOperByDivision(cbSelectOper.SelectedValue.ToString());
            }
        }

        public async void GetOperByDivision(string id_indiv)
        {
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

                                WHERE i.id =" + id_indiv;

                m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    tbRank.Text = Convert.ToString(_sqlReader["rank"]);              //Разряд
                    tbTarif.Text = Convert.ToString(_sqlReader["TAR_VR"]);           //Тарифная ставка
                    tbWorkersCnt.Text = Convert.ToString(_sqlReader["workers_cnt"]); //Кол-во работников
                    tbNVR.Text = Convert.ToString(_sqlReader["NVRforOper"]);                //Норма времени
                    tbMatRate.Text = Convert.ToString(_sqlReader["MatRate"]);               //Расход ткани
                    tbCost.Text = Convert.ToString(_sqlReader["cost"]);              //Расценка на 1м
                    tbNVRbyItem.Text = Convert.ToString(_sqlReader["NVRbyItem"]);     //Норма времени на 1ед
                    tbSumItem.Text = Convert.ToString(_sqlReader["SumItem"]);       //Стоимость 1ед
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

        private void btSave_Click(object sender, EventArgs e)
        {
            if (((cbSelectOper.SelectedIndex == -1) && (cbSelectOper.Visible)) || (cbSelectWorker.SelectedIndex == -1))
            {
                MessageBox.Show("Не все обязательные поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (id_rec == "")  //Если  id записи не передан (новая запись) 
                {

                    string querySQLite = @"INSERT INTO PieceWork (id_worker,id_indivision,id_division,cardnum,cnt,mm,yy) 
                VALUES (@id_worker,@id_indivision,@id_division,@cardnum,@cnt,@mm,@yy)";
                    m_sqlCmd = new SQLiteCommand(querySQLite, _dblite);
                    m_sqlCmd.Parameters.AddWithValue("id_worker", cbSelectWorker.SelectedValue.ToString());
                    m_sqlCmd.Parameters.AddWithValue("id_indivision", cbSelectOper.SelectedValue.ToString());
                    m_sqlCmd.Parameters.AddWithValue("id_division", Division.id);
                    m_sqlCmd.Parameters.AddWithValue("cardnum", tbCardnum.Text);
                    m_sqlCmd.Parameters.AddWithValue("cnt", tbCount.Text.Replace(",", "."));
                    m_sqlCmd.Parameters.AddWithValue("mm", Division.mm);
                    m_sqlCmd.Parameters.AddWithValue("yy", Division.yy);


                }
                else
                {
                    string querySQLite = @"UPDATE PieceWork SET id_worker=@id_worker,id_indivision=@id_indivision,
                id_division=@id_division,cardnum=@cardnum,cnt=@cnt,mm=@mm,yy=@yy WHERE id=" + id_rec;
                    m_sqlCmd = new SQLiteCommand(querySQLite, _dblite);
                    m_sqlCmd.Parameters.AddWithValue("id_worker", cbSelectWorker.SelectedValue.ToString());
                    m_sqlCmd.Parameters.AddWithValue("id_indivision", lblInDiv.Text);
                    m_sqlCmd.Parameters.AddWithValue("id_division", Division.id);
                    m_sqlCmd.Parameters.AddWithValue("cardnum", tbCardnum.Text);
                    m_sqlCmd.Parameters.AddWithValue("cnt", tbCount.Text.Replace(",", "."));
                    m_sqlCmd.Parameters.AddWithValue("mm", Division.mm);
                    m_sqlCmd.Parameters.AddWithValue("yy", Division.yy);
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

        }

        public async void GetPieceWork(string id_rec)
        {
            try
            {
                string query = @"SELECT pw.id_indivision,i.id_oper,d.Name as OperName,d.PER,i.rank,i.MatRate,dt.TAR_VR,i.NVRforOper,i.workers_cnt,parent,
                                IFNULL(ROUND(i.NVRforOper * dt.TAR_VR,5),0) as Cost,
                                CASE WHEN d.UCH between 1 and 2 THEN IFNULL(ROUND(i.NVRforOper * i.MatRate * i.workers_cnt,2),0) ELSE NVRforOper END as NVRbyItem,
                                CASE WHEN d.UCH between 1 and 2 THEN IFNULL(ROUND(ROUND(i.NVRforOper * dt.TAR_VR,5) * i.MatRate * i.workers_cnt,5),0) 
                                    ELSE IFNULL(ROUND(i.NVRforOper * dt.TAR_VR * i.workers_cnt,5),0) END as SumItem,
                                pw.cnt,pw.cardnum, dw.tab_nom || ' . '||dw.FIO as tabFIO
                                FROM PieceWork as pw
                                LEFT JOIN inDivision as i on pw.id_indivision=i.id 
                                LEFT JOIN DirOpers as d on d.id=i.id_oper
                                LEFT JOIN DirTarif as dt on dt.rank=i.rank
                                LEFT JOIN DirWorkers as dw on pw.id_worker=dw.id
                                WHERE pw.id =" + id_rec;

                m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    lblInDiv.Text = Convert.ToString(_sqlReader["id_indivision"]);
                    tbCardnum.Text = Convert.ToString(_sqlReader["cardnum"]);
                    tbCount.Text = Convert.ToString(_sqlReader["cnt"]);
                    tbRank.Text = Convert.ToString(_sqlReader["rank"]);              //Разряд
                    tbTarif.Text = Convert.ToString(_sqlReader["TAR_VR"]);           //Тарифная ставка
                    tbWorkersCnt.Text = Convert.ToString(_sqlReader["workers_cnt"]); //Кол-во работников
                    tbNVR.Text = Convert.ToString(_sqlReader["NVRforOper"]);                //Норма времени
                    tbMatRate.Text = Convert.ToString(_sqlReader["MatRate"]);               //Расход ткани
                    tbCost.Text = Convert.ToString(_sqlReader["cost"]);              //Расценка на 1м
                    tbNVRbyItem.Text = Convert.ToString(_sqlReader["NVRbyItem"]);     //Норма времени на 1ед
                    tbSumItem.Text = Convert.ToString(_sqlReader["SumItem"]);       //Стоимость 1ед
                    tbOper.Text = Convert.ToString(_sqlReader["OperName"]);
                    cbSelectWorker.Text = Convert.ToString(_sqlReader["tabFIO"]);
                    cbSelectOper.Text = Convert.ToString(_sqlReader["OperName"]);

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

        private void tbCount_KeyPress(object sender, KeyPressEventArgs e)
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
                if (tbCount.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void tbCardnum_KeyPress(object sender, KeyPressEventArgs e)
        {

            // ограничение ввода
            if ((e.KeyChar < '0' | e.KeyChar > '9' && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Escape && e.KeyChar != (char)Keys.Delete) || (e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }
    }
}
