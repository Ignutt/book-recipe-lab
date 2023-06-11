using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using systemoffurniture;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;


namespace systemoffurniture
{
    public partial class MainForm : Form
    {
        private List<Recipe> _recipes = new List<Recipe>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            SQLiteConnection connection = new SQLiteConnection("Integrated Security = SSPI; Data Source = db.db");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT ID, Name, Country, Difficult   FROM Furniture";
            using (var rd1 = command.ExecuteReader())
            {

                while (rd1.Read())
                {

                    _recipes.Add(new Recipe(rd1.GetInt32(0), rd1.GetString(1), rd1.GetString(2), rd1.GetInt32(3)));

                }
            }
            connection.Close();
            connection.Open();

            command.CommandText = @"SELECT Country, COUNT(*) AS Count FROM Furniture GROUP BY Country";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string category = reader.GetString(0);
                    int count = reader.GetInt32(1);
                    chart1.Series[0].ChartType = SeriesChartType.Column;
                    chart1.Series["Series1"].Points.AddXY(category, count);
                }
            }
            connection.Close();
            dataGridView1.DataSource = _recipes;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Add("Все товары категории Кухня");
            comboBox1.Items.Add("Товары дешевле 1500");
            comboBox1.Items.Add("Все товары категории Офис");
            comboBox1.Items.Add("Все товары категории Ванная");
            comboBox1.Items.Add("Все товары с именем Стол");
            comboBox1.Items.Add("Количество товаров по категориям");
            comboBox1.Items.Add("Товары, сгрупированные по категориям");
            comboBox1.Items.Add("Товары, имя которых начинается на С");
            comboBox1.Items.Add("Товары дороже 1500");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(n => n.Name == "Кухня");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }
            if (comboBox1.SelectedIndex == 1)
            {
                IEnumerable<Recipe> numQuery4 = _recipes.Where(n => n.Difficult < 1500);
                this.dataGridView2.DataSource = numQuery4.ToArray();
            }

            if (comboBox1.SelectedIndex == 2)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(n => n.Name == "Офис");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 3)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(n => n.Name == "Ванная");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 4)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(n => n.Country == "Стол");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 5)
            {
                var query = _recipes.GroupBy(pet => pet.Name, pet => pet.Country);

                foreach (var petGroup in query)
                {
                    richTextBox1.AppendText("Категория  - " + petGroup.Key + " \n");

                    foreach (string name in petGroup)
                    {
                        richTextBox1.AppendText(" " + name + "  \n");
                    }
                    richTextBox1.AppendText(" Колличество - " + _recipes.Where(pet => pet.Name == petGroup.Key).Count().ToString() + " \n");
                    richTextBox1.AppendText("___________________________________ \n");
                }
            }

            if (comboBox1.SelectedIndex == 6)
            {
                var query = _recipes.GroupBy(n => n.Name, n => n.Country);
                foreach (var group in query)
                {
                    richTextBox1.AppendText(group.Key + "  \n");
                    foreach (var item in group)
                    {
                        richTextBox1.AppendText("  " + item + "  \n");
                    }
                    richTextBox1.AppendText("--------------------------------- \n");
                }
            }

            if (comboBox1.SelectedIndex == 7)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(n => n.Country.StartsWith("С"));
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 8)
            {
                IEnumerable<Recipe> numQuery4 = _recipes.Where(n => n.Difficult > 1500);
                this.dataGridView2.DataSource = numQuery4.ToArray();
            }
        }

        private void button1_Click(object sender, EventArgs e)

        {
            SQLiteConnection connection = new SQLiteConnection("Integrated Security = SSPI; Data Source = db.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand($"INSERT INTO [Furniture] (ID, Name, Contry, Difficult) VALUES (@ID, @Name, @Category, @Price)", connection);
            command.Parameters.AddWithValue("ID", textBox1.Text);
            command.Parameters.AddWithValue("Name", textBox2.Text);
            command.Parameters.AddWithValue("Contry", textBox3.Text);
            command.Parameters.AddWithValue("Difficult", textBox4.Text);
            MessageBox.Show("Товар успешно добавлен", command.ExecuteNonQuery().ToString());

        }
 
    }

}


