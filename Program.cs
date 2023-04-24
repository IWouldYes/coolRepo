



using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp9
{
    internal class Program
    {



        static string lenght(int max, int min, string var, string varName)
        {
            while (var.Length < min)
            {
                Console.WriteLine(varName + " is too short, it must be at least " + min + " characters long, please try again");
                Console.Write(varName + ":");
                var = Console.ReadLine();

            }
            while (var.Length > max)
            {
                Console.WriteLine(varName + " is too long, it must be less than " + max + " characters long, please try again");
                Console.Write(varName + ":");
                var = Console.ReadLine();

            }
            return var;
        }
        
        static void selectName(int userid)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();



            //selectowanie wszystkiego z wybranej tabeli i schemy
            SqlCommand login = new SqlCommand();
            login.Connection = conn;
            login.CommandText = string.Format("select first_name, last_name from [User] where id = '{0}'; ", userid);


            //liczba kolumn
            SqlDataReader reader2 = login.ExecuteReader();






            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    Console.Write("Logged in as ");
                    Console.Write(reader2[0]);
                    Console.Write(" ");
                    Console.WriteLine(reader2[1]);

                }
            }
            else
                Console.WriteLine("Not logged in");
            conn.Close();
        
    }
        
        static int register()
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();

            string login, fName, lName, password, phoneNumber, description, country, city, street;
            Console.Write("Login:");
            login = Console.ReadLine();
            login = lenght(30, 3, login, "Login");



            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [user] WHERE login = @Login", conn);
            command.Parameters.AddWithValue("@Login", login);

            // Execute the query and get the count of records
            int count = (int)command.ExecuteScalar();

            // If count is greater than 0, the login already exists in the database
            if (count > 0)
            {
                Console.WriteLine("Login already exists in the database.");
            }
            else
            {
                Console.WriteLine("Login does not exist in the database.");
            }




            Console.Write("First name:");
            fName = Console.ReadLine();
            fName = lenght(50, 2, fName, "First name");
            Console.WriteLine("Your first name is: " + fName);

            Console.Write("Last name:");
            lName = Console.ReadLine();
            lName = lenght(50, 2, lName, "Last name");
            Console.Write("Password:");
            password = Console.ReadLine();
            password = lenght(30, 4, password, "Password");
            Console.Write("Phone number:");
            phoneNumber = Console.ReadLine();
            phoneNumber = lenght(9, 3, phoneNumber, "Phone number");
            Console.Write("Description(not required so you can leave empty):");
            description = Console.ReadLine();
            description = lenght(500, 0, description, "Description");
            Console.Write("Country:");
            country = Console.ReadLine();
            country = lenght(30, 0, country, "Country");
            Console.Write("City:");
            city = Console.ReadLine();
            city = lenght(50, 0, city, "City");
            Console.Write("Street:");
            street = Console.ReadLine();
            street = lenght(50, 0, street, "Street");

            country = country.ToLower();
            city = city.ToLower();
            street = street.ToLower();





            SqlCommand register;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = string.Format("insert into[user](first_name, last_name, login, password, phone_number, description, country, city, street)values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');", fName, lName, login, password, phoneNumber, description, country, city, street);
            register = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();

            return selectId(password, login);
        }

        static int selectId(string password, string loginsql)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();



            //selectowanie wszystkiego z wybranej tabeli i schemy
            SqlCommand login = new SqlCommand();
            login.Connection = conn;
            login.CommandText = string.Format("select id from [User] where login = '{0}' AND password = '{1}'; ", loginsql, password);


            //liczba kolumn
            SqlDataReader reader2 = login.ExecuteReader();






            if (reader2.HasRows)
            {
                while (reader2.Read())
                {

                        id = (int)reader2[0];


                }
            }
            return id;
            conn.Close();
        }

        static int login(bool isLoggedIn)
        {
            int id = 0;
            string loginn, password;
            Console.Write("Login:");
            loginn = Console.ReadLine();
            Console.Write("Password:");
            password = Console.ReadLine();

            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();



            //selectowanie wszystkiego z wybranej tabeli i schemy
            SqlCommand login = new SqlCommand();
            login.Connection = conn;
            login.CommandText = string.Format("select id from [user] where login = '{0}' AND password = '{1}'", loginn, password);


            //liczba kolumn
            SqlDataReader reader2 = login.ExecuteReader();
            int numberOfColumns = reader2.FieldCount;

            //nazwy kolumn
            DataTable schemaTable = reader2.GetSchemaTable();
            string[] columnNames = new string[numberOfColumns];
            for (int i = 0; i < numberOfColumns; i++)
            {
                columnNames[i] = reader2.GetName(i);
            }




            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        Console.Write(columnNames[i]);
                        Console.Write(": ");
                        Console.WriteLine(reader2[i]);
                        id = (int)reader2[0];
                    }
                    Console.WriteLine();

                }
            }
            if (id == 0)
                isLoggedIn = false;
            else
                isLoggedIn = true;
            return id;
            conn.Close();
            //end of select
        }

        static void dLD(int id)
        {
                string connectionString = @"Server=192.168.0.109;Database=yesyesnono;User Id=1ht;Password=1ht;";
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();


                SqlCommand searchcomm = new SqlCommand();
                searchcomm.Connection = conn;
                searchcomm.CommandText = string.Format("select title, price, description, quantity from [listings] where id = '{0}'", id);


                //liczba kolumn
                SqlDataReader reader2 = searchcomm.ExecuteReader();
                int numberOfColumns = reader2.FieldCount;

                //nazwy kolumn
                string[] columnNames = new string[numberOfColumns];
                for (int i = 0; i < numberOfColumns; i++)
                {
                    columnNames[i] = reader2.GetName(i);
                }



                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        for (int i = 0; i < numberOfColumns; i++)
                        {
                            Console.Write(columnNames[i]);
                            Console.Write(": ");
                            Console.WriteLine(reader2[i]);
                        }
                        Console.WriteLine();


                    };
                }
                else
                    Console.WriteLine("No results");
                conn.Close();

        }
        static void search(int uid)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            Console.Write("Search:");
            string search = Console.ReadLine();


            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select id, title, price from [listings] where title LIKE '%{0}%' or description LIKE '%{0}%'", search);


            //liczba kolumn
            SqlDataReader reader2 = searchcomm.ExecuteReader();

            string[] serch = new string[20];
            int[] choose = new int[20];
            int i = 0;

            Console.WriteLine();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    Console.WriteLine("Title: " + reader2[1] + "\nPrice: " + reader2[2] + "\n");
                    serch[i] = "Title: " + reader2[1] + "\n  Price: " + reader2[2] + "\n";
                    choose[i] = (int)reader2[0];
                    i++;
                }
                dLD(choose[cantThinkOfANameRn(serch, uid)]);
            }
            else
                Console.WriteLine("No results");
            conn.Close();

        }

        static void editListing(int id)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();
            string title;
            Console.Write("Title of the listing you want to change:");
            title = Console.ReadLine();


            string price, quantity, newtitle, description;


            Console.Write("New title:");
            newtitle = Console.ReadLine();
            newtitle = lenght(50, 2, title, "New title");
            Console.Write("Price:");
            price = Console.ReadLine();
            price = lenght(7, 1, price, "Price");
            Console.Write("Quantity:");
            quantity = Console.ReadLine();
            quantity = lenght(1, 1, quantity, "Quantity");
            Console.Write("Description:");
            description = Console.ReadLine();
            description = lenght(500, 0, description, "Description");

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
            string sql = string.Format("UPDATE Listings SET Category_id = '{0}', Title = '{1}', Price = {2}, Quantity = {3}, Description = '{4}' WHERE user_id = '{5}';", cantThinkOfANameRn(categorytab, id), newtitle, price, quantity, description, id);
            editListing = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        static void delListing(int id)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();
            string title;
            Console.Write("Title of the listing you want to delete:");
            title = Console.ReadLine();

            SqlCommand delListing;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = string.Format("delete from listings where title = '{0}' and user_id = '{1}'", title, id);
            delListing = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        static void myListings(int id)
        {
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();



            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select title, price from [listings] where user_id = '{0}'", id);


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


                }
                Console.WriteLine();
                Console.WriteLine();

                Console.Write("do you want to delete or edit your listing?[del/edit]");
                string y = Console.ReadLine();
                switch (y)
                {
                    case "del":
                        delListing(id);
                        break;
                    case "edit":
                        editListing(id);
                        break;
                    default:
                        Console.WriteLine("Invalid");
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

        static int cantThinkOfANameRn(string[] text, int uid)
        {


            int pos = 0;
            int txtsum;
            while (true)
            {
                Console.Clear();
                int spcnum = 0;

                for (int i = 0; i < text.Length; i++)
                {
                    if (pos == i)
                    {
                        Console.Write("> ");
                        Console.WriteLine(text[i]);
                    }
                    else
                    {
                        Console.Write("  ");
                        Console.WriteLine(text[i]);
                    }

                }

                for (int i = 0; i < pos; i++)
                {
                    spcnum += 1;
                }

                Console.WriteLine("――――――――――――――――――――――――――――――――");
                Console.WriteLine("use up and down arrows to choose");
                Console.WriteLine();
                selectName(uid);




                if (pos >= text.Length)
                    pos--;
                else if (pos < 0)
                    pos++;
                else
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.DownArrow:
                            pos++;
                            break;
                        case ConsoleKey.UpArrow:
                            pos--;
                            break;
                        case ConsoleKey.Enter:
                            Console.Clear();
                            return pos;
                            Thread.Sleep(5000);
                            Console.Clear();
                            break;
                    }
            }
        }

        static void addListing(int userid)
        {
            string price, quantity, title, description;
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();

            Console.Write("Title:");
            title = Console.ReadLine();
            title = lenght(50, 2, title, "Title");
            Console.Write("Price:");
            price = Console.ReadLine();
            price = lenght(7, 1, price, "Price");
            Console.Write("Quantity:");
            quantity = Console.ReadLine();
            quantity = lenght(1, 1, quantity, "Quantity");
            Console.Write("Description:");
            description = Console.ReadLine();
            description = lenght(500, 0, description, "Description");
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
            string sql = string.Format("insert into[listings] (title, price, views, user_id, category_id, quantity, description) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", title, price, 0, userid, cantThinkOfANameRn(categorytab, userid) + 1, quantity, description);
            addlisting = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }

        static void Main(string[] args)
        {
            Boolean isLoggedIn = false;
            int userid = 0;
            string[] myAcc = new string[] { "Login", "Register new account", "Edit account", "Back" };
            string[] hub = new string[] { "Search", "My account", "My listings" };
            string[] meinListings = new string[] { "edit/delete listing", "Create listing", "Back" };
            while (true)
            {
                switch (cantThinkOfANameRn(hub, userid))
                {
                    case 0:
                        search(userid);
                        Console.Write("click enter to return to main menu");
                        Console.ReadLine();
                        break;

                    case 1:
                        switch (cantThinkOfANameRn(myAcc, userid))
                        {
                            case 0:

                                userid = login(isLoggedIn);
                                if (userid != 0)
                                {
                                    selectName(userid);
                                    Console.Write("click enter to return to main menu");
                                    Console.ReadLine();
                                    isLoggedIn = true;
                                }
                                break;

                            case 1:
                                userid = register();
                                isLoggedIn = true;
                                selectName(userid);
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
                            switch (cantThinkOfANameRn(meinListings, userid))
                            {
                                case 0:
                                    myListings(userid);
                                    break;

                                case 1:
                                    addListing(userid);
                                    break;

                            }

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

