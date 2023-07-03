using System;

namespace DetectDupeItem.Models
{
    internal class Item
    {
        public string RobotId { get; set; }
        public string PrimaryId { get; set; }
        public string PrimaryTypeId { get; set; }
        public string SecondId { get; set; }
        public string SecondTypeId { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsValid { get; set; }
    }
}
