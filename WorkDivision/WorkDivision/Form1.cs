using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
//using Settings2Ini;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;
using System.Collections;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using View = System.Windows.Forms.View;
using Font = System.Drawing.Font;
using System.Data.SqlClient;

namespace WorkDivision
{
    public partial class Form1 : Form
    {
        private SQLiteConnection dblite;
        private SQLiteCommand m_sqlCmd;
        private SQLiteDataReader sqlReader;
        private liteDB liteDB;

        public fAddWorker fAddWorker;
        public fAddBrig fAddBrig;
        public fAddProf fAddProf;
        public fAddOper fAddOper;
        public fAddModel fAddModel;
        public fAddProd fAddProd;
        public fAddTarif fAddTarif;
        public fAddNormNastil fAddNormNastil;
        public fAddNormControl fAddNormControl;
        public fAddDivision fAddDivision;
        public fOpersList fOpersList;
        public fEditOperByDivision fEditOperByDivision;
        public fAddSigner fAddSigner;
        //public fAuth fAuth;
        Word._Application oWord = new Word.Application();

        public Form1()
        {
            InitializeComponent();

            fAddWorker = new fAddWorker();
            fAddBrig = new fAddBrig();
            fAddProf = new fAddProf();
            fAddOper = new fAddOper();
            fAddModel = new fAddModel();
            fAddProd = new fAddProd();
            fAddTarif = new fAddTarif();
            fAddNormNastil = new fAddNormNastil();
            fAddNormControl= new fAddNormControl();
            fAddDivision= new fAddDivision();
            fOpersList = new fOpersList();
            fEditOperByDivision = new fEditOperByDivision();
            fAddSigner = new fAddSigner();
            liteDB = new liteDB();

        }
        

        // авторасширение колонок listView по размеру содержимого
        public static void autoResizeColumns(ListView lv)
        {
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListView.ColumnHeaderCollection cc = lv.Columns;
            for (int i = 0; i < cc.Count; i++)
            {
                
                int colWidth = TextRenderer.MeasureText(cc[i].Text, lv.Font).Width + 10;
                if (i>0)
                {
                    if (colWidth > cc[i].Width)
                    {
                        cc[i].Width = colWidth;
                    }
                }
                else
                {
                    cc[i].Width = 0;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
            dblite.Open();

            tpinDivision.Parent = null;  //Скрываем вкладку 
            //Настройка даты
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Value = DateTime.Now;

            //------ОТРИСОВКА ЗАГОЛОВКОВ
            //Справочник работников
            lvDirWorkers.GridLines = true;
            lvDirWorkers.FullRowSelect = true;
            lvDirWorkers.View = View.Details;
            lvDirWorkers.Font = new Font(lvDirWorkers.Font, FontStyle.Bold);
            lvDirWorkers.Columns.Add("ID");
            lvDirWorkers.Columns.Add("Таб.№");
            lvDirWorkers.Columns.Add("ФИО");
            lvDirWorkers.Columns.Add("Разряд");
            lvDirWorkers.Columns.Add("Профессия");
            lvDirWorkers.Columns.Add("KO");
            lvDirWorkers.Columns.Add("Номер бригады");
            lvDirWorkers.Columns.Add("Название бригады");

            //Справочник бригад
            lvDirBrigs.GridLines = true;
            lvDirBrigs.FullRowSelect = true;
            lvDirBrigs.View = View.Details;
            lvDirBrigs.Font = new Font(lvDirBrigs.Font, FontStyle.Bold);
            lvDirBrigs.Columns.Add("ID");
            lvDirBrigs.Columns.Add("Номер");
            lvDirBrigs.Columns.Add("Название бригады");
            lvDirBrigs.Columns.Add("NUMK");

            //Справочник профессий
            lvDirProfs.GridLines = true;
            lvDirProfs.FullRowSelect = true;
            lvDirProfs.View = View.Details;
            lvDirProfs.Font = new Font(lvDirProfs.Font, FontStyle.Bold);
            lvDirProfs.Columns.Add("ID");
            lvDirProfs.Columns.Add("Код профессии");
            lvDirProfs.Columns.Add("Наименование");
            lvDirProfs.Columns.Add("PR");

            //Справочник операций
            lvDirOpers.GridLines = true;
            lvDirOpers.FullRowSelect = true;
            lvDirOpers.View = View.Details;
            lvDirOpers.Font = new Font(lvDirOpers.Font, FontStyle.Bold);
            lvDirOpers.Columns.Add("id");
            lvDirOpers.Columns.Add("Участок");
            lvDirOpers.Columns.Add("Переход");
            lvDirOpers.Columns.Add("Наименование");
            lvDirOpers.Columns.Add("Коэф.");
            lvDirOpers.Columns.Add("Норма времени, сек");
            
            //Справочник продукции
            lvDirProducts.GridLines = true;
            lvDirProducts.FullRowSelect = true;
            lvDirProducts.View = View.Details;
            lvDirProducts.Font = new Font(lvDirProducts.Font, FontStyle.Bold);
            lvDirProducts.Columns.Add("id");
            lvDirProducts.Columns.Add("Наименование");
            
            //Справочник тарифных ставок
            lvDirTarif.GridLines = true;
            lvDirTarif.FullRowSelect = true;
            lvDirTarif.View = View.Details;
            lvDirTarif.Font = new Font(lvDirTarif.Font, FontStyle.Bold);
            lvDirTarif.Columns.Add("id");
            lvDirTarif.Columns.Add("Разряд");
            lvDirTarif.Columns.Add("Тар.ставка");
            lvDirTarif.Columns.Add("Коэф.сдел."); 
            
            //Справочник норм на настил
            lvDirNormNastil.GridLines = true;
            lvDirNormNastil.FullRowSelect = true;
            lvDirNormNastil.View = View.Details;
            lvDirNormNastil.Font = new Font(lvDirNormNastil.Font, FontStyle.Bold);
            lvDirNormNastil.Columns.Add("id");
            lvDirNormNastil.Columns.Add("Вид ткани");
            lvDirNormNastil.Columns.Add("Затрата врем. на 1м, сек");

            //Справочник норм на контроль
            lvDirNormControl.GridLines = true;
            lvDirNormControl.FullRowSelect = true;
            lvDirNormControl.View = View.Details;
            lvDirNormControl.Font = new Font(lvDirNormControl.Font, FontStyle.Bold);
            lvDirNormControl.Columns.Add("id");
            lvDirNormControl.Columns.Add("Вид ткани");
            lvDirNormControl.Columns.Add("Затрата врем. на 1м, сек");

            //Справочник подписантов
            lvDirSigners.GridLines = true;
            lvDirSigners.FullRowSelect = true;
            lvDirSigners.View = View.Details;
            lvDirSigners.Font = new Font(lvDirSigners.Font, FontStyle.Bold);
            lvDirSigners.Columns.Add("id");
            lvDirSigners.Columns.Add("Должность");
            lvDirSigners.Columns.Add("ФИО");
            lvDirSigners.Columns.Add("Порядок");

            //Справочник моделей
            lvDirModels.GridLines = true;
            lvDirModels.FullRowSelect = true;
            lvDirModels.View = View.Details;
            lvDirModels.Font = new Font(lvDirModels.Font, FontStyle.Bold);
            lvDirModels.Columns.Add("id");
            lvDirModels.Columns.Add("КОД");
            lvDirModels.Columns.Add("Название модели");
            lvDirModels.Columns.Add("Вид изделия");
            lvDirModels.Columns.Add("Категория 1");
            lvDirModels.Columns.Add("Категория 2");
            lvDirModels.Columns.Add("Ед.изм.");

            //Разделения
            lvDivision.GridLines = true;
            lvDivision.FullRowSelect = true;
            lvDivision.View = View.Details;
            lvDivision.Font = new Font(lvDivision.Font, FontStyle.Bold);
            lvDivision.Columns.Add("id");
            lvDivision.Columns.Add("Модель");
            lvDivision.Columns.Add("Вид изделия");
            lvDivision.Columns.Add("Время обработки");
            lvDivision.Columns.Add("Стоимость обработки");
            //autoResizeColumns(lvDivision);

            //Детализация разделения
            lvinDivision.GridLines = true;
            lvinDivision.FullRowSelect = true;
            lvinDivision.View = View.Details;
            lvinDivision.Font = new Font(lvinDivision.Font, FontStyle.Bold);
            lvinDivision.Columns.Add("id");
            lvinDivision.Columns.Add("Участок");
            lvinDivision.Columns.Add("№ пп");
            lvinDivision.Columns.Add("Операция");
            lvinDivision.Columns.Add("Расход ткани");
            lvinDivision.Columns.Add("Разряд");
            lvinDivision.Columns.Add("Норма времени");
            lvinDivision.Columns.Add("Кол-во раб.");
            lvinDivision.Columns.Add("Расценка на 1м");
            lvinDivision.Columns.Add("Норма врем на 1ед.");
            lvinDivision.Columns.Add("Стоимость 1ед.");
            autoResizeColumns(lvinDivision);


            dateTimePicker1_ValueChanged(this, null);

            tabControl1.SelectedIndex = 0;
            tsStatusNVRbyItem.Text = "";
            tsStatusSumItem.Text = "";
        }

        //Изменение даты
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            lvDivision.Items.Clear();  //Чистим listview2

            try
            {
               LoadDivisions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.00.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDivision.Items.Count;

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpDivision)
            {
                dateTimePicker1_ValueChanged(this, null);
                if (lvDivision.Items.Count == 0)
                {
                    toolStripStatusLabel1.Text = "Нет разделений за выбранный месяц.";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Кол-во строк: "+ lvDivision.Items.Count;
                }
                tsStatusNVRbyItem.Text = "";
                tsStatusSumItem.Text = "";
            }
            if (tabControl1.SelectedTab == tpDirs)  //Если выбрана закладка справочники
            {
                if (tabControl2.SelectedTab == tpDirWorkers)
                {
                    LoadDirWorkers();
                }
                tsStatusNVRbyItem.Text = "";
                tsStatusSumItem.Text = "";
            }

            
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab == tpDirWorkers)
            {
                LoadDirWorkers();
            }
            if (tabControl2.SelectedTab == tpDirBrigs)
            {
                LoadDirBrigs();
            }
            if (tabControl2.SelectedTab == tpDirProfs)
            {
                LoadDirProfs();
            }
            if (tabControl2.SelectedTab == tpDirOpers)
            {
                LoadDirOpers();
            }
            if (tabControl2.SelectedTab == tpDirModels)
            {
                LoadDirModels();
            }
            if (tabControl2.SelectedTab == tpDirProducts)
            {
                LoadDirProducts();
            }
            if (tabControl2.SelectedTab == tpDirTarif)
            {
                LoadDirTarif();
            }            
            if (tabControl2.SelectedTab == tpDirNormNastil)
            {
                LoadDirNormNastil();
            }            
            if (tabControl2.SelectedTab == tpDirNormControl)
            {
                LoadDirNormControl();
            }

        }
        public async void LoadDirWorkers()
        {
            lvDirWorkers.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT dw.id,dw.tab_nom,dw.FIO,dw.rank,dw.KO,dbr.KODBR,dbr.Name,pr.Name as prof
                            FROM DirWorkers as dw
                            LEFT JOIN DirBrigs as dbr on dw.brig_id = dbr.id
                            LEFT JOIN DirProfs as pr on pr.kprof = dw.kprof
                            ORDER BY FIO";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["Tab_nom"]),
                    Convert.ToString(sqlReader["FIO"]),
                    Convert.ToString(sqlReader["rank"]),
                    Convert.ToString(sqlReader["prof"]),
                    Convert.ToString(sqlReader["KO"]),
                    Convert.ToString(sqlReader["KODBR"]),
                    Convert.ToString(sqlReader["Name"])
                    });
                    item.Font = new Font(lvDirWorkers.Font, FontStyle.Regular);
                    lvDirWorkers.Items.Add(item);
                }

                autoResizeColumns(lvDirWorkers);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.00.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirWorkers.Items.Count;
        }       
        

        private void lvDirWorkers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirWorkers.SelectedItems.Count > 0)
            {
                fAddWorker.id_rec = Convert.ToString(lvDirWorkers.SelectedItems[0].SubItems[0].Text);
                fAddWorker.StartPosition = FormStartPosition.CenterParent;
                fAddWorker.Text = "Изменить";
                fAddWorker.ShowDialog();
                lvDirWorkers.Items.Clear();
                LoadDirWorkers();
            }
        }

        private void tsBtnAddWorker_Click(object sender, EventArgs e)
        {
            fAddWorker.id_rec = "";
            fAddWorker.StartPosition = FormStartPosition.CenterParent;
            fAddWorker.Text = "Добавить";
            fAddWorker.ShowDialog();
            lvDirWorkers.Items.Clear();
            LoadDirWorkers();
        }

        private void tsBtnEditWorker_Click(object sender, EventArgs e)
        {
            if (lvDirWorkers.SelectedItems.Count > 0)
            {
                lvDirWorkers_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.00.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private async void tsBtnDelWorker_Click(object sender, EventArgs e)
        {
            if (lvDirWorkers.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirWorkers WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirWorkers.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.00.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirWorkers();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.00.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirBrigs()
        {
            lvDirBrigs.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT id,KODBR,Name,Numk
                            FROM DirBrigs ORDER BY KODBR";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["KODBR"]),
                    Convert.ToString(sqlReader["Name"]),
                    Convert.ToString(sqlReader["Numk"])
                    });
                    item.Font = new Font(lvDirBrigs.Font, FontStyle.Regular);
                    lvDirBrigs.Items.Add(item);
                }

                autoResizeColumns(lvDirBrigs);

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
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirBrigs.Items.Count;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            dblite.Close();
            //oWord.Quit();
            //fAuth.Close();
        }

        private void lvDirBrigs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirBrigs.SelectedItems.Count > 0)
            {
                fAddBrig.id_rec = Convert.ToString(lvDirBrigs.SelectedItems[0].SubItems[0].Text);
                fAddBrig.StartPosition = FormStartPosition.CenterParent;
                fAddBrig.Text = "Изменить";
                fAddBrig.ShowDialog();
                lvDirBrigs.Items.Clear();
                LoadDirBrigs();
            }
        }

        private void tsBtnAddBrig_Click(object sender, EventArgs e)
        {
            fAddBrig.id_rec = "";
            fAddBrig.StartPosition = FormStartPosition.CenterParent;
            fAddBrig.Text = "Добавить";
            fAddBrig.ShowDialog();
            lvDirBrigs.Items.Clear();
            LoadDirBrigs();
        }

        private void tsBtnEditBrig_Click(object sender, EventArgs e)
        {
            if (lvDirBrigs.SelectedItems.Count > 0)
            {
                lvDirBrigs_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.01.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelBrig_Click(object sender, EventArgs e)
        {
            if (lvDirBrigs.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirBrigs WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirBrigs.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.01.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirBrigs();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.00.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirProfs()
        {
            lvDirProfs.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT * FROM DirProfs ORDER BY kprof";

                m_sqlCmd = new SQLiteCommand(query, dblite);

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["kprof"]),
                    Convert.ToString(sqlReader["Name"]),
                    Convert.ToString(sqlReader["PR"])
                    });
                    item.Font = new Font(lvDirProfs.Font, FontStyle.Regular);
                    lvDirProfs.Items.Add(item);
                }

                autoResizeColumns(lvDirProfs);

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
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirProfs.Items.Count;
        }

        private void lvDirProfs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirProfs.SelectedItems.Count > 0)
            {
                fAddProf.id_rec = Convert.ToString(lvDirProfs.SelectedItems[0].SubItems[0].Text);
                fAddProf.StartPosition = FormStartPosition.CenterParent;
                fAddProf.Text = "Изменить";
                fAddProf.ShowDialog();
                lvDirProfs.Items.Clear();
                LoadDirProfs();
            }
        }

        private void tsBtnAddProf_Click(object sender, EventArgs e)
        {
            fAddProf.id_rec = "";
            fAddProf.StartPosition = FormStartPosition.CenterParent;
            fAddProf.Text = "Добавить";
            fAddProf.ShowDialog();
            lvDirProfs.Items.Clear();
            LoadDirProfs();
        }

        private void tsBtnEditProf_Click(object sender, EventArgs e)
        {
            if (lvDirProfs.SelectedItems.Count > 0)
            {
                lvDirProfs_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelProf_Click(object sender, EventArgs e)
        {
            if (lvDirProfs.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirProfs WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirProfs.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.02.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirProfs();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lvDirOpers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int item = 0;
            if (lvDirOpers.SelectedItems.Count > 0)
            {
                item = Convert.ToInt32(lvDirOpers.SelectedItems[0].Index);
                fAddOper.id_rec = Convert.ToString(lvDirOpers.SelectedItems[0].SubItems[0].Text);
                fAddOper.StartPosition = FormStartPosition.CenterParent;
                fAddOper.Text = "Изменить";
                fAddOper.ShowDialog();
                lvDirOpers.Items.Clear();
                LoadDirOpers();
                lvDirOpers.Items[item].Selected = true;
                lvDirOpers.EnsureVisible(item);
            }
        }

        public async void LoadDirOpers()
        {
            lvDirOpers.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirOpers ORDER BY UCH,PER";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["UCH"]),
                    Convert.ToString(sqlReader["PER"]),
                    Convert.ToString(sqlReader["Name"]),
                    Convert.ToString(sqlReader["KOEF"]),
                    Convert.ToString(sqlReader["NVR"])
                    });
                    item.Font = new Font(lvDirOpers.Font, FontStyle.Regular);
                    lvDirOpers.Items.Add(item);
                    if (Convert.ToInt32(sqlReader["UCH"]) % 2 == 0)  // Выделение цветом нечетных заходов
                    {
                        item.BackColor = Color.LightBlue;
                    }
                }

                autoResizeColumns(lvDirOpers);

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
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirOpers.Items.Count;
        }

        private void tsBtnAddOper_Click(object sender, EventArgs e)
        {
            fAddOper.id_rec = "";
            fAddOper.StartPosition = FormStartPosition.CenterParent;
            fAddOper.Text = "Добавить";
            fAddOper.ShowDialog();
            lvDirOpers.Items.Clear();
            LoadDirOpers();
        }

        private void tsBtnEditOper_Click(object sender, EventArgs e)
        {
            if (lvDirOpers.SelectedItems.Count > 0)
            {
                lvDirOpers_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.05.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelOper_Click(object sender, EventArgs e)
        {
            if (lvDirOpers.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirOpers WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirOpers.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.05.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirOpers();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.05.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirModels()
        {
            lvDirModels.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT dm.id,dm.KMOD,dm.Name,dp.Name as pName,dpc.category,dpg.GRP,dm.EI FROM DirModels as dm 
                               LEFT JOIN DirProducts as dp on dp.id=dm.id_product
                               LEFT JOIN DirProdCat as dpc on dpc.id=dm.id_cat 
                               LEFT JOIN DirProdGRP as dpg on dpg.id=dm.id_grp ORDER BY dm.Name";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["KMOD"]),
                    Convert.ToString(sqlReader["Name"]),
                    Convert.ToString(sqlReader["pName"]),
                    Convert.ToString(sqlReader["category"]),
                    Convert.ToString(sqlReader["GRP"]),
                    Convert.ToString(sqlReader["EI"])
                    });
                    item.Font = new Font(lvDirModels.Font, FontStyle.Regular);
                    lvDirModels.Items.Add(item);

                }

                autoResizeColumns(lvDirModels);

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
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirModels.Items.Count;
        }

        private void lvDirModels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int item = 0;
            if (lvDirModels.SelectedItems.Count > 0)
            {
                item = Convert.ToInt32(lvDirModels.SelectedItems[0].Index);
                fAddModel.id_rec = Convert.ToString(lvDirModels.SelectedItems[0].SubItems[0].Text);
                fAddModel.StartPosition = FormStartPosition.CenterParent;
                fAddModel.Text = "Изменить";
                fAddModel.ShowDialog();
                lvDirModels.Items.Clear();
                LoadDirModels();
                lvDirModels.Items[item].Selected = true;
                lvDirModels.EnsureVisible(item);
            }
        }

        private void tsBtnAddMod_Click(object sender, EventArgs e)
        {
            fAddModel.id_rec = "";
            fAddModel.StartPosition = FormStartPosition.CenterParent;
            fAddModel.Text = "Добавить";
            fAddModel.ShowDialog();
            lvDirModels.Items.Clear();
            LoadDirModels();
        }

        private void tsBtnEditMod_Click(object sender, EventArgs e)
        {
            if (lvDirModels.SelectedItems.Count > 0)
            {
                lvDirModels_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.06.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelMod_Click(object sender, EventArgs e)
        {
            if (lvDirModels.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirModels WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirModels.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.06.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirModels();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.06.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirProducts()
        {
            lvDirProducts.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirProducts ORDER BY id";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["Name"])
                    });
                    item.Font = new Font(lvDirProducts.Font, FontStyle.Regular);
                    lvDirProducts.Items.Add(item);
                }

                autoResizeColumns(lvDirProducts);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.07.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirProducts.Items.Count;
        }

        private void lvDirProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirProducts.SelectedItems.Count > 0)
            {
                fAddProd.id_rec = Convert.ToString(lvDirProducts.SelectedItems[0].SubItems[0].Text);
                fAddProd.StartPosition = FormStartPosition.CenterParent;
                fAddProd.Text = "Изменить";
                fAddProd.ShowDialog();
                lvDirProducts.Items.Clear();
                LoadDirProducts();
            }
        }

        private void tsBtnAddProd_Click(object sender, EventArgs e)
        {
            fAddProd.id_rec = "";
            fAddProd.StartPosition = FormStartPosition.CenterParent;
            fAddProd.Text = "Добавить";
            fAddProd.ShowDialog();
            lvDirProducts.Items.Clear();
            LoadDirProducts();
        }

        private void tsBtnEditProd_Click(object sender, EventArgs e)
        {
            if (lvDirProducts.SelectedItems.Count > 0)
            {
                lvDirProducts_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelProd_Click(object sender, EventArgs e)
        {
            if (lvDirProducts.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirProducts WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirProducts.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.07.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirProducts();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirTarif()
        {
            lvDirTarif.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirTarif ORDER BY rank";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["rank"]),
                    Convert.ToString(sqlReader["TAR_VR"]),
                    Convert.ToString(sqlReader["K_SD"])
                    });
                    item.Font = new Font(lvDirTarif.Font, FontStyle.Regular);
                    lvDirTarif.Items.Add(item);
                }

                autoResizeColumns(lvDirTarif);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.08.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirTarif.Items.Count;
        }

        private void lvDirTarif_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirTarif.SelectedItems.Count > 0)
            {
                fAddTarif.id_rec = Convert.ToString(lvDirTarif.SelectedItems[0].SubItems[0].Text);
                fAddTarif.StartPosition = FormStartPosition.CenterParent;
                fAddTarif.Text = "Изменить";
                fAddTarif.ShowDialog();
                lvDirTarif.Items.Clear();
                LoadDirTarif();
            }
        }

        private void tsBtnAddTarif_Click(object sender, EventArgs e)
        {
            fAddTarif.id_rec = "";
            fAddTarif.StartPosition = FormStartPosition.CenterParent;
            fAddTarif.Text = "Добавить";
            fAddTarif.ShowDialog();
            lvDirTarif.Items.Clear();
            LoadDirTarif();
        }

        private void tsBtnEditTarif_Click(object sender, EventArgs e)
        {
            if (lvDirTarif.SelectedItems.Count > 0)
            {
                lvDirTarif_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.08.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelTarif_Click(object sender, EventArgs e)
        {
            if (lvDirTarif.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirTarif WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirTarif.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.08.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirTarif();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.08.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirNormNastil()
        {
            lvDirNormNastil.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirNormNastil ORDER BY id";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["VIDTK"]),
                    Convert.ToString(sqlReader["NORMVR"])
                    });
                    item.Font = new Font(lvDirNormNastil.Font, FontStyle.Regular);
                    lvDirNormNastil.Items.Add(item);
                }

                autoResizeColumns(lvDirNormNastil);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirNormNastil.Items.Count;
        }

        private void lvDirNormNastil_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirNormNastil.SelectedItems.Count > 0)
            {
                fAddNormNastil.id_rec = Convert.ToString(lvDirNormNastil.SelectedItems[0].SubItems[0].Text);
                fAddNormNastil.StartPosition = FormStartPosition.CenterParent;
                fAddNormNastil.Text = "Изменить";
                fAddNormNastil.ShowDialog();
                lvDirNormNastil.Items.Clear();
                LoadDirNormNastil();
            }
        }

        private void tsBtnAddNormNastil_Click(object sender, EventArgs e)
        {
            fAddNormNastil.id_rec = "";
            fAddNormNastil.StartPosition = FormStartPosition.CenterParent;
            fAddNormNastil.Text = "Добавить";
            fAddNormNastil.ShowDialog();
            lvDirNormNastil.Items.Clear();
            LoadDirNormNastil();
        }

        private void tsBtnEditNormNastil_Click(object sender, EventArgs e)
        {
            if (lvDirNormNastil.SelectedItems.Count > 0)
            {
                lvDirNormNastil_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelNormNastil_Click(object sender, EventArgs e)
        {
            if (lvDirNormNastil.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirNormNastil WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirNormNastil.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.09.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirNormNastil();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirNormControl()
        {
            lvDirNormControl.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirNormControl ORDER BY id";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["VIDTK"]),
                    Convert.ToString(sqlReader["NORMVR"])
                    });
                    item.Font = new Font(lvDirNormControl.Font, FontStyle.Regular);
                    lvDirNormControl.Items.Add(item);
                }

                autoResizeColumns(lvDirNormControl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirNormControl.Items.Count;
        }

        private void lvDirNormControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirNormControl.SelectedItems.Count > 0)
            {
                fAddNormControl.id_rec = Convert.ToString(lvDirNormControl.SelectedItems[0].SubItems[0].Text);
                fAddNormControl.StartPosition = FormStartPosition.CenterParent;
                fAddNormControl.Text = "Изменить";
                fAddNormControl.ShowDialog();
                lvDirNormControl.Items.Clear();
                LoadDirNormControl();
            }
        }

        private void tsBtnAddNormControl_Click(object sender, EventArgs e)
        {
            fAddNormControl.id_rec = "";
            fAddNormControl.StartPosition = FormStartPosition.CenterParent;
            fAddNormControl.Text = "Добавить";
            fAddNormControl.ShowDialog();
            lvDirNormControl.Items.Clear();
            LoadDirNormControl();
        }

        private void tsBtnEditNormControl_Click(object sender, EventArgs e)
        {
            if (lvDirNormControl.SelectedItems.Count > 0)
            {
                lvDirNormControl_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async void LoadDirSigners()
        {
            lvDirSigners.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT * FROM DirSigners ORDER BY ord";

                m_sqlCmd = new SQLiteCommand(query, dblite);

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["post"]),
                    Convert.ToString(sqlReader["FIO"]),
                    Convert.ToString(sqlReader["ord"])
                    });
                    item.Font = new Font(lvDirSigners.Font, FontStyle.Regular);
                    lvDirSigners.Items.Add(item);
                }

                autoResizeColumns(lvDirSigners);

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
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDirSigners.Items.Count;
        }

        private void lvDirSigners_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirSigners.SelectedItems.Count > 0)
            {
                fAddSigner.id_rec = Convert.ToString(lvDirSigners.SelectedItems[0].SubItems[0].Text);
                fAddSigner.StartPosition = FormStartPosition.CenterParent;
                fAddSigner.Text = "Изменить";
                fAddSigner.ShowDialog();
                lvDirSigners.Items.Clear();
                LoadDirSigners();
            }
        }

        private void tsBtnAddSigner_Click(object sender, EventArgs e)
        {
            fAddSigner.id_rec = "";
            fAddSigner.StartPosition = FormStartPosition.CenterParent;
            fAddSigner.Text = "Добавить";
            fAddSigner.ShowDialog();
            lvDirSigners.Items.Clear();
            LoadDirSigners();
        }

        private void tsBtnEditSigner_Click(object sender, EventArgs e)
        {
            if (lvDirSigners.SelectedItems.Count > 0)
            {
                lvDirSigners_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelSigner_Click(object sender, EventArgs e)
        {
            if (lvDirSigners.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirSigners WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirSigners.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.02.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirSigners();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void tsBtnDelNormControl_Click(object sender, EventArgs e)
        {
            if (lvDirNormControl.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirNormControl WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirNormControl.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.10.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirNormControl();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.10.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsBtnAddDivision_Click(object sender, EventArgs e)
        {
            fAddDivision.id_rec = "";
            fAddDivision.StartPosition = FormStartPosition.CenterParent;
            fAddDivision.Text = "Добавить";
            fAddDivision.ShowDialog();
            lvDivision.Items.Clear();
            LoadDivisions();
        }

        public async void LoadDivisions()
        {
            lvDivision.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT div.id,dm.name as mName,
                    dp.Name as pName, dc.category, dg.GRP,
                    (SELECT SUM(CASE WHEN d.UCH between 1 and 2 THEN ifnull(ROUND(i.NVRforOper * i.MatRate * i.workers_cnt,2),0) ELSE NVRforOper END)
                    FROM inDivision as i LEFT JOIN DirOpers as d on d.id=i.id_oper LEFT JOIN DirTarif as dt on dt.rank=i.rank
                    WHERE id_division=div.id) as work_time,
                    (SELECT SUM(CASE WHEN d.UCH between 1 and 2 THEN ifnull(ROUND(ROUND(i.NVRforOper * dt.TAR_VR,5) * i.MatRate * i.workers_cnt,5),0) 
                    ELSE ifnull(ROUND(i.NVRforOper * dt.TAR_VR * i.workers_cnt,5),0) END)
                    FROM inDivision as i LEFT JOIN DirOpers as d on d.id=i.id_oper LEFT JOIN DirTarif as dt on dt.rank=i.rank
                    WHERE id_division=div.id) as cost
                    FROM Division as div
                    LEFT JOIN DirModels as dm on div.id_model=dm.id 
                    LEFT JOIN DirProducts as dp on dm.id_product=dp.id
                    LEFT JOIN DirProdCat as dc on dm.id_cat=dc.id
                    LEFT JOIN DirProdGRP as dg on dm.id_grp=dg.id
                    WHERE div.mm=" + dateTimePicker1.Value.Month.ToString()+" and div.yy="
                    + dateTimePicker1.Value.Year.ToString() +
                    " ORDER BY div.id";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["mName"]),
                    Convert.ToString(sqlReader["pName"])+" "+Convert.ToString(sqlReader["category"])+" "+Convert.ToString(sqlReader["GRP"]),
                    Convert.ToString(sqlReader["work_time"]),
                    Convert.ToString(sqlReader["cost"])
                    });
                    item.Font = new Font(lvDivision.Font, FontStyle.Regular);
                    lvDivision.Items.Add(item);
                }

                autoResizeColumns(lvDivision);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvDivision.Items.Count;
        }

        private void tsBtnEditDivision_Click(object sender, EventArgs e)
        {
            if (lvDivision.SelectedItems.Count > 0)
            {
                fAddDivision.id_rec = lvDivision.SelectedItems[0].SubItems[0].Text;
                fAddDivision.StartPosition = FormStartPosition.CenterParent;
                fAddDivision.Text = "Изменить";
                fAddDivision.ShowDialog();
                lvDivision.Items.Clear();
                LoadDivisions();
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Переход к операциям по разделению
        private void lvDivision_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            if (lvDivision.SelectedItems.Count > 0)
            {
                Division.id = lvDivision.SelectedItems[0].SubItems[0].Text;
                Division.mm = dateTimePicker1.Value.Month.ToString();
                Division.yy = dateTimePicker1.Value.Year.ToString();
                tpinDivision.Parent = tabControl1;
                tpDirs.Parent = null;
                tpDirs.Parent = tabControl1;
                tpinDivision.Text = @"Разделение труда по модели " + lvDivision.SelectedItems[0].SubItems[1].Text;
                tabControl1.SelectedTab = tpinDivision;
                LoadOpersByDivision(lvDivision.SelectedItems[0].SubItems[0].Text);
            }
        }

        private async void tsBtnDelDivision_Click(object sender, EventArgs e)
        {
            if (lvDivision.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM Division WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDivision.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.10.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDivisions();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.10.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Переход к операциям по разделению
        private void tsBtnGotoDivision_Click(object sender, EventArgs e)
        {
            lvDivision_MouseDoubleClick(this, null);
        }

        //Отбор операция для разделения
        private void tsBtnAddOperInDivision_Click(object sender, EventArgs e)
        {

            fOpersList.StartPosition = FormStartPosition.CenterParent;
            fOpersList.ShowDialog();
            LoadOpersByDivision(Division.id);
        }



        public async void LoadOpersByDivision(string id_div)
        {
            lvinDivision.Items.Clear();  //Чистим listview
            int counter = 0;
            string st_cnt = string.Empty;
            try
            {
                string query = @"SELECT i.id,d.UCH,i.id_oper,d.Name,d.parent,i.MatRate,i.rank,dt.TAR_VR,i.NVRforOper,i.workers_cnt,
                                IFNULL(ROUND(i.NVRforOper * dt.TAR_VR,5),0) as Cost,
                                CASE WHEN d.UCH between 1 and 2 THEN ifnull(ROUND(i.NVRforOper * i.MatRate * i.workers_cnt,2),0) ELSE NVRforOper END as NVRbyItem,
                                CASE WHEN d.UCH between 1 and 2 THEN ifnull(ROUND(ROUND(i.NVRforOper * dt.TAR_VR,5) * i.MatRate * i.workers_cnt,5),0) 
                                    ELSE ifnull(ROUND(i.NVRforOper * dt.TAR_VR * i.workers_cnt,5),0) END as SumItem
                                FROM inDivision as i 
                                LEFT JOIN DirOpers as d on d.id=i.id_oper
                                LEFT JOIN DirTarif as dt on dt.rank=i.rank
                                WHERE id_division='" + id_div +@"' ORDER BY d.PER";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;

                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    //Если подкатегории операции
                    if ((Convert.ToInt32(sqlReader["parent"]) > 0) && (Convert.ToInt32(sqlReader["parent"])!= Convert.ToInt32(sqlReader["id_oper"])))
                    {
                        st_cnt = string.Empty;  //Порядковый номер пустой
                    }
                    else
                    {
                        counter++;
                        st_cnt = counter.ToString();
                    }

                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["UCH"]),
                    Convert.ToString(st_cnt),
                    Convert.ToString(sqlReader["Name"]),
                    Convert.ToString(sqlReader["MatRate"]),
                    Convert.ToString(sqlReader["rank"]),
                    Convert.ToString(sqlReader["NVRforOper"]),
                    Convert.ToString(sqlReader["workers_cnt"]),
                    Convert.ToString(sqlReader["Cost"]),
                    Convert.ToString(sqlReader["NVRbyItem"]),
                    Convert.ToString(sqlReader["SumItem"])
                    });
                    item.Font = new Font(lvDivision.Font, FontStyle.Regular);
                    lvinDivision.Items.Add(item);
                    if (Convert.ToInt32(sqlReader["UCH"]) % 2 == 0)  // Выделение цветом нечетных заходов
                    {
                        item.BackColor = Color.LightBlue;
                    }
                }

                autoResizeColumns(lvinDivision);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во строк: " + lvinDivision.Items.Count;

            double absSumNVR = 0;
            double absSumItem = 0;

            for (int i = 0; i < lvinDivision.Items.Count; i++)
            {
                absSumNVR += double.Parse(lvinDivision.Items[i].SubItems[9].Text);
                absSumItem += double.Parse(lvinDivision.Items[i].SubItems[10].Text);

            }
            tsStatusNVRbyItem.Text = "Время обработки: " +absSumNVR.ToString();
            tsStatusSumItem.Text = "Стоимость обработки: " +absSumItem.ToString();
        }



        //Удалить операцию из распределения
        private async void tsBtnDelOperInDivision_Click(object sender, EventArgs e)
        {
            if (lvinDivision.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM inDivision WHERE id=@id", dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvinDivision.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.10.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadOpersByDivision(Division.id);

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка 5.10.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Ввод параметров по каждой операции в разделении
        private void tsBtnEditOperInDivision_Click(object sender, EventArgs e)
        {
            if (lvinDivision.SelectedItems.Count > 0)
            {

                fEditOperByDivision.id_rec = lvinDivision.SelectedItems[0].SubItems[0].Text;   //id записи
                fEditOperByDivision.number = int.Parse(lvinDivision.SelectedItems[0].SubItems[1].Text); //номер по порядку                
                fEditOperByDivision.rank = lvinDivision.SelectedItems[0].SubItems[5].Text;   //разряд               
                fEditOperByDivision.StartPosition = FormStartPosition.CenterParent;
                fEditOperByDivision.Text = "Изменить";
                fEditOperByDivision.ShowDialog();
                lvinDivision.Items.Clear();
                LoadOpersByDivision(Division.id);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Редактирование операций по разделению
        private void lvinDivision_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsBtnEditOperInDivision_Click(this,null);
        }



        //Создать резервную копию БД
        private void BackUpDBItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(Environment.CurrentDirectory + "\\divisionDB.db", Environment.CurrentDirectory + "\\DBbackups\\div_"+DateTime.Now.ToString("ddMMyyyy")+".db", true);
                MessageBox.Show("BackUp","Резервная копия создана. ("+ Environment.CurrentDirectory + "\\DBbackups\\div_" + DateTime.Now.ToString("ddMMyyyy") + ".db)");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Напечатать разделение
        private void tsPrintDivision_Click(object sender, EventArgs e)
        {
            string Filename= Environment.CurrentDirectory + "\\Reports\\Division" + dateTimePicker1.Value.ToString("MM_yyyy")+ ".docx";
            _Document oDoc = GetDoc(Environment.CurrentDirectory + "\\DivisionTemplate.docx");
            oDoc.SaveAs(FileName: Filename); //Сохраняем документ
            oDoc.Close();
            System.Diagnostics.Process.Start(Filename);  //Открыть документ разделения
        }

        //Получаем документ
        private _Document GetDoc(string path)
        {
            _Document oDoc = oWord.Documents.Add(path);
            SetTemplate(oDoc);
            return oDoc;
        }

        //Заполняем шаблон
        private async void SetTemplate(Microsoft.Office.Interop.Word._Document oDoc)
        {
            int i=0;
            double NVRsec = Convert.ToDouble(lvDivision.SelectedItems[0].SubItems[3].Text);
            if (lvDivision.SelectedItems.Count > 0)
            {
                try
                {
                    //Титульный лист
                    oDoc.Bookmarks["product"].Range.Text = lvDivision.SelectedItems[0].SubItems[2].Text;
                    oDoc.Bookmarks["model"].Range.Text = lvDivision.SelectedItems[0].SubItems[1].Text;
                    oDoc.Bookmarks["mmyy"].Range.Text = "за "+dateTimePicker1.Value.ToString("Y").ToUpper()+" г.";
                    oDoc.Bookmarks["sumNVR"].Range.Text = NVRsec.ToString()+"с = "+Math.Round(NVRsec* 0.000278,2)+" ч";
                    oDoc.Bookmarks["sumItem"].Range.Text = lvDivision.SelectedItems[0].SubItems[4].Text;

                    //Подписанты
                    try
                    {
                        string query = @"SELECT * FROM DirSigners ORDER BY ord LIMIT 2";

                        m_sqlCmd = new SQLiteCommand(query, dblite);

                        sqlReader = m_sqlCmd.ExecuteReader();

                        while (await sqlReader.ReadAsync())
                        {
                            i++;
                            oDoc.Bookmarks["Signer"+i.ToString()].Range.Text = Convert.ToString(sqlReader["post"])+
                                                @"                       /"+ Convert.ToString(sqlReader["FIO"])+@"/";
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


                    //Таблица разделения
                    oDoc.Bookmarks["model2"].Range.Text = lvDivision.SelectedItems[0].SubItems[1].Text;
                    Table wTable = oDoc.Tables[1];
                    for (int row = 0; row < lvinDivision.Items.Count; row++) //проход по строкам
                    {
                        for (int col = 0; col < lvinDivision.Items[row].SubItems.Count-2; col++)
                        {
                            wTable.Cell(row + 2, col+1).Range.Text = lvinDivision.Items[row].SubItems[col+2].Text;
                        }
                        wTable.Rows.Add();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка 5.00.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите разделение для печати.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


    }
    class Division
    {
        public static string id { get; set; }
        public static string mm { get; set; }
        public static string yy { get; set; }
    }
}
