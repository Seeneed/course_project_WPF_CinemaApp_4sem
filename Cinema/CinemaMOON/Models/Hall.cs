using System;
using System.Collections.Generic;

namespace CinemaMOON.Models
{
	public class Hall
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Capacity { get; set; }
		public string Type { get; set; }

		public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
	}
}