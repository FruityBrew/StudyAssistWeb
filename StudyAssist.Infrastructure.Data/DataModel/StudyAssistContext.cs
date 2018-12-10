using System;
using Microsoft.EntityFrameworkCore;

namespace StudyAssist.Infrastructure.Data.DataModel
{
    public class StudyAssistContext : DbContext
    {
        #region Constructors

        public StudyAssistContext()
        {
            Database.EnsureCreated();
        }

        #endregion Constructors

        #region Properties

        private DbSet<Category> Categories  { get; set; }

        #endregion Properties

        #region MethodOverrides

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudyAssist_1;Trusted_Connection=True;");
        }

        #endregion MethodOverrides

    }
}
