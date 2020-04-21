using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;


namespace Wembsite.Models
{
    public class CRUD
    {
        public static string connectionString = "data source=localhost; Initial Catalog=Wembsite;Integrated Security=true";
        public static SqlConnection connect = new SqlConnection(connectionString);
        //public static List<User> getAllUsers()
        //{
        //    connect.Open();
        //    SqlCommand cmd;
        //    try
        //    {
        //        cmd = new SqlCommand("ViewUsers", connect);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        SqlDataReader rdr = cmd.ExecuteReader();

        //        List<User> list = new List<User>();
        //        while (rdr.Read())
        //        {
        //            User user = new User();

        //            user.userId = rdr["userId"].ToString();
        //            user.password = rdr["password"].ToString();
        //            user.dateOfBirth = rdr["dateOfBirth"].ToString();
        //            list.Add(user);
        //        }
        //        rdr.Close();
        //        con.Close();

        //        return list;


        //    }

        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine("SQL Error" + ex.Message.ToString());
        //        return null;

        //    }

        //}
        public static int signUp(string username, string firstname, string lastname, string email, string password)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("NewUser", connect);
            int result = 0;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UName", SqlDbType.NVarChar, 30).Value = username;
            cmd.Parameters.Add("@FName", SqlDbType.NVarChar, 20).Value = firstname;
            cmd.Parameters.Add("@LName", SqlDbType.NVarChar, 20).Value = lastname;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 30).Value = email;
            cmd.Parameters.Add("@Pass", SqlDbType.NVarChar, 30).Value = password;

            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
            return result = Convert.ToInt32(cmd.Parameters["@Out"].Value);

        }
        //public static int Login(string userId, string password)
        //{
        //    SqlConnection con = new SqlConnection(connectionString);
        //    con.Open();
        //    SqlCommand cmd;
        //    int result = 0;

        //    try
        //    {
        //        cmd = new SqlCommand("UserLoginProc", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@userId", SqlDbType.NVarChar, 50).Value = userId;
        //        cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password;


        //        cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

        //        cmd.ExecuteNonQuery();
        //        result = Convert.ToInt32(cmd.Parameters["@output"].Value);



        //    }

        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine("SQL Error" + ex.Message.ToString());
        //        result = -1; //-1 will be interpreted as "error while connecting with the database."
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;

        //}

        public static List<User> AllUsers()
        {
            connect.Open();
            SqlCommand cmd;

            cmd = new SqlCommand("AllUsers", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader rdr = cmd.ExecuteReader();

            List<User> list = new List<User>();
            while (rdr.Read())
            {
                User user = new User();

                user.username = rdr["username"].ToString();
                user.password = rdr["upassword"].ToString();
                user.firstname = rdr["firstname"].ToString();
                user.lastname = rdr["lastname"].ToString();
                user.email = rdr["email"].ToString();
                list.Add(user);
            }
            rdr.Close();
            connect.Close();

            return list;
        }

        public static User getUser(string username)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("getUser", connect);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
            
            SqlDataReader rdr=cmd.ExecuteReader();
            User user = new User();
            if(rdr.Read())
            {
                user.username = rdr["username"].ToString();
                user.password = rdr["upassword"].ToString();
                user.firstname = rdr["firstname"].ToString();
                user.lastname = rdr["lastname"].ToString();
                user.email = rdr["email"].ToString();
                rdr.Close();
                connect.Close();
            }
           
            return user;
        }

        public static void deleteUser(string username)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("DeleteUser", connect);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = username;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            

        }

    }
}