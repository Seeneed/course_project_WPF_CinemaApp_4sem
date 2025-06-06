using System;
using System.Collections.Generic;

namespace CinemaMOON.Models
{
	public class Movie
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Director { get; set; }
		public string Description { get; set; }
		public string Genre { get; set; }
		public int Duration { get; set; }
		public string Rating { get; set; }
		public string Photo { get; set; }
		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;

		public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
	}
}