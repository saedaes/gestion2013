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
		NewTaskView newTaskView;
		UIToolbar toolbar;
		TasksService ts;
		public static string tareaId;
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

			ts = new TasksService ();
			ts.setUser(MainView.user);

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

			List<TasksService> tableItems = ts.All ();
			this.tblTasks.Source = new TasksTableSource (tableItems,this);
			headerView.BackgroundColor = UIColor.Clear;
			tblTasks.TableHeaderView = headerView;
			Add(tblTasks);


			//Boton de nueva tarea
			btnNuevo.TouchUpInside += (sender, e) => {
				newTaskView = new NewTaskView();
				this.NavigationController.PushViewController(newTaskView, true);
			};

			//Boton de mapa
			btnMap.TouchUpInside += (sender, e) => {
				mapView = new MapViewController();
				this.NavigationController.PushViewController(mapView, true);
			};

			/* // creacion de la barra de herramientas
			float toolbarHeight = 50;
			toolbar = new UIToolbar (new RectangleF (0
			                                         , this.View.Frame.Height - this.NavigationController.NavigationBar.Frame.Height
			                                         , this.View.Frame.Width, toolbarHeight));
			toolbar.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
			toolbar.TintColor = UIColor.FromRGB(62,150,15);


			// boton Todos
			UIBarButtonItem btnTodos = new UIBarButtonItem("Todos",UIBarButtonItemStyle.Bordered, null);
			btnTodos.Clicked += (s, e) => { 
				//List<String> tableItemsTodos = new List<String>();
				//tableItemsTodos.Add("Tarea Finalizada 1");
				//tableItemsTodos.Add("Tarea Finalizada 2");
				//tableItemsTodos.Add("Tarea Finalizada 3");
				this.tblTasks.Source = new TasksTableSource(tableItems, this);
				this.tblTasks.ReloadData();
			};
			// fixed width
			//UIBarButtonItem fixedWidth = new UIBarButtonItem (UIBarButtonSystemItem.FixedSpace);
			//fixedWidth.Width = 35;

			// flexible width space
			UIBarButtonItem flexibleWidth0 = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);


			// boton Finalizados
			UIBarButtonItem btnFinalizados = new UIBarButtonItem("Finalizados",UIBarButtonItemStyle.Bordered, null);
			btnFinalizados.Clicked += (s, e) => { 
				List<String> tableItemsFinalizados = new List<String>();
				tableItemsFinalizados.Add("Tarea Finalizada 1");
				tableItemsFinalizados.Add("Tarea Finalizada 2");
				tableItemsFinalizados.Add("Tarea Finalizada 3");
				this.tblTasks.Source = new TasksTableSource(tableItemsFinalizados, this);
				this.tblTasks.ReloadData();
			};
			// fixed width
			//UIBarButtonItem fixedWidth = new UIBarButtonItem (UIBarButtonSystemItem.FixedSpace);
			//fixedWidth.Width = 35;

			// flexible width space
			UIBarButtonItem flexibleWidth1 = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);

			// boton En proceso
			UIBarButtonItem btnProceso = new UIBarButtonItem("En Proceso",UIBarButtonItemStyle.Bordered, null);
			btnProceso.Clicked += (s, e) => { 
				List<String> tableItemsProceso = new List<String>();
				tableItemsProceso.Add("Tarea en Proceso 1");
				tableItemsProceso.Add("Tarea en Proceso 2");
				tableItemsProceso.Add("Tarea en Proceso 3");
				this.tblTasks.Source = new TasksTableSource(tableItemsProceso, this);
				this.tblTasks.ReloadData();
			};

			// flexible width space
			UIBarButtonItem flexibleWidth2 = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);

			// boton Cancelados
			UIBarButtonItem btnCancelados = new UIBarButtonItem("Cancelados",UIBarButtonItemStyle.Bordered, null);
			btnCancelados.Clicked += (s, e) => { 
				List<String> tableItemsCancelados = new List<String>();
				tableItemsCancelados.Add("Tarea Cancelada 1");
				tableItemsCancelados.Add("Tarea Cancelada 2");
				tableItemsCancelados.Add("Tarea Cancelada 3");
				this.tblTasks.Source = new TasksTableSource(tableItemsCancelados, this);
				this.tblTasks.ReloadData();
			};

			// arreglo de botones para toolbar
			UIBarButtonItem[] items = new UIBarButtonItem[] { btnTodos,flexibleWidth0,
				btnFinalizados, flexibleWidth1, btnProceso, flexibleWidth2, btnCancelados};

			// agregar los botones a la toolbar
			toolbar.SetItems (items, false);			

			// agregar la vista a la pantalla
			this.View.AddSubview (toolbar);*/
		}

		//Clase para manejar la lista
		class TasksTableSource : UITableViewSource 
		{
			List<TasksService> tableItems;
			string cellIdentifier = "TableCell";
			TasksView controller;
			TaskDetailView taskDetailView;
			public TasksTableSource (List<TasksService> items,TasksView controller ) 
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
				

				cell.TextLabel.Text = tableItems[indexPath.Row].ToString();
				cell.TextLabel.Font = UIFont.SystemFontOfSize(15);
				cell.TextLabel.Lines = 2;
				
				cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
				return cell;
			}
			
			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				taskDetailView = new TaskDetailView ();
				taskDetailView.setTask (tableItems[indexPath.Row]);
				TasksView.tareaId = tableItems[indexPath.Row].idTarea;
				controller.NavigationController.PushViewController (taskDetailView, true);
			}	
			
			public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
			{
				UIAlertView alert = new UIAlertView(){
					Title = tableItems[indexPath.Row].ToString() , Message = "¿Que desea hacer?"
				};
				alert.AddButton ("Editar");
				alert.AddButton ("Borrar");
				alert.AddButton ("Cancelar");
				alert.Clicked += (s, o) => {
					if(o.ButtonIndex==0){
						UIAlertView alert1 = new UIAlertView(){
							Title = tableItems[indexPath.Row].ToString() , Message = "Prueba Edicion"
						};
						alert1.AddButton ("Aceptar");
					}else if(o.ButtonIndex == 1){
						UIAlertView alert2 = new UIAlertView(){
							Title = tableItems[indexPath.Row].ToString() , Message = "¿Estás seguro?"
						};
						alert2.AddButton ("Aceptar");
						alert2.AddButton ("Cancelar");
					}
				};
				alert.Show();
			}
		}
	}
}

