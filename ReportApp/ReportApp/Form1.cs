using System;
using System.Collections.Generic;
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
            dataGridView1.Columns[0].Width = this.Width;

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

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                Order order = new Order();
                order.Number = textBox1.Text;
                if (textBox1.Text.Length == 0)
                {
                    this.Text = "";
                    return;
                }

                // Find the current input data in data.db
                int cnt = daoService.GetSameOrder(order);

                // the current data is already in the database
                if (cnt > 0)
                {
                    List<Order> orders = daoService.GetOrder();
                    
                    // return the corresponding row index
                    int pos = orders.FindIndex(a => a.Number == order.Number);
                    if (pos > 0)
                    {
                        var row = pos - 1;
                        dataGridView1.Rows[pos].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = pos;
                        this.Text = $"{order.Number} 舊資料";
                    }
                }
                else // it's new
                {
                    this.Text = $"{order.Number} 新資料";
                }

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddOrder();
                LoadData();
                MoveToKeyRow();
            }
        }
        public void AddOrder()
        {
            Order order = new Order();
            order.Number = textBox1.Text;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();  
            sw.Start(); 
            var count = daoService.AddOrder(order);
            sw.Stop();

            //印出所花費的總秒數
            string result1 = sw.Elapsed.TotalSeconds.ToString();
            if (count == 0)
            {
                this.Text = $"資料已寫入完成，花費:{result1} 秒";
            }
            else
            {
                this.Text = $"該筆資料重複: {order.Number} ";
            }
        }

        private int MoveToKeyRow()
        {
            Order order = new Order();
            order.Number = this.textBox1.Text;
            var orders = daoService.GetOrder();
            int pos = orders.FindIndex(a => a.Number == order.Number);
            if (pos > 0)
            {
                dataGridView1.Rows[pos].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = pos;
            }
            return pos;
        }
    }
}
