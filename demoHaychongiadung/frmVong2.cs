using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demoHaychongiadung
{
    public partial class frmVong2 : Form
    {
        private List<Product> products_list;
        private Player Winner1;
        public frmVong2(Player winner, List<Product> products_l)
        {
            InitializeComponent();
            Winner1 = winner;
            products_list = products_l;
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
