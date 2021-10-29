using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLHS.Models
{
    public class DatabaseContext : DbContext
    { 
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<NewFeed> NewFeeds { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => new { c.PostId, c.StudentId, c.CommentContent })
                                   .HasName("pk_comment");
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasOne(e => e.StudentNavigation)
                   .WithMany(e => e.Comments)
                   .HasForeignKey(e => e.StudentId) 
                   .HasConstraintName("fk_comment_student");
                entity.HasOne(e => e.PostNavigation)
                  .WithMany(e => e.Comments)
                  .HasForeignKey(e => e.PostId) 
                  .HasConstraintName("fk_comment_post");
            }
           );

            modelBuilder.Entity<NewFeed>(entity =>
            {
                entity.HasKey(c => c.Id)
                                  .HasName("pk_newfeed");
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(c => new { c.StudentId, c.SubjectId})
                                  .HasName("pk_score");
                entity.HasOne(e => e.StudentNavigation)
                  .WithMany(e => e.Scores)
                  .HasForeignKey(e => e.StudentId)
                  .HasConstraintName("fk_score_student");
                entity.HasOne(e => e.SubjectNavigation)
                  .WithMany(e => e.Scores)
                  .HasForeignKey(e => e.SubjectId)
                  .HasConstraintName("fk_score_post");
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(c => c.Id)
                                  .HasName("pk_student"); 
            });
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(c => c.Id)
                                  .HasName("pk_subject"); 
            });
        }
    }
}
