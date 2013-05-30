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
		DetailService ds;
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
			this.lblEncargado.Text = this.task.Encargado;
			this.lblDescripcion.Text = this.task.Descripcion;
			this.lblNombreSolic.Text = this.task.nombreSolicitante;
			this.lblDireccionSolic.Text = this.task.direccionSolicitante;
			this.lblColoniaSolic.Text = this.task.coloniaSolicitante;
			this.lblTelefonoSolic.Text = this.task.telCasaSolicitante;
			this.lblCelSolic.Text = this.task.telCelularSolicitante;
			this.lblCorreoSolic.Text = this.task.correoSolicitante;
			this.lblSeccionSolic.Text = this.task.idSeccion;
			this.lblClaveSolic.Text = this.task.idSolicitante;
			this.lblCategoria.Text = this.task.idCategoria;
			this.lblPrioridad.Text = this.task.idPrioridad;
			this.lblEstatus.Text = this.task.idEstatus;
			this.lblFecAsignacion.Text = this.task.fechaAsignacion;
			this.lblFecCompromiso.Text = this.task.fechaCompromiso;
			this.lblFecTermino.Text = this.task.fechaTermino;
			this.lblFecContacto.Text = this.task.fechaContacto;

			this.btnVerDetalle.TouchUpInside += (sender, e) => {
				detailTaskView = new DetailTaskView();
				detailTaskView.setTitulo(this.task.Titulo);
				this.NavigationController.PushViewController(detailTaskView, true);
			};
		}
	}
}

