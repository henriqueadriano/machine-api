using Dapper;
using machine_api.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace machine_api.DataBase
{
    public class DB_Sqlite
    {

        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "./DataBase/FirstDB.db"; }
        }
        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }

        public void TestDB()
        {
            using (var cnn = SimpleDbConnection())
            {

                cnn.Open();
                var createCommand = cnn.CreateCommand();
                createCommand.CommandText = "select * from person";


                using (var reader = createCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var message = reader.GetString(1);
                        Console.WriteLine(message);
                    }
                }

            }
        }

        public IList<User> GetAllUsers()
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                var result = cnn.Query<User>(
                    @"select * from User").ToList();
                return result;
            }
        }


    }
}


//using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
//{
//    connection.Open();

//Create a table (drop if already exists first):

//var delTableCmd = connection.CreateCommand();
//delTableCmd.CommandText = "DROP TABLE IF EXISTS favorite_beers";
//delTableCmd.ExecuteNonQuery();

//var createTableCmd = connection.CreateCommand();
//createTableCmd.CommandText = "CREATE TABLE favorite_beers(name VARCHAR(50))";
//createTableCmd.ExecuteNonQuery();

////Seed some data:
//using (var transaction = connection.BeginTransaction())
//{
//    var insertCmd = connection.CreateCommand();

//    insertCmd.CommandText = "INSERT INTO favorite_beers VALUES('LAGUNITAS IPA')";
//    insertCmd.ExecuteNonQuery();

//    insertCmd.CommandText = "INSERT INTO favorite_beers VALUES('JAI ALAI IPA')";
//    insertCmd.ExecuteNonQuery();

//    insertCmd.CommandText = "INSERT INTO favorite_beers VALUES('RANGER IPA')";
//    insertCmd.ExecuteNonQuery();

//    transaction.Commit();
//}

//Read the newly inserted data:
//var selectCmd = connection.CreateCommand();
//selectCmd.CommandText = "select * from person";

//using (var reader = selectCmd.ExecuteReader())
//{
//    while (reader.Read())
//    {
//        var message = reader.GetString(1);
//        Console.WriteLine(message);
//    }
//}
