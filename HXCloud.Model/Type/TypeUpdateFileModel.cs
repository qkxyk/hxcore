using System;

namespace HXCloud.Model
{
    public class TypeUpdateFileModel :BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Version { get; set; }
        //public string Account { get; set; }
        //public DateTime Dt { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
    }
}