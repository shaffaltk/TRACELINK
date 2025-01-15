using System.ComponentModel.DataAnnotations;

namespace TraceLink.Models
{
    public class TagDetails
    {

        [Key]
        public int TaggingId {  get; set; }
       
        public string? VehicleNumber { get; set; }
        public string? ChasisNumber { get; set; }
        public string? Rto { get; set; }

        public string? DeviceIMEI { get; set; }
        public string? DeviceUID { get; set; }
        public string DeviceModel { get; set; }

        public int NoOfPanicButton { get; set; }

        
        public DateTime TaggingValidity { get; set; }


        public int MobileNumber { get; set; }









    }
}
