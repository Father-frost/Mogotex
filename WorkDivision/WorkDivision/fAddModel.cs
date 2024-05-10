using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WorkDivision
{
	public partial class fAddModel : Form
	{
		private SQLiteConnection dblite;
		private SQLiteDataReader sqlReader;
		SQLiteCommand m_sqlCmd = null;
		liteDB liteDB = new liteDB();

		public fAddModel()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (id_rec == "")  //Если  id записи не передан (новая запись) 
			{

				string querySQLite = @"INSERT INTO DirModels (Name,KMOD,EI,id_product,id_cat,id_grp) 
                                        VALUES (@Name,@KMOD,@EI,@id_product,@id_cat,@id_grp)";
				m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
				m_sqlCmd.Parameters.AddWithValue("@Name", tbName.Text);
				m_sqlCmd.Parameters.AddWithValue("@KMOD", tbKMOD.Text);
				m_sqlCmd.Parameters.AddWithValue("@EI", tbEI.Text);
				m_sqlCmd.Parameters.AddWithValue("@id_product", cbProduct.SelectedValue);
				m_sqlCmd.Parameters.AddWithValue("@id_cat", cbCatProd.SelectedValue);
				m_sqlCmd.Parameters.AddWithValue("@id_grp", cbProdGRP.SelectedValue);
			}
			else
			{
				string querySQLite = @"UPDATE DirModels SET Name=@Name, KMOD=@KMOD, EI=@EI, id_product=@id_product, id_cat=@id_cat,
                                    id_grp=@id_grp WHERE id=" + id_rec;
				m_sqlCmd = new SQLiteCommand(querySQLite, dblite);
				m_sqlCmd.Parameters.AddWithValue("@Name", tbName.Text);
				m_sqlCmd.Parameters.AddWithValue("@KMOD", tbKMOD.Text);
				m_sqlCmd.Parameters.AddWithValue("@EI", tbEI.Text);
				m_sqlCmd.Parameters.AddWithValue("@id_product", cbProduct.SelectedValue);
				m_sqlCmd.Parameters.AddWithValue("@id_cat", cbCatProd.SelectedValue);
				m_sqlCmd.Parameters.AddWithValue("@id_grp", cbProdGRP.SelectedValue);
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
			//Подключение к БД
			dblite = liteDB.GetConn();
			dblite.Open();
			DataTable dt = new DataTable();

			string query = @"SELECT id,Name as prodName FROM DirProducts";
			string query1 = @"SELECT id,category FROM DirProdCat order by Category";
			string query2 = @"SELECT id,GRP FROM DirProdGRP order by GRP";
			try
			{

				SQLiteCommand m_sqlCmd = new SQLiteCommand(query, dblite);
				SQLiteDataAdapter da = new SQLiteDataAdapter(m_sqlCmd);
				DataSet ds = new DataSet();

				da.Fill(ds);
				dt = ds.Tables[0];

				cbProduct.DataSource = dt.AsDataView();
				cbProduct.DisplayMember = "prodName";
				cbProduct.ValueMember = "id";


				m_sqlCmd = new SQLiteCommand(query1, dblite);
				da = new SQLiteDataAdapter(m_sqlCmd);

				DataSet ds1 = new DataSet();
				da.Fill(ds1);
				dt = ds1.Tables[0];

				cbCatProd.DataSource = dt.AsDataView();
				cbCatProd.DisplayMember = "Category";
				cbCatProd.ValueMember = "id";

				m_sqlCmd = new SQLiteCommand(query2, dblite);
				da = new SQLiteDataAdapter(m_sqlCmd);

				DataSet ds2 = new DataSet();
				da.Fill(ds2);
				dt = ds2.Tables[0];

				cbProdGRP.DataSource = dt.AsDataView();
				cbProdGRP.DisplayMember = "GRP";
				cbProdGRP.ValueMember = "id";


				//Если выбран существующий интервал, загружаем параметры
				if (id_rec != "")
				{
					//MessageBox.Show(id_rec.ToString());
					GetModel();
				}
				else
				{
					tbName.Text = "";
					tbKMOD.Text = "";
					tbEI.Text = "";
					cbProduct.Text = "";
					cbCatProd.Text = "";
					cbProdGRP.Text = "";
				}

			}
			catch (SQLiteException ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		public async void GetModel()
		{
			try
			{
				string query = @"SELECT dm.Name,dm.KMOD,dm.EI,dp.Name as pName,dpc.category, dpg.GRP FROM DirModels as dm 
                               LEFT JOIN DirProducts as dp on dp.id=dm.id_product
                               LEFT JOIN DirProdCat as dpc on dpc.id=dm.id_cat 
                               LEFT JOIN DirProdGRP as dpg on dpg.id=dm.id_grp WHERE dm.id =" + id_rec;

				m_sqlCmd = new SQLiteCommand(query, dblite);

				sqlReader = m_sqlCmd.ExecuteReader();

				while (await sqlReader.ReadAsync())
				{
					tbName.Text = Convert.ToString(sqlReader["Name"]);
					tbKMOD.Text = Convert.ToString(sqlReader["KMOD"]);
					tbEI.Text = Convert.ToString(sqlReader["EI"]);
					cbProduct.Text = Convert.ToString(sqlReader["pName"]);
					cbCatProd.Text = Convert.ToString(sqlReader["category"]);
					cbProdGRP.Text = Convert.ToString(sqlReader["GRP"]);
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

	}
}
