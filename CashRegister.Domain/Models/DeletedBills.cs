namespace CashRegister.Domain.Models
{
    public class DeletedBills
    {
        public int Id { get; set; }
        public int BillId { get; set; }

        public Bill Bill { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
