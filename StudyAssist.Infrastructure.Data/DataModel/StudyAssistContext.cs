using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    public class StudyAssistContext : DbContext
    {
        #region Constructors

        public StudyAssistContext()
        {
            //Database.EnsureCreated();
        }

        #endregion Constructors

        #region Properties

        internal DbSet<Category> Categories  { get; set; }

        #endregion Properties

        #region MethodOverrides

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //var configBuilder = new ConfigurationBuilder();

            //var v = Directory.GetCurrentDirectory();

            //configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            //configBuilder.AddJsonFile("appsettings.json");
            //var config = configBuilder.Build();
            //string connString = config.GetConnectionString("Def");
            string connString = "Server=tcp:tokeyserver.database.windows.net,1433;Initial Catalog=StudyAssistDb;Persist Security Info=False;User ID=fruitybrew;Password=k19061906!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            builder.UseSqlServer(connString);
        }

        #endregion MethodOverrides

    }
}
