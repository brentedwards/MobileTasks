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
		UIActivityIndicatorView Busy { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch Completed { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIDatePicker Date { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Description { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch HasDueDate { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Busy != null) {
				Busy.Dispose ();
				Busy = null;
			}
			if (Completed != null) {
				Completed.Dispose ();
				Completed = null;
			}
			if (Date != null) {
				Date.Dispose ();
				Date = null;
			}
			if (Description != null) {
				Description.Dispose ();
				Description = null;
			}
			if (HasDueDate != null) {
				HasDueDate.Dispose ();
				HasDueDate = null;
			}
		}
	}
}
