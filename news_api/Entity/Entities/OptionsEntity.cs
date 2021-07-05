using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_options")]
    public class OptionsEntity : BaseEntity
    {
		[Key]
		[Column("option_id")]
		public string OptionId { get; set; }

		[Column("option_name")]
		public string OptionName { get; set; }

		[Column("option_value")]
		public string OptionValue { get; set; }

		[Column("autoload")]
		public string Autoload { get; set; }


    }
}
