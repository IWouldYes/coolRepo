using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Listing
    {
        public static void editListing(int id)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();
            string title;
            Console.Write("Title of the listing you want to change:");
            title = Console.ReadLine();

            // Check if the listing exists
            SqlCommand checkListingCmd = new SqlCommand("SELECT COUNT(*) FROM Listings WHERE Title = @Title AND user_id = @UserId", conn);
            checkListingCmd.Parameters.AddWithValue("@Title", title);
            checkListingCmd.Parameters.AddWithValue("@UserId", id);
            int listingCount = (int)checkListingCmd.ExecuteScalar();

            if (listingCount == 0)
            {
                Console.WriteLine("Listing with the specified title does not exist.");
                conn.Close();
                return;
            }

            string price, quantity, newtitle, description;

            Console.Write("New title:");
            newtitle = Console.ReadLine();
            newtitle = Other.lenght(50, 2, title, "New title", false);
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
            string sql = string.Format("UPDATE Listings SET Category_id = '{0}', Title = '{1}', Price = {2}, Quantity = {3}, Description = '{4}' WHERE user_id = '{5}' AND Title = '{6}';",
                Other.cantThinkOfANameRn(categorytab, id), newtitle, price, quantity, description, id, title);
            editListing = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        public static void delListing(int uid)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();
            Console.Write("Search:");
            string search = Console.ReadLine();

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
                string sql2 = string.Format("delete from listings where andid = '{0}'", id[Other.cantThinkOfANameRn(name.ToArray(), uid)]);
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
                Console.WriteLine("click enter to Edit, Delete or Leave");
                Console.ReadLine();


                string[] y = { "Delete", "Edit", "Main menu" };
                switch (Other.cantThinkOfANameRn(y, id))
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
            string sql = string.Format("insert into[listings] (title, price, views, user_id, category_id, quantity, description) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", title, price, 0, userid, Other.cantThinkOfANameRn(categorytab, userid) + 1, quantity, description);
            addlisting = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        public static void dLD(int lid, int uid)
        {//dLD - display Listing Data
            while (true) 
            {
                string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();


                SqlCommand searchcomm = new SqlCommand();
                searchcomm.Connection = conn;
                searchcomm.CommandText = string.Format("SELECT[user].[first_name], [user].[last_name], [listings].[price], [listings].[title], [listings].[description], [user].[phone_number], [listings].[user_id], [reviews].[content] FROM[user] INNER JOIN[listings] ON[user].[id] = [listings].[user_id] LEFT JOIN[reviews] ON[reviews].[listing_id] = [listings].[id] WHERE[listings].[id] = '{0}'", lid);


                //liczba kolumn
                SqlDataReader reader2 = searchcomm.ExecuteReader();


                string[] authordetails = { "Check author profile", "Post a review", "Main menu" };

                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        Console.Clear();
                        Console.WriteLine(reader2[3]);
                        Console.WriteLine(reader2[2] + "zł");
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

                switch (Other.cantThinkOfANameRn(authordetails, uid))
                {
                    case 0:
                        reader2 = searchcomm.ExecuteReader();
                        User.dUD((int)reader2[6], uid);
                        reader2.Close();
                        break;
                    case 1:
                        AddReview(uid, lid);
                        break;
                    case 2:
                        return;
                }


                conn.Close();
            }
            

        }
        public static void AddReview(int reviewerId, int revieweeId)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();

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
