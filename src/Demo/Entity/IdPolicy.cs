using System;
using System.Collections.Generic;

namespace Demo.Entity;

public partial class IdPolicy
{
    public decimal PolicyId { get; set; }

    public string? Name { get; set; }

    public decimal? CustomerId { get; set; }

    public string? PolicyNumber { get; set; }

    public decimal? InsuredTypeId { get; set; }

    public string? SponsorNumber { get; set; }

    public string? VersionType { get; set; }

    public string? PhoneNumber { get; set; }

    public string? MobileNumber { get; set; }

    public string? PostalCode { get; set; }

    public string? PoBox { get; set; }

    public string? City { get; set; }

    public string? MaxLiabilit { get; set; }

    public decimal? DriverAgeLimit { get; set; }

    public DateOnly? CreationDate { get; set; }

    public DateTime? SysDate { get; set; }

    public decimal? ElmId { get; set; }

    public decimal? HomeBuildingNo { get; set; }

    public string? HomeCity { get; set; }

    public decimal? HomeZipCode { get; set; }

    public decimal? HomeAdditionalNumber { get; set; }

    public string? HomeZone { get; set; }

    public string? HomeDistrict { get; set; }

    public decimal? HomeUnitNo { get; set; }

    public string? HomeStreet { get; set; }

    public decimal? WaselBuildingNo { get; set; }

    public decimal? WaselAdditionalNumber { get; set; }

    public string? WaselCity { get; set; }

    public string? WaselZone { get; set; }

    public decimal? WaselZipCode { get; set; }

    public decimal? AddressIdentifier { get; set; }

    public DateTime? IssueGreDate { get; set; }

    public DateTime? EffectiveGreDate { get; set; }

    public DateTime? ExpireGreDate { get; set; }

    public DateTime? OwnerDateOfBirth { get; set; }

    public decimal? PolicyIssueCity { get; set; }

    public decimal? NewOrRenew { get; set; }

    public decimal? OwnerNationality { get; set; }

    public decimal? UserType { get; set; }

    public string? SalesUserId { get; set; }

    public long? SponsorNumberN { get; set; }

    public decimal? InsuranceInstallment { get; set; }

    public string? Unn { get; set; }
}
