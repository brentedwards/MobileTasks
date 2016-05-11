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
	[Register ("DetailController")]
	partial class DetailController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch Completed { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIDatePicker DateDue { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Description { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch HasDateDue { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Completed != null) {
				Completed.Dispose ();
				Completed = null;
			}
			if (DateDue != null) {
				DateDue.Dispose ();
				DateDue = null;
			}
			if (Description != null) {
				Description.Dispose ();
				Description = null;
			}
			if (HasDateDue != null) {
				HasDateDue.Dispose ();
				HasDateDue = null;
			}
		}
	}
}
