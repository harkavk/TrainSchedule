using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SingletonLibrary;

namespace WindowsFormsApp1
{
    public partial class TrainForm : Form
    {
        private dbManager db;
        public TrainForm()
        {
            InitializeComponent();

            db = dbManager.GetInstance();

            LoadTrains();

            Logger.GetInstance().Log(
                "Відкриття форми",
                "Відкрито форму роботи з потягами"
            );
        }
        private void LoadTrains()
        {
            try
            {
                db.OpenConnection();

                string query = "SELECT Number, Type, Capacity FROM Train";

                using (MySqlCommand command =
                       new MySqlCommand(query, db.GetConnection()))
                {
                    using (MySqlDataAdapter adapter =
                           new MySqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        dgvTrains.DataSource = table;
                    }
                }

                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Помилка завантаження потягів:\n" + ex.Message,
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("Оберіть потяг.");
                return;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity))
            {
                MessageBox.Show("Місткість повинна бути числом.");
                return;
            }

            try
            {
                db.OpenConnection();

                string query =
                    "UPDATE Train " +
                    "SET Type = @type, Capacity = @capacity " +
                    "WHERE Number = @number";

                using (MySqlCommand command =
                       new MySqlCommand(query, db.GetConnection()))
                {
                    command.Parameters.AddWithValue("@number", txtNumber.Text);
                    command.Parameters.AddWithValue("@type", txtType.Text);
                    command.Parameters.AddWithValue("@capacity", capacity);

                    command.ExecuteNonQuery();
                }

                db.CloseConnection();

                Logger.GetInstance().Log(
                    "Редагування потяга",
                    "Змінено дані потяга " + txtNumber.Text
                );

                LoadTrains();

                MessageBox.Show("Дані потяга оновлено!");

            }
            catch (Exception ex)
            {
                db.CloseConnection();

                MessageBox.Show(
                    "Помилка редагування:\n" + ex.Message
                );
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("Оберіть потяг.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Ви впевнені, що хочете видалити потяг " +
                txtNumber.Text + "?",
                "Підтвердження",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                db.OpenConnection();

                string query =
                    "DELETE FROM Train WHERE Number = @number";

                using (MySqlCommand command =
                       new MySqlCommand(query, db.GetConnection()))
                {
                    command.Parameters.AddWithValue(
                        "@number",
                        txtNumber.Text
                    );

                    command.ExecuteNonQuery();
                }

                db.CloseConnection();

                Logger.GetInstance().Log(
                    "Видалення потяга",
                    "Видалено потяг " + txtNumber.Text
                );

                LoadTrains();
                ClearFields();

                MessageBox.Show("Потяг видалено!");
            }
            catch (Exception ex)
            {
                db.CloseConnection();

                MessageBox.Show(
                    "Помилка видалення:\n" + ex.Message
                );
            }
        }

        private void TrainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadTrains();

            Logger.GetInstance().Log(
                "Завантаження потягів",
                "Завантажено список потягів з бази даних"
            );
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumber.Text) ||
       string.IsNullOrWhiteSpace(txtType.Text) ||
       string.IsNullOrWhiteSpace(txtCapacity.Text))
            {
                MessageBox.Show(
                    "Заповніть усі поля.",
                    "Попередження",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity))
            {
                MessageBox.Show(
                    "Місткість повинна бути числом.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            try
            {
                db.OpenConnection();

                string query =
                    "INSERT INTO Train (Number, Type, Capacity) " +
                    "VALUES (@number, @type, @capacity)";

                using (MySqlCommand command =
                       new MySqlCommand(query, db.GetConnection()))
                {
                    command.Parameters.AddWithValue("@number", txtNumber.Text);
                    command.Parameters.AddWithValue("@type", txtType.Text);
                    command.Parameters.AddWithValue("@capacity", capacity);

                    command.ExecuteNonQuery();
                }

                db.CloseConnection();

                Logger.GetInstance().Log(
                    "Додавання потяга",
                    "Додано потяг " + txtNumber.Text
                );

                MessageBox.Show(
                    "Потяг додано!",
                    "Успішно",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadTrains();
                ClearFields();
            }
            catch (Exception ex)
            {
                db.CloseConnection();

                MessageBox.Show(
                    "Помилка додавання:\n" + ex.Message,
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dgvTrains_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTrains_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNumber.Text =
                    dgvTrains.Rows[e.RowIndex]
                    .Cells["Number"].Value.ToString();

                txtType.Text =
                    dgvTrains.Rows[e.RowIndex]
                    .Cells["Type"].Value.ToString();

                txtCapacity.Text =
                    dgvTrains.Rows[e.RowIndex]
                    .Cells["Capacity"].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            txtNumber.Clear();
            txtType.Clear();
            txtCapacity.Clear();
        }
    }
}
