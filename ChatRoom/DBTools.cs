using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ChatRoom
{
    static class DBTools
    {
        private static DataSet mDataSet;
        public static DataSet MDataSet { get { return mDataSet; } }
        private static string connString = ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString;
        public static void Select()
        {
            
            
            mDataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select * from AccountInfo",sqlConnection))
                {
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
                    {
                        sqlDataAdapter.SelectCommand = sqlCommand;
                        sqlDataAdapter.Fill(mDataSet);
                        string a = mDataSet.Tables[0].Rows[0]["account"].ToString();
                    }
                }
            }
        }
        public static bool IsAccountInfoRight(string account, string password)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select account,pwd from AccountInfo where account=@p1 and pwd=@p2;", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddRange(new SqlParameter[]{ new SqlParameter("@p1", SqlDbType.NVarChar) { Value = account}, new SqlParameter("@p2", SqlDbType.NVarChar) { Value = password},});
                    sqlCommand.Connection = sqlConnection;
                    
                    SqlDataReader sdr = sqlCommand.ExecuteReader();
                    
                    if (sdr.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public static bool isAccountExist(string setUpAccount)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select account,pwd from AccountInfo where account=@p1;", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddRange(new SqlParameter[] { new SqlParameter("@p1", SqlDbType.NVarChar) { Value = setUpAccount } });
                    sqlCommand.Connection = sqlConnection;

                    SqlDataReader sdr = sqlCommand.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public static string GetNickNameByAccount(string account)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select nick from AccountInfo where account=@p1;", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddRange(new SqlParameter[] { new SqlParameter("@p1", SqlDbType.NVarChar) { Value = account }});
                    sqlCommand.Connection = sqlConnection;

                    string str = sqlCommand.ExecuteScalar() as string;

                    if (str != null)
                    {
                        return str;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static bool SetUp(string nick, string account, string pwd)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("insert into AccountInfo (nick, account, pwd) values(@nick, @account, @pwd);", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddRange(new SqlParameter[] { new SqlParameter("@nick", SqlDbType.NVarChar) { Value = nick } ,
                    new SqlParameter("@account", SqlDbType.NVarChar) { Value = account },
                    new SqlParameter("@pwd", SqlDbType.NVarChar) { Value = pwd }});

                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        //防止恶意注册有待实现


    }
}
