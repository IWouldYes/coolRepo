using ConsoleShop100percentLegitNoScam.Program;
using Microsoft.VisualBasic.FileIO;
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
        public static int czos(int uid)
        {
            string connectionString = @Program.connectionString;
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

            return id[Other.cantThinkOfANameRn(name.ToArray(), uid, "Your listings")];
        }
        public static void editListing(int uid)
        {
            int lid = czos(uid);
            SqlConnection conn = new SqlConnection();
            string price, quantity, newtitle, description;
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
            string connectionString = @Program.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string selectQuery = string.Format("SELECT oh.id, oh.user_id, u.first_name, u.last_name, l.title, oh.quantity, oh.price, oh.date_ordered, l.id AS listing_id FROM order_history oh JOIN [user] u ON oh.user_id = u.id JOIN listings l ON oh.listing_id = l.id WHERE oh.confirmed = 0 and l.user_id = {0}", uid);

            SqlCommand selectCommand = new SqlCommand(selectQuery, conn);
            SqlDataReader reader = selectCommand.ExecuteReader();

            List<string> name = new List<string>();
            List<int> id = new List<int>();
            List<int> listingIds = new List<int>();
            List<int> selerid = new List<int>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string fullName = reader["title"] + "\n" + reader["price"] + "zł\n" + reader["first_name"] + " " + reader["last_name"] + "\nx" + reader["quantity"]+"\n";
                    int orderId = (int)reader["id"];
                    int listingId = (int)reader["listing_id"];
                    selerid.Add((int)reader[1]);
                    name.Add(fullName);
                    id.Add(orderId);
                    listingIds.Add(listingId);
                }
            }
            else
            {
                Console.WriteLine("No awaiting orders found.");
            }
            reader.Close();

            name.Add("Back");
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



                    Console.WriteLine(); // Add an empty line for spacing

                    string[] actionMenuOptions = { "View user details", "Confirm order", "View listing details", "Back" };
                    int selectedAction = Other.cantThinkOfANameRn(actionMenuOptions, uid, "Action Menu");

                    switch (selectedAction)
                    {
                        case 0:
                            int userId = selerid[oid];
                            User.dUD(userId, uid);
                            break;
                        case 1:
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine("Order confirmed successfully.");
                            Thread.Sleep(1000);

                            break;
                        case 2:
                            int listingId = listingIds[oid];
                            dLD(listingId, uid, false);
                            break;
                        case 3:
                            // Go back to the previous menu
                            break;
                    }
                }
            }

            conn.Close();
        }




        public static void cart(int uid, bool loggedin)
        {
            string connectionString = @Program.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string selectQuery = string.Format("SELECT cart.id, cart.listing_id, listings.title, listings.price, cart.quantity, SUM(listings.price * cart.quantity) AS TotalPrice, listings.id AS listings_id FROM cart JOIN listings ON cart.listing_id = listings.id WHERE cart.user_id = {0} GROUP BY cart.id, cart.listing_id, listings.id, listings.title, listings.price, cart.quantity; SELECT SUM(subquery.TotalPrice) AS CartTotal FROM( SELECT SUM(listings.price * cart.quantity) AS TotalPrice FROM cart JOIN listings ON cart.listing_id = listings.id WHERE cart.user_id = {0} GROUP BY cart.user_id) AS subquery;", uid);

            SqlCommand selectCommand = new SqlCommand(selectQuery, conn);
            SqlDataReader reader = selectCommand.ExecuteReader();

            double totalprice = 0;
            string stryng;
            int lid = 0;
            List<string> name = new List<string>(); // Declare the 'name' list outside the while loop
            List<int> id = new List<int>(); // Declare the 'id' list outside the while loop

            while (reader.Read())
            {
                lid = (int)reader["listings_id"];

                string fullName = reader["title"] + "x" + reader["quantity"] + "\n";
                if (reader["TotalPrice"] != DBNull.Value)
                {
                    double totalPrice = (double)reader["TotalPrice"];
                    fullName += totalPrice.ToString("0.00") + "zł\n";
                }
                else
                {
                    fullName += "Price Unavailable\n";
                }

                int cartId = (int)reader["id"];
                name.Add(fullName);
                id.Add(cartId);
            }

            name.Add("Back");
            if (name.Count > 0) // Check if the 'name' list is not empty
            {
                reader.NextResult();

                if (reader.Read() && reader["CartTotal"] != DBNull.Value)
                {
                    totalprice = (double)reader["CartTotal"];
                }
                stryng = "Total price:" + totalprice.ToString("0.00");

                int cid = Other.cantThinkOfANameRn(name.ToArray(), uid, stryng);
                if (cid == name.Count - 1)
                {
                    conn.Close();
                    return;
                }

                string[] cartoptions = { "View details", "Delete from cart", "Edit quantity", "Back" };
                int selectedAction = Other.cantThinkOfANameRn(cartoptions, uid, "Action menu");
                reader.Close();
                switch (selectedAction)
                {
                    case 0:
                        if (cid >= 0 && cid < id.Count)
                        {
                            int cartId = id[cid];
                            string listingIdQuery = string.Format("SELECT listing_id FROM cart WHERE id = {0}", cartId);
                            SqlCommand listingIdCommand = new SqlCommand(listingIdQuery, conn);
                            int listingId = (int)listingIdCommand.ExecuteScalar();
                            conn.Close();
                            dLD(listingId, uid, loggedin);
                        }
                        break;
                    case 1:
                        string deleteQuery = string.Format("DELETE FROM cart WHERE id = '{0}'", id[cid]);
                        SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn);
                        deleteCommand.ExecuteNonQuery();
                        conn.Close();
                        break;
                    case 2:
                        Console.Write("Quantity:");
                        string quantity = Console.ReadLine();
                        string updateCartQuery = string.Format("UPDATE cart SET quantity={1} WHERE id = {0}", id[cid], quantity);
                        SqlCommand updateCartCommand = new SqlCommand(updateCartQuery, conn);
                        updateCartCommand.ExecuteNonQuery();
                        conn.Close();
                        break;
                    case 3:
                        conn.Close();
                        return;
                }
            }
            else
            {
                Console.WriteLine("No items in the cart.");
                conn.Close();
            }
        }




        public static void OrderWholeCart(int uid)
        {
            string connectionString = Program.connectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string selectQuery = string.Format("SELECT listing_id, quantity FROM cart WHERE user_id = {0}", uid);
                SqlCommand selectCommand = new SqlCommand(selectQuery, conn);
                SqlDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    int listingId = (int)reader["listing_id"];
                    int quantity = (int)reader["quantity"];
                    double price = GetListingPrice(listingId);
                    PlaceOrder(uid, listingId, quantity, price, conn);
                }

                reader.Close();
                conn.Close();
            }
        }

        public static double GetListingPrice(int listingId)
        {
            string connectionString = Program.connectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string priceQuery = string.Format("SELECT price FROM listings WHERE id = {0}", listingId);
                SqlCommand priceCommand = new SqlCommand(priceQuery, conn);
                double price = (double)priceCommand.ExecuteScalar();

                conn.Close();
                return price;
            }
        }

        public static void PlaceOrder(int uid, int lid, int quantity, double price, SqlConnection conn)
        {
            string query = string.Format("INSERT INTO [order_history] (user_id, listing_id, quantity, price, date_ordered, confirmed) VALUES ('{0}', '{1}', '{2}', '{3}', CONVERT(date, GETDATE(), 101), '0')", uid, lid, quantity, price.ToString("0.00", CultureInfo.InvariantCulture));
            SqlCommand insertOrderCommand = new SqlCommand(query, conn);
            insertOrderCommand.ExecuteNonQuery();
            Console.WriteLine("Order placed successfully.");
        }

        public static void delListing(int uid)
        {
            string connectionString = @Program.connectionString;
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

        public static void myListings(int uid)
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
            conn.Open();

            SqlCommand searchComm = new SqlCommand();
            searchComm.Connection = conn;
            searchComm.CommandText = string.Format("SELECT title, price, views FROM [listings] WHERE user_id = '{0}'", uid);

            SqlDataReader reader = searchComm.ExecuteReader();
            Console.WriteLine("Your listings:");
            Console.WriteLine();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.Write("Title: ");
                    Console.WriteLine(reader[0]);
                    Console.Write("Price: ");
                    Console.WriteLine(reader[1]);
                    Console.Write("Views: ");
                    Console.WriteLine(reader[2]);
                    Console.WriteLine();
                }
                Console.WriteLine("Press Enter to open the action menu");
                Console.ReadLine();

                string[] actionMenuOptions = { "Delete", "Edit", "Main menu" };
                string[] editMenuOptions = { "Edit info", "Add photo", "Replace photo", "Delete photo" };

                int actionChoice = Other.cantThinkOfANameRn(actionMenuOptions, uid, "Listing actions");
                int lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy;
                switch (actionChoice)
                {
                    case 0:
                        delListing(uid);
                        break;
                    case 1:
                        int editChoice = Other.cantThinkOfANameRn(editMenuOptions, uid, "Listing edition");
                        switch (editChoice)
                        {
                            case 0:
                                editListing(uid);
                                break;
                            case 1:
                                lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy = czos(uid);
                                Console.WriteLine("Enter the path of the image you want to add:");
                                string imagePath = Console.ReadLine();
                                byte[] imageBytes = PhotoManager.ReadImageBytes(imagePath);
                                PhotoManager.InsertListingPhoto(lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy, imageBytes);
                                Console.WriteLine("Photo added successfully.");
                                break;
                            case 2:
                                lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy = czos(uid);
                                Console.WriteLine("Enter the path of the new image:");
                                string newImagePath = Console.ReadLine();
                                PhotoManager.ReplacePhoto(lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy, newImagePath);
                                break;
                            case 3:
                                lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy = czos(uid);
                                PhotoManager.DeleteListingPhoto(lildadefrnidkjgbuwiesfweshujgrsvyftsdcgyescytad6zrcgfy);
                                Console.WriteLine("Photo deleted successfully.");
                                break;
                        }
                        break;
                    case 2:
                        break;
                }
            }
            else
            {
                Console.WriteLine("No results");
                Console.Write("Press Enter to return to the main menu");
                Console.ReadLine();
            }

            conn.Close();
        }




        public static void addListing(int userid)
        {
            string price, quantity, title, description;
            SqlConnection conn = new SqlConnection(Program.connectionString);
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
        {
            while (true)
            {
                string connectionString = @Program.connectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();

                SqlCommand searchcomm = new SqlCommand();
                searchcomm.Connection = conn;
                searchcomm.CommandText = string.Format("SELECT [listings].[id], [user].[first_name], [user].[last_name], [listings].[price], [listings].[title], [listings].[description], [user].[phone_number], [listings].[user_id], [reviews].[content], listings.quantity FROM [user] INNER JOIN [listings] ON [user].[id] = [listings].[user_id] LEFT JOIN [reviews] ON [reviews].[listing_id] = [listings].[id] WHERE [listings].[id] = '{0}'", lid);
                double price = 0;

                // Number of columns
                SqlDataReader reader2 = searchcomm.ExecuteReader();

                string[] authordetails = { "Check author profile", "Post a review", "Show listing photo", "Add to cart", "Order now", "Main menu" };

                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        Console.Clear();
                        Console.WriteLine(reader2[4]);
                        if (double.TryParse(reader2[3].ToString(), out double parsedPrice))
                        {
                            price = parsedPrice;
                            Console.WriteLine(parsedPrice + "zł");
                        }
                        else
                        {
                            Console.WriteLine("Invalid price format");
                        }
                        Console.WriteLine(reader2[9] + "in stock");
                        Console.WriteLine();
                        Console.WriteLine(reader2[1] + " " + reader2[2]);
                        Console.WriteLine("Phone number: " + reader2[6]);
                        Console.WriteLine();
                        Console.WriteLine(reader2[5]);
                        Other.viewsCounter(lid);

                        int listingId = (int)reader2["id"];
                    }
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

                Console.WriteLine("Click enter to see author profile or return to the main menu");
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
                        AddReview(uid, lid, loggedin);
                        break;
                    case 2:
                        //show listing photo
                        PhotoManager.ShowListingPhoto(lid);
                        break;
                    case 3:
                        // Add to cart
                        if (!loggedin)
                        {
                            Console.WriteLine("Please log in first");
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;
                        }
                        addtocart(uid, lid);
                        break;
                    case 4:
                        // Order now
                        if (!loggedin)
                        {
                            Console.WriteLine("Please log in first");
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;
                        }
                        order(uid, lid, price);
                        break;
                    case 5:
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
                Console.WriteLine("Invalid quantity. Please enter a valid number.\n returning in 1 second");
                Thread.Sleep(1000);
                return;
            }

            // Retrieve the available quantity from the database
            string connectionString = Program.connectionString;
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
                Console.WriteLine("Invalid quantity. Please enter a valid number.\n returning in 1 second");
                Thread.Sleep(1000);
                return;
            }

            // Retrieve the available quantity from the database
            string connectionString = Program.connectionString;
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

                string query = string.Format("INSERT INTO [order_history] (user_id, listing_id, quantity, price, date_ordered, confirmed) VALUES ('{0}', '{1}', '{2}', '{3}', CONVERT(date, '{4}', 101), '0')", uid, lid, quantity, price, currentDateTime.ToString("MM/dd/yyyy"));



                SqlCommand insertcartCommand = new SqlCommand(query, conn);
                insertcartCommand.ExecuteNonQuery();
                Console.WriteLine("Order placed successfully.");
                Thread.Sleep(1000);
            }
        }


        public static void AddReview(int reviewerId, int revieweeId, bool loggedin)
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
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
