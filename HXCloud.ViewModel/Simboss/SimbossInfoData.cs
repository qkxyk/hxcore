using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //simboss卡信息，系统只处理simboss单卡信息
    public class SimbossInfoData
    {

    }

    #region simboss卡数据
    public class SystemParams
    {
        public string appid { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
    }

    public class SingleCardParams : SystemParams
    {
        public string iccid { get; set; }
        public string imsi { get; set; }
        public string msisdn { get; set; }
    }

    /// <summary>
    /// simboss返回数据格式
    /// </summary>
    public class ResponseData
    {
        public string message { get; set; }
        public string detail { get; set; }
        public string code { get; set; }
        public bool success { get; set; }
    }
    /// <summary>
    /// simboss单卡返回数据
    /// </summary>
    public class SingelCardResponse : ResponseData
    {
        /// <summary>
        /// 单卡数据，新包需要返回的数据
        /// </summary>
        public CardResponse data { get; set; }
    }
    /// <summary>
    /// 批量卡数据返回
    /// </summary>
    public class BatchCardResponse : ResponseData
    {
        public List<CardResponse> data { get; set; }
    }

    public class DeviceCardInfo
    {
        public string DeviceSn { get; set; }
        public string DeviceNo { get; set; }
        public string DeviceName { get; set; }//设备名称
        public Nullable<int> ProjectId { get; set; }
        public string FullId { get; set; }//项目的完整路径标示，以/分割（主要用于权限验证，不用再递归项目）
        public string FullName { get; set; }//项目完整路径的项目的名称，以/分割
        public CardResponse CardResponse { get; set; }
    }
    /// <summary>
    /// simboss单卡数据
    /// </summary>
    public class CardResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imsi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string carrier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deviceStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expireDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastSyncDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int activeDuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool realnameRequired { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cardPoolId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string testingExpireDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ratePlanId { get; set; }
        /// <summary>
        /// 10.0M/月
        /// </summary>
        public string iratePlanName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double usedDataVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double totalDataVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool useCountAsVolume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ratePlanEffetiveDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ratePlanExpirationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double dataUsage { get; set; }
        /// <summary>
        /// 张三
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string realNameCertifystatus { get; set; }
        /// <summary>
        /// 根据 xxxxxxxxxx 订单号出库
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string memo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orgName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imeiStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double speedLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> functions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool nbCard { get; set; }
    }
    #endregion
}
