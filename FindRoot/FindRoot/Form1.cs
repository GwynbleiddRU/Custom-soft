using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindRoot
{
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();
        int ind_f = 0;
        public Form1()
        {
            InitializeComponent();
        }
        double func(double x, int i)
        {
            //exp(x+3*sin(x))  - 8*x*sin(x)*sin(x)
            //8*x*sin(x)^2+exp(x-2*sin(x))-4*x
            switch (i)
            {
                case 0: { return 8 * x * Math.Sin(x) * Math.Sin(x) + Math.Exp(x + 3 * Math.Sin(x)) - 4 * x;}
                case 1: { return Math.Pow((4+x),3)-2*x*x-16*x+3; }
                case 2: { return 2*(3*x-4)/(1+x*x); }
                case 3: { return 2*(x-1)*Math.Sqrt(x); }
                case 4: { return Math.Pow((7*x-6),3); }
            }
            return 0;
        }
        double deffunc(double x, int i)
        {
            //exp(x+3*sin(x))  - 8*x*sin(x)*sin(x)
            //8*x*sin(x)^2+exp(x-2*sin(x))-4*x
            switch (i)
            {
                case 0: 
               { return 16 * x * Math.Sin(x) * Math.Cos(x) + (3 * Math.Cos(x) + 1) * Math.Exp(x + 3 * Math.Sin(x)) + 8 * Math.Sin(x) * Math.Sin(x)-4;}
                case 1: 
               { return -4 * x + 3 * (4 + x) * (4 + x)  - 16; }
                case 2:
               { return 2 * (3 * (x * x + 1) - (3 * x - 4) * 2 * x) / Math.Pow((x * x + 1), 2); }
                case 3: 
               { return (6*x-1)/(2*Math.Sqrt(x)); }
                case 4: 
               { return 21*Math.Pow((7*x-6),2); }
            }
            return 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            double x0 = Convert.ToDouble(textBox1.Text);
            double xn = Convert.ToDouble(textBox2.Text);
            double dx = 0.05;
            double x = x0;
            while (x <= xn)
            {
                chart1.Series[0].Points.AddXY(x, func(x, ind_f));
                x += dx;
            
            }
            
        }
        void CreateDataTable1()
        {
            dt.Columns.Add("#");
            //dt.Columns[0].Width = 20;
            dt.Columns.Add("a");
            dt.Columns.Add("b");
            dt.Columns.Add("F(a)");
            dt.Columns.Add("F(b)");
            dt.Columns.Add("F(c)");
           
        }
        void CreateDataTable2()
        {
            dt.Columns.Add("#");
            //dt.Columns[0].Width = 20;
            dt.Columns.Add("x0");
            dt.Columns.Add("x");
            dt.Columns.Add("F(x0)");
            dt.Columns.Add("F(x)");
            dt.Columns.Add("eps");

        }

        void CreateRow1(DataRow dr, ref DataTable dt, int i, double a, double b)
        {
            dr = dt.NewRow();
            dr["#"] = i.ToString();
            dr["a"] = a.ToString();
            dr["b"] = b.ToString();
            if (func(a, ind_f) > 0)
            {
                dr["F(a)"] = ">0";
            }
            else
            {
                dr["F(a)"] = "<0";
            }
            if (func(b, ind_f) > 0)
            {
                dr["F(b)"] = ">0";
            }
            else
            {
                dr["F(b)"] = "<0";
            }
            dr["F(c)"] = func((a + b) / 2, ind_f).ToString();
            dt.Rows.Add(dr);
        }
        void CreateRow2(DataRow dr, ref DataTable dt, int i, double xp, double xn)
        {
            dr = dt.NewRow();
            dr["#"] = i.ToString();
            dr["x0"] = xp.ToString();
            dr["x"] = xn.ToString();
            dr["F(x0)"] = (func(xp, ind_f)).ToString();
            dr["F(x)"] = (func(xn, ind_f)).ToString();                   
            dr["eps"] = Math.Abs(xn-xp).ToString();
            dt.Rows.Add(dr);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox3.Text);
            double b = Convert.ToDouble(textBox4.Text);
            double eps = Convert.ToDouble(textBox5.Text);
            double root=0;
            table_proc.DataSource = null;
            dt = new DataTable();
            CreateDataTable1();
            DataRow dr;
            dr = dt.NewRow();
            if (func(a, ind_f) * func(b, ind_f) > 0)
            {
               MessageBox.Show ("Функция на концах отрезка принимает значение одного знака\n"+
                                "условия метода нарушены","Бинарный поиск");             
            } 
           else
            {
                int i = 1; 
               while (Math.Abs(a-b)>eps) 
               {
                 CreateRow1(dr, ref dt, i, a,b);
                 double c=(a+b)/2;
                 if (Math.Abs(func(c, ind_f)) < eps)
                 {
                   root =c;
                   break;
                 }
                 if (func(c, ind_f) * func(b, ind_f) < 0) { a = c; }
                 if (func(c, ind_f) * func(a, ind_f) < 0) { b = c; }
                i++;
               }
             root=(a+b)/2;
             label8.Text = "Корень:";
             label9.Text = "F(" + root.ToString() + ")=" + func(root, ind_f).ToString();
             label9.Visible = true;
             table_proc.DataSource = dt; 
             table_proc.AutoResizeColumns();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ind_f = comboBox1.SelectedIndex;
            label12.Text = comboBox1.Items[comboBox1.SelectedIndex].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            double x0 = Convert.ToDouble(textBox3.Text);
            double x = x0;
            double eps = Convert.ToDouble(textBox5.Text);
            double root = 0;
            table_proc.DataSource = null;
            dt = new DataTable();
            CreateDataTable2();
            DataRow dr;
            dr = dt.NewRow();
            int n = 0;
            CreateRow2(dr, ref dt, n, x0, x);
           do
            {
                x = x0;
                x0 = x - func(x, ind_f) / deffunc(x, ind_f);
                n++;
                CreateRow2(dr, ref dt, n, x0, x);
            } while (Math.Abs(x - x0) >= eps);
           root = x;
           label8.Text = "Корень:";
           label9.Text = "F(" + root.ToString() + ")=" + func(root, ind_f).ToString();
           label9.Visible = true;
           table_proc.DataSource = dt;
           table_proc.AutoResizeColumns();
        }
         
    }
}
