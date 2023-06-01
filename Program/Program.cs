//2137 lines

using System.Reflection.Metadata.Ecma335;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Program
    {










        public static readonly string connectionString = "workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
        private static readonly string FileName = "boolValue.txt";
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
        private static readonly byte EncryptionKey = 0x7F;

        private static byte[] Encrypt(byte[] data)
        {
            byte[] encryptedData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                encryptedData[i] = (byte)(data[i] ^ EncryptionKey);
            }
            return encryptedData;
        }

        private static byte[] Decrypt(byte[] encryptedData)
        {
            return Encrypt(encryptedData);
        }

        public static void Write(int userId)
        {
            byte[] encryptedBytes = Encrypt(BitConverter.GetBytes(userId));
            string encryptedString = Convert.ToBase64String(encryptedBytes);
            File.WriteAllText(FilePath, encryptedString);
            Console.WriteLine($"User ID written to file: {FilePath}");
        }

        public static int Read()
        {
            if (File.Exists(FilePath))
            {
                string encryptedString = File.ReadAllText(FilePath);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
                byte[] decryptedBytes = Decrypt(encryptedBytes);
                int userId = BitConverter.ToInt32(decryptedBytes, 0);
                return userId;
            }
            return 0;
        }


        static void Main(string[] args) 
        { 

            bool isLoggedIn = false;
            int userid = 0;
            string[] myAcc = new string[] { "Login", "Register new account","Log out", "Edit account","Delete account","View account data","My cart","Back" };
            string[] hub = new string[] { "Search", "My account", "My listings", "Chat", "Exit" };
            string[] meinListings = new string[] { "Listing actions", "Create listing","Sales", "Back" };
            string[] searchui = { "Search Listing", "Search User", "Filters", "Back" };
            userid = Read();
            if (userid != 0) { isLoggedIn = true; }
            while (true)
            {
                switch (Other.cantThinkOfANameRn(hub, userid,"Main menu"))
                {
                    case 0:
                        switch (Other.cantThinkOfANameRn(searchui, userid,"Search menu"))
                        {
                            case 0:
                                //search listing
                                Search.searchListing(userid, isLoggedIn);
                                break;
                            case 1:
                                //search user
                                Search.searchUser(userid, isLoggedIn);
                                break;
                            case 2:
                                //filters
                                Search.Filters(userid, isLoggedIn);
                                Console.ReadLine();
                                break;
                        }

                        break;

                    case 1:
                        string name;
                        if (userid == 0)
                            name = "My account";
                        else
                            name = Other.selectName(userid);
                        switch (Other.cantThinkOfANameRn(myAcc, userid, name))
                        {
                            case 0:

                                userid = User.login(isLoggedIn);
                                if (userid != 0)
                                {
                                    Console.Write("click enter to return to main menu");
                                    Console.ReadLine();
                                    isLoggedIn = true;
                                }
                                break;

                            case 1:
                                userid = User.register();
                                isLoggedIn = true;
                                Console.WriteLine(Other.selectName(userid));
                                Console.Write("click enter to return to main menu");
                                Console.ReadLine();
                                break;
                            case 2:
                                userid = 0;
                                break;
                            case 3:
                                User.EditAccount(userid);
                                break;
                            case 4:

                                break;
                            case 5:
                                User.dUD(userid, userid);
                                break;
                            case 6:
                                Listing.cart(userid, isLoggedIn);
                                break;
                            case 7:
                                break;
                        }
                        break;
                    case 2:
                        if (isLoggedIn)
                            switch (Other.cantThinkOfANameRn(meinListings, userid, "Listings action menu"))
                            {
                                case 0:
                                    Listing.myListings(userid);
                                    break;

                                case 1:
                                    Listing.addListing(userid);
                                    break;
                                case 2:
                                    Listing.Sales(userid);
                                    break;

                            }

                        else
                        {
                            Console.WriteLine("Please log in first");
                            Console.Write("click enter to return to main menu");
                            Console.ReadLine();
                        }
                        break;
                    case 3:
                        if (isLoggedIn)
                            Chat.chat(userid);
                        else
                        {
                            Console.WriteLine("Please log in first");
                            Console.Write("click enter to return to main menu");
                            Console.ReadLine();
                        }
                        break;
                     case 4:
                        Write(userid);
                        return;
                }
            }
        }
    }
}
