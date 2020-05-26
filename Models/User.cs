using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Wembsite.Models
{
    
    public class User
    {
        
        [Key]
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public User() { }

        public User(string uname, string fname, string lname, string em, string upwd)
        {
            username = uname;
            firstname = fname;
            lastname = lname;
            email = em;
            password = upwd;
        }
    }

    public class UserContent
    {
        [Key]
        public int contentID { get; set; }
        public string username { get; set; }
        public string privacy { get; set; }
        public DateTime DateCreation { get; set; }
        public string FileType { get; set; }
        public string RawData { get; set; }
        public int likes { get; set; }
        public string filePath { get; set; }


        public UserContent() { }

    }

    public class Comment
    {
        [Key]
        public int commentID { get; set; }
        public int contentID { get; set; }
        public string commentedBy { get; set; }
        public string commentText { get; set; }

        public Comment() { }

    }

    public class UserDBContext : DbContext
    {
        public DbSet<User> userList { get; set; }

        public System.Data.Entity.DbSet<Wembsite.Models.UserContent> UserContents { get; set; }
    }

    public class UserContentDBContext : DbContext
    {
        public DbSet<UserContent> userContentList { get; set; }
    }

    public class CommentDBContext : DbContext
    {
        public DbSet<Comment> commentList { get; set; }
    }



}