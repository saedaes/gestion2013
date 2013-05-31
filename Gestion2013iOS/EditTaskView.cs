using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class EditTaskView : UIViewController
	{
		TasksService task;
		public EditTaskView () : base ("EditTaskView", null)
		{
			this.Title = "Editar tarea";
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
			
			this.cmpTitulo.Text = this.task.Titulo;
			this.cmpDescripcion.Text = this.task.Descripcion;
			this.cmpSolicitante.Text = this.task.nombreSolicitante;
			this.cmpTelCasa.Text = this.task.telCasaSolicitante;
			this.cmpTelCel.Text = this.task.telCelularSolicitante;
			this.cmpCorreo.Text = this.task.correoSolicitante;
			this.lblCategoria.Text = this.task.idCategoria;
			this.lblResponsable.Text = this.task.idResponsable;
			this.lblPrioridad.Text = this.task.idPrioridad;
			this.lblFechaCont.Text = this.task.fechaContacto;
			this.lblFechaCompr.Text = this.task.fechaCompromiso;

			this.cmpDescripcion.Layer.BorderWidth = 1.0f;
			this.cmpDescripcion.Layer.BorderColor = UIColor.Gray.CGColor;
			this.cmpDescripcion.Layer.ShadowColor = UIColor.Black.CGColor;
			this.cmpDescripcion.Layer.CornerRadius = 8;
		}
	}
}

