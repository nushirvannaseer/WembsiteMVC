using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wembsite.Models;

namespace Wembsite.Data
{
    public class WembsiteContext: DbContext
    {
        public WembsiteContext(DbContextOptions<WembsiteContext> options)
            : base(options)
        {
        }

        public DbSet<User> UserList { get; set; }
    }
}
