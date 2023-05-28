

namespace ConsoleShop100percentLegitNoScam.Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isLoggedIn = false;
            int userid = 0;
            string[] myAcc = new string[] { "Login", "Register new account", "Edit account", "Back" };
            string[] hub = new string[] { "Search", "My account", "My listings", "Chat" };
            string[] meinListings = new string[] { "edit/delete listing", "Create listing", "Back" };

            string[] searchui = { "Search Listing", "Search User", "Filters", "Back" };
            while (true)
            {
                switch (Other.cantThinkOfANameRn(hub, userid))
                {
                    case 0:
                        switch (Other.cantThinkOfANameRn(searchui, userid))
                        {
                            case 0:
                                //search listing
                                Search.searchListing(userid);
                                break;
                            case 1:
                                //search user
                                Search.searchUser(userid);
                                break;
                            case 2:
                                //filters
                                Search.Filters(userid);
                                Console.ReadLine();
                                break;
                        }

                        break;

                    case 1:
                        switch (Other.cantThinkOfANameRn(myAcc, userid))
                        {
                            case 0:

                                userid = User.login(isLoggedIn);
                                if (userid != 0)
                                {
                                    Other.selectName(userid);
                                    Console.Write("click enter to return to main menu");
                                    Console.ReadLine();
                                    isLoggedIn = true;
                                }
                                break;

                            case 1:
                                userid = User.register();
                                isLoggedIn = true;
                                Other.selectName(userid);
                                Console.Write("click enter to return to main menu");
                                Console.ReadLine();
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                        }
                        break;

                    case 2:
                        if (isLoggedIn)
                            switch (Other.cantThinkOfANameRn(meinListings, userid))
                            {
                                case 0:
                                    Listing.myListings(userid);
                                    break;

                                case 1:
                                    Listing.addListing(userid);
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
                }
            }
        }
    }
}