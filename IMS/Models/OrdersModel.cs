namespace IMS.Models
{
    public class OrdersModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? First { get; set; }
        public string? Middle { get; set; }
        public string? Last { get; set; }
        public int? MobileNo { get; set; }

        public int? ProductId { get; set; }
        public int SuppliersId { get; set; }
        public string? NumberShipped { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class OrdersViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? First { get; set; }
        public string? Middle { get; set; }
        public string? Last { get; set; }
        public int? MobileNo { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }

        public int SuppliersId { get; set; }

        public string? Supplier_Name { get; set; }
        public string? NumberShipped { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class OrderReportModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? First { get; set; }
        public string ProductName { get; set; }
        public string Supplier_Name{get;set;}
        public string? Middle { get; set; }
        public string? Last { get; set; }
        public int? MobileNo { get; set; }
        public int? ProductId { get; set; }
        

        public int SuppliersId { get; set; }

        
        public string? NumberShipped { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
