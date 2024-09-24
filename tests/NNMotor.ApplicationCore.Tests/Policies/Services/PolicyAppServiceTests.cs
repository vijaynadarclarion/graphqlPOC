using System.Linq.Expressions;
using System.Text;
using Adf.Core;
using Ardalis.Specification;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NNMotor.ApplicationCore.Entities;
using NNMotor.ApplicationCore.Interfaces;
using NNMotor.ApplicationCore.Policies;
using NNMotor.ApplicationCore.Policies.Dtos;
using NNMotor.ApplicationCore.Policies.Interfaces;
using NNMotor.ApplicationCore.Policies.Services;
using NNMotor.ApplicationCore.Policies.Validators;
using NNMotor.ApplicationCore.Resources;
using NNMotor.Tests;
using Xunit;

namespace NNMotor.ApplicationCore.Tests.Policies.Services;

public class PolicyAppServiceTests
{
    private readonly IStringLocalizer<AppResources> _localizer;

    private VehicleInsurancePolicyDetailValidator _vehicleInsurancePolicyDetailValidator;
    private InsuranceCompanyListValidator _insuranceCompanyListValidator;
    private PolicyOrVehicleCountRequestValidator _policyOrVehicleCountRequestValidator;
    private Mock<IVehicleInsurancePolicyRepository> _vehicleInsurancePolicyRepository;
    private IPolicyAppService _policyAppService;


    public PolicyAppServiceTests()
    {
        var options = Options.Create(new LocalizationOptions());
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _localizer = new StringLocalizer<AppResources>(factory);

        SetUp();
    }

    #region Common

    private void SetUp()
    {
        Mock<IReadOnlyRepository<IdVehicle>> _idVehicleReadOnlyRepo = new Mock<IReadOnlyRepository<IdVehicle>>();
        Mock<IReadOnlyRepository<IdPolicy>> _idPolicyReadOnlyRepo = new Mock<IReadOnlyRepository<IdPolicy>>();
        Mock<IReadOnlyRepository<PlateCharacterMaster>> _plateCharacterMasterReadOnlyRepository = new Mock<IReadOnlyRepository<PlateCharacterMaster>>();
        Mock<IReadOnlyRepository<PlateTypeMaster>> _plateTypeMasterReadOnlyRepository = new Mock<IReadOnlyRepository<PlateTypeMaster>>();
        _vehicleInsurancePolicyRepository = new Mock<IVehicleInsurancePolicyRepository>();

        _idPolicyReadOnlyRepo.Setup(x => x.ListAsync(It.IsAny<ISpecification<IdPolicy>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
       .ReturnsAsync((ISpecification<IdPolicy> spec, string memberName, string sourceFilePath, int sourceLineNumber, CancellationToken cancellationToken) =>
       {
           return TestData.GetIdPolicies();
       });

        _idPolicyReadOnlyRepo.Setup(x => x.CountAsync(It.IsAny<ISpecification<IdPolicy>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync((ISpecification<IdPolicy> spec, string memberName, string sourceFilePath, int sourceLineNumber, CancellationToken cancellationToken) =>
           {
               return TestData.GetIdPolicies().Count;
           });

        _idVehicleReadOnlyRepo.Setup(x => x.ListAsync(It.IsAny<ISpecification<IdVehicle>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((ISpecification<IdVehicle> spec, string memberName, string sourceFilePath, int sourceLineNumber, CancellationToken cancellationToken) =>
             {
                 return TestData.GetIdVehicles().ToList();
             });

        _idVehicleReadOnlyRepo.Setup(x => x.ListPagedAsync(It.IsAny<ISpecification<IdVehicle, IdVehicle>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync((ISpecification<IdVehicle, IdVehicle> spec, int pageNumber, int pageSize, string memberName, string sourceFilePath, int sourceLineNumber, CancellationToken cancellationToken) =>
           {
               return new PagedList<IdVehicle>(TestData.GetIdVehicles(), pageNumber, pageSize, 10);
           });

        _idVehicleReadOnlyRepo.Setup(x => x.CountAsync(It.IsAny<ISpecification<IdVehicle>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((ISpecification<IdVehicle> spec, string memberName, string sourceFilePath, int sourceLineNumber, CancellationToken cancellationToken) =>
             {
                 return TestData.GetIdVehicles().Count;
             });

        _plateCharacterMasterReadOnlyRepository.Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<PlateCharacterMaster, bool>>>()))
             .ReturnsAsync((Expression<Func<PlateCharacterMaster, bool>> expression) =>
             {
                 var data = TestData.GetPlateCharacters().FirstOrDefault(expression.Compile());
                 return data;
             });

        _plateTypeMasterReadOnlyRepository.Setup(x => x.GetFirstOrDefault(It.IsAny<Expression<Func<PlateTypeMaster, bool>>>()))
            .ReturnsAsync((Expression<Func<PlateTypeMaster, bool>> expression) =>
            {
                var data = TestData.GetPlateTypes().FirstOrDefault(expression.Compile());
                return data;
            });

        _vehicleInsurancePolicyRepository.Setup(x => x.GetVehicleInsurancePolicyDetailAsync(It.IsAny<VehicleInsurancePolicyRequestDto>()))
            .ReturnsAsync(TestData.GetVehicleInsurancePolicyDetailResponseDTO());

        _vehicleInsurancePolicyRepository.Setup(x => x.GetInsuranceCompanyListAsync(It.IsAny<InsuranceCompanyListRequestDto>()))
            .ReturnsAsync(TestData.GetCompanyIds());

        _vehicleInsurancePolicyRepository.Setup(x => x.GetPolicyDetailsByPolicyIDAsync(It.IsAny<long>()))
            .ReturnsAsync(TestData.GetPolicyDetailResponseDto());

        _vehicleInsurancePolicyRepository.Setup(x => x.GetVehicleDetailsByVehicleIDAsync(It.IsAny<long>()))
          .ReturnsAsync(TestData.GetVehicleDetailResponseDto());

        _vehicleInsurancePolicyRepository.Setup(x => x.GetPolicyExpiryDataAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<short>(), It.IsAny<short>()))
          .ReturnsAsync(TestData.GetInquiryResponseDto());

        var appSettings = @"{""AppSettings"":{
            ""DefaultPageNumberWhenPaginationIsFalse"" : ""1"",
            ""DefaultPageSizeWhenPaginationIsFalse"" : ""10"",
            ""MaximumPageSize"" : ""1500""
            }}";

        var builder = new ConfigurationBuilder();

        builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

        var configuration = builder.Build();

        _vehicleInsurancePolicyDetailValidator = new VehicleInsurancePolicyDetailValidator(_localizer, configuration);
        _insuranceCompanyListValidator = new InsuranceCompanyListValidator(_localizer);
        _policyOrVehicleCountRequestValidator = new PolicyOrVehicleCountRequestValidator(_localizer);

        _policyAppService = new PolicyAppService(_idVehicleReadOnlyRepo.Object, _idPolicyReadOnlyRepo.Object, _vehicleInsurancePolicyDetailValidator, _insuranceCompanyListValidator, _policyOrVehicleCountRequestValidator, _vehicleInsurancePolicyRepository.Object);
    }

    #endregion


    #region Get Vehicle Insurance Policy Detail

    [Fact]
    public async Task Check_valid_request_dto()
    {
        VehicleInsurancePolicyRequestDto vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        var vehicleInsurancePolicyDetailResponseDto = await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto);
        vehicleInsurancePolicyDetailResponseDto.ShouldNotNull();
    }

    [Fact]
    public async Task Check_request_dto_should_not_null()
    {
        VehicleInsurancePolicyRequestDto vehicleInsurancePolicyDetailRequestDto = null;
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
    }

    [Fact]
    public async Task Check_request_dto_page_size_exceed()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.PageSize = 1700;

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "PageSize" && o.ErrorCode == PolicyErrorCode.MaximumPageSizeExceed);
    }

    [Fact]
    public async Task Check_request_dto_plate_number_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.PlateNumber = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
      
        Assert.Contains(ex.Errors, o => o.PropertyName == "PlateNumber" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_first_char_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.FirstChar = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
     
        Assert.Contains(ex.Errors, o => o.PropertyName == "FirstChar" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_second_char_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.SecondChar = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "SecondChar" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_third_char_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.ThirdChar = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
     
        Assert.Contains(ex.Errors, o => o.PropertyName == "ThirdChar" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_custom_id_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.CustomID = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
   
        Assert.Contains(ex.Errors, o => o.PropertyName == "CustomID" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_mobile_no_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.MobileNo = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
    
        Assert.Contains(ex.Errors, o => o.PropertyName == "MobileNo" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_culture_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.Culture = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
     
        Assert.Contains(ex.Errors, o => o.PropertyName == "Culture" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_sponsor_number_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.Sponsornumber = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
   
        Assert.Contains(ex.Errors, o => o.PropertyName == "Sponsornumber" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_vehicle_id_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.VehicleId = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "VehicleId" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_policy_id_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.PolicyId = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "PolicyId" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_chassis_number_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.ChassisNumber = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
      
        Assert.Contains(ex.Errors, o => o.PropertyName == "ChassisNumber" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_defined_by_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.DefinedBy = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
 
        Assert.Contains(ex.Errors, o => o.PropertyName == "DefinedBy" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_case_number_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.CaseNumber = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
 
        Assert.Contains(ex.Errors, o => o.PropertyName == "CaseNumber" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_manufacturer_year_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.ManufacturerYear = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "ManufacturerYear" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_select_fields_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.SelectFields = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "SelectFields" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_search_text_not_contain_xss()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.SearchText = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "SearchText" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_request_dto_fields_should_not_null()
    {
        var vehicleInsurancePolicyDetailRequestDto = TestData.GetVehicleInsurancePolicyDetailRequestDTO();
        vehicleInsurancePolicyDetailRequestDto.PlateNumber = null;
        vehicleInsurancePolicyDetailRequestDto.FirstChar = null;
        vehicleInsurancePolicyDetailRequestDto.SecondChar = null;
        vehicleInsurancePolicyDetailRequestDto.ThirdChar = null;
        vehicleInsurancePolicyDetailRequestDto.ChassisNumber = null;
        vehicleInsurancePolicyDetailRequestDto.AccidentDate = null;
        vehicleInsurancePolicyDetailRequestDto.PlateType = null;
        vehicleInsurancePolicyDetailRequestDto.SequenceNumber = null;
        vehicleInsurancePolicyDetailRequestDto.CustomID = null;
        vehicleInsurancePolicyDetailRequestDto.MobileNo = null;
        vehicleInsurancePolicyDetailRequestDto.Culture = null;
        vehicleInsurancePolicyDetailRequestDto.Sponsornumber = null;
        vehicleInsurancePolicyDetailRequestDto.PolicyNumber = null;
        vehicleInsurancePolicyDetailRequestDto.SubPolicyNumber = null;
        vehicleInsurancePolicyDetailRequestDto.PolicyId = null;
        vehicleInsurancePolicyDetailRequestDto.DefinedBy = null;
        vehicleInsurancePolicyDetailRequestDto.ManufacturerYear = null;


        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetVehicleInsurancePolicyDetailAsync(vehicleInsurancePolicyDetailRequestDto));
     
        Assert.Contains(ex.Errors, o => o.ErrorMessage == _localizer["INVALID_PARAMETERS"]);
    }

    #endregion    

    #region Get Insurance Company List

    [Fact]
    public async Task Check_get_insurance_company_list()
    {
        InsuranceCompanyListRequestDto insuranceCompanyListRequestDto = TestData.GetInsuranceCompanyListRequestDto();

        List<string> insuranceCompanyListResponseDto = await _policyAppService.GetInsuranceCompanyListAsync(insuranceCompanyListRequestDto);

        (insuranceCompanyListResponseDto.Count > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_insurance_company_list_request_dto_should_not_null()
    {
        InsuranceCompanyListRequestDto insuranceCompanyListRequestDto = null;
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _policyAppService.GetInsuranceCompanyListAsync(insuranceCompanyListRequestDto));
    }

    [Fact]
    public async Task Check_first_char_not_contain_xss()
    {
        InsuranceCompanyListRequestDto insuranceCompanyListRequestDto = TestData.GetInsuranceCompanyListRequestDto();
        insuranceCompanyListRequestDto.FirstChar = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetInsuranceCompanyListAsync(insuranceCompanyListRequestDto));
        
        Assert.Contains(ex.Errors, o => o.PropertyName == "FirstChar" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_second_char_not_contain_xss()
    {
        InsuranceCompanyListRequestDto insuranceCompanyListRequestDto = TestData.GetInsuranceCompanyListRequestDto();
        insuranceCompanyListRequestDto.SecondChar = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetInsuranceCompanyListAsync(insuranceCompanyListRequestDto));
      
        Assert.Contains(ex.Errors, o => o.PropertyName == "SecondChar" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_third_char_not_contain_xss()
    {
        InsuranceCompanyListRequestDto insuranceCompanyListRequestDto = TestData.GetInsuranceCompanyListRequestDto();
        insuranceCompanyListRequestDto.ThirdChar = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetInsuranceCompanyListAsync(insuranceCompanyListRequestDto));

        Assert.Contains(ex.Errors, o => o.PropertyName == "ThirdChar" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    [Fact]
    public async Task Check_dto_plate_number_not_contain_xss()
    {
        InsuranceCompanyListRequestDto insuranceCompanyListRequestDto = TestData.GetInsuranceCompanyListRequestDto();
        insuranceCompanyListRequestDto.PlateNumber = "<script>";

        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _policyAppService.GetInsuranceCompanyListAsync(insuranceCompanyListRequestDto));
       
        Assert.Contains(ex.Errors, o => o.PropertyName == "PlateNumber" && o.ErrorCode == PolicyErrorCode.InvalidDataErrorCode);
    }

    #endregion

    #region Get Policy Details By PolicyID

    [Fact]
    public async Task Check_get_policy_details_by_policy_id()
    {
        List<PolicyDetailResponseDto> policyDetailResponseDtos = await _policyAppService.GetPolicyDetailsByPolicyIDAsync(1);

        (policyDetailResponseDtos.Count > 0).ShouldBeTrue();
    }

    #endregion

    #region Get Vehicle Details By VehicleID

    [Fact]
    public async Task Check_get_vehicle_details_by_vehicle_id()
    {
        List<VehicleDetailResponseDto> vehicleDetailResponseDtos = await _policyAppService.GetVehicleDetailsByVehicleIDAsync(1);

        (vehicleDetailResponseDtos.Count > 0).ShouldBeTrue();
    }

    #endregion

    #region Get Policy Expiry Data

    [Fact]
    public async Task Check_get_policy_expiry_data()
    {
        List<InquiryResponseDto> inquiryResponseDtos = await _policyAppService.GetPolicyExpiryDataAsync(DateTime.Now, DateTime.Now, DateTime.Now, 10, 1);

        (inquiryResponseDtos.Count > 0).ShouldBeTrue();
    }

    #endregion

    #region Get Policy Or Vehicle Count

    [Fact]
    public async Task Check_get_all_count()
    {
        PolicyOrVehicleCountRequestDto policyOrVehicleCountRequestDto = TestData.GetPolicyOrVehicleCountRequestDto();
        policyOrVehicleCountRequestDto.CountBy = CountBy.All;

        PolicyOrVehicleCountResponseDto policyOrVehicleCountResponseDto = await _policyAppService.GetPolicyOrVehicleCountAsync(policyOrVehicleCountRequestDto);

        policyOrVehicleCountResponseDto.ShouldNotNull();
        (policyOrVehicleCountResponseDto.TotalInsurancePolicies > 0).ShouldBeTrue();
        (policyOrVehicleCountResponseDto.TotalInsuredVehicles > 0).ShouldBeTrue();
        (policyOrVehicleCountResponseDto.TotalVehicles > 0).ShouldBeTrue();
        (policyOrVehicleCountResponseDto.TotalNonInsuredVehicles > 0).ShouldBeTrue();
        (policyOrVehicleCountResponseDto.TotalVehiclesOfExpiringPolicy > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_get_total_insurance_policies()
    {
        PolicyOrVehicleCountRequestDto policyOrVehicleCountRequestDto = TestData.GetPolicyOrVehicleCountRequestDto();
        policyOrVehicleCountRequestDto.CountBy = CountBy.TotalInsurancePolicies;

        PolicyOrVehicleCountResponseDto policyOrVehicleCountResponseDto = await _policyAppService.GetPolicyOrVehicleCountAsync(policyOrVehicleCountRequestDto);

        (policyOrVehicleCountResponseDto.TotalInsurancePolicies > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_get_total_insured_vehicles()
    {
        PolicyOrVehicleCountRequestDto policyOrVehicleCountRequestDto = TestData.GetPolicyOrVehicleCountRequestDto();
        policyOrVehicleCountRequestDto.CountBy = CountBy.TotalInsuredVehicles;

        PolicyOrVehicleCountResponseDto policyOrVehicleCountResponseDto = await _policyAppService.GetPolicyOrVehicleCountAsync(policyOrVehicleCountRequestDto);

        (policyOrVehicleCountResponseDto.TotalInsuredVehicles > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_get_total_non_insured_vehicles()
    {
        PolicyOrVehicleCountRequestDto policyOrVehicleCountRequestDto = TestData.GetPolicyOrVehicleCountRequestDto();
        policyOrVehicleCountRequestDto.CountBy = CountBy.TotalNonInsuredVehicles;

        PolicyOrVehicleCountResponseDto policyOrVehicleCountResponseDto = await _policyAppService.GetPolicyOrVehicleCountAsync(policyOrVehicleCountRequestDto);

        (policyOrVehicleCountResponseDto.TotalNonInsuredVehicles > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_get_total_vehicles()
    {
        PolicyOrVehicleCountRequestDto policyOrVehicleCountRequestDto = TestData.GetPolicyOrVehicleCountRequestDto();
        policyOrVehicleCountRequestDto.CountBy = CountBy.TotalVehicles;

        PolicyOrVehicleCountResponseDto policyOrVehicleCountResponseDto = await _policyAppService.GetPolicyOrVehicleCountAsync(policyOrVehicleCountRequestDto);

        (policyOrVehicleCountResponseDto.TotalVehicles > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_get_total_vehicles_of_expiring_policy()
    {
        PolicyOrVehicleCountRequestDto policyOrVehicleCountRequestDto = TestData.GetPolicyOrVehicleCountRequestDto();
        policyOrVehicleCountRequestDto.CountBy = CountBy.TotalVehiclesOfExpiringPolicy;

        PolicyOrVehicleCountResponseDto policyOrVehicleCountResponseDto = await _policyAppService.GetPolicyOrVehicleCountAsync(policyOrVehicleCountRequestDto);

        (policyOrVehicleCountResponseDto.TotalVehiclesOfExpiringPolicy > 0).ShouldBeTrue();
    }

    #endregion

}
