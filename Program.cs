
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
                Console.Clear();
            }
            while (var.Length > max)
            {
                Console.WriteLine(varName + " is too long, it must be less than " + max + " characters long, please try again");
                Console.Write(varName + ":");
                var = Console.ReadLine();
                Console.Clear();
            }
            return var;
        }




        static void register()
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

        }

        static int login()
        {
            int id = 0;
            string loginn, password;
            Console.Write("Login:");
            loginn = Console.ReadLine();
            Console.Write("Password:");
            password = Console.ReadLine();

            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
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
                        id = (int)reader2[1];
                    }
                    Console.WriteLine();

                }
            }
            return id;
            conn.Close();
            //end of select
        }



        static void search()
        {
            SqlConnection conn = new SqlConnection("workstation id = application.mssql.somee.com; packet size = 4096; user id = app_SQLLogin_1; pwd = yespassword; data source = application.mssql.somee.com; persist security info = False; initial catalog = application");
            conn.Open();

            Console.Write("Search:");
            string search = Console.ReadLine();


            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("select title, price from [listings] where title LIKE '%{0}%' or description LIKE '%{0}%'", search);


            //liczba kolumn
            SqlDataReader reader2 = searchcomm.ExecuteReader();
            int numberOfColumns = reader2.FieldCount;

            //nazwy kolumn
            string[] columnNames = new string[numberOfColumns];
            for (int i = 0; i < numberOfColumns; i++)
            {
                columnNames[i] = reader2.GetName(i);
            }



            Console.WriteLine();
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


                }
                Console.ReadLine();
            }
            else
                Console.WriteLine("No results");
            conn.Close();

        }




        static int cantThinkOfANameRn(string[] text)
        {


            int pos = 0;
            int txtsum;
            while (true)
            {
                Console.Clear();
                int spcnum = 0;

                for (int i = 0; i < text.Length; i++)
                {
                    Console.Write(text[i]);
                    Console.Write("   ");
                }

                for (int i = 0; i < pos; i++)
                {
                    spcnum += text[i].Length + 3;
                }
                Console.WriteLine();

                for (int i = 0; i < spcnum; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("^");

                if (pos >= text.Length)
                    pos--;
                else if (pos < 0)
                    pos++;
                else
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.RightArrow:
                            pos++;
                            break;
                        case ConsoleKey.LeftArrow:
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
            string sql = string.Format("insert into[listings] (title, price, views, user_id, category_id, quantity, description) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", title, price, 0, userid, cantThinkOfANameRn(categorytab) + 1, quantity, description);
            addlisting = new SqlCommand(sql, conn);
            adapter.InsertCommand = new SqlCommand(sql, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }




        static void Main(string[] args)
        {
            Boolean isLoggedIn = true;
            int userid = 2;
            string[] myAcc = new string[] { "Login", "Register" };
            string[] hub = new string[] { "Search", "My account", "Add listing" };
            switch (cantThinkOfANameRn(hub))
            {
                case 0:
                    search();
                    break;

                case 1:
                    if (cantThinkOfANameRn(myAcc) == 0)
                        userid = login();
                    else
                        register();
                    break;

                case 2:
                    if (isLoggedIn)
                        addListing(userid);
                    else
                    {
                        Console.WriteLine("You need to create a new account first or login");


                        if (cantThinkOfANameRn(myAcc) == 0)
                            login();
                        else
                            register();
                        isLoggedIn = true;


                    }
                    break;
            }
    }
}


