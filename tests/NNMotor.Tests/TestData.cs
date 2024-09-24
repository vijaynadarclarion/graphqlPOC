using Adf.Core;
using NNMotor.ApplicationCore.Entities;
using NNMotor.ApplicationCore.Policies.Dtos;

namespace NNMotor.Tests
{
    public static class TestData
    {
        private static Random random = new Random();

        #region Policy

        public static VehicleInsurancePolicyRequestDto GetVehicleInsurancePolicyDetailRequestDTO()
        {
            return new VehicleInsurancePolicyRequestDto()
            {
                AccidentDate = DateTime.Now,
                CustomID = "622332900",
                FirstChar = "1",
                SecondChar = "2",
                ThirdChar = "3",
                PlateNumber = "9857",
                PlateType = "2",
                PolicyNumber = "12689413",
                SequenceNumber = 622332900,
                ChassisNumber = "1PKEC16T36Q177508",
                SourceID = 1,
                SubPolicyNumber = "2378968",
                Culture = "en-US",
                MobileNo = "123456789",
                Sponsornumber = "2011655343",
                UserId = 1,
                PageSize = 1,
                PageNumber = 1,
                DefinedBy = "1",
                PolicyId = "85301934",
                CaseNumber = "225234",
                ManufacturerYear = "1994",
                SelectFields = "Policy_ID,Name,Customer_ID,Policy_Number,Insured_Type_ID",
                SearchText = "test"
            };
        }

        public static PagedList<dynamic> GetVehicleInsurancePolicyDetailResponseDTO()
        {            
            var listResponse = new List<dynamic> {               
                new 
            {
                Policy_ID = 96299,
                Name = "AHMAD BAQER M AL JUBARA ",
                Customer_ID = 123,
                Policy_Number = "F57-22-300-00000391",
                Insured_Type_ID = 1,
                Sponsor_Number = "1030718116",
                Version_Type = "1",
                Phone_Number = "0505140201",
                Mobile_Number = "0505140201",
                Postal_Code = "36377",
                PO_Box = "36377",
                City = "الجفر",
                Max_Liability = "100",
                Driver_Age_Limit = 5,
                Creation_Date = DateTime.Now,
                Sys_Date = DateTime.Now,
                ELM_ID = 572843,
                Owner_Nationality = "Saudi Arabia",
                Owner_Nationality_ID = 244,
                Vehicle_ID = 1227121,
                Sub_Policy_Number = "7823862",
                Defiend_By = "2",
                Plat_Number = "7616",
                First_Plat_Letter = "15",
                Second_Plate_Letter = "13",
                Third_Plate_Letter = "4",
                Plate_Type = "Private Car",
                Custom_ID = "1234",
                Seq_Number = 885324,
                Coverage_Type = "2",
                Issue_Hijri_Date = DateTime.Now,
                Effective_Hijri_Date = DateTime.Now,
                Expire_Hijri_Date = DateTime.Now,
                Issue_Gre_Date = DateTime.Now,
                Effective_Gre_Date = DateTime.Now,
                Expire_Gre_Date = DateTime.Now,
                Category = "109",
                Manufacture = "Ford",
                Manufacturing_Year = "2011",
                Model = "Explorer",
                Color = "Gray",
                Chassis_Number = "1FMHK8D85BGA21170",
                VehCustomer_ID = 123,
                VehSys_Date = DateTime.Now,
                VehELM_ID = 773468,
                Under21Age = "false",
                InsuranceCompanyName = "Walaa Cooperative Insurance Company",
                InsuranceCompanyNameAR = "شركة ولاء للتأمين التعاوني",
                Owner_Date_Of_Birth = DateTime.Now,
                Model_Id = 2034,
                Color_Id = 11,
                Manufacture_Id = 109
            }
            };

            return new PagedList<dynamic>(listResponse, 1, 10, 1);
        }
            
        public static List<IdPolicy> GetIdPolicies()
        {
            return new List<IdPolicy> {
                new IdPolicy()
                {
                Name = "SAHIR MOUSA TAHLAWI ALJUHNI",
                CustomerId = 102,
                PolicyNumber = "8494890",
                InsuredTypeId = 1,
                SponsorNumber = "1028432894",
                VersionType = "1",
                PhoneNumber = null,
                MobileNumber = null,
                PostalCode = "12345",
                PoBox = "12345",
                City = null,
                MaxLiabilit = "1",
                DriverAgeLimit = 30,
                CreationDate = null,
                SysDate = DateTime.Now.AddDays(-1),
                ElmId = 288880
            } };
        }

        public static List<IdVehicle> GetIdVehicles()
        {
            return new List<IdVehicle> {
                new IdVehicle()
                {
                     PolicyId = 3103290,
            VehicleId = 14668594,
            SubPolicyNumber = "2381319",
            DefiendBy =  "1",
            PlatNumber = "9664",
            FirstPlatLetter = "9",
            SecondPlateLetter = "13",
            ThirdPlateLetter = "13",
            PlateType = "1",
            CustomId = null,
            SeqNumber = null,
            CoverageType = "2",
            IssueHijriDate =  DateTime.Now.AddDays(-1),
            EffectiveHijriDate = DateTime.Now.AddDays(-1),
            ExpireHijriDate =  DateTime.Now.AddDays(-1),
            IssueGreDate =  DateTime.Now.AddDays(-1),
            EffectiveGreDate =  DateTime.Now.AddDays(-1),
            ExpireGreDate =  DateTime.Now.AddDays(-1),
            Category = "JEEP",
            Manufacture = "G.M.C.",
            ManufacturingYear = "2007",
            Model = "SUBERBN",
            Color = null,
            ChassisNumber = "221364",
            CustomerId = 102,
            SysDate = DateTime.Now.AddDays(-1),
            ElmId = 3499322,
            Under21 =null,
            Policy = new IdPolicy()
            {
                Name = "SAHIR MOUSA TAHLAWI ALJUHNI",
                CustomerId = 102,
                PolicyNumber = "8494890",
                InsuredTypeId = 1,
                SponsorNumber = "1028432894",
                VersionType =  "1",
                PhoneNumber = null,
                MobileNumber = null,
                PostalCode = "12345",
                PoBox ="12345",
                City = null,
                MaxLiabilit = "1",
                DriverAgeLimit = 30,
                CreationDate = null,
                SysDate = DateTime.Now.AddDays(-1),
                ElmId = 288880
            }
                },
                 new IdVehicle()
                {
                     PolicyId = 46235423,
            VehicleId = 1463244,
            SubPolicyNumber = "3241319",
            DefiendBy =  "2",
            PlatNumber = "3664",
            FirstPlatLetter = "2",
            SecondPlateLetter = "3",
            ThirdPlateLetter = "5",
            PlateType = "3",
            CustomId = null,
            SeqNumber = null,
            CoverageType = "1",
            IssueHijriDate =  DateTime.Now.AddDays(-1),
            EffectiveHijriDate = DateTime.Now.AddDays(-1),
            ExpireHijriDate =  DateTime.Now.AddDays(-1),
            IssueGreDate =  DateTime.Now.AddDays(-1),
            EffectiveGreDate =  DateTime.Now.AddDays(-1),
            ExpireGreDate =  DateTime.Now.AddDays(-1),
            Category = "SUV",
            Manufacture = "TATA",
            ManufacturingYear = "2010",
            Model = "NEXON",
            Color = null,
            ChassisNumber = "451364",
            CustomerId = 101,
            SysDate = DateTime.Now.AddDays(-1),
            ElmId = 2233322,
            Under21 =null,
            Policy = new IdPolicy()
            {
                Name = "ALJUHNI",
                CustomerId = 100,
                PolicyNumber = "2394890",
                InsuredTypeId = 2,
                SponsorNumber = "2218432894",
                VersionType =  "2",
                PhoneNumber = null,
                MobileNumber = null,
                PostalCode = "2345",
                PoBox ="44345",
                City = null,
                MaxLiabilit = "2",
                DriverAgeLimit = 40,
                CreationDate = null,
                SysDate = DateTime.Now.AddDays(-1),
                ElmId = 456780
            }
                }
            };
        }

        public static List<PlateCharacterMaster> GetPlateCharacters()
        {
            return new List<PlateCharacterMaster> {
                new PlateCharacterMaster()
                {
                    PlateCharacterId = 1,
                    PlateCharacterEnglishName = "A",
                    PlateCharacterArabicName = "ا",
                    InsertedBy = 1,
                    InsertedDatetime = DateTime.Now
                },
                 new PlateCharacterMaster()
                {
                    PlateCharacterId = 2,
                    PlateCharacterEnglishName = "B",
                    PlateCharacterArabicName = "ب",
                    InsertedBy = 1,
                    InsertedDatetime = DateTime.Now
                },
                 new PlateCharacterMaster()
                {
                    PlateCharacterId = 3,
                    PlateCharacterEnglishName = "D",
                    PlateCharacterArabicName = "د",
                    InsertedBy = 1,
                    InsertedDatetime = DateTime.Now
                }
            };
        }

        public static List<PlateTypeMaster> GetPlateTypes()
        {
            return new List<PlateTypeMaster> {
                new PlateTypeMaster()
                {
                    PlateTypeId = 1,
                    PlateTypeEnglishName = "Private Car",
                    PlateTypeArabicName = "خصوصي",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = null
                },
                new PlateTypeMaster()
                {
                    PlateTypeId = 2,
                    PlateTypeEnglishName = "Public Transport",
                    PlateTypeArabicName = "نقل عام",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = null
                },
                new PlateTypeMaster()
                {
                    PlateTypeId = 3,
                    PlateTypeEnglishName = "Private Transport",
                    PlateTypeArabicName = "نقل خاص",
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = null
                }
            };
        }

        public static List<Make> GetVehicleMakes()
        {
            return new List<Make> {
                new Make()
                {
                    Id = 1,
                    Active = true,
                    EnglishName = "VOGELE",
                    ArabicName = "فوجلي"
                },
                new Make()
                {
                    Id = 2,
                    Active = true,
                    EnglishName = "TGB",
                    ArabicName = "جي تي بي"
                },
            };
        }

        public static List<Model> GetVehicleModels()
        {
            return new List<Model> {
                new Model()
                {
                    Id = 1,
                    MakeId = 1,
                    Active = 1,
                    EnglishName = "PALISADE",
                    ArabicName = "فوجلي"
                },
                new Model()
                {
                    Id = 2,
                    MakeId= 2,
                    Active = 1,
                    EnglishName = "TELLURIDE",
                    ArabicName = "جي تي بي"
                },
            };
        }

        public static List<CoverageTypeMaster> GetCoverageTypes()
        {
            return new List<CoverageTypeMaster> {
                new CoverageTypeMaster()
                {
                    CoverageTypeId = 1,
                    IsActive = true,
                    CoverageTypeEnglishName = "Comprehensive",
                    CoverageTypeArabicName = "شامل"
                },
                new CoverageTypeMaster()
                {
                    CoverageTypeId = 2,
                    IsActive = true,
                    CoverageTypeEnglishName = "TPL",
                    CoverageTypeArabicName = "طرف ثالث"
                },
            };
        }

        public static PolicyOrVehicleCountRequestDto GetPolicyOrVehicleCountRequestDto()
        {
            return new PolicyOrVehicleCountRequestDto()
            {
                CrnOrUnnNumbers = new List<long>() { 2011655343 },
                PolicyNumbers = new List<string>() { "12689413" },
                CountBy = ApplicationCore.Policies.CountBy.All
            };
        }

        public static List<PolicyDetailResponseDto> GetPolicyDetailResponseDto()
        {
            return new List<PolicyDetailResponseDto> {
                new PolicyDetailResponseDto()
            {
                Policy_ID = 96299,
                Customer_ID = 123,
                Policy_Number = "F57-22-300-00000391",
                Version_Type = "1",
                Phone_Number = "0505140201",
                Mobile_Number = "0505140201",
                Max_Liability = "100",
                Driver_Age_Limit = null,
                Creation_Date = null,
                Sys_Date = DateTime.Now,
                Owner_Nationality = 1,
                Owner_Date_Of_Birth = DateTime.Now,
                CRNumber = "CR001"
            }
            };
        }

        public static List<VehicleDetailResponseDto> GetVehicleDetailResponseDto()
        {
            return new List<VehicleDetailResponseDto> {
                new VehicleDetailResponseDto()
            {
                Policy_ID = 96299,
                Customer_ID = 123,
                Defiend_By =  "1",
                Plat_Number = "9664",
                First_Plate_Letter = "9",
                Second_Plate_Letter = "13",
                Third_Plate_Letter = "13",
                Plate_Type = "1",
                Custom_ID = null,
                Seq_Number = null,
                Coverage_Type = "2",
                Manufacture = "G.M.C.",
                Model = "SUBERBN",
                Color = null,
                Chassis_Number = "221364"
            }
            };
        }

        public static PagedList<InquiryResponseDto> GetInquiryResponseDto()
        {
            var list = new List<InquiryResponseDto> {
                new InquiryResponseDto()
            {
                Customer_ID = 123,
                First_Plat_Letter = "9",
                Second_Plate_Letter = "13",
                Third_Plate_Letter = "13",
                Plate_Type = "1",
                Custom_ID = null,
                Seq_Number = null,
                Coverage_Type = "2",
                Plate_Number = "3664",
                Effective_Gre_Date =  DateTime.Now.AddDays(-1),
                Expire_Gre_Date =  DateTime.Now.AddDays(-1)
            }
            };

            return new PagedList<InquiryResponseDto>(list, 1, 10, 1);
        }

        public static List<string> GetCompanyIds()
        {
            return new List<string>()
            {
               "1323423",
               "23132321"
            };
        }

        public static InsuranceCompanyListRequestDto GetInsuranceCompanyListRequestDto()
        {
            return new InsuranceCompanyListRequestDto()
            {
                FirstChar = "1",
                SecondChar = "2",
                ThirdChar = "3",
                PlateNumber = "9857",
                PlateType = 2
            };
        }

        #endregion


        #region Common

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}

