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
        public List<AuthorDetails> GetAllAuthors(out string errorMessage)
        {
            errorMessage = string.Empty;
            List<AuthorDetails> authorsList = new List<AuthorDetails>();

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True"))
            {
                string sqlQuery = "SELECT AuthorID, Name FROM Authors";

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

        public List<AuthorDetails> GetAuthorDetailsList(out string errorMessage)
        {
            errorMessage = string.Empty;
            List<AuthorDetails> authorDetailsList = new List<AuthorDetails>();

            using (SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bokhanteringsdatabas;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=False"))
            {
                string sqlQuery = "SELECT AuthorID, Name FROM Authors";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                authorDetailsList.Add(new AuthorDetails
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
                }
            }

            return authorDetailsList;
        }


    }
}
