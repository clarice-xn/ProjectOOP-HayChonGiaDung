using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demoHaychongiadung
{
    public partial class frmVong3 : Form
    {
        private Player winner3;
        private List<Product> products;
        private List<Product> currentPack;
        private double packPrice;
        private double guessPrice;
        private RoundFinal round3 = new RoundFinal();
        private int timeRemaining;
        
        public frmVong3(Player Winner3, List<Product> product_list)
        {
            winner3 = Winner3;
            products = product_list;
            InitializeComponent();
            timeRemaining = 60;
            lblTimer.Text = "⏱️ 01:00";
            LoadProductPack();
            Round3Timer.Start();
            round3.WinnerDeclared += RoundFinal_WinnerDeclared;
            this.BackColor = ColorTranslator.FromHtml("#1e1e2f");
            pnlGuessPrice.BackColor= ColorTranslator.FromHtml("#1e1e2f");
            panel1.BackColor = ColorTranslator.FromHtml("#62626d");
            panel2.BackColor = ColorTranslator.FromHtml("#E91E63");
            lblName.BackColor = ColorTranslator.FromHtml("#F8BBD0");
            lblScore.BackColor = ColorTranslator.FromHtml("#F8BBD0");
            btnConfirm.BackColor = ColorTranslator.FromHtml("#4CAF50");
        }
        private void LoadProductPack()
        {
            currentPack = round3.GetRandomProductPack(products);
            if (currentPack == null)
            {
                MessageBox.Show("Không còn sản phẩm nào để chơi! Vòng 3 kết thúc.", "Lỗi Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            List<PictureBox> picBoxes = new List<PictureBox> { picProduct1, picProduct2, picProduct3 };
            List<Label> nameProducts = new List<Label> { lblProduct1, lblProduct2, lblProduct3 };
            for (int i = 0; i < currentPack.Count; i++)
            {
                Product currentProduct = currentPack[i];
                PictureBox picBox = picBoxes[i];
                nameProducts[i].Text = currentProduct.Name;

                string path = Path.Combine(Application.StartupPath, currentProduct.ImagePath);

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

            for (int i = 0; i < currentPack.Count; i++)
                packPrice += currentPack[i].Price;

            double hintLower = packPrice * 0.8;
            double hintUpper = packPrice * 1.2;

            lblHint.Text = $"Giá gói quà nằm trong khoảng:\n{hintLower:N0} VNĐ - {hintUpper:N0} VNĐ";

        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(txtGuess.Text))
                {
                    MessageBox.Show("Vui lòng nhập giá trước khi Xác nhận.", "Chưa đủ dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Round3Timer.Start();
                    btnConfirm.Enabled = true;
                    return;
                }

                if (!double.TryParse(txtGuess.Text, out guessPrice))
                    throw new FormatException();
                if (guessPrice <= 0)
                    throw new NegativePriceException();
                if (guessPrice > 100000000)
                    throw new TooHighException();
                winner3.DuDoanGia(guessPrice);
            }
            catch (FormatException)
            {
                MessageBox.Show($"Người chơi {winner3.Ten} Vui lòng nhập giá bằng số!", "Lỗi Nhập Giá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGuess.Focus();
                return;
            }
            catch (NegativePriceException ex)
            {
                MessageBox.Show($"Người chơi {winner3.Ten}: {ex.Message}", "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGuess.Focus();
                return;
            }
            catch (TooHighException ex)
            {
                MessageBox.Show($"Người chơi {winner3.Ten}: {ex.Message}", "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGuess.Focus();
                return;
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Dự Đoán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Round3Timer.Stop();
            btnConfirm.Enabled = false;
            RoundResult();
        }

        private void RoundResult()
        {

            double lowerBound = packPrice * 0.9;

            if (guessPrice >= lowerBound && guessPrice <= packPrice)
            {
                for (int i = 0; i < currentPack.Count; i++) winner3.AddPrize(currentPack[i]);
                winner3.CapNhatTrangThai("Vô địch");
                WinnerEventArgs args = new WinnerEventArgs(winner3);
                round3.OnWinnerDeclared(this, args);
            }
            else
            {

                MessageBox.Show($"Giá thật: {packPrice:N0} VNĐ\n\n" + 
                    $"{currentPack[0].Name}:{currentPack[0].Price}\n" +
                    $"{currentPack[1].Name}:{currentPack[1].Price}\n" +
                    $"{currentPack[2].Name}:{currentPack[2].Price}\n\n" +
                    $"RẤT TIẾC! Bạn đã thua cuộc.", "KẾT QUẢ VÒNG 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                winner3.CapNhatTrangThai("Thua chung kết");
                LoseGame();
            }

        }
        private void LoseGame()
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }
        private void RoundFinal_WinnerDeclared(object sender, WinnerEventArgs e)
        {  
            Player winner = e.Winner;
            MessageBox.Show($"Giá thật: {packPrice:N0} VNĐ\n" +
                    $"{currentPack[0].Name}:{currentPack[0].Price}\n" +
                    $"{currentPack[1].Name}:{currentPack[1].Price}\n" +
                    $"{currentPack[2].Name}:{currentPack[2].Price}\n\n" +
                    $"🎉 CHÚC MỪNG! Người chơi {winner.Ten} đã chiến thắng\n+" +
                    $"Bạn sẽ nhận được toàn bộ gói quà!",
                    "KẾT QUẢ VÒNG 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult result = MessageBox.Show($"Người chiến thắng: {winner.Ten}\n\nBạn có muốn xem lại lịch sử chơi chi tiết không?",
                "Xem Lịch Sử",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string historyDetails = string.Join("\r\n", winner.LichSu);
 
                string historyContent = $"TÊN: {winner.Ten}\r\n" +
                                        $"TỔNG ĐIỂM: {winner.Diem}\r\n" +
                                        "=================================\r\n" +
                                        historyDetails; 

                MessageBox.Show(historyContent,$"📊 Lịch Sử Chơi của {winner.Ten}",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }

            frmWinner f = new frmWinner();
            f.Show();
            this.Close();
        }
        private void btnExitr3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn quay về màn hình chính?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                frmWelcome f = new frmWelcome();
                f.Show();
                this.Close();
            }
        }
        private void frmVong3_Load(object sender, EventArgs e)
        {
            lblWinner2.Text = winner3.Ten;
            lblScorev2.Text = winner3.Diem.ToString();
        }
       
        private void Round3Timer_Tick_1(object sender, EventArgs e)
        {
            timeRemaining--;
            TimeSpan t = TimeSpan.FromSeconds(timeRemaining);
            lblTimer.Text = "⏱️ " + string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

            if (timeRemaining <= 0)
            {
                Round3Timer.Stop();
                btnConfirm.Enabled = false;

                MessageBox.Show("Đã hết thời gian!", "Hết Giờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RoundResult();
            }
            if (timeRemaining <= 30)
            {
                lblTimer.ForeColor = Color.Red;
            }
        }

        
    }
}
