using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class NewDetailTaskView : UIViewController
	{
		NewDetailService newDetailService;
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
						try{
							newDetailService = new NewDetailService();
							String respuesta = newDetailService.SetData(TaskDetailView.tareaId, this.cmpDescripcion.Text, MainView.user);
							if(respuesta.Equals("1")){
								SuccesConfirmation();
							}else if(respuesta.Equals("0")){
								ErrorConfirmation();
							}
						}catch(System.Net.WebException){
							ServerError();
						}
					}
				};
				alert.Show();
			};
		}

		public void SuccesConfirmation(){
			UIAlertView alert = new UIAlertView(){
				Title = "Correcto", Message = "Detalle guardado correctamente"
			};
			alert.AddButton("Aceptar");
			alert.Show();
			alert.Clicked += (s, o) => {
				if (o.ButtonIndex == 0) {
					NavigationController.PopViewControllerAnimated(true);
				}
			};
		}

		public void ErrorConfirmation(){
			UIAlertView alert = new UIAlertView(){
				Title = "Error", Message = "El detalle no pudo guardarse, intentelo de nuevo"
			};
			alert.AddButton("Aceptar");
			alert.Show();
		}

		public void ServerError(){
			UIAlertView alert = new UIAlertView(){
				Title = "Error", Message = "Error de conexión, no se pudo conectar con el servidor, intentelo de nuevo"
			};
			alert.AddButton("Aceptar");
			alert.Show();
		}
	}
}

