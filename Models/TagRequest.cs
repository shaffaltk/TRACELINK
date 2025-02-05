namespace TraceLink.Models
{
    public class TaggingRequest
    {
        public int Id { get; set; }
       
        
       public  int? DealerId {  get; set; }

        public Dealer Dealer { get; set; }
        public int?  SubDealerId { get; set; } 
        public SubDealer SubDealer { get; set; }
        public string? IMEI { get; set; } // IMEI of the device being tagged
        public string? VehicleNumber { get; set; } // Vehicle number to be tagged
        public string? ChassisNumber { get; set; } // Chassis number (last 5 digits)
        public string? DeviceUID { get; set; } // UID of the device
        public string? DeviceModel { get; set; } // Model of the device
        public string? Remarks { get; set; } // Remarks about the tagging request
        public int? TaggingStatusId { get; set; }
        
        public TaggingStatus TaggingStatus { get; set; }
        
        public string? RTO { get; set; }
        public int? NoOfPanicButtons { get; set; }
       public string? MobileNumber { get; set; }
        public int? TaggingValidity { get; set; }
        public DateTime RequestDate { get; set; }
        
        public string? TagNumber { get; set; }
    
    }

}

