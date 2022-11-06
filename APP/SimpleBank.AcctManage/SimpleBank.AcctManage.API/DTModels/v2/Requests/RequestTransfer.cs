namespace SimpleBank.AcctManage.API.DTModels.v2.Requests
{


    /// <summary> Request obj to make a new Transfer. </summary>
    public class RequestTransferCreate
    {
        /// <summary> Account Id from where transfer withdraws funds. </summary>
        public Guid FromAccountId { get; set; }

        /// <summary> Account Id to where transfer amount is sent. </summary>
        public Guid ToAccountId { get; set; }

        /// <summary> Amount to transfer between accounts. </summary>
        public decimal Amount { get; set; }
    }






}
