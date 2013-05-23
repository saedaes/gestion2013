// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Gestion2013iOS
{
	[Register ("NewTaskView")]
	partial class NewTaskView
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnPrioridad { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnFechaCont { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnFechaCompr { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblPrioridad { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblFechaCont { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblFechaCompr { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnPrioridad != null) {
				btnPrioridad.Dispose ();
				btnPrioridad = null;
			}

			if (btnFechaCont != null) {
				btnFechaCont.Dispose ();
				btnFechaCont = null;
			}

			if (btnFechaCompr != null) {
				btnFechaCompr.Dispose ();
				btnFechaCompr = null;
			}

			if (lblPrioridad != null) {
				lblPrioridad.Dispose ();
				lblPrioridad = null;
			}

			if (lblFechaCont != null) {
				lblFechaCont.Dispose ();
				lblFechaCont = null;
			}

			if (lblFechaCompr != null) {
				lblFechaCompr.Dispose ();
				lblFechaCompr = null;
			}
		}
	}
}
