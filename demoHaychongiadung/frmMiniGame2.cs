using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demoHaychongiadung
{
    public partial class frmMiniGame2 : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
        private Player Winner2;
        private List<Product>  products_list; 
        private List<double> currentPrice;
        private MinigamePickPrice minigame2=new MinigamePickPrice();     
        private Product currentProduct;
        private int timeRemaining;
        private int successCount = 0; 
        private int currentRound = 1;
        public frmMiniGame2(Player winner2, List<Product> products)
        {
            InitializeComponent();
            Winner2= winner2;
            products_list = products;
            lblScore2.Text = "0/2";
            btnPrice1.Click += Button_Click;
            btnPrice2.Click += Button_Click;
            btnPrice3.Click += Button_Click;
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
            lblRuleGame2.Font = new Font(myfonts.Families[0], 18);
            lblTimer.Font = new Font(myfonts.Families[0], 14);
            lblProductName.Font = new Font(myfonts.Families[2], 12, FontStyle.Italic);
            lblProduct.Font = new Font(myfonts.Families[0], 12);
            panel2.BackColor = ColorTranslator.FromHtml("#a5a5ac");
            panel1.BackColor = ColorTranslator.FromHtml("#1ABC9C");
            pnlGuessPrice.BackColor = ColorTranslator.FromHtml("#1e1e2f");

        }

        private void LoadProduct()
        {
            currentProduct = minigame2.GetProductForMinigame(products_list);
            if (currentProduct == null)
            {
                MessageBox.Show("Không còn sản phẩm nào để chơi! Minigame kết thúc.", "Lỗi Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblProductName.Text = currentProduct.Name;

            string path = Path.Combine(Application.StartupPath, currentProduct.ImagePath);

            if (!string.IsNullOrEmpty(path))
            {
                picProduct.SizeMode = PictureBoxSizeMode.StretchImage;
                picProduct.Image = Image.FromFile(path);

            }
            else
            {
                picProduct.Image = null;
                MessageBox.Show("Không tìm thấy ảnh: " + path);
            }

        }
        private void LoadRound()
        {
            timeRemaining = 10;
            lblTimer.Text = "⏱️ 10 s";
            Game2Timer.Start();
            if (currentRound > 2)
            {
                EndGame();
                return;
            }
            LoadProduct();
            currentPrice = minigame2.GenerateAndShufflePrices(currentProduct.Price); 
           
            btnPrice1.Text = $"{currentPrice[0]:N0} VNĐ";
            btnPrice2.Text = $"{currentPrice[1]:N0} VNĐ";
            btnPrice3.Text = $"{currentPrice[2]:N0} VNĐ";
            EnableButtons(true);
        }
        private void Button_Click(object sender, EventArgs e)
        {
            EnableButtons(false);
            Button clickedButton = (Button)sender;
            double selectedPrice = 0;
            Game2Timer.Stop();
            if (clickedButton == btnPrice1) selectedPrice = currentPrice[0];
            else if (clickedButton == btnPrice2)  selectedPrice = currentPrice[1];
            else if (clickedButton == btnPrice3)  selectedPrice = currentPrice[2];

            if (selectedPrice == currentProduct.Price)
            {
                successCount++;
                MessageBox.Show("Bạn chọn chính xác!","Kết Quả", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"SAI! Giá thật: {currentProduct.Price:N0} VNĐ", "Kết Quả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          
            }

            lblScore2.Text = successCount.ToString() + "/2";
            currentRound++;
            LoadRound(); 
        }
        private void EndGame()
        {
            Game2Timer.Stop();
            bool pass = (successCount == 2);
            if (pass)
            {
                Winner2.Diem += 20; 
                Winner2.CapNhatTrangThai("Thắng Minigame Giá Nào Đúng ở vòng 2");
           
                MessageBox.Show($" Số câu đúng: {successCount}/2 câu\n\n"+
                    $"🎉 CHÚC MỪNG {Winner2.Ten.ToUpper()} THẮNG VÒNG 2!\n" +
                    "$ Bạn được cộng 20 điểm.", "KẾT QUẢ VÒNG 2", MessageBoxButtons.OK, MessageBoxIcon.Information );
                DialogResult dialogResult = MessageBox.Show($"Người thắng: {Winner2.Ten}. Tiếp tục sang Vòng 3?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    frmVong3 f3 = new frmVong3(Winner2, products_list);
                    f3.Show();
                    this.Close();
                }
            }
            else
            {
                Winner2.CapNhatTrangThai("Bị loại sau vòng 2");              
                MessageBox.Show($" Số câu đúng: {successCount}/2 câu\n\n" + 
                    " RẤT TIẾC! Bạn đã thua Minigame GIÁ NÀO ĐÚNG. Game kết thúc!"
                    ,"KẾT QUẢ VÒNG 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                LoseGame();
            }
        }

        private void EnableButtons(bool enable)
        {
            btnPrice1.Enabled = enable;
            btnPrice2.Enabled = enable;
            btnPrice3.Enabled = enable;
        }
        private void LoseGame()
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }
        private void btnExitGame2_Click(object sender, EventArgs e)
        {
            Game2Timer.Stop();
            DialogResult result = MessageBox.Show("Bạn có muốn thoát minigame?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                frmVong2 f = new frmVong2(Winner2, products_list);
                f.Show();
                this.Close();
            }
        }
        private void Game2Timer_Tick_1(object sender, EventArgs e)
        {

            timeRemaining--;
            TimeSpan t = TimeSpan.FromSeconds(timeRemaining);
            lblTimer.Text = "⏱️ " + string.Format("{0:D2} s", t.Seconds);

            if (timeRemaining <= 0)
            {
                Game2Timer.Stop();
                EnableButtons(false);
                MessageBox.Show("Đã hết thời gian!", "Hết Giờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblScore2.Text = successCount.ToString() + "/2"; 
                currentRound++;
                LoadRound();
            }
        }

        private void btnStartGame2_Click_1(object sender, EventArgs e)
        {
            lblRuleGame2.Visible = false;
            btnStartGame2.Visible = false;
            LoadRound();

        }

       
    }
}
