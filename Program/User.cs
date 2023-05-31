using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class User
    {
        public static int register()
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
            conn.Open();

            string login, fName, lName, password, phoneNumber, description, country, city, street;
            Console.Write("Login:");
            login = Console.ReadLine();
            login = Other.lenght(30, 3, login, "Login", false);



            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [user] WHERE login = @Login", conn);
            command.Parameters.AddWithValue("@Login", login);

            // Execute the query and get the count of records
            int count = (int)command.ExecuteScalar();

            // If count is greater than 0, the login already exists in the database
            if (count > 0)
            {
                Console.WriteLine("Login already exists in the database.");
                return 0;
            }
            else
            {
                Console.WriteLine("Login does not exist in the database.");
            }




            Console.Write("First name:");
            fName = Console.ReadLine();
            fName = Other.lenght(50, 2, fName, "First name", false);
            Console.WriteLine("Your first name is: " + fName);

            Console.Write("Last name:");
            lName = Console.ReadLine();
            lName = Other.lenght(50, 2, lName, "Last name", false);
            Console.Write("Password:");
            password = Console.ReadLine();
            password = Other.lenght(30, 4, password, "Password", false);
            Console.Write("Phone number:");
            phoneNumber = Console.ReadLine();
            phoneNumber = Other.lenght(9, 3, phoneNumber, "Phone number", true);
            Console.Write("Description(not required so you can leave empty):");
            description = Console.ReadLine();
            description = Other.lenght(500, 0, description, "Description", false);
            Console.Write("Country:");
            country = Console.ReadLine();
            country = Other.lenght(30, 0, country, "Country", false);
            Console.Write("City:");
            city = Console.ReadLine();
            city = Other.lenght(50, 0, city, "City", false);
            Console.Write("Street:");
            street = Console.ReadLine();
            street = Other.lenght(50, 0, street, "Street", false);

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

            return Other.selectId(password, login);
        }

        public static int login(bool isLoggedIn)
        {
            int id = 0;
            string loginn, password;
            Console.Write("Login:");
            loginn = Console.ReadLine();
            Console.Write("Password:");
            password = Console.ReadLine();

            string connectionString = @Program.connectionString;
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
                    Console.WriteLine(Other.selectName(id));

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

        public static void dUD(int uid, int muid)
        {
            while (true)
            {
                string connectionString = @Program.connectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();

                // Retrieve user data
                SqlCommand searchUserCommand = new SqlCommand();
                searchUserCommand.Connection = conn;
                searchUserCommand.CommandText = string.Format("SELECT [first_name], [last_name], [phone_number], [description], [country], [city], [street] FROM [user] WHERE id = '{0}'", uid);

                SqlDataReader userReader = searchUserCommand.ExecuteReader();

                string[] authorActions = { "Message author", "Comment", "Main menu" };
                string name;
                if (userReader.HasRows)
                {
                    while (userReader.Read())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(userReader[0] + " " + userReader[1]);
                        Console.ResetColor();
                        Console.WriteLine("Phone number: " + userReader[2]);
                        Console.WriteLine(userReader[3]);
                        Console.WriteLine(userReader[4]);
                        Console.WriteLine(userReader[5] + " " + userReader[6]);
                    }
                }
                else
                {
                    Console.WriteLine("No user data found");
                }

                userReader.Close();

                // Display reviews with reviewer's name
                SqlCommand searchCommentCommand = new SqlCommand();
                searchCommentCommand.Connection = conn;
                searchCommentCommand.CommandText = string.Format("SELECT [comments].[commenter_id], [comments].[content], [user].[first_name], [user].[last_name] FROM [comments] INNER JOIN [user] ON [comments].[commenter_id] = [user].[id] WHERE [comments].[commentee_id] = '{0}'", uid);

                SqlDataReader commentReader = searchCommentCommand.ExecuteReader();

                if (commentReader.HasRows)
                {
                    Console.WriteLine("\n-----------------------");
                    while (commentReader.Read())
                    {
                        int reviewerId = (int)commentReader["commenter_id"];
                        string content = (string)commentReader["content"];
                        string commenterFirstName = (string)commentReader["first_name"];
                        string commenterLastName = (string)commentReader["last_name"];

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{commenterFirstName} {commenterLastName}");
                        Console.ResetColor();
                        Console.WriteLine(content);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nNo reviews found");
                }

                commentReader.Close();

                Console.Write("\nPress Enter to open the action menu");
                Console.ReadLine();

                switch (Other.cantThinkOfANameRn(authorActions, muid,""))
                {
                    case 0:
                        // Message author
                        Chat.messageAuthor(uid, muid);
                        break;
                    case 1:
                        AddComment(muid, uid);
                        break;
                    case 2:
                        // Main menu
                        return;
                }

                conn.Close();
            }

        }

        public static void AddComment(int CommenterId, int CommenteeId)
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
            conn.Open();

            Console.WriteLine("Enter the comment content:");
            string CommentContent = Console.ReadLine();

            // Insert the review into the reviews table
            SqlCommand insertReviewCommand = new SqlCommand("INSERT INTO [comments] (commenter_id, commentee_id, content) VALUES (@CommenterId, @CommenteeId, @Content)", conn);
            insertReviewCommand.Parameters.AddWithValue("@CommenterId", CommenterId);
            insertReviewCommand.Parameters.AddWithValue("@CommenteeId", CommenteeId);
            insertReviewCommand.Parameters.AddWithValue("@Content", CommentContent);
            insertReviewCommand.ExecuteNonQuery();

            Console.Clear();

            conn.Close();
        }

    }
}
