using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("Role")]
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Prioritize { get; set; }
        public string Color { get; set; }
        public string Classes
        {
            get
            {
                if(this.Prioritize == -1)
                {
                    return "system-control";
                }
                return string.Empty;
            }
        }
    }
}
