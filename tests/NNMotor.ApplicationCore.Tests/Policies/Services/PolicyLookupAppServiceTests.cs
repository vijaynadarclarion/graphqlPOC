using System.Linq.Expressions;
using Ardalis.Specification;
using AutoMapper;
using Moq;
using NNMotor.ApplicationCore.Entities;
using NNMotor.ApplicationCore.Interfaces;
using NNMotor.ApplicationCore.Policies;
using NNMotor.ApplicationCore.Policies.Dtos;
using NNMotor.ApplicationCore.Policies.Interfaces;
using NNMotor.ApplicationCore.Policies.Services;
using NNMotor.Tests;
using Xunit;

namespace NNMotor.ApplicationCore.Tests.Policies.Services;

public class PolicyLookupAppServiceTests
{
    private readonly IMapper _mapper;
    private IPolicyLookupAppService _policyLookupAppService;

    public PolicyLookupAppServiceTests()
    {
        var myProfile = new PolicyObjectMapping();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);

        SetUp();
    }

    #region Common

    private void SetUp()
    {
        Mock<IReadOnlyRepository<Make>> _makeRepository = new Mock<IReadOnlyRepository<Make>>();
        Mock<IReadOnlyRepository<Model>> _modelRepository = new Mock<IReadOnlyRepository<Model>>();
        Mock<IReadOnlyRepository<CoverageTypeMaster>> _coverageTypeRepository = new Mock<IReadOnlyRepository<CoverageTypeMaster>>();
        Mock<IReadOnlyRepository<PlateCharacterMaster>> _plateCharacterRepository = new Mock<IReadOnlyRepository<PlateCharacterMaster>>();

        _makeRepository.Setup(x => x.GetByQuery(It.IsAny<Expression<Func<Make, bool>>>()))
              .ReturnsAsync((Expression<Func<Make, bool>> expression) =>
              {
                  var data = TestData.GetVehicleMakes().Where(expression.Compile()).ToList();
                  return data;
              });

        _modelRepository.Setup(x => x.GetByQuery(It.IsAny<Expression<Func<Model, bool>>>()))
         .ReturnsAsync((Expression<Func<Model, bool>> expression) =>
         {
             var data = TestData.GetVehicleModels().Where(expression.Compile()).ToList();
             return data;
         });

        _coverageTypeRepository.Setup(x => x.GetByQuery(It.IsAny<Expression<Func<CoverageTypeMaster, bool>>>()))
       .ReturnsAsync((Expression<Func<CoverageTypeMaster, bool>> expression) =>
       {
           var data = TestData.GetCoverageTypes().Where(expression.Compile()).ToList();
           return data;
       });

        _plateCharacterRepository.Setup(x => x.GetAll())
                .ReturnsAsync(TestData.GetPlateCharacters());

        _policyLookupAppService = new PolicyLookupAppService(_makeRepository.Object, _modelRepository.Object, _coverageTypeRepository.Object, _plateCharacterRepository.Object, _mapper);
    }

    #endregion

    #region Test Cases

    [Fact]
    public async Task Check_vehicle_make_data()
    {
        List<LookupDataResponseDto> result = await _policyLookupAppService.GetAllVehicleMakeDataAsync();

        (result.Count > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_vehicle_model_data()
    {
        List<LookupDataResponseDto> result = await _policyLookupAppService.GetVehicleModelDataAsync(1);

        (result.Count > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_vehicle_model_data_exist()
    {
        List<LookupDataResponseDto> result = await _policyLookupAppService.GetVehicleModelDataAsync(5);

        (result.Count > 0).ShouldBeFalse();
    }

    [Fact]
    public async Task Check_coverage_type_data()
    {
        List<LookupDataResponseDto> result = await _policyLookupAppService.GetCoverageTypeDataAsync();

        (result.Count > 0).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_plate_character_data()
    {
        List<LookupDataResponseDto> result = await _policyLookupAppService.GetPlateCharacterDataAsync();

        (result.Count > 0).ShouldBeTrue();
    }

    #endregion
}
