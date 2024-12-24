namespace ShoppingCentre
{
    public class Wallet // Instances of this class will be serialized into a wallet json file where we keep the information about the users money.
    {
        public long Id { get; set; }
        public double MoneyInWallet { get; set; }
        public int TimesInPrison { get; set; }

        public Wallet(long id, double moneyInWallet, int timesInPrison)
        {
            Id = id;
            MoneyInWallet = moneyInWallet;
            TimesInPrison = timesInPrison;

        }
    }
}
