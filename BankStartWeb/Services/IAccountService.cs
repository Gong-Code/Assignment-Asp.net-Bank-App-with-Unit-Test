namespace BankStartWeb.Services
{
    public interface IAccountService
    {
        ErrorCode MakeDeposit(int accountId, decimal amount);
        ErrorCode MakeWithdrawal(int accountId, decimal amount);
        ErrorCode Transfer(int accountId, int receiverId, decimal amount);

        
        public enum ErrorCode
        {
            ok,
            BalanceIsToLow,
            AmountIsNegative,
            InSufficientFunds
        }
    }
}
