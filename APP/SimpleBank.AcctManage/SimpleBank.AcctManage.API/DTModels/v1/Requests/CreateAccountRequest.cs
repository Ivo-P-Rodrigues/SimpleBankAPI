namespace SimpleBank.AcctManage.API.DTModels.v1.Requests
{
    /// <summary>
    /// CreateAccountRequest - obj. with needed details for a new Account.
    /// </summary>
    public class CreateAccountRequest
    {
        /// <summary>
        /// The Amount of money the Account starts with. 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The Currency of the money in the Account.
        /// </summary>
        public string Currency { get; set; }

    }
}
