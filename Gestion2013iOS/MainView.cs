
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class MainView : UIViewController
	{
		//Declaramos el usuario y la contraseña como estaticos para poder acceder a ellos. 
		public static string user;
		public static string password;

		TasksView tasksView;

		public MainView () : base ("MainView", null)
		{
			this.Title = "Gestión 2013";
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
			this.cmpContrasena.SecureTextEntry = true;

			try{
				this.btnIngresar.TouchUpInside += (sender, e) => {
					user = this.cmpUsuario.Text;
					password = this.cmpContrasena.Text;

					if(user.Equals("")&& password.Equals("")){
						UIAlertView alert = new UIAlertView(){
							Title = "ERROR", Message = "Ingresa correctamente los datos"
						};
						alert.AddButton("Aceptar");
						alert.Show();
					}else{
						LoginService loginService = new LoginService();
						String respuesta = loginService.SetUserAndPassword(user, password);
						if(respuesta.Equals("0")){	
							UIAlertView alert = new UIAlertView(){
								Title = "ERROR", Message = "Datos incorrectos"
							};
							alert.AddButton("Aceptar");
							alert.Show();
						}else if(respuesta.Equals("1")){
							tasksView = new TasksView();
							this.NavigationController.PushViewController(tasksView, true);
						}
						else{
							UIAlertView alert = new UIAlertView(){
								Title = "ERROR", Message = "Error del Servidor, intentelo de nuevo"
							};
							alert.AddButton("Aceptar");
							alert.Show();
						}
					}
				};
			}catch(System.Net.WebException){
				UIAlertView alert = new UIAlertView(){
					Title = "ERROR", Message = "No se pudo conectar al servidor, verifique su conexión a internet"
				};
				alert.AddButton("Aceptar");
				alert.Show();
			}

			this.cmpContrasena.ShouldReturn += (textField) => { textField.ResignFirstResponder(); return true; };
			this.cmpUsuario.ShouldReturn += (textField) => { textField.ResignFirstResponder(); return true; };
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			this.cmpContrasena.Text = "";
			this.cmpUsuario.Text= "";
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			user = "";
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			foreach (var item in this) {
				var tf = item as UITextField;
				if (tf != null && tf.IsFirstResponder) {
					tf.ResignFirstResponder ();
				}
			}
			base.TouchesEnded (touches, evt);
		}
	}
}
