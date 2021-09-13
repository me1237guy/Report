using System;
using System.Linq;
using System.Windows.Forms;
using ReportApp.Model;
using ReportApp.Service;

namespace ReportApp
{
    public partial class Form1 : Form
    {
        DAOService daoService = new DAOService();
        public Form1()
        {
            InitializeComponent();


            LoadData();
            
        }
        private void LoadData()
        {
            // reset dataGridView1 
            dataGridView1.Columns.Clear();

            // request the database
            var orders = daoService.GetOrder();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = orders.ToList();
            dataGridView1.Columns[0].Width = this.Width + 800;

            // set ReadOnly property
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].ReadOnly = true;
            }
            // display how many rows of Number that it has
            this.label1.Text = dataGridView1.Rows.Count.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
