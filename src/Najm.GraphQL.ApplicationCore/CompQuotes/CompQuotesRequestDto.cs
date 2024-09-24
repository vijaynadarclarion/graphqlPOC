using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Najm.GraphQL.ApplicationCore.CompQuotes;
public class CompQuotesRequestDto
{
    public string RequestReferenceNo { get; set; }
    public int? QuoteRequestSourceID { get; set; }
    public int? GroupID { get; set; }
    public int? ServiceID { get; set; }
    public int? CoverageTypeID { get; set; }
    public int? PolicyholderIdentityTypeCode { get; set; }
    public string PolicyHolderID { get; set; } // Updated - Yash
    public DateTime? PolicyholderIDExpiry { get; set; }
    public int? PolicyHolderNCDEligibility { get; set; }
    public int? PurposeofVehicleUseID { get; set; }
    public string FullName { get; set; }
    public string ArabicFirstName { get; set; }
    public string ArabicMiddleName { get; set; }
    public string ArabicLastName { get; set; }
    public string EnglishFirstName { get; set; }
    public string EnglishMiddleName { get; set; }
    public string EnglishLastName { get; set; }
    public DateTime? DateOfBirthG { get; set; }
    public DateTime? DateOfBirthH { get; set; }
    public string Occupation { get; set; }
    public string Education { get; set; }
    public int? MaritalStatus { get; set; }
    public int? ChildrenBelow16 { get; set; }
    public string WorkCompanyName { get; set; }
    public int? WorkCityID { get; set; }
    public int? Cylinders { get; set; }
    public int? VehicleCapacity { get; set; }
    public int? PolicyholderNationalityID { get; set; }
    public int? VehicleUniqueTypeID { get; set; }
    public long? VehicleSequenceNumber { get; set; }
    public long? VehicleCustomID { get; set; }
    public int? PolicyholderGender { get; set; }
    public int? VehicleDriveRegionID { get; set; }
    public bool? HasTrailer { get; set; }
    public int? TrailerSumInsured { get; set; }
    public int? VehicleDriveCityID { get; set; }
    public int? VehiclePlateTypeID { get; set; }
    public int? VehiclePlateNumber { get; set; }
    public int? FirstPlateLetterID { get; set; }
    public int? SecondPlateLetterID { get; set; }
    public int? ThirdPlateLetterID { get; set; }
    public int? MakeID { get; set; }
    public string MakeText { get; set; }
    public int? ModelID { get; set; }
    public string ModelText { get; set; }
    public int? ManufactureYear { get; set; }
    public int? VehicleColorCode { get; set; }
    public int? VehicleRegistrationCityCode { get; set; }
    public string VehicleVIN { get; set; }
    public string VehicleRegistrationExpiryDate { get; set; }
    public int? VehicleMileage { get; set; }
    public int? VehicleExpectedMileageYear { get; set; }
    public float? VehicleEngineSizeCC { get; set; }
    public int? VehicleTransmission { get; set; }
    public int? VehicleNightParking { get; set; }
    public int? VehicleAntitheftAlarm { get; set; }
    public int? VehicleABS { get; set; }
    public int? VehicleAutoBraking { get; set; }
    public int? VehicleCruiseControl { get; set; }
    public int? VehicleAdaptiveCruiseControl { get; set; }
    public int? VehicleRearSensors { get; set; }
    public int? VehicleFrontSensors { get; set; }
    public int? VehicleRearCamera { get; set; }
    public int? VehicleFrontCamera { get; set; }
    public int? Vehicle360Camera { get; set; }
    public int? VehicleFireExtinguisher { get; set; }
    public string VehicleModifications { get; set; }
    public int? VehicleAxleWeight { get; set; }
    public string PolicyEffectiveDate { get; set; }
    public int? PolicyTitleID { get; set; }
    public string MobileNo { get; set; }
    public int? BuildingNumber { get; set; }
    public string Street { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public int? ZipCode { get; set; }
    public int? AdditionalNumber { get; set; }
    public int? VehicleWeight { get; set; }
    public int? VehicleBodyCode { get; set; }
    public int? CoverAgeLimitID { get; set; }
    public List<DriverDetails> DriverDetails { get; set; } = default;
    public int? VehicleSumInsured { get; set; }
    public int? RepairMethod { get; set; }
    public bool? IsRenewal { get; set; }
    public string PolicyNumber { get; set; }
    public bool? IsScheme { get; set; }
    public List<SchemeDetails> SchemeDetails { get; set; }
    public int? NajmPolicyHolderNCDEligibility { get; set; }
    public string Field1 { get; set; }
    public string Field2 { get; set; }
    public string Field3 { get; set; }
    public string Field4 { get; set; }
    public string Field5 { get; set; }
    public string Field6 { get; set; }
    public string Field7 { get; set; }
    public string Field8 { get; set; }
    public string Field9 { get; set; }
    public string Field10 { get; set; }
    public string Field11 { get; set; }
    public string Field12 { get; set; }
    public string Field13 { get; set; }
    public string Field14 { get; set; }
    public string Field15 { get; set; }
}

public class DriverDetails
{
    public int? DriverID { get; set; }
    public string DriverName { get; set; }
    public bool? IsPolicyHolder { get; set; }
    public bool? IsUser { get; set; }
    public int? DriverRelation { get; set; }
    public int? VehicleUsagePercentage { get; set; }
    public string DriverDateOfBirthG { get; set; }
    public string DriverDateOfBirthH { get; set; }
    public int? DriverNationalityID { get; set; }
    public int? DriverGender { get; set; }
    public int? DriverNCDEligibility { get; set; }
    public string DriverOccupation { get; set; }
    public string DriverEducation { get; set; }
    public int? DriverMaritalStatus { get; set; }
    public int? DriverChildrenBelow16 { get; set; }
    public string DriverWorkCompanyName { get; set; }
    public int? DriverWorkCityID { get; set; }
    public string DriverHomeAddressCity { get; set; }
    public string DriverHomeAddress { get; set; }
    public int? DriverLicenseType { get; set; }
    public int? DriverLicenseOwnYears { get; set; }
    public List<CountriesValidDrivingLicense> CountriesValidDrivingLicense { get; set; }
    public string DriverTrafficViolationsCode { get; set; }
    public string DriverHealthConditionsCode { get; set; }
    //public NCDCalculations NCDCalculations { get; set; }


}

public class CountriesValidDrivingLicense
{
    public int? DrivingLicenseCountryID { get; set; }
    public int? DriverLicenseYears { get; set; }
}

public class SchemeDetails
{
    public string SchemeRef { get; set; }
    public string IcSchemeRef { get; set; }
    public int? SchemeTypeID { get; set; }
    public int? PositionNameCode { get; set; }
}
public class NCDCalculations
{
    //public int? NajmPolicyHolderNCDEligibility { get; set; }
    public int? NajmDriverNCDEligibility { get; set; }
}
