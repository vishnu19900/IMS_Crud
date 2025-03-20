namespace IMS.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int PartNumber { get; set; }
        public string ProductLabel { get; set; }
        public int StartingInventory { get; set; }
        public int InventoryReceived { get; set; }
        public int Inventoryshipped { get; set; }
        public int InventoryOnHand { get; set; }
        public int MinimumRequired { get; set; }
    }
}
