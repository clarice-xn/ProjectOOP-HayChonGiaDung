using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== ROUND CLASSES ====================
    public class ProductPair
    {
        public Product A { get; set; }
        public Product B { get; set; }

        public ProductPair(Product a, Product b)
        {
            A = a;
            B = b;
        }
    }

    public abstract class Round
    {
        protected Random R = new Random();
        protected static List<Product> usedProducts = new List<Product>();
        protected DateTime startTime;
        protected TimeSpan timeLimit = TimeSpan.FromMinutes(5);

        public abstract Player Play(List<Player> players, List<Product> products);

        protected void StartTimer()
        {
            startTime = DateTime.Now;
        }

        protected void CheckTimeout()
        {
            if (DateTime.Now - startTime > timeLimit)
                throw new RoundTimeOverException();
        }

        protected List<Product> GetAvailable(List<Product> products)
        {
            List<Product> available = new List<Product>();
            for (int i = 0; i < products.Count; i++)
            {
                Product p = products[i];
                if (!usedProducts.Contains(p))
                {
                    available.Add(p);
                }
            }
            return available;
        }

        protected void MarkUsed(Product p)
        {
            if (p != null && !usedProducts.Contains(p))
            {
                usedProducts.Add(p);
            }
        }

        protected void MarkUsedMany(List<Product> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Product p = list[i];
                if (p != null && !usedProducts.Contains(p))
                {
                    usedProducts.Add(p);
                }
            }
        }

        public static void ResetUsedList()
        {
            usedProducts.Clear();
        }
    }
}
