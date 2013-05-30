using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class NewDetailTaskView : UIViewController
	{
		public NewDetailTaskView () : base ("NewDetailTaskView", null)
		{
			this.Title = "Nuevo detalle";
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			//Se establece un borde para el textarea de las observaciones
			this.cmpDescripcion.Layer.BorderWidth = 1.0f;
			this.cmpDescripcion.Layer.BorderColor = UIColor.Gray.CGColor;
			this.cmpDescripcion.Layer.ShadowColor = UIColor.Black.CGColor;
			this.cmpDescripcion.Layer.CornerRadius = 8;

			this.btnGuardar.TouchUpInside += (sender, e) => {
				UIAlertView alert = new UIAlertView(){
					Title = "Aviso" , Message = "¿Escorrecta la informacion?"
				};
				alert.AddButton ("SI");
				alert.AddButton ("NO");
				alert.Clicked += (s, o) => {
					if(o.ButtonIndex==0){
						NavigationController.PopViewControllerAnimated(true);
					}
				};
				alert.Show();
			};
		}
	}
}

