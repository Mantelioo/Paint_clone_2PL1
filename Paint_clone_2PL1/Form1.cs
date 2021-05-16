using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint_clone_2PL1
{
    public partial class Form1 : Form
    {

        #region kintamieji

        //Kintamieji
        Color Color1 = Color.Black;
        Color Color2 = Color.Blue;
        Font Font1;
        Bitmap Picture;
        float PenWidth = 1.0f;
        Pen Pen1;
        Graphics Gr;
        Pen Eraser = new Pen(Color.White, 1);
        string SavedFilePath = "";

        //Kuriant draw funkcija PictureBox
        Point CurrentPoint = new Point(-1, -1); //Linija bus uz PictureBox ribu.

        bool FirstDraw = false;

        //Piesiant kvadrata
        Point StartPoint = new Point(-1, -1);
        Point EndPoint = new Point(-1, -1);

        //Staciakampio gradientas
        LinearGradientMode Mode;

        //Trikampis
        Point[] TrianglePoints = new Point[3];
        int MouseClicks = 0;

        //Arc braizymas
        int a;
        int b;

        //MultiAngle
        Point First = new Point(-1, -1);
        Point Second = new Point(-1, -1);

        #endregion

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        //Inicializacijos funkcija. KAs turetu atsitikti pakraunant programa?
        void Init()
        {
            //define new bitmap. Po sitos eilutes atmintyje bus sukurta tam tikras kiekis atminties picture box elementui sukurti (Paveiksleliui).
            Picture = new Bitmap(MainPicture.Width, MainPicture.Height);
            Gr = Graphics.FromImage(Picture);

            //Define Backgroud. Isvalome paveiksleli, nurodome spalva kuria mes uzpaisysime fona.
            Gr.Clear(Color.White);

            //Place image in Picture Box.
            MainPicture.Image = Picture;

            //Create pen
            Pen1 = new Pen(Color1, PenWidth);
        }

        private void SetPen()
        {
            //Funkcija pen reiksmems nustatyti.
            Pen1.Width = PenWidth;
            Pen1.Color = Color1;
            Eraser.Width = PenWidth;
        }

        private void DisplayImage()
        {
            Gr = Graphics.FromImage(Picture);
            MainPicture.Image = Picture;

        }

        private void MainPicture_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            //Padarome mygtuka spalvai pasirinkti
            if (ColorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color1 = ColorDialog1.Color;
                ColorButton.BackColor = Color1;
                SetPen();
            }
        }

        private void Color2Button_Click(object sender, EventArgs e)
        {
            //Padarome antra mygtuka spalvai pasirinkti
            if (ColorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color2 = ColorDialog1.Color;
                Color2Button.BackColor = Color2;
            }
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            //Padarome antra mygtuka spalvai pasirinkti
            if (FontDialog1.ShowDialog() == DialogResult.OK)
            {
                Font1 = FontDialog1.Font;
                FontButton.Text = Font.Name;
            }
        }

        private void P1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //Keiciame piestuko stori
            PenWidth = 1f;
            SetPen();
        }

        private void P2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            PenWidth = 30f;
            SetPen();
        }

        private void P3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            PenWidth = 90f;
            SetPen();
        }

        //Paisymo funkcija

        private void MainPicture_Paint(object sender, PaintEventArgs e)
        {
            Gr = this.CreateGraphics();
            Gr = Graphics.FromImage(Picture);
        }

        private void MainPicture_MouseMove(object sender, MouseEventArgs e)
        {
            //tikriname koks peles mygtukas turejo buti paspaustas. darysime kazka tik paspaude kairi mygtuka.
            if (e.Button == MouseButtons.Left)
            {
                if (LineRadioButton.Checked)
                {
                    Gr.DrawLine(Pen1, e.Location, StartPoint);
                    StartPoint = e.Location;
                }

                else if (EraserRadioButton.Checked)
                {
                    //Nustatome trintuko plota
                    Eraser.Width = Pen1.Width;
                    Gr.DrawRectangle(Eraser, e.Location.X, e.Location.Y, PenWidth, PenWidth);
                }

                //Atvaizduojame vaizda.
                DisplayImage();
                FirstDraw = true;
            }
        }

        private void MainPicture_MouseDown(object sender, MouseEventArgs e)
        {
            StartPoint = e.Location;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(SavedFilePath))
            {
                SaveFileDialog1.Filter = "JPG file|*.jpg|PNG file|*.png|BMP file|*.bmp";

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SavedFilePath = SaveFileDialog1.FileName;

                    //save picture

                    Picture.Save(SavedFilePath);
                    this.Text = SavedFilePath;
                }
            }
            else
            {
                Picture.Save(SavedFilePath);
            }
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog1.Filter = "JPG file|*.jpg|PNG file|*.png|BMP file|*.bmp";
            SavedFilePath = "";

            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SavedFilePath = SaveFileDialog1.FileName;

                //save picture

                Picture.Save(SavedFilePath);
                this.Text = SavedFilePath;
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FirstDraw)
            {
                DialogResult result = MessageBox.Show("Ar norite issaugoti darba?", "Issaugoti?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.No)
                {
                    SavedFilePath = "";
                    FirstDraw = false;
                    this.Text = "Untitled";
                    Gr.Clear(Color.White);
                    DisplayImage();
                }

                if (result == DialogResult.Yes)
                {
                    SaveToolStripMenuItem.PerformClick();

                    SavedFilePath = "";
                    FirstDraw = false;
                    this.Text = "Untitled";
                    Gr.Clear(Color.White);
                    DisplayImage();
                }
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gr.Clear(Color.White);
            DisplayImage();
            FirstDraw = false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FirstDraw)
            {
                DialogResult result = MessageBox.Show("Ar norite issaugoti darba?", "Issaugoti?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.No)
                {
                    //this.Close();
                    Application.Exit();
                }

                if (result == DialogResult.Yes)
                {
                    SaveToolStripMenuItem.PerformClick();
                    Application.Exit();
                }
                this.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FirstDraw)
            {
                SaveToolStripMenuItem.PerformClick();
            }

            var ofd = new OpenFileDialog();
            ofd.Filter = "JPG file|*.jpg|PNG file|*.png|BMP file|*.bmp";
            ofd.Title = "SelectImage";

            ofd.CheckFileExists = false;
            ofd.CheckPathExists = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Picture = new Bitmap(ofd.FileName);
                // SavedFilePath = ofd.FileName;
                DisplayImage();
                //this.Text = ofd.FileName;
            }
            SavedFilePath = "";

        }

        private void MainPicture_MouseUp(object sender, MouseEventArgs e)
        {
            //ivykis kuris suveikia atleidus peles mygtuka. Kuriame rectangle pasiymo funkcija
            if (e.Button == MouseButtons.Left)
            {
                if (RectangleRadioButton.Checked)
                {
                    Point temp = new Point();
                    EndPoint = e.Location;

                    temp.X = EndPoint.X - StartPoint.X;
                    temp.Y = EndPoint.Y - StartPoint.Y;

                    if (temp.X < 0)
                    {
                        StartPoint.X = EndPoint.X;
                        temp.X = Math.Abs(temp.X);
                    }

                    if (temp.Y < 0)
                    {
                        StartPoint.Y = EndPoint.Y;
                        temp.Y = Math.Abs(temp.Y);
                    }
                    if (temp.X == 0 || temp.Y == 0)
                    {
                        return;
                    }
                    Gr.DrawRectangle(Pen1, StartPoint.X, StartPoint.Y, temp.X, temp.Y);
                }
                else if (GradientRectAngleRadioButton.Checked)
                {
                    Point temp = new Point();
                    EndPoint = e.Location;

                    temp.X = EndPoint.X - StartPoint.X;
                    temp.Y = EndPoint.Y - StartPoint.Y;

                    if (temp.X < 0)
                    {
                        StartPoint.X = EndPoint.X;
                        temp.X = Math.Abs(temp.X);
                    }

                    if (temp.Y < 0)
                    {
                        StartPoint.Y = EndPoint.Y;
                        temp.Y = Math.Abs(temp.Y);
                    }
                    if (temp.X == 0 || temp.Y == 0)
                    {
                        return;
                    }
                    Brush MyBrush = new LinearGradientBrush(new Rectangle(StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y),
                        Color1, Color2, Mode
                        );
                    Gr.FillRectangle(MyBrush, StartPoint.X, StartPoint.Y, temp.X, temp.Y);
                }
                else if (TriangleRadioButton.Checked)
                {
                    //Kuriame trikampio paisymo funkcija.
                    TrianglePoints[MouseClicks] = e.Location;
                    MouseClicks++;

                    //tarp dviju tasku braizo linija.
                    if (MouseClicks == 2)
                    {
                        Gr.DrawLine(Pen1, TrianglePoints[0], TrianglePoints[1]);
                    }

                    //Jei paspaudem 3 kartus reikia nubarizyti trikampi
                    if (MouseClicks > 2)
                    {
                        //braizome
                        MouseClicks = 0;
                        Gr.DrawLine(Pen1, TrianglePoints[1], TrianglePoints[2]);
                        Gr.DrawLine(Pen1, TrianglePoints[0], TrianglePoints[2]);
                    }
                }
                else if (CircleRadioButton.Checked)
                {

                    //Kuriame apskritimo braizymo funkcija. C# nubraizys kvadrata ir jo viduje nubres apskritima. Tad reikia nukopijuoti Rectangle braizyma.

                    Point temp = new Point();
                    EndPoint = e.Location;

                    temp.X = EndPoint.X - StartPoint.X;
                    temp.Y = EndPoint.Y - StartPoint.Y;

                    if (temp.X < 0)
                    {
                        StartPoint.X = EndPoint.X;
                        temp.X = Math.Abs(temp.X);
                    }

                    if (temp.Y < 0)
                    {
                        StartPoint.Y = EndPoint.Y;
                        temp.Y = Math.Abs(temp.Y);
                    }

                    if (temp.X < temp.Y)
                    {
                        //jeigu x mazesnis, y turi buti toks pat kaip x
                        temp.Y = temp.X;
                    }

                    //Turime sulyginti koordinates kad gautusi grazus apskritimas
                    if (temp.X < temp.Y)
                    {
                        temp.X = temp.Y;
                    }
                    if (temp.X == 0 || temp.Y == 0)
                    {
                        return;
                    }
                    Gr.DrawArc(Pen1, StartPoint.X, StartPoint.Y, temp.X, temp.Y, 0, 360);
                }

                else if (ArcRadioButton.Checked)
                {
                    Point temp = new Point();
                    EndPoint = e.Location;

                    temp.X = EndPoint.X - StartPoint.X;
                    temp.Y = EndPoint.Y - StartPoint.Y;

                    if (temp.X < 0)
                    {
                        StartPoint.X = EndPoint.X;
                        temp.X = Math.Abs(temp.X);
                    }

                    if (temp.Y < 0)
                    {
                        StartPoint.Y = EndPoint.Y;
                        temp.Y = Math.Abs(temp.Y);
                    }

                    if (temp.X < temp.Y)
                    {
                        //jeigu x mazesnis, y turi buti toks pat kaip x
                        temp.Y = temp.X;
                    }

                    //Turime sulyginti koordinates kad gautusi grazus apskritimas

                    if (temp.X < temp.Y)
                    {
                        temp.X = temp.Y;
                    }
                    if (temp.X == 0 || temp.Y == 0)
                    {
                        return;
                    }

                    //Jeigu pazymeta varnele up, turime apkeisti DUOTI KAIP UZDUOTI NUSTATYTI ARC DYDZIUS

                    if (chkUpDown.Checked)
                    {
                        a = trcStart.Value;
                        b = -trcSweep.Value;
                    }
                    else
                    {
                        a = trcStart.Value;
                        b = trcSweep.Value;
                    }


                    Gr.DrawArc(Pen1, StartPoint.X, StartPoint.Y, temp.X, temp.Y, 520, 520);
                }

                else if (MultiAngleRadioButton.Checked)
                {

                    EndPoint = e.Location;
                    if (First.X == -1 && First.Y == -1)
                    {
                        First = EndPoint;
                    }
                    if (Second.X == -1 && Second.Y == -1)
                    {
                        Second = EndPoint;
                    }

                    Gr.DrawLine(Pen1, Second, EndPoint);
                    Second = EndPoint;
                    //taciau nesujungia pirmo tasko su paskutiniu

                }

                else if (TextRadioButton.Checked)
                {
                    //Paisome teksta
                    EndPoint = e.Location;
                    Gr.DrawString(TextDrawTextBox.Text, Font1, new SolidBrush(Color1), EndPoint);

                }

                DisplayImage();
            }

            //tikrinam ar buvo paspaustas kitas mygtukas. su juo sujungsime taskus
            else if (e.Button == MouseButtons.Right)
            {
                if (MultiAngleRadioButton.Checked)
                {
                    EndPoint = e.Location; // kur mes paspaudeme desine klavisa
                    Gr.DrawLine(Pen1, First, Second);
                    DisplayImage();
                    First = new Point(-1, -1);
                    Second = new Point(-1, -1); // Perkuriame is naujo taska
                }
            }
        }

        private void ArcRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            grbArc.Visible = ArcRadioButton.Checked;
        }

        private void TextRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //darome text box mygtuka
            TextDrawTextBox.Visible = TextRadioButton.Checked;
            FontButton.Visible = true;
        }
    }
}
