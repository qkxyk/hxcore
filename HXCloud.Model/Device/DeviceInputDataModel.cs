using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DeviceInputDataModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string DeviceSn { get; set; }
        public DateTime Dt { get; set; } = DateTime.Now;
        public int WaterInflow { get; set; }//日进水量
        public string COD_In { get; set; }//cod进水
        public string COD_Out { get; set; }//cod出水
        public string COD_Unit { get; set; }//cod单位mg/L
        public string NH3_N_In { get; set; }//NH3-N进水
        public string NH3_N_Out { get; set; }//NH3-N出水
        public string NH3_N_Unit { get; set; }//NH3-N单位mg/L
        public string TN_In { get; set; }//TN进水
        public string TN_Out { get; set; }//TN出水
        public string TN_Unit { get; set; }//TN单位mg/L
        public string TP_In { get; set; }//TP进水
        public string TP_Out { get; set; }//TP出水
        public string TP_Unit { get; set; }//TP单位mg/L
        public string SS_In { get; set; }//SS进水
        public string SS_Out { get; set; }//SS出水
        public string SS_Unit { get; set; }//SS单位mg/L
        public string PH_In { get; set; }//PH进水
        public string PH_Out { get; set; }//PH出水
        public string PH_Unit { get; set; }//PH单位
        public virtual DeviceModel Device { get; set; }

    }
}
