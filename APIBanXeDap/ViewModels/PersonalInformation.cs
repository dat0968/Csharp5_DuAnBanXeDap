using APIBanXeDap.Models;

namespace APIBanXeDap.ViewModels
{
    public class PersonalInformation
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string? SDT { get; set; }
        public string VaiTro { get; set; }
        public string RefreshToken { get; set; }
    }
}
