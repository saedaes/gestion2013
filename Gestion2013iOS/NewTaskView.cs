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
		PickerDataModel pickerDataModel;
		PickerDataModelPeople pickerDataModelPeople;
		PickerDataModelCategories pickerDataModelCategories;
		bool listo = false;
		CLLocationManager iPhoneLocationManager = null;
		PeopleService peopleService;
		CategoryService categoryService;
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


			//Opciones para la lista de prioridades
			List<String> opciones1 = new List<String>();
			opciones1.Add ("Alta");
			opciones1.Add ("Normal");
			opciones1.Add ("Baja");

			//Declaramos el datamodel para las categorias
			pickerDataModelCategories = new PickerDataModelCategories ();
			//Declaramos el datamodel para el picker de prioridades
			pickerDataModel = new PickerDataModel ();
			//Declaramos el datamodel para el picker de buisqueda de personas
			pickerDataModelPeople = new PickerDataModelPeople ();
			//Declaramos el actionsheet donde se mostrara el picker
			actionSheetPicker = new ActionSheetPicker(this.View);

			this.btnCategoria.TouchUpInside += (sender, e) => {
				categoryService = new CategoryService();
				pickerDataModelCategories.Items = categoryService.All();//llenamos el picker view con la respuesta del servicio de categorias
				actionSheetPicker.Picker.Source = pickerDataModelCategories;
				actionSheetPicker.Show();
			};

			this.btnPrioridad.TouchUpInside += (sender, e) => {
				pickerDataModel.Items = opciones1;//llenamos el pickerview con la lista de prioridades declarada mas arriba
				actionSheetPicker.Picker.Source = pickerDataModel;
				actionSheetPicker.Show();
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
				DateTime fecha2 =fecha1.AddDays(-1);
				String fecha3 = String.Format("{0:dd/MM/yyyy}",fecha2);
				this.lblFechaCont.Text = fecha3;
			};

			actionSheetDatePicker1.Picker.ValueChanged += (s, e) => {
				DateTime fecha1 = (s as UIDatePicker).Date;
				DateTime fecha2 =fecha1.AddDays(-1);
				String fecha3 = String.Format("{0:dd/MM/yyyy}",fecha2);
				this.lblFechaCompr.Text = fecha3;
			};

			pickerDataModelCategories.ValueChanged += (sender, e) => {
				this.lblCategoria.Text = pickerDataModelCategories.SelectedItem.ToString();
			};

			pickerDataModel.ValueChanged += (sender, e) => {
				this.lblPrioridad.Text = pickerDataModel.SelectedItem.ToString();
			};

			pickerDataModelPeople.ValueChanged += (sender, e) => {
				this.cmpSolicitante.Text = pickerDataModelPeople.SelectedItem.ToString();
			};

			this.btnBuscar.TouchUpInside += (sender, e) => {
				peopleService = new PeopleService();
				peopleService.FindPeople(this.cmpNombre.Text, this.cmpPaterno.Text, this.cmpMaterno.Text);

				pickerDataModelPeople.Items = peopleService.All();
				actionSheetPicker = new ActionSheetPicker(this.View);
				actionSheetPicker.Picker.Source = pickerDataModelPeople;
				actionSheetPicker.Show();
			};

			this.cmpSolicitante.Enabled = false;

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

		/** Clase para manejar el picker de prioridades**/
		protected class PickerDataModel : UIPickerViewModel 
		{
			public event EventHandler<EventArgs> ValueChanged;

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public List<String> Items
			{
				get { return items; }
				set { items = value; }
			}
			List<String> items = new List<String>();

			/// <summary>
			/// The current selected item
			/// </summary>
			public String SelectedItem
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

