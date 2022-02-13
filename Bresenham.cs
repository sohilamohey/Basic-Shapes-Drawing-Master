using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Project_Dr_Yasser
{
    public partial class Bresenham : Form
    {
        public Bresenham()
        {
            InitializeComponent();
        }    
        private const int SIZE = 104;
        Point[] points_of_array = new Point[SIZE];
        Point The_start;
        Point The_end;
        public int xa, xb, ya, yb;
       
        private int index = 0;
        Bitmap bitmap;
        private DataGridView TABLE_GRAD = new DataGridView();
        public int xmin, ymin, xmax, ymax;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TABLE_GRAD.DataSource = points_of_array[index];
        }

        public void bresenham(int xa, int ya,int xb,int yb)
        {
               int dx = Math.Abs(xa - xb), dy = Math.Abs(ya - yb),
                p = 2 * dy - dx,
                two_dy = 2 * dy,
                two_dy_dx = 2 * dy - 2 * dx,
                x, y, xEnd;
            if (xa > xb)
            {
                x = xb;
                y = yb;
                xEnd = xa;
            }
            else
            {
                x = xa;
                y = ya;
                xEnd = xb;
            }
            points_of_array[index++] = new Point(x, y);
            while (x < xEnd)
            {
                x++;
                if (p < 0)
                {
                    p += two_dy;
                }
                else
                {
                    y++;
                    p += two_dy_dx;
                }
                points_of_array[index++] = new Point(x, y);
               
            }
            The_end = points_of_array[index];
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
            pictureBox1.Image = bitmap;
            TABLE_GRAD.Rows.Clear();
            //TABLE_GRAD.Columns.Clear();
            draw_table();
        }
        void draw_table()
        {
            dataGridView1.DataSource = points_of_array;
        }
        public void button1_Click(object sender, EventArgs e)
        {  
            xa = int.Parse(textBox1.Text);
            ya = int.Parse(textBox2.Text);
            xb = int.Parse(textBox3.Text);
            yb = int.Parse(textBox4.Text);
            bresenham(xa, ya, xb, yb);
           
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
            The_start = new Point(points_of_array[0].X, points_of_array[0].Y);
            draw_line();
            draw_table();
            pictureBox1.Image = bitmap;
        }
        public void Scale_Function(double sx, double sy)
        {
            double[,] Scal_array = { { sx, 0, 0 },
                         { 0, sy, 0 },
                         { 0, 0, 1 } };

            for (int i = 0; i < index; i++)
            {
                double[] arra_of_3Ponits = { points_of_array[i].X, points_of_array[i].Y, 1 };
                arra_of_3Ponits[0] -= The_start.X;
                arra_of_3Ponits[1] -= The_start.Y;
                double[] newPoint = Multiply_Double_Numbers(Scal_array, arra_of_3Ponits);
                newPoint[0] += The_start.X;
                newPoint[1] += The_start.Y;
                points_of_array[i] = new Point((int)Math.Round(newPoint[0]), (int)Math.Round(newPoint[1]));
            }
            draw_line();
            pictureBox1.Image = bitmap;
        }
        
        public void Bresenham_Load(object sender, EventArgs e) {}
        void textBox1_TextChanged(object sender, EventArgs e){}
        public void textBox2_TextChanged(object sender, EventArgs e){ }
        public void textBox3_TextChanged(object sender, EventArgs e) { }
        public void textBox4_TextChanged(object sender, EventArgs e) { }
        public void label2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
       private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int angle = int.Parse(textBox7.Text);
            Bitmap_for_Rotation(angle);
        }
        public void ClearImage()
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.SeaShell);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ClearImage();
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int tx = int.Parse(textBox5.Text),
              ty = int.Parse(textBox6.Text);
            Translate_Function(tx, ty);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            float sx = float.Parse(textBox8.Text);
            float sy = float.Parse(textBox9.Text);
            Scale_Function(sx, sy);
        }

        private void button7_Click(object sender, EventArgs e)
        {           
                pictureBox1.Image = null;
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();           
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }


        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        public Bitmap Bitmap_for_Rotation(int angle)
        {
            double theta_angle = angle * 3.14 / 180;
            double[,] R = {{Math.Cos(theta_angle), -Math.Sin(theta_angle), 0 },
                        {Math.Sin(theta_angle), Math.Cos(theta_angle), 0 },
                        {0, 0, 0 } };
            for (int i = 0; i < index; i++)
            {
                double[] TEMP = { points_of_array[i].X, points_of_array[i].Y, 1 };
                TEMP[0] -= The_start.X;
                TEMP[1] -= The_start.Y;
                double[] newPoint = Multiply_Double_Numbers(R, TEMP);
                newPoint[0] += The_start.X;
                newPoint[1] += The_start.Y;
                points_of_array[i] = new Point((int)(newPoint[0]), (int)(newPoint[1]));
            }
            draw_line();
            return bitmap;
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
/* public void Clip_Function(int x_start, int y_start, int x_end, int y_end)
        {
            xmin = The_start.X;
            ymin = The_start.Y;
            xmax = The_end.X;
            ymax = The_end.Y;
            //if(Regoin_arrary()!="Inside")
            //int[] first_decode = Region_coding(xmin, ymin);
            //int [] second_decode = Region_coding(xmax, ymax);
           // int[] trivially_reject = { 0, 0, 0, 0 };
           // int[] and_result = And_Gate(first_decode, second_decode);
            //if(and_result[1]==0&& and_result[2]==0&& and_result[3]==0&& and_result[0] == 0)

            //{ }//trivially_reject

            //for(int i = 1; i <= 4; i++)
            //{

            //}
            //if (first_decode && second_decode== trivially_reject) { 

            //}
        }*/

/*  public void Region_coding(int x, int y)
        {
            
            string s;           
            if ((x < xmin) && (y > ymax))
                s = "Top_left";
            if ((x < xmin) && ((y < ymax) && (y > ymin)))//the point is at the Left
              s = "Left";
            if ((x < xmin) && (y < ymin))//Point is at Bottom - Left
               s = "Bottom_Left";
             
            if (((x > xmin) && (x < xmax)) && (y > ymax))//Point is at Top
                s = "Top";
            if (((x > xmin) && (x < xmax)) && (((y > ymin) && (y < ymax))))// Point is at Inside
               s = "Inside";
             
            if (((x > xmin) && (x < xmax)) && (y < ymin))//Point is at Bottom
              s = "Bottom";
             
            if ((x > xmax) && (y > ymax))// Point is at Top - Right
               s = "Top_Right";
            
            if ((x > xmax) && ((y > ymin) && (y < ymax)))//Point is at Right
                s = "Right";
            if ((x > xmax) && (y < ymin))//Point is at Bottom - Right
            s = "Bottom_Right";
            
            //return s;
        }*/
/*  public int[] Regoin_array(string s)
        {
            int[] array_of_Region_coding = { 0 };
            if (s=="Top_left")
            {
                    array_of_Region_coding[1] = 1;//Point is at Top - Left
                    array_of_Region_coding[2] = 0;
                    array_of_Region_coding[3] = 0;
                    array_of_Region_coding[4] = 1;

            }
            if (s == "Left")
            {
                array_of_Region_coding[1] = 0;
                array_of_Region_coding[2] = 0;
                array_of_Region_coding[3] = 0;
                array_of_Region_coding[4] = 1;
            }if (s== "Bottom_Left")
            {
                array_of_Region_coding[1] = 0;
                array_of_Region_coding[2] = 1;
                array_of_Region_coding[3] = 0;
                array_of_Region_coding[4] = 1;
            }
            if (s == "Top")
            {
                array_of_Region_coding[1] = 1;
                array_of_Region_coding[2] = 0;
                array_of_Region_coding[3] = 0;
                array_of_Region_coding[4] = 0;
            }
            if (s == "Inside")
            {
                array_of_Region_coding[1] = 0;
                array_of_Region_coding[2] = 0;
                array_of_Region_coding[3] = 0;
                array_of_Region_coding[4] = 0;
            }
            if (s == "Bottom")
            {
                array_of_Region_coding[1] = 0;
                array_of_Region_coding[2] = 1;
                array_of_Region_coding[3] = 0;
                array_of_Region_coding[4] = 0;
            }
            if(s == "Top_Right")
            {
                array_of_Region_coding[1] = 1;
                array_of_Region_coding[2] = 0;
                array_of_Region_coding[3] = 1;
                array_of_Region_coding[4] = 0;
            }
            if (s == "Right")
            {
                array_of_Region_coding[1] = 0;
                array_of_Region_coding[2] = 0;
                array_of_Region_coding[3] = 1;
                array_of_Region_coding[4] = 0;
            }
            if (s == "Bottom_Right")
            {
                array_of_Region_coding[1] = 0;
                array_of_Region_coding[2] = 1;
                array_of_Region_coding[3] = 1;
                array_of_Region_coding[4] = 0;
            }
                return array_of_Region_coding;
        }
      */