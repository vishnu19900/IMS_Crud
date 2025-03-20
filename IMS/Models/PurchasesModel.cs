namespace IMS.Models
{
    public class PurchasesModel
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int Number_Received { get; set; }
        public DateTime Purchase_date { get; set; }
    }
    public class PurchaseReportModel
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string Supplier_Name { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Number_Received { get; set; }
        public DateTime Purchase_date { get; set; }
    }
}
