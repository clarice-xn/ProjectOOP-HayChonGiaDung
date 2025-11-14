using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{

    // ==================== ROUND FINAL ====================
    public class RoundFinal : Round
    {
      public event WinnerEventHandler WinnerDeclared;
      public void OnWinnerDeclared(object sender, WinnerEventArgs e)
      {
          Console.WriteLine($"\n🏆🏆🏆 NGƯỜI CHIẾN THẮNG CHUNG CUỘC: {e.Winner.Ten.ToUpper()} 🏆🏆🏆");
            if (WinnerDeclared != null)
            {
                WinnerDeclared(sender, e);
            }
      }
       
        public List<Product> GetRandomProductPack(List<Product> products)
        {
            List<Product> available = GetAvailable(products);
            if (available.Count < 3)
            {
                throw new NotFoundProductException();   
            }
  
            List<Product> pack = new List<Product>();
            for (int i = 0; i < 3; i++)
            {
                int idx = R.Next(0, available.Count);
                Product p = available[idx];
                pack.Add(p);
                MarkUsed(p);
                available.RemoveAt(idx);
            }

            return pack;
        }
        public override Player Play(List<Player> players, List<Product> products)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║   VÒNG CHUNG KẾT                        ║");
            Console.WriteLine("║   (Đoán giá gói 3 sản phẩm)             ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            StartTimer();

            if (players == null || players.Count == 0 || products == null || products.Count == 0)
                throw new GameNotInitializedException();

            Player player = players[0];
            Console.WriteLine($"\n👤 Người chơi: {player.Ten}");
            List<Product> available = GetAvailable(products);
            List<Product> pack = new List<Product>();
            for (int i = 0; i < 3; i++)
            {
                int idx = R.Next(0, available.Count);
                Product p = available[idx];
                pack.Add(p);
                MarkUsed(p);
                available.RemoveAt(idx);
            }
            // Tính tổng giá
            double packPrice = 0;
            for (int i = 0; i < pack.Count; i++)
                packPrice += pack[i].Price;

            double lower = packPrice * 0.9; // Không được nhỏ hơn 90% (tức là nhỏ hơn 10%)
            double upper = packPrice;       // Không được vượt quá giá thật

            Console.WriteLine("\n🎁 GÓI QUÀ GỒM 3 SẢN PHẨM:");
            Console.WriteLine(new string('─', 50));
            for (int i = 0; i < pack.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. {pack[i].Name}");
            }
            Console.WriteLine(new string('─', 50));

            // Gợi ý khoảng giá (cách xa để dễ chơi hơn)
            double hintLower = packPrice * 0.7;  // Gợi ý từ 70%
            double hintUpper = packPrice * 1.2;  // đến 120%
            Console.WriteLine($"\n💡 GỢI Ý: Giá gói quà nằm trong khoảng");
            Console.WriteLine($"   {hintLower:N0} VNĐ - {hintUpper:N0} VNĐ");
            Console.WriteLine($"\n⚠️  LƯU Ý: ");
            Console.WriteLine($"   • Đoán giá PHẢI từ {lower:N0} VNĐ trở lên");
            Console.WriteLine($"   • Đoán giá KHÔNG ĐƯỢC vượt quá {upper:N0} VNĐ");

            Console.Write($"\n❓ Nhập dự đoán giá gói quà (VNĐ): ");
            CheckTimeout();
            string inStr = Console.ReadLine();
            double guess;
            bool ok = double.TryParse(inStr, out guess);
            if (!ok) guess = 0;

            player.DuDoanGia(guess);

            Console.WriteLine($"\n{new string('═', 50)}");
            Console.WriteLine($"💰 GIÁ THỰC CỦA GÓI QUÀ: {packPrice:N0} VNĐ");
            Console.WriteLine($"📊 DỰ ĐOÁN CỦA BẠN: {guess:N0} VNĐ");
            Console.WriteLine(new string('═', 50));

            // Kiểm tra điều kiện: >= 90% và <= 100%
            if (guess >= lower && guess <= packPrice)
            {
                Console.WriteLine("\n✅ CHÍNH XÁC! BẠN ĐÃ THẮNG CHUNG KẾT!");
                Console.WriteLine("\n🎁 BẠN NHẬN ĐƯỢC CẢ 3 SẢN PHẨM:");
                for (int i = 0; i < pack.Count; i++)
                {
                    player.AddPrize(pack[i]);
                    Console.WriteLine($"   ✓ {pack[i].Name}");
                }

                player.CapNhatTrangThai("Vô địch");

                if (WinnerDeclared != null)
                    WinnerDeclared(this, new WinnerEventArgs(player));

                return player;
            }
            else
            {
                Console.WriteLine("\n❌ TIẾC QUÁ! BẠN KHÔNG TRÚNG GÓI QUÀ");
                if (guess < lower)
                    Console.WriteLine($"   Lý do: Giá đoán thấp hơn 10% so với giá thật");
                else
                    Console.WriteLine($"   Lý do: Giá đoán vượt quá giá thật");

                Console.WriteLine("\n🎁 BẠN ĐƯỢC CHỌN 1 SẢN PHẨM MANG VỀ:");
                for (int i = 0; i < pack.Count; i++)
                    Console.WriteLine($"   {i + 1}. {pack[i].Name}");

                Console.Write("\n❓ Chọn sản phẩm (1-3): ");
                string pickStr = Console.ReadLine();
                int pick;
                bool ok2 = int.TryParse(pickStr, out pick);
                if (!ok2) pick = 1;
                if (pick < 1 || pick > pack.Count) pick = 1;

                player.AddPrize(pack[pick - 1]);
                Console.WriteLine($"\n✓ Bạn đã chọn: {pack[pick - 1].Name}");
                player.CapNhatTrangThai("Thua chung kết");

                return null;
            }
        }
    }
}
