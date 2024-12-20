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
            if(textBox1.Text == null || textBox2.Text == null || textBox3.Text == null)
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                string sql = "INSERT INTO country (name_,capital,population,continent_id) VALUES (@name_,@capital,@population,@continent_id)";
                try
                {

                    NpgsqlConnection sconn = new NpgsqlConnection(connectionString);
                    sconn.Open();
                    NpgsqlCommand command = new NpgsqlCommand(sql, sconn);
                    command.Parameters.AddWithValue("@name_", textBox1.Text);
                    command.Parameters.AddWithValue("@capital", textBox2.Text);
                    command.Parameters.AddWithValue("@population", Convert.ToInt32(textBox3.Text));
                    command.Parameters.AddWithValue("@continent_id", comboBox1.SelectedIndex + 1);

                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                        MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    sconn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBox.Show("Cтрана добавлена");
            }
        }

        private void FormAddCoutry_Load(object sender, EventArgs e)
        {
            string sql = "SELECT name_ FROM continent GROUP BY name_";
            try
            {
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
        }
    }
}
