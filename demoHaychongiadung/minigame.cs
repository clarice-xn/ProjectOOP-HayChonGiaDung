using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== MINIGAME BASE ====================
    public abstract class MinigameBase : Round
    {
        protected Product PickUniqueProduct(List<Product> products)
        {
            List<Product> available = GetAvailable(products);
                if (available.Count == 0)
                {
                return null;//throw new NotFoundProductException();
                }
            int idx = R.Next(0, available.Count);
            Product p = available[idx];
            MarkUsed(p);
            return p;
        }

        protected ProductPair PickUniquePair(List<Product> products)
        {
            List<Product> available = GetAvailable(products);
            if (available.Count < 2) return null;

            Product a = available[R.Next(0, available.Count)];
            Product b = a;
            while (b == a)
            {
                b = available[R.Next(0, available.Count)];
            }

            MarkUsed(a);
            MarkUsed(b);
            return new ProductPair(a, b);
        }

        protected void ShuffleListDouble(List<double> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = R.Next(0, i + 1);
                double tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }
    }

    // ==================== MINIGAME 1 ====================
    public class MinigameHighLow : MinigameBase
    {
        public override Player Play(List<Player> players, List<Product> products)
        {
            return null;
        }
        public ProductPair GetRandomProductPair(List<Product> products)
        {
            return PickUniquePair(products);
        }
        public bool CheckPlayerGuess(ProductPair pair, bool guessAHigher)
        {
            bool actualAHigher = (pair.A.Price > pair.B.Price);

            if ((guessAHigher && actualAHigher) || (!guessAHigher && !actualAHigher))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Play(Player player, List<Product> products)
        {
           
            int correct = 0;

            for (int round = 1; round <= 3; round++)
            {
                Console.WriteLine($"\n─── LƯỢT {round}/3 ───");
                CheckTimeout();
                ProductPair pair = PickUniquePair(products);
                if (pair == null)
                {
                    Console.WriteLine("⚠ Không đủ sản phẩm cho minigame.");
                    break;
                }

                Console.WriteLine($"\n📦 Sản phẩm A: {pair.A.Name}");
                Console.WriteLine($"📦 Sản phẩm B: {pair.B.Name}");
                Console.Write("\n❓ Chọn [A] nếu A cao hơn / [B] nếu B cao hơn: ");
                string input = Console.ReadLine();
                if (input == null) input = "";
                input = input.Trim().ToUpper();

                bool guessAHigher = (input == "A");
                bool actualAHigher = (pair.A.Price > pair.B.Price);

                if ((guessAHigher && actualAHigher) || (!guessAHigher && !actualAHigher))
                {
                    Console.WriteLine("✅ ĐÚNG RỒI!");
                    correct++;
                }
                else
                {
                    Console.WriteLine($"❌ SAI! Giá A: {pair.A.Price:N0} | Giá B: {pair.B.Price:N0}");
                }
            }

            bool pass = (correct >= 2);
            Console.WriteLine($"\n{new string('═', 50)}");
            Console.WriteLine($"📊 KẾT QUẢ: {correct}/3 đúng");
            Console.WriteLine(pass ? "🎉 THẮNG MINIGAME!" : "❌ THUA MINIGAME (cần ít nhất 2/3)");
            Console.WriteLine(new string('═', 50));
            return pass;
        }
    }

    // ==================== MINIGAME 2 ====================
    public class MinigamePickPrice : MinigameBase
    {
        public override Player Play(List<Player> players, List<Product> products)
        {
            return null;
        }
        public Product GetProductForMinigame(List<Product> products)
        {
              return PickUniqueProduct(products);
        }
        public double NormalizePrice(double price)
        {
            double unit;
            if (price < 100000)
                unit = 1000;
            else if (price < 1000000)
                unit = 10000;
            else if (price < 10000000)
                unit = 100000;
            else
                unit = 1000000;
            return Math.Round(price / unit) * unit;
        }

        public List<double> GenerateAndShufflePrices(double productPrice)
        {
            List<double> options = new List<double>();
            Random R = new Random();
            double minRange = productPrice * 0.8;
            double maxRange = productPrice * 1.2;
            options.Add(productPrice);

            for (int i = 0; i < 2; i++)
            {
                double randomPrice;
                do
                {
                    randomPrice = R.NextDouble() * (maxRange - minRange) + minRange;    
                }
                while (options.Contains(NormalizePrice(randomPrice))); 
                options.Add(NormalizePrice(randomPrice));
            }
       
            ShuffleListDouble(options);
            return options;
        }


        public bool Play(Player player, List<Product> products)
        {
           
            int success = 0;

            for (int round = 1; round <= 2; round++)
            {
                Console.WriteLine($"\n─── LƯỢT {round}/2 ───");
                CheckTimeout();
                Product p = PickUniqueProduct(products);
                if (p == null)
                {
                    Console.WriteLine("⚠ Không đủ sản phẩm để chơi.");
                    break;
                }

                List<double> options = new List<double>();
                options.Add(p.Price);
                options.Add(p.Price + 10000);
                options.Add(p.Price - 5000);

                ShuffleListDouble(options);

                Console.WriteLine($"\n📦 Sản phẩm: {p.Name}");
                Console.WriteLine("\n💰 Chọn giá đúng:");
                for (int i = 0; i < options.Count; i++)
                    Console.WriteLine($"   {i + 1}. {options[i]:N0} VNĐ");

                Console.Write("\n❓ Lựa chọn (1-3): ");
                string inStr = Console.ReadLine();
                int pick;
                bool ok = int.TryParse(inStr, out pick);
                if (!ok || pick < 1 || pick > 3) pick = 1;

                if (options[pick - 1] == p.Price)
                {
                    Console.WriteLine("✅ ĐÚNG RỒI!");
                    success++;
                }
                else
                {
                    Console.WriteLine($"❌ SAI! Giá thực: {p.Price:N0} VNĐ");
                }
            }

            bool pass = (success == 2);
            Console.WriteLine($"\n{new string('═', 50)}");
            Console.WriteLine($"📊 KẾT QUẢ: {success}/2 đúng");
            Console.WriteLine(pass ? "🎉 THẮNG MINIGAME!" : "❌ THUA MINIGAME (cần 2/2)");
            Console.WriteLine(new string('═', 50));
            return pass;
        }
    }

    // ==================== MINIGAME 3 ====================
    public class MinigameSortPrice : MinigameBase
    {
        public override Player Play(List<Player> players, List<Product> products)
        {
            return null;
        }
        // Trong lớp MinigameSortPrice.cs (Giả định)
        // Lưu ý: Em cần đảm bảo Product có thuộc tính 'Used' (bool)

        public List<Product> Get5RandomProducts(List<Product> products)
        {

            List<Product> available = GetAvailable(products);

            if (available.Count < 5)
            {
                throw new NotFoundProductException();
            }
           
            List<Product> five = new List<Product>();
            for (int i = 0; i < 5; i++)
            {
                int idx = R.Next(0, available.Count);
                Product sel = available[idx];
                five.Add(sel);
                MarkUsed(sel);
                available.RemoveAt(idx);
            }

            return five;
        }
        public List<Product> GetCorrectSortedOrder(List<Product> fives)
        {
          
            List<Product> sorted = new List<Product>(fives);
            for (int i = 0; i < sorted.Count - 1; i++)
            {
                for (int j = i + 1; j < sorted.Count; j++)
                {
                    if (sorted[i].Price > sorted[j].Price)
                    { 
                        Product tmp = sorted[i];
                        sorted[i] = sorted[j];
                        sorted[j] = tmp;
                    }
                }
            }
           
            return sorted;
        }
        public bool Play(Player player, List<Product> products)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║   MINIGAME 3: SẮP XẾP GIÁ TĂNG DẦN     ║");
            Console.WriteLine("║   (Phải đúng hết)                        ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            StartTimer();

            List<Product> available = GetAvailable(products);
            if (available.Count < 5)
            {
                Console.WriteLine("⚠ Không đủ sản phẩm cho minigame.");
                return false;
            }

            List<Product> five = new List<Product>();
            for (int i = 0; i < 5; i++)
            {
                int idx = R.Next(0, available.Count);
                Product sel = available[idx];
                five.Add(sel);
                MarkUsed(sel);
                available.RemoveAt(idx);
            }

            Console.WriteLine("\n📦 5 SẢN PHẨM:");
            for (int i = 0; i < five.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. {five[i].Name}");
            }

            Console.WriteLine("\n📝 Hãy sắp xếp theo thứ tự giá TĂNG DẦN (từ rẻ đến đắt)");
            Console.Write("   Nhập thứ tự (ví dụ: 3 1 2 5 4): ");
            CheckTimeout();
            string line = Console.ReadLine();
            if (line == null) line = "";
            string[] tokens = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 5)
            {
                Console.WriteLine("❌ Input không hợp lệ.");
                return false;
            }

            List<Product> userOrder = new List<Product>();
            for (int i = 0; i < tokens.Length; i++)
            {
                int ind;
                bool ok = int.TryParse(tokens[i], out ind);
                if (!ok || ind < 1 || ind > 5)
                {
                    Console.WriteLine("❌ Số không hợp lệ.");
                    return false;
                }
                userOrder.Add(five[ind - 1]);
            }

            // Sắp xếp đúng theo giá
            List<Product> sorted = new List<Product>();
            for (int i = 0; i < five.Count; i++) sorted.Add(five[i]);

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                for (int j = i + 1; j < sorted.Count; j++)
                {
                    if (sorted[i].Price > sorted[j].Price)
                    {
                        Product tmp = sorted[i];
                        sorted[i] = sorted[j];
                        sorted[j] = tmp;
                    }
                }
            }

            bool allMatch = true;
            for (int i = 0; i < sorted.Count; i++)
            {
                if (!object.ReferenceEquals(sorted[i], userOrder[i]))
                {
                    allMatch = false;
                    break;
                }
            }

            Console.WriteLine($"\n{new string('═', 50)}");
            Console.WriteLine("📊 THỨ TỰ ĐÚNG (giá tăng dần):");
            for (int i = 0; i < sorted.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. {sorted[i].Name} - {sorted[i].Price:N0} VNĐ");
            }
            Console.WriteLine(allMatch ? "\n✅ CHÍNH XÁC! THẮNG MINIGAME!" : "\n❌ SAI RỒI! THUA MINIGAME!");
            Console.WriteLine(new string('═', 50));
            return allMatch;
        }
    }
}
