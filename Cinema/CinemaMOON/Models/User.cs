using System;
using System.Collections.Generic;

namespace CinemaMOON.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }  
		public string Salt { get; set; } 
		public bool IsAdmin { get; set; }

		public ICollection<Order> Orders { get; set; } = new List<Order>();
	}
}