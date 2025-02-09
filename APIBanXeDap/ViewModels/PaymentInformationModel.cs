namespace APIBanXeDap.ViewModels
{
    public class PaymentInformationModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Amout { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
