using System;
using System.Collections.Generic;

namespace Najm.GraphQL.ApplicationCore.Entity;

public partial class IdVehicle
{
    public decimal VehicleId { get; set; }

    public decimal? PolicyId { get; set; }

    public string? SubPolicyNumber { get; set; }

    public string? DefiendBy { get; set; }

    public string? PlatNumber { get; set; }

    public string? FirstPlatLetter { get; set; }

    public string? SecondPlateLetter { get; set; }

    public string? ThirdPlateLetter { get; set; }

    public string? PlateType { get; set; }

    public string? CustomId { get; set; }

    public decimal? SeqNumber { get; set; }

    public string? CoverageType { get; set; }

    public DateOnly? IssueHijriDate { get; set; }

    public DateOnly? EffectiveHijriDate { get; set; }

    public DateOnly? ExpireHijriDate { get; set; }

    public DateOnly? IssueGreDate { get; set; }

    public DateOnly? EffectiveGreDate { get; set; }

    public DateOnly? ExpireGreDate { get; set; }

    public string? Category { get; set; }

    public string? Manufacture { get; set; }

    public string? ManufacturingYear { get; set; }

    public string? Model { get; set; }

    public string? Color { get; set; }

    public string? ChassisNumber { get; set; }

    public decimal? CustomerId { get; set; }

    public DateTime? SysDate { get; set; }

    public decimal? ElmId { get; set; }

    public int? FirstNumber { get; set; }

    public int? SecondNumber { get; set; }

    public int? ThirdNumber { get; set; }

    public string? XmlIssueHijri { get; set; }

    public string? XmlEffectiveHijri { get; set; }

    public string? XmlExpiredHijri { get; set; }

    public string? Under21 { get; set; }

    public string? SysDateFormatted { get; set; }

    public DateTime? PolicyExpirieDate { get; set; }

    public decimal? SeatingCapacity { get; set; }

    public decimal? TrimId { get; set; }

    public DateTime? HijriIstmaraExpireDate { get; set; }

    public DateTime? GreIstmaraExpireDate { get; set; }

    public decimal? WeightOfVehicle { get; set; }

    public decimal? PersonalAccedentCoverage { get; set; }

    public string? GeographicCoverageGcc { get; set; }

    public string? NaturalDisasterCoverage { get; set; }

    public decimal? PremiumPolicyAmount { get; set; }

    public string? FinancialTranactionId { get; set; }

    public decimal? UserType { get; set; }

    public string? SalesUserId { get; set; }

    public long? CustomIdN { get; set; }

    public decimal? Saaed { get; set; }

    public decimal? RepairType { get; set; }

    public string? Iban { get; set; }

    public decimal? RestrictionOnUse { get; set; }

    public decimal? BaseRate { get; set; }

    public decimal? NcdEligibility { get; set; }

    public decimal? NoClaimDiscount { get; set; }

    public decimal? LoyalityDiscount { get; set; }

    public decimal? MarketValue { get; set; }

    public decimal? BankId { get; set; }

    public decimal? DeductibleAmount { get; set; }

    public int? NoOfAccidentCount { get; set; }

    public DateTime? IssueGreDateTime { get; set; }

    public DateTime? EffectiveGreDateTime { get; set; }

    public DateTime? ExpireGreDateTime { get; set; }
}

public class VehicleListDetail
{
    public string PlateNumber { get; set; }
    public string AppliedInNajm { get; set; }
    public string AppliedInElm { get; set; }
    public string ElmRejectionreason { get; set; }
    public string Trans_Type { get; set; }
    public string Sys_Date { get; set; }
    public string First_Plat_Letter { get; set; }
    public string Second_Plate_Letter { get; set; }
    public string Third_Plate_Letter { get; set; }
}

public class VehicleDetail
{
    public int Vehicle_ID { get; set; }
    public int Policy_ID { get; set; }
    public string Sub_Policy_Number { get; set; }
    public string Defiend_By { get; set; }
    public string Plat_Number { get; set; }
    public string First_Plate_Letter { get; set; }
    public string Second_Plate_Letter { get; set; }
    public string Third_Plate_Letter { get; set; }
    public string Custom_ID { get; set; }
    public long Seq_Number { get; set; }
    public string Coverage_Type { get; set; }
    public string Plate_Type { get; set; }
    public string Issue_Gre_Date { get; set; }
    public string Effective_Gre_Date { get; set; }
    public string Expire_Gre_Date { get; set; }
    public string Manufacture { get; set; }
    public string Manufacturing_Year { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string Chassis_Number { get; set; }
    public int? Customer_ID { get; set; }
    public string Personal_Accident_Coverage { get; set; }
    public string Geographic_Coverage_Gcc { get; set; }
    public List<VehicleListDetail> VehicleListDetail { get; set; }
}

public class VehicleInfo
{
    public List<object> Errors { get; set; }
    public bool IsValid { get; set; }
    public List<VehicleDetail> Value { get; set; }
}
