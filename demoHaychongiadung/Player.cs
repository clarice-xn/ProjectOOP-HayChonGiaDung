using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== PLAYER CLASS ====================
    public class Player
    {
        public string Ten { get; set; }
        public int Diem { get; set; }
        public string TrangThai { get; set; }
        public double? DuDoan { get; set; }
        public List<string> LichSu { get; set; }
        public List<Product> Prizes { get; set; }

        public Player(string ten)
        {
            Ten = ten;
            Diem = 0;
            TrangThai = "Đang chơi";
            DuDoan = null;
            LichSu = new List<string>();
            Prizes = new List<Product>();
        }

        public void DuDoanGia(double gia)
        {
            DuDoan = gia;
            LichSu.Add($"Dự đoán giá: {gia:N0} VNĐ");
        }

        public void CapNhatTrangThai(string trangThaiMoi)
        {
            TrangThai = trangThaiMoi;
            LichSu.Add($"Trạng thái: {trangThaiMoi}");
        }

        public void AddPrize(Product product)
        {
            Prizes.Add(product);
            LichSu.Add($"🎁 Nhận giải thưởng: {product.Name}");
        }

        public static Player operator +(Player player, int diem)
        {
            player.Diem += diem;
            player.LichSu.Add($"⭐ +{diem} điểm. Tổng: {player.Diem} điểm");
            return player;
        }

        public string ThongTin()
        {
            return $"👤 {Ten} | 🏆 {Diem} điểm | 📍 {TrangThai}";
        }

        public void HienThiLichSu()
        {
            Console.WriteLine($"\n═══ LỊCH SỬ: {Ten} ═══");
            foreach (string item in LichSu)
            {
                Console.WriteLine($"  • {item}");
            }
            Console.WriteLine("════════════════════════════");
        }
    }
}
