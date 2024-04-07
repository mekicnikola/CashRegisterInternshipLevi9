using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CashRegister.Infrastructure.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly CashRegisterDBContext _context;
        private readonly ILogger<BillRepository> _logger;

        public BillRepository(CashRegisterDBContext context, ILogger<BillRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddBillsAsync(IEnumerable<Bill> bills)
        {
            _context.Bills.AddRange(bills);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the entity changes.");
                throw;
            }
        }

        public async Task<Bill?> GetBillByIdAsync(int id, IEnumerable<int> excludedBillIds)
        {
            var bill = await _context.Bills
                .Include(b => b.ProductBills)
                .ThenInclude(pb => pb.Product)
                .AsNoTracking()
                .Where(b => !excludedBillIds.Contains(b.Id) && b.Id == id)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefaultAsync();

            return bill;
        }

        public async Task UpdateBillAsync(Bill bill)
        {
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<int>> GetDeletedBillIdsAsync()
        {
            return new HashSet<int>(await _context.DeletedBills.Select(db => db.BillId).ToListAsync());
        }

        public async Task SoftDeleteBillAsync(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill == null)
            {
                throw new KeyNotFoundException("Bill not found.");
            }

            var deletedBill = new DeletedBills
            {
                BillId = bill.Id,
                DeletedAt = DateTime.UtcNow
            };

            _context.DeletedBills.Add(deletedBill);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bill>> GetBillsByUserIdAsync(int userId)
        {
            var activeBills = GetActiveBillsQuery();
            return await activeBills.Where(bill => bill.UserId == userId).ToListAsync();
        }

        private IQueryable<Bill> GetActiveBillsQuery()
        {
            var deletedBillIds = _context.DeletedBills.Select(db => db.BillId).ToList();
            return _context.Bills.Where(bill => !deletedBillIds.Contains(bill.Id));
        }

    }
}
