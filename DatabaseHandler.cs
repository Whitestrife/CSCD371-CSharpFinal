//Cole Wolff
//Description:
//Manages initialization and read/writes to the database

using System;
using System.Collections;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;

namespace TetrisFinal
{
    class DatabaseHandler
    {

        static SQLiteCommand command = null;
        static SQLiteConnection connection = null;
        static ArrayList databaseWrites;       

        //Prepares the Database for Utilization
        public static void initializeDatabase()
        {
            string fileName = "TetrisScores.db";
            
            string connectionString = "Data Source=" + fileName + "; "+
                "Version = 3; New = True; Compress = True; ";           

            connection = new SQLiteConnection(connectionString);
            connection.Open();

            command = new SQLiteCommand(connection);

            command.CommandText = "CREATE TABLE if not exists scores(name STRING, " +
                "score INTEGER)";
            command.ExecuteNonQuery();

            databaseWrites = new ArrayList();           

        }

        //Database Access Methods
        public static void writeToDatabase()
        {
            for(int i = 0; i < 10; i++) 
            {
                if (databaseWrites.Count > i)
                {
                    String[] dbwrite = (String[])databaseWrites[i];
                    command = new SQLiteCommand(connection);
                    command.CommandText = "INSERT INTO scores(name, score) " +
                        "VALUES('" + dbwrite[0] + "', '" + Int32.Parse(dbwrite[1]) + "')";
                    command.ExecuteNonQuery();
                }
            }
            databaseWrites.Clear();                     
        }
        //Used to grab database elements to display
        public static ArrayList readDatabase()
        {
            command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM scores";
            SQLiteDataReader rdr;

            ArrayList databaseReads = new ArrayList();

            try {
                rdr = command.ExecuteReader();
            }catch(Exception e)
            {              
                return databaseReads;
            }
            
            while (rdr.Read())
            {
                DatabaseRead dbRead = new DatabaseRead() { Name = rdr.GetString(0), Score = rdr.GetInt32(1) };              
                databaseReads.Add(dbRead);               
            }
            
            return databaseReads;
        }

        //Mainly for testing purposes but has utility moving forward
        public static void clearDBTable(string tableName)
        {
            command = new SQLiteCommand(connection);
            command.CommandText = "DROP TABLE IF EXISTS " + tableName;
            command.ExecuteNonQuery();
        }

        //Stores each file change to be written once user decides to save changes
        public static void preWriteStorage(string name, int score)
        {
            String[] write = new String[2];
            write[0] = name;
            write[1] = score.ToString();           
            databaseWrites.Add(write);
        }
    }
    //Custom class for more easily packaging data to be displayed
    public class DatabaseRead : IComparable
    {
        public string Name { get; set; }
        public int Score { get; set; } 
       
        public int CompareTo(Object other)
        {
            DatabaseRead otherRead = (DatabaseRead)other;
            int returnValue = this.Score.CompareTo(otherRead.Score);
            return (returnValue * -1);
        }       
    }    
}
