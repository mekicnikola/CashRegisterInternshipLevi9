using AutoMapper;
using CashRegister.Application.Services;
using CashRegister.Application.Services.CurrencyManager;
using CashRegister.Application.Services.Dto;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Context;
using CashRegister.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace CashRegister.Application.Tests
{
    [TestClass]
    public class BillServiceTests
    {
        private Mock<IBillRepository>? _billRepositoryMock;
        private Mock<CashRegisterDBContext>? _contextMock;
        private Mock<ICreditCardService>? _creditCardServiceMock;
        private Mock<ICurrencyManager>? _currencyManagerMock;
        private Mock<IMapper>? _mapperMock;
        private BillService? _billService;

        [TestInitialize]
        public void Setup()
        {
            _billRepositoryMock = new Mock<IBillRepository>();
            _contextMock = new Mock<CashRegisterDBContext>(new DbContextOptions<CashRegisterDBContext>());

            _creditCardServiceMock = new Mock<ICreditCardService>();
            _currencyManagerMock = new Mock<ICurrencyManager>();

            _mapperMock = new Mock<IMapper>();

            _billService = new BillService(_contextMock.Object, _creditCardServiceMock.Object, _billRepositoryMock.Object, _currencyManagerMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task CreateBillAsync_UserDoesNotExist_ThrowsArgumentException()
        {
            var options = new DbContextOptionsBuilder<CashRegisterDBContext>()
                .UseInMemoryDatabase(databaseName: "CreateBillAsync_UserDoesNotExist")
                .Options;

            await using var context = new CashRegisterDBContext(options);
            var billService = new BillService(context, _creditCardServiceMock!.Object, _billRepositoryMock!.Object, _currencyManagerMock!.Object, _mapperMock!.Object);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => billService.CreateBillAsync(new CreateBillRequest()));
        }


        [TestMethod]
        public async Task GetBillAsync_BillNotFound_ThrowsKeyNotFoundException()
        {
            _billRepositoryMock!.Setup(x => x.GetBillByIdAsync(It.IsAny<int>(), It.IsAny<List<int>>())).ReturnsAsync((Bill)null!);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _billService!.GetBillAsync(1));
        }

        [TestMethod]
        public async Task SoftDeleteBillAsync_CallsRepositorySoftDeleteMethod()
        {
            const int billId = 1;
            var fakeBill = new Bill { Id = billId };

            _billRepositoryMock!.Setup(repo => repo.GetBillByIdAsync(billId, It.IsAny<List<int>>())).ReturnsAsync(fakeBill);

            await _billService!.SoftDeleteBillAsync(billId);

            _billRepositoryMock.Verify(repo => repo.SoftDeleteBillAsync(billId), Times.Once);
        }


        [DataTestMethod]
        [DataRow(1, true, DisplayName = "Bill exists and is successfully soft deleted")]
        [DataRow(2, false, DisplayName = "Bill does not exist and throws KeyNotFoundException")]
        public async Task SoftDeleteBillAsync_TestDifferentScenarios(int billId, bool billExists)
        {
            if (billExists)
            {
                _billRepositoryMock!.Setup(repo => repo.GetBillByIdAsync(billId, It.IsAny<List<int>>())).ReturnsAsync(new Bill { Id = billId });
                _billRepositoryMock.Setup(repo => repo.SoftDeleteBillAsync(billId)).Returns(Task.CompletedTask);
            }
            else
            {
                _billRepositoryMock!.Setup(repo => repo.GetBillByIdAsync(billId, It.IsAny<List<int>>())).ReturnsAsync((Bill)null!);
            }
            
            if (billExists)
            {
                await _billService!.SoftDeleteBillAsync(billId);
                _billRepositoryMock.Verify(repo => repo.SoftDeleteBillAsync(billId), Times.Once, "The bill should be soft deleted exactly once.");
            }
            else
            {
                await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _billService!.SoftDeleteBillAsync(billId), "Should throw KeyNotFoundException for non-existing bill.");
            }
        }

    }
}