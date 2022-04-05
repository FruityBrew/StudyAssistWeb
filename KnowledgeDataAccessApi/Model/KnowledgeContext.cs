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

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                //@"Server=KOVALEVAS\SQLEXPRESS;Database=KnowledgeDb;Trusted_Connection=true;"); 
            @"workstation id=knowledgeDb.mssql.somee.com;packet size=4096;user id=FruityBrew_SQLLogin_1;pwd=4f65smahle;data source=knowledgeDb.mssql.somee.com;persist security info=False;initial catalog=knowledgeDb");
        }
    }
}
