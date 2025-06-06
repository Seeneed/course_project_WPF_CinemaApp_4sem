using System;

namespace CinemaMOON.Models
{
	public class Order
	{
		public Guid Id { get; set; }
		public string Seats { get; set; }
		public decimal TotalPrice { get; set; }
		public string OrderStatus { get; set; }
		public DateTime BookingTimestamp { get; set; } = DateTime.Now;
		public int? UserRating { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid ScheduleId { get; set; }
		public Schedule Schedule { get; set; }
	}
}