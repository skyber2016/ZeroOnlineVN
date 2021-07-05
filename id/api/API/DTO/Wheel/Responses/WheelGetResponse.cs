using System.Collections.Generic;

namespace API.DTO.Wheel.Responses
{
    public class WheelGetResponse
    {
        public IEnumerable<WheelLogResponse> Histories { get; set; }
        public int WheelCount { get; set; }
    }
}
