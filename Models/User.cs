﻿using System;
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

    
    public class UserDBContext : DbContext
    {
        public DbSet<User> userList { get; set; }
    }

    //public class UserFollowing : User
    //{
    //    [Key]
    //    public int contentID { get; set; }
    //    public bool onlyMe { get; set; }
    //    public DateTime DateCreation { get; set; }
    //    public string fileType { get; set; }

    //}

}