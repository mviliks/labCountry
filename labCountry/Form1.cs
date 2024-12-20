using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace labCountry
{
    public partial class Form1 : Form
    {
        const string connectionString = "Host=localhost;Database=Country_DB;Username=isp-232o-mikhailova;Password=student";

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormAddCoutry(connectionString).Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT name_ FROM country GROUP BY name_";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                DataTable stable = new DataTable();
                NpgsqlDataAdapter sadapter = new NpgsqlDataAdapter(comm);
                sadapter.Fill(stable);
                comboBox1.Items.Clear();
                foreach (DataRow row in stable.Rows)
                {
                    comboBox1.Items.Add(row["name_"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            try
            {
                string sql = "SELECT id as Айди,name_ as Название, capital as Столица, population as Население,continent_id as Айди_континент  FROM country";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                DataTable table = new DataTable();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (comboBox1.Text == "")
                 sql = "SELECT id as Айди,name_ as Название, capital as Столица, population as Население,continent_id as Айди_континент  FROM country";
            else
                 sql = $"SELECT id as Айди,name_ as Название, capital as Столица, population as Население, continent_id as Айди_континента  FROM country WHERE name_ = '{comboBox1.Text}'"; 
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                DataTable table = new DataTable();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
