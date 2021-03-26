﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Model
{
    public class KnowledgeContext : DbContext
    {
        public DbSet<Catalog> Catalogs { get; set; }

        public DbSet<Theme> Themes { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public KnowledgeContext(DbContextOptions<KnowledgeContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        public KnowledgeContext()
        {
            Database.EnsureCreated();
        }
    }
}