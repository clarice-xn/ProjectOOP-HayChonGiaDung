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
    public partial class frmMiniGame3 : Form
    {
        PrivateFontCollection myfonts = new PrivateFontCollection();
        private List<Product> products;
        private Player winner3;
        private MinigameSortPrice minigame3 =new MinigameSortPrice() ;
        private List<Product> fiveProducts; 
        private List<Product> originalOrder; 
        private int timeRemaining;
        private List<PictureBox> picBoxes;
        public frmMiniGame3(Player Winner3, List<Product> products_list)
        {
            InitializeComponent();
            EnableDragDrop();
            winner3 = Winner3;
            products = products_list;
            picBoxes = new List<PictureBox> { pic1, pic2, pic3, pic4, pic5 };
            flowLayoutPanel1.AllowDrop = true;
            flowLayoutPanel1.DragEnter += FlowLayoutPanel1_DragEnter;
            flowLayoutPanel1.DragDrop += FlowLayoutPanel1_DragDrop;
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
            lblRuleGame3.Font = new Font(myfonts.Families[0], 18);
            lblTimer.Font = new Font(myfonts.Families[0], 14);
            lblProduct.Font = new Font(myfonts.Families[0], 12);
            panel2.BackColor = ColorTranslator.FromHtml("#a5a5ac");
            panel1.BackColor = ColorTranslator.FromHtml("#1ABC9C");
            pnlGuessPrice.BackColor = ColorTranslator.FromHtml("#1e1e2f");
            btnConfirm.BackColor = ColorTranslator.FromHtml("#4CAF50");
            btnReset.BackColor = ColorTranslator.FromHtml("#FF9800");
        }
        private void LoadRound()
        { 
            timeRemaining = 30; 
            lblTimer.Text = "⏱️ 30s";
            Game3Timer.Start();
            lblScore3.Text = "0/1";
            LoadProduct();
        }
        private void LoadProduct()
        {
            flowLayoutPanel1.Controls.Clear();
            fiveProducts = minigame3.Get5RandomProducts(products);
            if (fiveProducts == null || fiveProducts.Count < 5)
            {
                MessageBox.Show("Không đủ sản phẩm để chơi minigame!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            originalOrder = new List<Product>(fiveProducts);
      
            for (int i = 0; i < fiveProducts.Count; i++)
            {
                Product currentProduct = fiveProducts[i];
                PictureBox picBox = picBoxes[i];
                picBox.Tag = currentProduct;
                string path = Path.Combine(Application.StartupPath, currentProduct.ImagePath);

                if (!string.IsNullOrEmpty(path))
                {
                    picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    picBox.Image = Image.FromFile(path);

                }
                else
                {
                    picBox.Image = null;
                    MessageBox.Show("Không tìm thấy ảnh: " + path);
                }

                flowLayoutPanel1.Controls.Add(picBox);
            }
        }
        private void EnableDragDrop()
        {
           
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                PictureBox pic = ctrl as PictureBox;
                if (pic != null)
                {
                    pic.MouseDown += Pic_MouseDown;
                }
            }
           
        }
        private void Pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic != null && e.Button == MouseButtons.Left)
            {
                pic.DoDragDrop(pic, DragDropEffects.Move);
            }
        }

        private void FlowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FlowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox dragged = (PictureBox)e.Data.GetData(typeof(PictureBox));
            if (dragged == null)
                return;

            // Vị trí con trỏ trong FlowLayoutPanel
            Point dropPoint = flowLayoutPanel1.PointToClient(new Point(e.X, e.Y));
            Control target = flowLayoutPanel1.GetChildAtPoint(dropPoint);

            if (target == null || target == dragged)
                return;

            int draggedIndex = flowLayoutPanel1.Controls.GetChildIndex(dragged);
            int targetIndex = flowLayoutPanel1.Controls.GetChildIndex(target);

            flowLayoutPanel1.Controls.SetChildIndex(dragged, targetIndex);
            flowLayoutPanel1.Controls.SetChildIndex(target, draggedIndex);
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
           
            flowLayoutPanel1.Controls.Clear();

            for (int i = 0; i < originalOrder.Count; i++)
            {
                PictureBox picBox = picBoxes[i];
                picBox.Tag = originalOrder[i];
                flowLayoutPanel1.Controls.Add(picBox);
            }

            fiveProducts = new List<Product>(originalOrder);
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Game3Timer.Stop();

            List<Product> currentOrder = new List<Product>();
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                PictureBox pic = ctrl as PictureBox;
                if (pic != null && pic.Tag is Product)
                {
                    currentOrder.Add((Product)pic.Tag);
                }
            }

            List<Product> correctOrder = minigame3.GetCorrectSortedOrder(fiveProducts);
            bool isCorrect = true;

            if (currentOrder.Count != correctOrder.Count)
            {
                isCorrect = false;
            }
            else
            {
                for (int i = 0; i < correctOrder.Count; i++)
                {
                    if (!object.ReferenceEquals(currentOrder[i], correctOrder[i]))
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }
            // Hiển thị kết quả
            EndGame(isCorrect, correctOrder);
        }
        private void EndGame(bool isCorrect, List<Product> correctOrder)
        {
            Game3Timer.Stop();
            
            if (isCorrect)
            {
                winner3.Diem += 20;
                lblScore3.Text = "1/1";
                winner3.CapNhatTrangThai("Thắng Minigame vòng 2");

                MessageBox.Show($"🎉 CHÚC MỪNG {winner3.Ten.ToUpper()} THẮNG VÒNG 2!\n" +
                    "$ Bạn được cộng 20 điểm.", "KẾT QUẢ VÒNG 2", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult dialogResult = MessageBox.Show($"Người thắng: {winner3.Ten}. Tiếp tục sang Vòng 3?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    frmVong3 f3 = new frmVong3(winner3, products);
                    f3.Show();
                    this.Close();
                }
            }
            else
            {
                winner3.CapNhatTrangThai("Bị loại sau vòng 2");
                string result = "Thứ tự đúng:\n\n";
                for (int i = 0; i < correctOrder.Count; i++)
                {
                    result += $"{i + 1}. {correctOrder[i].Name} - {correctOrder[i].Price:N0} VNĐ\n";
                }
                MessageBox.Show(result + " RẤT TIẾC! Bạn đã thua Minigame SẮP XẾP TĂNG. Game kết thúc!"
                    , "KẾT QUẢ VÒNG 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoseGame();

            }
        }
        private void LoseGame()
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Close();
        }
      
        private void btnExitGame3_Click(object sender, EventArgs e)
        {
            Game3Timer.Stop();
            DialogResult result = MessageBox.Show("Bạn có muốn thoát minigame?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                frmVong2 f= new frmVong2(winner3,products);
                f.Show();
                this.Close();
            }    
        }

        private void Game3Timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            TimeSpan t = TimeSpan.FromSeconds(timeRemaining);
            lblTimer.Text = "⏱️ " + string.Format("{0:D2} s", t.Seconds);

            if (timeRemaining <= 0)
            {
                Game3Timer.Stop();
                MessageBox.Show("Đã hết thời gian!", "Hết Giờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSubmit_Click(sender, e);
            }
           
        }
        private void btnStartGame3_Click(object sender, EventArgs e)
        {

            lblRuleGame3.Visible = false;
            btnStartGame3.Visible = false;
            LoadRound();
        }

        
    }
}
