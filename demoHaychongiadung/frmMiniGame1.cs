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
    public partial class frmMiniGame1 : Form
    {
        private List<Product> products;
        private ProductPair currentPair;
        private Player Winner2;
        private int timeRemaining;
        private int successCount ;
        private int currentRound;
        private MinigameHighLow minigame1=new MinigameHighLow();
        public frmMiniGame1(Player winner2,List<Product> products_list)
        {
            InitializeComponent();
          
            Winner2 = winner2;
            products = products_list;
            currentRound = 1;
            successCount = 0;
            lblScore.Text = "0/3";
           
            picA.Click -= Picture_Click;
            picB.Click -= Picture_Click;
            picA.Click += Picture_Click;
            picB.Click += Picture_Click;
            panel2.BackColor=ColorTranslator.FromHtml("#a5a5ac");
            panel1.BackColor = ColorTranslator.FromHtml("#1ABC9C");
            pnlGuessPrice.BackColor = ColorTranslator.FromHtml("#1e1e2f");
        }
        private void LoadRound()
        {
            if (currentRound > 3)
            {
                EndGame();
                return;
            }
            
            timeRemaining = 10;
            lblTimer.Text = "⏱️ 10 s";
        
            LoadPairProduct();
            EnableButtons(true);
            Game1Timer.Start();

        }
        private void LoadPairProduct()
        {
            currentPair = minigame1.GetRandomProductPair(products);
            if (currentPair == null)
            {
                MessageBox.Show("Không còn sản phẩm nào để chơi! Vòng 2 kết thúc.", "Lỗi Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblProductA.Text = currentPair.A.Name;
            lblProductB.Text = currentPair.B.Name;
            LoadImage(picA, currentPair.A.ImagePath);
            LoadImage(picB, currentPair.B.ImagePath);
        }
        private void LoadImage(PictureBox picBox,string image_path)
        {
            string path = Path.Combine(Application.StartupPath, image_path);

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                picBox.Image = Image.FromFile(path);
            }
            else
            {
                picBox.Image = null;
                MessageBox.Show("Không tìm thấy ảnh: " + path);
            }
        }
        private void Picture_Click(object sender, EventArgs e)
        {
            Game1Timer.Stop(); 
            EnableButtons(false);
            
            PictureBox clickedPic = sender as PictureBox;
            if (clickedPic == null || currentPair == null) return;

            bool guessAHigher = (clickedPic == picA);

            bool isCorrect = minigame1.CheckPlayerGuess(currentPair, guessAHigher);

            if (isCorrect)
            {
                successCount++;
                MessageBox.Show("Bạn chọn chính xác!", "Kết Quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Bạn đoán sai rồi!" +
                    $"Giá của 2 sản phẩm:\n"+ 
                    $"{currentPair.A.Name}: {currentPair.A.Price:N0} VNĐ\n"+
                    $"{currentPair.B.Name}: {currentPair.B.Price:N0} VNĐ\n"
                    , "Kết Quả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            lblScore.Text = successCount.ToString() + "/3";
            currentRound++;
            LoadRound();      
        }

        private void EndGame()
        {
            Game1Timer.Stop();
            bool pass = (successCount >= 2);
            if (pass)
            {
                Winner2.Diem += 20;
                Winner2.CapNhatTrangThai("Thắng Minigame Cao Hay Thấp ở vòng 2");

                MessageBox.Show($" Số câu đúng: {successCount}/3 câu\n\n" +
                    $"🎉 CHÚC MỪNG {Winner2.Ten.ToUpper()} THẮNG VÒNG 2!\n" +
                    "$ Bạn được cộng 20 điểm.", "KẾT QUẢ VÒNG 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult dialogResult = MessageBox.Show($"Người thắng: {Winner2.Ten}. Tiếp tục sang Vòng 3?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    frmVong3 f3 = new frmVong3(Winner2, products);
                    f3.Show();
                    this.Close();
                }
            } 
            else
            {
                Winner2.CapNhatTrangThai("Bị loại sau vòng 2");
                MessageBox.Show($" Số câu đúng: {successCount}/3 câu\n\n" +
                    " RẤT TIẾC! Bạn đã thua Minigame CAO HAY THẤP. Game kết thúc!"
                    , "KẾT QUẢ VÒNG 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EnableButtons(false);
                LoseGame();
            }
        }
        private void EnableButtons(bool enable)
        {
            picA.Enabled = enable;
            picB.Enabled = enable;
        }    
        private void LoseGame()
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }

        private void btnStartGame1_Click_1(object sender, EventArgs e)
        {
           
            lblRuleGame1.Visible = false;
            btnStartGame1.Visible = false;
            
            LoadRound();
        }
        private void btnExitr1_Click(object sender, EventArgs e)
        {
            Game1Timer.Stop();
            DialogResult result = MessageBox.Show("Bạn có muốn thoát minigame?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                frmVong2 f = new frmVong2(Winner2, products);
                f.Show();
                this.Close();
            }
        }
       
        private void Game1Timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            TimeSpan t = TimeSpan.FromSeconds(timeRemaining);
            lblTimer.Text = "⏱️ " + string.Format("{0:D2} s", t.Seconds);
            if (timeRemaining <= 0)
            {
                Game1Timer.Stop();
                EnableButtons(false);
                MessageBox.Show("Đã hết thời gian!", "Hết Giờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblScore.Text = successCount.ToString() + "/3";
                currentRound++;
                LoadRound();
            }
        }

        
    }
}
