using ConsoleShop100percentLegitNoScam.Program;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;

public class PhotoManager
{
    

    public static void ReplacePhoto(int listingId, string imagePath)
    {
        byte[] imageBytes = ReadImageBytes(imagePath);

        if (imageBytes == null)
        {
            Console.WriteLine("Invalid image file. Please provide a valid image.\nReturning in 1 sec");
            Thread.Sleep(1000);
            return;
        }

        DeleteListingPhoto(listingId);
        InsertListingPhoto(listingId, imageBytes);

        Console.WriteLine("Photo replaced successfully.");
    }

    public static void DeletePhoto(int imageId)
    {
        DeleteImage(imageId);
        Console.WriteLine("Photo deleted successfully.");
    }

    public static void ShowListingPhoto(int listingId)
    {
        byte[] imageBytes = GetListingPhoto(listingId);

        if (imageBytes == null)
        {
            Console.WriteLine("No photo found for the listing.\nReturning in 1 sec");
            Thread.Sleep(1000);
            return;
        }

        string fileName = $"listing_photo_{listingId}.jpg"; // Change the file name as desired
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

    
    
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            fs.Write(imageBytes, 0, imageBytes.Length);
        }

        Console.WriteLine("Photo downloaded successfully.\n"+filePath+"\nThe photo is now on your desktop :D\n click enter to return");
        Console.ReadLine();

    
    }

    public static byte[] ReadImageBytes(string imagePath)
    {
        byte[] imageBytes = null;

        try
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            using (MemoryStream ms = new MemoryStream())
            {
                fs.CopyTo(ms);
                imageBytes = ms.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading image file: {ex.Message}");
        }

        return imageBytes;
    }

    public static void InsertListingPhoto(int listingId, byte[] imageBytes)
    {
        string query = "INSERT INTO [images] ([image]) VALUES (@image);" +
            "DECLARE @imageId INT = SCOPE_IDENTITY();" +
            "UPDATE [listings] SET [mainimage_id] = @imageId WHERE [id] = @listingId;";

        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                command.Parameters.AddWithValue("@image", imageBytes);
                command.Parameters.AddWithValue("@listingId", listingId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting listing photo: {ex.Message}");
            }
        }
    }

    public static void DeleteListingPhoto(int listingId)
    {
        string query = "SELECT [mainimage_id] FROM [listings] WHERE [id] = @listingId";

        int imageId = 0;

        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                command.Parameters.AddWithValue("@listingId", listingId);
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    imageId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving listing photo ID: {ex.Message}");
            }
        }

        if (imageId != 0)
        {
            DeleteImage(imageId);
        }
    }
    
    public static void DeleteImage(int imageId)
    {
        string query = "DELETE FROM [images] WHERE [id] = @imageId";

        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                command.Parameters.AddWithValue("@imageId", imageId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }
    }

    public static byte[] GetListingPhoto(int listingId)
    {
        string query = "SELECT [image] FROM [images] WHERE [id] IN (SELECT [mainimage_id] FROM [listings] WHERE [id] = @listingId)";

        byte[] imageBytes = null;

        using (SqlConnection connection = new SqlConnection(Program.connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            try
            {
                command.Parameters.AddWithValue("@listingId", listingId);
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    imageBytes = (byte[])result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving listing photo: {ex.Message}");
            }
        }

        return imageBytes;
    }
}
