namespace ShoppingCentre
{
    public class Client // Clients basic information
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Id { get; set; }
        public int TimesInPrison { get; set; } // How many times they have been in prison. The default is 0.
    }
}
