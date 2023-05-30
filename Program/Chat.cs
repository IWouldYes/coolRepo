using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShop100percentLegitNoScam.Program
{
    public class Chat
    {
        public static void messageAuthor(int recid, int sendid)
        {
            string message;
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();

            if (sendid != 0)
            {
                Console.WriteLine("Write Message");
                Console.WriteLine("Senderid - " + sendid + " recipientid - " + recid);
                Console.Write(":");
                message = Console.ReadLine();
                SqlCommand send;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string sql = string.Format("insert into [messages] (sender_id, recipient_id, message_content) values ('{1}','{0}','{2}')", recid, sendid, message);
                send = new SqlCommand(sql, conn);
                adapter.InsertCommand = new SqlCommand(sql, conn);
                adapter.InsertCommand.ExecuteNonQuery();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Please log in first");
                Thread.Sleep(1000);
                return;
            }
        }

        public static void writeChat(int recid, int sendid)
        {
            string message;
            SqlConnection conn = new SqlConnection("workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application");
            conn.Open();
            bool isnotesc = true;
            string[] writemessageui = { "Write message", "See chat", "Main menu" };
            while (isnotesc)
            {

                Console.WriteLine("――――――Click enter to open action menu or leave―――――――");
                Console.ReadLine();

                switch (Other.cantThinkOfANameRn(writemessageui, sendid,"Chat"))
                {
                    case 2:
                        isnotesc = false;
                        return;
                    case 0:
                        Console.WriteLine("Write Message");
                        Console.WriteLine("Senderid - " + sendid + " recipientid - " + recid);
                        Console.Write(":");
                        message = Console.ReadLine();
                        SqlCommand send;
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        string sql = string.Format("insert into [messages] (sender_id, recipient_id, message_content) values ('{1}','{0}','{2}')", recid, sendid, message);
                        send = new SqlCommand(sql, conn);
                        adapter.InsertCommand = new SqlCommand(sql, conn);
                        adapter.InsertCommand.ExecuteNonQuery();

                        break;
                    case 1:
                        showChat(recid, sendid);
                        break;
                }
            }



            conn.Close();
        }

        public static void showChat(int recid, int sendid)
        {
            if (recid == 0) return;
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string query = string.Format(
               "SELECT [sender].[first_name] AS [sender_first_name], [sender].[last_name] AS [sender_last_name], " +
               "[recipient].[first_name] AS [recipient_first_name], [recipient].[last_name] AS [recipient_last_name], " +
               "[messages].[message_content] " +
               "FROM [messages] " +
               "INNER JOIN [user] AS [sender] ON [messages].[sender_id] = [sender].[id] " +
               "INNER JOIN [user] AS [recipient] ON [messages].[recipient_id] = [recipient].[id] " +
               "WHERE ([messages].[recipient_id] = {0} AND [messages].[sender_id] = {1}) " +
               "   OR ([messages].[recipient_id] = {1} AND [messages].[sender_id] = {0}) " +
               "ORDER BY [messages].[id];",
               recid, sendid);

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string senderFirstName = reader.GetString(0);
                            string senderLastName = reader.GetString(1);
                            string recipientFirstName = reader.GetString(2);
                            string recipientLastName = reader.GetString(3);
                            string messageContent = reader.GetString(4);

                            string senderName = $"{senderFirstName} {senderLastName}";
                            string recipientName = $"{recipientFirstName} {recipientLastName}";

                            Console.WriteLine($"[{senderName}]\n {messageContent}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No results");
                    }
                }
            }

            writeChat(recid, sendid);
            conn.Close();
        }

        public static void chat(int uid)
        {
            string connectionString = @"workstation id=application.mssql.somee.com;packet size=4096;user id=app_SQLLogin_1;pwd=yespassword;data source=application.mssql.somee.com;persist security info=False;initial catalog=application";
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand searchcomm = new SqlCommand();
            searchcomm.Connection = conn;
            searchcomm.CommandText = string.Format("SELECT [user].[first_name],[user].[last_name],[user].[id] FROM [user] JOIN [messages] ON [user].[id] = [messages].[sender_id] WHERE [messages].[recipient_id] = {0};", uid);
            SqlDataReader reader2 = searchcomm.ExecuteReader();

            List<string> name = new List<string>();
            List<int> id = new List<int>();

            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    string fullName = reader2[0] + " " + reader2[1];
                    int userId = (int)reader2[2];

                    if (!name.Contains(fullName)) // Check if the name already exists in the list
                    {
                        name.Add(fullName);
                        id.Add(userId);
                    }
                }
            }
            name.Add("Back");
            id.Add(0);

            showChat(id[Other.cantThinkOfANameRn(name.ToArray(), uid,"Your chats")], uid);
            conn.Close();
        }
    }
}
