using System.IO;

namespace ShoppingCentre
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (FileStream overallBlackList = new FileStream("overallblacklist.json", FileMode.OpenOrCreate));
            Client client = new Client();

            Shopping.Welcome(client);



        }
    }
}
