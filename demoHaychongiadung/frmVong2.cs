using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demoHaychongiadung
{
    public partial class frmVong2 : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
        private List<Product> products_list;
        private Player Winner1;
        public frmVong2(Player winner, List<Product> products_l)
        {
            InitializeComponent();
            Winner1 = winner;
            products_list = products_l;
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
            lblMinigame.Font= new Font(myfonts.Families[1], 24);
            lblPickGame.Font = new Font(myfonts.Families[0], 24);
            lblName.Font = new Font(myfonts.Families[2], 8);
            lblScore.Font = new Font(myfonts.Families[2], 8);
            lblRules2.Font = new Font(myfonts.Families[4], 10);
            lblGame1.Font = new Font(myfonts.Families[1], 14);
            lblGame2.Font = new Font(myfonts.Families[1], 14);
            lblGame3.Font = new Font(myfonts.Families[1], 14);
            this.BackColor = ColorTranslator.FromHtml("#1e1e2f"); 
            panel3.BackColor = ColorTranslator.FromHtml("#62626d");
            panel2.BackColor = ColorTranslator.FromHtml("#E91E63");
            lblName.BackColor = ColorTranslator.FromHtml("#F8BBD0");
            lblScore.BackColor = ColorTranslator.FromHtml("#F8BBD0");
        }

        private void frmVong2_Load(object sender, EventArgs e)
        {
            lblWinner1.Text = Winner1.Ten;
            lblScorev1.Text = Winner1.Diem.ToString();  
        }

        private void picGame1_Click(object sender, EventArgs e)
        {
            frmMiniGame1 f1 = new frmMiniGame1(Winner1,products_list);
            this.Close();
            f1.ShowDialog();
        }

        private void picGame2_Click(object sender, EventArgs e)
        {
            frmMiniGame2 f2 = new frmMiniGame2(Winner1,products_list);
            this.Close();
            f2.ShowDialog();
        }

        private void picGame3_Click(object sender, EventArgs e)
        {
            frmMiniGame3 f3 = new frmMiniGame3(Winner1,products_list);
            this.Close();
            f3.ShowDialog();
        }
        private void btnExitR2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn quay về màn hình chính?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                frmWelcome f = new frmWelcome();
                f.Show();
                this.Close();
            }
        }

       
    }
}
