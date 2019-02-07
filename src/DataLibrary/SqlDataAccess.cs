using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;

namespace DataLibrary
{
    //https://www.youtube.com/watch?v=bIiEv__QNxw&t=2095s
    public static class SqlDataAccess
    {
        private static string ConnectionString()
        {
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=alpha;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //return ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        }

        public static List<T> Load<T>(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                return dbConnection.Query<T>(sql).ToList();
            }
        }

        public static List<T> Load<T>(string sql, T data)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                return dbConnection.Query<T>(sql, data).ToList();
            }
        }

        public static int Save<T>(string sql, T data)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                return dbConnection.Execute(sql, data);
            }
        }
    }
}
