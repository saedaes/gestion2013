// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Gestion2013iOS
{
	[Register ("MainView")]
	partial class MainView
	{
		[Outlet]
		MonoTouch.UIKit.UITextField cmpUsuario { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField cmpContrasena { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnIngresar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cmpUsuario != null) {
				cmpUsuario.Dispose ();
				cmpUsuario = null;
			}

			if (cmpContrasena != null) {
				cmpContrasena.Dispose ();
				cmpContrasena = null;
			}

			if (btnIngresar != null) {
				btnIngresar.Dispose ();
				btnIngresar = null;
			}
		}
	}
}
