using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WorkDivision
{
	public partial class fOpersList : Form
    { 
        private SQLiteConnection dblite;
        private SQLiteDataReader sqlReader;
        SQLiteCommand m_sqlCmd = null;
        liteDB liteDB = new liteDB();
        SQLiteCommand insertDirCommand = null;

        public fOpersList()
        {
            InitializeComponent();
        }

        //Поиск операции в справочнике
        private void cbOperation_TextUpdate(object sender, EventArgs e)
        {
            DataTable DF = new DataTable();
            DF.Clear();
            string query = @"SELECT name, id FROM DirOpers WHERE 
                                        Name like '%" + cbOperation.Text + @"%'";
            if (this.cbOperation.Text != "")
            {
                string st = cbOperation.Text;
                cbOperation.Items.Clear();
                treeView1.Nodes.Clear();
                DF = liteDB.GetLiteData(query);
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
            else
            {
                //Выбор всех операций
                getOpersForModel();
            }
            cbOperation.SelectionStart = cbOperation.Text.Length;

        }
        private void cbOperation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(this,null);
            }
        }

        private async void getOpersForModel()
        {
            treeView1.Nodes.Clear();
            try
            {
                string query = @"SELECT * FROM DirOpers ORDER BY PER";
                m_sqlCmd = new SQLiteCommand(query, dblite);
                sqlReader = m_sqlCmd.ExecuteReader();
                while (await sqlReader.ReadAsync())
                {
                    var node = new TreeNode(sqlReader["Name"].ToString());
                    node.Tag = sqlReader["id"].ToString();
                    treeView1.Nodes.Add(node);
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

        private void fOpersList_Load(object sender, EventArgs e)
        {
            //Подключение к БД
            dblite = liteDB.GetConn();
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