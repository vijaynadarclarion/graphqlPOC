using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Najm.GraphQL.ApplicationCore.Entities;
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
