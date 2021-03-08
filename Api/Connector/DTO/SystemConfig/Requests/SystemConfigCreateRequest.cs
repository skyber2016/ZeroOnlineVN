namespace Forum_API.DTO.SystemConfig.Requests
{
    public class SystemConfigCreateRequest
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}
