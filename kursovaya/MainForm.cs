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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Windows.Threading;

namespace systemoffurniture
{
    public partial class MainForm : Form
    {
        private List<Recipe> _recipes = new List<Recipe>();
        private BindingSource bindingSource;

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
            command.CommandText = @"SELECT ID, Name, Country, Difficult   FROM Recipes";
            using (var rd1 = command.ExecuteReader())
            {

                while (rd1.Read())
                {
                    _recipes.Add(new Recipe(rd1.GetInt32(0), rd1.GetString(1), rd1.GetString(2), rd1.GetInt32(3)));
                }
            }
            connection.Close();
            connection.Open();

            command.CommandText = @"SELECT Country, COUNT(*) AS Count FROM Recipes GROUP BY Country";
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

            bindingSource = new BindingSource();
            bindingSource.DataSource = _recipes;
            dataGridView1.DataSource = bindingSource;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Add("Все рецепты сложностью <3");
            comboBox1.Items.Add("Все рецепты из России");
            comboBox1.Items.Add("Все рецепты из Японии");
            comboBox1.Items.Add("Все рецепты из Турции");
            comboBox1.Items.Add("Все рецепты из Италии");
            comboBox1.Items.Add("Рецепты сложнее 4");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(recipe => recipe.Difficult < 3);
                this.dataGridView2.DataSource = kitchen.ToArray();
            }
            if (comboBox1.SelectedIndex == 1)
            {
                IEnumerable<Recipe> numQuery4 = _recipes.Where(recipe => recipe.Country == "Россия");
                this.dataGridView2.DataSource = numQuery4.ToArray();
            }

            if (comboBox1.SelectedIndex == 2)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(recipe => recipe.Country == "Япония");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 3)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(recipe => recipe.Country == "Турция");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 4)
            {
                IEnumerable<Recipe> kitchen = _recipes.Where(recipe => recipe.Country == "Италия");
                this.dataGridView2.DataSource = kitchen.ToArray();
            }

            if (comboBox1.SelectedIndex == 5)
            {
                IEnumerable<Recipe> numQuery4 = _recipes.Where(n => n.Difficult > 1500);
                this.dataGridView2.DataSource = numQuery4.ToArray();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddData();
        }

        private async void AddData()
        {
                chart1.Series["Series1"].Points.Clear();

            await Task.Run(() =>
            {
                SQLiteConnection connection = new SQLiteConnection("Integrated Security = SSPI; Data Source = db.db");
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"INSERT INTO [Recipes] (ID, Name, Country, Difficult) VALUES (@ID, @Name, @Country, @Difficult)", connection);
                command.Parameters.AddWithValue("ID", textBox1.Text);
                command.Parameters.AddWithValue("Name", textBox2.Text);
                command.Parameters.AddWithValue("Country", textBox3.Text);
                command.Parameters.AddWithValue("Difficult", textBox4.Text);
                MessageBox.Show("Товар успешно добавлен", command.ExecuteNonQuery().ToString());

                connection.Close();
                connection.Open();

                command = connection.CreateCommand();
                command.CommandText = @"SELECT ID, Name, Country, Difficult   FROM Recipes";
                _recipes.Clear();
                using (var rd1 = command.ExecuteReader())
                {

                    while (rd1.Read())
                    {

                        _recipes.Add(new Recipe(rd1.GetInt32(0), rd1.GetString(1), rd1.GetString(2), rd1.GetInt32(3)));

                    }
                }

                connection.Close();
                connection.Open();

                command.CommandText = @"SELECT Country, COUNT(*) AS Count FROM Recipes GROUP BY Country";
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
            });

            bindingSource.ResetBindings(false);

        }

    }

}


