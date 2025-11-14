using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== CUSTOM EXCEPTIONS ====================
    public class NegativePriceException : Exception
    {
        public NegativePriceException() : base("Giá không được nhỏ hơn hay bằng 0. Vui lòng nhập lại") { }
    }

    public class DuplicateNameException : Exception
    {
        public DuplicateNameException(string message) : base(message) { }
    }

    public class DuplicatePriceException : Exception
    {
        public DuplicatePriceException() : base("Giá này đã được người chơi khác chọn. Vui lòng nhập giá khác") { }
    }

    public class TooHighException : Exception
    {
        public TooHighException() : base("Chỉ được nhập giá dưới 1000000000. Vui lòng nhập lại") { }
    }

    public class NotFoundProductException : Exception
    {
        public NotFoundProductException() : base("Không tìm thấy sản phẩm hợp lệ") { }
    }

    public class GameNotInitializedException : Exception
    {
        public GameNotInitializedException() : base("Vòng chơi chưa được khởi tạo. Vui lòng đợi game được bắt đầu!") { }
    }

    public class RoundTimeOverException : Exception
    {
        public RoundTimeOverException() : base("Đã hết thời gian!") { }
    }

    public class NoWinnerException : Exception
    {
        public NoWinnerException() : base("Không có người chơi chiến thắng") { }
    }
}
