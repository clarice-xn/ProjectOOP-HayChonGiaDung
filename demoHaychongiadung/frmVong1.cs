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
    public partial class frmVong1 : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
        private List<Player> players_list;
        private List<Product> products_list;
        private Product currentProduct;
        private Game game;
        private Round1 round1= new Round1();
        private int timeRemaining ;
        public frmVong1(Game currentGame)
        {
            game= currentGame;
            Round.ResetUsedList();
            game.InitializeSampleProducts();
            products_list = game.GetProducts();
            players_list  = game.GetPlayers();
            InitializeComponent();
            ShowPlayerName();
            LoadProduct();
            timeRemaining = 60; 
            lblTimer.Text = "⏱️ 01:00";
            Round1Timer.Start();
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
            lblGuessPrice.Font= new Font(myfonts.Families[1], 24);
            lblProduct.Font= new Font(myfonts.Families[2], 12,FontStyle.Italic);
            lblGuess.Font= new Font(myfonts.Families[2], 12);
            lblTimer.Font= new Font(myfonts.Families[0], 12);
            lblPlayer1.Font = new Font(myfonts.Families[3], 12);
            lblPlayer2.Font = new Font(myfonts.Families[3], 12);
            lblPlayer3.Font = new Font(myfonts.Families[3], 12);
            lblPlayer4.Font = new Font(myfonts.Families[3], 12);
            lblName.Font= new Font(myfonts.Families[3], 8);
            lblScore.Font= new Font(myfonts.Families[3], 8);
            lblRule1.Font= new Font(myfonts.Families[4], 10);
            this.BackColor = ColorTranslator.FromHtml("#1e1e2f");
            pnlGuessPrice.BackColor = ColorTranslator.FromHtml("#1e1e2f");
            panel1.BackColor= ColorTranslator.FromHtml("#62626d");
            panel2.BackColor = ColorTranslator.FromHtml("#E91E63");
            pnlGuessPrice.ForeColor = ColorTranslator.FromHtml("#ffffff");
            btnConfirm.BackColor = ColorTranslator.FromHtml("#4CAF50");
            btnReset.BackColor = ColorTranslator.FromHtml("#FF9800");
            lblName.BackColor= ColorTranslator.FromHtml("#F8BBD0");
            lblScore.BackColor =ColorTranslator.FromHtml("#F8BBD0");
            
        }
        private void ShowPlayerName()
        {
            List<Label> playerNameLabels = new List<Label> { lblPlayername1, lblPlayername2, lblPlayername3, lblPlayername4 };
           
            if (players_list != null && players_list.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    playerNameLabels[i].Text = players_list[i].Ten;    
                }
            }
            else
            {
                MessageBox.Show("Lỗi: Không tìm thấy đủ 4 người chơi để bắt đầu.", "Lỗi Dữ Liệu");
            }
            UpdateScores();

        }
        private void UpdateScores()
        {
            List<Label>scoreLabels = new List<Label> { lblScore1, lblScore2, lblScore3, lblScore4 };

            if (players_list != null && players_list.Count == 4)
            {

                for (int i = 0; i < players_list.Count; i++)
                {
                    scoreLabels[i].Text = players_list[i].Diem.ToString();
                }
            }
        }

        
        private void LoadProduct()
        {
            currentProduct = round1.GetRandomProduct(products_list);
            if (currentProduct == null)
            {
                MessageBox.Show("Không còn sản phẩm nào để chơi! Vòng 1 kết thúc.", "Lỗi Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            
            lblProduct.Text = currentProduct.Name;

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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            double[] guesses = new double[4];
            List <TextBox>guessBoxes = new List<TextBox> { txtGuess1, txtGuess2, txtGuess3, txtGuess4 };
            List<double> usedGuesses = new List<double>();
            bool allGuessed = true;

            for (int i = 0; i < players_list.Count; i++)
            {
                TextBox currentTextBox = guessBoxes[i];
                Player currentPlayer = players_list[i];
                try
                {
                    if (string.IsNullOrWhiteSpace(guessBoxes[i].Text))
                    {
                        allGuessed = false;
                        continue;
                    }

                    double guess;
                    if (!double.TryParse(guessBoxes[i].Text, out guess))
                        throw new FormatException();
                    if (guess <= 0)
                        throw new NegativePriceException();
                    if (guess > 100000000)
                        throw new TooHighException(); 
                    foreach (double usedPrice in usedGuesses)
                        if (Math.Abs(guess - usedPrice) < 1)
                            throw new DuplicatePriceException();

                    guesses[i] = guess;
                    usedGuesses.Add(guess);
                    
                }
                catch (FormatException)
                {
                    MessageBox.Show($"Người chơi {i + 1} {players_list[i].Ten} Vui lòng nhập giá bằng số!","Lỗi Nhập Giá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    currentTextBox.Focus();
                    return;
                }
                catch (NegativePriceException ex)
                {
                    MessageBox.Show($"Người chơi {players_list[i].Ten}: {ex.Message}", "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    currentTextBox.Focus(); 
                    return;
                }
                catch (TooHighException ex)
                {
                    MessageBox.Show($"Người chơi {players_list[i].Ten}: {ex.Message}", "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    currentTextBox.Focus();
                    return;
                }
                catch (DuplicatePriceException ex)
                {
                    MessageBox.Show($"Người chơi {players_list[i].Ten}: {ex.Message}", "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    currentTextBox.Focus();
                    return;
                }
            
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (!allGuessed)
            {
                MessageBox.Show("Vui lòng nhập đủ giá trước khi Xác nhận.", "Chưa đủ dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Round1Timer.Stop();
            ProcessRoundResults();
        }
        private void ProcessRoundResults()
        {
            List<TextBox> guessBoxes = new List<TextBox> { txtGuess1, txtGuess2, txtGuess3, txtGuess4 };
            double[] guesses = new double[4];
            List<double> usedGuesses = new List<double>();

            for (int i = 0; i < players_list.Count; i++)
            {
                TextBox currentTextBox = guessBoxes[i];
                Player currentPlayer = players_list[i];
                double guess;
                if (double.TryParse(currentTextBox.Text, out guess))
                {
                    if (guess > 0 && guess <= 100000000)
                    {
                        guesses[i] = guess;
                        currentPlayer.DuDoanGia(guess); 
                        continue;
                    }
                }
                guesses[i] = 0;
            }

            double realPrice = currentProduct.Price;
            double closestPrice = 0;
            Player winner = null;

            for (int i = 0; i < players_list.Count; i++)
            {
                double guessPrice = guesses[i];
                Player currentPlayer = players_list[i];

                if (guessPrice <= realPrice)
                {
                    if (guessPrice > closestPrice)
                    {
                        closestPrice = guessPrice;
                        winner = currentPlayer;
                    }
                }
            }

            ShowResult(winner, closestPrice, realPrice);

            btnConfirm.Enabled = false;
            btnReset.Enabled = false;
        }
        private void ShowResult(Player winner, double closestPrice, double realPrice)
        {

            if (winner != null)
            {
                winner.Diem += 10;
                winner.CapNhatTrangThai($"Thắng Vòng 1 (Giá đoán: {closestPrice:N0} VNĐ)");
                UpdateScores();

                foreach (Player p in players_list)
                {
                    if (p != winner)
                    {
                        p.CapNhatTrangThai("Bị loại sau vòng 1");
                    }
                }
               

                MessageBox.Show($"Giá thật: {realPrice:N0} VNĐ\n\n"+
                    $"🎉 CHÚC MỪNG {winner.Ten.ToUpper()} THẮNG VÒNG 1!\n" +
                          $"Giá đoán gần nhất: {closestPrice:N0} VNĐ.\n" +
                          $"Bạn được cộng 10 điểm.", "KẾT QUẢ VÒNG 1", 
                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult dialogResult = MessageBox.Show($"Người thắng: {winner.Ten}. Tiếp tục sang Vòng 2?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    frmVong2 f2 = new frmVong2(winner, products_list);
                    f2.Show();
                    this.Close();
                }
            }
            else
            {
                foreach (Player p in players_list)
                {
                    p.CapNhatTrangThai("Bị loại sau Vòng 1 ");
                }
                MessageBox.Show($"Giá thật: {realPrice:N0} VNĐ\n\n" + "RẤT TIẾC! Không ai đoán hợp lệ.Game kết thúc!", "KẾT QUẢ VÒNG 1", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoseGame();
            }
        }
        private void LoseGame()
        {
            frmLogin f =new frmLogin();
            f.Show();
            this.Close();
        }
        private void btnExitr1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn quay về màn hình chính?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                frmWelcome f = new frmWelcome();
                f.Show();
                this.Close();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtGuess1.Clear();
            txtGuess2.Clear();
            txtGuess3.Clear();
            txtGuess4.Clear();
        }

        private void Round1Timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--; 
            TimeSpan t = TimeSpan.FromSeconds(timeRemaining);
            lblTimer.Text ="⏱️ "+ string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

            if (timeRemaining <= 0)
            {
                Round1Timer.Stop();
                btnConfirm.Enabled = false;
                btnReset.Enabled = false;

                MessageBox.Show("Đã hết thời gian!", "Hết Giờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProcessRoundResults();
            }
            if (timeRemaining <= 30)
            {
                lblTimer.ForeColor = Color.Red;
            }
        }

       
    }
}
