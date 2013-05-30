// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Gestion2013iOS
{
	[Register ("DetailTaskView")]
	partial class DetailTaskView
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tblDetalles { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView headerView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblTitulo { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblDetalles != null) {
				tblDetalles.Dispose ();
				tblDetalles = null;
			}

			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}

			if (lblTitulo != null) {
				lblTitulo.Dispose ();
				lblTitulo = null;
			}
		}
	}
}
