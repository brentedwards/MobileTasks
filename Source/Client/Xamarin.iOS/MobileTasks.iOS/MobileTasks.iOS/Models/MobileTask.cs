using System;

namespace MobileTasks.iOS.Models
{
	public class MobileTask
	{
		public int Id { get; set; }
		public string Sid { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateCompleted { get; set; }

		public string Description { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime? DateDue { get; set; }
	}
}
