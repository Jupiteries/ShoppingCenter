namespace ShoppingCentre
{
    public class Blacklists // Instances of this class will be serialized into a json file where we keep the list of blacklisted users
    {
        public long Id { get; set; }

        public Blacklists(long id)
        {
            Id = id;
        }
    }
}
