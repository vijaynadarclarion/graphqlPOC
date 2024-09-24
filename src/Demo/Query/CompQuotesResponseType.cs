namespace Demo.Query;

public class CompQuotesResponseType : ObjectType<CompQuotesResponse>
{
    protected override void Configure(IObjectTypeDescriptor<CompQuotesResponse> descriptor)
    {
        descriptor
           .Field(p => p.QuoteReferenceNo)
           .Description("QuoteReferenceNo.");

        descriptor
            .Field(p => p.InsuranceCompanyCode)
            .Description("InsuranceCompanyCode.");

        descriptor
           .Field(p => p.Status)
           .Description("Status.")
           .Authorize();
    }
}
