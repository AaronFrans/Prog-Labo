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
            Dictionary<string, List<int>> alleStraten = new Dictionary<string, List<int>>();
            foreach (Provincie provincie in toInsert)
            {
                foreach (Gemeente gemeente in provincie.m_Gemeenten)
                {
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
                        alleStraten.Add(straat.m_Naam, new List<int>() { straat.m_StraatID, straat.m_Graaf.m_GraafID });
                    }
                }
            }
            Console.WriteLine("alle lijsten klaar");
            toInsert = null;
            InsertPunten(allePunten);
            Console.WriteLine("alle punten klaar");
            allePunten = null;
            InsertKnopen(alleKnopen);
            Console.WriteLine("alle Knopen klaar");
            alleKnopen = null;
            InsertSegmenten(alleSegmenten);
            Console.WriteLine("alle Segmenten klaar");
            alleSegmenten = null;
            InsertGraven(alleGraven);
            Console.WriteLine("alle Graven klaar");
            alleGraven = null;
            InsertStraten(alleStraten);
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
                        foreach(var pair in graaf.m_Map)
                        {
                            foreach(Segment segment in pair.Value)
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
        private void InsertStraten(Dictionary<string, List<int>> toInsert)
        {

        }
    }
}
