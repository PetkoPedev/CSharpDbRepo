using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> Prescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                //PrimaryKey
                entity.HasKey(p => p.PatientId);

                entity
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity
                .Property(p => p.Address)
                .HasMaxLength(250)
                .IsRequired(true)
                .IsUnicode(true);

                entity
                .Property(p => p.Email)
                .HasMaxLength(80)
                .IsRequired(true)
                .IsUnicode(false);

                entity
                .Property(p => p.HasInsurance)
                .HasDefaultValue(true);
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                //PrimaryKey
                entity.HasKey(v => v.VisitationId);

                entity
                .Property(v => v.Date)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired(true);

                entity
                .Property(v => v.Comments)
                .HasMaxLength(250)
                .IsRequired(false)
                .IsUnicode(true);

                entity.HasOne(v => v.Patient)
                .WithMany(p => p.Visitations)
                .HasForeignKey(v => v.PatientId)
                .HasConstraintName("FK_Visitation_Patient");
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                //PrimaryKey
                entity.HasKey(d => d.DiagnoseId);

                entity
                .Property(d => d.Name)
                .HasMaxLength(50)
                .IsRequired(true);

                entity
                .Property(d => d.Comments)
                .HasMaxLength(250)
                .IsRequired(false)
                .IsUnicode(true);

                entity.HasOne(e => e.Patient)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(e => e.PatientId);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                //PrimaryKey
                entity.HasKey(m => m.MedicamentId);

                entity
                .Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.MedicamentId });

                entity.HasOne(e => e.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(e => e.MedicamentId);

                entity.HasOne(e => e.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(e => e.PatientId);
            });
        }
    }
}
