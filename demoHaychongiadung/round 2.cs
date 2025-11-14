using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== ROUND 2 ====================
    public class Round2 : Round
    {
        public override Player Play(List<Player> players, List<Product> products)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║   VÒNG 2: CHỌN MINIGAME                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            StartTimer();

            if (players == null || players.Count == 0 || products == null || products.Count == 0)
                throw new GameNotInitializedException();

            Player player = players[0];

            Console.WriteLine($"\n👤 Người chơi: {player.Ten}");
            Console.WriteLine("\n📋 CHỌN 1 TRONG 3 MINIGAME:");
            Console.WriteLine("   1️⃣  Cao hay Thấp (đúng ≥2/3)");
            Console.WriteLine("   2️⃣  Giá nào đúng (đúng 2/2)");
            Console.WriteLine("   3️⃣  Sắp xếp giá tăng dần (đúng hết)");
            Console.Write("\n❓ Lựa chọn (1-3): ");

            string inStr = Console.ReadLine();
            int choice;
            bool ok = int.TryParse(inStr, out choice);
            if (!ok || choice < 1 || choice > 3) choice = 1;

            MinigameBase game;
            if (choice == 1) game = new MinigameHighLow();
            else if (choice == 2) game = new MinigamePickPrice();
            else game = new MinigameSortPrice();

            bool pass = false;
            if (game is MinigameHighLow)
                pass = ((MinigameHighLow)game).Play(player, products);
            else if (game is MinigamePickPrice)
                pass = ((MinigamePickPrice)game).Play(player, products);
            else if (game is MinigameSortPrice)
                pass = ((MinigameSortPrice)game).Play(player, products);

            if (pass)
            {
                Console.WriteLine($"\n🎉 {player.Ten} THẮNG VÒNG 2 - Vào được chung kết!");
                player.CapNhatTrangThai("Thắng Vòng 2");
                return player;
            }
            else
            {
                Console.WriteLine($"\n❌ {player.Ten} THUA VÒNG 2 - Bị loại!");
                player.CapNhatTrangThai("Bị loại sau Vòng 2");
                return null;
            }
        }
    }

}
