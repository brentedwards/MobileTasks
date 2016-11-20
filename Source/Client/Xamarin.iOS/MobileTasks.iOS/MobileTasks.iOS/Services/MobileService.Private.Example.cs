using Microsoft.WindowsAzure.MobileServices;

namespace MobileTasks.iOS.Services
{
	public partial class MobileService
	{
		public MobileService()
		{
			this.Client = new MobileServiceClient("https://myapp.azurewebsites.net");
		}
	}
}
