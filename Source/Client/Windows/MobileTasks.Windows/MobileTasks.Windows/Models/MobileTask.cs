using System;

namespace MobileTasks.Windows.Models
{
	public class MobileTask : ObservableObject
    {
        public int Id { get; set; }
        public string Sid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }

		private string description;
		public string Description
		{
			get { return this.description; }
			set
			{
				this.description = value;
				this.OnPropertyChanged();
			}
		}

		private bool isCompleted;
		public bool IsCompleted
		{
			get { return this.isCompleted; }
			set
			{
				this.isCompleted = value;
				this.OnPropertyChanged();
			}
		}

		private DateTime? dateDue;
		public DateTime? DateDue
		{
			get { return this.dateDue; }
			set
			{
				this.dateDue = value;
				this.OnPropertyChanged();
			}
		}
	}
}
