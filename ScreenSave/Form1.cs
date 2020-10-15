using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSave
{
    public partial class Form1 : Form  // Хранение координат каждой точки по трем измерениям
    {
        public class Point 
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }
        
        private Point[] points = new Point[1500]; // 15к элементов массива точек

        private Random random = new Random(); // Объект для генерации случайных чисел

        private Graphics graphics; // Объект для отрисовки заставки с помощью GDI

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e) // Обработка движения точек и их отрисовка
        {
            graphics.Clear(Color.Black); // Очистка черным цветом

            foreach (var point in points)
            {
                DrawPoint(point);
                MovePoint(point);
            }

            pictureBox1.Refresh(); // Команда для отображения на pictureBox
        }

        private void MovePoint(Point point) // Скорость движения точек в третьем измерении 
        {
            point.Z -= 10;
            if (point.Z < 1)
            {
                point.X = random.Next(-pictureBox1.Width, pictureBox1.Width); // Сбрасывание по осям, чтобы точки появлялись заново
                point.Y = random.Next(-pictureBox1.Height, pictureBox1.Height);
                point.Z = random.Next(1, pictureBox1.Width);
            }
        }

        private void DrawPoint(Point point)
        {
            float pointSize = 4; // Размер точки

            float x = Interpolation(point.X / point.Z, 0, 1, 0, pictureBox1.Width) + pictureBox1.Width / 2; // Преобразование координат

            float y = Interpolation(point.Y / point.Z, 0, 1, 0, pictureBox1.Height) + pictureBox1.Height / 2;

            graphics.FillEllipse(Brushes.White, x, y, pointSize, pointSize); // Метод для отрисовки кругов
        }

        private float Interpolation(float n, float start1, float stop1, float start2, float stop2) // Интерполяция для расчета положения точек ( Взято с форума и переделано  )
        {
            return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Создается картинка(Bitmap), соответствующая размеру boxa

            graphics = Graphics.FromImage(pictureBox1.Image); // Создается объект, в который в качестве параметра передается созданное изображение (Оперативка)

            for (int i = 0; i < points.Length; i++) // Создание каждого элемента ( точки )
            {
                points[i] = new Point() // Создание точки
                {
                    X = random.Next(-pictureBox1.Width, pictureBox1.Width), // Становление координаты с помощью генератора случайных чисел
                    Y = random.Next(-pictureBox1.Height, pictureBox1.Height),
                    Z = random.Next(1, pictureBox1.Width)
                };
            }

            timer1.Start();
        }
    }
}
