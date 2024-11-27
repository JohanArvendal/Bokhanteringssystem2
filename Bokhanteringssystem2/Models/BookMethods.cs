using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Bokhanteringssystem2.Models;
using System.Data;

namespace Bokhanteringssystem2.Models

{
    public class BookMethods
    {
        // Konstruktor
        public BookMethods() { }

        // Publika metoder
        public int InsertBook(BookDetails bookDetails, out string errorMessage)
        {
            errorMessage = string.Empty;  // Initiera felmeddelandet
            int rowsAffected = 0;  // Initiera antalet påverkade rader

            // Skapa koppling mot lokal instans av databas
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                // SQL INSERT 
                string sqlstring = "INSERT INTO Books (Title, PublishedYear, AuthorID) VALUES (@Title, @PublishedYear, @AuthorID)";

                using (SqlCommand command = new SqlCommand(sqlstring, sqlConnection))
                {
                    // Lägg till parametrar
                    command.Parameters.AddWithValue("@Title", bookDetails.Title);
                    command.Parameters.AddWithValue("@PublishedYear", bookDetails.PublishedYear);
                    command.Parameters.AddWithValue("@AuthorID", bookDetails.AuthorID);

                    try
                    {
                        sqlConnection.Open();  // Öppna anslutningen
                        rowsAffected = command.ExecuteNonQuery();  // Utför INSERT och få antal påverkade rader
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "Insert command failed: " + ex.Message;  // Returnera felmeddelandet om något går fel
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }

            return rowsAffected;  // Returnera antalet påverkade rader
        }
        public List<BookDetails> GetBookList(out string errormessage)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"); 

            string sqlstring = "SELECT b.BookID, b.Title, b.PublishedYear, b.AuthorID, a.Name AS AuthorName FROM Books b JOIN Authors a ON b.AuthorID = a.AuthorID";

            SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            List<BookDetails> bookDetailsList = new List<BookDetails>();

            try
            {
                sqlConnection.Open();
                sqlDataAdapter.Fill(dataSet, "Books");

                int count = dataSet.Tables["Books"].Rows.Count;

                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        BookDetails bookDetails = new BookDetails
                        {
                            BookID = Convert.ToInt32(dataSet.Tables["Books"].Rows[i]["BookID"]),
                            Title = dataSet.Tables["Books"].Rows[i]["Title"]?.ToString() ?? "Unknown Title",  
                            PublishedYear = Convert.ToInt32(dataSet.Tables["Books"].Rows[i]["PublishedYear"]),
                            AuthorID = Convert.ToInt32(dataSet.Tables["Books"].Rows[i]["AuthorID"]),
                            Author = new AuthorDetails
                            {
                                AuthorID = Convert.ToInt32(dataSet.Tables["Books"].Rows[i]["AuthorID"]),
                                Name = dataSet.Tables["Books"].Rows[i]["AuthorName"]?.ToString() ?? "Unknown Author" 
                            }
                        };

                        bookDetailsList.Add(bookDetails);
                    }
                    errormessage = "";
                    return bookDetailsList;
                }

                else
                {
                    errormessage = "No Book details found.";
                    return new List<BookDetails>();
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return new List<BookDetails>();
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public List<BookDetails> GetBooksByAuthorId(int authorId, out string errorMessage)
        {
            errorMessage = string.Empty;
            List<BookDetails> bookDetailsList = new List<BookDetails>();

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True"))
            {
                string sqlQuery = "SELECT BookID, Title, PublishedYear FROM Books WHERE AuthorID = @AuthorID";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.AddWithValue("@AuthorID", authorId);

                    try
                    {
                        sqlConnection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookDetailsList.Add(new BookDetails
                                {
                                    BookID = Convert.ToInt32(reader["BookID"]),
                                    Title = reader["Title"].ToString(),
                                    PublishedYear = Convert.ToInt32(reader["PublishedYear"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error retrieving books: {ex.Message}";
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }

            return bookDetailsList;
        }

    }
}
