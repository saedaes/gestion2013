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
				/*taskDetailView = new TaskDetailView ();
				taskDetailView.setTask (tableItems[indexPath.Row]);
				controller.NavigationController.PushViewController (taskDetailView, true);*/
			}	

			/*public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
			{

			}*/
		}
	}
}

