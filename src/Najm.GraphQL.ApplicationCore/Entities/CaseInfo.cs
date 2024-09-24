using System;
using System.Collections.Generic;

namespace Najm.GraphQL.ApplicationCore.Entity;

public partial class AccidentInfo
{
    public int CaseInfoId { get; set; }

    public string? CaseNumber { get; set; }

    public byte CaseStatusId { get; set; }

    public int? CaseTypeId { get; set; }

    public int CaseTypeReasonId { get; set; }

    public int CaseStageId { get; set; }

    public int CallCenterAgentId { get; set; }

    public int? CommandCenterAgentId { get; set; }

    public int? DataEntryId { get; set; }

    public DateTime? CaseMoveToCommandQueuTime { get; set; }

    public DateTime? CaseAttendedByCommandAgentTime { get; set; }

    public DateTime? CaseRegisterationTime { get; set; }

    public DateTime? CaseClosingTime { get; set; }

    public int? SurveyorAssigned { get; set; }

    public DateTime? SurveyorAssignedTime { get; set; }

    public DateTime? SurveyorArrivalTime { get; set; }

    public DateTime? SurveyorClosingTime { get; set; }

    public DateTime? SurveyorAcceptJobHandsetTime { get; set; }

    public DateTime? SurveyorArrivalHandsetTime { get; set; }

    public DateTime? SurveyorClosingHandsetTime { get; set; }

    public string? AccidentDesc { get; set; }

    public int? CityId { get; set; }

    public int? ZoneId { get; set; }

    public int? SubZoneId { get; set; }

    public int? AreaId { get; set; }

    public int? StreetId { get; set; }

    public int? CrossStreetId { get; set; }

    public int? LandmarkId { get; set; }

    public string? LatId { get; set; }

    public string? LongId { get; set; }

    public int? TypePriorityId { get; set; }

    public string? TypePriorityDesc { get; set; }

    public int? OutofCoverageId { get; set; }

    public string? OutofCoverageDesc { get; set; }

    public string? LocationDesc { get; set; }

    public DateTime? DataEntryCompletedTime { get; set; }

    public bool? IsQtyFraud { get; set; }

    public bool? IsQtyImage { get; set; }

    public bool? IsQtyReports { get; set; }

    public DateTime? PhotoUploadTime { get; set; }

    public int? MainCaseInfoId { get; set; }

    public string? PoliceName { get; set; }

    public int? TypeOfAccidentLocationId { get; set; }

    public int? TypeOfAccidentWayId { get; set; }

    public int? TypeOfTrafficSideId { get; set; }

    public DateTime? CaseAssignedToDataEntryTime { get; set; }

    public string? SurveyorAcceptJobLat { get; set; }

    public string? SurveyorAcceptJobLong { get; set; }

    public string? SurveyorArrivalLat { get; set; }

    public string? SurveyorArrivalLong { get; set; }

    public string? SurveyorClosingLat { get; set; }

    public string? SurveyorClosingLong { get; set; }

    public DateTime? CaseActivationJobHandsetTime { get; set; }

    public double? SurveyorActiveCaseLat { get; set; }

    public double? SurveyorActiveCaseLong { get; set; }

    public string? SurveyorArrivalLocAccuracy { get; set; }

    public string? SurveyorClosingLocAccuracy { get; set; }

    public DateTime? SplitRegistrationDateTime { get; set; }

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public byte IsFemaleInvolved { get; set; }

    public byte? CaseFraudSourceId { get; set; }

    public byte? NoOfPartiesInvolved { get; set; }

    public bool? IsRemoteSurveying { get; set; }

    public bool? IsRemoteSurveyingPhotoUpload { get; set; }

    public int? SurveyorShiftId { get; set; }

    public bool? IsRoadSecurityCase { get; set; }

    public int? DrawDamageSceneUserId { get; set; }

    public int? EventTypeId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? DisabilityTypeIds { get; set; }

    public bool? IsOfflineCase { get; set; }

   // public ICollection<AccidentParty> AccidentParties { get; set; }

}
