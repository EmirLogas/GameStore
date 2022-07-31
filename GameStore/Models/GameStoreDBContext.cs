﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GameStore.Models
{
    public partial class GameStoreDBContext : DbContext
    {
        public GameStoreDBContext()
        {
        }

        public GameStoreDBContext(DbContextOptions<GameStoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<ContentImage> ContentImages { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameOsystem> GameOsystems { get; set; } = null!;
        public virtual DbSet<Osystem> Osystems { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGame> UserGames { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GameStoreDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CommentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CommentText)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__GameID__5535A963");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__UserID__5441852A");
            });

            modelBuilder.Entity<ContentImage>(entity =>
            {
                entity.Property(e => e.ContentImageId).HasColumnName("ContentImageID");

                entity.Property(e => e.ContentImagePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.ContentImages)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ContentIm__GameI__44FF419A");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.GameCategoryId).HasColumnName("GameCategoryID");

                entity.Property(e => e.GameCoverImagePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GameDescription)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.GameDeveloper)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GameName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GamePrice).HasColumnType("money");

                entity.Property(e => e.GamePublisher)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GameReleaseDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.GameCategory)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.GameCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Games__GameCateg__412EB0B6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Games__UserID__4222D4EF");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Games)
                    .UsingEntity<Dictionary<string, object>>(
                        "GameTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GameTags__TagID__4AB81AF0"),
                        r => r.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GameTags__GameID__49C3F6B7"),
                        j =>
                        {
                            j.HasKey("GameId", "TagId").HasName("PK__GameTags__FCEF58799DDCA0A9");

                            j.ToTable("GameTags");

                            j.IndexerProperty<int>("GameId").HasColumnName("GameID");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                        });
            });

            modelBuilder.Entity<GameOsystem>(entity =>
            {
                entity.HasKey(e => e.GameOsystemsId)
                    .HasName("PK__GameOSys__4BB8449BC408CB8A");

                entity.ToTable("GameOSystems");

                entity.HasIndex(e => new { e.GameId, e.OsystemId }, "UQ__GameOSys__9AE3098DD910C10E")
                    .IsUnique();

                entity.Property(e => e.GameOsystemsId).HasColumnName("GameOSystemsID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.OsystemId).HasColumnName("OSystemID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameOsystems)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameOSyst__GameI__70DDC3D8");

                entity.HasOne(d => d.Osystem)
                    .WithMany(p => p.GameOsystems)
                    .HasForeignKey(d => d.OsystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameOSyst__OSyst__71D1E811");
            });

            modelBuilder.Entity<Osystem>(entity =>
            {
                entity.ToTable("OSystems");

                entity.Property(e => e.OsystemId).HasColumnName("OSystemID");

                entity.Property(e => e.OsystemName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OSystemName");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserRegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasMany(d => d.UserId1s)
                    .WithMany(p => p.UserId2s)
                    .UsingEntity<Dictionary<string, object>>(
                        "FriendUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId1").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__3B75D760"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId2").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__3C69FB99"),
                        j =>
                        {
                            j.HasKey("UserId1", "UserId2").HasName("PK__FriendUs__28476395FA18C71D");

                            j.ToTable("FriendUsers");

                            j.IndexerProperty<int>("UserId1").HasColumnName("UserID1");

                            j.IndexerProperty<int>("UserId2").HasColumnName("UserID2");
                        });

                entity.HasMany(d => d.UserId2s)
                    .WithMany(p => p.UserId1s)
                    .UsingEntity<Dictionary<string, object>>(
                        "FriendUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId2").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__3C69FB99"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId1").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__3B75D760"),
                        j =>
                        {
                            j.HasKey("UserId1", "UserId2").HasName("PK__FriendUs__28476395FA18C71D");

                            j.ToTable("FriendUsers");

                            j.IndexerProperty<int>("UserId1").HasColumnName("UserID1");

                            j.IndexerProperty<int>("UserId2").HasColumnName("UserID2");
                        });
            });

            modelBuilder.Entity<UserGame>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.GameId }, "UQ__UserGame__D52345D0ACAE0A13")
                    .IsUnique();

                entity.Property(e => e.UserGameId).HasColumnName("UserGameID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.UserGames)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserGames__GameI__59FA5E80");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGames)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserGames__UserI__59063A47");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.UserTypeName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
