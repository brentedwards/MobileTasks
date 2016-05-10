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
	[Register ("TaskCell")]
	partial class TaskCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DateDue { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Description { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Status { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (DateDue != null) {
				DateDue.Dispose ();
				DateDue = null;
			}
			if (Description != null) {
				Description.Dispose ();
				Description = null;
			}
			if (Status != null) {
				Status.Dispose ();
				Status = null;
			}
		}
	}
}
