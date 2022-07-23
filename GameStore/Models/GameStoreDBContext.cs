using System;
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
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Osystem> Osystems { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAuthorization> UserAuthorizations { get; set; } = null!;
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

                entity.Property(e => e.CommentDate).HasColumnType("date");

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

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.GameCategoryId).HasColumnName("GameCategoryID");

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
                    .HasConstraintName("FK__Games__GameCateg__44FF419A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Games__UserID__45F365D3");

                entity.HasMany(d => d.Osystems)
                    .WithMany(p => p.Games)
                    .UsingEntity<Dictionary<string, object>>(
                        "GameOsystem",
                        l => l.HasOne<Osystem>().WithMany().HasForeignKey("OsystemId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GameOSyst__OSyst__5165187F"),
                        r => r.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GameOSyst__GameI__5070F446"),
                        j =>
                        {
                            j.HasKey("GameId", "OsystemId").HasName("PK__GameOSys__9AE3098CF9086BE7");

                            j.ToTable("GameOSystems");

                            j.IndexerProperty<int>("GameId").HasColumnName("GameID");

                            j.IndexerProperty<int>("OsystemId").HasColumnName("OSystemID");
                        });

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Games)
                    .UsingEntity<Dictionary<string, object>>(
                        "GameTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GameTags__TagID__4BAC3F29"),
                        r => r.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GameTags__GameID__4AB81AF0"),
                        j =>
                        {
                            j.HasKey("GameId", "TagId").HasName("PK__GameTags__FCEF5879A572749C");

                            j.ToTable("GameTags");

                            j.IndexerProperty<int>("GameId").HasColumnName("GameID");

                            j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                        });
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

                entity.HasMany(d => d.GamesNavigation)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserGame",
                        l => l.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserGames__GameI__59063A47"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserGames__UserI__5812160E"),
                        j =>
                        {
                            j.HasKey("UserId", "GameId").HasName("PK__UserGame__D52345D1A1079024");

                            j.ToTable("UserGames");

                            j.IndexerProperty<int>("UserId").HasColumnName("UserID");

                            j.IndexerProperty<int>("GameId").HasColumnName("GameID");
                        });

                entity.HasMany(d => d.UserId1s)
                    .WithMany(p => p.UserId2s)
                    .UsingEntity<Dictionary<string, object>>(
                        "FriendUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId1").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__3F466844"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId2").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__403A8C7D"),
                        j =>
                        {
                            j.HasKey("UserId1", "UserId2").HasName("PK__FriendUs__2847639511AF441C");

                            j.ToTable("FriendUsers");

                            j.IndexerProperty<int>("UserId1").HasColumnName("UserID1");

                            j.IndexerProperty<int>("UserId2").HasColumnName("UserID2");
                        });

                entity.HasMany(d => d.UserId2s)
                    .WithMany(p => p.UserId1s)
                    .UsingEntity<Dictionary<string, object>>(
                        "FriendUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId2").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__403A8C7D"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId1").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FriendUse__UserI__3F466844"),
                        j =>
                        {
                            j.HasKey("UserId1", "UserId2").HasName("PK__FriendUs__2847639511AF441C");

                            j.ToTable("FriendUsers");

                            j.IndexerProperty<int>("UserId1").HasColumnName("UserID1");

                            j.IndexerProperty<int>("UserId2").HasColumnName("UserID2");
                        });
            });

            modelBuilder.Entity<UserAuthorization>(entity =>
            {
                entity.ToTable("UserAuthorization");

                entity.Property(e => e.UserAuthorizationId).HasColumnName("UserAuthorizationID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAuthorizations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserAutho__UserI__3B75D760");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.UserAuthorizations)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserAutho__UserT__3C69FB99");
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
