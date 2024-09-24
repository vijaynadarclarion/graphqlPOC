namespace Demo.Query;

public class CompQuotesRequest
{
    public string RequestReferenceNo { get; set; }
    public string? InsuranceTypeID { get; set; }
    public int? PolicyholderIdentityTypeCode { get; set; }
    public string? PolicyHolderID { get; set; }

}

public class SelectQutationRequest
{
    public int PolicyRequestReferenceNo { get; set; }
    public int? InsuranceCompanyCode { get; set; }
    public int? RequestReferenceNo { get; set; }
    public string? PolicyHolderID { get; set; }

}

public class SelectQutationResponse
{
    public string InsuranceCompanyCode { get; set; }
    public string QuoteReferenceNo { get; set; }
    public int Status { get; set; }

}

public class CompQuotesResponse 
{
    public string InsuranceCompanyCode { get; set; }
    public string QuoteReferenceNo { get; set; }
    public int Status { get; set; }

}

public class QueryType : ObjectType<Query>
{
    public QueryType()
    {
       
    }

    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
                   .Field("compQuotes")
                   .Argument("input", a => a.Type<NonNullType<InputObjectType<CompQuotesRequest>>>()) // Register the input type here
                   
                   .Resolver(context => GetCompQuotes(default!))
                   .Description("Represents the accident information.");

        descriptor
                 .Field("selectQutation")
                 .Argument("input", a => a.Type<NonNullType<InputObjectType<SelectQutationRequest>>>()) // Register the input type here                
                 .Resolver(context => GetCompQuotes(default!))
                 .Description("Represents the accident information.");

    }

    private SelectQutationResponse SelectQutation(SelectQutationRequest input)
    {
        // Your logic to get the comp quotes data based on the input
        // Return the appropriate CompQuotesResponse object

        // Example:
        return new SelectQutationResponse
        {
            InsuranceCompanyCode = "123",
            Status = 2,
            QuoteReferenceNo = "123"
            // Populate the response object with data
        };
    }

    private CompQuotesResponse GetCompQuotes(CompQuotesRequest input)
    {
        // Your logic to get the comp quotes data based on the input
        // Return the appropriate CompQuotesResponse object

        // Example:
        return new CompQuotesResponse
        {
            InsuranceCompanyCode = "123",
            Status = 2,
            QuoteReferenceNo ="123"
            // Populate the response object with data
        };
    }
}
