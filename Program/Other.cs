using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Other
    {
        public static string selectName(int userid)
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
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
                    return reader2[0] + " " + reader2[1];

                }
                return "nigger";
            }
            else
                return "Not logged in";
            conn.Close();

        }
        public static int cantThinkOfANameRn(string[] text, int uid, string startcontent)
        {


            int pos = 0;
            int txtsum;
            while (true)
            {
                Console.Clear();
                int spcnum = 0;
                Console.WriteLine(startcontent);Console.WriteLine("-------------------------");

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
                if (uid==0)
                    Console.WriteLine("Not logged in");



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
                    }
            }
        }

        public static string lenght(int maxLength, int minLength, string variable, string variableName, bool isNumber)
        {
            while (true)
            {
                if (variable.Length < minLength || variable.Length > maxLength)
                {
                    if (variable.Length < minLength)
                    {
                        Console.WriteLine($"{variableName} is too short. It must be at least {minLength} characters long. Please try again.");
                    }
                    else if (variable.Length > maxLength)
                    {
                        Console.WriteLine($"{variableName} is too long. It must be less than or equal to {maxLength} characters long. Please try again.");
                    }

                    Console.Write($"{variableName}: ");
                    variable = Console.ReadLine();
                    continue; // Go to the next iteration of the loop
                }

                if (isNumber)
                {
                    if (!int.TryParse(variable, out _))
                    {
                        Console.WriteLine("Nie mozesz tak robic!! 1!1!1!1!1!!!!!!\n dzwonie po kamila\nlease enter a valid integer.");
                        Console.Write($"{variableName}: ");
                        variable = Console.ReadLine();

                        continue; // Go to the next iteration of the loop
                    }
                }

                break; // Exit the loop if both checks pass
            }
            

            return variable.Replace("'", "");;
        }


        public static void loginfo(int userid)
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
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

        public static int selectId(string password, string loginsql)
        {
            int id = 0;
            SqlConnection conn = new SqlConnection(Program.connectionString);
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

        public static void viewsCounter(int lid)
        {
            SqlConnection conn = new SqlConnection(Program.connectionString);
            conn.Open();

            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("SELECT [views] FROM [listings] WHERE [listings].[id] = '{0}';", lid);

            int views = 0;
            using (SqlDataReader reader = searchcomm.ExecuteReader())
            {
                if (reader.Read())
                {
                    views = (int)reader["views"];
                }
            }

            string updateSql = string.Format("UPDATE [listings] SET views = '{0}' WHERE id = '{1}';", views + 1, lid);
            SqlCommand updateCommand = new SqlCommand(updateSql, conn);
            updateCommand.ExecuteNonQuery();

            conn.Close();
        }

    }
}
