namespace application.MVVM.Model;

public class OrderModel
{
	public Guid ID { get; set; }
	public Guid Entity_Managers_ID { get; set; }
	public string INN { get; set; } = string.Empty;
	public string KPP { get; set; } = string.Empty;
	public string OGRN { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Payment_Account { get; set; } = string.Empty;
	public string ToBIK { get; set; } = string.Empty;
	public string ToBank { get; set; } = string.Empty;
	public string FromCor_Account { get; set; } = string.Empty;
	public string FromBIK { get; set; } = string.Empty;
	public string FromBank { get; set; } = string.Empty;
	public Guid Entity_Storage_ID { get; set; }
	public DateTime Plan_Date_Shipment { get; set; }
	public string Shipping_Address { get; set; } = string.Empty;
	public DateTime Application_Date { get; set; }
	public string Delivery_Point { get; set; } = string.Empty;
	public string Delivery_Address { get; set; } = string.Empty;
	public DateTime Plan_Date_Receipt { get; set; }
	public string TransporterFullName { get; set; } = string.Empty;
	public string TransporterShortName { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
	public double VAT { get; set; }

	public OrderModel()
	{

	}
    
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