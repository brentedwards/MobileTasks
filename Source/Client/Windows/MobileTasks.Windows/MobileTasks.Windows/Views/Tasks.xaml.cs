using MobileTasks.Windows.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileTasks.Windows.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Tasks : MobileTasksPage
	{
		private TasksViewModel ViewModel { get; set; }

		public Tasks()
		{
			this.InitializeComponent();
			this.ViewModel = new TasksViewModel();

			this.WireUpViewModel(this.ViewModel);
		}
	}
}
