using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_customer_lookup")]
    public class WcCustomerLookupEntity : BaseEntity
    {
		[Key]
		[Column("customer_id")]
		public string CustomerId { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("username")]
		public string Username { get; set; }

		[Column("first_name")]
		public string FirstName { get; set; }

		[Column("last_name")]
		public string LastName { get; set; }

		[Column("email")]
		public string Email { get; set; }

		[Column("date_last_active")]
		public string DateLastActive { get; set; }

		[Column("date_registered")]
		public string DateRegistered { get; set; }

		[Column("country")]
		public char Country { get; set; }

		[Column("postcode")]
		public string Postcode { get; set; }

		[Column("city")]
		public string City { get; set; }

		[Column("state")]
		public string State { get; set; }


    }
}
