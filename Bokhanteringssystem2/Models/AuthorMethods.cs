using Microsoft.Data.SqlClient;
using System.Data;

namespace Bokhanteringssystem2.Models
{
    public class AuthorMethods
    {
        // Konstruktor
        public AuthorMethods() { }

        public int GetAuthorIdByName(string authorName, out string errorMessage)
        {
            errorMessage = string.Empty;
            int authorID = 0;

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "SELECT AuthorID FROM Authors WHERE Name = @Name";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.AddWithValue("@Name", authorName);

                    try
                    {
                        sqlConnection.Open();
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            authorID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error retrieving author ID: {ex.Message}";
                    }
                }
            }

            return authorID;
        }

        public int InsertAuthor(AuthorDetails authorDetails, out string errorMessage)
        {
            errorMessage = string.Empty;
            int newAuthorID = 0;

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "INSERT INTO Authors (Name) OUTPUT INSERTED.AuthorID VALUES (@Name)";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.AddWithValue("@Name", authorDetails.Name);

                    try
                    {
                        sqlConnection.Open();
                        newAuthorID = (int)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error inserting author: {ex.Message}";
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }

            return newAuthorID;
        }

        public List<AuthorDetails> GetAuthorList(out string errorMessage)
        {
            errorMessage = string.Empty;
            List<AuthorDetails> authorsList = new List<AuthorDetails>();

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "SELECT * FROM Authors";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                authorsList.Add(new AuthorDetails
                                {
                                    AuthorID = Convert.ToInt32(reader["AuthorID"]),
                                    Name = reader["Name"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error retrieving authors: {ex.Message}";
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
            return authorsList;
        }

        public AuthorDetails GetAuthor(int authorID, out string errorMessage)
        {
            errorMessage = string.Empty;

            // skapar och använder koppling till databasen
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "SELECT * FROM Authors WHERE AuthorID = @id";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.Add("id", SqlDbType.Int).Value = authorID;

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

                    DataSet dataSet = new DataSet();

                    try
                    {
                        sqlConnection.Open();

                        // Lägger till en tabell i datasetobjektet och fyller detta med data baserat på selectsatsen
                        sqlDataAdapter.Fill(dataSet, "Authors");

                        int count = 0;

                        count = dataSet.Tables["Authors"].Rows.Count;

                        AuthorDetails authorDetails = new AuthorDetails();

                        if (count == 1)
                        {
                            // Läser ut data från dataset och fyller objekt
                            authorDetails.AuthorID = Convert.ToInt32(dataSet.Tables["Authors"].Rows[0]["AuthorID"]);
                            authorDetails.Name = dataSet.Tables["Authors"].Rows[0]["Name"].ToString();

                            return authorDetails;
                        }
                        else
                        {
                            errorMessage = "No author details is fetched";
                            return authorDetails;
                        }
                        
                        
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error retrieving author: {ex.Message}";
                        return null;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }

        public AuthorDetails UpdateAuthor (AuthorDetails authorDetails, out string errorMessage)
        {
            errorMessage = string.Empty;

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "UPDATE Authors SET Name = @newName WHERE AuthorID = @id";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.AddWithValue("@newName", authorDetails.Name);
                    command.Parameters.AddWithValue("@id", authorDetails.AuthorID);

                    try
                    {
                        sqlConnection.Open(); // Se till att anslutningen är öppen
                        int rowsAffected = command.ExecuteNonQuery(); // Utför kommandot
                        Console.WriteLine($"{rowsAffected} row(s) updated");

                        if (rowsAffected > 0)
                        {
                            return authorDetails; // Returnerar det uppdaterade objektet
                        }
                        else
                        {
                            errorMessage = "No rows were updated. Author could not be found.";
                            return null; // Ingen uppdatering, returnera null
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error updating author: {ex.Message}";
                        return null; // Vid fel returnera null
                    }
                }
            }
        }

        public bool DeleteAuthor(int authorID, out string errorMessage)
        {
            errorMessage = string.Empty;

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "DELETE FROM Authors WHERE AuthorID = @id";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = authorID;

                    try
                    {
                        sqlConnection.Open(); // Se till att anslutningen är öppen
                        int rowsAffected = command.ExecuteNonQuery(); // Utför kommandot

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"{rowsAffected} row(s) deleted");
                            return true; // Operationen lyckades
                        }
                        else
                        {
                            errorMessage = "No rows were deleted. Author could not be found.";
                            return false; // Ingen rad raderades, returnera false
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error deleting author: {ex.Message}";
                        return false; // Vid fel returnera false
                    }
                }
            }
        }

    }
}
