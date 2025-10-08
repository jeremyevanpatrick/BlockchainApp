using System;
using System.Runtime.Serialization;

[Serializable]
public class Transaction
{
    [DataMember(Name = "TransactionId")]
    public string TransactionId { get; set; }

    [DataMember(Name = "TransactionNumber")]
    public string TransactionNumber { get; set; }

    [DataMember(Name = "ToAccount")]
    public string ToAccount { get; set; }

    [DataMember(Name = "FromAccount")]
    public string FromAccount { get; set; }

    [DataMember(Name = "Amount")]
    public decimal Amount { get; set; }

    [DataMember(Name = "Timestamp")]
    public DateTime Timestamp { get; set; }

    [DataMember(Name = "Notes")]
    public string Notes { get; set; }

}