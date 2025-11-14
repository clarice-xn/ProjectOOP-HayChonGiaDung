using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Text;
using System.IO;

namespace demoHaychongiadung
{
    public partial class frmWelcome : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
       
        public frmWelcome()
        {
            InitializeComponent();
            string fontPath1 = Path.Combine(Application.StartupPath, "Resources/Fonts/000 CCBattleCry [TeddyBear].ttf");
            string fontPath2 = Path.Combine(Application.StartupPath, "Resources/Fonts/CCElephantmenTall.otf");
            string fontPath3 = Path.Combine(Application.StartupPath, "Resources/Fonts/ProtestRiot-Regular.ttf");
            string fontPath4 = Path.Combine(Application.StartupPath, "Resources/Fonts/UTM Alter Gothic.ttf");
            myfonts.AddFontFile(fontPath1);
            myfonts.AddFontFile(fontPath2);
            myfonts.AddFontFile(fontPath3);
            myfonts.AddFontFile(fontPath4);
            btnStart.Font=new Font(myfonts.Families[1], 12);
            btnExit.Font = new Font(myfonts.Families[1], 12);
            btnStart.BackColor = ColorTranslator.FromHtml("#4CAF50");
            btnExit.BackColor = ColorTranslator.FromHtml("#E57373");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Owner = this;
            this.Hide();
            f.Show();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmWelcome_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=System.Windows.Forms.DialogResult.Yes)
                e.Cancel = true;
        }

        private void frmWelcome_Paint(object sender, PaintEventArgs e)
        {
            
            Font f1 = new Font(myfonts.Families[1], 180);
            Graphics g= this.CreateGraphics();
            SolidBrush bigBrush = new SolidBrush(Color.Gold);
            g.DrawString("?", f1, bigBrush, new Point(250,-60));
            
            Font f2 = new Font(myfonts.Families[1], 36, FontStyle.Bold);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            g.DrawString("Hãy\nChọn\nGiá\nĐúng", f2, redBrush, new Point(390, 0));
            
        }

       
    }
}
