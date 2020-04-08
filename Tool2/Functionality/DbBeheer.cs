using Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Tool2.Utilities
{
    class DbBeheer
    {

        private readonly string connectionString;
        public DbBeheer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public void InsertIntoDB(List<Provincie> toInsert)
        {


            List<Punt> allePunten = new List<Punt>();
            List<Knoop> alleKnopen = new List<Knoop>();
            List<Segment> alleSegmenten = new List<Segment>();
            List<Graaf> alleGraven = new List<Graaf>();
            Dictionary<int, KeyValuePair<string, int>> alleStraten = new Dictionary<int, KeyValuePair<string, int>>();
            Dictionary<int, string> alleGemeentenNamen = new Dictionary<int, string>();
            Dictionary<int, List<int>> alleStratenPerGemeente = new Dictionary<int, List<int>>();
            Dictionary<int, string> alleProvincieNamen = new Dictionary<int, string>();
            Dictionary<int, List<int>> alleGemeentenPerProvincie = new Dictionary<int, List<int>>();

            foreach (Provincie provincie in toInsert)
            {
                if (!alleGemeentenPerProvincie.ContainsKey(provincie.m_ProvincieID))
                {
                    alleGemeentenPerProvincie.Add(provincie.m_ProvincieID, new List<int>());
                }
                foreach (Gemeente gemeente in provincie.m_Gemeenten)
                {
                    if (!alleStratenPerGemeente.ContainsKey(gemeente.m_GemeenteID))
                    {
                        alleStratenPerGemeente.Add(gemeente.m_GemeenteID, new List<int>());
                    }
                    foreach (Straat straat in gemeente.m_Straten)
                    {
                        foreach (var map in straat.m_Graaf.m_Map)
                        {
                            alleKnopen.Add(map.Key);
                            foreach (Segment segment in map.Value)
                            {
                                foreach (Punt punt in segment.m_Vertices)
                                {
                                    allePunten.Add(punt);
                                }
                                alleKnopen.Add(segment.m_EindKnoop);
                                alleSegmenten.Add(segment);
                            }
                        }
                        alleGraven.Add(straat.m_Graaf);
                        alleStraten.Add(straat.m_StraatID, new KeyValuePair<string, int>(straat.m_Naam, straat.m_Graaf.m_GraafID));
                        alleStratenPerGemeente[gemeente.m_GemeenteID].Add(straat.m_StraatID);
                    }
                    alleGemeentenNamen.Add(gemeente.m_GemeenteID, gemeente.m_Naam);
                    alleGemeentenPerProvincie[provincie.m_ProvincieID].Add(gemeente.m_GemeenteID);
                }
                alleProvincieNamen.Add(provincie.m_ProvincieID, provincie.m_Naam);
            }
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Lijsten klaar");
            Console.WriteLine("*****************************************************");
            toInsert = null;
            InsertPunten(allePunten);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Punten klaar");
            Console.WriteLine("*****************************************************");
            allePunten = null;
            InsertKnopen(alleKnopen);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Knopen klaar");
            Console.WriteLine("*****************************************************");
            alleKnopen = null;
            InsertSegmenten(alleSegmenten);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Segmenten klaar");
            Console.WriteLine("*****************************************************");
            alleSegmenten = null;
            InsertGraven(alleGraven);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Graven klaar");
            Console.WriteLine("*****************************************************");
            alleGraven = null;
            InsertStraten(alleStraten);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Straten klaar");
            Console.WriteLine("*****************************************************");
            alleStraten = null;
            InsertGemeenten(alleGemeentenNamen, alleStratenPerGemeente);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Alle Gemeenten klaar");
            Console.WriteLine("*****************************************************");
            alleGemeentenNamen = null;
            alleStratenPerGemeente = null;
            InsertProvincies(alleProvincieNamen, alleGemeentenPerProvincie);
            Console.WriteLine("*****************************************************");
            Console.WriteLine("alle Provincies klaar");
            Console.WriteLine("*****************************************************");
            alleProvincieNamen = null;
            alleGemeentenPerProvincie = null;
        }

        private void InsertPunten(List<Punt> toInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("X", typeof(double));
                    table.Columns.Add("Y", typeof(double));

                    DataColumn[] keyColumns = new DataColumn[2];
                    keyColumns[0] = table.Columns["X"];
                    keyColumns[1] = table.Columns["Y"];
                    table.PrimaryKey = keyColumns;
                    bulkCopy.BulkCopyTimeout = 0;
                    int counter = 1;
                    foreach (Punt punt in toInsert)
                    {
                        Console.WriteLine("Punt " + counter + " out of " + toInsert.Count);
                        counter++;
                        object[] keyVals = new object[] { punt.m_X, punt.m_Y };

                        if (!table.Rows.Contains(keyVals))
                        {
                            DataRow row = table.NewRow();
                            row["X"] = punt.m_X;
                            row["Y"] = punt.m_Y;
                            table.Rows.Add(row);
                        }
                    }
                    bulkCopy.DestinationTableName = "Punt";
                    bulkCopy.ColumnMappings.Add("X", "X");
                    bulkCopy.ColumnMappings.Add("Y", "Y");
                    bulkCopy.WriteToServer(table);
                }
            }
        }
        private void InsertKnopen(List<Knoop> toInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID", typeof(int));
                    table.Columns.Add("PuntX", typeof(double));
                    table.Columns.Add("PuntY", typeof(double));

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["ID"];
                    table.PrimaryKey = keyColumns;

                    bulkCopy.BulkCopyTimeout = 0;

                    int counter = 1;
                    foreach (Knoop knoop in toInsert)
                    {
                        Console.WriteLine("Knoop " + counter + " out of " + toInsert.Count);
                        counter++;

                        if (!table.Rows.Contains(knoop.m_KnoopID))
                        {
                            DataRow row = table.NewRow();
                            row["ID"] = knoop.m_KnoopID;
                            row["PuntX"] = knoop.m_Punt.m_X;
                            row["PuntY"] = knoop.m_Punt.m_Y;
                            table.Rows.Add(row);
                        }
                    }

                    bulkCopy.DestinationTableName = "Knoop";
                    bulkCopy.ColumnMappings.Add("ID", "ID");
                    bulkCopy.ColumnMappings.Add("PuntX", "PuntX");
                    bulkCopy.ColumnMappings.Add("PuntY", "PuntY");
                    bulkCopy.WriteToServer(table);
                }
            }
        }
        private void InsertSegmenten(List<Segment> toInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID", typeof(int));
                    table.Columns.Add("BeginknoopID", typeof(int));
                    table.Columns.Add("EindknoopID", typeof(int));

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["ID"];
                    table.PrimaryKey = keyColumns;

                    bulkCopy.BulkCopyTimeout = 0;

                    int counter = 1;
                    foreach (Segment segment in toInsert)
                    {
                        Console.WriteLine("Segment " + counter + " out of " + toInsert.Count);
                        counter++;

                        if (!table.Rows.Contains(segment.m_SegmentID))
                        {
                            DataRow row = table.NewRow();
                            row["ID"] = segment.m_SegmentID;
                            row["BeginknoopID"] = segment.m_BeginKnoop.m_KnoopID;
                            row["EindknoopID"] = segment.m_EindKnoop.m_KnoopID;
                            table.Rows.Add(row);
                        }
                    }

                    bulkCopy.DestinationTableName = "Segment";
                    bulkCopy.ColumnMappings.Add("ID", "ID");
                    bulkCopy.ColumnMappings.Add("BeginknoopID", "BeginknoopID");
                    bulkCopy.ColumnMappings.Add("EindknoopID", "EindknoopID");
                    bulkCopy.WriteToServer(table);
                }


                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("SegmentID", typeof(int));
                    table.Columns.Add("PuntX", typeof(double));
                    table.Columns.Add("PuntY", typeof(double));

                    bulkCopy.BulkCopyTimeout = 0;
                    int counter = 1;
                    foreach (Segment segment in toInsert)
                    {
                        Console.WriteLine("Segment vertices " + counter + " out of " + toInsert.Count);
                        counter++;
                        foreach (Punt punt in segment.m_Vertices)
                        {
                            DataRow row = table.NewRow();
                            row["SegmentID"] = segment.m_SegmentID;
                            row["PuntX"] = punt.m_X;
                            row["PuntY"] = punt.m_Y;
                            table.Rows.Add(row);
                        }

                    }
                    bulkCopy.DestinationTableName = "SegmentVertices";
                    bulkCopy.ColumnMappings.Add("SegmentID", "SegmentID");
                    bulkCopy.ColumnMappings.Add("PuntX", "PuntX");
                    bulkCopy.ColumnMappings.Add("PuntY", "PuntY");
                    bulkCopy.WriteToServer(table);
                }
            }
        }
        private void InsertGraven(List<Graaf> toInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID", typeof(int));

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["ID"];
                    table.PrimaryKey = keyColumns;

                    bulkCopy.BulkCopyTimeout = 0;

                    int counter = 1;
                    foreach (Graaf graaf in toInsert)
                    {
                        Console.WriteLine("Graaf " + counter + " out of " + toInsert.Count);
                        counter++;

                        if (!table.Rows.Contains(graaf.m_GraafID))
                        {
                            DataRow row = table.NewRow();
                            row["ID"] = graaf.m_GraafID;
                            table.Rows.Add(row);
                        }
                    }
                    bulkCopy.DestinationTableName = "Graaf";
                    bulkCopy.ColumnMappings.Add("ID", "ID");
                    bulkCopy.WriteToServer(table);
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("GraafID", typeof(int));
                    table.Columns.Add("SegmentID", typeof(int));

                    bulkCopy.BulkCopyTimeout = 0;
                    int counter = 1;

                    foreach (Graaf graaf in toInsert)
                    {
                        Console.WriteLine("Graaf Map " + counter + " out of " + toInsert.Count);
                        counter++;
                        foreach (var pair in graaf.m_Map)
                        {
                            foreach (Segment segment in pair.Value)
                            {
                                DataRow row = table.NewRow();
                                row["GraafID"] = graaf.m_GraafID;
                                row["SegmentID"] = segment.m_SegmentID;
                                table.Rows.Add(row);
                            }
                        }

                    }
                    bulkCopy.DestinationTableName = "GraafMap";
                    bulkCopy.ColumnMappings.Add("GraafID", "GraafID");
                    bulkCopy.ColumnMappings.Add("SegmentID", "SegmentID");
                    bulkCopy.WriteToServer(table);
                }
            }

        }
        private void InsertStraten(Dictionary<int, KeyValuePair<string, int>> toInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID", typeof(int));
                    table.Columns.Add("Naam", typeof(string));
                    table.Columns.Add("GraafID", typeof(int));

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["ID"];
                    table.PrimaryKey = keyColumns;

                    bulkCopy.BulkCopyTimeout = 0;
                    int counter = 1;

                    foreach (var straatInfo in toInsert)
                    {
                        Console.WriteLine("Straat " + counter + " out of " + toInsert.Count);
                        counter++;
                        DataRow row = table.NewRow();
                        row["ID"] = straatInfo.Key;
                        row["Naam"] = straatInfo.Value.Key;
                        row["GraafID"] = straatInfo.Value.Value;
                        table.Rows.Add(row);
                    }
                    bulkCopy.DestinationTableName = "Straat";
                    bulkCopy.ColumnMappings.Add("ID", "ID");
                    bulkCopy.ColumnMappings.Add("Naam", "Naam");
                    bulkCopy.ColumnMappings.Add("GraafID", "GraafID");
                    bulkCopy.WriteToServer(table);
                }
            }

        }
        private void InsertGemeenten(Dictionary<int, string> gemeentenToInsert, Dictionary<int, List<int>> gemeenteStratenToInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID", typeof(int));
                    table.Columns.Add("Naam", typeof(string));

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["ID"];
                    table.PrimaryKey = keyColumns;

                    bulkCopy.BulkCopyTimeout = 0;

                    int counter = 1;
                    foreach (var pair in gemeentenToInsert)
                    {
                        Console.WriteLine("Gemeente " + counter + " out of " + gemeentenToInsert.Count);
                        counter++;

                        if (!table.Rows.Contains(pair.Key))
                        {
                            DataRow row = table.NewRow();
                            row["ID"] = pair.Key;
                            row["Naam"] = pair.Value;
                            table.Rows.Add(row);
                        }
                    }
                    bulkCopy.DestinationTableName = "Gemeente";
                    bulkCopy.ColumnMappings.Add("ID", "ID");
                    bulkCopy.ColumnMappings.Add("Naam", "Naam");
                    bulkCopy.WriteToServer(table);
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("GemeenteID", typeof(int));
                    table.Columns.Add("StraatID", typeof(int));

                    bulkCopy.BulkCopyTimeout = 0;
                    int counter = 1;

                    foreach (var pair in gemeenteStratenToInsert)
                    {
                        Console.WriteLine("Gemeente straten " + counter + " out of " + gemeenteStratenToInsert.Count);
                        counter++;
                        foreach (int StraatID in pair.Value)
                        {
                            DataRow row = table.NewRow();
                            row["GemeenteID"] = pair.Key;
                            row["StraatID"] = StraatID;
                            table.Rows.Add(row);

                        }

                    }
                    bulkCopy.DestinationTableName = "GemeenteStraaten";
                    bulkCopy.ColumnMappings.Add("GemeenteID", "GemeenteID");
                    bulkCopy.ColumnMappings.Add("StraatID", "StraatID");
                    bulkCopy.WriteToServer(table);
                }
            }
        }
        private void InsertProvincies(Dictionary<int, string> provinciesToInsert, Dictionary<int, List<int>> provincieGemeenteToInsert)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID", typeof(int));
                    table.Columns.Add("Naam", typeof(string));

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["ID"];
                    table.PrimaryKey = keyColumns;

                    bulkCopy.BulkCopyTimeout = 0;

                    int counter = 1;
                    foreach (var pair in provinciesToInsert)
                    {
                        Console.WriteLine("Provincie " + counter + " out of " + provinciesToInsert.Count);
                        counter++;

                        if (!table.Rows.Contains(pair.Key))
                        {
                            DataRow row = table.NewRow();
                            row["ID"] = pair.Key;
                            row["Naam"] = pair.Value;
                            table.Rows.Add(row);
                        }
                    }
                    bulkCopy.DestinationTableName = "Provincie";
                    bulkCopy.ColumnMappings.Add("ID", "ID");
                    bulkCopy.ColumnMappings.Add("Naam", "Naam");
                    bulkCopy.WriteToServer(table);
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ProvincieID", typeof(int));
                    table.Columns.Add("GemeenteID", typeof(int));

                    bulkCopy.BulkCopyTimeout = 0;
                    int counter = 1;

                    foreach (var pair in provincieGemeenteToInsert)
                    {
                        Console.WriteLine("Provincie gemeenten " + counter + " out of " + provincieGemeenteToInsert.Count);
                        counter++;
                        foreach (int StraatID in pair.Value)
                        {
                            DataRow row = table.NewRow();
                            row["ProvincieID"] = pair.Key;
                            row["GemeenteID"] = StraatID;
                            table.Rows.Add(row);

                        }

                    }
                    bulkCopy.DestinationTableName = "ProvincieGemeenten";
                    bulkCopy.ColumnMappings.Add("ProvincieID", "ProvincieID");
                    bulkCopy.ColumnMappings.Add("GemeenteID", "GemeenteID");
                    bulkCopy.WriteToServer(table);
                }
            }
        }

    }
}
