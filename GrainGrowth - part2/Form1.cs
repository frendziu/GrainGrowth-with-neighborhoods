using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GrainGrowth___part2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        Bitmap DrawArea;
        int val = 1;
        int sizeX, sizeY, ilosc_wiersz, ilosc_kolumna, promien, ilosc, promien_sasiedztwo;
        float x_f, y_f, size_x, size_y;
        int[,] cells_status;
        bool periodyczne;
        bool grain_growth;
        bool vonneumann = false;
        bool moore = false;
        bool pentarandom = false;
        bool heksaleft = false;
        bool heksaright = false;
        bool heksarandom = false;
        bool zpromieniem = false;
        string sasiedztwo;

        SolidBrush[] solidBrushes;
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random random = new Random();

        
        private void InitializeData()
        {
            x_f = (float)sizeX;
            y_f = (float)sizeY;
            size_x = pictureBox1.Size.Width / x_f;
            size_y = pictureBox1.Size.Height / y_f;
            if (size_x < size_y)
                size_y = size_x;
            else
                size_x = size_y;

            Match_somsiad();

        }

        private void Set_Limits()
        {
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 10000; //rozmiar siatki x
            numericUpDown1.Value = 50;

            numericUpDown2.Minimum = 1;
            numericUpDown2.Maximum = 10000; //rozmiar siatki y
            numericUpDown2.Value = 50;

            numericUpDown3.Minimum = 1;
            numericUpDown3.Maximum = sizeX; //ilosc ziaren w wierszu
            numericUpDown3.Value = 1;

            numericUpDown4.Minimum = 1;
            numericUpDown4.Maximum = sizeY; //ilosc ziaren w kolumnie
            numericUpDown4.Value = 1;

            numericUpDown5.Minimum = 1;
            numericUpDown5.Maximum = 20; //promien
            numericUpDown5.Value = 1;

            numericUpDown6.Minimum = 1;
            numericUpDown6.Maximum = 20; //ilosc
            numericUpDown6.Value = 1;

            numericUpDown7.Minimum = 1;
            numericUpDown7.Maximum = 20; //promień sądziedztwa
            numericUpDown7.Value = 1;

        }


        private void Colors()
        {
            Random rand = new Random();
            int r, g, b;
            solidBrushes = new SolidBrush[1000];
            solidBrushes[0] = new SolidBrush(Color.White);
            for (int i = 1; i < 1000; i++)
            {
                r = rand.Next(255);
                g = rand.Next(255);
                b = rand.Next(255);
                solidBrushes[i] = new SolidBrush(Color.FromArgb(r, g, b));
            }
        }

        private void Fill_combobox()
        {
            comboBox1.Items.Add("Jednorodne");
            comboBox1.Items.Add("Losowe");
            comboBox1.Items.Add("Z promieniem");
            comboBox1.Items.Add("Wyklikanie");

            comboBox2.Items.Add("VonNeumann");
            comboBox2.Items.Add("Moore");
            comboBox2.Items.Add("Pentagonalne Losowe");
            comboBox2.Items.Add("Heksagonalne Lewe");
            comboBox2.Items.Add("Heksagonalne Prawe");
            comboBox2.Items.Add("Heksagonalne Losowe");
            comboBox2.Items.Add("Z Promieniem");



        }

        private void new_thread()
        {
            
                while (grain_growth)
                {
                    Print_Grain();
                    Thread.Sleep(1000);
                }
 
        }

        private void Print_Grain()
        {
            InitializeData();
            lock (graphics)
            {
               Graphics grp;
                grp = Graphics.FromImage(DrawArea);
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        for (int k = 0; k < 1000; k++)
                        {
                            if (cells_status[i, j] == k)
                                grp.FillRectangle(solidBrushes[k], j * size_x, i * size_y, size_x, size_y);
                        }
                    }
                }

                if (vonneumann)
                {
                    cells_status = fun_vonNeumann(cells_status, sizeY, sizeX);
                    
                }
                else if (moore)
                {
                    cells_status = fun_moore(cells_status, sizeY, sizeX);
                    
                }

                else if (pentarandom)
                {
                    cells_status = fun_pentarandon(cells_status, sizeY, sizeX);
                   
                }
                else if (heksaleft)
                {
                    cells_status = fun_heksaleft(cells_status, sizeY, sizeX);
                    
                }
                else if (heksaright)
                {
                    cells_status = fun_heksaright(cells_status, sizeY, sizeX);
                   
                }
               else if (heksarandom)
                {
                   
                       cells_status = fun_heksarandom(cells_status, sizeY, sizeX);
                 
                }
                //else if (zpromieniem)
                //{
                //    cells_status = fun_z_promieniem(cells_status, sizeY, sizeX);
                //}
                
                pictureBox1.Image = DrawArea;
               grp.Dispose();
            }
           


        }

        public int [,] fun_heksarandom(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            Random rand = new Random();
            int wartosc = 0;
            int max_wartosc = 0;
            int licznik = 0;
            int max_licznik = 0;
            int[] neighbour;
            neighbour = new int[7];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (periodyczne)
                    {
                        switch (rand.Next(2))
                        {
                            case 0:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, n - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, 0]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[m - 1, j]; neighbour[1] = tab[m - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, j - 1]; neighbour[6] = tab[0, j];
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = tab[m - 1, j]; neighbour[1] = tab[m - 1, j + 1]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, n - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = tab[m - 1, j]; neighbour[1] = tab[m - 1, 0]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, 0]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[0, j - 1]; neighbour[6] = tab[0, j];
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, n - 1]; neighbour[6] = tab[0, j];
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    for (int l = 0; l < 7; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 7; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                            case 1:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, n - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, 0];
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[m - 1, j - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, j]; neighbour[6] = tab[0, j + 1];
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = tab[m - 1, n - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = tab[m - 1, j - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, 0];
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[0, j]; neighbour[6] = tab[0, 0];
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = tab[i - 1, n - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, j]; neighbour[6] = tab[0, j + 1];
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    for (int l = 0; l < 7; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 7; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                        }
                    }
                    else
                    {
                        switch (rand.Next(2))
                        {
                            case 0:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0; neighbour[6] = 0;
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                                    }
                                    for (int l = 0; l < 7; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 7; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                            case 1:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j]; neighbour[6] = 0;
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j]; neighbour[6] = 0;
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0; neighbour[6] = 0;
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                                    }
                                    for (int l = 0; l < 7; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 7; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                        }
                    }

                }
            }

            return tab1;
        }

        public int [,] fun_pentarandon(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;
            Random rand = new Random();

            int wartosc = 0;
            int aktualna_wartosc = 0;
            int max_wartosc = 0;
            int licznik = 0;
            int max_licznik = 0;
            int[] neighbour;
            neighbour = new int[6];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (periodyczne)
                    {
                        if (tab[i, j] == 0)
                        {
                            switch (rand.Next(4))
                            {
                                case 0:
                                    for (int r = -1; r < 2; r++)
                                        for (int t = 0; t < 2; t++)
                                        {
                                            licznik = 0;
                                            aktualna_wartosc = tab[(i + r + m) % m, (j + t + n) % n];
                                            for (int k = -1; k < 2; k++)
                                            {
                                                for (int l = 0; l < 2; l++)
                                                {
                                                    if ((aktualna_wartosc == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                        licznik++;
                                                }
                                            }
                                            if (licznik > max_licznik)
                                            {
                                                max_licznik = licznik;
                                                max_wartosc = aktualna_wartosc;
                                            }
                                            licznik = 0;
                                        }

                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                    max_licznik = 0;
                                    break;
                                case 1:
                                    for (int r = -1; r < 2; r++)
                                        for (int t = -1; t < 1; t++)
                                        {
                                            licznik = 0;
                                            aktualna_wartosc = tab[(i + r + m) % m, (j + t + n) % n];
                                            for (int k = -1; k < 2; k++)
                                            {
                                                for (int l = -1; l < 1; l++)
                                                {
                                                    if ((aktualna_wartosc == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                        licznik++;
                                                }
                                            }
                                            if (licznik > max_licznik)
                                            {
                                                max_licznik = licznik;
                                                max_wartosc = aktualna_wartosc;
                                            }
                                            licznik = 0;
                                        }

                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                    max_licznik = 0;
                                    break;
                                case 2:
                                    for (int r = 0; r < 2; r++)
                                        for (int t = -1; t < 2; t++)
                                        {
                                            licznik = 0;
                                            aktualna_wartosc = tab[(i + r + m) % m, (j + t + n) % n];
                                            for (int k = 0; k < 2; k++)
                                            {
                                                for (int l = -1; l < 2; l++)
                                                {
                                                    if ((aktualna_wartosc == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                        licznik++;
                                                }
                                            }
                                            if (licznik > max_licznik)
                                            {
                                                max_licznik = licznik;
                                                max_wartosc = aktualna_wartosc;
                                            }
                                            licznik = 0;
                                        }

                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                    max_licznik = 0;
                                    break;
                                case 3:
                                    for (int r = -1; r < 1; r++)
                                        for (int t = -1; t < 2; t++)
                                        {
                                            licznik = 0;
                                            aktualna_wartosc = tab[(i + r + m) % m, (j + t + n) % n];
                                            for (int k = -1; k < 1; k++)
                                            {
                                                for (int l = -1; l < 2; l++)
                                                {
                                                    if ((aktualna_wartosc == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                        licznik++;
                                                }
                                            }
                                            if (licznik > max_licznik)
                                            {
                                                max_licznik = licznik;
                                                max_wartosc = aktualna_wartosc;
                                            }
                                            licznik = 0;
                                        }

                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                    max_licznik = 0;
                                    break;

                            }
                        }
                        else
                            tab1[i, j] = tab[i, j];
                    }
                    else
                    {
                        switch (rand.Next(4))
                        {
                            case 0:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    for (int l = 0; l < 6; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 6; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                            case 1:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                                    }
                                    for (int l = 0; l < 6; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 6; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                            case 2:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = 0; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = 0; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = 0; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                                    }
                                    for (int l = 0; l < 6; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 6; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                            case 3:
                                wartosc = 0; licznik = 0; max_licznik = 0;
                                if (tab[i, j] == 0)
                                {
                                    if (j == 0 & i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                                    }
                                    else if (j == n - 1 && i != 0 && i != m - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0;
                                    }
                                    else if (i == 0 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                                    }
                                    else if (i == m - 1 && j != 0 && j != n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                                    }
                                    else if (i == 0 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                                    }
                                    else if (i == 0 && j == n - 1)
                                    {
                                        neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == n - 1)
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0;
                                    }
                                    else if (i == m - 1 && j == 0)
                                    {
                                        neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                                    }
                                    else
                                    {
                                        neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                                    }
                                    for (int l = 0; l < 6; l++)
                                    {
                                        wartosc = neighbour[l];
                                        for (int k = 0; k < 6; k++)
                                        {
                                            if (wartosc == neighbour[k] && neighbour[k] != 0)
                                            {
                                                licznik++;
                                            }
                                        }
                                        if (licznik > max_licznik)
                                        {
                                            max_licznik = licznik;
                                            max_wartosc = wartosc;
                                        }
                                        licznik = 0;

                                    }
                                    tab1[i, j] = max_wartosc;
                                    max_wartosc = 0;
                                }
                                else
                                    tab1[i, j] = tab[i, j];
                                break;
                        }
                    }
                }
            }
            return tab1;
        }
    

        public int[,] fun_vonNeumann(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tabNeighbours = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tabNeighbours[i, j] = 0;

            int value = 0;
            int max_value = 0;
            int nighbour_number = 0;
            int max = 0;
            int[] neighbour;
            neighbour = new int[4];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    value = 0; nighbour_number = 0; max = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {

                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i, n - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, 0]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[0, j];
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = tab[i, n - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, 0]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, 0]; neighbour[3] = tab[0, j];
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = tab[i, n - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[0, j];
                            }
                            else
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1) //dol
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1) //gora
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == 0 && j != 0 && j != n - 1) // lewa
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = 0; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1) //prawa
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0;
                            }
                            else if (i == 0 && j == 0) //rog lewo-dol
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == 0 && j == n - 1)// rog lewo-gora
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j == n - 1)//prawo-gora
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = 0;
                            }
                            else if (i == m - 1 && j == 0) //prawo-dol
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0;
                            }
                            else //srodek
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j];
                            }
                        }
                        for (int l = 0; l < 4; l++)
                        {
                            value = neighbour[l];
                            for (int k = 0; k < 4; k++)
                            {
                                if (value == neighbour[k] && neighbour[k] != 0)
                                {
                                    nighbour_number++;
                                }
                            }
                            if (nighbour_number > max)
                            {
                                max = nighbour_number;
                                max_value = value;
                            }
                            nighbour_number = 0;

                        }
                        tabNeighbours[i, j] = max_value;
                        max_value = 0;
                    }
                    else
                        tabNeighbours[i, j] = tab[i, j];
                }
            }

            return tabNeighbours;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            grain_growth = true;
            if (grain_growth)
            {
                Thread th = new Thread(new_thread);
                th.Start();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            grain_growth = false;
        }

        public int[,] fun_moore(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }

            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            int value = 0;
            int aktualna_value = 0;
            int max_value = 0;
            int nighbour_number = 0;
            int max_nighbour_number = 0;
            int[] neighbour;
            neighbour = new int[9];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            for (int r = -1; r < 2; r++)
                                for (int t = -1; t < 2; t++)
                                {
                                    nighbour_number = 0;
                                    aktualna_value = tab[(i + r + m) % m, (j + t + n) % n];
                                    for (int k = -1; k < 2; k++)
                                    {
                                        for (int l = -1; l < 2; l++)
                                        {
                                            if ((aktualna_value == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                nighbour_number++;
                                        }
                                    }
                                    if (nighbour_number > max_nighbour_number)
                                    {
                                        max_nighbour_number = nighbour_number;
                                        max_value = aktualna_value;
                                    }
                                    nighbour_number = 0;
                                }
                        }
                        else
                        {

                            value = 0; nighbour_number = 0; max_nighbour_number = 0;
                            if (tab[i, j] == 0)
                            {
                                if (j == 0 & i != 0 && i != m - 1)
                                {
                                    neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1]; neighbour[6] = 0; neighbour[7] = tab[i + 1, j]; neighbour[8] = tab[i + 1, j + 1];
                                }
                                else if (j == n - 1 && i != 0 && i != m - 1)
                                {
                                    neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0; neighbour[6] = tab[i + 1, j - 1]; neighbour[7] = tab[i + 1, j]; neighbour[8] = 0;
                                }
                                else if (i == 0 && j != 0 && j != n - 1)
                                {
                                    neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1]; neighbour[6] = tab[i + 1, j - 1]; neighbour[7] = tab[i + 1, j]; neighbour[8] = tab[i + 1, j + 1];
                                }
                                else if (i == m - 1 && j != 0 && j != n - 1)
                                {
                                    neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1]; neighbour[6] = 0; neighbour[7] = 0; neighbour[8] = 0;
                                }
                                else if (i == 0 && j == 0)
                                {
                                    neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1]; neighbour[6] = 0; neighbour[7] = tab[i + 1, j]; neighbour[8] = tab[i + 1, j + 1];
                                }
                                else if (i == 0 && j == n - 1)
                                {
                                    neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0; neighbour[6] = tab[i + 1, j - 1]; neighbour[7] = tab[i + 1, j]; neighbour[8] = 0;
                                }
                                else if (i == m - 1 && j == n - 1)
                                {
                                    neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0; neighbour[6] = 0; neighbour[7] = 0; neighbour[8] = 0;
                                }
                                else if (i == m - 1 && j == 0)
                                {
                                    neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1]; neighbour[6] = 0; neighbour[7] = 0; neighbour[8] = 0;
                                }
                                else
                                {
                                    neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1]; neighbour[6] = tab[i + 1, j - 1]; neighbour[7] = tab[i + 1, j]; neighbour[8] = tab[i + 1, j + 1];
                                }
                                for (int l = 0; l < 9; l++)
                                {
                                    value = neighbour[l];
                                    for (int k = 0; k < 9; k++)
                                    {
                                        if (value == neighbour[k] && neighbour[k] != 0)
                                        {
                                            nighbour_number++;
                                        }
                                    }
                                    if (nighbour_number > max_nighbour_number)
                                    {
                                        max_nighbour_number = nighbour_number;
                                        max_value = value;
                                    }
                                    nighbour_number = 0;

                                }
                            }
                        }  
                            tab1[i, j] = max_value;
                        max_value = 0;
                        max_nighbour_number = 0;
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }
            return tab1;
        }

        public int[,] fun_pentaleft(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            int aktualna_value = 0;
            int max_value = 0;
            int value = 0;
            int nighbour_number = 0;
            int max_nighbour_number = 0;
            int[] neighbour;
            neighbour = new int[6];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    value = 0; nighbour_number = 0; max_nighbour_number = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            for (int r = -1; r < 2; r++)
                                for (int t = 0; t < 2; t++)
                                {
                                    nighbour_number = 0;
                                    aktualna_value = tab[(i + r + m) % m, (j + t + n) % n];
                                    for (int k = -1; k < 2; k++)
                                    {
                                        for (int l = 0; l < 2; l++)
                                        {
                                            if ((aktualna_value == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                nighbour_number++;
                                        }
                                    }
                                    if (nighbour_number > max_nighbour_number)
                                    {
                                        max_nighbour_number = nighbour_number;
                                        max_value = aktualna_value;
                                    }
                                    nighbour_number = 0;
                                }

                            tab1[i, j] = max_value;
                            max_value = 0;

                            max_nighbour_number = 0;
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j]; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j]; neighbour[3] = tab[i, j + 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            for (int l = 0; l < 6; l++)
                            {
                                value = neighbour[l];
                                for (int k = 0; k < 6; k++)
                                {
                                    if (value == neighbour[k] && neighbour[k] != 0)
                                    {
                                        nighbour_number++;
                                    }
                                }
                                if (nighbour_number > max_nighbour_number)
                                {
                                    max_nighbour_number = nighbour_number;
                                    max_value = value;
                                }
                                nighbour_number = 0;

                            }
                            tab1[i, j] = max_value;
                            max_value = 0;
                        }
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }
            return tab1;
        }


        public int[,] fun_pentaright(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            int aktualna_value = 0;
            int max_value = 0;
            int value = 0;
            int nighbour_number = 0;
            int max_nighbour_number = 0;
            int[] neighbour;
            neighbour = new int[6];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    value = 0; nighbour_number = 0; max_nighbour_number = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            for (int r = -1; r < 2; r++)
                                for (int t = 0; t < 2; t++)
                                {
                                    nighbour_number = 0;
                                    aktualna_value = tab[(i + r + m) % m, (j + t + n) % n];
                                    for (int k = -1; k < 2; k++)
                                    {
                                        for (int l = -1; l < 1; l++)
                                        {
                                            if ((aktualna_value == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                nighbour_number++;
                                        }
                                    }
                                    if (nighbour_number > max_nighbour_number)
                                    {
                                        max_nighbour_number = nighbour_number;
                                        max_value = aktualna_value;
                                    }
                                    nighbour_number = 0;
                                }

                            tab1[i, j] = max_value;
                            max_value = 0;

                            max_nighbour_number = 0;
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i + 1, j - 1]; neighbour[5] = tab[i + 1, j];
                            }
                            for (int l = 0; l < 6; l++)
                            {
                                value = neighbour[l];
                                for (int k = 0; k < 6; k++)
                                {
                                    if (value == neighbour[k] && neighbour[k] != 0)
                                    {
                                        nighbour_number++;
                                    }
                                }
                                if (nighbour_number > max_nighbour_number)
                                {
                                    max_nighbour_number = nighbour_number;
                                    max_value = value;
                                }
                                nighbour_number = 0;

                            }
                            tab1[i, j] = max_value;
                            max_value = 0;
                        }
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }
            return tab1;

        }

        private void NumericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            promien_sasiedztwo = decimal.ToInt32(numericUpDown7.Value);
        }

        public int[,] fun_pentaup(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i<m; i++)
                for (int j = 0; j<n; j++)
                    tab1[i, j] = 0;

            int aktualna_value = 0;
        int max_value = 0;
        int value = 0;
        int nighbour_number = 0;
        int max_nighbour_number = 0;
        int[] neighbour;
        neighbour = new int[6];
            for (int i = 0; i<m; i++)
            {
                for (int j = 0; j<n; j++)
                {
                    value = 0; nighbour_number = 0; max_nighbour_number = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            for(int r = 0; r < 2; r++)
                                for (int t = -1; t < 2; t++)
                                   {
                                     nighbour_number = 0;
                                        aktualna_value = tab[(i + r + m) % m, (j + t + n) % n];
                                             for (int k = 0; k < 2; k++)
                                              {
                                                    for (int l = -1; l < 2; l++)
                                                    {
                                                     if ((aktualna_value == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                      nighbour_number++;
                                                       }
                                                }
                                    if (nighbour_number > max_nighbour_number)
                                    {
                                    max_nighbour_number = nighbour_number;
                                    max_value = aktualna_value;
                                    }
                                 nighbour_number = 0;
                                }

                                tab1[i, j] = max_value;
                                max_value = 0;
                                max_nighbour_number = 0;
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = 0; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = 0; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = 0; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = 0; neighbour[4] = 0; neighbour[5] = 0;
                            }
                            else
                            {
                                neighbour[0] = tab[i, j - 1]; neighbour[1] = tab[i, j]; neighbour[2] = tab[i, j + 1]; neighbour[3] = tab[i + 1, j - 1]; neighbour[4] = tab[i + 1, j]; neighbour[5] = tab[i + 1, j + 1];
                            }
                            for (int l = 0; l< 6; l++)
                            {
                                value = neighbour[l];
                                for (int k = 0; k< 6; k++)
                                {
                                    if (value == neighbour[k] && neighbour[k] != 0)
                                    {
                                        nighbour_number++;
                                    }
                                }
                                if (nighbour_number > max_nighbour_number)
                                {
                                    max_nighbour_number = nighbour_number;
                                    max_value = value;
                                }
                                nighbour_number = 0;

                            }
                            tab1[i, j] = max_value;
                            max_value = 0;
                        }
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }
            return tab1;
        }
        public int[,] fun_pentadown(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            int aktualna_value = 0;
            int max_value = 0;
            int value = 0;
            int nighbour_number = 0;
            int max_nighbour_number = 0;
            int[] neighbour;
            neighbour = new int[6];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    value = 0; nighbour_number = 0; max_nighbour_number = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            for (int r = -1; r < 1; r++)
                                for (int t = -1; t < 2; t++)
                                {
                                    nighbour_number = 0;
                                    aktualna_value = tab[(i + r + m) % m, (j + t + n) % n];
                                    for (int k = -1; k < 1; k++)
                                    {
                                        for (int l = -1; l < 2; l++)
                                        {
                                            if ((aktualna_value == tab[(i + k + m) % m, (j + l + n) % n]) && tab[(i + k + m) % m, (j + l + n) % n] != 0)
                                                nighbour_number++;
                                        }
                                    }
                                    if (nighbour_number > max_nighbour_number)
                                    {
                                        max_nighbour_number = nighbour_number;
                                        max_value = aktualna_value;
                                    }
                                    nighbour_number = 0;
                                }

                            tab1[i, j] = max_value;
                            max_value = 0;
                            max_nighbour_number = 0;
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0;
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = 0;
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = 0; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i - 1, j + 1]; neighbour[3] = tab[i, j - 1]; neighbour[4] = tab[i, j]; neighbour[5] = tab[i, j + 1];
                            }
                            for (int l = 0; l < 6; l++)
                            {
                                value = neighbour[l];
                                for (int k = 0; k < 6; k++)
                                {
                                    if (value == neighbour[k] && neighbour[k] != 0)
                                    {
                                        nighbour_number++;
                                    }
                                }
                                if (nighbour_number > max_nighbour_number)
                                {
                                    max_nighbour_number = nighbour_number;
                                    max_value = value;
                                }
                                nighbour_number = 0;

                            }
                            tab1[i, j] = max_value;
                            max_value = 0;
                        }
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }
            return tab1;
        }
        public int[,] fun_heksaleft(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            int value = 0;
            int max_value = 0;
            int nighbour_number = 0;
            int max_nighbour_number = 0;
            int[] neighbour;
            neighbour = new int[7];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    value = 0; nighbour_number = 0; max_nighbour_number = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, n - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, 0]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[m - 1, j]; neighbour[1] = tab[m - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, j - 1]; neighbour[6] = tab[0, j];
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = tab[m - 1, j]; neighbour[1] = tab[m - 1, j + 1]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, n - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = tab[m - 1, j]; neighbour[1] = tab[m - 1, 0]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, 0]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[0, j - 1]; neighbour[6] = tab[0, j];
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, n - 1]; neighbour[6] = tab[0, j];
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = tab[i + 1, j];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0; neighbour[6] = 0;
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j]; neighbour[1] = tab[i - 1, j + 1]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j - 1]; neighbour[6] = tab[i + 1, j];
                            }
                        }
                        for (int l = 0; l < 7; l++)
                        {
                            value = neighbour[l];
                            for (int k = 0; k < 7; k++)
                            {
                                if (value == neighbour[k] && neighbour[k] != 0)
                                {
                                    nighbour_number++;
                                }
                            }
                            if (nighbour_number > max_nighbour_number)
                            {
                                max_nighbour_number = nighbour_number;
                                max_value = value;
                            }
                            nighbour_number = 0;

                        }
                        tab1[i, j] = max_value;
                        max_value = 0;
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }

            return tab1;
        }
        public int[,] fun_heksaright(int[,] tab, int m, int n)
        {
            if (checkBox1.Checked) { periodyczne = true; } else { periodyczne = false; }
            int[,] tab1 = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    tab1[i, j] = 0;

            int value = 0;
            int max_value = 0;
            int nighbour_number = 0;
            int max_nighbour_number = 0;
            int[] neighbour;
            neighbour = new int[7];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    value = 0; nighbour_number = 0; max_nighbour_number = 0;
                    if (tab[i, j] == 0)
                    {
                        if (periodyczne)
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, n - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, 0];
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[m - 1, j - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, j]; neighbour[6] = tab[0, j + 1];
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = tab[m - 1, n - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = tab[m - 1, j - 1]; neighbour[1] = tab[m - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, 0];
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, 0]; neighbour[5] = tab[0, j]; neighbour[6] = tab[0, 0];
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = tab[i - 1, n - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, n - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[0, j]; neighbour[6] = tab[0, j + 1];
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                        }
                        else
                        {
                            if (j == 0 & i != 0 && i != m - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                            else if (j == n - 1 && i != 0 && i != m - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j]; neighbour[6] = 0;
                            }
                            else if (i == 0 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                            else if (i == m - 1 && j != 0 && j != n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                            }
                            else if (i == 0 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                            else if (i == 0 && j == n - 1)
                            {
                                neighbour[0] = 0; neighbour[1] = 0; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = tab[i + 1, j]; neighbour[6] = 0;
                            }
                            else if (i == m - 1 && j == n - 1)
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = 0; neighbour[5] = 0; neighbour[6] = 0;
                            }
                            else if (i == m - 1 && j == 0)
                            {
                                neighbour[0] = 0; neighbour[1] = tab[i - 1, j]; neighbour[2] = 0; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = 0; neighbour[6] = 0;
                            }
                            else
                            {
                                neighbour[0] = tab[i - 1, j - 1]; neighbour[1] = tab[i - 1, j]; neighbour[2] = tab[i, j - 1]; neighbour[3] = tab[i, j]; neighbour[4] = tab[i, j + 1]; neighbour[5] = tab[i + 1, j]; neighbour[6] = tab[i + 1, j + 1];
                            }
                        }
                        for (int l = 0; l < 7; l++)
                        {
                            value = neighbour[l];
                            for (int k = 0; k < 7; k++)
                            {
                                if (value == neighbour[k] && neighbour[k] != 0)
                                {
                                    nighbour_number++;
                                }
                            }
                            if (nighbour_number > max_nighbour_number)
                            {
                                max_nighbour_number = nighbour_number;
                                max_value = value;
                            }
                            nighbour_number = 0;

                        }
                        tab1[i, j] = max_value;
                        max_value = 0;
                    }
                    else
                        tab1[i, j] = tab[i, j];
                }
            }

            return tab1;
        }

        //public int[,] fun_z_promieniem(int[,] tab, int m, int n)
        //{

           
        //}


        public Form1()
        {
            InitializeComponent();
            Fill_combobox();
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            graphics = Graphics.FromImage(DrawArea);
            
            Colors();
            Set_Limits();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            InitializeData();
            Graphics graphics;
            graphics = Graphics.FromImage(DrawArea);

            MouseEventArgs me = (MouseEventArgs)e;
            int x = me.Location.X;
            int y = me.Location.Y;

            x = me.Location.X;
            y = me.Location.Y;

            float j_f = x / size_x;
            float i_f = y / size_y;
            int j_i = (int)j_f;
            int i_i = (int)i_f;

            cells_status[i_i, j_i] = val;
            val++;
            graphics.Clear(Color.DarkGray);

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    for (int k = 0; k < 1000; k++)
                        if (cells_status[i, j] == k)
                            graphics.FillRectangle(solidBrushes[k], j * size_x, i * size_y, size_x, size_y);

                }


            }

            pictureBox1.Image = DrawArea;
            graphics.Dispose();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            sizeX = decimal.ToInt32(numericUpDown1.Value);
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            sizeY = decimal.ToInt32(numericUpDown2.Value);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            ilosc_wiersz = decimal.ToInt32(numericUpDown3.Value);
        }

        private void NumericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            ilosc_kolumna = decimal.ToInt32(numericUpDown4.Value);
        }

        private void NumericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            promien = decimal.ToInt32(numericUpDown5.Value);
        }

        private void NumericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            ilosc = decimal.ToInt32(numericUpDown6.Value);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Match_somsiad()
        {
            switch (sasiedztwo)
            {
                case "VonNeumann":
                    {
                        vonneumann = true;
                        break;
                    }
                case "Moore":
                    {
                        moore = true;
                        break;
                    }
                case "Pentagonalne Losowe":
                    {
                        pentarandom = true;
                        break;
                    }
                case "Heksagonalne Lewe":
                    {
                        heksaleft = true;
                        break;
                    }
                case "Heksagonalne Prawe":
                    {
                        heksaright = true;
                        break;
                    }
                case "Heksagonalne Losowe":
                    {
                        heksarandom = true;
                        break;
                    }
                case "Z promieniem":
                    {
                        zpromieniem = true;
                        break;
                    }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            InitializeData();

            grain_growth = true;

            Graphics grps;
            grps = Graphics.FromImage(DrawArea);

            cells_status = new int[sizeY, sizeX];
            for (int i = 0; i < sizeY; i++)
                for (int j = 0; j < sizeX; j++)
                    cells_status[i, j] = 0;

            string sasiedztwo = comboBox2.SelectedItem.ToString();
            if (sasiedztwo == "VonNeumann")
            {
                vonneumann = true;
            }
            if (sasiedztwo == "Moore")
            {
                moore = true;
            }
            if (sasiedztwo == "Pentagonalne Losowe")
            {
                pentarandom = true;
            }
            if (sasiedztwo == "Heksagonalne Lewe")
            {
                heksaleft = true;
            }
            if (sasiedztwo == "Heksagonalne Prawe")
            {
                heksaright = true;
            }
            if (sasiedztwo == "Heksagonalne Losowe")
            {
                heksarandom = true;
            }
            if (sasiedztwo == "Z Promieniem")
            {
                zpromieniem = true;
            }



            string tekst = comboBox1.SelectedItem.ToString();
            if (tekst == "Jednorodne")
            {
                grps.Clear(Color.DarkGray);
                float ilosc_wiersz_f = (float)ilosc_wiersz;
                float ilosc_kolumna_f = (float)ilosc_kolumna;
                float odstep_wiersz_f = x_f / ilosc_wiersz_f;
                int odstep_wiersz = (int)Math.Ceiling(odstep_wiersz_f);
                float odstep_kolumna_f = y_f / ilosc_kolumna_f;
                int odstep_kolumna = (int)Math.Ceiling(odstep_kolumna_f);

                int val = 1;
                for (int i = 0; i < sizeY; i += odstep_kolumna)
                    for (int j = 0; j < sizeX; j += odstep_wiersz)
                    {
                        cells_status[(odstep_kolumna / 2) + i, (odstep_wiersz / 2) + j] = val;
                        val++;
                    }
                if (grain_growth)
                {
                    Thread th = new Thread(new_thread);
                    th.Start();
                }


            }
            else if (tekst == "Z promieniem")
            {
                bool matched = true;
                double odleglosc = 0.0;
                double d = 0.0;
                grps.Clear(Color.DarkGray);
                int promien1 = promien;
                int ilosc1 = ilosc;
                Random rand = new Random();

                for (int k = 1; k < ilosc1 + 1; k++)
                {
                    odleglosc = 0.0;
                    int a = rand.Next(sizeY);
                    int b = rand.Next(sizeX);
                    matched = true;
                    if (cells_status[a, b] == 0)
                    {
                        for (int i = 0; i < sizeY; i++)
                        {
                            for (int j = 0; j < sizeX; j++)
                            {
                                if (cells_status[i, j] != 0)
                                {
                                    d = (i * size_y - a * size_y) * (i * size_y - a * size_y) + (j * size_x - b * size_x) * (j * size_x - b * size_x);
                                    odleglosc = Math.Sqrt(d);
                                    if (odleglosc > 2 * promien1 * size_x)
                                        matched = true;
                                    else
                                    {
                                        matched = false;
                                    }
                                }
                                if (matched == false)
                                    break;
                            }
                            if (matched == false)
                            {
                                if (k > 1)
                                    k--;
                                break;
                            }
                        }
                        if (matched)
                        {
                            cells_status[a, b] = k;
                            matched = false;
                        }

                    }

                }
                if (grain_growth)
                {
                    Thread th = new Thread(new_thread);
                    th.Start();
                }


            }
            else if (tekst == "Losowe")
            {
                grps.Clear(Color.DarkGray);
                Random rand = new Random();
                int ilosc2 = ilosc;
                for (int i = 1; i < ilosc2 + 1; i++)
                {
                    int a = rand.Next(sizeY);
                    int b = rand.Next(sizeX);
                    if (cells_status[a, b] == 0)
                        cells_status[a, b] = i;
                }
                if (grain_growth)
                {
                    Thread th = new Thread(new_thread);
                    th.Start();
                }

            }
            else
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        if (cells_status[i, j] == 1)
                            grps.FillRectangle(blackBrush, j * size_x, i * size_y, size_x, size_y);
                        else
                            grps.FillRectangle(whiteBrush, j * size_x, i * size_y, size_x, size_y);
                    }
                }
                //if (grain_growth)
                //{
                //    Thread th = new Thread(new_thread);
                //    th.Start();
                //}

                pictureBox1.Image = DrawArea;
                grps.Dispose();
            }
        }
    }
}
