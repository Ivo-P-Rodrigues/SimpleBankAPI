using SimpleBankAPI.WebAPI;

namespace SimpleBankAPI.NUnitTests
{
    [TestFixture]
    public class AccountsBusinessTests 
    {
        
        private Mock<IUnitOfWork> _unitOfWork;
        private AccountBusiness _accountBusiness;


        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _accountBusiness = new AccountBusiness(_unitOfWork.Object) { };
        }

        #region GetAllUserAccounts
        [Test]
        public void GetAllUserAccounts_UserHasAccounts_ReturnsOk()
        {
            //arrange
            var userId = 1;
            _unitOfWork.Setup(un => un.Accounts.GetAllWhere(account => account.UserId == userId)).Returns(new[] { new Account() { UserId = userId } });

            //act
            var result = _accountBusiness.GetAllUserAccounts(userId);

            //assert
            Assert.That(result.Any(account => account.UserId == userId));
        }

        [Test]
        public void GetAllUserAccounts_UserHasNoAccounts_ReturnsNotFound()
        {
            //arrange
            var userId = 1;
            _unitOfWork.Setup(un => un.Accounts.GetAllWhere(account => account.UserId == userId)).Returns(new[] { new Account() {  } });

            //act
            var result = _accountBusiness.GetAllUserAccounts(userId);

            //assert
            Assert.That(result.Count(accountResp => accountResp.UserId == userId), Is.EqualTo(0));
        }
        #endregion


        #region GetAccountWithMovements
        [Test]
        public async Task GetAccountWithMovements_GetCorrectAccount_ReturnsTupleWithFalseBoolAccountAndMovements()
        {
            //arrange
            int accountId = 1;
            var userId = 1;
            Account? account = new Account() { AccountId = accountId, UserId = userId };
            AccountResponse accountRsp = new AccountResponse() { AccountId = accountId };
            IEnumerable<Movement> movements = new Movement[] { new Movement { AccountId = accountId } };
            ICollection<Movim> movims = new Movim[] { new Movim { } };

            _unitOfWork.Setup(un => un.Accounts.GetAsync(accountId)).ReturnsAsync(account);
            _unitOfWork.Setup(un => un.Movements.GetAllWhere(movement => movement.AccountId == accountId)).Returns(movements);

            //act
            var result = await _accountBusiness.GetAccountWithMovements(accountId, userId);

            //assert
            Assert.That(result.Item1, Is.False);
            Assert.That(result.Item2, Is.EqualTo(account));
            Assert.That(result.Item3, Is.EqualTo(movements));
        }

        [Test]
        public async Task GetAccountWithMovements_GetUnexistingAccount_ReturnsTupleWithFalseBoolNullAccountAndNullMovements()
        {
            //arrange
            int accountId = 1;
            var userId = 1;
            _unitOfWork.Setup(un => un.Accounts.GetAsync(accountId)).ReturnsAsync((Account?)null);

            //act
            var result = await _accountBusiness.GetAccountWithMovements(accountId, userId);

            //assert
            Assert.That(result.Item1, Is.False);
            Assert.That(result.Item2, Is.Null);
            Assert.That(result.Item3, Is.Null);
        }

        [Test]
        public async Task GetAccountWithMovements_GetUnauthorizedAccount_ReturnsTupleWithTrueBoolNullAccountAndNullMovements()
        {
            //arrange
            int accountId = 1;
            var userId = 1;
            Account? account = new Account() { AccountId = accountId, UserId = userId + 1 };
            _unitOfWork.Setup(un => un.Accounts.GetAsync(accountId)).ReturnsAsync(account);

            //act
            var result = await _accountBusiness.GetAccountWithMovements(accountId, userId);

            //assert
            Assert.That(result.Item1, Is.True);
            Assert.That(result.Item2, Is.Null);
            Assert.That(result.Item3, Is.Null);
        }
        #endregion


        #region CreateAccount
        [Test]
        public async Task CreateAccount_NormalProcedure_ReturnsNewAccount()
        {
            //arrange
            var account = new Account() { };
            var savedAccount = new Account() { };

            _unitOfWork.Setup(un => un.Accounts.DirectAddAsync(account)).ReturnsAsync(savedAccount);

            //act
            var result = await _accountBusiness.CreateAccount(account);

            //assert
            Assert.That(result, Is.EqualTo(savedAccount));
        }

        [Test]
        public async Task CreateAccount_ErrorOnSavingAccount_ReturnsNull()
        {
            //arrange
            var account = new Account() { };

            _unitOfWork.Setup(un => un.Accounts.DirectAddAsync(account)).ReturnsAsync((Account) null);

            //act
            var result = await _accountBusiness.CreateAccount(account);

            //assert
            Assert.That(result, Is.Null);
        }
        #endregion



    }
}



