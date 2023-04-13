
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

        


        static void Main(string[] args)
        {
            
            }



        }
    }
}


