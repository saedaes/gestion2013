using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace Gestion2013iOS
{
	public partial class DetailTaskView : UIViewController
	{
		DetailService detailService;
		public DetailTaskView () : base ("DetailTaskView", null)
		{
			this.Title = "Detalles de tarea";
		}

		public void setTitulo(String titulo){
			//this.lblTitulo.Text = titulo;
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

			detailService = new DetailService ();
			List<DetailService> tableItems = detailService.All ();
			this.tblDetalles.Source = new DetailTableSource (tableItems,this);
			Add(tblDetalles);
		}

		//Clase para manejar la lista
		class DetailTableSource : UITableViewSource 
		{
			List<DetailService> tableItems;
			string cellIdentifier = "TableCell";
			DetailTaskView controller;
			TaskDetailView taskDetailView;
			DeleteDetailService deleteDetailService;
			DetailService ds = new DetailService ();
			public DetailTableSource (List<DetailService> items, DetailTaskView controller ) 
			{
				tableItems = items;
				this.controller=controller;
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return tableItems.Count;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return 3;	    
			}

			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return 75f;
			}

			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				// if there are no cells to reuse, create a new one
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);

				DetailService detalle = tableItems [indexPath.Section];
				if(indexPath.Row == 0){
					cell.TextLabel.Text = "Descripcion:";
					cell.TextLabel.Font = UIFont.SystemFontOfSize(16);
					cell.DetailTextLabel.Text= detalle.Descripcion;
					cell.DetailTextLabel.Lines = 3;
				}
				if (indexPath.Row == 1) {
					cell.TextLabel.Text = "Fecha de Alta:";
					cell.TextLabel.Font = UIFont.SystemFontOfSize(16);
					cell.DetailTextLabel.Text= detalle.fechaAlta.Substring(0,10); 
					cell.DetailTextLabel.Lines = 1;
				}
				if (indexPath.Row == 2) {
					cell.TextLabel.Text = "Usuario:";
					cell.TextLabel.Font = UIFont.SystemFontOfSize(16);
					cell.DetailTextLabel.Text= detalle.usuarioAlta;
					cell.DetailTextLabel.Lines = 1;
				}
				//cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
				return cell;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				UIAlertView alert = new UIAlertView(){
					Title = "¿BORRAR?", Message = "¿Desea borrar el detalle?"
				};
				alert.AddButton("Aceptar");
				alert.AddButton("Cancelar");
				alert.Clicked += (s, o) => {
					if (o.ButtonIndex == 0) {
						ds = tableItems [indexPath.Section];
						Confirmation();
					}
				};
				alert.Show();
			}	

			public void Confirmation(){
				UIAlertView alert = new UIAlertView(){
					Title = "¿ESTA SEGURO?", Message = "¿Esta seguro de borrar el detalle?"
				};
				alert.AddButton("Aceptar");
				alert.AddButton("Cancelar");
				alert.Clicked += (s, o) => {
					try{
					if(o.ButtonIndex==0){
						deleteDetailService = new DeleteDetailService();
						String respuesta = deleteDetailService.SetDetail(ds.idTareaDetalle);
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
				alert.Show();
			}

			public void SuccesConfirmation(){
				UIAlertView alert = new UIAlertView(){
					Title = "Correcto", Message = "Detalle Borrado Correctamente"
				};
				alert.AddButton("Aceptar");
				alert.Clicked += (s, o) => {
					if (o.ButtonIndex == 0) {
						controller.NavigationController.PopViewControllerAnimated(true);
					}
				};
				alert.Show();

			}

			public void ErrorConfirmation(){
				UIAlertView alert = new UIAlertView(){
					Title = "Error", Message = "El detalle no pudo ser borrado, intentelo de nuevo"
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
}

