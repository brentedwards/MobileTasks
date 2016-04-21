// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MobileTasks.iOS
{
	[Register ("LoginController")]
	partial class LoginController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Facebook { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Google { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Microsoft { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Twitter { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Facebook != null) {
				Facebook.Dispose ();
				Facebook = null;
			}
			if (Google != null) {
				Google.Dispose ();
				Google = null;
			}
			if (Microsoft != null) {
				Microsoft.Dispose ();
				Microsoft = null;
			}
			if (Twitter != null) {
				Twitter.Dispose ();
				Twitter = null;
			}
		}
	}
}
