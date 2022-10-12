using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;

namespace SimpleBankAPI.NUnitTests
{
    [TestFixture]
    public class TransferBusinessTestsTwo
    {

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ILogger<TransferBusiness>> _logger;
        private TransferBusiness _transferBusiness;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<TransferBusiness>>(); //no setup on logger
            _transferBusiness = new TransferBusiness(_unitOfWork.Object, _logger.Object) { };

        }


        [Test]
        public async Task MakeTransfer_NormalTransfer_ReturnsOk()
        {
            //arrange
            var userId = 1;
            var accountFromUserId = userId;

            var accountFromId = 1;
            var accountFromBalance = 100;
            var accountFromCurrency = "EUR";
            var accountFrom = new Account() { AccountId = accountFromId, UserId = accountFromUserId, Balance = accountFromBalance, Currency = accountFromCurrency };

            var accountToId = 2;
            var accountToCurrency = "EUR";
            var accountTo = new Account() { AccountId = accountToId, Currency = accountToCurrency };

            var amount = 50;
            var transfer = new Transfer() { FromAccountId = accountFromId, ToAccountId = accountToId, Amount = amount };


            _unitOfWork.Setup(un => un.Accounts.GetAsync(transfer.FromAccountId)).ReturnsAsync(accountFrom);
            _unitOfWork.Setup(un => un.Accounts.GetAsync(transfer.ToAccountId)).ReturnsAsync(accountTo);

            _unitOfWork.Setup(un => un.Transfers.Add(transfer));
            _unitOfWork.Setup(un => un.Movements.AddRange(new[] { new Movement() }));

            //act
            var result = await _transferBusiness.MakeTransfer(transfer, userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.False);
                Assert.That(result.Item2, Is.False);
                Assert.That(result.Item3, Is.Not.Null);
            });
        }









    }
}



