using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Listing
    {
        public static void editListing(int uid)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();


            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select id, title, price from [listings] where user_id = {0}", uid);
            SqlDataReader reader2 = searchcomm.ExecuteReader();

            List<string> name = new List<string>();
            List<int> id = new List<int>();

            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    string fullName = "Title: " + reader2[1] + "\nPrice: " + reader2[2] + "\n";
                    int userId = (int)reader2[0];

                    if (!name.Contains(fullName)) // Check if the name already exists in the list
                    {
                        name.Add(fullName);
                        id.Add(userId);
                    }
                }
            }
            reader2.Close();
            string price, quantity, newtitle, description;
            int lid = id[Other.cantThinkOfANameRn(name.ToArray(), uid, "Your listings")];
            Console.Write("New title:");
            newtitle = Console.ReadLine();
            newtitle = Other.lenght(50, 2, newtitle, "New title", false);
            Console.Write("Price:");
            price = Console.ReadLine();
            price = Other.lenght(7, 1, price, "Price", true);
            Console.Write("Quantity:");
            quantity = Console.ReadLine();
            quantity = Other.lenght(3, 1, quantity, "Quantity", true);
            Console.Write("Description:");
            description = Console.ReadLine();
            description = Other.lenght(500, 0, description, "Description", false);

            string[] categorytab = new string[] {
        "toys",
        "digital services",
        "cosmetics and body care",
        "food and beverage",
        "health and wellness",
        "household items",
        "media",
        "pet care",
        "office equipment"};

            SqlCommand editListing;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = string.Format("UPDATE Listings SET Category_id = '{0}', Title = '{1}', Price = {2}, Quantity = {3}, Description = '{4}' WHERE id = '{5}';",Other.cantThinkOfANameRn(categorytab, uid,"Choose a category"), newtitle, price, quantity, description, lid);
            Console.WriteLine(sql);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        public static void delListing(int uid)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select id, title, price from [listings] where user_id = {0}",uid);
            SqlDataReader reader2 = searchcomm.ExecuteReader();

            List<string> name = new List<string>();
            List<int> id = new List<int>();

            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    string fullName = "Title: " + reader2[1] + "\nPrice: " + reader2[2] + "\n";
                    int userId = (int)reader2[0];

                    if (!name.Contains(fullName)) // Check if the name already exists in the list
                    {
                        name.Add(fullName);
                        id.Add(userId);
                    }
                }
                string sql2 = string.Format("delete from listings where andid = '{0}'", id[Other.cantThinkOfANameRn(name.ToArray(), uid,"Your listings")]);
                SqlCommand delListing;
                SqlDataAdapter adapter = new SqlDataAdapter();
                delListing = new SqlCommand(sql2, conn);
                adapter.InsertCommand = new SqlCommand(sql2, conn);
                adapter.InsertCommand.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                Console.WriteLine("no resulte\nClick enter to return to main menu");
                Console.Read();
            }



            

        }

        public static void myListings(int id)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();



            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select title, price, views from [listings] where user_id = '{0}'", id);


            //liczba kolumn
            SqlDataReader reader2 = searchcomm.ExecuteReader();
            Console.WriteLine("Your listings:");
            Console.WriteLine();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    Console.Write("title:");
                    Console.WriteLine(reader2[0]);
                    Console.Write("price:");
                    Console.WriteLine(reader2[1]);
                    Console.Write("views:");
                    Console.WriteLine(reader2[2]);


                }
                Console.WriteLine();
                Console.WriteLine("Click enter to open action menu");
                Console.ReadLine();


                string[] y = { "Delete", "Edit", "Main menu" };
                switch (Other.cantThinkOfANameRn(y, id,"Listing actions"))
                {
                    case 0:
                        delListing(id);
                        break;
                    case 1:
                        editListing(id);
                        break;
                    case 2:
                        break;
                }
            }
            else
            {
                Console.WriteLine("No results");
                Console.Write("click enter to return to main menu");
                Console.ReadLine();
            }
            conn.Close();

        }

        public static void addListing(int userid)
        {
            string price, quantity, title, description;
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();

            Console.Write("Title:");
            title = Console.ReadLine();
            title = Other.lenght(50, 2, title, "Title", false);
            Console.Write("Price:");
            price = Console.ReadLine();
            price = Other.lenght(7, 1, price, "Price", true);
            price = price.Replace(',', '.');
            Console.Write("Quantity:");
            quantity = Console.ReadLine();
            quantity = Other.lenght(3, 1, quantity, "Quantity", true);
            Console.Write("Description:");
            description = Console.ReadLine();
            description = Other.lenght(500, 0, description, "Description", false);
            string[] categorytab = new string[] {
                "toys",
                "digital services",
                "cosmetics and body care",
                "food and beverage",
                "health and wellness",
                "household items",
                "media",
                "pet care",
                "office equipment"};



            SqlCommand addlisting;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = string.Format("insert into[listings] (title, price, views, user_id, category_id, quantity, description) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", title, price, 0, userid, Other.cantThinkOfANameRn(categorytab, userid, "Choose category") + 1, quantity, description);
            addlisting = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        public static void dLD(int lid, int uid, bool loggedin)
        {//dLD - display Listing Data
            while (true) 
            {
                string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();


                SqlCommand searchcomm = new SqlCommand();
                searchcomm.Connection = conn;
                searchcomm.CommandText = string.Format("SELECT[user].[first_name], [user].[last_name], [listings].[price], [listings].[title], [listings].[description], [user].[phone_number], [listings].[user_id], [reviews].[content] FROM[user] INNER JOIN[listings] ON[user].[id] = [listings].[user_id] LEFT JOIN[reviews] ON[reviews].[listing_id] = [listings].[id] WHERE[listings].[id] = '{0}'", lid);
                double price = 0;

                //liczba kolumn
                SqlDataReader reader2 = searchcomm.ExecuteReader();


                string[] authordetails = { "Check author profile", "Post a review","Add to cart","Order now", "Main menu" };

                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        Console.Clear();
                        Console.WriteLine(reader2[3]);
                        Console.WriteLine(reader2[2] + "zł");
                        price = (double)reader2[2];
                        Console.WriteLine();
                        Console.WriteLine(reader2[0] + " " + reader2[1]);
                        Console.WriteLine("Phone number: " + reader2[5]);
                        Console.WriteLine();
                        Console.WriteLine(reader2[4]);
                        Other.viewsCounter(lid);





                    };
                }
                else
                    Console.WriteLine("No results");


                SqlCommand searchReviewCommand = new SqlCommand();
                searchReviewCommand.Connection = conn;
                searchReviewCommand.CommandText = string.Format("SELECT [reviews].[user_id], [reviews].[content], [user].[first_name], [user].[last_name] FROM [reviews] INNER JOIN [user] ON [reviews].[user_id] = [user].[id] WHERE [reviews].[listing_id] = '{0}'", lid);
                reader2.Close();
                SqlDataReader reviewReader = searchReviewCommand.ExecuteReader();

                if (reviewReader.HasRows)
                {
                    Console.WriteLine("\n-----------------------");
                    while (reviewReader.Read())
                    {
                        int userId = (int)reviewReader["user_id"];
                        string content = (string)reviewReader["content"];
                        string userFirstName = (string)reviewReader["first_name"];
                        string userLastName = (string)reviewReader["last_name"];

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{userFirstName} {userLastName}");
                        Console.ResetColor();
                        Console.WriteLine(content);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nNo reviews found");
                }
                reviewReader.Close();

                Console.WriteLine("Click enter to see author profile or return to main menu");
                Console.ReadLine();
                SqlDataReader reader3 = searchcomm.ExecuteReader();
                reader3.Read();
                int iud = (int)reader3[6];
                switch (Other.cantThinkOfANameRn(authordetails, uid, "Listing actions"))
                {
                    case 0:
                        User.dUD(iud, uid);
                        reader2.Close();
                        break;
                    case 1:
                        AddReview(uid, lid,loggedin);
                        break;
                    case 2:
                        //add to cart
                        if (!loggedin) { Console.WriteLine("Please log in first"); Thread.Sleep(1000); Console.Clear(); break; }
                        addtocart(uid,lid);
                        break;
                    case 3:
                        //order now
                        if (!loggedin) { Console.WriteLine("Please log in first"); Thread.Sleep(1000); Console.Clear(); break; }
                        order(uid, lid,price);
                        break;
                    case 4:
                        return;
                }


                conn.Close();
            }
            

        }
        public static void addtocart(int uid, int lid)
        {
            Console.Write("Quantity:");
            string quantity = Console.ReadLine();
            DateTime currentDateTime = DateTime.Now.Date; // Use Date property to remove the time component

            string connectionString = "workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string query = string.Format("insert into [cart] (user_id, listing_id, quantity, date_added) values ({0},{1},{2},'{3}')", uid, lid, quantity, currentDateTime.ToString("yyyy-MM-dd"));


            SqlCommand insertcartCommand = new SqlCommand(query, conn);
            insertcartCommand.ExecuteNonQuery();
        }
        public static void order(int uid, int lid, double priceW)
        {
            string price = priceW.ToString("0.00", CultureInfo.InvariantCulture);
            Console.Write("Quantity:");
            string quantity = Console.ReadLine();
            DateTime currentDateTime = DateTime.Now.Date; // Use Date property to remove the time component

            string connectionString = "workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string query = string.Format("INSERT INTO [order_history] (user_id, listing_id, quantity, price, date_ordered) VALUES ('{0}','{1}','{2}','{3}','{4}')", uid, lid, quantity, price, currentDateTime.ToString("yyyy-MM-dd"));
            Console.WriteLine(query);
            SqlCommand insertcartCommand = new SqlCommand(query, conn);
            insertcartCommand.ExecuteNonQuery();


        }
        public static void AddReview(int reviewerId, int revieweeId, bool loggedin)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();
            if (!loggedin) { Console.WriteLine("Please log in first"); Thread.Sleep(1000); Console.Clear(); return; }
            Console.WriteLine("Enter the review content:");
            string reviewContent = Console.ReadLine();

            string query = string.Format("INSERT INTO [reviews] (user_id, listing_id, content) VALUES ({0},{1},'{2}')", reviewerId, revieweeId, reviewContent);

            SqlCommand insertReviewCommand = new SqlCommand(query, conn);


            insertReviewCommand.ExecuteNonQuery();

            Console.WriteLine("Review added successfully.");

            conn.Close();
        }

    }
}
