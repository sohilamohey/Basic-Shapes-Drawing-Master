using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project_Dr_Yasser
{
    public partial class Circle : Form
    {
        public Circle()
        {
            InitializeComponent();
        }
       
        Bitmap bitmap;
        private DataGridView TABLE_GRAD = new DataGridView();
        Point The_start_point;
        Point The_end_point;
        DataTable dt = new DataTable(); 
        private const int Size_Of_array = 10004;
        Point[] points_of_array = new Point[Size_Of_array];
        public float xa, xb, ya, yb;
        public int index = 0;
        public int xCenter, yCenter, radius;
        public int l = 1, r = 2, b = 4, t = 8;
        private const byte INSIDE = 0; // 0000
        private const byte LEFT = 1;   // 0001
        private const byte RIGHT = 2;  // 0010
        private const byte BOTTOM = 4; // 0100
        private const byte TOP = 8;
        
        public void Draw_Circle(int xCenter, int yCenter, int radius)
        {
            int x = 0, y = radius, p = 1 - radius;
           
            while (x < y)
            {
                x++;
                if (p < 0) p += 2 * x + 1;
                else
                {
                    y--;
                    p += (2 * x) - 2 * y;
                }
                points_of_array[index++] = new Point(xCenter + x, yCenter + y);
                points_of_array[index++] = new Point(xCenter + x, yCenter - y);
                points_of_array[index++] = new Point(xCenter - x, yCenter + y);
                points_of_array[index++] = new Point(xCenter + y, yCenter + x);
                points_of_array[index++] = new Point(xCenter + y, yCenter - x);
                points_of_array[index++] = new Point(xCenter - y, yCenter + x);
                points_of_array[index++] = new Point(xCenter - x, yCenter - y);
                points_of_array[index++] = new Point(xCenter - y, yCenter - x);
            }
            draw_circle();
        }
        void draw_circle()
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
        private void button1_Click(object sender, EventArgs e)
        {
            xCenter = int.Parse(textBox1.Text);
            yCenter = int.Parse(textBox2.Text);
               radius = int.Parse(textBox3.Text);
            if (xCenter < radius) xCenter += radius - xCenter;
            if (yCenter < radius) yCenter += radius - yCenter;
            Draw_Circle(xCenter, yCenter, radius);
        }
        public void transformation(int tx, int ty)
        {
            int[,] Translate_matix = { { 1, 0, tx },
                         { 0, 1, ty },
                         { 0, 0, 1 } };
            for (int i = 0; i < index; i++)
            {
                int[] p = Multiply_Int_Numbers(Translate_matix,
                    new int[] { points_of_array[i].X, points_of_array[i].Y, 1 });
                points_of_array[i].X = p[0];
                points_of_array[i].Y = p[1];
            }
            The_start_point = new Point(points_of_array[0].X, points_of_array[0].Y);
            draw_circle();
            pictureBox1.Image = bitmap;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int tx = int.Parse(textBox5.Text),
               ty = int.Parse(textBox6.Text);
            transformation(tx, ty);
        }
        public void Scale_Function(double sx, double sy)
        {
            double[,] Scal_array = { { sx, 0, 0 },{ 0, sy, 0 },{ 0, 0, 1 } };

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
            draw_circle();
            pictureBox1.Image = bitmap;
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void Rotation_Function(int angle)
        {
            double theta_angle = angle * 3.14 / 180;
            double[,] R = {{Math.Cos(theta_angle), -Math.Sin(theta_angle), 0 },
                        {Math.Sin(theta_angle), Math.Cos(theta_angle), 0 },
                        {0, 0, 0 } };
            for (int i = 0; i < index; i++)
            {
                double[] temporary = { points_of_array[i].X, points_of_array[i].Y, 1 };
                temporary[0] = temporary[0] - The_start_point.X;
                temporary[1] = temporary[1] - The_start_point.Y;
                double[] NEW_POINTS = Multiply_Double_Numbers(R, temporary);
                NEW_POINTS[0] = NEW_POINTS[0]+ The_start_point.X;
                NEW_POINTS[1] = NEW_POINTS[1]+ The_start_point.Y;
                points_of_array[i] = new Point((int)Math.Round(NEW_POINTS[0]), (int)Math.Round(NEW_POINTS[1]));
            }
            draw_circle();
            pictureBox1.Image = bitmap;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int angle = int.Parse(textBox7.Text);
            Rotation_Function(angle);
        }
       
       
        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            ClearImage();
            pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            double sx = double.Parse(textBox8.Text),
                sy = double.Parse(textBox9.Text);
            Scale_Function(sx, sy);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TABLE_GRAD.DataSource = points_of_array[index];
        }
        private void button2_Click(object sender, EventArgs e)
        {          
            this.Hide();
        }

        private void Circle_Load(object sender, EventArgs e)
        {

        }
        public void ClearImage()
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.LemonChiffon);
        }
        int[] Multiply_Int_Numbers(int[,] matrix, int[] p)
        {
            int[] result = new int[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] += matrix[i, 0] * p[0];
                result[i] += matrix[i, 1] * p[1];
                result[i] += matrix[i, 2] * p[2];
            }
            return result;
        }
        double[] Multiply_Double_Numbers(double[,] matrix, double[] p)

        {
            double[] result = new double[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] += matrix[i, 0] * p[0];
                result[i] += matrix[i, 1] * p[1];
                result[i] += matrix[i, 2] * p[2];
            }
            return result;
        }
    }
    
}

