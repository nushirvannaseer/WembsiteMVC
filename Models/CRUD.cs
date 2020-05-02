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
            connect.Close();

        }

        public static User AnonymousObject(User usr)
        {
            return new User
            {
                username = usr.username,
                firstname = usr.firstname,
                lastname = usr.lastname,
                email = usr.email,
                password = usr.password
            };
        }

        public static List<User> DisplayFollowersOfAUser(string username)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from following where usernameA=@username";
            cmd.Parameters.AddWithValue("@username", username);
            //cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader= cmd.ExecuteReader();
            List<User> users= new List<User>();
            while(reader.Read())
            {
                User usr = new User();
                usr.username = reader["usernameB"].ToString();
                users.Add(usr);
            }
            reader.Close();
            connect.Close();
            return users;
        }

        public static List<User> DisplayFollowingsOfAUser(string username)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from following where usernameB=@username";
            cmd.Parameters.AddWithValue("@username", username);
            //cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                User usr = new User();
                usr.username = reader["usernameA"].ToString();
                users.Add(usr);
            }
            reader.Close();
            connect.Close();
            return users;
        }

        public static List<User> DisplayFollowRequestsOfAUser(string username)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from followRequests where receiver=@username";
            cmd.Parameters.AddWithValue("@username", username);
            //cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                User usr = new User();
                usr.username = reader["sender"].ToString();
                users.Add(usr);
            }
            reader.Close();
            connect.Close();
            return users;
        }

        public static void SendFollowRequest(string sender, string receiver)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "insert into followRequests values(@usernameA, @usernameB)";
            cmd.Parameters.AddWithValue("@usernameA", sender);
            cmd.Parameters.AddWithValue("@usernameB", receiver);
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static bool RequestSent(string usernameA, string usernameB)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from followRequests where sender = @usernameA and receiver = @usernameB";
            cmd.Parameters.AddWithValue("@usernameA", usernameA);
            cmd.Parameters.AddWithValue("@usernameB", usernameB);
            SqlDataReader r=cmd.ExecuteReader();
            if (r.Read())
            {
                r.Close();
                connect.Close();
                return true;
            }

            else
            {
                r.Close();
                connect.Close();
                return false;
            }
        }

        public static void CancelFollowRequest(string sender, string receiver)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "delete from followRequests where sender=@usernameA and receiver= @usernameB";
            cmd.Parameters.AddWithValue("@usernameA", sender);
            cmd.Parameters.AddWithValue("@usernameB", receiver);
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static void AcceptFollowRequest(string sender, string receiver)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "delete from followRequests where sender=@usernameA and receiver= @usernameB";
            cmd.Parameters.AddWithValue("@usernameA", sender);
            cmd.Parameters.AddWithValue("@usernameB", receiver);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("NewFollower", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserA", SqlDbType.VarChar, 30).Value = receiver;
            cmd.Parameters.Add("@UserB", SqlDbType.VarChar, 30).Value = sender;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static bool IsFollowing(string usernameA, string usernameB)//does userA follow userB
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from following where usernameA = @usernameB and usernameB = @usernameA";
            cmd.Parameters.AddWithValue("@usernameA", usernameA);
            cmd.Parameters.AddWithValue("@usernameB", usernameB);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                r.Close();
                connect.Close();
                return true;
            }

            else
            {
                r.Close();
                connect.Close();
                return false;
            }
        }

        public static void DeleteFollower(string followee, string follower)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("DeleteFollower", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserA", SqlDbType.VarChar, 30).Value = followee;
            cmd.Parameters.Add("@UserB", SqlDbType.VarChar, 30).Value = follower;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static void NewPost(string user, string post)
        {
            connect.Open();
            SqlCommand cmd = new SqlCommand("AddPost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UName", SqlDbType.VarChar, 30).Value = user;
            cmd.Parameters.Add("@privacy", SqlDbType.VarChar, 20).Value = "public";
            cmd.Parameters.Add("@RData", SqlDbType.VarChar, 100).Value = post;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

    }
}