using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Wembsite.Models;
using System.Web.UI.WebControls;
using System.IO;

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
            if(connect.State==ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Close();
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("DeleteUser", connect);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = username;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();

        }

        public static List<User> DisplayFollowersOfAUser(string username)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
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
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("DeleteFollower", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserA", SqlDbType.VarChar, 30).Value = followee;
            cmd.Parameters.Add("@UserB", SqlDbType.VarChar, 30).Value = follower;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static void NewPost(string user, string post, string privacy, string path="", string FileT="")
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("AddPost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UName", SqlDbType.VarChar, 30).Value = user;
            cmd.Parameters.Add("@privacy", SqlDbType.VarChar, 20).Value = privacy;
            cmd.Parameters.Add("@RData", SqlDbType.VarChar, 8000).Value = post;
            cmd.Parameters.Add("@filepath", SqlDbType.VarChar, 300).Value = path;
            cmd.Parameters.Add("@FileT", SqlDbType.VarChar, 20).Value = FileT;

            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static List<UserContent> AllPostsOfAUser(string username)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from UserContent where username=@username order by DateCreation DESC";
            cmd.Parameters.AddWithValue("@username", username);
            SqlDataReader reader = cmd.ExecuteReader();

            List<UserContent> postList = new List<UserContent>();
            while(reader.Read())
            {
                UserContent post = new UserContent();
                post.contentID = Convert.ToInt32(reader["contentID"]);
                post.username = reader["username"].ToString();
                post.privacy = reader["privacy"].ToString();
                post.DateCreation = Convert.ToDateTime(reader["DateCreation"]);
                post.FileType = reader["FileType"].ToString();
                post.RawData = reader["RawData"].ToString();
                post.likes = Convert.ToInt32(reader["likes"]);
                post.filePath = reader["filePath"].ToString();
                postList.Add(post);
            }

            connect.Close();
            reader.Close();
            return postList;
        }

        public static List<UserContent> HomepagePost(string username)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();

            SqlCommand cmd = new SqlCommand("homepage", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@userID", SqlDbType.VarChar, 30).Value = username;
            SqlDataReader reader = cmd.ExecuteReader();
 

            List<UserContent> postList = new List<UserContent>();
            while(reader.Read())
            {
                UserContent post = new UserContent();
                post.contentID = Convert.ToInt32(reader["contentID"]);
                post.username = reader["username"].ToString();
                post.privacy = reader["privacy"].ToString();
                post.DateCreation = Convert.ToDateTime(reader["DateCreation"]);
                post.FileType = reader["FileType"].ToString();
                post.RawData = reader["RawData"].ToString();
                post.likes = Convert.ToInt32(reader["likes"]);
                post.filePath = reader["filePath"].ToString();
                postList.Add(post);
            }

            connect.Close();
            reader.Close();
            return postList;
 
        }

        public static UserContent GetUserPost(int id)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("", connect);
            cmd.CommandText = "select * from UserContent where contentID=@contentID";
            cmd.Parameters.AddWithValue("@contentID", id);
            SqlDataReader reader = cmd.ExecuteReader();
            UserContent post = new UserContent();
            if (reader.Read())
            {
                post.contentID = Convert.ToInt32(reader["contentID"]);
                post.username = reader["username"].ToString();
                post.privacy = reader["privacy"].ToString();
                post.DateCreation = Convert.ToDateTime(reader["DateCreation"]);
                post.FileType = reader["FileType"].ToString();
                post.RawData = reader["RawData"].ToString();
                post.likes = Convert.ToInt32(reader["likes"]);
                post.filePath = reader["filePath"].ToString();
            }

            connect.Close();
            reader.Close();
            return post;
        }

        public static void EditPost(int id, string privacy, string RawData)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("EditPost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@contID", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@privacy", SqlDbType.VarChar,20).Value = privacy;
            cmd.Parameters.Add("@RawData", SqlDbType.VarChar, 8000).Value = RawData;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static void DeletePost(int id)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("DeletePost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@ContID", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static void LikePost(int contentID, string likedBy )
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("likePost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@likedBy", SqlDbType.VarChar, 30).Value = likedBy;
            cmd.Parameters.Add("@contentID", SqlDbType.Int).Value = contentID;

            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static void UnLikePost(int contentID, string unlikedBy)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("unlikePost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@unlikedBy", SqlDbType.VarChar, 30).Value = unlikedBy;
            cmd.Parameters.Add("@contentID", SqlDbType.Int).Value = contentID;

            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public static List<User> GetLikeList(int contentID)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("GetLikeList", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@contentID", SqlDbType.Int).Value = contentID;
            SqlDataReader r= cmd.ExecuteReader();
            List<User> likers = new List<User>();

            while(r.Read())
            {
                User usr = new User();
                usr.username = r["likedBy"].ToString();
                likers.Add(usr);
            }
            r.Close();
            connect.Close();

            return likers;
            
        }

        public static List<UserContent> getNonSessionUserPosts(string username, string sessionUser)
        {
            int sessionUserIsFollower = 0;
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("getNonSessionUserPosts", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = username;
            if (IsFollowing(sessionUser, username))
            {
                sessionUserIsFollower = 1;
            }
            cmd.Parameters.Add("@sessionUserIsFollowing", SqlDbType.Int).Value = sessionUserIsFollower;
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlDataReader r = cmd.ExecuteReader();
            List<UserContent> posts = new List<UserContent>();

            while (r.Read())
            {
                UserContent post = new UserContent();
                post.contentID = Convert.ToInt32( r["contentID"]);
                post.username = r["username"].ToString();
                post.likes = Convert.ToInt32(r["likes"]);
                post.DateCreation = Convert.ToDateTime(r["DateCreation"]);
                post.privacy = r["privacy"].ToString();
                post.RawData = r["RawData"].ToString();
                post.filePath = r["filePath"].ToString();

                posts.Add(post);
            }
            r.Close();
            connect.Close();

            return posts;
        }

        public static List<User> Search(string searchText)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("Search", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@searchText", SqlDbType.VarChar, 50).Value = searchText;
            SqlDataReader r = cmd.ExecuteReader();
            List<User> likers = new List<User>();

            while (r.Read())
            {
                User usr = new User();
                usr.username = r["username"].ToString();
                likers.Add(usr);
            }
            r.Close();
            connect.Close();

            return likers;
        }

        public static List<Comment> GetCommentsOfAPost(int contentID)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("getCommentsOfAPost", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@contentID", SqlDbType.Int).Value = contentID;
            SqlDataReader r = cmd.ExecuteReader();
            List<Comment> cmnts = new List<Comment>();

            while (r.Read())
            {
                Comment cmnt = new Comment();
                cmnt.commentedBy = r["commentedBy"].ToString();
                cmnt.commentID=Convert.ToInt32(r["commentID"]);
                cmnt.contentID = Convert.ToInt32 (r["contentID"]);
                cmnt.commentText = r["commentText"].ToString();
                cmnts.Add(cmnt);
            }
            r.Close();
            connect.Close();

            return cmnts;
        }

        public static List<Comment> AddComment(int contentID, string commentedBy, string commentText)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }
            connect.Open();
            SqlCommand cmd = new SqlCommand("addComment", connect);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@postID", SqlDbType.Int).Value = contentID;
            cmd.Parameters.Add("@userIDD", SqlDbType.VarChar, 30).Value = commentedBy;
            cmd.Parameters.Add("@text", SqlDbType.VarChar, 3000).Value = commentText;
            cmd.Parameters.Add("@Out", SqlDbType.Int).Value = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            connect.Close();

            return GetCommentsOfAPost(contentID);
    }
    }
}