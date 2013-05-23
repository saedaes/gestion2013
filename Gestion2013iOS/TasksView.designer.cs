// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Gestion2013iOS
{
	[Register ("TasksView")]
	partial class TasksView
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tblTasks { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView headerView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnNuevo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnMap { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblTasks != null) {
				tblTasks.Dispose ();
				tblTasks = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (btnNuevo != null) {
				btnNuevo.Dispose ();
				btnNuevo = null;
			}

			if (btnMap != null) {
				btnMap.Dispose ();
				btnMap = null;
			}
		}
	}
}
