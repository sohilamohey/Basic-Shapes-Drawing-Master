using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project_Dr_Yasser
{
    public partial class Ellips : Form
    {
        public Ellips()
        {
            InitializeComponent();
        }
        public int round(double x)
        {
            return (int)(x + 0.5);
        }
        private DataGridView TABLE_GRAD = new DataGridView();
        Point The_start_point;
        private const int SIZE = 10000;
        Point[] points_of_array = new Point[SIZE];
        public float xa, xb, ya, yb;
        private int index = 0;
        public float x0, y0, r1, r2;
        Bitmap bitmap;
        public void Draw_ellips(float x0, float y0, float r1, float r2)
        {
            if (x0 < r1) x0 += r1 - x0;
            if (x0 < r2) x0 += r2 - x0;
            if (y0< r1) y0 += r1 - y0;
            if (y0 < r2) y0 += r2 - y0;
            int x = 0, y = (int)r2;
            double p0 = round((r2 * r2) - (r1 * r1 * r2) + (0.25 * r1 * r1));
            while ((2 * x * Math.Pow(r2, 2)) < (2 * y * Math.Pow(r1, 2)))
            {               
                x++;
                if (p0 < 0)
                    p0 += (2 * x * r2 * r2) + (r2 * r2);
                else
                {
                    y--;
                    p0 += (2 * x * r2 * r2) - (2 * y * r1 * r1) + (r2 * r2);
                }
                points_of_array[index++] = new Point((int)x0 + x, (int)y0 + y);
                points_of_array[index++] = new Point((int)x0 - x, (int)y0 + y);
                points_of_array[index++] = new Point((int)x0 + x, (int)y0 - y);
                points_of_array[index++] = new Point((int)x0 - x, (int)y0 - y);
            }
            int p0_2 = (int)(Math.Pow(r2, 2) * Math.Pow(x, 2) + (Math.Pow(r2, 2) * x) +
                (Math.Pow(r2, 2) / 4) + (Math.Pow(r1, 2) * Math.Pow(y, 2)) - 
                (2 * Math.Pow(r1, 2) * y) - (Math.Pow(r1, 2)) - (Math.Pow(r1, 2) * Math.Pow(r2, 2)));
            while (y >= 0)
            {
                if (p0_2 >= 0){
                    y--;
                    p0_2 = p0_2 - (int)((2 * y * Math.Pow(r1, 2)) + Math.Pow(r1, 2));
                }
                else{
                    y--;
                    x++;
                    p0_2 = p0_2 + (int)((2 * x * Math.Pow(r2, 2)) - (2 * y * Math.Pow(r1, 2)) + Math.Pow(r1, 2));
                }
            
                points_of_array[index++] = new Point((int)x0 + x, (int)y0 + y);
                points_of_array[index++] = new Point((int)x0 - x, (int)y0 + y);
                points_of_array[index++] = new Point((int)x0 + x, (int)y0 - y); 
                points_of_array[index++] = new Point((int)x0 - x, (int)y0 - y); 
            }
            draw_ellips();
        }
        void draw_ellips()
        {
             bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            for (int i = 0; i < index; i++)
            {
                if (points_of_array[i].X >= 0 && points_of_array[i].Y >= 0)
                    bitmap.SetPixel(points_of_array[i].X, points_of_array[i].Y, Color.Blue);
            }
            pictureBox1.Image = bitmap;
            dataGridView1.DataSource = points_of_array;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            int x0 = int.Parse(textBox1.Text),
             y0 = int.Parse(textBox2.Text),
             redius1 = int.Parse(textBox3.Text),
             redius2 = int.Parse(textBox4.Text);
            Draw_ellips(x0, y0, redius1, redius2);
        }
        public void Translate_Function(int tx, int ty)
        {
            int[,] Translate_matix = { { 1, 0, tx }, { 0, 1, ty }, { 0, 0, 1 } };
            for (int i = 0; i < index; i++)
            {
                int[] p = Multiply_Int_Numbers(Translate_matix,
                    new int[] { points_of_array[i].X, points_of_array[i].Y, 1 });//[x,y,1]
                points_of_array[i].X = p[0];
                points_of_array[i].Y = p[1];
            }
            The_start_point = new Point(points_of_array[0].X, points_of_array[0].Y);
            draw_ellips();
            pictureBox1.Image = bitmap;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int tx = int.Parse(textBox5.Text),
                ty = int.Parse(textBox6.Text);
            Translate_Function(tx, ty);
        }
        public Bitmap Rotate_Function(int rotate_angle)
        {
            double theta_angle = rotate_angle * 3.14 / 180;
            double[,] Rotation_matrix = {{Math.Cos(theta_angle), -Math.Sin(theta_angle), 0 },
                        {Math.Sin(theta_angle), Math.Cos(theta_angle), 0 },
                        {0, 0, 0 } };

            for (int i = 0; i < index; i++)
            {
                double[] TEMP = { points_of_array[i].X, points_of_array[i].Y, 1 };
                TEMP[0] -= The_start_point.X;
                TEMP[1] -= The_start_point.Y;
                double[] newPoint = Multiply_Double_Numbers(Rotation_matrix, TEMP);
                newPoint[0] += The_start_point.X;
                newPoint[1] += The_start_point.Y;

                points_of_array[i] = new Point((int)Math.Round(newPoint[0]), (int)Math.Round(newPoint[1]));
            }
            draw_ellips();
            return bitmap;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int angle = int.Parse(textBox7.Text);
            Rotate_Function(angle);
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
            draw_ellips();
            pictureBox1.Image = bitmap;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            double sx = double.Parse(textBox8.Text),
               sy = double.Parse(textBox9.Text);
            Scale_Function(sx, sy);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
       
       
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            TABLE_GRAD.DataSource = points_of_array[index];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            List<Point> temp = new List<Point>();
            for (int i = 0; i < index; i++)
                temp.Add(points_of_array[i]);
            dataGridView1.DataSource =temp ;
        }

        private void Ellips_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
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
            double[] result = new double[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] = result[i] + matrix[i, 0] * p[0];
                result[i] = result[i] + matrix[i, 1] * p[1];
                result[i] = result[i] + matrix[i, 2] * p[2];
            }
            return result;
        }
    }
}
