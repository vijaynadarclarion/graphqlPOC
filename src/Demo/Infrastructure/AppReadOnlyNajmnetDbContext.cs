using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Demo.Entity;

public partial class AppReadOnlyNajmnetDbContext : DbContext
{
    public AppReadOnlyNajmnetDbContext()
    {
    }

    public AppReadOnlyNajmnetDbContext(DbContextOptions<AppReadOnlyNajmnetDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<IdPolicy> IdPolicies { get; set; }

    public virtual DbSet<IdVehicle> IdVehicles { get; set; }

   // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=NajmnetReplicaAppDbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId);

            entity.ToTable("ID_POLICIES");

            entity.HasIndex(e => new { e.EffectiveGreDate, e.ExpireGreDate }, "IX_ID_POLICIES_EFFECTIVE_GRE_DATE_EXPIRE_GRE_DATE").HasFillFactor(100);

            entity.HasIndex(e => e.MobileNumber, "IX_ID_POLICIES_MOBILE_NUMBER").HasFillFactor(80);

            entity.HasIndex(e => e.MobileNumber, "IX_ID_POLICIES_MOBILE_NUMBER_NN").HasFillFactor(80);

            entity.HasIndex(e => e.SponsorNumber, "IX_ID_POLICIES_SPONSOR_NUMBER_NN").HasFillFactor(80);

            entity.HasIndex(e => e.SponsorNumberN, "IX_ID_POLICIES_SPONSOR_NUMBER_N_NN").HasFillFactor(80);

            entity.HasIndex(e => new { e.SponsorNumberN, e.SysDate }, "IX_SPONSOR_NUMBER_N_SYS_DATE");

            entity.HasIndex(e => e.PolicyNumber, "ix_PolicyNumber_Sub").HasFillFactor(80);

            entity.Property(e => e.PolicyId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("POLICY_ID");
            entity.Property(e => e.AddressIdentifier)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("ADDRESS_IDENTIFIER");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("CITY");
            entity.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");
            entity.Property(e => e.CustomerId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.DriverAgeLimit)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("DRIVER_AGE_LIMIT");
            entity.Property(e => e.EffectiveGreDate)
                .HasColumnType("datetime")
                .HasColumnName("EFFECTIVE_GRE_DATE");
            entity.Property(e => e.ElmId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("ELM_ID");
            entity.Property(e => e.ExpireGreDate)
                .HasColumnType("datetime")
                .HasColumnName("EXPIRE_GRE_DATE");
            entity.Property(e => e.HomeAdditionalNumber)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("HOME_ADDITIONAL_NUMBER");
            entity.Property(e => e.HomeBuildingNo)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("HOME_BUILDING_NO");
            entity.Property(e => e.HomeCity)
                .HasMaxLength(50)
                .HasColumnName("HOME_CITY");
            entity.Property(e => e.HomeDistrict)
                .HasMaxLength(100)
                .HasColumnName("HOME_DISTRICT");
            entity.Property(e => e.HomeStreet)
                .HasMaxLength(100)
                .HasColumnName("HOME_STREET");
            entity.Property(e => e.HomeUnitNo)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("HOME_UNIT_NO");
            entity.Property(e => e.HomeZipCode)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("HOME_ZIP_CODE");
            entity.Property(e => e.HomeZone)
                .HasMaxLength(100)
                .HasColumnName("HOME_ZONE");
            entity.Property(e => e.InsuranceInstallment)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("INSURANCE_INSTALLMENT");
            entity.Property(e => e.InsuredTypeId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("INSURED_TYPE_ID");
            entity.Property(e => e.IssueGreDate)
                .HasColumnType("datetime")
                .HasColumnName("ISSUE_GRE_DATE");
            entity.Property(e => e.MaxLiabilit)
                .HasMaxLength(50)
                .HasColumnName("MAX_LIABILIT");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(50)
                .HasColumnName("MOBILE_NUMBER");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
            entity.Property(e => e.NewOrRenew)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("NEW_OR_RENEW");
            entity.Property(e => e.OwnerDateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("OWNER_DATE_OF_BIRTH");
            entity.Property(e => e.OwnerNationality)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("OWNER_NATIONALITY");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.PoBox)
                .HasMaxLength(50)
                .HasColumnName("PO_BOX");
            entity.Property(e => e.PolicyIssueCity)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("POLICY_ISSUE_CITY");
            entity.Property(e => e.PolicyNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("POLICY_NUMBER");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(50)
                .HasColumnName("POSTAL_CODE");
            entity.Property(e => e.SalesUserId)
                .HasMaxLength(100)
                .HasColumnName("SALES_USER_ID");
            entity.Property(e => e.SponsorNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SPONSOR_NUMBER");
            entity.Property(e => e.SponsorNumberN).HasColumnName("SPONSOR_NUMBER_N");
            entity.Property(e => e.SysDate)
                .HasColumnType("datetime")
                .HasColumnName("SYS_DATE");
            entity.Property(e => e.Unn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UNN");
            entity.Property(e => e.UserType)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("USER_TYPE");
            entity.Property(e => e.VersionType)
                .HasMaxLength(100)
                .HasColumnName("VERSION_TYPE");
            entity.Property(e => e.WaselAdditionalNumber)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("WASEL_ADDITIONAL_NUMBER");
            entity.Property(e => e.WaselBuildingNo)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("WASEL_BUILDING_NO");
            entity.Property(e => e.WaselCity)
                .HasMaxLength(50)
                .HasColumnName("WASEL_CITY");
            entity.Property(e => e.WaselZipCode)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("WASEL_ZIP_CODE");
            entity.Property(e => e.WaselZone)
                .HasMaxLength(100)
                .HasColumnName("WASEL_ZONE");
        });

        modelBuilder.Entity<IdVehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId);

            entity.ToTable("ID_VEHICLES");

            entity.HasIndex(e => new { e.PolicyId, e.SubPolicyNumber, e.EffectiveGreDate, e.ExpireGreDate }, "ID_VEHICLES_PolicyID_SubPoNu_Eff_EXP_NN").HasFillFactor(80);

            entity.HasIndex(e => new { e.ExpireGreDate, e.ChassisNumber, e.CustomerId, e.EffectiveGreDate, e.PolicyId }, "IX_ID_VEHICLES_CHASSIS_NUMBER,CUSTOMER_ID").HasFillFactor(100);

            entity.HasIndex(e => e.CustomIdN, "IX_ID_VEHICLES_CUSTOM_ID_N").HasFillFactor(80);

            entity.HasIndex(e => e.CustomId, "IX_ID_VEHICLES_CUSTOM_ID_NN").HasFillFactor(80);

            entity.HasIndex(e => e.CustomIdN, "IX_ID_VEHICLES_CUSTOM_ID_N_NN").HasFillFactor(80);

            entity.HasIndex(e => new { e.EffectiveGreDate, e.ExpireGreDate }, "IX_ID_VEHICLES_EFF_GRE_DATE_EXP_GRE_DATE_NN").HasFillFactor(80);

            entity.HasIndex(e => new { e.ExpireGreDate, e.ChassisNumber, e.CustomerId, e.EffectiveGreDate, e.PolicyId }, "IX_ID_VEHICLES_EXPIRE_GRE_DATE_CHASSIS_NUMBER_CUSTOMER_ID");

            entity.HasIndex(e => new { e.PlatNumber, e.FirstPlatLetter, e.SecondPlateLetter, e.ThirdPlateLetter, e.CustomIdN, e.VehicleId, e.SeqNumber, e.ExpireGreDate }, "IX_ID_VEHICLES_MulCols").HasFillFactor(80);

            entity.HasIndex(e => new { e.PlatNumber, e.FirstPlatLetter, e.SecondPlateLetter, e.ThirdPlateLetter, e.SeqNumber, e.VehicleId, e.ExpireGreDate, e.CustomIdN }, "IX_ID_VEHICLES_MulCols2").HasFillFactor(80);

            entity.HasIndex(e => new { e.PlatNumber, e.FirstPlatLetter, e.SecondPlateLetter, e.ThirdPlateLetter, e.SeqNumber, e.VehicleId, e.ExpireGreDate, e.CustomerId, e.CustomIdN }, "IX_ID_VEHICLES_Mulcols1").HasFillFactor(80);

            entity.HasIndex(e => e.PolicyId, "IX_ID_VEHICLES_POLICY_ID_NN").HasFillFactor(80);

            entity.HasIndex(e => new { e.SeqNumber, e.CustomId, e.ExpireGreDate }, "IX_ID_VEHICLES_SEQ_CUST_EXPIRE_NN").HasFillFactor(80);

            entity.HasIndex(e => e.SeqNumber, "IX_ID_VEHICLES_SEQ_NUMBER_NN").HasFillFactor(80);

            entity.HasIndex(e => e.SysDate, "IX_ID_VEHICLES_SYS_DATE").HasFillFactor(80);

            entity.HasIndex(e => new { e.PlatNumber, e.FirstPlatLetter, e.SecondPlateLetter, e.ThirdPlateLetter }, "IX_Vehicle_Plate_No_ID_VEhicles_NN").HasFillFactor(80);

            entity.HasIndex(e => e.PolicyId, "ix_id_vehicles_vehicle_policy");

            entity.HasIndex(e => e.SeqNumber, "missing_index_19_18_ID_VEHICLES").HasFillFactor(80);

            entity.Property(e => e.VehicleId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("VEHICLE_ID");
            entity.Property(e => e.BankId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("BANK_ID");
            entity.Property(e => e.BaseRate)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("BASE_RATE");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.ChassisNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CHASSIS_NUMBER");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COLOR");
            entity.Property(e => e.CoverageType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COVERAGE_TYPE");
            entity.Property(e => e.CustomId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUSTOM_ID");
            entity.Property(e => e.CustomIdN).HasColumnName("CUSTOM_ID_N");
            entity.Property(e => e.CustomerId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.DeductibleAmount)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("DEDUCTIBLE_AMOUNT");
            entity.Property(e => e.DefiendBy)
                .HasMaxLength(50)
                .HasColumnName("DEFIEND_BY");
            entity.Property(e => e.EffectiveGreDate).HasColumnName("EFFECTIVE_GRE_DATE");
            entity.Property(e => e.EffectiveGreDateTime)
                .HasColumnType("datetime")
                .HasColumnName("EFFECTIVE_GRE_DATE_TIME");
            entity.Property(e => e.EffectiveHijriDate).HasColumnName("EFFECTIVE_HIJRI_DATE");
            entity.Property(e => e.ElmId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("ELM_ID");
            entity.Property(e => e.ExpireGreDate).HasColumnName("EXPIRE_GRE_DATE");
            entity.Property(e => e.ExpireGreDateTime)
                .HasColumnType("datetime")
                .HasColumnName("EXPIRE_GRE_DATE_TIME");
            entity.Property(e => e.ExpireHijriDate).HasColumnName("EXPIRE_HIJRI_DATE");
            entity.Property(e => e.FinancialTranactionId)
                .HasMaxLength(50)
                .HasColumnName("FINANCIAL_TRANACTION_ID");
            entity.Property(e => e.FirstPlatLetter)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("FIRST_PLAT_LETTER");
            entity.Property(e => e.GeographicCoverageGcc)
                .HasMaxLength(50)
                .HasColumnName("GEOGRAPHIC_COVERAGE_GCC");
            entity.Property(e => e.GreIstmaraExpireDate)
                .HasColumnType("datetime")
                .HasColumnName("GRE_ISTMARA_EXPIRE_DATE");
            entity.Property(e => e.HijriIstmaraExpireDate).HasColumnName("HIJRI_ISTMARA_EXPIRE_DATE");
            entity.Property(e => e.Iban)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IBAN");
            entity.Property(e => e.IssueGreDate).HasColumnName("ISSUE_GRE_DATE");
            entity.Property(e => e.IssueGreDateTime)
                .HasColumnType("datetime")
                .HasColumnName("ISSUE_GRE_DATE_TIME");
            entity.Property(e => e.IssueHijriDate).HasColumnName("ISSUE_HIJRI_DATE");
            entity.Property(e => e.LoyalityDiscount)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("LOYALITY_DISCOUNT");
            entity.Property(e => e.Manufacture)
                .HasMaxLength(50)
                .HasColumnName("MANUFACTURE");
            entity.Property(e => e.ManufacturingYear)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MANUFACTURING_YEAR");
            entity.Property(e => e.MarketValue)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("MARKET_VALUE");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MODEL");
            entity.Property(e => e.NaturalDisasterCoverage)
                .HasMaxLength(50)
                .HasColumnName("NATURAL_DISASTER_COVERAGE");
            entity.Property(e => e.NcdEligibility)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("NCD_ELIGIBILITY");
            entity.Property(e => e.NoClaimDiscount)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("NO_CLAIM_DISCOUNT");
            entity.Property(e => e.NoOfAccidentCount).HasDefaultValue(0);
            entity.Property(e => e.PersonalAccedentCoverage)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("PERSONAL_ACCEDENT_COVERAGE");
            entity.Property(e => e.PlatNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLAT_NUMBER");
            entity.Property(e => e.PlateType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLATE_TYPE");
            entity.Property(e => e.PolicyExpirieDate)
                .HasColumnType("datetime")
                .HasColumnName("POLICY_EXPIRIE_DATE");
            entity.Property(e => e.PolicyId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("POLICY_ID");
            entity.Property(e => e.PremiumPolicyAmount)
                .HasColumnType("numeric(38, 2)")
                .HasColumnName("PREMIUM_POLICY_AMOUNT");
            entity.Property(e => e.RepairType)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("REPAIR_TYPE");
            entity.Property(e => e.RestrictionOnUse)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("RESTRICTION_ON_USE");
            entity.Property(e => e.Saaed)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("SAAED");
            entity.Property(e => e.SalesUserId)
                .HasMaxLength(100)
                .HasColumnName("SALES_USER_ID");
            entity.Property(e => e.SeatingCapacity)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("SEATING_CAPACITY");
            entity.Property(e => e.SecondPlateLetter)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("SECOND_PLATE_LETTER");
            entity.Property(e => e.SeqNumber)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("SEQ_NUMBER");
            entity.Property(e => e.SubPolicyNumber)
                .HasMaxLength(50)
                .HasColumnName("SUB_POLICY_NUMBER");
            entity.Property(e => e.SysDate)
                .HasColumnType("datetime")
                .HasColumnName("SYS_DATE");
            entity.Property(e => e.SysDateFormatted)
                .HasMaxLength(16)
                .HasColumnName("SYS_DATE_FORMATTED");
            entity.Property(e => e.ThirdPlateLetter)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("THIRD_PLATE_LETTER");
            entity.Property(e => e.TrimId)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("TRIM_ID");
            entity.Property(e => e.Under21)
                .HasMaxLength(5)
                .HasColumnName("UNDER_21");
            entity.Property(e => e.UserType)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("USER_TYPE");
            entity.Property(e => e.WeightOfVehicle)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("WEIGHT_OF_VEHICLE");
            entity.Property(e => e.XmlEffectiveHijri)
                .HasMaxLength(75)
                .HasColumnName("XML_EFFECTIVE_HIJRI");
            entity.Property(e => e.XmlExpiredHijri)
                .HasMaxLength(75)
                .HasColumnName("XML_EXPIRED_HIJRI");
            entity.Property(e => e.XmlIssueHijri)
                .HasMaxLength(75)
                .HasColumnName("XML_ISSUE_HIJRI");
        });
        modelBuilder.HasSequence("PolicyIDSeq").StartsAt(48776820L);
        modelBuilder.HasSequence("PolicyIDSeq2").StartsAt(48776820L);
        modelBuilder.HasSequence("VehicleIDSeq").StartsAt(79886610L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
