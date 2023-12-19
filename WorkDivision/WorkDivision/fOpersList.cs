using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace WorkDivision
{
    public partial class fOpersList : Form
    {
        //Поделючение к базе PED
        SqlConnection connMed;
        //MedDB meddb = null;

        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        SQLiteCommand m_sqlCmd = null;

        SQLiteCommand updateDirCommand = null;
        SQLiteCommand insertDirCommand = null;

        public fOpersList()
        {
            InitializeComponent();
            //meddb = new MedDB();        // БД PED
            //connMed = meddb.GetConnection();
        }

        //Кнопка Найти
        private async void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void fAddOutput_Load(object sender, EventArgs e)
        {

        }

        private void fAddSickList_FormClosed(object sender, FormClosedEventArgs e)
        {
            dblite.Close();
        }

        private void cbFIO_TextUpdate(object sender, EventArgs e)
        {
            DataTable DF = new DataTable();
            DF.Clear();


            if (this.cbOperation.Text != "")
            {
                string st = cbOperation.Text;
                cbOperation.Items.Clear();
                treeView1.Nodes.Clear();
                //Запрос к базе данных
                string query = @"SELECT name, id FROM DirOpers WHERE 
                                        Name like '%" + cbOperation.Text + @"%'";
                DF = GetMedData(query);
                if (DF.Rows.Count > 0)
                {
                    for (int i = 0; i < DF.Rows.Count; i++)
                    {
                        cbOperation.Items.Add(DF.Rows[i].ItemArray[0].ToString());
                        var node = new TreeNode(DF.Rows[i].ItemArray[0].ToString());
                        node.Tag = DF.Rows[i].ItemArray[1].ToString();  //id
                        treeView1.Nodes.Add(node);

                        cbOperation.SelectionStart = cbOperation.Text.Length;
                    }
                    cbOperation.SelectionStart = cbOperation.Text.Length;
                    //cbDeed.DroppedDown = true;
                    cbOperation.Text = st;

                }
                else
                {
                    cbOperation.Text = st;
                    cbOperation.SelectionStart = cbOperation.Text.Length;
                    cbOperation.Items.Clear();
                    treeView1.Nodes.Clear();
                }
            }
            cbOperation.SelectionStart = cbOperation.Text.Length;
        }

        public DataTable GetMedData(string query)
        {
            //connMed.Open();
            DataTable dt = new DataTable();

            try
            {
                SqlCommand comm = new SqlCommand(query, connMed);

                SqlDataAdapter da = new SqlDataAdapter(comm);
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
                //connMed.Close();
            }

            return dt;
        }

        private void cbDeed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //btnSearch.Select();
                btnOk_Click(this,null);
            }
        }

        private async void getOpersForModel()
        {
            string st;
            treeView1.Nodes.Clear();
            //TreeNode visitNode = new TreeNode("Консультации и приемы");
            try
            {
                string query = @"SELECT * FROM DirOpers ORDER BY PER";
                m_sqlCmd = new SQLiteCommand(query, dblite);
                sqlReader = m_sqlCmd.ExecuteReader();
                //treeView1.Nodes[1].Nodes.Remove();
                while (await sqlReader.ReadAsync())
                {
                    //MessageBox.Show(st + @"(" + Convert.ToDateTime(sqlMedReader["visit_date"]).ToString("dd.MM.yyyy HH:mm") + @")");
                    //st = sqlReader["UCH"].ToString() + " " + sqlReader["Name"].ToString();
                    var node = new TreeNode(sqlReader["Name"].ToString());
                    node.Tag = sqlReader["id"].ToString();
                    treeView1.Nodes.Add(node);
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
            //treeView1.Nodes.Add(visitNode);
            //treeView1.Expand(); //Раскрываем приемы
            //treeView1.SelectedNode = visitNode.LastNode; //Выделяем последний прием
        }

        private void fDeedsList_Load(object sender, EventArgs e)
        {
            dblite = new SQLiteConnection("Data Source=divisionDB.db;Version=3;");
            dblite.Open();
            getOpersForModel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (TreeNode opersnode in treeView1.Nodes)
                {
                    if (opersnode.Checked)
                    {
                        insertDirCommand = new SQLiteCommand(@"INSERT INTO inDivision (id_division,id_oper,mm,yy) 
                        VALUES (@id_division,@id_oper,@mm,@yy)", dblite);
                        insertDirCommand.Parameters.AddWithValue("id_division", Division.id);
                        insertDirCommand.Parameters.AddWithValue("id_oper", Convert.ToInt32(opersnode.Tag));
                        insertDirCommand.Parameters.AddWithValue("mm",Division.mm);
                        insertDirCommand.Parameters.AddWithValue("yy",Division.yy);
                        await insertDirCommand.ExecuteNonQueryAsync();
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка 5.21.08", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}