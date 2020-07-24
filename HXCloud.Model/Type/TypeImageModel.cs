namespace HXCloud.Model
{
    public class TypeImageModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Url { get; set; }
        public int Rank { get; set; } = 1;//图片顺序
        public string Description { get; set; }

        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
    }
}