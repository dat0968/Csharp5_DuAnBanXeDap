using System.ComponentModel.DataAnnotations;

namespace APIBanXeDap.Models
{
    public class Vanchuyen
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public float Gia { get; set; }
        [Required]
        public string Phuong { get; set; }
        [Required]
        public string QuanHuyen { get; set; }
        [Required]
        public string ThanhPho { get; set; }
    }
}
