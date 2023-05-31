using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Search
    {
        static string ifnulthen(string var, string qfrg, bool max)
        {
            if (string.IsNullOrEmpty(var.Trim()))
                return null;
            else
            {
                if (max)
                    return "AND " + qfrg + var;
                else
                    return qfrg + var;
            }
        }

        public static void searchListing(int uid, bool loggedin)
        {
            string connectionString = @Program.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();
            Console.Write("Search:");
            string search = Console.ReadLine();

            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select id, title, price from [listings] where title LIKE '%{0}%' or description LIKE '%{0}%'", search);
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
                Listing.dLD(id[Other.cantThinkOfANameRn(name.ToArray(), uid,"Search:"+search)], uid, loggedin);
                conn.Close();
            }
            else
            {
                Console.WriteLine("no resulte\nClick enter to return to main menu");
                Console.Read();
            }

        }

        public static void searchUser(int uid, bool loggedin)
        {
            Console.Write("Search:");
            string search = Console.ReadLine();



            string connectionString = @Program.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select id, first_name, last_name, phone_number from [user] where first_name LIKE '%{0}%' or last_name LIKE '%{0}%' or description LIKE '%{0}%'", search);
            SqlDataReader reader2 = searchcomm.ExecuteReader();

            List<string> name = new List<string>();
            List<int> id = new List<int>();

            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    string fullName = reader2[1] + " " + reader2[2] + "\n" + "phone:" + reader2[3] + "\n";
                    int userId = (int)reader2[0];

                    if (!name.Contains(fullName)) // Check if the name already exists in the list
                    {
                        name.Add(fullName);
                        id.Add(userId);
                    }
                }
            }

            User.dUD(id[Other.cantThinkOfANameRn(name.ToArray(), uid, "Search:"+search)], uid);
            conn.Close();

        }
        public static void mainsearch(int uid, string search, bool loggedin)
        {

            string connectionString = @Program.connectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();


            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format(search);
            Console.WriteLine(search);
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
                Listing.dLD(id[Other.cantThinkOfANameRn(name.ToArray(), uid, "Search:"+search)], uid,loggedin);
                conn.Close();
            }
            else
            {
                Console.WriteLine("no resulte\nClick enter to return to main menu");
                Console.Read();
            }

        }

        public static void Filters(int uid,bool loggedin)
        {
            string[] categorytab = new string[] {
        "toys",
        "digital services",
        "cosmetics and body care",
        "food and beverage",
        "health and wellness",
        "household items",
        "media",
        "pet care",
        "office equipment",
        "none"
    };

            Console.WriteLine("Filters\n(leave blank for no restrictions)\n");
            Console.Write("Price\nfrom: ");
            string minprice = Console.ReadLine();
            minprice = Other.lenght(4, 0, minprice, "minimum price", true);
            string minPriceCondition = ifnulthen(minprice, "Price >= ", false);

            Console.Write("to: ");
            string maxprice = Console.ReadLine();
            maxprice = Other.lenght(4, 0, maxprice, "maximum price", true);
            string maxPriceCondition = ifnulthen(maxprice, "Price <= ", false);

            int categoryid = Other.cantThinkOfANameRn(categorytab, uid,"Choose a category");
            string catq = "";
            if (categoryid != 9)
                catq = ifnulthen(categoryid.ToString(), "category_id = ", false);

            Console.Write("Views\nfrom: ");
            string minviews = Console.ReadLine();
            minviews = Other.lenght(4, 0, minviews, "minimum views", true);
            string minViewsCondition = ifnulthen(minviews, "views >= ", false);

            Console.Write("to: ");
            string maxviews = Console.ReadLine();
            maxviews = Other.lenght(4, 0, maxviews, "maximum views", true);
            string maxViewsCondition = ifnulthen(maxviews, "views <= ", false);

            Console.WriteLine("\nSelected Filters:");
            Console.WriteLine($"Price: {minprice} - {maxprice}");
            Console.WriteLine($"Category: {categorytab[categoryid]}");
            Console.WriteLine($"Views: {minviews} - {maxviews}");

            StringBuilder sqlQuery = new StringBuilder("SELECT id, title, price FROM listings WHERE ");
            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(minPriceCondition))
                conditions.Add(minPriceCondition);
            if (!string.IsNullOrEmpty(maxPriceCondition))
                conditions.Add(maxPriceCondition);
            if (!string.IsNullOrEmpty(catq))
                conditions.Add(catq);
            if (!string.IsNullOrEmpty(minViewsCondition))
                conditions.Add(minViewsCondition);
            if (!string.IsNullOrEmpty(maxViewsCondition))
                conditions.Add(maxViewsCondition);

            sqlQuery.Append(string.Join(" AND ", conditions));

            Console.WriteLine(sqlQuery.ToString());
            mainsearch(uid, sqlQuery.ToString(), loggedin);
        }




    }
}
