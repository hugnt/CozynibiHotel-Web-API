namespace CozynibiHotel.API.Models
{
    public class Email
    {
        public string ToAddress { get; set; }
        public string CustommerName { get; set; }
        public byte[] QRcode { get; set; }
        public string CheckinCode { get; set; }

    }
}
