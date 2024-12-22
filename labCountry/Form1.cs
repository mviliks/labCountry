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
                string sql = "SELECT name_ FROM continent  GROUP BY name_";
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
                string sql = "SELECT name_ FROM form_of_government GROUP BY name_";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                DataTable stable = new DataTable();
                NpgsqlDataAdapter sadapter = new NpgsqlDataAdapter(comm);
                sadapter.Fill(stable);
                comboBox2.Items.Clear();
                foreach (DataRow row in stable.Rows)
                {
                    comboBox2.Items.Add(row["name_"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            try
            {
                string sql = @"
        SELECT 
            c.name_ AS Название,
            c.capital AS Столица,
            c.population AS Население,
            co.name_ AS Континент,
            fg.name_ AS Форма_правления
        FROM 
            Country c
        JOIN 
            Continent co ON c.continent_id = co.id
        JOIN 
            Form_of_government fg ON c.Form_of_government_id = fg.id";

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                    DataTable table = new DataTable();
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                sql = @"
            SELECT 
                c.name_ AS Название,
                c.capital AS Столица,
                c.population AS Население,
                co.name_ AS Континент,
                fg.name_ AS Форма_правления
            FROM 
                Country c
            JOIN 
                Continent co ON c.continent_id = co.id
            JOIN 
                Form_of_government fg ON c.Form_of_government_id = fg.id";
            }
            else
            {
                sql = @"
            SELECT 
                c.name_ AS Название,
                c.capital AS Столица,
                c.population AS Население,
                co.name_ AS Континент,
                fg.name_ AS Форма_правления
            FROM 
                Country c
            JOIN 
                Continent co ON c.continent_id = co.id
            JOIN 
                Form_of_government fg ON c.Form_of_government_id = fg.id
            WHERE 
                co.name_ = @continentName";
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(comboBox1.Text))
                    {
                        comm.Parameters.AddWithValue("@continentName", comboBox1.Text);
                    }

                    DataTable table = new DataTable();
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                sql = @"
            SELECT 
                c.name_ AS Название,
                c.capital AS Столица,
                c.population AS Население,
                co.name_ AS Континент,
                fg.name_ AS Форма_правления
            FROM 
                Country c
            JOIN 
                Continent co ON c.continent_id = co.id
            JOIN 
                Form_of_government fg ON c.Form_of_government_id = fg.id";
            }
            else
            {
                sql = @"
            SELECT 
                c.name_ AS Название,
                c.capital AS Столица,
                c.population AS Население,
                co.name_ AS Континент,
                fg.name_ AS Форма_правления
            FROM 
                Country c
            JOIN 
                Continent co ON c.continent_id = co.id
            JOIN 
                Form_of_government fg ON c.Form_of_government_id = fg.id
            WHERE 
                fg.name_ = @FormName";
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(comboBox1.Text))
                    {
                        comm.Parameters.AddWithValue("@FormName", comboBox2.Text);
                    }

                    DataTable table = new DataTable();
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(comm);
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
