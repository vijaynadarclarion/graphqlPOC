using System;
using System.Collections.Generic;
using Demo.Entity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data;

public partial class AppReadOnlyDbContext : DbContext
{
    public AppReadOnlyDbContext()
    {
    }

    public AppReadOnlyDbContext(DbContextOptions<AppReadOnlyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccidentInfo> CaseInfos { get; set; }

    public virtual DbSet<AccidentParty> CaseParties { get; set; }

   // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      //  => optionsBuilder.UseSqlServer("Name=AppDbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccidentInfo>(entity =>
        {
            entity.ToTable("CaseInfo", "dbo");           

            entity.Property(e => e.CaseInfoId).HasColumnName("CaseInfoID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CallCenterAgentId).HasColumnName("CallCenterAgentID");
            entity.Property(e => e.CaseActivationJobHandsetTime).HasColumnType("datetime");
            entity.Property(e => e.CaseAssignedToDataEntryTime).HasColumnType("datetime");
            entity.Property(e => e.CaseAttendedByCommandAgentTime).HasColumnType("datetime");
            entity.Property(e => e.CaseClosingTime).HasColumnType("datetime");
            entity.Property(e => e.CaseMoveToCommandQueuTime).HasColumnType("datetime");
            entity.Property(e => e.CaseNumber).HasMaxLength(15);
            entity.Property(e => e.CaseRegisterationTime).HasColumnType("datetime");
            entity.Property(e => e.CaseStageId).HasColumnName("CaseStageID");
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.CaseTypeId).HasColumnName("CaseTypeID");
            entity.Property(e => e.CaseTypeReasonId).HasColumnName("CaseTypeReasonID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CommandCenterAgentId).HasColumnName("CommandCenterAgentID");
            entity.Property(e => e.CrossStreetId).HasColumnName("CrossStreetID");
            entity.Property(e => e.DataEntryCompletedTime).HasColumnType("datetime");
            entity.Property(e => e.DataEntryId).HasColumnName("DataEntryID");
            entity.Property(e => e.DisabilityTypeIds).HasMaxLength(50);
            entity.Property(e => e.DrawDamageSceneUserId).HasColumnName("DrawDamageSceneUserID");
            entity.Property(e => e.EventTypeId).HasDefaultValue(0);
            entity.Property(e => e.IsFemaleInvolved).HasDefaultValue((byte)1);
            entity.Property(e => e.IsRemoteSurveying).HasDefaultValue(false);
            entity.Property(e => e.IsRemoteSurveyingPhotoUpload).HasDefaultValue(false);
            entity.Property(e => e.LandmarkId).HasColumnName("LandmarkID");
            entity.Property(e => e.LatId)
                .HasMaxLength(50)
                .HasColumnName("LatID");
            entity.Property(e => e.LongId)
                .HasMaxLength(50)
                .HasColumnName("LongID");
            entity.Property(e => e.MainCaseInfoId).HasColumnName("MainCaseInfoID");
            entity.Property(e => e.NoOfPartiesInvolved).HasDefaultValue((byte)2);
            entity.Property(e => e.OutofCoverageId).HasColumnName("OutofCoverageID");
            entity.Property(e => e.PhotoUploadTime).HasColumnType("datetime");
            entity.Property(e => e.PoliceName).HasMaxLength(100);
            entity.Property(e => e.SplitRegistrationDateTime).HasColumnType("datetime");
            entity.Property(e => e.StreetId).HasColumnName("StreetID");
            entity.Property(e => e.SubZoneId).HasColumnName("SubZoneID");
            entity.Property(e => e.SurveyorAcceptJobHandsetTime).HasColumnType("datetime");
            entity.Property(e => e.SurveyorAcceptJobLat).HasMaxLength(100);
            entity.Property(e => e.SurveyorAcceptJobLong).HasMaxLength(100);
            entity.Property(e => e.SurveyorArrivalHandsetTime).HasColumnType("datetime");
            entity.Property(e => e.SurveyorArrivalLat).HasMaxLength(100);
            entity.Property(e => e.SurveyorArrivalLocAccuracy).HasMaxLength(35);
            entity.Property(e => e.SurveyorArrivalLong).HasMaxLength(100);
            entity.Property(e => e.SurveyorArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.SurveyorAssignedTime).HasColumnType("datetime");
            entity.Property(e => e.SurveyorClosingHandsetTime).HasColumnType("datetime");
            entity.Property(e => e.SurveyorClosingLat).HasMaxLength(100);
            entity.Property(e => e.SurveyorClosingLocAccuracy).HasMaxLength(35);
            entity.Property(e => e.SurveyorClosingLong).HasMaxLength(100);
            entity.Property(e => e.SurveyorClosingTime).HasColumnType("datetime");
            entity.Property(e => e.TypeOfAccidentLocationId).HasColumnName("TypeOfAccidentLocationID");
            entity.Property(e => e.TypeOfAccidentWayId).HasColumnName("TypeOfAccidentWayID");
            entity.Property(e => e.TypeOfTrafficSideId).HasColumnName("TypeOfTrafficSideID");
            entity.Property(e => e.TypePriorityId).HasColumnName("TypePriorityID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");
           
            entity.HasKey(e => e.CaseInfoId);
        });

        modelBuilder.Entity<AccidentParty>(entity =>
        {
            entity.ToTable("CaseParties", "dbo");

            entity.Property(e => e.CasepartyId).HasColumnName("CasepartyID");
            entity.Property(e => e.CaseInfoId).HasColumnName("CaseInfoID");
            entity.Property(e => e.CreatedBy).HasDefaultValue(88888);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CustomID");
            entity.Property(e => e.DisabilityTypeIds).HasMaxLength(50);
            entity.Property(e => e.DriverBirthDateGeregorin).HasColumnType("datetime");
            entity.Property(e => e.DriverBirthDateHijra).HasMaxLength(10);
            entity.Property(e => e.DriverGenderId)
                .HasDefaultValue((byte)1)
                .HasColumnName("DriverGenderID");
            entity.Property(e => e.DriverId)
                .HasMaxLength(20)
                .HasColumnName("DriverID");
            entity.Property(e => e.DriverLicenseExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.DriverLicenseExpiryHijraDate).HasMaxLength(10);
            entity.Property(e => e.DriverLicenseTypeId).HasColumnName("DriverLicenseTypeID");
            entity.Property(e => e.DriverName).HasMaxLength(50);
            entity.Property(e => e.DriverNationalityId).HasColumnName("DriverNationalityID");
            entity.Property(e => e.DriverPhone).HasMaxLength(20);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FixedItemTypeId).HasColumnName("FixedItemTypeID");
            entity.Property(e => e.IdentityTypeId).HasColumnName("IdentityTypeID");
            entity.Property(e => e.InsCoverageTypeId).HasColumnName("InsCoverageTypeID");
            entity.Property(e => e.InsuranceCompanyId).HasColumnName("InsuranceCompanyID");
            entity.Property(e => e.InsuranceEndDateHijra).HasMaxLength(10);
            entity.Property(e => e.InsuranceFirstDateHijra).HasMaxLength(10);
            entity.Property(e => e.InsurancePolicyNo).HasMaxLength(50);
            entity.Property(e => e.InsuranceTypeId).HasColumnName("InsuranceTypeID");
            entity.Property(e => e.InsurnaceCustomNo).HasMaxLength(20);
            entity.Property(e => e.InsurnaceEndDate).HasColumnType("datetime");
            entity.Property(e => e.InsurnaceFirstDate).HasColumnType("datetime");
            entity.Property(e => e.InsurnacePlateNo).HasMaxLength(20);
            entity.Property(e => e.IsEda)
                .HasDefaultValue(false)
                .HasColumnName("IsEDA");
            entity.Property(e => e.IsEdaeligible)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("IsEDAEligible");
            entity.Property(e => e.IsMatchingTaqdeerStandard).HasDefaultValue(false);
            entity.Property(e => e.IsRecovery).HasDefaultValue(false);
            entity.Property(e => e.LicenseNumber).HasMaxLength(30);
            entity.Property(e => e.OwnerId)
                .HasMaxLength(20)
                .HasColumnName("OwnerID");
            entity.Property(e => e.PartyFiscode)
                .HasMaxLength(50)
                .HasColumnName("PartyFISCode");
            entity.Property(e => e.PartyTypeId).HasColumnName("PartyTypeID");
            entity.Property(e => e.PlateNo).HasMaxLength(50);
            entity.Property(e => e.PlateTypeId).HasColumnName("PlateTypeID");
            entity.Property(e => e.PolicyIssueTime)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ResidentStatusId).HasColumnName("ResidentStatusID");
            entity.Property(e => e.SponsorNumber).HasMaxLength(100);
            entity.Property(e => e.VehicleChassisNo).HasMaxLength(50);
            entity.Property(e => e.VehicleColorId).HasColumnName("VehicleColorID");
            entity.Property(e => e.VehicleId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.VehicleMakeId).HasColumnName("VehicleMakeID");
            entity.Property(e => e.VehicleManufactureYearId).HasColumnName("VehicleManufactureYearID");
            entity.Property(e => e.VehicleModelId).HasColumnName("VehicleModelID");
            entity.Property(e => e.VehicleOwnerName).HasMaxLength(50);
            entity.Property(e => e.VehiclePlateCountryId).HasColumnName("VehiclePlateCountryID");
            entity.Property(e => e.VehiclePlateNo).HasMaxLength(20);
            entity.Property(e => e.VehicleRegistrationTypeId).HasColumnName("VehicleRegistrationTypeID");
            entity.Property(e => e.VehicleSequenceNumber).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.VisaNumber).HasMaxLength(50);

            modelBuilder.Entity<AccidentParty>()
                .HasKey(ap => new { ap.CaseInfoId, ap.CasepartyId });

        });
       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
