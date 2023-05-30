using ConsoleShop100percentLegitNoScam.Program;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

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

        public static void Sales(int uid)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string selectQuery = string.Format("SELECT oh.id, oh.user_id, u.first_name, u.last_name, l.title, oh.quantity, oh.price, oh.date_ordered FROM order_history oh JOIN [user] u ON oh.user_id = u.id JOIN listings l ON oh.listing_id = l.id WHERE oh.confirmed = 0 and oh.user_id = {0}", uid);

                SqlCommand selectCommand = new SqlCommand(selectQuery, conn);
                SqlDataReader reader = selectCommand.ExecuteReader();

                List<string> name = new List<string>();
                List<int> id = new List<int>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string fullName = reader[5] + "\n" + reader[7]+"zł\n"+ reader[1] +"  " + reader[2]+"\nx"+reader[6]+"\nordered on:"+reader[8];
                        int userId = (int)reader[0];
                        name.Add(fullName);
                        id.Add(userId);
                    }
                }
                else
                {
                    Console.WriteLine("No awaiting orders found.");
                }
                name.Add("Back");
                reader.Close();
                int oid = -1;
                while (oid != name.Count - 1)
                {
                    oid = Other.cantThinkOfANameRn(name.ToArray(), uid, "Sales");

                    if (oid >= 0 && oid < name.Count - 1)
                    {
                        if (name[oid] == "Back")
                        {
                            break; // Break the loop if "Back" option is selected
                        }

                        string updateQuery = string.Format("UPDATE order_history SET confirmed = 1 WHERE id = {0}", id[oid]);
                        SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
                        updateCommand.ExecuteNonQuery();

                        Console.WriteLine("Order confirmed successfully.");
                    }
                }


            }
        }

        public static void cart(int uid, bool loggedin)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string selectQuery = string.Format("SELECT cart.id, listings.title, listings.price, cart.quantity, SUM(listings.price * cart.quantity) AS TotalPrice, listings.id AS listings_id FROM cart JOIN listings ON cart.listing_id = listings.id WHERE cart.user_id = {0} GROUP BY cart.id, listings.id, listings.title, listings.price, cart.quantity; SELECT SUM(subquery.TotalPrice) AS CartTotal FROM( SELECT SUM(listings.price * cart.quantity) AS TotalPrice FROM cart JOIN listings ON cart.listing_id = listings.id WHERE cart.user_id = {0} GROUP BY cart.user_id) AS subquery;", uid);

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, conn))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        double totalprice = 0;
                        string stryng;
                        int lid=0;
                        List<string> name = new List<string>(); // Declare the 'name' list outside the while loop
                        List<int> id = new List<int>(); // Declare the 'id' list outside the while loop

                        while (reader.Read())
                        {
                            lid = (int)reader["listings_id"];

                            string fullName = reader["title"] + "x" + reader["quantity"] + "\n" + reader["TotalPrice"] + "zł";
                            int cartId = (int)reader["id"];
                            name.Add(fullName);
                            id.Add(cartId);
                        }

                        if (name.Count > 0) // Check if the 'name' list is not empty
                        {
                            reader.NextResult();

                            while (reader.Read())
                            {
                                totalprice = (double)reader["CartTotal"];
                            }
                            stryng = "Total price:" + totalprice;

                            name.Add("Back");

                            int cid = Other.cantThinkOfANameRn(name.ToArray(), uid, stryng);
                            if (cid == name.Count - 1)
                                return;

                            string[] cartoptions = { "View details", "Delete from cart", "Edit quantity", "Back" };
                            int selectedAction = Other.cantThinkOfANameRn(cartoptions, uid, "Action menu");

                            switch (selectedAction)
                            {
                                case 0:
                                    dLD(id[lid], uid, loggedin);
                                    break;
                                case 1:
                                    string deleteQuery = string.Format("DELETE FROM cart WHERE id = '{0}'", id[cid]);
                                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn))
                                    {
                                        deleteCommand.ExecuteNonQuery();
                                    }
                                    break;
                                case 2:
                                    Console.Write("Quantity:");
                                    string quantity = Console.ReadLine();
                                    string updateCartQuery = string.Format("UPDATE cart SET quantity={1} WHERE id = {0}", id[cid], quantity);
                                    using (SqlCommand updateCartCommand = new SqlCommand(updateCartQuery, conn))
                                    {
                                        updateCartCommand.ExecuteNonQuery();
                                    }
                                    break;
                                case 3:
                                    return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("No items in the cart.");
                        }
                    }
                }
            }


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
                string sql2 = string.Format("delete from listings where id = '{0}'", id[Other.cantThinkOfANameRn(name.ToArray(), uid,"Your listings")]);
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
                searchcomm.CommandText = string.Format("SELECT[user].[first_name], [user].[last_name], [listings].[price], [listings].[title], [listings].[description], [user].[phone_number], [listings].[user_id], [reviews].[content], listings.quantity FROM[user] INNER JOIN[listings] ON[user].[id] = [listings].[user_id] LEFT JOIN[reviews] ON[reviews].[listing_id] = [listings].[id] WHERE[listings].[id] = '{0}'", lid);
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
                        Console.WriteLine(reader2[8] + "in stock");
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
                {
                    Console.WriteLine(searchcomm.CommandText);
                    Console.WriteLine("No results");
                }




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
            string quantityStr = Console.ReadLine();
            if (!int.TryParse(quantityStr, out int quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a valid number.");
                return;
            }

            // Retrieve the available quantity from the database
            string connectionString = "workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string availableQuantityQuery = string.Format("SELECT quantity FROM listings WHERE id = {0}", lid);
                SqlCommand availableQuantityCommand = new SqlCommand(availableQuantityQuery, conn);
                int availableQuantity = (int)availableQuantityCommand.ExecuteScalar();

                if (quantity > availableQuantity)
                {
                    Console.WriteLine("Quantity exceeds available stock. Please enter a lower quantity.");
                    return;
                }

                DateTime currentDateTime = DateTime.Now.Date; // Use Date property to remove the time component

                string query = string.Format("INSERT INTO [cart] (user_id, listing_id, quantity, date_added) VALUES ({0}, {1}, {2}, '{3}')", uid, lid, quantity, currentDateTime.ToString("yyyy-MM-dd"));

                SqlCommand insertcartCommand = new SqlCommand(query, conn);
                insertcartCommand.ExecuteNonQuery();
                Console.WriteLine("Item added to cart successfully.");
            }
        }

        public static void order(int uid, int lid, double priceW)
        {
            Console.Write("Quantity:");
            string quantityStr = Console.ReadLine();
            if (!int.TryParse(quantityStr, out int quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a valid number.");
                return;
            }

            // Retrieve the available quantity from the database
            string connectionString = "workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string availableQuantityQuery = string.Format("SELECT quantity FROM listings WHERE id = {0}", lid);
                SqlCommand availableQuantityCommand = new SqlCommand(availableQuantityQuery, conn);
                int availableQuantity = (int)availableQuantityCommand.ExecuteScalar();

                if (quantity > availableQuantity)
                {
                    Console.WriteLine("Quantity exceeds available stock. Please enter a lower quantity.");
                    return;
                }

                string price = priceW.ToString("0.00", CultureInfo.InvariantCulture);
                DateTime currentDateTime = DateTime.Now; // Get the current date and time

                string query = string.Format("INSERT INTO [order_history] (user_id, listing_id, quantity, price, date_ordered, confirmed) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{0}')", uid, lid, quantity, price, currentDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                SqlCommand insertcartCommand = new SqlCommand(query, conn);
                insertcartCommand.ExecuteNonQuery();
                Console.WriteLine("Order placed successfully.");
                Thread.Sleep(1000);
            }
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
