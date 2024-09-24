using System;
using System.Collections.Generic;
using HotChocolate.Authorization;

namespace Najm.GraphQL.ApplicationCore.Entity;

public partial class AccidentParty
{
    public int CasepartyId { get; set; }

    public int CaseInfoId { get; set; }

    public int PartyNo { get; set; }

    public string? DriverName { get; set; }

    public string? DriverId { get; set; }

    public int? PlateTypeId { get; set; }

    public string? DriverPhone { get; set; }

    public int? DriverAge { get; set; }

    public string? DriverBirthDateHijra { get; set; }

    public DateTime? DriverBirthDateGeregorin { get; set; }

    public int? DriverLicenseTypeId { get; set; }

    public int? DriverNationalityId { get; set; }

    public string? DriverFeedback { get; set; }

    public bool? IsInsured { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? VehicleOwnerName { get; set; }

    public int? VehicleMakeId { get; set; }

    public int? VehicleModelId { get; set; }

    public int? VehicleManufactureYearId { get; set; }

    public int? VehicleColorId { get; set; }

    public string? VehiclePlateNo { get; set; }

    public int? VehiclePlateCountryId { get; set; }

    public string? VehicleChassisNo { get; set; }

    public byte? IsVehicleMobile { get; set; }

    public int? InsCoverageTypeId { get; set; }

    public int? InsuranceTypeId { get; set; }

    public int? InsuranceCompanyId { get; set; }

    public string? InsurancePolicyNo { get; set; }

    public string? InsurnacePlateNo { get; set; }

    public string? InsurnaceCustomNo { get; set; }

    public DateTime? InsurnaceFirstDate { get; set; }

    public string? InsuranceFirstDateHijra { get; set; }

    public DateTime? InsurnaceEndDate { get; set; }

    public string? InsuranceEndDateHijra { get; set; }

    public int? LiabilityPercentage { get; set; }

    public int? PartyTypeId { get; set; }

    public int? SoudiPlateType { get; set; }

    public int? IdentityTypeId { get; set; }

    public int? FixedItemTypeId { get; set; }

    public DateTime? DriverLicenseExpiryDate { get; set; }

    public string? DriverLicenseExpiryHijraDate { get; set; }

    public string? VisaNumber { get; set; }

    public int? VehicleRegistrationTypeId { get; set; }

    public int? RepairFrom { get; set; }

    public string? PartyFiscode { get; set; }

    public decimal? VehicleSequenceNumber { get; set; }

    public decimal? CustomId { get; set; }

    public int? PlateChar1 { get; set; }

    public int? PlateChar2 { get; set; }

    public int? PlateChar3 { get; set; }

    public string? PlateNo { get; set; }

    public int? ResidentStatusId { get; set; }

    public byte? IsAgreeOnLiability { get; set; }

    public byte? InsuranceFillingSource { get; set; }

    public byte? DriverFillingSource { get; set; }

    public byte? VehicleFillingSource { get; set; }

    public byte? ReceiptPrintStatus { get; set; }

    public string? OwnerId { get; set; }

    public decimal? VehicleId { get; set; }

    public string? SponsorNumber { get; set; }

    public string? LicenseNumber { get; set; }

    public bool? IsRecovery { get; set; }

    public string? PolicyIssueTime { get; set; }

    public short? PlateDigit { get; set; }

    public byte? FirstPlateCharacterId { get; set; }

    public byte? SecondPlateCharacterId { get; set; }

    public byte? ThirdPlateCharacterId { get; set; }

    public double? OwnerFillingSource { get; set; }

    public byte DriverGenderId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public bool? IsMatchingTaqdeerStandard { get; set; }

    public bool? IsEda { get; set; }

    public bool? IsEdaeligible { get; set; }

    public int? CreatedBy { get; set; }

    public int? PartyCountryId { get; set; }

    public int? PlateCharactersTypeId { get; set; }

    public bool? IsSpecialNeeds { get; set; }

    public string? DisabilityTypeIds { get; set; }

    public int? NoStatementReasonId { get; set; }

   
    public byte? IsCashRepair { get; set; }


}
