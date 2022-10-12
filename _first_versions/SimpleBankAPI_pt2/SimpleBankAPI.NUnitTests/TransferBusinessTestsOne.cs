using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.NUnitTests
{
    [TestFixture]
    public class TransferBusinessTestsOne
    {

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ILogger<TransferBusiness>> _logger;
        private TransferBusiness _transferBusiness;

        private Account _accountFrom;
        private Account _accountTo;
        private Transfer _transfer;

        private int _userId;


        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<TransferBusiness>>(); //no setup on logger
            _transferBusiness = new TransferBusiness(_unitOfWork.Object, _logger.Object) { };

            //normal situation - each test adjusts objs state accordingly
            _userId = 1;
            _accountFrom = new Account() { AccountId = 1, UserId = _userId, Balance = 100 };
            _accountTo = new Account() { AccountId = 2 };
            _transfer = new Transfer() { FromAccountId = _accountFrom.AccountId, ToAccountId = _accountTo.AccountId, Amount = 50 };

            _unitOfWork.Setup(un => un.Accounts.GetAsync(_transfer.FromAccountId)).ReturnsAsync(_accountFrom);
            _unitOfWork.Setup(un => un.Accounts.GetAsync(_transfer.ToAccountId)).ReturnsAsync(_accountTo);

            _unitOfWork.Setup(un => un.Transfers.Add(_transfer));
            _unitOfWork.Setup(un => un.Movements.AddRange(new[] { new Movement() }));
        }



        [Test]
        public async Task MakeTransfer_NormalTransfer_ReturnsOk()
        {
            //arrange -> setup

            //act
            var result = await _transferBusiness.MakeTransfer(_transfer, _userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.False);
                Assert.That(result.Item2, Is.False);
                Assert.That(result.Item3, Is.Not.Null);
            });
        }

        [Test]
        public async Task MakeTransfer_DiferentCurrencies_ReturnsFalseTrueNull()
        {
            //arrange
            _accountTo.Currency = "DOL";

            //act
            var result = await _transferBusiness.MakeTransfer(_transfer, _userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.False);
                Assert.That(result.Item2, Is.True);
                Assert.That(result.Item3, Is.Null);
            });
        }


        [Test]
        public async Task MakeTransfer_TransferToSameAccount_ReturnsFalseTrueNull()
        {
            //arrange
            _transfer.FromAccountId = _transfer.ToAccountId;

            //act
            var result = await _transferBusiness.MakeTransfer(_transfer, _userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.False);
                Assert.That(result.Item2, Is.True);
                Assert.That(result.Item3, Is.Null);
            });
        }

        [Test]
        public async Task MakeTransfer_NotEnoughBalanceInAccount_ReturnsFalseTrueNull()
        {
            //arrange
            _transfer.Amount = 2000;

            //act
            var result = await _transferBusiness.MakeTransfer(_transfer, _userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.False);
                Assert.That(result.Item2, Is.True);
                Assert.That(result.Item3, Is.Null);
            });
        }


        [Test]
        public async Task MakeTransfer_UserDoesntOwnAccountFrom_ReturnsTrueFalseNull()
        {
            //arrange
            _accountFrom.UserId++;

            //act
            var result = await _transferBusiness.MakeTransfer(_transfer, _userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.True);
                Assert.That(result.Item2, Is.False);
                Assert.That(result.Item3, Is.Null);
            });
        }








    }
}



