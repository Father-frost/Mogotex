using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WorkDivision.Contracts;
using Font = System.Drawing.Font;
using View = System.Windows.Forms.View;
using Word = Microsoft.Office.Interop.Word;

namespace WorkDivision
{
    public partial class Form1 : Form
    {
        private SQLiteConnection _dblite;
        private SQLiteCommand _m_sqlCmd;
        private SQLiteDataReader _sqlReader;
        private liteDB _liteDB;

        public fAddWorker fAddWorker;
        public fAddBrig fAddBrig;
        public fAddProf fAddProf;
        public fAddOper fAddOper;
        public fAddModel fAddModel;
        public fAddProd fAddProd;
        public fAddCat1 fAddCat1;
        public fAddCat2 fAddCat2;
        public fAddTarif fAddTarif;
        public fAddNormNastil fAddNormNastil;
        public fAddNormControl fAddNormControl;
        public fAddDivision fAddDivision;
        public fOpersList fOpersList;
        public fEditOperByDivision fEditOperByDivision;
        public fAddSigner fAddSigner;
        public fSelectDivisionToCopy fSelectDivisionToCopy;
        public fAddPieceWork fAddPieceWork;
        //public fAuth fAuth;

        public Form1()
        {
            InitializeComponent();

            fAddWorker = new fAddWorker();
            fAddBrig = new fAddBrig();
            fAddProf = new fAddProf();
            fAddOper = new fAddOper();
            fAddModel = new fAddModel();
            fAddProd = new fAddProd();
            fAddCat1 = new fAddCat1();
            fAddCat2 = new fAddCat2();
            fAddTarif = new fAddTarif();
            fAddNormNastil = new fAddNormNastil();
            fAddNormControl = new fAddNormControl();
            fAddDivision = new fAddDivision();
            fOpersList = new fOpersList();
            fEditOperByDivision = new fEditOperByDivision();
            fAddSigner = new fAddSigner();
            fSelectDivisionToCopy = new fSelectDivisionToCopy();
            fAddPieceWork = new fAddPieceWork();
            _liteDB = new liteDB();

        }

        public delegate void LoadOpersByDivisionDelegate(string id_div);

        private void Form1_Load(object sender, EventArgs e)
        {
            //Подключение к БД (в Form_Load для каждой формы)
            _dblite = _liteDB.GetConn();
            _dblite.Open();

            tpinDivision.Parent = null;  //Скрываем вкладку инфы по разделению
            tpPieceWork.Parent = null;  //Скрываем вкладку начислений
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
            lvDirWorkers.Columns.Add("ФИО                   ");
            lvDirWorkers.Columns.Add("Разряд");
            lvDirWorkers.Columns.Add("Профессия");
            lvDirWorkers.Columns.Add("KO");
            lvDirWorkers.Columns.Add("Номер бригады");
            lvDirWorkers.Columns.Add("Название бригады");
            Division.autoResizeColumns(lvDirWorkers);

            //Справочник бригад
            lvDirBrigs.GridLines = true;
            lvDirBrigs.FullRowSelect = true;
            lvDirBrigs.View = View.Details;
            lvDirBrigs.Font = new Font(lvDirBrigs.Font, FontStyle.Bold);
            lvDirBrigs.Columns.Add("ID");
            lvDirBrigs.Columns.Add("Номер");
            lvDirBrigs.Columns.Add("Название бригады");
            Division.autoResizeColumns(lvDirBrigs);

            //Справочник профессий
            lvDirProfs.GridLines = true;
            lvDirProfs.FullRowSelect = true;
            lvDirProfs.View = View.Details;
            lvDirProfs.Font = new Font(lvDirProfs.Font, FontStyle.Bold);
            lvDirProfs.Columns.Add("ID");
            //            lvDirProfs.Columns.Add("Код профессии");
            lvDirProfs.Columns.Add("Наименование            ");
            lvDirProfs.Columns.Add("PR");
            Division.autoResizeColumns(lvDirProfs);

            //Справочник операций
            lvDirOpers.GridLines = true;
            lvDirOpers.FullRowSelect = true;
            lvDirOpers.View = View.Details;
            lvDirOpers.Font = new Font(lvDirOpers.Font, FontStyle.Bold);
            lvDirOpers.Columns.Add("ID");
            lvDirOpers.Columns.Add("Участок");
            lvDirOpers.Columns.Add("Переход");
            lvDirOpers.Columns.Add("Операция");
            lvDirOpers.Columns.Add("ID родительской операции");
            Division.autoResizeColumns(lvDirOpers);

            //lvDirOpers.Columns.Add("Коэф.");
            //lvDirOpers.Columns.Add("Норма времени, сек");

            //Справочник продукции
            lvDirProducts.GridLines = true;
            lvDirProducts.FullRowSelect = true;
            lvDirProducts.View = View.Details;
            lvDirProducts.Font = new Font(lvDirProducts.Font, FontStyle.Bold);
            lvDirProducts.Columns.Add("id");
            lvDirProducts.Columns.Add("Наименование             ");
            Division.autoResizeColumns(lvDirProducts);

            //Справочник Категорий 1
            lvDirCat1.GridLines = true;
            lvDirCat1.FullRowSelect = true;
            lvDirCat1.View = View.Details;
            lvDirCat1.Font = new Font(lvDirCat1.Font, FontStyle.Bold);
            lvDirCat1.Columns.Add("id");
            lvDirCat1.Columns.Add("Наименование             ");
            Division.autoResizeColumns(lvDirCat1);

            //Справочник Категорий 2
            lvDirCat2.GridLines = true;
            lvDirCat2.FullRowSelect = true;
            lvDirCat2.View = View.Details;
            lvDirCat2.Font = new Font(lvDirCat2.Font, FontStyle.Bold);
            lvDirCat2.Columns.Add("id");
            lvDirCat2.Columns.Add("Наименование             ");
            Division.autoResizeColumns(lvDirCat2);

            //Справочник тарифных ставок
            lvDirTarif.GridLines = true;
            lvDirTarif.FullRowSelect = true;
            lvDirTarif.View = View.Details;
            lvDirTarif.Font = new Font(lvDirTarif.Font, FontStyle.Bold);
            lvDirTarif.Columns.Add("id");
            lvDirTarif.Columns.Add("Разряд");
            lvDirTarif.Columns.Add("Тар.ставка");
            lvDirTarif.Columns.Add("Коэф.сдел.");
            Division.autoResizeColumns(lvDirTarif);

            //Справочник норм на настил
            lvDirNormNastil.GridLines = true;
            lvDirNormNastil.FullRowSelect = true;
            lvDirNormNastil.View = View.Details;
            lvDirNormNastil.Font = new Font(lvDirNormNastil.Font, FontStyle.Bold);
            lvDirNormNastil.Columns.Add("id");
            lvDirNormNastil.Columns.Add("Вид ткани                      ");
            lvDirNormNastil.Columns.Add("Затрата врем. на 1м, сек");
            Division.autoResizeColumns(lvDirNormNastil);

            //Справочник норм на контроль
            lvDirNormControl.GridLines = true;
            lvDirNormControl.FullRowSelect = true;
            lvDirNormControl.View = View.Details;
            lvDirNormControl.Font = new Font(lvDirNormControl.Font, FontStyle.Bold);
            lvDirNormControl.Columns.Add("id");
            lvDirNormControl.Columns.Add("Вид ткани                     ");
            lvDirNormControl.Columns.Add("Затрата врем. на 1м, сек");
            Division.autoResizeColumns(lvDirNormControl);

            //Справочник подписантов
            lvDirSigners.GridLines = true;
            lvDirSigners.FullRowSelect = true;
            lvDirSigners.View = View.Details;
            lvDirSigners.Font = new Font(lvDirSigners.Font, FontStyle.Bold);
            lvDirSigners.Columns.Add("id");
            lvDirSigners.Columns.Add("Должность             ");
            lvDirSigners.Columns.Add("ФИО                   ");
            lvDirSigners.Columns.Add("Порядок");
            lvDirSigners.Columns.Add("Место использования");
            Division.autoResizeColumns(lvDirSigners);

            //Справочник моделей
            lvDirModels.GridLines = true;
            lvDirModels.FullRowSelect = true;
            lvDirModels.View = View.Details;
            lvDirModels.Font = new Font(lvDirModels.Font, FontStyle.Bold);
            lvDirModels.Columns.Add("id");
            lvDirModels.Columns.Add("КОД            ");
            lvDirModels.Columns.Add("Название модели");
            lvDirModels.Columns.Add("Вид изделия                ");
            lvDirModels.Columns.Add("Категория 1                ");
            lvDirModels.Columns.Add("Категория 2                ");
            lvDirModels.Columns.Add("Ед.изм.");
            Division.autoResizeColumns(lvDirModels);

            //Разделения
            lvDivision.GridLines = true;
            lvDivision.FullRowSelect = true;
            lvDivision.View = View.Details;
            lvDivision.Font = new Font(lvDivision.Font, FontStyle.Bold);
            lvDivision.Columns.Add("id");
            lvDivision.Columns.Add("№ модели");
            lvDivision.Columns.Add("Модель     ");
            lvDivision.Columns.Add("Вид изделия                             ");
            lvDivision.Columns.Add("Время обработки");
            lvDivision.Columns.Add("Стоимость обработки");
            Division.autoResizeColumns(lvDivision);

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
            Division.autoResizeColumns(lvinDivision);

            //Информация для начисления
            lvPieceWork.GridLines = true;
            lvPieceWork.FullRowSelect = true;
            lvPieceWork.View = View.Details;
            lvPieceWork.Font = new Font(lvPieceWork.Font, FontStyle.Bold);
            lvPieceWork.Columns.Add("id");
            lvPieceWork.Columns.Add("Номер карты");
            lvPieceWork.Columns.Add("Таб.ном.");
            lvPieceWork.Columns.Add("ФИО               ");
            lvPieceWork.Columns.Add("Операция");
            lvPieceWork.Columns.Add("Количество");
            lvPieceWork.Columns.Add("Расценка");
            lvPieceWork.Columns.Add("Сумма          ");
            lvPieceWork.Columns.Add("Кол-во по карте");
            Division.autoResizeColumns(lvPieceWork);


            dateTimePicker1_ValueChanged(this, null);

            tabControl1.SelectedIndex = 0;
            tsStatusSumCost.Text = "";
            tsStatusNVRbyItem.Text = "";
            tsStatusSumItem.Text = "";
        }

        //Закрытие формы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _dblite.Close();
        }

        //Изменение даты
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            tpinDivision.Parent = null;
            tpPieceWork.Parent = null;
            lvDivision.Items.Clear();  //Чистим listview2

            try
            {
                LoadDivisions();  //Загружаем разделения за выбранный период

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.00.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDivision.Items.Count;

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpDivision)   //Выбрана вкладка разделение
            {
                dateTimePicker1_ValueChanged(this, null);
                if (lvDivision.Items.Count == 0)
                {
                    toolStripStatusLabel1.Text = "Нет разделений за выбранный период.";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Кол-во записей: " + lvDivision.Items.Count;
                }
                //Обнуляем статусбары
                tsStatusSumCost.Text = "";
                tsStatusNVRbyItem.Text = "";
                tsStatusSumItem.Text = "";
            }
            if (tabControl1.SelectedTab == tpDirs)  //Если выбрана закладка справочники
            {
                if (tabControl2.SelectedTab == tpDirWorkers)
                {
                    LoadDirWorkers();
                }
                //Обнуляем статусбары
                tsStatusSumCost.Text = "";
                tsStatusNVRbyItem.Text = "";
                tsStatusSumItem.Text = "";
            }
            if (tabControl1.SelectedTab == tpinDivision)
            {
                //TODO: Добавить делегат для вызова обновления listview операций разделения
                Invoke(new LoadOpersByDivisionDelegate(LoadOpersByDivision), new object[] { Division.id });
            }
            if (tabControl1.SelectedTab == tpPieceWork)
            {

            }


        }

        //Навигация по справочникам
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
            if (tabControl2.SelectedTab == tpDirCat1)
            {
                LoadDirCat1();
            }
            if (tabControl2.SelectedTab == tpDirCat2)
            {
                LoadDirCat2();
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
            if (tabControl2.SelectedTab == tpDirSigners)
            {
                LoadDirSigners();
            }
        }

        //Загрузить справочник работников 
        public async void LoadDirWorkers()
        {
            lvDirWorkers.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT dw.id,dw.tab_nom,dw.FIO,dw.rank,dw.KO,dbr.KODBR,dbr.Name,pr.Name as prof
                            FROM DirWorkers as dw
                            LEFT JOIN DirBrigs as dbr on dw.brig_id = dbr.id
                            LEFT JOIN DirProfs as pr on pr.id = dw.prof_id
                            ORDER BY FIO";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["Tab_nom"]),
                    Convert.ToString(_sqlReader["FIO"]),
                    Convert.ToString(_sqlReader["rank"]),   //разряд
                    Convert.ToString(_sqlReader["prof"]),   //профессия
                    Convert.ToString(_sqlReader["KO"]),
                    Convert.ToString(_sqlReader["KODBR"]),   //Код бригады
                    Convert.ToString(_sqlReader["Name"])     //Название бригады
                    });
                    item.Font = new Font(lvDirWorkers.Font, FontStyle.Regular);
                    lvDirWorkers.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirWorkers);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.00.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirWorkers.Items.Count;
        }

        //Двойной щелчок по элементу справочника рабочих (Изменить запись данные работника)
        private void lvDirWorkers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirWorkers.SelectedItems.Count > 0)
            {
                fAddWorker.id_rec = Convert.ToString(lvDirWorkers.SelectedItems[0].SubItems[0].Text);
                fAddWorker.StartPosition = FormStartPosition.CenterParent;
                fAddWorker.Text = "Изменить запись запись";
                fAddWorker.ShowDialog();
                LoadDirWorkers();
            }
        }

        //Добавить работника
        private void tsBtnAddWorker_Click(object sender, EventArgs e)
        {
            fAddWorker.id_rec = "";
            fAddWorker.StartPosition = FormStartPosition.CenterParent;
            fAddWorker.Text = "Добавить запись";
            fAddWorker.ShowDialog();
            LoadDirWorkers();
        }

        //Изменить запись работника
        private void tsBtnEditWorker_Click(object sender, EventArgs e)
        {
            if (lvDirWorkers.SelectedItems.Count > 0)
            {
                lvDirWorkers_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.00.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //Удалить работника
        private async void tsBtnDelWorker_Click(object sender, EventArgs e)
        {
            if (lvDirWorkers.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirWorkers WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.00.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник бригад
        public async void LoadDirBrigs()
        {
            lvDirBrigs.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT id,KODBR,Name,Numk
                            FROM DirBrigs ORDER BY KODBR";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["KODBR"]),  //код бригады
                    Convert.ToString(_sqlReader["Name"]),   //название бригады
                    //Convert.ToString(sqlReader["Numk"])
                    });
                    item.Font = new Font(lvDirBrigs.Font, FontStyle.Regular);
                    lvDirBrigs.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirBrigs);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirBrigs.Items.Count;
        }

        //Двойной щелчок по элементу справочника Бригады (Изменить запись)
        private void lvDirBrigs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirBrigs.SelectedItems.Count > 0)
            {
                fAddBrig.id_rec = Convert.ToString(lvDirBrigs.SelectedItems[0].SubItems[0].Text);
                fAddBrig.StartPosition = FormStartPosition.CenterParent;
                fAddBrig.Text = "Изменить запись";
                fAddBrig.ShowDialog();
                LoadDirBrigs();
            }
        }

        //Добавить бригаду
        private void tsBtnAddBrig_Click(object sender, EventArgs e)
        {
            fAddBrig.id_rec = "";
            fAddBrig.StartPosition = FormStartPosition.CenterParent;
            fAddBrig.Text = "Добавить запись";
            fAddBrig.ShowDialog();
            LoadDirBrigs();
        }

        //Изменить запись бригаду
        private void tsBtnEditBrig_Click(object sender, EventArgs e)
        {
            if (lvDirBrigs.SelectedItems.Count > 0)
            {
                lvDirBrigs_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.01.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить бригаду
        private async void tsBtnDelBrig_Click(object sender, EventArgs e)
        {
            if (lvDirBrigs.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirBrigs WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.00.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник профессий
        public async void LoadDirProfs()
        {
            lvDirProfs.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirProfs ORDER BY name";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["Name"]),    //название профессии
                    Convert.ToString(_sqlReader["PR"])       //процент
                    });
                    item.Font = new Font(lvDirProfs.Font, FontStyle.Regular);
                    lvDirProfs.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirProfs);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirProfs.Items.Count;
        }

        //Изменить запись профессию (двойной клик по элементу)
        private void lvDirProfs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirProfs.SelectedItems.Count > 0)
            {
                fAddProf.id_rec = Convert.ToString(lvDirProfs.SelectedItems[0].SubItems[0].Text);
                fAddProf.StartPosition = FormStartPosition.CenterParent;
                fAddProf.Text = "Изменить запись";
                fAddProf.ShowDialog();
                LoadDirProfs();
            }
        }

        //Добавить профессию
        private void tsBtnAddProf_Click(object sender, EventArgs e)
        {
            fAddProf.id_rec = "";
            fAddProf.StartPosition = FormStartPosition.CenterParent;
            fAddProf.Text = "Добавить запись";
            fAddProf.ShowDialog();
            LoadDirProfs();
        }

        //Изменить запись профессию
        private void tsBtnEditProf_Click(object sender, EventArgs e)
        {
            if (lvDirProfs.SelectedItems.Count > 0)
            {
                lvDirProfs_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить профессию
        private async void tsBtnDelProf_Click(object sender, EventArgs e)
        {
            if (lvDirProfs.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirProfs WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Загрузить справочник операций
        public async void LoadDirOpers()
        {
            lvDirOpers.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirOpers ORDER BY UCH,PER";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["UCH"]),  //Учсток
                    Convert.ToString(_sqlReader["PER"]),   //Переход
                    Convert.ToString(_sqlReader["Name"]),    //Название операции
                    Convert.ToString(_sqlReader["parent"]),  //id родительской операции
                    });
                    item.Font = new Font(lvDirOpers.Font, FontStyle.Regular);
                    lvDirOpers.Items.Add(item);
                    if (Convert.ToInt32(_sqlReader["UCH"]) % 2 == 0)  // Выделение цветом нечетных заходов
                    {
                        item.BackColor = Color.LightBlue;
                    }
                }

                //Division.autoResizeColumns(lvDirOpers);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirOpers.Items.Count;
        }

        //Добавить операцию
        private void tsBtnAddOper_Click(object sender, EventArgs e)
        {
            fAddOper.id_rec = "";
            fAddOper.StartPosition = FormStartPosition.CenterParent;
            fAddOper.Text = "Добавить запись";
            fAddOper.ShowDialog();
            LoadDirOpers();
        }

        //Изменить запись операцию (двойной клик по элементу)
        private void lvDirOpers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int item = 0;
            if (lvDirOpers.SelectedItems.Count > 0)
            {
                item = Convert.ToInt32(lvDirOpers.SelectedItems[0].Index);
                fAddOper.id_rec = Convert.ToString(lvDirOpers.SelectedItems[0].SubItems[0].Text);
                fAddOper.StartPosition = FormStartPosition.CenterParent;
                fAddOper.Text = "Изменить запись";
                fAddOper.ShowDialog();
                LoadDirOpers();
                //Переход к редактируемой записейе
                lvDirOpers.Items[item].Selected = true;
                lvDirOpers.EnsureVisible(item);
            }
        }

        //Изменить запись операцию
        private void tsBtnEditOper_Click(object sender, EventArgs e)
        {
            if (lvDirOpers.SelectedItems.Count > 0)
            {
                lvDirOpers_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.05.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить операцию
        private async void tsBtnDelOper_Click(object sender, EventArgs e)
        {

            if (lvDirOpers.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        string deleted_id = Convert.ToString(lvDirOpers.SelectedItems[0].SubItems[0].Text);

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirOpers WHERE id=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", deleted_id);

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.05.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //Удаление операции из разделений
                        delArrCommand = new SQLiteCommand("DELETE FROM inDivision WHERE id_oper=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", deleted_id);

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.05.44", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        // автообновление после удаления
                        LoadDirOpers();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.05.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник моделей
        public async void LoadDirModels()
        {
            lvDirModels.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT dm.id,dm.KMOD,dm.Name,dp.Name as pName,dpc.category,dpg.GRP,dm.EI FROM DirModels as dm 
                               LEFT JOIN DirProducts as dp on dp.id=dm.id_product
                               LEFT JOIN DirProdCat as dpc on dpc.id=dm.id_cat 
                               LEFT JOIN DirProdGRP as dpg on dpg.id=dm.id_grp ORDER BY dm.Name";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["KMOD"]),
                    Convert.ToString(_sqlReader["Name"]),
                    Convert.ToString(_sqlReader["pName"]),
                    Convert.ToString(_sqlReader["category"]),
                    Convert.ToString(_sqlReader["GRP"]),
                    Convert.ToString(_sqlReader["EI"])
                    });
                    item.Font = new Font(lvDirModels.Font, FontStyle.Regular);
                    lvDirModels.Items.Add(item);

                }

                //Division.autoResizeColumns(lvDirModels);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.06.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirModels.Items.Count;
        }

        //Изменить запись модель (двойной клик по элементу)
        private void lvDirModels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int item = 0;
            if (lvDirModels.SelectedItems.Count > 0)
            {
                item = Convert.ToInt32(lvDirModels.SelectedItems[0].Index);
                fAddModel.id_rec = Convert.ToString(lvDirModels.SelectedItems[0].SubItems[0].Text);
                fAddModel.StartPosition = FormStartPosition.CenterParent;
                fAddModel.Text = "Изменить запись";
                fAddModel.ShowDialog();
                LoadDirModels();        //обновить список моделей
                lvDirModels.Items[item].Selected = true;
                lvDirModels.EnsureVisible(item);
            }
        }

        //Добавить модель
        private void tsBtnAddMod_Click(object sender, EventArgs e)
        {
            fAddModel.id_rec = "";
            fAddModel.StartPosition = FormStartPosition.CenterParent;
            fAddModel.Text = "Добавить запись";
            fAddModel.ShowDialog();
            LoadDirModels();        //обновить список моделей
        }

        //Изменить запись модель
        private void tsBtnEditMod_Click(object sender, EventArgs e)
        {
            if (lvDirModels.SelectedItems.Count > 0)
            {
                lvDirModels_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.06.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить модель
        private async void tsBtnDelMod_Click(object sender, EventArgs e)
        {
            if (lvDirModels.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirModels WHERE id=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirModels.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.06.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        LoadDirModels();        //обновить список моделей

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.06.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник изделий
        public async void LoadDirProducts()
        {
            lvDirProducts.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirProducts ORDER BY id";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["Name"])
                    });
                    item.Font = new Font(lvDirProducts.Font, FontStyle.Regular);
                    lvDirProducts.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirProducts);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.07.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirProducts.Items.Count;
        }

        //Изменить запись изделие (двойной клик по элементу)
        private void lvDirProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirProducts.SelectedItems.Count > 0)
            {
                fAddProd.id_rec = Convert.ToString(lvDirProducts.SelectedItems[0].SubItems[0].Text);
                fAddProd.StartPosition = FormStartPosition.CenterParent;
                fAddProd.Text = "Изменить запись";
                fAddProd.ShowDialog();
                LoadDirProducts();
            }
        }

        //Добавить изделие
        private void tsBtnAddProd_Click(object sender, EventArgs e)
        {
            fAddProd.id_rec = "";
            fAddProd.StartPosition = FormStartPosition.CenterParent;
            fAddProd.Text = "Добавить запись";
            fAddProd.ShowDialog();
            LoadDirProducts();
        }

        //Изменить запись изделие
        private void tsBtnEditProd_Click(object sender, EventArgs e)
        {
            if (lvDirProducts.SelectedItems.Count > 0)
            {
                lvDirProducts_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить изделие
        private async void tsBtnDelProd_Click(object sender, EventArgs e)
        {
            if (lvDirProducts.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirProducts WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник тарифных ставок
        public async void LoadDirTarif()
        {
            lvDirTarif.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirTarif ORDER BY rank";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["rank"]),
                    Convert.ToString(_sqlReader["TAR_VR"]),
                    Convert.ToString(_sqlReader["K_SD"])
                    });
                    item.Font = new Font(lvDirTarif.Font, FontStyle.Regular);
                    lvDirTarif.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirTarif);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.08.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirTarif.Items.Count;
        }

        //Изменить запись тариф (двойной клик по элементу)
        private void lvDirTarif_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirTarif.SelectedItems.Count > 0)
            {
                fAddTarif.id_rec = Convert.ToString(lvDirTarif.SelectedItems[0].SubItems[0].Text);
                fAddTarif.StartPosition = FormStartPosition.CenterParent;
                fAddTarif.Text = "Изменить запись";
                fAddTarif.ShowDialog();
                LoadDirTarif();
            }
        }

        //Добавить тариф
        private void tsBtnAddTarif_Click(object sender, EventArgs e)
        {
            fAddTarif.id_rec = "";
            fAddTarif.StartPosition = FormStartPosition.CenterParent;
            fAddTarif.Text = "Добавить запись";
            fAddTarif.ShowDialog();
            LoadDirTarif();
        }

        //Изменить запись тариф
        private void tsBtnEditTarif_Click(object sender, EventArgs e)
        {
            if (lvDirTarif.SelectedItems.Count > 0)
            {
                lvDirTarif_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.08.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить тариф
        private async void tsBtnDelTarif_Click(object sender, EventArgs e)
        {
            if (lvDirTarif.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirTarif WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.08.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник норм на настилание ткани
        public async void LoadDirNormNastil()
        {
            lvDirNormNastil.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirNormNastil ORDER BY id";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["VIDTK"]),
                    Convert.ToString(_sqlReader["NORMVR"])
                    });
                    item.Font = new Font(lvDirNormNastil.Font, FontStyle.Regular);
                    lvDirNormNastil.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirNormNastil);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirNormNastil.Items.Count;
        }

        //Изменить запись норму на настилание (двойной клик по элементу)
        private void lvDirNormNastil_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirNormNastil.SelectedItems.Count > 0)
            {
                fAddNormNastil.id_rec = Convert.ToString(lvDirNormNastil.SelectedItems[0].SubItems[0].Text);
                fAddNormNastil.StartPosition = FormStartPosition.CenterParent;
                fAddNormNastil.Text = "Изменить запись";
                fAddNormNastil.ShowDialog();
                LoadDirNormNastil();
            }
        }

        //Добавить норму на настилание
        private void tsBtnAddNormNastil_Click(object sender, EventArgs e)
        {
            fAddNormNastil.id_rec = "";
            fAddNormNastil.StartPosition = FormStartPosition.CenterParent;
            fAddNormNastil.Text = "Добавить запись";
            fAddNormNastil.ShowDialog();
            LoadDirNormNastil();
        }

        //Изменить запись норму на настилание
        private void tsBtnEditNormNastil_Click(object sender, EventArgs e)
        {
            if (lvDirNormNastil.SelectedItems.Count > 0)
            {
                lvDirNormNastil_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить норму на настилание
        private async void tsBtnDelNormNastil_Click(object sender, EventArgs e)
        {
            if (lvDirNormNastil.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirNormNastil WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить норму на контроль
        public async void LoadDirNormControl()
        {
            lvDirNormControl.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT * FROM DirNormControl ORDER BY id";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["VIDTK"]),
                    Convert.ToString(_sqlReader["NORMVR"])
                    });
                    item.Font = new Font(lvDirNormControl.Font, FontStyle.Regular);
                    lvDirNormControl.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirNormControl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirNormControl.Items.Count;
        }

        //Изменить запись норму на контроль (двойной клик по элементу)
        private void lvDirNormControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirNormControl.SelectedItems.Count > 0)
            {
                fAddNormControl.id_rec = Convert.ToString(lvDirNormControl.SelectedItems[0].SubItems[0].Text);
                fAddNormControl.StartPosition = FormStartPosition.CenterParent;
                fAddNormControl.Text = "Изменить запись";
                fAddNormControl.ShowDialog();
                LoadDirNormControl();
            }
        }

        //Добавить норму на контроль
        private void tsBtnAddNormControl_Click(object sender, EventArgs e)
        {
            fAddNormControl.id_rec = "";
            fAddNormControl.StartPosition = FormStartPosition.CenterParent;
            fAddNormControl.Text = "Добавить запись";
            fAddNormControl.ShowDialog();
            LoadDirNormControl();
        }

        //Изменить запись норму на контроль
        private void tsBtnEditNormControl_Click(object sender, EventArgs e)
        {
            if (lvDirNormControl.SelectedItems.Count > 0)
            {
                lvDirNormControl_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить норму на контроль
        private async void tsBtnDelNormControl_Click(object sender, EventArgs e)
        {
            if (lvDirNormControl.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirNormControl WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.10.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить справочник подписантов
        public async void LoadDirSigners()
        {
            lvDirSigners.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT * FROM DirSigners ORDER BY ord";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["post"]),
                    Convert.ToString(_sqlReader["FIO"]),
                    Convert.ToString(_sqlReader["ord"]),
                    Enum.GetValues(typeof(PlaceEnum)).GetValue(Convert.ToInt32(_sqlReader["place"])).ToString(),
                    });
                    item.Font = new Font(lvDirSigners.Font, FontStyle.Regular);
                    lvDirSigners.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDirSigners);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirSigners.Items.Count;
        }

        //Изменить запись подписанта (двойной клик по элементу)
        private void lvDirSigners_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirSigners.SelectedItems.Count > 0)
            {
                fAddSigner.id_rec = Convert.ToString(lvDirSigners.SelectedItems[0].SubItems[0].Text);
                fAddSigner.StartPosition = FormStartPosition.CenterParent;
                fAddSigner.Text = "Изменить запись";
                fAddSigner.ShowDialog();
                LoadDirSigners();
            }
        }

        //Добавить подписанта
        private void tsBtnAddSigner_Click(object sender, EventArgs e)
        {
            fAddSigner.id_rec = "";
            fAddSigner.StartPosition = FormStartPosition.CenterParent;
            fAddSigner.Text = "Добавить запись";
            fAddSigner.ShowDialog();
            LoadDirSigners();
        }

        //Изменить запись подписанта
        private void tsBtnEditSigner_Click(object sender, EventArgs e)
        {
            if (lvDirSigners.SelectedItems.Count > 0)
            {
                lvDirSigners_MouseDoubleClick(this, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Удалить подписанта
        private async void tsBtnDelSigner_Click(object sender, EventArgs e)
        {
            if (lvDirSigners.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirSigners WHERE id=@id", _dblite);

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
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.02.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Добавить разделение
        private void tsBtnAddDivision_Click(object sender, EventArgs e)
        {
            fAddDivision.id_rec = "";
            fAddDivision.dt = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            fAddDivision.StartPosition = FormStartPosition.CenterParent;
            fAddDivision.Text = "Добавить запись";
            fAddDivision.ShowDialog();
            LoadDivisions();
        }

        //Загрузить список разделение за период
        public async void LoadDivisions()
        {
            lvDivision.Items.Clear();  //Чистим listview

            try
            {
                string query = @"SELECT div.id,div.id_model,dm.name as mName,
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
                    WHERE div.mm=" + dateTimePicker1.Value.Month.ToString() + " and div.yy="
                    + dateTimePicker1.Value.Year.ToString() +
                    " ORDER BY div.id";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);
                _m_sqlCmd.Connection = _dblite;

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["id_model"]),
                    Convert.ToString(_sqlReader["mName"]),
                    Convert.ToString(_sqlReader["pName"])+" "+Convert.ToString(_sqlReader["category"])+" "+Convert.ToString(_sqlReader["GRP"]),
                    Convert.ToString(_sqlReader["work_time"]),
                    Convert.ToString(_sqlReader["cost"])
                    });
                    item.Font = new Font(lvDivision.Font, FontStyle.Regular);
                    lvDivision.Items.Add(item);
                }

                //Division.autoResizeColumns(lvDivision);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDivision.Items.Count;
        }

        //Изменить запись разделение
        private void tsBtnEditDivision_Click(object sender, EventArgs e)
        {
            if (lvDivision.SelectedItems.Count > 0)
            {
                fAddDivision.id_rec = lvDivision.SelectedItems[0].SubItems[0].Text;   //id разделения
                fAddDivision.dt = dateTimePicker1.Value.ToString("yyyy-MM-dd");         //Дата из датапикера
                fAddDivision.id_model = lvDivision.SelectedItems[0].SubItems[1].Text;         //id модели
                fAddDivision.StartPosition = FormStartPosition.CenterParent;
                fAddDivision.Text = "Изменить запись";
                fAddDivision.ShowDialog();
                LoadDivisions();
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Division.product = lvDivision.SelectedItems[0].SubItems[3].Text;    //Запоминаем изделие в поле класса  
                Division.model = lvDivision.SelectedItems[0].SubItems[2].Text;      //Запоминаем модель в поле класса  
                tpinDivision.Parent = tabControl1;
                tpDirs.Parent = null;
                tpDirs.Parent = tabControl1;
                tpinDivision.Text = @"Разделение труда по модели " + lvDivision.SelectedItems[0].SubItems[2].Text;
                tabControl1.SelectedTab = tpinDivision;
                LoadOpersByDivision(lvDivision.SelectedItems[0].SubItems[0].Text);
            }
        }

        //Удалить разделение
        private async void tsBtnDelDivision_Click(object sender, EventArgs e)
        {
            if (lvDivision.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить разделение? Также удалится информация по начислениям для данного разделения.", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM Division WHERE id=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDivision.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.10.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //Удалить внутренности разделения
                        delArrCommand = new SQLiteCommand("DELETE FROM inDivision WHERE id_division=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDivision.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.11.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //Удалить начисления по разделению
                        delArrCommand = new SQLiteCommand("DELETE FROM PieceWork WHERE id_division=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDivision.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.12.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        // автообновление после удаления
                        LoadDivisions();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.10.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        //Загрузить операции по разделению
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
                                WHERE id_division='" + id_div + @"' ORDER BY d.UCH,d.PER";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    //Если подкатегории операции
                    if ((Convert.ToInt32(_sqlReader["parent"]) > 0) && (Convert.ToInt32(_sqlReader["parent"]) != Convert.ToInt32(_sqlReader["id_oper"])))
                    {
                        st_cnt = string.Empty;  //Порядковый номер пустой
                    }
                    else
                    {
                        counter++;
                        st_cnt = counter.ToString();
                    }

                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["UCH"]),     //Участок
                    Convert.ToString(st_cnt),               //Порядковый номер
                    Convert.ToString(_sqlReader["Name"]),    //Наименование операции
                    Convert.ToString(_sqlReader["MatRate"]), //Расход ткани
                    Convert.ToString(_sqlReader["rank"]),        //Разряд
                    Convert.ToString(_sqlReader["NVRforOper"]),  //Норма времени для операции
                    Convert.ToString(_sqlReader["workers_cnt"]), //кол-во работников
                    Convert.ToString(_sqlReader["Cost"]),        //Расценка
                    Convert.ToString(_sqlReader["NVRbyItem"]),   //Норма времени на единицу
                    Convert.ToString(_sqlReader["SumItem"])      //Стоимость единицы
                    });
                    item.Font = new Font(lvDivision.Font, FontStyle.Regular);
                    lvinDivision.Items.Add(item);
                    if (Convert.ToInt32(_sqlReader["UCH"]) % 2 == 0)  // Выделение цветом нечетных заходов
                    {
                        item.BackColor = Color.LightBlue;
                    }
                }

                //Division.autoResizeColumns(lvinDivision);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.09.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvinDivision.Items.Count;

            //Подсчет сумм по разделению и отображение в статусбаре
            Division.absSumCost = 0;
            Division.absSumNVR = 0;
            Division.absSumItem = 0;


            //Расчет сумм
            for (int i = 0; i < lvinDivision.Items.Count; i++)
            {
                Division.absSumCost += double.Parse(lvinDivision.Items[i].SubItems[8].Text);
                Division.absSumNVR += double.Parse(lvinDivision.Items[i].SubItems[9].Text);
                Division.absSumItem += double.Parse(lvinDivision.Items[i].SubItems[10].Text);

            }
            tsStatusSumCost.Text = "Расценка: " + Division.absSumCost.ToString();
            tsStatusNVRbyItem.Text = "Время обработки: " + Division.absSumNVR.ToString();
            tsStatusSumItem.Text = "Стоимость обработки: " + Division.absSumItem.ToString();
        }



        //Удалить операцию из распределения
        private async void tsBtnDelOperInDivision_Click(object sender, EventArgs e)
        {
            if (lvinDivision.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить выделенные записи?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:
                        //Пакетное удаление операций
                        foreach (ListViewItem item in lvinDivision.SelectedItems)
                        {
                            SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM inDivision WHERE id=@id", _dblite);

                            delArrCommand.Parameters.AddWithValue("id", Convert.ToString(item.SubItems[0].Text));

                            try
                            {
                                await delArrCommand.ExecuteNonQueryAsync();
                            }
                            catch (SQLiteException ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка 5.10.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            lvinDivision.Items.Remove(item);
                        }

                        // автообновление после удаления
                        LoadOpersByDivision(Division.id);

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.10.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Ввод параметров по каждой операции в разделении
        private void tsBtnEditOperInDivision_Click(object sender, EventArgs e)
        {
            if (lvinDivision.SelectedItems.Count > 0)
            {

                fEditOperByDivision.id_rec = lvinDivision.SelectedItems[0].SubItems[0].Text;   //id записи             
                fEditOperByDivision.rank = lvinDivision.SelectedItems[0].SubItems[5].Text;   //разряд               
                fEditOperByDivision.StartPosition = FormStartPosition.CenterParent;
                fEditOperByDivision.Text = "Изменить запись";
                fEditOperByDivision.ShowDialog();
                LoadOpersByDivision(Division.id);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Редактирование операций по разделению
        private void lvinDivision_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsBtnEditOperInDivision_Click(this, null);
        }



        //Создать резервную копию БД
        private void BackUpDBItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(Environment.CurrentDirectory + "\\divisionDB.db", Environment.CurrentDirectory + "\\DBbackups\\div_" + DateTime.Now.ToString("ddMMyyyy") + ".db", true);
                MessageBox.Show("Резервная копия создана. (" + Environment.CurrentDirectory + "\\DBbackups\\div_" + DateTime.Now.ToString("ddMMyyyy") + ".db)", "BackUp");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Напечатать разделение
        private void tsPrintDivision_Click(object sender, EventArgs e)
        {
            if (lvinDivision.Items.Count > 0)
            {
                string Filename = Environment.CurrentDirectory + "\\Reports\\Division" + dateTimePicker1.Value.ToString("MM_yyyy") + "_" + Division.model + ".docx";
                _Document oDoc = GetDoc(Environment.CurrentDirectory + "\\DivisionTemplate.docx", true);
                try
                {
                    oDoc.SaveAs(FileName: Filename); //Сохраняем документ
                    oDoc.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Документ уже открыт!", "Ошибка открытия документа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //System.Diagnostics.Process.Start(Filename);  //Открыть документ разделения
                System.Diagnostics.Process obj = new System.Diagnostics.Process();
                obj.StartInfo.FileName = Filename;
                obj.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized; // it Maximized application  
                obj.Start();
            }
            else
            {
                MessageBox.Show("Выберите разделение для печати.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //Получаем документ
        private _Document GetDoc(string path, bool f_div)
        {
            Word._Application oWord = new Word.Application();
            _Document oDoc = oWord.Documents.Add(path);
            SetTemplate(oDoc, f_div);
            return oDoc;
        }

        //коэф. для перевода секунд в часы
        const double hours_in_sec = 0.000278;

        //Заполняем шаблон
        private async void SetTemplate(Microsoft.Office.Interop.Word._Document oDoc, bool f_div)
        {
            int i = 0;
            double absCount = 0;
            double absCost = 0;
            double absSum = 0;
            double absCntInCard = 0;
            string query = "";
            try
            {
                if (f_div)  //Если печать разделения
                {
                    //Подписант руководитель 
                    try
                    {
                        query = @"SELECT * FROM DirSigners where place=" + (int)PlaceEnum.DivisionTitul + " ORDER BY ord LIMIT 1";

                        _m_sqlCmd = new SQLiteCommand(query, _dblite);

                        _sqlReader = _m_sqlCmd.ExecuteReader();

                        while (await _sqlReader.ReadAsync())
                        {
                            oDoc.Bookmarks["Signer0"].Range.Text = Convert.ToString(_sqlReader["post"]);
                            oDoc.Bookmarks["Signer0FIO"].Range.Text = Convert.ToString(_sqlReader["FIO"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (_sqlReader != null && !_sqlReader.IsClosed)
                            _sqlReader.Close();
                    }

                    double NVRsec = Division.absSumNVR;
                    //Титульный лист
                    oDoc.Bookmarks["product"].Range.Text = Division.product;
                    oDoc.Bookmarks["model"].Range.Text = Division.model;
                    oDoc.Bookmarks["mmyy"].Range.Text = "за " + dateTimePicker1.Value.ToString("Y").ToUpper() + " г.";
                    oDoc.Bookmarks["sumNVR"].Range.Text = NVRsec.ToString() + "с = " + Math.Round(NVRsec * hours_in_sec, 2) + " ч";
                    oDoc.Bookmarks["sumItem"].Range.Text = Division.absSumItem.ToString();

                    //Подписанты (2 первых)
                    try
                    {
                        query = @"SELECT * FROM DirSigners where place=" + (int)PlaceEnum.DivisionBottom + " ORDER BY ord LIMIT 2";

                        _m_sqlCmd = new SQLiteCommand(query, _dblite);

                        _sqlReader = _m_sqlCmd.ExecuteReader();

                        while (await _sqlReader.ReadAsync())
                        {
                            i++;
                            oDoc.Bookmarks["Signer" + i.ToString()].Range.Text = Convert.ToString(_sqlReader["post"]) +
                                                @"                       /" + Convert.ToString(_sqlReader["FIO"]) + @"/";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (_sqlReader != null && !_sqlReader.IsClosed)
                            _sqlReader.Close();
                    }

                    //Таблица разделения
                    //Название модели
                    oDoc.Bookmarks["model2"].Range.Text = Division.model;
                    Table wTable = oDoc.Tables[1];
                    for (int row = 0; row < lvinDivision.Items.Count; row++) //проход по записям
                    {
                        for (int col = 0; col < lvinDivision.Items[row].SubItems.Count - 2; col++)  //проход по столбцам
                        {
                            wTable.Cell(row + 2, col + 1).Range.Text = lvinDivision.Items[row].SubItems[col + 2].Text;
                        }
                        wTable.Rows.Add();  //Добавить запись
                    }

                    //ИТОГО
                    Table sumTable = oDoc.Tables[2];
                    sumTable.Cell(1, 2).Range.Text = Division.absSumCost.ToString();  //Суммарная расценка
                    sumTable.Cell(1, 3).Range.Text = Division.absSumNVR.ToString();     //Суммарная норма времени
                    sumTable.Cell(1, 4).Range.Text = Division.absSumItem.ToString();    //Сумарная стоимость

                }
                else    //Если печать начислений
                {
                    try
                    {
                        //Таблица начислений
                        oDoc.Bookmarks["mmyy"].Range.Text = "за " + dateTimePicker1.Value.ToString("Y").ToUpper() + " г.";
                        //Название модели
                        oDoc.Bookmarks["model2"].Range.Text = Division.model;
                        Table wTable = oDoc.Tables[1];
                        for (int row = 0; row < lvPieceWork.Items.Count; row++) //проход по записям
                        {
                            wTable.Rows.Add();  //Добавить запись
                            for (int col = 0; col < lvPieceWork.Items[row].SubItems.Count - 1; col++)  //проход по столбцам
                            {
                                wTable.Cell(row + 2, col + 1).Range.Text = lvPieceWork.Items[row].SubItems[col + 1].Text;
                            }
                            absCount += Math.Round(Convert.ToDouble(lvPieceWork.Items[row].SubItems[5].Text), 5);
                            absCost += Math.Round(Convert.ToDouble(lvPieceWork.Items[row].SubItems[6].Text), 5);
                            absSum += Math.Round(Convert.ToDouble(lvPieceWork.Items[row].SubItems[7].Text), 5);
                            absCntInCard += Math.Round(Convert.ToDouble(lvPieceWork.Items[row].SubItems[8].Text), 5);
                        }

                        //ИТОГО
                        Table sumTable = oDoc.Tables[2];
                        sumTable.Cell(1, 2).Range.Text = absCount.ToString();  //Суммарная расценка
                        sumTable.Cell(1, 3).Range.Text = absCost.ToString();     //Суммарная норма времени
                        sumTable.Cell(1, 4).Range.Text = absSum.ToString();    //Сумарная стоимость
                        sumTable.Cell(1, 5).Range.Text = absCntInCard.ToString();    //Сумарное кол-во по картам

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (_sqlReader != null && !_sqlReader.IsClosed)
                            _sqlReader.Close();
                    }

                    //Подписанты (2 первых)
                    try
                    {
                        query = @"SELECT * FROM DirSigners where place=" + (int)PlaceEnum.PieceworkBottom + " ORDER BY ord LIMIT 2";

                        _m_sqlCmd = new SQLiteCommand(query, _dblite);

                        _sqlReader = _m_sqlCmd.ExecuteReader();

                        while (await _sqlReader.ReadAsync())
                        {
                            i++;
                            oDoc.Bookmarks["Signer" + i.ToString()].Range.Text = Convert.ToString(_sqlReader["post"]);
                            oDoc.Bookmarks["Signer" + i.ToString() + "FIO"].Range.Text = Convert.ToString(_sqlReader["FIO"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (_sqlReader != null && !_sqlReader.IsClosed)
                            _sqlReader.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.00.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ПО \"Разделение труда\", версия 2.1 (11.06.2024)", "О программе");
        }

        //Печать разделения из меню
        private void printDivisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsPrintDivision_Click(this, null);
        }

        //Скопировать разделение
        private async void tsBtnCopyDivision_Click(object sender, EventArgs e)
        {
            if (lvinDivision.Items.Count > 0)
            {
                DialogResult res = MessageBox.Show("В разделении уже есть операции. Они будут удалены перед копированием. Продолжить?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM inDivision WHERE id_division=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Division.id);

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.10.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        fSelectDivisionToCopy.StartPosition = FormStartPosition.CenterParent;
                        fSelectDivisionToCopy.ShowDialog();
                        break;
                }
            }
            else
            {
                fSelectDivisionToCopy.StartPosition = FormStartPosition.CenterParent;
                fSelectDivisionToCopy.ShowDialog();
            }

            LoadOpersByDivision(Division.id);
        }

        public async void LoadDirCat1()
        {
            lvDirCat1.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT * FROM DirProdCat ORDER BY Category";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["Category"])
                    });
                    item.Font = new Font(lvDirCat1.Font, FontStyle.Regular);
                    lvDirCat1.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirCat1.Items.Count;
        }

        public async void LoadDirCat2()
        {
            lvDirCat2.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT * FROM DirProdGRP ORDER BY GRP";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["GRP"])
                    });
                    item.Font = new Font(lvDirCat2.Font, FontStyle.Regular);
                    lvDirCat2.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.09", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvDirCat2.Items.Count;
        }

        //Добавить категорию
        private void tsBtnCat1Add_Click(object sender, EventArgs e)
        {
            fAddCat1.id_rec = "";
            fAddCat1.StartPosition = FormStartPosition.CenterParent;
            fAddCat1.Text = "Добавить запись";
            fAddCat1.ShowDialog();
            LoadDirCat1();
        }

        //Изменить категорию (двойной клик)
        private void lvDirCat1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvDirCat1.SelectedItems.Count > 0)
            {
                fAddCat1.id_rec = Convert.ToString(lvDirCat1.SelectedItems[0].SubItems[0].Text);
                fAddCat1.StartPosition = FormStartPosition.CenterParent;
                fAddCat1.Text = "Изменить запись";
                fAddCat1.ShowDialog();
                LoadDirCat1();
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Изменить категорию 
        private void tsBtnCat1Edit_Click(object sender, EventArgs e)
        {
            lvDirCat1_MouseDoubleClick(this, null);
        }

        //Удалить категорию
        private async void tsBtnCat1Del_Click(object sender, EventArgs e)
        {
            if (lvDirCat1.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirProdCat WHERE id=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirCat1.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.07.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirCat1();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Добавить категорию
        private void tsBtnCat2Add_Click(object sender, EventArgs e)
        {
            fAddCat2.id_rec = "";
            fAddCat2.StartPosition = FormStartPosition.CenterParent;
            fAddCat2.Text = "Добавить запись";
            fAddCat2.ShowDialog();
            LoadDirCat2();
        }

        //Изменить категорию
        private void tsBtnCat2Edit_Click(object sender, EventArgs e)
        {
            if (lvDirCat2.SelectedItems.Count > 0)
            {
                fAddCat2.id_rec = Convert.ToString(lvDirCat2.SelectedItems[0].SubItems[0].Text);
                fAddCat2.StartPosition = FormStartPosition.CenterParent;
                fAddCat2.Text = "Изменить запись";
                fAddCat2.ShowDialog();
                LoadDirCat2();
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Изменить категорию (двойной клик)
        private void lvDirCat2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsBtnCat2Edit_Click(this, null);
        }

        //Удалить категорию
        private async void tsBtnCat2Del_Click(object sender, EventArgs e)
        {
            if (lvDirCat2.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM DirProdGRP WHERE id=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvDirCat2.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.07.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadDirCat2();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Загрузить начисления по разделению
        public async void LoadPieceWork(string id_div)
        {
            lvPieceWork.Items.Clear();  //Чистим listview2

            try
            {
                string query = @"SELECT pw.*,dw.tab_nom,dw.fio,d.Name as OperName, 
                                IFNULL(ROUND(i.NVRforOper * dt.TAR_VR,5),0) as Cost
                                FROM PieceWork as pw
                                LEFT JOIN inDivision as i on i.id = pw.id_indivision
                                LEFT JOIN DirWorkers as dw on dw.id = pw.id_worker
                                LEFT JOIN DirTarif as dt on dt.rank=i.rank
                                LEFT JOIN DirOpers as d on d.id=i.id_oper
                                WHERE pw.id_division=" + id_div + " ORDER BY cast(tab_nom as int)";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();

                while (await _sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(_sqlReader["id"]),
                    Convert.ToString(_sqlReader["cardnum"]),
                    Convert.ToString(_sqlReader["tab_nom"]),
                    Convert.ToString(_sqlReader["FIO"]),
                    Convert.ToString(_sqlReader["OperName"]),
                    Convert.ToString(_sqlReader["cnt"]),
                    Convert.ToString(_sqlReader["Cost"]),
                    Convert.ToString(Math.Round(Convert.ToDouble(_sqlReader["Cost"])*Convert.ToDouble(_sqlReader["cnt"]),5)),
                    Convert.ToString(_sqlReader["cnt_in_card"]),
                    });
                    item.Font = new Font(lvPieceWork.Font, FontStyle.Regular);
                    lvPieceWork.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.11", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }
            toolStripStatusLabel1.Text = "Кол-во записей: " + lvPieceWork.Items.Count;
        }

        //Перейти к начислению
        private void tsBtnGotoPiecework_Click(object sender, EventArgs e)
        {
            if (lvDivision.SelectedItems.Count > 0)
            {
                Division.id = lvDivision.SelectedItems[0].SubItems[0].Text;
                Division.mm = dateTimePicker1.Value.Month.ToString();
                Division.yy = dateTimePicker1.Value.Year.ToString();
                Division.product = lvDivision.SelectedItems[0].SubItems[3].Text;    //Запоминаем изделие в поле класса  
                Division.model = lvDivision.SelectedItems[0].SubItems[2].Text;      //Запоминаем модель в поле класса  
                tpPieceWork.Parent = tabControl1;
                tpDirs.Parent = null;
                tpDirs.Parent = tabControl1;
                tpPieceWork.Text = @"Информация для начисления по модели " + lvDivision.SelectedItems[0].SubItems[2].Text;
                tabControl1.SelectedTab = tpPieceWork;
                LoadPieceWork(lvDivision.SelectedItems[0].SubItems[0].Text);
            }
            else
            {
                MessageBox.Show("Не выбрано разделение!", "Выберите разделение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Добавить начисление
        private void tsBtnAddPieceWork_Click(object sender, EventArgs e)
        {
            fAddPieceWork.id_rec = "";
            fAddPieceWork.StartPosition = FormStartPosition.CenterParent;
            fAddPieceWork.Text = "Добавить запись";
            fAddPieceWork.ShowDialog();
            LoadPieceWork(Division.id);
        }

        //Изменить начисление
        private void tsBtnEditPieceWork_Click(object sender, EventArgs e)
        {
            if (lvPieceWork.SelectedItems.Count > 0)
            {
                fAddPieceWork.id_rec = lvPieceWork.SelectedItems[0].SubItems[0].Text;
                fAddPieceWork.StartPosition = FormStartPosition.CenterParent;
                fAddPieceWork.Text = "Изменить запись";
                fAddPieceWork.ShowDialog();
                LoadPieceWork(Division.id);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lvPieceWork_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsBtnEditPieceWork_Click(this, null);
        }

        private async void tsBtnDelPieceWork_Click(object sender, EventArgs e)
        {
            if (lvPieceWork.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Удаление...", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SQLiteCommand delArrCommand = new SQLiteCommand("DELETE FROM PieceWork WHERE id=@id", _dblite);

                        delArrCommand.Parameters.AddWithValue("id", Convert.ToString(lvPieceWork.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await delArrCommand.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка 5.07.04", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // автообновление после удаления
                        LoadPieceWork(Division.id);

                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка 5.07.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Печать  информации по начислениям
        private void tsBtnPrintPieceWork_Click(object sender, EventArgs e)
        {
            if (lvPieceWork.Items.Count > 0)
            {
                string Filename = Environment.CurrentDirectory + "\\Reports\\Nachisl_" + dateTimePicker1.Value.ToString("MM_yyyy") + "_" + Division.model + ".docx";
                _Document oDoc = GetDoc(Environment.CurrentDirectory + "\\PwbyDivTemplate.docx", false);
                try
                {
                    oDoc.SaveAs(FileName: Filename); //Сохраняем документ
                    oDoc.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Документ уже открыт!", "Ошибка открытия документа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                System.Diagnostics.Process obj = new System.Diagnostics.Process();
                obj.StartInfo.FileName = Filename;
                obj.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized; // it Maximized application  
                obj.Start();  //Открыть документ начисления                                                        
            }
            else
            {
                MessageBox.Show("Выберите разделение для печати.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Печать начислений за месяц
        private void tsBtnPrintMonth_Click(object sender, EventArgs e)
        {
            if (lvDivision.Items.Count > 0)
            {
                string Filename = Environment.CurrentDirectory + "\\Reports\\Nachisl_" + dateTimePicker1.Value.ToString("MM_yyyy") + ".docx";
                _Document oDoc = GetDocPW(Environment.CurrentDirectory + "\\PieceworkTemplate.docx");
                try
                {
                    oDoc.SaveAs(FileName: Filename); //Сохраняем документ
                    oDoc.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Документ уже открыт!", "Ошибка открытия документа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //var prc = new System.Diagnostics.Process();
                //prc.StartInfo.FileName = Filename;
                //prc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                //prc.Start();  //Открыть документ начисления                                                        
                System.Diagnostics.Process obj = new System.Diagnostics.Process();
                obj.StartInfo.FileName = Filename;
                obj.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized; // it Maximized application  
                obj.Start();
            }
            else
            {
                MessageBox.Show("В выбранном месяце не было разделений.", "Ошибка 5.09.05", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Получаем документ
        private _Document GetDocPW(string path)
        {
            Word._Application oWord = new Word.Application();
            _Document oDoc = oWord.Documents.Add(path);
            SetTemplatePW(oDoc);
            return oDoc;
        }

        //Заполняем шаблон
        private async void SetTemplatePW(Microsoft.Office.Interop.Word._Document oDoc)
        {
            int i = 0;
            double absCount = 0;
            double absCost = 0;
            double absSum = 0;
            double absCardCount = 0;
            string query = "";
            List<string> tabList = new List<string>();
            List<string> cardList = new List<string>();

            try
            {
                oDoc.Bookmarks["mmyy"].Range.Text = "за " + dateTimePicker1.Value.ToString("Y").ToUpper() + " г.";


                //Подписанты (2 первых)
                try
                {
                    query = @"SELECT * FROM DirSigners where place=" + (int)PlaceEnum.PieceworkBottom + " ORDER BY ord LIMIT 2";

                    _m_sqlCmd = new SQLiteCommand(query, _dblite);

                    _sqlReader = _m_sqlCmd.ExecuteReader();

                    while (await _sqlReader.ReadAsync())
                    {
                        i++;
                        oDoc.Bookmarks["Signer" + i.ToString()].Range.Text = Convert.ToString(_sqlReader["post"]);
                        oDoc.Bookmarks["Signer" + i.ToString() + "FIO"].Range.Text = Convert.ToString(_sqlReader["FIO"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (_sqlReader != null && !_sqlReader.IsClosed)
                        _sqlReader.Close();
                }

                Table wTable = oDoc.Tables[1];
                //Табельные номера
                query = @"SELECT distinct id_worker,dw.Tab_nom,dw.FIO FROM Piecework as pw
                LEFT JOIN DirWorkers as dw on dw.id = pw.id_worker
                WHERE mm=" + dateTimePicker1.Value.Month.ToString() + " AND yy=" + dateTimePicker1.Value.Year.ToString() + " order by cast(tab_nom as int)";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();
                i = 2;

                while (await _sqlReader.ReadAsync())
                {
                    i++;
                    wTable.Columns.Add();  //Добавить столбец
                    wTable.Cell(1, i).Range.Text = Convert.ToString(_sqlReader["tab_nom"]);  //+"\n"+ Convert.ToString(_sqlReader["FIO"]);                                                                                                                              

                    tabList.Add(Convert.ToString(_sqlReader["tab_nom"])); //Собираем табельные номера в список
                }

                //Номера карт
                query = @"SELECT distinct cardnum,ifnull(cnt_in_card,0) as cnt_in_card FROM Piecework
                WHERE mm=" + dateTimePicker1.Value.Month.ToString() + " AND yy=" + dateTimePicker1.Value.Year.ToString() + " order by cardnum";

                _m_sqlCmd = new SQLiteCommand(query, _dblite);

                _sqlReader = _m_sqlCmd.ExecuteReader();
                i = 0;
                while (await _sqlReader.ReadAsync())
                {
                    i++;
                    wTable.Rows.Add();  //Добавить запись
                    wTable.Cell(i + 2, 1).Range.Text = Convert.ToString(_sqlReader["cardnum"]);  //номера карт в первый столбец
                    wTable.Cell(i + 2, 2).Range.Text = Convert.ToString(_sqlReader["cnt_in_card"]);  //кол-во по карте

                    if (_sqlReader["cnt_in_card"] != DBNull.Value)
                    {
                        absCardCount += Convert.ToInt32(_sqlReader["cnt_in_card"]);
                    }

                    cardList.Add(Convert.ToString(_sqlReader["cardnum"]));  //Собираем номера карт в список
                }
                wTable.Cell(i + 3, 1).Range.Text = "ИТОГО";
                //wTable.Cell(i + 3, 2).Range.Text = absCardCount.ToString();  //Cумма по картам

                for (int row = 0; row < cardList.Count; row++) //проход по строкам (номера карт)
                {
                    for (int col = 0; col < tabList.Count; col++)  //проход по столбцам (табельные номера)
                    {
                        query = @"SELECT pw.cnt,
                                IFNULL(ROUND(i.NVRforOper * dt.TAR_VR,5),0) as Cost,
                                ROUND(SUM(IFNULL(ROUND(i.NVRforOper * dt.TAR_VR,5),0) * pw.cnt),5) as SumPW
                                FROM PieceWork as pw
                                LEFT JOIN inDivision as i on i.id = pw.id_indivision
                                LEFT JOIN DirWorkers as dw on dw.id = pw.id_worker
                                LEFT JOIN DirTarif as dt on dt.rank=i.rank
                WHERE pw.mm=" + dateTimePicker1.Value.Month.ToString() + " AND pw.yy=" + dateTimePicker1.Value.Year.ToString() +
                " AND cardnum=" + cardList[row] + " AND tab_nom=" + tabList[col] + " GROUP BY cardnum";

                        _m_sqlCmd = new SQLiteCommand(query, _dblite);

                        _sqlReader = _m_sqlCmd.ExecuteReader();
                        if (_sqlReader.HasRows)
                        {
                            while (await _sqlReader.ReadAsync())
                            {
                                wTable.Cell(row + 3, col + 3).Range.Text = Convert.ToDouble(_sqlReader["SumPW"]).ToString();
                            }
                        }
                        else
                        {
                            wTable.Cell(row + 3, col + 3).Range.Text = "0";
                        }
                    }
                }
                for (int col = 0; col < tabList.Count + 1; col++)
                    wTable.Cell(i + 3, col + 2).Formula("= SUM(ABOVE)");  //Cумма по картам
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.01.07", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_sqlReader != null && !_sqlReader.IsClosed)
                    _sqlReader.Close();
            }




        }

        //Печать разделения за месяц из разделения
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tsBtnPrintMonth_Click(this, null);
        }
    }
}
