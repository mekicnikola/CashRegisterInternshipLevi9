namespace CashRegister.Domain.Models
{
    public class ProductBill
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
