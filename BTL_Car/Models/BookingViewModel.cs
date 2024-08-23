using System.ComponentModel.DataAnnotations;

namespace BTL_Car.Models
{
    public class BookingViewModel
    {
        public int CarId { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime RentalStartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Compare("RentalStartDate", ErrorMessage = "Ngày kết thúc phải lớn hơn ngày bắt đầu.")]
        public DateTime RentalEndDate { get; set; }
        public decimal TotalCost { get; set; } // Có thể không cần trong view model nếu tính toán trên server


    }
    
}
