using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using DataLibrary.Models;
using System.Transactions;

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

        public static T LoadSingle<T>(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                return dbConnection.Query<T>(sql).Single();
            }
        }

        public static T LoadSingle<T>(string sql, T data)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                var result = dbConnection.Query<T>(sql, data);
                if(result.Count() == 0)
                {
                    return default(T);
                }
                return result.Single();
            }
        }

        //public static List<PatientModel> LoadPatientsWithDetails()
        //{
        //    string sql = @"SELECT P.Id, P.NHSNumber, P.Password, P.FK_PatientDetails_Id,
        //                        PD.Id, PD.Dob, PD.IsFemale, PD.IsBlack
        //                    FROM Patient P
        //                    INNER JOIN PatientDetails PD
        //                    ON P.FK_PatientDetails_Id = PD.Id";

        //    using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
        //    {
        //        return dbConnection.Query<PatientModel, PatientDetailsModel, PatientModel>(sql, (patient, patientDetails) =>
        //        {
        //            patient.Details = patientDetails;
        //            return patient;
        //        }).ToList();
        //    }
        //}

        //public static PatientModel LoadPatientWithDetails(int patientId)
        //{
        //    string sql = @"SELECT P.Id, P.NHSNumber, P.Password, P.FK_PatientDetails_Id,
        //                        PD.Id, PD.Dob, PD.IsFemale, PD.IsBlack
        //                    FROM Patient P
        //                    LEFT JOIN PatientDetails PD
        //                    ON P.FK_PatientDetails_Id = PD.Id
        //                    WHERE P.Id = @PatientId";

        //    PatientModel patientModel = null;
        //    using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
        //    {
        //        var patientList = dbConnection.Query<PatientModel, PatientDetailsModel, PatientModel>(sql, (patient, patientDetails) =>
        //        {
        //            patient.Details = patientDetails;
        //            return patient;
        //        }, new { PatientId = patientId }).ToList();

        //        if(patientList.Count != 0)
        //        {
        //            patientModel = patientList[0];
        //        }
        //    }

        //    return patientModel;
        //}

        public static int Save<T>(string sql, T data)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                return dbConnection.Execute(sql, data);
            }
        }

        public static int SaveList<T>(string sql, List<T> data)
        {
            int rowsAffected = 0;

            if(data != null && data.Count != 0)
            {
                using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
                {
                    dbConnection.Open();
                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            rowsAffected = dbConnection.Execute(sql, data, transaction: transaction);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Unique Index constraint has been violated
                            if (ex.GetBaseException().GetType() == typeof(SqlException))
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                    dbConnection.Close();
                }
            }

            return rowsAffected;
        }

        public static int SaveReturnId<T>(string sql, T data)
        {
            //https://stackoverflow.com/questions/8270205/how-do-i-perform-an-insert-and-return-inserted-identity-with-dapper
            sql += @"SELECT CAST(SCOPE_IDENTITY() as int)";

            using (IDbConnection dbConnection = new SqlConnection(ConnectionString()))
            {
                return dbConnection.Query<int>(sql, data).Single();
            }
        }

        public static int Execute<T>(string sql, T data)
        {
            return Save<T>(sql, data);
        }
    }
}
