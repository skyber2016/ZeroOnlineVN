namespace Cambopay_API.Entities
{
    public class StatusEntity
    {
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public StatusEntity()
        {
            this.IsEnabled = true;
        }
    }
}
