using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace placdarm
{
    public partial class Form1 : Form
    {
        public static char [,] fields;
        char[,] newGame;
        int [,] coords;
        Graphics g;
        Color back;
        Color circle;
        static Fishka[] figures;
        bool createFigures;
        public Form1()
        {
             fields = new char[,] { 
                                  {'b','b','b','b','b','b','e','n','n','0'},
                                  {'b','b','b','z','b','b','b','e','0','0'},
                                  {'.','b','b','b','b','b','b','.','e','e'},
                                  {'.','.','b','b','b','b','b','.','.','e'},
                                  {'.','.','.','.','.','.','.','.','.','.'},
                                  {'e','.','.','w','w','w','w','w','.','.'},
                                  {'e','e','.','w','w','w','w','w','w','.'},
                                  {'e','e','e','w','w','w','q','w','w','w'},
                                  {'e','e','e','e','w','w','w','w','w','w'},
                                };             
             newGame = new char[,] { 
                                  {'b','b','b','b','b','b','e','n','n','0'},
                                  {'b','b','b','z','b','b','b','e','0','0'},
                                  {'.','b','b','b','b','b','b','.','e','e'},
                                  {'.','.','b','b','b','b','b','.','.','e'},
                                  {'.','.','.','.','.','.','.','.','.','.'},
                                  {'e','.','.','w','w','w','w','w','.','.'},
                                  {'e','e','.','w','w','w','w','w','w','.'},
                                  {'e','e','e','w','w','w','q','w','w','w'},
                                  {'e','e','e','e','w','w','w','w','w','w'},
                                };
             newGame = new char[,] { //test
                                  {'.','.','.','b','b','b','e','n','n','0'},
                                  {'b','.','.','z','b','.','b','e','0','0'},
                                  {'b','.','.','b','b','.','b','.','e','e'},
                                  {'.','b','.','b','b','.','b','.','.','e'},
                                  {'.','.','.','.','.','.','.','.','.','.'},
                                  {'e','.','.','w','.','.','.','w','.','.'},
                                  {'e','e','.','w','.','.','.','w','w','.'},
                                  {'e','e','e','w','.','.','q','w','w','w'},
                                  {'e','e','e','e','w','w','w','w','w','w'},
                                };
            //---------------------------------------------------------------
            InitializeComponent();
            createFigures = true;
            coords = new int[70, 2];
            figures = new Fishka[70];
            back = Color.DarkBlue;
            circle = Color.Black;
            g = pictureBox1.CreateGraphics();
            //-----------------------------------------------------------------------------
            RegistryKey openKey = Registry.CurrentUser.OpenSubKey("Software\\Placdarm");
            if (openKey == null)
            {
                Registry.CurrentUser.CreateSubKey(@"Software\Placdarm", RegistryKeyPermissionCheck.ReadWriteSubTree);
                openKey = Registry.CurrentUser.OpenSubKey("Software\\Placdarm", RegistryKeyPermissionCheck.ReadWriteSubTree);
                openKey.SetValue("error", false);
                openKey.Close();
            }
            else
            {
                if (openKey.GetValue("error").ToString()=="True")
                {
                    DialogResult res;

                    if (DialogResult.Yes == (res = MessageBox.Show("Игра была завершена некорректно!\nПопытаться восстановить предыдущую игру?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)))
                    {
                        loadGame("saves\\tmp.txt");
                    }
                }
            }
            //-----------------------------------------------------------------------------
        }
        //-----------------------------------------------------------
        //-----------------------------------------------------------
        private void paintField(char[,] field)
        {
            int counter = 0;
            g.Clear(back);
           // int radius = pictureBox1.Size.Width/15;
            int radius = 55;
            byte space = Convert.ToByte(trackBar1.Value);
            Pen MyPen;
            if (radius % 2 != 0)
                radius++;
          
            for (int i = 0; i < field.GetLength(0); i++)
            {

                for (int j = 0; j < field.GetLength(1); j++)
                {
                   
                    if (field[i, j] == 'e' || field[i, j] == '0' || field[i, j] == '1' || field[i, j] == '2' || field[i, j] == '3' ||
                        field[i, j] == '4' || field[i, j] == '5' || field[i, j] == '6' || field[i, j] == '7' || field[i, j] == '8' ||
                        field[i, j] == '9' || field[i, j] == 'n')
                    {
                        MyPen = new Pen(back);
                    }
                    else
                    {
                        MyPen = new Pen(circle);

                        if (i % 2 == 0)
                        {
                           // g.DrawImage(Image.FromFile("images\\black.png"), new Point(j * (radius + space), i * (radius + space - 7)));
                            if (counter != 2 && counter != 3 && counter != 9 && counter != 60 && counter != 66 && counter != 67)
                            {
                                g.FillEllipse(Brushes.White, j * (radius + space) + 6, i * (radius + space - 7), radius, radius);
                            }
                            else g.FillEllipse(Brushes.Aqua, j * (radius + space) + 6, i * (radius + space - 7), radius, radius);
                                coords[counter, 0] = j * (radius + space)+6;
                            coords[counter, 1] = i * (radius + space - 7);

                           // g.DrawString((counter + 1).ToString(), new Font("Arial", radius / 4), Brushes.Black, coords[counter, 0] + radius * 2 / 3 - 4, coords[counter, 1] + radius - radius / 3 - 7);
                            if (createFigures)
                            {
                                int tmp = 0;
                                switch (i)
                                {
                                    case 0: tmp = j - 2; break;
                                    case 2: tmp = j - 1; break;
                                    case 4: tmp = j; break;
                                    case 6: tmp = j + 1; break;
                                    case 8: tmp = j + 2; break;
                                }

                                figures[counter] = new Fishka(field[i, j], (byte)i, (byte)tmp, coords[counter, 0], coords[counter, 1], (byte)(counter + 1), pictureBox1);
                            }
                            else figures[counter].updateXYpix(coords[counter, 0], coords[counter, 1]);
                            counter++;
                        }
                        else
                        {
                            if (counter != 2 && counter != 3 && counter != 9 && counter != 60 && counter != 66 && counter != 67)
                            {
                                g.FillEllipse(Brushes.White, 6 + (j * (radius + space) + radius / 2 + space / 2), i * (radius + space - 7), radius, radius);
                            }
                            else g.FillEllipse(Brushes.Aqua, 6 + (j * (radius + space) + radius / 2 + space / 2), i * (radius + space - 7), radius, radius);
                                //g.DrawImage(Image.FromFile("images\\black.png"), new Point(j * (radius + space) + radius / 2 + space / 2, i * (radius + space - 7)));
                            coords[counter, 0] = 6+(j * (radius + space) + radius / 2 + space / 2);
                            coords[counter, 1] = i * (radius + space - 7);
                          //g.DrawString((counter + 1).ToString(), new Font("Arial", radius / 4), Brushes.Black, coords[counter, 0] + radius * 2 / 3 - 4, coords[counter, 1] + radius - radius / 3);
                            if (createFigures)
                            {
                                int tmp = 0;
                                switch (i)
                                {
                                    case 1: tmp = j - 1; break;
                                    case 3: tmp = j; break;
                                    case 5: tmp = j + 1; break;
                                    case 7: tmp = j + 2; break;
                                    case 9: tmp = j + 3; break;
                                }

                                figures[counter] = new Fishka(field[i, j], (byte)i, (byte)tmp, coords[counter, 0], coords[counter, 1], (byte)(counter + 1), pictureBox1);
                            }
                            else figures[counter].updateXYpix(coords[counter, 0], coords[counter, 1]);
                            counter++;
                        }
                    }

                }
            }
              createFigures = false;
              drawLine();  
        }
        //-----------------------------------------------------------
        //---------------------------------------------------------
        public  void drawLine()
        {
            g.DrawLine(new Pen(Brushes.White), new Point(figures[0].getXpix()+11, figures[0].getYpix()), new Point(figures[5].getXpix() + 45, figures[5].getYpix()));
            g.DrawLine(new Pen(Brushes.White), new Point(figures[5].getXpix() + 45, figures[5].getYpix()),new Point(figures[39].getXpix()+60, figures[39].getYpix()+25));
            g.DrawLine(new Pen(Brushes.White), new Point(figures[39].getXpix() + 60, figures[39].getYpix() + 25), new Point(figures[69].getXpix() + 46, figures[69].getYpix()+56));
            g.DrawLine(new Pen(Brushes.White), new Point(figures[69].getXpix() + 46, figures[69].getYpix() + 56),new Point(figures[64].getXpix()+12, figures[64].getYpix()+56));
            g.DrawLine(new Pen(Brushes.White), new Point(figures[64].getXpix()+12, figures[64].getYpix() + 56),new Point(figures[30].getXpix()-5, figures[30].getYpix()+31));
            g.DrawLine(new Pen(Brushes.White), new Point(figures[30].getXpix() - 6, figures[30].getYpix() + 31), new Point(figures[0].getXpix() + 11, figures[0].getYpix()));
        }
        //-----------------------------------------------------------
        //---------------------------------------------------------
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            paintField(convertMas(fields));
        }
        //-----------------------------------------------------------
        //-----------------------------------------------------------
        private void Form1_Resize(object sender, EventArgs e)
        {
            paintField(convertMas(fields));
        }
        //---------------------------------------------------------
        //---------------------------------------------------------
        private static char[,] convertMas(char[,] inputMatr)
        {
            char[,] matr = new char[9, 10];
  
            //------FIRST LINE---------------------------------------
            matr[0, 0] = matr[0, 1] = matr[0, 8] = matr[0, 9] = 'e';
            for (int i = 0; i < 6; i++)
                matr[0, i + 2] = inputMatr[0, i];
            //------SECOND LINE--------------------------------------
                matr[1, 0] = matr[1, 8] = matr[1, 9] = 'e';
                for (int i = 0; i < 7; i++)
                    matr[1, i + 1] = inputMatr[1, i];
            //------THIRD LINE---------------------------------------
                matr[2, 0] = matr[2, 9] = 'e';
                for (int i = 0; i < 8; i++)
                    matr[2, i + 1] = inputMatr[2, i];
            //------FOURTH LINE--------------------------------------
                matr[3, 9]  = 'e';
                for (int i = 0; i < 9; i++)
                    matr[3, i ] = inputMatr[3, i];
            //------FIFTH LINE--------------------------------------
                for (int i = 0; i < 10; i++)
                    matr[4, i] = inputMatr[4, i];
            //------------------------------------------------------
            //------SIXTH LINE--------------------------------------
                matr[5, 9] = 'e';
                for (int i = 0; i < 9; i++)
                    matr[5, i] = inputMatr[5, i+1];
            //------SEVENTH LINE------------------------------------
            matr[6, 0] = matr[6, 9] = 'e';
                for (int i = 1; i <= 8; i++)
                    matr[6, i ] = inputMatr[6, i+1];
          //  matr[6,8]='.';
            //------EIGHTH LINE------------------------------------
                matr[7, 0]= matr[7,8] = matr[7, 9] = 'e';
                for (int i = 1; i < 8; i++)
                    matr[7, i] = inputMatr[7, i + 2];
            //------NINETH LINE-----------------------------------
                matr[8, 0] = matr[8, 1] = matr[8, 8] = matr[8, 9] = 'e';
                for (int i = 2; i < 8; i++)
                {
                    matr[8, i] = inputMatr[8, i + 2];
                }
            //-----------------------------------------------------------
            return matr;
        }
        //---------------------------------------------------------------
        //--------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            back = colors();
            paintField(convertMas(fields));
        }
        //---------------------------------------------------------------
        //---------------------------------------------------------
        private Color colors ()
         {
             if (colorDialog1.ShowDialog() == DialogResult.OK)
                return colorDialog1.Color;
             return Color.Black;          
         }
        //---------------------------------------------------------
         //---------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
         {
             circle = colors();
             paintField(convertMas(fields));
         }
        //---------------------------------------------------------
         //---------------------------------------------------------
        private void pictureBox1_Resize(object sender, EventArgs e)
         {
             g = pictureBox1.CreateGraphics();
         }
        private void button1_Click_1(object sender, EventArgs e)
        {
            paintField(convertMas(fields));
        }
        //---------------------------------------------------------
        //---------------------------------------------------------
        public static void repaintFigures()
        {
            char[,] field =  convertMas(fields);
            byte index = 0;
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                {

                    if (field[i, j] == 'e' || field[i, j] == '0' || field[i, j] == '1' || field[i, j] == '2' || field[i, j] == '3' ||
                        field[i, j] == '4' || field[i, j] == '5' || field[i, j] == '6' || field[i, j] == '7' || field[i, j] == '8' ||
                        field[i, j] == '9' || field[i, j] == 'n')
                    { }else
                    {
                        if (i == 8 && j == 9)
                        {
                            MessageBox.Show(field[i, j].ToString());
                        }
                        figures[index].setType(field[i, j],i,j);
                        index++;
                    }
                }
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res;
            RegistryKey openKey = Registry.CurrentUser.OpenSubKey("Software\\Placdarm", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (DialogResult.Cancel==(res = MessageBox.Show("Сохранить игру перед выходом?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)))
            {

            }
            else
            {
                
                if (res == DialogResult.Yes)
                {
                    if (saveGame("saves\\save.txt"))
                    {
                        MessageBox.Show("Запись успешно произведена");

                        openKey.SetValue("error", false);
                        openKey.Close();
                        this.Close();
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        openKey.SetValue("error", false);
                        openKey.Close();
                    }
                }
                try
                {
                    openKey.SetValue("error", false);
                    openKey.Close();
                    this.Close();
                    this.Dispose();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void начатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
           RegistryKey  openKey = Registry.CurrentUser.OpenSubKey("Software\\Placdarm", RegistryKeyPermissionCheck.ReadWriteSubTree);
           openKey.SetValue("error", true);
           openKey.Close();
           for (int i = 0; i < fields.GetLength(0); i++)
               for (int j = 0; j < fields.GetLength(1); j++)
                   fields[i,j]=newGame[i,j];
           paintField(convertMas(fields));
           repaintFigures();
           timer1.Enabled = true;
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private bool saveGame(string fileToSave)
        {
            byte counter = 1;
            StreamWriter writer = new StreamWriter(fileToSave);
            for (int i = 0; i < fields.GetLength(0); i++)
                for (int j = 0; j < fields.GetLength(1); j++)
                    if (counter!=90)
                    {
                        writer.Write(fields[i, j]+"\n");
                        counter++;
                    }
                    else writer.Write(fields[i, j]);
            writer.Close();
            StreamReader reader = new StreamReader("saves\\save.txt");
            counter = 0;
            while (reader.ReadLine()!=null)
            {
                counter++;
            }
            reader.Close();
            if (counter == 90)
                return true;
            return false;
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void loadGame(string fileToLoad)
        {
            StreamReader reader = new StreamReader(fileToLoad);
            for (int i = 0; i < fields.GetLength(0); i++)
                for (int j = 0; j < fields.GetLength(1); j++)
                    fields[i, j] = reader.ReadLine().ToCharArray()[0];
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void сохранитьТекущуюИгруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveGame("saves\\save.txt"))
            {
                MessageBox.Show("Запись успешно произведена");
            }
            else MessageBox.Show("Ошибка записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult res;
            RegistryKey openKey = Registry.CurrentUser.OpenSubKey("Software\\Placdarm", RegistryKeyPermissionCheck.ReadWriteSubTree);
            res = MessageBox.Show("Сохранить игру перед выходом?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
          

                if (res == DialogResult.Yes)
                {
                    if (saveGame("saves\\save.txt"))
                    {
                        MessageBox.Show("Запись успешно произведена");

                        openKey.SetValue("error", false);
                        openKey.Close();
                      
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        openKey.SetValue("error", false);
                        openKey.Close();
                    }
                }
                else
                {
                    openKey.SetValue("error", false);
                    openKey.Close();
                
                    this.Dispose();
                }
               
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            saveGame("saves\\tmp.txt");
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            loadGame("saves\\save.txt");
            paintField(convertMas(fields));
            repaintFigures();
        }

        private void натсройкаАвтосохраненияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void выгрузитьМассивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savemas = new SaveFileDialog();
            StreamWriter write;
            if (savemas.ShowDialog() == DialogResult.OK)
            {
                write = new StreamWriter(savemas.FileName);
                for (int i = 0; i < fields.GetLength(0); i++)
                {
                    for (int j = 0; j < fields.GetLength(1); j++)
                    {
                        write.Write(fields[i,j]);
                        write.Write(" ");
                    }
                    write.Write("\n");
                }
                write.Close();
            }
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------
    }
}
