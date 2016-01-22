using MobileTasks.Windows.ViewModels;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileTasks.Windows.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Tasks : MobileTasksPage
	{
		public Tasks()
		{
			this.InitializeComponent();

			this.WireUpViewModel(new TasksViewModel());
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			this.Navigate("TaskDetail");
		}
	}
}
