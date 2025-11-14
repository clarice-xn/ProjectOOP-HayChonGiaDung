using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demoHaychongiadung
{
    public partial class frmWinner : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
        Timer rainbowTimer = new Timer();   
        Timer fireworkTimer = new Timer();
        float hue = 0;
        List<Firework> fireworkList = new List<Firework>();
        Random rand = new Random();
        private SoundPlayer player = new SoundPlayer();
        public frmWinner()
        {
            InitializeComponent();
            string fontPath1 = Path.Combine(Application.StartupPath, "Resources/Fonts/000 CCBattleCry [TeddyBear].ttf");
            string fontPath2 = Path.Combine(Application.StartupPath, "Resources/Fonts/CCElephantmenTall.otf");
            string fontPath3 = Path.Combine(Application.StartupPath, "Resources/Fonts/ProtestRiot-Regular.ttf");
            string fontPath4 = Path.Combine(Application.StartupPath, "Resources/Fonts/UTM Alter Gothic.ttf");
            string fontPath5 = Path.Combine(Application.StartupPath, "Resources/Fonts/Freeman-Regular.ttf");
            myfonts.AddFontFile(fontPath1);
            myfonts.AddFontFile(fontPath2);
            myfonts.AddFontFile(fontPath3);
            myfonts.AddFontFile(fontPath4);
            myfonts.AddFontFile(fontPath5);
            lblWinner.Font = new Font(myfonts.Families[0], 24);
            this.DoubleBuffered = true;
        }

        private void frmWinner_Load(object sender, EventArgs e)
        {
            rainbowTimer.Interval = 10;
            rainbowTimer.Tick += RainbowTimer_Tick;
            rainbowTimer.Start();
           
            fireworkTimer.Interval = 50;
            fireworkTimer.Tick += FireworkTimer_Tick;
            fireworkTimer.Start();
            player.SoundLocation = "Sofia-the-First-theme-song-_-Lyrics_.wav";
            player.Load();
            player.PlayLooping();
        }
        
        private void FireworkTimer_Tick(object sender, EventArgs e)
        {
            if (rand.Next(0, 100) < 20)
            {
                int x = rand.Next(50, this.Width - 50);
                int y = rand.Next(50, this.Height / 2);
                fireworkList.Add(new Firework(x, y, rand));
            }

            for (int i = fireworkList.Count - 1; i >= 0; i--)
            {
                fireworkList[i].Update();
                if (fireworkList[i].IsDead())
                {
                    fireworkList.RemoveAt(i);
                }
            }

            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (Firework f in fireworkList)
            {
                f.Draw(g);
            }
        }
        private void RainbowTimer_Tick(object sender, EventArgs e)
        {
            hue += 2;
            if (hue >= 360) hue = 0;
            this.BackColor = FromHsv(hue, 0.3, 1);
        }

        public static Color FromHsv(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);
            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0: return Color.FromArgb(255, v, t, p);
                case 1: return Color.FromArgb(255, q, v, p);
                case 2: return Color.FromArgb(255, p, v, t);
                case 3: return Color.FromArgb(255, p, q, v);
                case 4: return Color.FromArgb(255, t, p, v);
                default: return Color.FromArgb(255, v, p, q);
            }
        }
        
        private class Firework
        {
            public List<Particle> Particles = new List<Particle>();
            private int age = 0;
            public Firework(int x, int y, Random rand)
            {
                double randomHue = rand.NextDouble() * 360;
               
                Color color = frmWinner.FromHsv(randomHue, 1.0, 1.0);
                for (int i = 0; i < 50; i++)
                {
                    double angle = rand.NextDouble() * Math.PI * 2;
                    double speed = rand.NextDouble() * 5 + 2;
                    Particles.Add(new Particle(x, y, angle, speed, color));
                }
            }
            public void Update()
            {
                age++;
                foreach (Particle p in Particles)
                {
                    p.Update();
                }
            }
            public bool IsDead()
            {
                return age > 60;
            }
            public void Draw(Graphics g)
            {
                foreach (Particle p in Particles)
                {
                    p.Draw(g);
                }
            }
        }
        private class Particle
        {
            public float X, Y;
            public float VelX, VelY;
            public Color Color;
            public float Alpha = 255;

            public Particle(int x, int y, double angle, double speed, Color color)
            {
                X = x;
                Y = y;
                VelX = (float)(Math.Cos(angle) * speed);
                VelY = (float)(Math.Sin(angle) * speed);
                Color = color;
            }

            public void Update()
            {
                X += VelX;
                Y += VelY;
                VelY += 0.15f; 
                Alpha -= 4; 
                if (Alpha < 0) Alpha = 0;
            }

            public void Draw(Graphics g)
            {
                Color c = Color.FromArgb((int)Alpha, Color);
                g.FillEllipse(new SolidBrush(c), X - 2, Y - 2, 4, 4);
            }
        }

        private void frmWinner_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
