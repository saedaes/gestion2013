using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace Gestion2013iOS
{
	public partial class TasksView : UIViewController
	{
		MapViewController mapView;
		public TasksView () : base ("TasksView", null)
		{
			this.Title = "Listado de tareas";
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

			//Escondemos el boton de regreso hacia la pagina de inicio
			this.NavigationItem.HidesBackButton = true;
			
			//Creamos el boton de cerrar sesion
			UIBarButtonItem cerrarSesion = new UIBarButtonItem();
			cerrarSesion.Style = UIBarButtonItemStyle.Plain;
			cerrarSesion.Target = this;
			cerrarSesion.Title = "Cerrar Sesión";
			cerrarSesion.Clicked += (sender, e) => {
				UIAlertView alert = new UIAlertView(){
					Title = "¿Salir?" , Message = "¿Quieres cerrar la sesión?"
				};
				alert.AddButton("Aceptar");
				alert.AddButton("Cancelar");
				alert.Clicked += (s, o) => {
					if(o.ButtonIndex==0){
						NavigationController.PopViewControllerAnimated(true);
					}
				};
				alert.Show();
			};
			
			//posionamiento del boton
			this.NavigationItem.LeftBarButtonItem = cerrarSesion;

			//string[] tableItems = new string[] {"Tarea1","Tarea2","Tarea3","Tarea4","Tarea5","Tarea5"};
			List<String> tableItems = new List<String>();
			tableItems.Add("Tarea 1");
			tableItems.Add("Tarea 2");
			tableItems.Add("Tarea 3");
			this.tblTasks.Source = new TasksTableSource(tableItems, this);

			headerView.BackgroundColor = UIColor.Clear;
			tblTasks.TableHeaderView = headerView;
			Add(tblTasks);


			btnMap.TouchUpInside += (sender, e) => {
				mapView = new MapViewController();
				this.NavigationController.PushViewController(mapView, true);
			};
		}

		class TasksTableSource : UITableViewSource 
		{
			List<String> tableItems;
			string cellIdentifier = "TableCell";
			TasksView controller;
			public TasksTableSource (List<String> items,TasksView controller ) 
			{
				tableItems = items;
				this.controller=controller;
			}
			
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return tableItems.Count;		    
			}
			
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				// if there are no cells to reuse, create a new one
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);
				

				cell.TextLabel.Text = tableItems[indexPath.Row];
				cell.TextLabel.Font = UIFont.SystemFontOfSize(15);
				cell.TextLabel.Lines = 2;
				
				cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
				return cell;
			}
			
			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{

			}	
			
			public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
			{

			}
		}
	}
}

