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
using System.Xml.Linq;

namespace demoHaychongiadung
{
    public partial class frmLogin : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
        Game game = new Game();
        private List<Player> Players { get; set; }
        
        public frmLogin()
        {
            InitializeComponent();
            txtPlayer1.LostFocus += TxtPlayer_LostFocus;
            txtPlayer2.LostFocus += TxtPlayer_LostFocus;
            txtPlayer3.LostFocus += TxtPlayer_LostFocus;
            txtPlayer4.LostFocus += TxtPlayer_LostFocus;
            string fontPath1 = Path.Combine(Application.StartupPath, "Resources/Fonts/000 CCBattleCry [TeddyBear].ttf");
            string fontPath2 = Path.Combine(Application.StartupPath, "Resources/Fonts/CCElephantmenTall.otf");
            string fontPath3 = Path.Combine(Application.StartupPath, "Resources/Fonts/ProtestRiot-Regular.ttf");
            string fontPath4 = Path.Combine(Application.StartupPath, "Resources/Fonts/UTM Alter Gothic.ttf");
            myfonts.AddFontFile(fontPath1);
            myfonts.AddFontFile(fontPath2);
            myfonts.AddFontFile(fontPath3);
            myfonts.AddFontFile(fontPath4);
            label1.Font = new Font(myfonts.Families[1], 36);
            btnLogin.Font= new Font(myfonts.Families[1], 12);
            btnExit.Font = new Font(myfonts.Families[1], 12);
            lblPlayer1.Font= new Font(myfonts.Families[2], 12);
            lblPlayer2.Font = new Font(myfonts.Families[2], 12);
            lblPLayer3.Font = new Font(myfonts.Families[2], 12);
            lblPlayer4.Font = new Font(myfonts.Families[2], 12);
            label1.BackColor = ColorTranslator.FromHtml("#e91e63"); 
            this.BackColor = ColorTranslator.FromHtml("#1e1e2f");
            btnLogin.BackColor = ColorTranslator.FromHtml("#4CAF50");
            btnExit.BackColor = ColorTranslator.FromHtml("#E57373");
        }
        
        private void TxtPlayer_LostFocus(object sender, EventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            string currentName = currentTextBox.Text.Trim();
            if (string.IsNullOrEmpty(currentName))
                return;
            TextBox[] playerBoxes = { txtPlayer1, txtPlayer2, txtPlayer3, txtPlayer4 };

            foreach (TextBox otherTextBox in playerBoxes)
            {
                if (otherTextBox == currentTextBox)
                    continue;

                string otherName = otherTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(otherName) &&
                    currentName.Equals(otherName, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Tên '{currentName}' đã bị trùng với tên người chơi khác. Vui lòng nhập tên khác.",
                                    "Lỗi Trùng Tên",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                  
                    currentTextBox.Clear();
                    currentTextBox.Focus();
                    return; 
                }
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            TextBox[] playerBoxes = { txtPlayer1, txtPlayer2, txtPlayer3, txtPlayer4 };
            Game game=new Game();
            foreach (TextBox txtBox in playerBoxes)
            {
                string ten = txtBox.Text.Trim();
                try
                {
                    game.AddPlayer(ten);
                   
                }
                catch (ArgumentException ex) 
                {
                    MessageBox.Show(ex.Message, "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBox.Focus();
                    return;
                }
                catch (DuplicateNameException ex) 
                {
                    MessageBox.Show(ex.Message, "Lỗi Trùng Tên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBox.Focus();
                    return;
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Đã xảy ra lỗi không xác định: {ex.Message}", "Lỗi Hệ Thống");
                    return;
                }

            }
            
            this.Players = game.GetPlayers();
            MessageBox.Show("Đăng nhập thành công! Sẵn sàng bắt đầu trò chơi.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            frmVong1 f = new frmVong1(game);
            f.Show();
            this.Close();
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

       
    }
}
