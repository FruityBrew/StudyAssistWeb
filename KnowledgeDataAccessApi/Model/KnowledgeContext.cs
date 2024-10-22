using Microsoft.EntityFrameworkCore;
using StudyAssist.Model;

namespace KnowledgeDataAccessApi.Model
{
	public class KnowledgeContext : DbContext
    {
        public DbSet<Catalog> Catalogs { get; set; }

        public DbSet<Theme> Themes { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<IssueUnderStudy> IssuesUnderStudy { get; set; }

        public KnowledgeContext(DbContextOptions<KnowledgeContext> options) 
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public KnowledgeContext()
        {
            //Database.EnsureCreated();
        }

    }
}
