using System;

namespace CinemaMOON.Models
{
	public class Schedule
	{
		public Guid Id { get; set; }
		public DateTime ShowTime { get; set; }
		public int AvailableSeats { get; set; }
		public bool IsDeleted { get; set; } = false;

		public Guid MovieId { get; set; }
		public Movie Movie { get; set; }

		public Guid HallId { get; set; }
		public Hall Hall { get; set; }

		public ICollection<Order> Orders { get; set; } = new List<Order>();
	}
}