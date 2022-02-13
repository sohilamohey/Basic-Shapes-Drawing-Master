
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project_Dr_Yasser
{

    public partial class DDAcs : Form
    {
        public DDAcs()
        {
            InitializeComponent();
         
        }
        private DataGridView TABLE_GRAD = new DataGridView();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TABLE_GRAD.DataSource = points_of_array[index];
        }
        Bitmap bitmap;
        int Angle;
        Point The_start_point;
        Point The_end_point;
        private const int SIZE = 100;
        Point[] points_of_array = new Point[SIZE];
        public float xa, xb, ya, yb;
        private int index = 0;
        public int l = 1, r = 2, b = 4, t = 8;


        public void DDA(float xa, float ya, float xb, float yb)
        {
            float dx = Math.Abs(xb - xa), dy = Math.Abs(yb - ya), steps;
            float xIncrement, yIncrement, x = xa, y = ya;
            steps = Math.Max(dx, dy);
            xIncrement = dx / steps;
            yIncrement = dy / steps;
            The_start_point = new Point((int)Math.Round(x + .5), (int)Math.Round(y + .5));
            while (steps > 0)
            {
                points_of_array[index++] = new Point((int)Math.Round(x + .5), (int)Math.Round(y + .5));
                x += xIncrement;
                y += yIncrement;
                steps--;
            }
           
            draw_line();
        }
        void draw_line()
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            for (int i = 0; i < index; i++)
            {
                if (points_of_array[i].X >= 0 && points_of_array[i].Y >= 0)
                    bitmap.SetPixel(points_of_array[i].X, points_of_array[i].Y, Color.Blue);
            }
            The_end_point =new Point((int)(points_of_array[index-1].X), (int)(points_of_array[index-1].Y));
            pictureBox1.Image = bitmap;
            TABLE_GRAD.Rows.Clear();
            TABLE_GRAD.Columns.Clear();
            draw_table();
        }
        void draw_table()
        {
            dataGridView1.DataSource = points_of_array;
        }
        public void button1_Click(object sender, EventArgs e)
        {
            xa = float.Parse(textBox1.Text);
            ya = float.Parse(textBox2.Text);
            xb = float.Parse(textBox3.Text);
            yb = float.Parse(textBox4.Text);
            DDA(xa, ya, xb, yb);
        }      
        public void Rotate_Function(int angle)
        {
            double theta_angle = angle * 3.14 / 180;
            double[,] R = {{Math.Cos(theta_angle), -Math.Sin(theta_angle), 0 },
                        {Math.Sin(theta_angle), Math.Cos(theta_angle), 0 },
                        {0, 0, 0 } };
            for (int i = 0; i < index; i++)
            {
                double[] TEMP = { points_of_array[i].X, points_of_array[i].Y, 1 };
                TEMP[0] -= The_start_point.X;
                TEMP[1] -= The_start_point.Y;
                double[] newPoint = Multiply_Double_Numbers(R, TEMP);
                newPoint[0] += The_start_point.X;
                newPoint[1] += The_start_point.Y;
                points_of_array[i] = new Point((int)(newPoint[0]), (int)(newPoint[1]));
            }
            draw_line();
            draw_table();
            pictureBox1.Image = bitmap;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            Angle = int.Parse(textBox7.Text);
            Rotate_Function(Angle);
        }
        public void Translate_Function(int tx, int ty)
        {
            int[,] T = { { 1, 0, tx },
                         { 0, 1, ty },
                         { 0, 0, 1 } };
            for (int i = 0; i < index; i++)
            {
                int[] p = Multiply_Int_Numbers(T, new int[] { points_of_array[i].X, points_of_array[i].Y, 1 });
                points_of_array[i].X = p[0];
                points_of_array[i].Y = p[1];
            }
            The_start_point = new Point(points_of_array[0].X, points_of_array[0].Y);
            draw_line();
            draw_table();
            pictureBox1.Image = bitmap;
        }
        public void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int tx = int.Parse(textBox5.Text),
              ty = int.Parse(textBox6.Text);
            Translate_Function(tx, ty);
        }
        public void Scale_Function(double sx, double sy)
        {
            double[,] Scal_array = { { sx, 0, 0 },
                         { 0, sy, 0 },
                         { 0, 0, 1 } };

            for (int i = 0; i < index; i++)
            {
                double[] arra_of_3Ponits = { points_of_array[i].X, points_of_array[i].Y, 1 };
                arra_of_3Ponits[0] -= The_start_point.X;
                arra_of_3Ponits[1] -= The_start_point.Y;
                double[] newPoint = Multiply_Double_Numbers(Scal_array, arra_of_3Ponits);
                newPoint[0] += The_start_point.X;
                newPoint[1] += The_start_point.Y;
                points_of_array[i] = new Point((int)Math.Round(newPoint[0]), (int)Math.Round(newPoint[1]));
            }
            draw_line();
            pictureBox1.Image = bitmap;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            float sx = float.Parse(textBox8.Text);
            float sy = float.Parse(textBox9.Text);
            Scale_Function(sx, sy);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public void DDAcs_Load(object sender, EventArgs e)
        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }

        int[] Multiply_Int_Numbers(int[,] matrix, int[] p)
        {
            int[] result = new int[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] = result[i] + matrix[i, 0] * p[0];
                result[i] = result[i] + matrix[i, 1] * p[1];
                result[i] = result[i] + matrix[i, 2] * p[2];
            }
            return result;
        }
        double[] Multiply_Double_Numbers(double[,] matrix, double[] p)

        {
            double[] result = new double[4];
            for (int i = 0; i < 3; i++)
            {
                result[i] = result[i]+ matrix[i, 0] * p[0];
                result[i] = result[i] + matrix[i, 1] * p[1];
                result[i] = result[i] + matrix[i, 2] * p[2];
            }
            return result;
        }
    }
}
