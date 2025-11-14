using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== ROUND 1 ====================
    public class Round1 : Round
    {
        public Product GetRandomProduct(List<Product> products)
        {
            List<Product> available = GetAvailable(products); 

            if (available.Count == 0)
            {
                return null; //throw new NotFoundProductException();
            }

            int idx = R.Next(0, available.Count);
            Product target = available[idx];
            MarkUsed(target); 
            return target;
        }
        public override Player Play(List<Player> players, List<Product> products)
        {
            StartTimer();

            if (players == null || players.Count == 0 || products == null || products.Count == 0)
                throw new GameNotInitializedException();
            Product target = products[0];

            double bestGuess = -1;
            Player winner = null;
            List<double> usedGuesses = new List<double>();

            for (int i = 0; i < players.Count; i++)
            {
                CheckTimeout();
                Player p1 = players[i];
                bool validGuess = false;

                while (!validGuess)
                {
                    try
                    {
                        Console.Write($"👤 {p1.Ten}, dự đoán giá là (VNĐ): ");
                        string input = Console.ReadLine();
                        double guess;

                        if (!double.TryParse(input, out guess))
                            throw new FormatException("Vui lòng nhập giá bằng số");
                        if (guess <= 0)
                            throw new NegativePriceException();
                        if (guess > 1000000000)
                            throw new TooHighException();

                        // Kiểm tra trùng giá
                        foreach (double usedPrice in usedGuesses)
                        {
                            if (Math.Abs(guess - usedPrice) < 1)
                                throw new DuplicatePriceException();
                        }

                        usedGuesses.Add(guess);
                        p1.DuDoanGia(guess);

                        // Logic: gần nhất nhưng không vượt quá
                        if (guess <= target.Price)
                        {
                            if (guess > bestGuess)
                            {
                                bestGuess = guess;
                                winner = p1;
                            }
                        }

                        validGuess = true;
                        Console.WriteLine($"✓ Đã ghi nhận: {guess:N0} VNĐ\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ {ex.Message}\n");
                    }
                }
            }

            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine($"💰 GIÁ THỰC: {target.Price:N0} VNĐ");
            Console.WriteLine(new string('═', 50));

            if (winner != null)
            {
                Console.WriteLine($"\n🎉 CHÚC MỪNG {winner.Ten.ToUpper()} THẮNG VÒNG 1!");
                Console.WriteLine($"   Giá đoán: {bestGuess:N0} VNĐ");
                winner.CapNhatTrangThai("Thắng Vòng 1");
            }
            else
            {
                Console.WriteLine("\n❌ Không ai đoán hợp lệ (tất cả đều vượt quá giá thật)");
            }

            // Cập nhật trạng thái cho người thua
            foreach (Player p in players)
            {
                if (p != winner)
                {
                    p.CapNhatTrangThai("Bị loại sau Vòng 1");
                }
            }

            return winner;
        }
    }
}
