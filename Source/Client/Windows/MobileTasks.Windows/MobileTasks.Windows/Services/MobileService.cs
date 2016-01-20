using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace MobileTasks.Windows.Services
{
	public partial class MobileService
    {
		private static MobileService instance;
		public static MobileService Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MobileService();
				}

				return instance;
			}
		}

		public MobileServiceClient Client { get; set; }
		public MobileServiceUser User { get; set; }

		public async Task LoginAsync(MobileServiceAuthenticationProvider provider)
		{
			this.User = await this.Client.LoginAsync(provider);
		}
	}
}
