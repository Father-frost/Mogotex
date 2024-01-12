using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace WorkDivision
{
    public partial class fSelectDivisionToCopy : Form
    {
        private SQLiteConnection dblite;
        private SQLiteCommand m_sqlCmd, insertDirCommand;
        private SQLiteDataReader sqlReader;
        private liteDB liteDB;

        public fSelectDivisionToCopy()
        {
            InitializeComponent();
            liteDB = new liteDB();
        }

        private void fSelectDivisionToCopy_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
            dblite.Open();

            lvDivisionsToCopy.GridLines = true;
            lvDivisionsToCopy.FullRowSelect = true;
            lvDivisionsToCopy.View = View.Details;
            lvDivisionsToCopy.Font = new Font(lvDivisionsToCopy.Font, FontStyle.Bold);
            lvDivisionsToCopy.Columns.Add("id");
            lvDivisionsToCopy.Columns.Add("Месяц");
            lvDivisionsToCopy.Columns.Add("Год");
            lvDivisionsToCopy.Columns.Add("Модель");
            lvDivisionsToCopy.Columns.Add("Вид изделия");
            lvDivisionsToCopy.Columns.Add("Продолжительность");
            lvDivisionsToCopy.Columns.Add("Стоимость");

            LoadDivisionsForCopy(Division.id);
            Division.autoResizeColumns(lvDivisionsToCopy);
        }

        private async void LoadDivisionsForCopy(string without_id)
        {
            lvDivisionsToCopy.Items.Clear();  //Чистим listview

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
                    WHERE id_division=div.id) as cost, div.mm, div.yy
                    FROM Division as div
                    LEFT JOIN DirModels as dm on div.id_model=dm.id 
                    LEFT JOIN DirProducts as dp on dm.id_product=dp.id
                    LEFT JOIN DirProdCat as dc on dm.id_cat=dc.id
                    LEFT JOIN DirProdGRP as dg on dm.id_grp=dg.id
                    WHERE div.id<>"+Division.id+
                    " ORDER BY div.mm, div.yy";

                m_sqlCmd = new SQLiteCommand(query, dblite);
                m_sqlCmd.Connection = dblite;
                sqlReader = m_sqlCmd.ExecuteReader();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["id"]),
                    Convert.ToString(sqlReader["mm"]),
                    Convert.ToString(sqlReader["yy"]),
                    Convert.ToString(sqlReader["mName"]),
                    Convert.ToString(sqlReader["pName"])+" "+Convert.ToString(sqlReader["category"])+" "+Convert.ToString(sqlReader["GRP"]),
                    Convert.ToString(sqlReader["work_time"]),
                    Convert.ToString(sqlReader["cost"])
                    });
                    item.Font = new Font(lvDivisionsToCopy.Font, FontStyle.Regular);
                    lvDivisionsToCopy.Items.Add(item);
                }

                Division.autoResizeColumns(lvDivisionsToCopy);

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
        }

        private async void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                insertDirCommand = new SQLiteCommand(@"INSERT INTO inDivision (id_division,id_oper,rank,MatRate, NVRforOper, workers_cnt, mm,yy) 
                                                 SELECT @id_div,id_oper, rank, MatRate, NVRforOper, workers_cnt,01,2024 FROM inDivision where id_division=@copied_id",dblite);
                insertDirCommand.Parameters.AddWithValue("id_div", Division.id);
                insertDirCommand.Parameters.AddWithValue("mm", Division.mm);
                insertDirCommand.Parameters.AddWithValue("yy", Division.yy);
                insertDirCommand.Parameters.AddWithValue("copied_id", lvDivisionsToCopy.SelectedItems[0].SubItems[0].Text);    //id копируемого разделения

                await insertDirCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 8.08.08", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                this.Close();
            }
        }
    }
}
