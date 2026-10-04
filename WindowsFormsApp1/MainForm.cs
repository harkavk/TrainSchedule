using MySql.Data.MySqlClient;
using SingletonLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private dbManager db;
        public MainForm()
        {
            InitializeComponent();

            db = dbManager.GetInstance();

            LoadTrainsToMain();

            Logger.GetInstance().Log(
                "Запуск програми",
                "Відкрито головну форму"
            );
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnTestSingleton_Click(object sender, EventArgs e)
        {
            dbManager db1 = dbManager.GetInstance();
            dbManager db2 = dbManager.GetInstance();

            Logger logger1 = Logger.GetInstance();
            Logger logger2 = Logger.GetInstance();

            DocumentSaver saver1 = DocumentSaver.GetInstance();
            DocumentSaver saver2 = DocumentSaver.GetInstance();

            bool dbSame = db1 == db2;
            bool loggerSame = logger1 == logger2;
            bool saverSame = saver1 == saver2;

            MessageBox.Show(
                $"dbManager: {dbSame}\n" +
                $"Logger: {loggerSame}\n" +
                $"DocumentSaver: {saverSame}",
                "Перевірка Singleton",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            Logger.GetInstance().Log(
                "Перевірка Singleton",
                "Перевірено єдиний екземпляр трьох Singleton"
            );
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string document =
       "РОЗКЛАД РУХУ ПОТЯГІВ\n" +
       "============================\n\n" +
       "001A | Київ – Львів | 08:00 – 13:30\n" +
       "002B | Київ – Одеса | 09:30 – 16:00\n" +
       "003C | Львів – Київ | 15:00 – 20:30\n" +
       "004D | Київ – Ужгород | 21:00 – 08:00\n";

            DocumentSaver saver = DocumentSaver.GetInstance();

            saver.SaveDocument(
                "Розклад_потягів",
                document
            );

            Logger.GetInstance().Log(
                "Збереження документа",
                "Збережено документ Розклад_потягів"
            );

            MessageBox.Show(
                "Розклад успішно збережено!",
                "Готово",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            Logger logger = Logger.GetInstance();

            string path = logger.GetLogFilePath();

            if (System.IO.File.Exists(path))
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    }
                );
            }
            else
            {
                MessageBox.Show(
                    "Файл журналу ще не створено.",
                    "Журнал",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void LoadTrainsToMain()
        {
            try
            {
                db.OpenConnection();

                string query =
                    "SELECT Number, Type, Capacity " +
                    "FROM Train " +
                    "ORDER BY Number";

                using (MySqlCommand command =
                       new MySqlCommand(query, db.GetConnection()))
                {
                    using (MySqlDataAdapter adapter =
                           new MySqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        dataGridViewSchedule.DataSource = table;
                    }
                }

                db.CloseConnection();

                lblStatus.Text = "Список потягів оновлено";
            }
            catch (Exception ex)
            {
                db.CloseConnection();

                MessageBox.Show(
                    "Помилка завантаження потягів:\n" + ex.Message,
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
       
        private void btnTrain_Click(object sender, EventArgs e)
        {
            TrainForm trainForm = new TrainForm();

            trainForm.FormClosed += (s, args) =>
            {
                LoadTrainsToMain();
            };

            trainForm.Show();

            Logger.GetInstance().Log(
                "Відкриття форми",
                "Відкрито форму роботи з потягами"
            );
        }
    }
}
