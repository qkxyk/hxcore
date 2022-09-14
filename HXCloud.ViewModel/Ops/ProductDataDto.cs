using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //污水生产数据，主要用来记录运维时处理的数据
    public class ProductDataDto
    {
        public int Id { get; set; }
        public float Water { get; set; }//处理数量
        public float Power { get; set; }//耗电量
        public ICollection<AgentiaModel> Agentias { get; set; }//药剂消耗量
    }

    /// <summary>
    /// 药剂，单位Kg
    /// </summary>
    public class AgentiaModel
    {
        //碳源
        public float CarbonSource { get; set; }
        //pac
        public float PAC { get; set; }
        //消毒剂
        public float Disinfectant { get; set; }
    }
}
