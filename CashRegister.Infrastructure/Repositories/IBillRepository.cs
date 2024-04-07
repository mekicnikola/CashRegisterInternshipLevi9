using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Infrastructure.Repositories
{
    public interface IBillRepository
    {
        Task AddBillsAsync(IEnumerable<Bill> bills);
        Task<Bill?> GetBillByIdAsync(int id, IEnumerable<int> excludedBillIds);
        Task<IEnumerable<int>> GetDeletedBillIdsAsync();

        Task UpdateBillAsync(Bill bill);
        Task SoftDeleteBillAsync(int billId);
        Task<IEnumerable<Bill>> GetBillsByUserIdAsync(int userId);
    }
}
