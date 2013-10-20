using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace placdarm
{
    class Fishka : PictureBox
    {
        private char type;
        private byte x;
        private byte y;
        private int xpix;
        private int ypix;
        private byte number;
        //-----------------------------------------------------------
        public Fishka(char _type, byte _x, byte _y, int _xpix, int _ypix ,byte _number, PictureBox p)
        {  
            this.type = _type;
            this.x = _y;
            this.y = _x;
            this.xpix = _xpix;
            this.ypix = _ypix;
            this.number = _number;
            this.Location = new Point(_xpix+11,_ypix+11);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Name = "pic" + number; ;
            this.Size = new System.Drawing.Size(38,38);
            if (number != 3 && number != 4 && number != 10 && number != 61 && number != 67 && number != 68)
            {
                this.BackColor = Color.White;
            }
            else this.BackColor = Color.Aqua;
          
                this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.BackgroundImage = Image.FromFile("images\\" + type + ".png");
                  
            p.Controls.Add(this);          
        }
        //------------------------------------------------------------
        public void setType (char _type,int i, int j)
        {
            string file=_type.ToString();
            if (_type == 'W' || _type == 'B' || _type == 'A' || _type == 'C' || _type == 'Q' || _type == 'Z')
            {
                file += _type;
            }
            this.type = _type;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            Bitmap MyImage = new Bitmap("images\\"+file+".png");
            this.BackgroundImage = MyImage;
            
        }
        //------------------------------------------------------------
        protected override void OnClick(EventArgs e)
        {
            string message = Hod.Click(Form1.fields, this.y, this.x);
            if (!message.Equals("0"))
            {
                MessageBox.Show(message);
            }
            
            
         Form1.repaintFigures();
        } 
        //------------------------------------------------------------
        public void updateXYpix(int _xpix, int _ypix)
        {
            this.xpix = _xpix;
            this.ypix = _ypix;
            this.Location = new Point(_xpix + 11, _ypix + 11);
        }
        //----------------------------------------------------------------
        public int getXpix()
        {
            return this.xpix;
        }
        //----------------------------------------------------------------
        public int getYpix()
        {
            return this.ypix;
        }
        //----------------------------------------------------------------
      
    }
}
