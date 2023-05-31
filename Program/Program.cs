

using System.Reflection.Metadata.Ecma335;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Program
    {
        public static readonly string connectionString = "workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
        private static readonly string FileName = "boolValue.txt";
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);


        public static void Write(int userId)
        {
            File.WriteAllText(FilePath, userId.ToString());
            Console.WriteLine($"User ID written to file: {FilePath}");
        }

        public static int Read()
        {
            if (File.Exists(FilePath))
            {
                string content = File.ReadAllText(FilePath);
                if (int.TryParse(content, out int userId))
                {
                    return userId;
                }
                else { return 0; }
            }
            else { return 0; }
        }


        static void Main(string[] args)
        {
            bool isLoggedIn = false;
            int userid = 0;
            string[] myAcc = new string[] { "Login", "Register new account", "Edit account","My cart","Back" };
            string[] hub = new string[] { "Search", "My account", "My listings", "Chat", "Exit" };
            string[] meinListings = new string[] { "Listing actions", "Create listing","Sales", "Back" };
            string[] searchui = { "Search Listing", "Search User", "Filters", "Back" };
            userid = Read();

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
                                break;
                            case 3:
                                Listing.cart(userid, isLoggedIn);
                                break;
                            case 4:
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
