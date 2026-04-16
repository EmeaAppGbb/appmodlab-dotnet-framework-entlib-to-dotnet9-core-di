using Microsoft.EntityFrameworkCore;
using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Data
{
    public class HealthClaimsDbContext : DbContext
    {
        public HealthClaimsDbContext(DbContextOptions<HealthClaimsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimLineItem> ClaimLineItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId);
                entity.ToTable("Patients");
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.SSN).HasMaxLength(11).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(100).IsRequired();
                entity.Property(e => e.City).HasMaxLength(50).IsRequired();
                entity.Property(e => e.State).HasMaxLength(2).IsRequired();
                entity.Property(e => e.ZipCode).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.InsurancePolicyNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.InsuranceGroupNumber).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(e => e.ProviderId);
                entity.ToTable("Providers");
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.NPI).HasMaxLength(10).IsRequired();
                entity.Property(e => e.TaxId).HasMaxLength(11).IsRequired();
                entity.Property(e => e.Specialty).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(100).IsRequired();
                entity.Property(e => e.City).HasMaxLength(50).IsRequired();
                entity.Property(e => e.State).HasMaxLength(2).IsRequired();
                entity.Property(e => e.ZipCode).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Fax).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(e => e.ClaimId);
                entity.ToTable("Claims");
                entity.HasIndex(e => e.ClaimNumber).IsUnique();
                entity.Property(e => e.ClaimNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PlaceOfService).HasMaxLength(20).IsRequired();
                entity.Property(e => e.DiagnosisPointer).HasMaxLength(50);
                entity.Property(e => e.TotalChargeAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalApprovedAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPatientResponsibility).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PriorAuthorizationNumber).HasMaxLength(50);
                entity.Property(e => e.ReferringProviderNPI).HasMaxLength(10);
                entity.Property(e => e.ProcessedBy).HasMaxLength(100);
                entity.Property(e => e.AdjudicationNotes);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.ModifiedBy).HasMaxLength(100);
                entity.Property(e => e.AdmissionDate).HasMaxLength(50);
                entity.Property(e => e.DischargeDate).HasMaxLength(50);
                entity.Property(e => e.AccidentDate).HasMaxLength(50);

                entity.HasOne(e => e.Patient)
                    .WithMany()
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Provider)
                    .WithMany()
                    .HasForeignKey(e => e.ProviderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.LineItems)
                    .WithOne()
                    .HasForeignKey(e => e.ClaimId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClaimLineItem>(entity =>
            {
                entity.HasKey(e => e.LineItemId);
                entity.ToTable("ClaimLineItems");
                entity.Property(e => e.CPTCode).HasMaxLength(10).IsRequired();
                entity.Property(e => e.CPTDescription).HasMaxLength(200);
                entity.Property(e => e.ICD10Code).HasMaxLength(10).IsRequired();
                entity.Property(e => e.ICD10Description).HasMaxLength(200);
                entity.Property(e => e.UnitCharge).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalCharge).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ApprovedAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PatientResponsibility).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DenialReason).HasMaxLength(500);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.ToTable("Payments");
                entity.HasIndex(e => e.PaymentNumber).IsUnique();
                entity.Property(e => e.PaymentNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CheckNumber).HasMaxLength(50);
                entity.Property(e => e.EFTTraceNumber).HasMaxLength(50);
                entity.Property(e => e.PayeeName).HasMaxLength(100);
                entity.Property(e => e.PayeeAddress).HasMaxLength(200);
                entity.Property(e => e.PaymentStatus).HasMaxLength(20).IsRequired();
                entity.Property(e => e.RemittanceAdviceNumber).HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.HasOne(e => e.Claim)
                    .WithMany()
                    .HasForeignKey(e => e.ClaimId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
