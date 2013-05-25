using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class NewTaskView : UIViewController
	{
		ActionSheetPicker actionSheetPicker;
		ActionSheetDatePicker actionSheetDatePicker;
		ActionSheetDatePicker actionSheetDatePicker1;
		PickerDataModel pickerDataModel;
		bool listo = false;

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

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

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

			
			this.btnPrioridad.TouchUpInside += (sender, e) => {
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
				this.lblFechaCont.Text = fecha3;
			};

			//Opciones para la lista de prioridades
			List<String> opciones1 = new List<String>();
			opciones1.Add ("Alta");
			opciones1.Add ("Normal");
			opciones1.Add ("Baja");

			pickerDataModel = new PickerDataModel ();
			pickerDataModel.Items = opciones1;

			//Propiedades para el pickerView
			actionSheetPicker = new ActionSheetPicker(this.View);
			actionSheetPicker.Title = "Listado de Estados";
			actionSheetPicker.Picker.Source = pickerDataModel;

			pickerDataModel.ValueChanged += (sender, e) => {
				this.lblPrioridad.Text = pickerDataModel.SelectedItem.ToString();
			};
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

		/** Clase para manejar el picker **/
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
	}
}

