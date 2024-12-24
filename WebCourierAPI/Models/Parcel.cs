using System;
using System.Collections.Generic;

namespace WebCourierAPI.Models;

public partial class Parcel
{
    public int ParcelId { get; set; }

    public string TrackingCode { get; set; } = null!;

    public int SenderCustomerId { get; set; }

    public DateTime? SendTime { get; set; }

    public int ReceiverCustomerId { get; set; }

    public DateTime? ReceiveTime { get; set; }

    public int SenderBranchId { get; set; }

    public int ReceiverBranchId { get; set; }

    public DateTime EstimatedReceiveTime { get; set; }

    public bool IsPaid { get; set; }

    public decimal Price { get; set; }

    public double Weight { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public string UpdateBy { get; set; } = null!;

    public DateTime? UpdateDate { get; set; }

    public bool SendingBranch { get; set; }

    public bool PercelSendingDestribution { get; set; }

    public bool RecebingDistributin { get; set; }

    public bool RecebingBranch { get; set; }

    public bool RecebingReceber { get; set; }

    public bool IsActive { get; set; }

    public int? VanId { get; set; }

    public int? DriverId { get; set; }

    public int? DeliveryChargeId { get; set; }

    public int? ParcelTypeId { get; set; }

    public virtual DeliveryCharge? DeliveryCharge { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ParcelType? ParcelType { get; set; }

    public virtual Branch ReceiverBranch { get; set; } = null!;

    public virtual Receiver ReceiverCustomer { get; set; } = null!;

    public virtual Branch SenderBranch { get; set; } = null!;

    public virtual Customer SenderCustomer { get; set; } = null!;

    public virtual Van? Van { get; set; }
}
