using System.Collections.Generic;

namespace API.DTO.Wheel.Responses
{
    public class WheelCreateResponse
    {
        public string Message { get; set; }
        public int WheelRemain { get; set; }
        public IEnumerable<WheelLogResponse> Histories { get; set; }
    }
}
