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

namespace demoHaychongiadung
{
    public partial class frmWelcome : Form
    {
        public frmWelcome()
        {
            InitializeComponent();
            
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
            
            Font f1 = new Font("000 Elephantmen TB", 180);
            Graphics g= this.CreateGraphics();
            SolidBrush bigBrush = new SolidBrush(Color.Gold);
            g.DrawString("?", f1, bigBrush, new Point(250,-60));
            
            Font f2 = new Font("000 Elephantmen TB", 36, FontStyle.Bold);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            g.DrawString("Hãy\nChọn\nGiá\nĐúng", f2, redBrush, new Point(390, 0));
            
        }

       
    }
}
