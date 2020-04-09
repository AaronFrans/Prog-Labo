using Objects;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
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

        #region stratenViaGemeenteNamen
        public void RequestStraatIDs(string gemeenteNaam)
        {
            List<int> straatIDs = RequestStraatIDsForGemeenteNaam(gemeenteNaam);

            if (straatIDs != null)
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("StraatIDs van gemeente {0}", gemeenteNaam);
                foreach (var straatdID in straatIDs)
                {
                    Console.WriteLine(straatdID);
                }
                Console.WriteLine("*****************************");
            }
            else
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("Geen straten gevonden voor gemeente {0}", gemeenteNaam);
                Console.WriteLine("*****************************");
            }
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
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int toAdd = (int)reader["StraatID"];
                        toReturn.Add(toAdd);
                    }
                }
                else
                {
                    toReturn = null;
                }


                reader.Close();

                connection.Close();
            }

            return toReturn;

        }
        #endregion

        #region straatViaID
        public void RequestStraat(int straatID)
        {
            Straat straatRequested = RequestStraatWithStraatID(straatID);
            if (straatRequested != null)
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("Straat met id {0}", straatID);
                straatRequested.ShowStraat();
                Console.WriteLine("*****************************");
            }
            else
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("Straat met id {0} kan niet gemaakt worden", straatID);
                Console.WriteLine("*****************************");
            }

        }
        private Straat RequestStraatWithStraatID(int straatID)
        {
            Straat toReturn = null;
            List<Segment> segmentensFromStraat = SegmentensFromStraat(straatID);

            string query = "SELECT dbo.Straat.* " +
                           "FROM dbo.Straat " +
                           "SHERE dbo.Straat.ID = @straatID;";

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@straatID", straatID);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    if (segmentensFromStraat != null)
                    {
                        while (reader.Read())
                        {
                            int graafID = (int)reader["GraafID"];
                            Graaf toReturnGraaf = Graaf.BuildGraaf(graafID, segmentensFromStraat);
                            int toReturnStraatID = (int)reader["ID"];
                            string toReturnStraatNaam = (string)reader["Naam"];
                            toReturn = new Straat(toReturnStraatID, toReturnStraatNaam, toReturnGraaf);
                        }
                    }
                    else
                    {
                        toReturn = null;
                    }
                }
                else
                {
                    toReturn = null;
                }
            }
            return toReturn;
        }
        private Dictionary<int, List<Punt>> VerticesForEachSegment(int straatID)
        {
            Dictionary<int, List<Punt>> toReturn = new Dictionary<int, List<Punt>>();
            string query = "SELECT dbo.Segment.ID as segmentID, dbo.SegmentVertices.PuntX, dbo.SegmentVertices.PuntY " +
                           "FROM dbo.Straat " +
                           "INNER JOIN dbo.Graaf " +
                           "ON dbo.graaf.ID = dbo.straat.GraafID " +
                           "INNER JOIN dbo.GraafMap " +
                           "ON dbo.graaf.ID = dbo.GraafMap.GraafID " +
                           "INNER JOIN dbo.Segment " +
                           "ON dbo.Segment.ID = dbo.GraafMap.SegmentID " +
                           "INNER JOIN dbo.SegmentVertices " +
                           "ON dbo.SegmentVertices.SegmentID = dbo.Segment.ID " +
                           "WHERE dbo.Straat.ID = @straatID;";



            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@straatID", straatID);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int segmentIDToAdd = (int)reader["segmentID"];
                        Punt puntToAdd = new Punt((double)reader["PuntX"], (double)reader["PuntY"]);
                        if (toReturn.ContainsKey(segmentIDToAdd))
                        {
                            toReturn[segmentIDToAdd].Add(puntToAdd);
                        }
                        else
                            toReturn.Add(segmentIDToAdd, new List<Punt>() { puntToAdd });
                    }
                }
                else
                {
                    toReturn = null;
                }
                reader.Close();

                connection.Close();

                return toReturn;
            }
        }
        private List<Segment> SegmentensFromStraat(int straatID)
        {
            List<Segment> toReturn = new List<Segment>();
            Dictionary<int, List<Punt>> verticesForEachSegment = VerticesForEachSegment(straatID);
            string query = "Select dbo.Segment.ID as segmentID, dbo.Segment.BeginknoopID, dbo.Segment.EindknoopID " +
                            "FROM dbo.Straat " +
                            "INNER JOIN dbo.Graaf " +
                            "ON dbo.graaf.ID = dbo.straat.GraafID " +
                            "INNER JOIN dbo.GraafMap " +
                            "ON dbo.graaf.ID = dbo.GraafMap.GraafID " +
                            "INNER JOIN dbo.Segment " +
                            "ON dbo.Segment.ID = dbo.GraafMap.SegmentID " +
                            "WHERE dbo.Straat.ID = @straatID;";


            using (SqlConnection connection = GetConnection())
            {




                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@straatID", straatID);


                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    if (verticesForEachSegment != null)
                    {
                        while (reader.Read())
                        {
                            int segmentID = (int)reader["segmentID"];
                            int beginKnoopID = (int)reader["BeginknoopID"];
                            int eindKnoopId = (int)reader["EindknoopID"];

                            Knoop beginKnoop = new Knoop(beginKnoopID, verticesForEachSegment[segmentID].First());
                            Knoop eindKnoop = new Knoop(eindKnoopId, verticesForEachSegment[segmentID].Last());
                            Segment toAdd = new Segment(segmentID, beginKnoop, eindKnoop, verticesForEachSegment[segmentID]);
                            toReturn.Add(toAdd);

                        }
                    }
                    else
                    {
                        toReturn = null;


                    }
                }
                else
                {
                    toReturn = null;


                }

                reader.Close();

                connection.Close();


                return toReturn;
            }
        }
        #endregion

        #region straatViaStraanEnGemeenteNaam
        public void RequestStraat(string straatNaam, string gemeenteNaam)
        {
            Straat straatRequested = RequestStraatWithStraatNaamAndGemeenteNaam(straatNaam, gemeenteNaam);
            if (straatRequested != null)
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("Straat met met naam {0} van gemeente met naam {1}", straatNaam, gemeenteNaam);
                straatRequested.ShowStraat();
                Console.WriteLine("*****************************");
            }
            else
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("Straat met met naam {0} van gemeente met naam {1} kan niet gemaakt worden", straatNaam , gemeenteNaam);
                Console.WriteLine("*****************************");
            }
        }
        private Straat RequestStraatWithStraatNaamAndGemeenteNaam(string straatNaam, string gemeenteNaam)
        {
            Straat toReturn = null;
            List<Segment> segmentensFromStraat = SegmentensFromStraat(straatNaam, gemeenteNaam);

            string query = "SELECT dbo.Straat.* " +
                           "FROM dbo.Straat " +
                           "INNER JOIN dbo.GemeenteStraaten " +
                           "ON dbo.Straat.ID = dbo.GemeenteStraaten.StraatID " +
                           "INNER JOIN dbo.Gemeente " +
                           "ON dbo.Gemeente.ID = dbo.GemeenteStraaten.GemeenteID " +
                           "WHERE dbo.Straat.Naam = @straatNaam " +
                           "AND dbo.Gemeente.Naam = @gemeenteNaam;";

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@straatNaam", straatNaam);
                command.Parameters.AddWithValue("@gemeenteNaam", gemeenteNaam);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    if (segmentensFromStraat != null)
                    {
                        while (reader.Read())
                        {
                            int graafID = (int)reader["GraafID"];
                            Graaf toReturnGraaf = Graaf.BuildGraaf(graafID, segmentensFromStraat);
                            int toReturnStraatID = (int)reader["ID"];
                            string toReturnStraatNaam = (string)reader["Naam"];
                            toReturn = new Straat(toReturnStraatID, toReturnStraatNaam, toReturnGraaf);
                        }
                    }
                    else
                    {
                        toReturn = null;
                    }
                }
                else
                {
                    toReturn = null;
                }
            }
            return toReturn;
        }
        private Dictionary<int, List<Punt>> VerticesForEachSegment(string straatNaam, string gemeenteNaam)
        {
            Dictionary<int, List<Punt>> toReturn = new Dictionary<int, List<Punt>>();
            string query = "SELECT dbo.Segment.ID as segmentID, dbo.SegmentVertices.PuntX, dbo.SegmentVertices.PuntY " +
                           "FROM dbo.Straat " +
                           "INNER JOIN dbo.Graaf " +
                           "ON dbo.graaf.ID = dbo.straat.GraafID " +
                           "INNER JOIN dbo.GraafMap " +
                           "ON dbo.graaf.ID = dbo.GraafMap.GraafID " +
                           "INNER JOIN dbo.Segment " +
                           "ON dbo.Segment.ID = dbo.GraafMap.SegmentID " +
                           "INNER JOIN dbo.SegmentVertices " +
                           "ON dbo.SegmentVertices.SegmentID = dbo.Segment.ID " +
                           "INNER JOIN dbo.GemeenteStraaten " +
                           "ON dbo.Straat.ID = dbo.GemeenteStraaten.StraatID " +
                           "INNER JOIN dbo.Gemeente " +
                           "ON dbo.Gemeente.ID = dbo.GemeenteStraaten.GemeenteID " +
                           "WHERE dbo.Straat.Naam = @straatNaam " +
                           "AND dbo.Gemeente.Naam = @gemeenteNaam;";



            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@straatNaam", straatNaam);
                command.Parameters.AddWithValue("@gemeenteNaam", gemeenteNaam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int segmentIDToAdd = (int)reader["segmentID"];
                        Punt puntToAdd = new Punt((double)reader["PuntX"], (double)reader["PuntY"]);
                        if (toReturn.ContainsKey(segmentIDToAdd))
                        {
                            toReturn[segmentIDToAdd].Add(puntToAdd);
                        }
                        else
                            toReturn.Add(segmentIDToAdd, new List<Punt>() { puntToAdd });
                    }
                }
                else
                {
                    toReturn = null;
                }
                reader.Close();

                connection.Close();

                return toReturn;
            }
        }
        List<Segment> SegmentensFromStraat(string straatNaam, string gemeenteNaam)
        {
            List<Segment> toReturn = new List<Segment>();
            Dictionary<int, List<Punt>> verticesForEachSegment = VerticesForEachSegment(straatNaam, gemeenteNaam);
            string query = "Select dbo.Segment.ID as segmentID, dbo.Segment.BeginknoopID, dbo.Segment.EindknoopID " +
                           "FROM dbo.Straat " +
                           "INNER JOIN dbo.Graaf " +
                           "ON dbo.graaf.ID = dbo.straat.GraafID " +
                           "INNER JOIN dbo.GraafMap " +
                           "ON dbo.graaf.ID = dbo.GraafMap.GraafID " +
                           "INNER JOIN dbo.Segment " +
                           "ON dbo.Segment.ID = dbo.GraafMap.SegmentID " +
                           "INNER JOIN dbo.GemeenteStraaten " +
                           "ON dbo.Straat.ID = dbo.GemeenteStraaten.StraatID " +
                           "INNER JOIN dbo.Gemeente " +
                           "ON dbo.Gemeente.ID = dbo.GemeenteStraaten.GemeenteID " +
                           "WHERE dbo.Straat.Naam = @straatNaam " +
                           "AND dbo.Gemeente.Naam = @gemeenteNaam;";


            using (SqlConnection connection = GetConnection())
            {




                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@straatNaam", straatNaam);
                command.Parameters.AddWithValue("@gemeenteNaam", gemeenteNaam);


                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    if (verticesForEachSegment != null)
                    {
                        while (reader.Read())
                        {
                            int segmentID = (int)reader["segmentID"];
                            int beginKnoopID = (int)reader["BeginknoopID"];
                            int eindKnoopId = (int)reader["EindknoopID"];

                            Knoop beginKnoop = new Knoop(beginKnoopID, verticesForEachSegment[segmentID].First());
                            Knoop eindKnoop = new Knoop(eindKnoopId, verticesForEachSegment[segmentID].Last());
                            Segment toAdd = new Segment(segmentID, beginKnoop, eindKnoop, verticesForEachSegment[segmentID]);
                            toReturn.Add(toAdd);

                        }
                    }
                    else
                    {
                        toReturn = null;


                    }
                }
                else
                {
                    toReturn = null;


                }

                reader.Close();

                connection.Close();


                return toReturn;
            }
        }
        #endregion

        #region stratenAlphabetischViaGemeenteID
        public void RequestStraatIDs(int gemeenteID)
        {
            List<string> straatNaamen = RequestStraatIDsForGemeenteNaam(gemeenteID);

            if (straatNaamen != null)
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("StraatIDs van gemeente met ID {0}", gemeenteID);
                foreach (var straatNaam in straatNaamen)
                {
                    Console.WriteLine(straatNaam);
                }
                Console.WriteLine("*****************************");
            }
            else
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("Geen straten gevonden voor gemeente met ID {0}", gemeenteID);
                Console.WriteLine("*****************************");
            }
        }
        private List<string> RequestStraatIDsForGemeenteNaam(int gemeenteID)
        {
            List<string> toReturn = new List<string>();

            string query = "SELECT dbo.Straat.Naam " +
                           "FROM dbo.GemeenteStraaten " +
                           "INNER JOIN dbo.Gemeente " +
                           "ON dbo.Gemeente.ID = dbo.GemeenteStraaten.GemeenteID " +
                           "INNER JOIN dbo.Straat " +
                           "ON dbo.Straat.ID = dbo.GemeenteStraaten.StraatID " +
                           "WHERE dbo.Gemeente.ID = @gemeenteID " +
                           "ORDER BY dbo.Straat.Naam ASC;";


            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@gemeenteID", gemeenteID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string toAdd = (string)reader["Naam"];
                        toReturn.Add(toAdd);
                    }
                }
                else
                {
                    toReturn = null;
                }


                reader.Close();

                connection.Close();
            }

            return toReturn;

        }
        #endregion

    }
}