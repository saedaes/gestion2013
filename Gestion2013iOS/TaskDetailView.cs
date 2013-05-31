using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class TaskDetailView : UIViewController
	{
		TasksService task;
		DetailTaskView detailTaskView;
		//DetailService ds;
		NewDetailTaskView newDetailTaskView;
		ChangeStatusService changeStatusService;
		public static string tareaId;
		public TaskDetailView () : base ("TaskDetailView", null)
		{
			this.Title = "Descripcion de tareas";
		}

		public void setTask(TasksService task1){
			this.task = task1;
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

			this.lblTitulo.Text = this.task.Titulo;
			this.lblEncargado.Text = this.task.idResponsable;
			this.lblDescripcion.Text = this.task.Descripcion;
			this.lblNombreSolic.Text = this.task.nombreSolicitante;
			this.lblDireccionSolic.Text = this.task.direccionSolicitante;
			this.lblColoniaSolic.Text = this.task.coloniaSolicitante;
			this.lblTelefonoSolic.Text = this.task.telCasaSolicitante;
			this.lblCelSolic.Text = this.task.telCelularSolicitante;
			this.lblCorreoSolic.Text = this.task.correoSolicitante;
			//this.lblSeccionSolic.Text = this.task.idSeccion;
			this.lblClaveSolic.Text = this.task.CveElector;
			this.lblCategoria.Text = this.task.idCategoria;
			this.lblPrioridad.Text = this.task.idPrioridad;
			this.lblEstatus.Text = this.task.idEstatus;
			this.lblFecAsignacion.Text = this.task.fechaAsignacion;
			this.lblFecCompromiso.Text = this.task.fechaCompromiso;
			this.lblFecTermino.Text = this.task.fechaTermino;
			this.lblFecContacto.Text = this.task.fechaContacto;

			tareaId = task.idTarea;

			this.btnVerDetalle.TouchUpInside += (sender, e) => {
				detailTaskView = new DetailTaskView();
				this.NavigationController.PushViewController(detailTaskView, true);
			};

			this.btnAgregarDetalle.TouchUpInside += (sender, e) => {
				newDetailTaskView = new NewDetailTaskView();
				this.NavigationController.PushViewController(newDetailTaskView, true);
			};

			this.btnFinalizar.TouchUpInside += (sender, e) => {
				UIAlertView alert = new UIAlertView(){
					Title = "¿Esta seguro?", Message = "¿Esta seguro de finalizar la tarea?"
				};
				alert.AddButton("Aceptar");
				alert.AddButton("Cancelar");
				alert.Show();
				alert.Clicked += (s, o) => {
					try{
						if(o.ButtonIndex==0){
							changeStatusService = new ChangeStatusService();
							String respuesta = changeStatusService.SetTask(task.idTarea);
							if(respuesta.Equals("1")){
								SuccesConfirmation();
							} else if(respuesta.Equals("0")){
								ErrorConfirmation();
							}
						
						}
					}catch(System.Net.WebException){
						ServerError();
					}
				};
			};
		}

		public void SuccesConfirmation(){
			UIAlertView alert = new UIAlertView(){
				Title = "Correcto", Message = "Tarea Finalizada Correctamente"
			};
			alert.AddButton("Aceptar");
			alert.Show();
		}

		public void ErrorConfirmation(){
			UIAlertView alert = new UIAlertView(){
				Title = "Error", Message = "La tarea no pudo ser finalizada, intentelo de nuevo"
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
