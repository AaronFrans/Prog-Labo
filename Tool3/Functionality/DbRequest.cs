using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Tool3
{
    class DbRequest
    {
        private readonly string connectionString;
        public DbRequest(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void RequestStraatIDs(string gemeenteNaam)
        {
            List<int> straatIDs = RequestStraatIDsForGemeenteNaam(gemeenteNaam);

            Console.WriteLine("*****************************");
            Console.WriteLine("StraatIDs van gemeente {0}", gemeenteNaam);
            foreach (var straatdID in straatIDs)
            {
                Console.WriteLine(straatdID);
            }
            Console.WriteLine("*****************************");
        }
        private List<int> RequestStraatIDsForGemeenteNaam(string gemeenteNaam)
        {
            List<int> toReturn = new List<int>();

            string query = "SELECT dbo.GemeenteStraaten.StraatID " +
                           "FROM dbo.GemeenteStraaten " +
                           "INNER JOIN dbo.Gemeente " +
                           "ON dbo.Gemeente.ID = dbo.GemeenteStraaten.GemeenteID " +
                           "WHERE dbo.Gemeente.Naam = @gemeenteNaam " +
                           "ORDER BY dbo.GemeenteStraaten.StraatID ASC;";


            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@gemeenteNaam", gemeenteNaam);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    int toAdd = (int)reader["StraatID"];
                    toReturn.Add(toAdd);
                }

                reader.Close();

                connection.Close();
            }

            return toReturn;

        }
    }
}
