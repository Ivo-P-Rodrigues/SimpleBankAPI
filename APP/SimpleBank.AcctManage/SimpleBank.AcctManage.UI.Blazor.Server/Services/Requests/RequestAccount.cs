namespace SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests
{
    /// <summary> Request obj to create a new Account. </summary>
    public class RequestAccountCreate
    {
        /// <summary> The amount of money the Account starts with. </summary>
        public decimal Balance { get; set; }

        /// <summary>  The Currency of the money in the Account. </summary>
        public string Currency { get; set; }
    }








}
