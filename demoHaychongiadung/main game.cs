using demoHaychongiadung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== MAIN GAME CLASS ====================
    public class Game
    {
        private List<Player> players = new List<Player>();
        private List<Product> products = new List<Product>();
        private FileHandler fileHandler;

        public Game()
        {
            fileHandler = new FileHandler("products.dat");
        }

        public void AddPlayer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Vui lòng nhập đầy đủ tên cho cả 4 người chơi!");

            foreach (Player p in players)
            {
                if (p.Ten.Equals(name, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateNameException($"Tên '{name}' đã được chọn. Vui lòng nhập tên khác");
            }
            players.Add(new Player(name));
            
        }
        public List<Player> GetPlayers()
        {
            return players;
        }

        public void AddProduct(Product product)
        {
            products.Add(product);
            //Console.WriteLine($"✓ Đã thêm sản phẩm: {product.Name}");
        }

        public void LoadProducts()
        {
            products = fileHandler.LoadFromFile();
            /*if (products.Count > 0)
            {
                Console.WriteLine($"✓ Đã load {products.Count} sản phẩm từ file.");
            }*/
        }

        public void SaveProducts()
        {
            fileHandler.SaveToFile(products);
        }
        public List<Product> GetProducts()
        {
            return products;
        }
        public void InitializeSampleProducts()
        {
            // Sản phẩm điện tử
            products.Add(new ElectronicProduct("E001", "iPhone 16 Pro Max", 28000000, "Apple", "Trung Quốc",
                "Điện thoại thông minh cao cấp với chip A17 Pro", "image/iphone-16-pro.jpg"));
            products.Add(new ElectronicProduct("E002", "Samsung Galaxy S24 Ultra", 22000000, "Samsung", "Việt Nam",
                "Flagship Android với bút S-Pen", "image/samsung_s24.jpg"));
            products.Add(new ElectronicProduct("E003", "MacBook Air M2", 32000000, "Apple", "Trung Quốc",
                "Laptop mỏng nhẹ cho văn phòng", "image/macbook_m2.jpg"));
            products.Add(new ElectronicProduct("E004", "iPad Pro 12.9 inch", 25000000, "Apple", "Trung Quốc",
                "Máy tính bảng chuyên nghiệp", "image/ipad_pro.jpg"));
            products.Add(new ElectronicProduct("E005", "AirPods Pro Gen 2", 5500000, "Apple", "Việt Nam",
                "Tai nghe không dây chống ồn", "image/airpods_pro.jpg"));
            products.Add(new ElectronicProduct("E006", "Sony WH-1000XM5", 8500000, "Sony", "Thái Lan",
                "Tai nghe over-ear chống ồn tốt nhất", "image/sony_wh1000xm5.jpg"));
            products.Add(new ElectronicProduct("E007", "Tivi Samsung 50 inch", 12000000, "Samsung", "Hàn Quốc",
                "", "image/Smart Tivi Samsung 4K 50 inch.jpg"));

            // Sản phẩm thực phẩm
            products.Add(new FoodProduct("F001", "Gạo ST25 cao cấp", 45000, "2025-12-31", 5,
                "Gạo ngon nhất thế giới 2019-2020", "image/gao_st25.jpg"));
            products.Add(new FoodProduct("F002", "Thịt bò Úc nhập khẩu", 350000, "2025-11-10", 1,
                "Thịt bò Wagyu cao cấp", "image/thit_bo_uc.jpg"));
            products.Add(new FoodProduct("F003", "Cá hồi Na Uy", 280000, "2025-11-08", 0.5,
                "Cá hồi tươi ngon từ Na Uy", "image/ca_hoi.jpg"));
            products.Add(new FoodProduct("F004", "Sữa tươi Vinamilk", 32000, "2025-11-20", 1,
                "Sữa tươi 100% ít đường", "image/sua_vinamilk.jpg"));
            products.Add(new FoodProduct("F005", "Trứng gà sạch Dalat", 55000, "2025-11-15", 0.6,
                "10 quả trứng gà organic", "image/trung_ga.jpg"));
            products.Add(new FoodProduct("F006", "Mật ong rừng nguyên chất", 250000, "2026-01-30", 0.5,
                "Mật ong từ hoa rừng U Minh", "image/mat_ong.jpg"));
            products.Add(new FoodProduct("F007", "Bánh quy Oreo", 35000, "20/12/2025", 0.2,
               "", "image/oreo.jpg"));

            // Sản phẩm quần áo
            products.Add(new ClothingProduct("C001", "Áo thun Polo Nam", 450000, "L", "Cotton",
                "Áo polo basic dễ phối đồ", "image/Polo_shirt.jpg"));
            products.Add(new ClothingProduct("C002", "Quần Jean Nam Slim Fit", 680000, "32", "Denim",
                "Quần jean ống đứng hiện đại", "image/jeans.jpg"));
            products.Add(new ClothingProduct("C003", "Áo khoác mùa đông", 1200000, "M", "Dạ",
                "Áo khoác dạ sang trọng mùa đông", "image/ao_da.jpg"));
            products.Add(new ClothingProduct("C004", "Giày thể thao Nike Air Max", 2500000, "42", "Da tổng hợp",
                "Giày chạy bộ đệm khí", "image/nike_airmax.jpg"));
            products.Add(new ClothingProduct("C005", "Váy công sở Nữ", 580000, "S", "Vải lụa",
                "Váy midi lịch sự cho văn phòng", "image/vay_congso.jpg"));
            products.Add(new ClothingProduct("C006", "Áo sơ mi trắng", 390000, "M", "Cotton",
                "Áo sơ mi basic không nhăn", "image/somi_trang.jpg"));
            products.Add(new ClothingProduct("C007", "Áo khoác Uniqlo", 899000, "L", "Polyester",
                "", "image/ao_khoac.jpg"));
            ElectronicProduct iphoneOrigin = new ElectronicProduct("E007", "AirPods Pro Gen 2 (Khuyến mãi)", 5500000, "Apple", "Việt Nam", "Phiên bản đặc biệt giảm giá", "image/airpods_pro_sale.jpg");
            DiscountedProduct iphoneDiscounted = new DiscountedProduct(iphoneOrigin, 0.25,"E007");
            products.Add(iphoneDiscounted);
            FoodProduct poloOrigin = new FoodProduct("F008", "Gạo ST25 cao cấp", 45000, "2025-12-31", 5, "Gạo giảm giá siêu ưu đãi", "image/gao_st25_sale.jpg");

            DiscountedProduct poloDiscounted = new DiscountedProduct(poloOrigin, 0.30,"F008");

            products.Add(poloDiscounted);

        }

        public void StartGame()
        {

            try
            {
               
                Round1 r1 = new Round1();
                Player winner1 = r1.Play(players, products);

                if (winner1 == null)
                {
                    Console.WriteLine("\n❌ Game kết thúc: Không có ai thắng Vòng 1!");
                    return;
                }

                winner1 = winner1 + 10; // Cộng 10 điểm

                // VÒNG 2: Người thắng Vòng 1 chơi minigame
                

                Round2 r2 = new Round2();
                List<Player> round2Players = new List<Player> { winner1 };
                Player winner2 = r2.Play(round2Players, products);

                if (winner2 == null)
                {
                    Console.WriteLine("\n❌ Game kết thúc: Không vượt qua được Vòng 2!");
                    return;
                }

                winner2 = winner2 + 20; // Cộng 20 điểm

                // VÒNG CHUNG KẾT
                

                RoundFinal rFinal = new RoundFinal();
                rFinal.WinnerDeclared += rFinal.OnWinnerDeclared;
                List<Player> finalPlayers = new List<Player> { winner2 };
                Player champion = rFinal.Play(finalPlayers, products);

                if (champion != null)
                {
                    champion = champion + 50; // Cộng 50 điểm
                    Console.WriteLine("\n");
                    Console.WriteLine("╔══════════════════════════════════════════════════╗");
                    Console.WriteLine("║          🎊 CHÚC MỪNG NHẤT QUÁN 🎊              ║");
                    Console.WriteLine("╚══════════════════════════════════════════════════╝");
                    Console.WriteLine(champion.ThongTin());
                    Console.WriteLine($"\n🎁 Tổng số giải thưởng: {champion.Prizes.Count} sản phẩm");
                    Console.WriteLine("\n📜 Lịch sử chi tiết:");
                    champion.HienThiLichSu();
                }
                else
                {
                    Console.WriteLine("\n📊 Người chơi đã cố gắng hết sức!");
                    Console.WriteLine(winner2.ThongTin());
                    Console.WriteLine($"\n🎁 Giải thưởng: {winner2.Prizes.Count} sản phẩm");
                }
            }
            catch (RoundTimeOverException)
            {
                Console.WriteLine("\n⏰ HẾT GIỜ! Game kết thúc!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Lỗi trong quá trình chơi: {ex.Message}");
            }

        }
    }
}
