﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DataServiceLib
{
    public class ImdbDatabase : DbContext
    {

         public static readonly ILoggerFactory MyLoggerFactory
           = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<Title_Basics> Title_basics { get; set; }
        public DbSet<TitleGenre> TitleGenre { get; set; }

        public DbSet<Users> Users { get; set; }
        public DbSet<Name_Basics> Names { get; set; }
        public DbSet<SearchTitleFunction> SearchTitle { get; set; }
        public DbSet<SearchActorFunction> SearchActor { get; set; }
        public DbSet<StructuredSearchFunction> StructuredSearch { get; set; }
        public DbSet<Users_SearchHistory> SearchHistory { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.UseNpgsql("host = rawdata.ruc.dk; db = raw4; uid = raw4; pwd =YGAvOMIt");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            TitlesModel(modelBuilder);
            UsersModel(modelBuilder);
            NamesModel(modelBuilder);
            SearchFunctionModel(modelBuilder);
            UsersSearchModel(modelBuilder);
        }

        private static void TitlesModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title_Basics>().ToTable("title_basics");
            modelBuilder.Entity<Title_Basics>().Property(x => x.Tconst).HasColumnName("tconst");
            modelBuilder.Entity<Title_Basics>().Property(x => x.TitleType).HasColumnName("titletype");
            modelBuilder.Entity<Title_Basics>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            modelBuilder.Entity<Title_Basics>().Property(x => x.OriginalTitle).HasColumnName("originaltitle");
            modelBuilder.Entity<Title_Basics>().Property(x => x.IsAdult).HasColumnName("isadult");
            modelBuilder.Entity<Title_Basics>().Property(x => x.StartYear).HasColumnName("startyear");
            modelBuilder.Entity<Title_Basics>().Property(x => x.EndYear).HasColumnName("endyear");
            modelBuilder.Entity<Title_Basics>().Property(x => x.RuntimeMinutes).HasColumnName("runtimeminutes");
            modelBuilder.Entity<Title_Basics>().Property(x => x.Poster).HasColumnName("poster");
            modelBuilder.Entity<Title_Basics>().Property(x => x.Awards).HasColumnName("awards");
            modelBuilder.Entity<Title_Basics>().Property(x => x.Plot).HasColumnName("plot");

            modelBuilder.Entity<TitleGenre>().HasKey(x => new { x.Tconst, x.Genre });
            modelBuilder.Entity<TitleGenre>().ToTable("title_basics_genres");
            modelBuilder.Entity<TitleGenre>().Property(x => x.Genre).HasColumnName("genres");
            modelBuilder.Entity<TitleGenre>().Property(x => x.Tconst).HasColumnName("tconst");
            modelBuilder.Entity<TitleGenre>().HasOne<Title_Basics>(x => x.TitleBasic).WithMany(y => y.Genres);
        }

        private static void UsersModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<Users>().Property(x => x.Userid).HasColumnName("userid");
            modelBuilder.Entity<Users>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<Users>().Property(x => x.Password).HasColumnName("password");
            modelBuilder.Entity<Users>().Property(x => x.Firstname).HasColumnName("firstname");
            modelBuilder.Entity<Users>().Property(x => x.Lastname).HasColumnName("lastname");
            modelBuilder.Entity<Users>().Property(x => x.Birthyear).HasColumnName("birthyear");
        }

        private static void NamesModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Name_Basics>().ToTable("name_basics");      
            modelBuilder.Entity<Name_Basics>().Property(x => x.Nconst).HasColumnName("nconst");
            modelBuilder.Entity<Name_Basics>().Property(x => x.PrimaryName).HasColumnName("primaryname");
            modelBuilder.Entity<Name_Basics>().Property(x => x.Birthyear).HasColumnName("birthyear");
            modelBuilder.Entity<Name_Basics>().Property(x => x.Deathyear).HasColumnName("deathyear");
        }

        private static void UsersSearchModel(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Users_SearchHistory>().HasKey(x => new { x.Userid, x.SearchDate });
            modelBuilder.Entity<Users_SearchHistory>().ToTable("users_searchhistory");
            modelBuilder.Entity<Users_SearchHistory>().Property(x => x.Userid).HasColumnName("userid");
            modelBuilder.Entity<Users_SearchHistory>().Property(x => x.SearchEntry).HasColumnName("searchentry");
            modelBuilder.Entity<Users_SearchHistory>().Property(x => x.SearchDate).HasColumnName("searchdate");
        }
        private static void SearchFunctionModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchTitleFunction>().HasNoKey();
            modelBuilder.Entity<SearchTitleFunction>().Property(x => x.Tconst).HasColumnName("tconst");
            modelBuilder.Entity<SearchTitleFunction>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");

            modelBuilder.Entity<SearchActorFunction>().HasNoKey();
            modelBuilder.Entity<SearchActorFunction>().Property(x => x.PrimaryName).HasColumnName("primaryname");
            modelBuilder.Entity<SearchActorFunction>().Property(x => x.PrimaryProfession).HasColumnName("primaryprofession");



            modelBuilder.Entity<StructuredSearchFunction>().HasNoKey();
            modelBuilder.Entity<StructuredSearchFunction>().Property(x => x.EntryTitle).HasColumnName("primarytitle");
            modelBuilder.Entity<StructuredSearchFunction>().Property(x => x.EntryPlot).HasColumnName("plot");
            modelBuilder.Entity<StructuredSearchFunction>().Property(x => x.EntryCharacters).HasColumnName("characters");
            modelBuilder.Entity<StructuredSearchFunction>().Property(x => x.EntryName).HasColumnName("primaryname");
        }
    }


}