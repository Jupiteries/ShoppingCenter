using System.Collections.Generic;

namespace ShoppingCentre
{
    public class Check // Instances of this class will be serialized into a json file where we keep the checks and read them out to the client.
    {
        public int GUID { get; set; }
        public double FinalPricePlusTaxes { get; set; }
        public string CashierName { get; set; }
        public string CashierSurname { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public long ClientID { get; set; }
        public List<Product> ChosenProducts { get; set; }

        public Check(int guid, double finalPricePlusTaxes, string cashierName, string cashierSurname, string clientFirstName, string clientLastName, long clientId, List<Product> chosenProducts)
        {
            GUID = guid;
            FinalPricePlusTaxes = finalPricePlusTaxes;
            CashierName = cashierName;
            CashierSurname = cashierSurname;
            ClientFirstName = clientFirstName;
            ClientLastName = clientLastName;
            ClientID = clientId;
            ChosenProducts = chosenProducts;
        }

    }
}
