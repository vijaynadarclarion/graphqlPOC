using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Najm.GraphQL.ApplicationCore.CompQuotes;

public class CompQuotesResList
{
    public string RequestReferenceNo { get; set; }
    public int? InsuranceCompanyCode { get; set; }
    public List<string> Errors { get; set; }
    public int? QuoteReferenceNo { get; set; }
    public int? PolicyHolderEligibility { get; set; }
    public int? PolicyTitleID { get; set; }
    public string MaxLiability { get; set; }
    public string PolicyEffectiveDate { get; set; }
    public string PolicyExpiryDate { get; set; }
    public int? VehicleSumInsured { get; set; }
    public bool? HasTrailer { get; set; }
    public double? TrailerSumInsure { get; set; }
    public double? TotalLossPercentage { get; set; }
    public Deductibles Deductibles { get; set; }
    public PolicyPremiumFeatures PolicyPremiumFeatures { get; set; }
    public DriverDetail DriverDetail { get; set; }
    public int InspectionTypeID { get; set; }
    public AdditionalCoverage AdditionalCoverage { get; set; }
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
public class Deductibles
{
    public int? DeductibleAmount { get; set; }
    public double? PolicyPremium { get; set; }
    public PremiumBreakdown PremiumBreakdown { get; set; }
    public double? TaxableAmount { get; set; }
    public Discounts Discounts { get; set; }
    public string DeductibleReferenceNo { get; set; }
    public DynamicPremiumFeatures DynamicPremiumFeatures { get; set; }
}

public class PolicyPremiumFeatures
{
    public int? FeatureID { get; set; }
    public int? FeatureTypeID { get; set; }
    public double? FeatureAmount { get; set; }
    public double? FeatureTaxableAmount { get; set; }
}

public class Discounts
{
    public int? DiscountTypeID { get; set; }
    public double? DiscountPercentage { get; set; }
    public double? DiscountAmount { get; set; }
}

public class DynamicPremiumFeatures
{
    public int? FeatureID { get; set; }
    public int? FeatureTypeID { get; set; }
    public double? FeatureAmount { get; set; }
    public double? FeatureTaxableAmount { get; set; }
}

public class PremiumBreakdown
{
    public int? BreakdownTypeID { get; set; }
    public double? BreakdownAmount { get; set; }
    public double? BreakdownPercentage { get; set; }
}

public class AdditionalCoverage
{
    public int? AddtionalCoverageID { get; set; }
    public int? AddtionalCoverageAmount { get; set; }
}


public class DriverDetail
{
    public int? DriverID { get; set; }
    public string DriverName { get; set; }
    public int? VehicleUsagePercentage { get; set; }
    public string DriverDateOfBirthG { get; set; }
    public string DriverDateOfBirthH { get; set; }
    public int? DriverGender { get; set; }
    public int? DriverNCDEligibility { get; set; }
}


public class CompQuotesSubResList
{
    public string RequestReferenceNo { get; set; }
    public int? InsuranceCompanyCode { get; set; }
    public CompQuotesSubEntity Entity { get; set; }
    public List<CompQuotesSubResError> Errors { get; set; }
}

public class CompQuotesSubResError
{
    public string Message { get; set; }
    public string Code { get; set; }
}

public class CompQuotesSubEntity
{
    public int? QuoteReferenceNo { get; set; }
    public int? PolicyHolderEligibility { get; set; }
    public int? PolicyTitleID { get; set; }
    public string MaxLiability { get; set; }
    public string PolicyEffectiveDate { get; set; }
    public string PolicyExpiryDate { get; set; }
    public int? VehicleSumInsured { get; set; }
    public bool? HasTrailer { get; set; }
    public double? TrailerSumInsure { get; set; }
    public double? TotalLossPercentage { get; set; }
    public Deductibles Deductibles { get; set; }
    public PolicyPremiumFeatures PolicyPremiumFeatures { get; set; }
    public DriverDetail DriverDetail { get; set; }
    public int InspectionTypeID { get; set; }
    public AdditionalCoverage AdditionalCoverage { get; set; }
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
