using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace Gestion2013iOS
{
	public partial class NewTaskView : UIViewController
	{
		ActionSheetPicker actionSheetPicker;
		ActionSheetDatePicker actionSheetDatePicker;
		ActionSheetDatePicker actionSheetDatePicker1;
		PickerDataModelResponsibles pickerDataModelResponsibles;
		PickerDataModel pickerDataModel;
		PickerDataModelPeople pickerDataModelPeople;
		PickerDataModelCategories pickerDataModelCategories;
		bool listo = false;
		CLLocationManager iPhoneLocationManager = null;
		PeopleService peopleService;
		CategoryService categoryService;
		NewTaskService newTaskService;
		PrioritiesService prioritiesService;
		ResponsibleService responsibleService;
		public NewTaskView () : base ("NewTaskView", null)
		{
			this.Title = "Nueva Tarea";
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Ocultamos el campo de observaciones, ya que por lo visto no se almacena en ningun lado, esperamos respuesta
			this.cmpObservaciones.Hidden = true;
			this.lblObservaciones.Hidden = true;

			//Ocultamos los labels donde se muestran las coordenadas del dispositivo
			this.lblLatitud.Hidden = true;
			this.lblLongitud.Hidden = true;

			//Declarar el Location Manager
			iPhoneLocationManager = new CLLocationManager ();
			iPhoneLocationManager.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;

			//Obtener la posicion del dispositivo
			//El metodo es diferente en iOS 6 se verifica la version del S.O. 
			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				iPhoneLocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
					UpdateLocation (e.Locations [e.Locations.Length - 1]);
				};
			}else{
				iPhoneLocationManager.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) => {
					UpdateLocation (e.NewLocation);
				};
			}

			iPhoneLocationManager.UpdatedHeading += (object sender, CLHeadingUpdatedEventArgs e) => {
			
			};


			//Actualizar la ubicacion 
			if (CLLocationManager.LocationServicesEnabled)
				iPhoneLocationManager.StartUpdatingLocation ();
			if (CLLocationManager.HeadingAvailable)
				iPhoneLocationManager.StartUpdatingHeading ();

			//Se esconde el booton para ir a la vista anterior
			this.NavigationItem.HidesBackButton = true;


			//se crea el boton para regresar a la vista anterior, verificando que la tarea haya sido dada de alta
			UIBarButtonItem regresar = new UIBarButtonItem();
			regresar.Style = UIBarButtonItemStyle.Plain;
			regresar.Target = this;
			regresar.Title = "Lista de tareas";
			regresar.Clicked += (sender, e) => {
				if(this.listo == false){
					UIAlertView alert = new UIAlertView(){
						Title = "¿Salir?" , Message = "Si sales se perderá el registro de la tarea"
					};
					alert.AddButton("Aceptar");
					alert.AddButton("Cancelar");
					alert.Clicked += (s, o) => {
						if(o.ButtonIndex==0){
							NavigationController.PopViewControllerAnimated(true);
						}
					};
					alert.Show();
				}else{
					NavigationController.PopViewControllerAnimated(true);
				}
			};

			//posionamiento del boton
			this.NavigationItem.LeftBarButtonItem = regresar;

			//Se establece un borde para el textarea de la descripcion
			this.cmpDescripcion.Layer.BorderWidth = 1.0f;
			this.cmpDescripcion.Layer.BorderColor = UIColor.Gray.CGColor;
			this.cmpDescripcion.Layer.ShadowColor = UIColor.Black.CGColor;
			this.cmpDescripcion.Layer.CornerRadius = 8;

			//Declaramos el datamodel para los responsables
			pickerDataModelResponsibles = new PickerDataModelResponsibles ();
			//Declaramos el datamodel para las categorias
			pickerDataModelCategories = new PickerDataModelCategories ();
			//Declaramos el datamodel para el picker de prioridades
			pickerDataModel = new PickerDataModel ();
			//Declaramos el datamodel para el picker de buisqueda de personas
			pickerDataModelPeople = new PickerDataModelPeople ();
			//Declaramos el actionsheet donde se mostrara el picker
			actionSheetPicker = new ActionSheetPicker(this.View);

			this.btnResponsable.TouchUpInside += (sender, e) => {
				try{
					responsibleService = new ResponsibleService();
					pickerDataModelResponsibles.Items = responsibleService.All();
					actionSheetPicker.Picker.Source = pickerDataModelResponsibles;
					actionSheetPicker.Show();
				}catch(System.Net.WebException){
					UIAlertView alert = new UIAlertView(){
						Title = "ERROR", Message = "No se pueden cargar los datos, verifique su conexión a internet"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				} catch(System.Exception){
					UIAlertView alert = new UIAlertView(){
						Title = "Lo sentimos", Message = "Ocurrio un problema de ejecucion, intentelo de nuevo"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				}
			};

			this.btnCategoria.TouchUpInside += (sender, e) => {
				try{
					categoryService = new CategoryService();
					pickerDataModelCategories.Items = categoryService.All();//llenamos el picker view con la respuesta del servicio de categorias
					actionSheetPicker.Picker.Source = pickerDataModelCategories;
					actionSheetPicker.Show();
				}catch(System.Net.WebException){
					UIAlertView alert = new UIAlertView(){
						Title = "ERROR", Message = "No se pueden cargar los datos, verifique su conexión a internet"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				} catch(System.Exception){
					UIAlertView alert = new UIAlertView(){
						Title = "Lo sentimos", Message = "Ocurrio un problema de ejecucion, intentelo de nuevo"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				}
			};

			this.btnPrioridad.TouchUpInside += (sender, e) => {
				try{
					prioritiesService = new PrioritiesService();
					pickerDataModel.Items = prioritiesService.All();//llenamos el pickerview con la lista de prioridades 
					actionSheetPicker.Picker.Source = pickerDataModel;
					actionSheetPicker.Show();
				} catch(System.Net.WebException){
					UIAlertView alert = new UIAlertView(){
						Title = "ERROR", Message = "No se pueden cargar los datos, verifique su conexión a internet"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				} catch(System.Exception){
					UIAlertView alert = new UIAlertView(){
						Title = "Lo sentimos", Message = "Ocurrio un problema de ejecucion, intentelo de nuevo"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				}
			};

			this.btnFechaCont.TouchUpInside += (sender, e) => {
				actionSheetDatePicker.Show();
			};

			this.btnFechaCompr.TouchUpInside += (sender, e) => {
				actionSheetDatePicker1.Show();
			};

			//Establecemos las propiedades del primer datepicker
			actionSheetDatePicker = new ActionSheetDatePicker (this.View);
			actionSheetDatePicker.Picker.Mode = UIDatePickerMode.Date;
			//actionSheetDatePicker.Picker.TimeZone = NSTimeZone.LocalTimeZone;
			//actionSheetDatePicker.Picker.MinimumDate = DateTime.Today.AddDays (-7);
			//actionSheetDatePicker.Picker.MaximumDate = DateTime.Today.AddDays (7);	

			//Establecemos las propiedades del segundo datepicker
			actionSheetDatePicker1 = new ActionSheetDatePicker (this.View);
			actionSheetDatePicker1.Picker.Mode = UIDatePickerMode.Date;
			//actionSheetDatePicker1.Picker.MinimumDate = DateTime.Today.AddDays (-7);
			//actionSheetDatePicker1.Picker.MaximumDate = DateTime.Today.AddDays (7);	

			actionSheetDatePicker.Picker.ValueChanged += (s, e) => {
				DateTime fecha1 = (s as UIDatePicker).Date;
				String fecha2 = String.Format("{0:yyyy-MM-dd}",fecha1);
				this.lblFechaCont.Text = fecha2;
			};

			actionSheetDatePicker1.Picker.ValueChanged += (s, e) => {
				DateTime fecha1 = (s as UIDatePicker).Date;
				String fecha2 = String.Format("{0:yyyy-MM-dd}",fecha1);
				this.lblFechaCompr.Text = fecha2;
			};

			String categoria="";
			pickerDataModelCategories.ValueChanged += (sender, e) => {
				categoria = pickerDataModelCategories.SelectedItem.idCategoria;
				this.lblCategoria.Text = pickerDataModelCategories.SelectedItem.ToString();
			};

			String prioridad = "";
			pickerDataModel.ValueChanged += (sender, e) => {
				prioridad = pickerDataModel.SelectedItem.idPrioridad;
				this.lblPrioridad.Text = pickerDataModel.SelectedItem.ToString();
			};

			String responsable = "";
			pickerDataModelResponsibles.ValueChanged += (sender, e) => {
				responsable = pickerDataModelResponsibles.SelectedItem.UserID;
				this.lblResponsable.Text = pickerDataModelResponsibles.SelectedItem.ToString();
			};

			String idPadron ="";
			pickerDataModelPeople.ValueChanged += (sender, e) => {
				idPadron = pickerDataModelPeople.SelectedItem.idPadron;
				this.cmpSolicitante.Text = pickerDataModelPeople.SelectedItem.ToString();
			};

			this.btnBuscar.TouchUpInside += (sender, e) => {
				try{
					peopleService = new PeopleService();
					peopleService.FindPeople(this.cmpNombre.Text, this.cmpPaterno.Text, this.cmpMaterno.Text);

					pickerDataModelPeople.Items = peopleService.All();
					if(pickerDataModelPeople.Items.Count == 0){
						UIAlertView alert = new UIAlertView(){
							Title = "Persona no encontrada", Message = "No se encontraron resultados, intentelo de nuevo"
						};
						alert.AddButton("Aceptar");
						alert.Show();
					}else {
						actionSheetPicker = new ActionSheetPicker(this.View);
						actionSheetPicker.Picker.Source = pickerDataModelPeople;
						actionSheetPicker.Show();
					}
				} catch(System.Net.WebException){
					UIAlertView alert = new UIAlertView(){
						Title = "ERROR", Message = "No se pueden cargar los datos, verifique su conexión a internet"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				} catch(System.Exception){
					UIAlertView alert = new UIAlertView(){
						Title = "Lo sentimos", Message = "Ocurrio un problema de ejecucion, intentelo de nuevo"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				}
			};

			this.cmpSolicitante.Enabled = false;

			//Se crea el boton para enviar la informacion al servidor
			this.btnGuardar.TouchUpInside += (sender, e) => {
				try{
					newTaskService = new NewTaskService();
					String respuesta = newTaskService.SetData(cmpTitulo.Text, cmpDescripcion.Text,categoria,responsable,prioridad,lblFechaCont.Text,lblFechaCompr.Text,idPadron,MainView.user
					                       ,cmpTelCasa.Text,cmpTelCel.Text,cmpCorreo.Text,lblLatitud.Text,lblLongitud.Text);
					if (respuesta.Equals("0")){
						UIAlertView alert = new UIAlertView(){
							Title = "ERROR", Message = "Error del Servidor, intentelo de nuevo"
						};
						alert.AddButton("Aceptar");
						alert.Show();
					}else if(respuesta.Equals("1")){
						UIAlertView alert = new UIAlertView(){
							Title = "Correcto", Message = "La tarea ha sido guardada correctamente"
						};
						alert.AddButton("Aceptar");
						alert.Clicked += (s, o) => {
							if(o.ButtonIndex==0){
								NavigationController.PopViewControllerAnimated(true);
							}
						};
						alert.Show();
					}
				}catch(System.Net.WebException ex){
					Console.WriteLine(ex.ToString());
					UIAlertView alert = new UIAlertView(){
						Title = "ERROR", Message = "Error del Servidor, intentelo de nuevo, o verifique su conexión a internet"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				}
			};

			//Se establece un borde para el textarea de las observaciones
			this.cmpObservaciones.Layer.BorderWidth = 1.0f;
			this.cmpObservaciones.Layer.BorderColor = UIColor.Gray.CGColor;
			this.cmpObservaciones.Layer.ShadowColor = UIColor.Black.CGColor;
			this.cmpObservaciones.Layer.CornerRadius = 8;
		}

		public void UpdateLocation (CLLocation newLocation)
		{
			this.lblLatitud.Text = newLocation.Coordinate.Latitude.ToString ();
			this.lblLongitud.Text = newLocation.Coordinate.Longitude.ToString ();
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

		/** Clase para manejar el picker de responsables**/
		protected class PickerDataModelResponsibles : UIPickerViewModel 
		{
			public event EventHandler<EventArgs> ValueChanged;

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public List<ResponsibleService> Items
			{
				get { return items; }
				set { items = value; }
			}
			List<ResponsibleService> items = new List<ResponsibleService>();

			/// <summary>
			/// The current selected item
			/// </summary>
			public ResponsibleService SelectedItem
			{
				get { return items[selectedIndex]; }
			}
			protected int selectedIndex = 0;

			/// <summary>
			/// default constructor
			/// </summary>
			public PickerDataModelResponsibles ()
			{
			}

			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return items.Count;
			}

			/// <summary>
			/// called by the picker to get the text for a particular row in a particular 
			/// spinner item
			/// </summary>
			public override string GetTitle (UIPickerView picker, int row, int component){
				return items[row].ToString();
			}

			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, int row, int component)
			{
				selectedIndex = row;
				if (this.ValueChanged != null)
				{
					this.ValueChanged (this, new EventArgs ());
				}	
			}
		}

		/** Clase para manejar el picker de prioridades**/
		protected class PickerDataModel : UIPickerViewModel 
		{
			public event EventHandler<EventArgs> ValueChanged;

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public List<PrioritiesService> Items
			{
				get { return items; }
				set { items = value; }
			}
			List<PrioritiesService> items = new List<PrioritiesService>();

			/// <summary>
			/// The current selected item
			/// </summary>
			public PrioritiesService SelectedItem
			{
				get { return items[selectedIndex]; }
			}
			protected int selectedIndex = 0;

			/// <summary>
			/// default constructor
			/// </summary>
			public PickerDataModel ()
			{
			}

			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return items.Count;
			}

			/// <summary>
			/// called by the picker to get the text for a particular row in a particular 
			/// spinner item
			/// </summary>
			public override string GetTitle (UIPickerView picker, int row, int component){
				return items[row].ToString();
			}

			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, int row, int component)
			{
				selectedIndex = row;
				if (this.ValueChanged != null)
				{
					this.ValueChanged (this, new EventArgs ());
				}	
			}
		}

		/** Clase para manejar el picker de busqueda de personas **/
		protected class PickerDataModelPeople : UIPickerViewModel 
		{
			public event EventHandler<EventArgs> ValueChanged;

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public List<PeopleService> Items
			{
				get { return items; }
				set { items = value; }
			}
			List<PeopleService> items = new List<PeopleService>();

			/// <summary>
			/// The current selected item
			/// </summary>
			public PeopleService SelectedItem
			{
				get { return items[selectedIndex]; }
			}
			protected int selectedIndex = 0;

			/// <summary>
			/// default constructor
			/// </summary>
			public PickerDataModelPeople ()
			{
			}

			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return items.Count;
			}

			/// <summary>
			/// called by the picker to get the text for a particular row in a particular 
			/// spinner item
			/// </summary>
			public override string GetTitle (UIPickerView picker, int row, int component){
				return items[row].ToString();
			}

			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, int row, int component)
			{
				selectedIndex = row;
				if (this.ValueChanged != null)
				{
					this.ValueChanged (this, new EventArgs ());
				}	
			}
		}

		/** Clase para manejar el picker de categorias **/
		protected class PickerDataModelCategories : UIPickerViewModel 
		{
			public event EventHandler<EventArgs> ValueChanged;

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public List<CategoryService> Items
			{
				get { return items; }
				set { items = value; }
			}
			List<CategoryService> items = new List<CategoryService>();

			/// <summary>
			/// The current selected item
			/// </summary>
			public CategoryService SelectedItem
			{
				get { return items[selectedIndex]; }
			}
			protected int selectedIndex = 0;

			/// <summary>
			/// default constructor
			/// </summary>
			public PickerDataModelCategories ()
			{
			}

			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return items.Count;
			}

			/// <summary>
			/// called by the picker to get the text for a particular row in a particular 
			/// spinner item
			/// </summary>
			public override string GetTitle (UIPickerView picker, int row, int component){
				return items[row].ToString();
			}

			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, int row, int component)
			{
				selectedIndex = row;
				if (this.ValueChanged != null)
				{
					this.ValueChanged (this, new EventArgs ());
				}	
			}
		}
	}
}

