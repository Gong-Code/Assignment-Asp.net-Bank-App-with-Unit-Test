namespace BankStartWeb.Services
{
    public interface IAccountService
    {
        ErrorCode MakeDeposit(int accountId, decimal amount);
        ErrorCode MakeWithdrawal(int accountId, decimal amount, string type);
        public enum ErrorCode
        {
            ok,
            BalanceIsToLow,
            AmountIsNegative
        }


    }
}
