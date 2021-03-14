namespace API.DTO.User.Responses
{
    public class UserGetResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public int VIP { get; set; }
        public string NetbarIP { get; set; }
        public int WebMoney { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
    }
}
