using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== PRODUCT CLASSES ====================
    [Serializable]
    public abstract class Product
    {
        private string id;
        private string name;
        private double price;
        private string description;
        private string imagePath;

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public double Price
        {
            get { return this.price; }
            set { this.price = value > 0 ? value : 0; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; } 
        }
        public string ImagePath
        {
            get { return this.imagePath; }
            set { this.imagePath = value; }
        }

        protected Product(string id, string name, double price, string description = "", string imagePath = "")
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.description = description;
            this.imagePath = imagePath;
        }

        public abstract void DisplayInfo();

        public virtual bool GuessPrice(double userGuess)
        {
            double diff = Math.Abs(this.price - userGuess);
            return diff <= this.price * 0.1;
        }

    }
    [Serializable]
    public class ElectronicProduct : Product
    {
        private string brand;
        private string origin;

        public ElectronicProduct(string id, string name, double price, string brand, string origin,
                                string description = "", string imagePath = "")
            : base(id, name, price, description, imagePath)
        {
            this.brand = brand;
            this.origin = origin;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"   Điện tử: {Name}");
            Console.WriteLine($"   Hãng: {brand} | Xuất xứ: {origin}");
        }
    }
    [Serializable]
    public class FoodProduct : Product
    {
        private string expiryDate;
        private double weight;

        public FoodProduct(string id, string name, double price, string expiryDate, double weight,
                          string description = "", string imagePath = "")
            : base(id, name, price, description, imagePath)
        {
            this.expiryDate = expiryDate;
            this.weight = weight;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"   Thực phẩm: {Name}");
            Console.WriteLine($"   HSD: {expiryDate} | Khối lượng: {weight}kg");
        }
    }
    [Serializable]
    public class ClothingProduct : Product
    {
        private string size;
        private string material;

        public ClothingProduct(string id, string name, double price, string size, string material,
                              string description = "", string imagePath = "")
            : base(id, name, price, description, imagePath)
        {
            this.size = size;
            this.material = material;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"   Quần áo: {Name}");
            Console.WriteLine($"   Size: {size} | Chất liệu: {material}");
        }
    }
    [Serializable]
    public class DiscountedProduct : Product
    {
        private readonly Product _baseProduct;
        private readonly double _discount;

        public DiscountedProduct(Product baseProduct, double discount, string newId = "")
            : base(
                  string.IsNullOrEmpty(newId) ? $"{baseProduct.Id}_DISCOUNT" : newId,
                  baseProduct.Name,
                  baseProduct.Price * (1 - discount),
                  baseProduct.Description,
                  baseProduct.ImagePath)
        {
            _baseProduct = baseProduct ?? throw new ArgumentNullException(nameof(baseProduct));
            _discount = discount;
        }

        public override void DisplayInfo()
        {
            _baseProduct.DisplayInfo();
            Console.WriteLine($" GIÁ GỐC: {_baseProduct.Price:N0} VNĐ");
            Console.WriteLine($" GIẢM: {_discount:P0} → GIÁ MỚI: {Price:N0} VNĐ (KHUYẾN MÃI)");
        }

    }
}
