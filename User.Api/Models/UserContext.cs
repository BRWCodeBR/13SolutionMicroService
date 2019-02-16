using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Models
{
    public class UserContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json")
            .Build();

            optionsBuilder.UseSqlServer(configuration.GetSection("AppSettings").GetSection("connectionString").Value);
        }

        public virtual ICollection<UserFood> UserFood { get; set; }

        public virtual ICollection<UserFace> UserFace { get; set; }

        public virtual ICollection<UserFoodRestriction> UserFoodRestriction { get; set; }

    }
}
