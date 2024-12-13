namespace application.MVVM.Model;

public class OrderModel
{
	public Guid ID { get; set; }
	public Guid Entity_Managers_ID { get; set; }
	public string INN { get; set; } = null!;
	public string KPP { get; set; } = null!;
	public string OGRN { get; set; } = null!;
	public string FullName { get; set; } = null!;
	public string Address { get; set; } = null!;
	public string Payment_Account { get; set; } = null!;
	public string ToBIK { get; set; } = null!;
	public string ToBank { get; set; } = null!;
	public string FromCor_Account { get; set; } = null!;
	public string FromBIK { get; set; } = null!;
	public string FromBank { get; set; } = null!;
	public Guid Entity_Storage_ID { get; set; }
	public DateTime Plan_Date_Shipment { get; set; }
	public string Shipping_Address { get; set; } = null!;
	public DateTime Application_Date { get; set; }
	public string Delivery_Point { get; set; } = null!;
	public string Delivery_Address { get; set; } = null!;
	public DateTime Plan_Date_Receipt { get; set; }
	public string TransporterFullName { get; set; } = null!;
	public string TransporterShortName { get; set; } = null!;
	public string Comment { get; set; } = null!;
	public double VAT { get; set; }

	public OrderModel() { }
    
	public OrderModel(
	    Guid id,
	    Guid entityManagersId,
	    string inn,
	    string kpp,
	    string ogrn,
	    string fullName,
	    string address,
	    string paymentAccount,
	    string toBik,
	    string toBank,
	    string fromCorAccount,
	    string fromBik,
	    string fromBank,
	    Guid entityStorageId,
	    DateTime planDateShipment,
	    string shippingAddress,
	    DateTime applicationDate,
	    string deliveryPoint,
	    string deliveryAddress,
	    DateTime planDateReceipt,
	    string transporterFullName,
	    string transporterShortName,
	    string comment,
	    double vat)
	{
	    ID = id;
	    Entity_Managers_ID = entityManagersId;
	    INN = inn;
	    KPP = kpp;
	    OGRN = ogrn;
	    FullName = fullName;
	    Address = address;
	    Payment_Account = paymentAccount;
	    ToBIK = toBik;
	    ToBank = toBank;
	    FromCor_Account = fromCorAccount;
	    FromBIK = fromBik;
	    FromBank = fromBank;
	    Entity_Storage_ID = entityStorageId;
	    Plan_Date_Shipment = planDateShipment;
	    Shipping_Address = shippingAddress;
	    Application_Date = applicationDate;
	    Delivery_Point = deliveryPoint;
	    Delivery_Address = deliveryAddress;
	    Plan_Date_Receipt = planDateReceipt;
	    TransporterFullName = transporterFullName;
	    TransporterShortName = transporterShortName;
	    Comment = comment;
	    VAT = vat;
	}
}