using System;
using Microsoft.EntityFrameworkCore;
using StudyAssist.Core.Interfaces;

namespace StudyAssist.Infrastructure.Data
{
    public class StudyAssistContext : DbContext
    {
        public DbSet<IProblem> Problems { get; set; }
    }
}
