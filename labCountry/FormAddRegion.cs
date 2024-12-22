using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labCountry
{
    public partial class FormAddCoutry : Form
    {
        string connectionString;
        public FormAddCoutry(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            new Form1().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            int continentId = -1;
            int formOfGovernmentId = -1;

            try
            {
                // Получение идентификатора континента
                string getContinentIdSql = "SELECT id FROM continent WHERE name_ = @continentName";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(getContinentIdSql, conn))
                    {
                        command.Parameters.AddWithValue("@continentName", comboBox1.Text);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            continentId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Континент не найден");
                            return;
                        }
                    }
                }

                // Получение идентификатора формы правления
                string getFormOfGovernmentIdSql = "SELECT id FROM form_of_government WHERE name_ = @formOfGovernmentName";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(getFormOfGovernmentIdSql, conn))
                    {
                        command.Parameters.AddWithValue("@formOfGovernmentName", comboBox2.Text);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            formOfGovernmentId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Форма правления не найдена");
                            return;
                        }
                    }
                }

                string insertCountrySql = "INSERT INTO country (name_, capital, population, continent_id, form_of_government_id) VALUES (@name_, @capital, @population, @continent_id, @form_of_government_id)";
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(insertCountrySql, conn))
                    {
                        command.Parameters.AddWithValue("@name_", textBox1.Text);
                        command.Parameters.AddWithValue("@capital", textBox2.Text);
                        command.Parameters.AddWithValue("@population", Convert.ToInt32(textBox3.Text));
                        command.Parameters.AddWithValue("@continent_id", continentId);
                        command.Parameters.AddWithValue("@form_of_government_id", formOfGovernmentId);
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                        {
                            MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                        }
                    }
                }

                MessageBox.Show("Страна добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormAddCoutry_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT name_ FROM continent GROUP BY name_";
                NpgsqlConnection sconn = new NpgsqlConnection(connectionString);
                NpgsqlCommand scomm = new NpgsqlCommand(sql, sconn);
                DataTable stable = new DataTable();
                NpgsqlDataAdapter sadapter = new NpgsqlDataAdapter(scomm);
                sadapter.Fill(stable);
                comboBox1.Items.Clear();
                foreach (DataRow row in stable.Rows)
                {
                    comboBox1.Items.Add(row["name_"].ToString());
                }

                sconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                string sql = "SELECT name_ FROM form_of_government GROUP BY name_";
                NpgsqlConnection sconn = new NpgsqlConnection(connectionString);
                NpgsqlCommand scomm = new NpgsqlCommand(sql, sconn);
                DataTable stable = new DataTable();
                NpgsqlDataAdapter sadapter = new NpgsqlDataAdapter(scomm);
                sadapter.Fill(stable);
                comboBox2.Items.Clear();
                foreach (DataRow row in stable.Rows)
                {
                    comboBox2.Items.Add(row["name_"].ToString());
                }

                sconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
