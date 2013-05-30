// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Gestion2013iOS
{
	[Register ("NewDetailTaskView")]
	partial class NewDetailTaskView
	{
		[Outlet]
		MonoTouch.UIKit.UITextView cmpDescripcion { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnGuardar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cmpDescripcion != null) {
				cmpDescripcion.Dispose ();
				cmpDescripcion = null;
			}

			if (btnGuardar != null) {
				btnGuardar.Dispose ();
				btnGuardar = null;
			}
		}
	}
}
