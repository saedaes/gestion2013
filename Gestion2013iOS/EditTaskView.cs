using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Gestion2013iOS
{
	public partial class EditTaskView : UIViewController
	{
		TasksService task;
		ActionSheetPicker actionSheetPicker;
		ActionSheetDatePicker actionSheetDatePicker;
		ActionSheetDatePicker actionSheetDatePicker1;
		PickerDataModel pickerDataModel;
		PickerDataModelPeople pickerDataModelPeople;
		PickerDataModelCategories pickerDataModelCategories;
		PickerDataModelResponsibles pickerDataModelResponsibles;
		bool listo = false;
		PeopleService peopleService;
		CategoryService categoryService;
		PrioritiesService prioritiesService;
		ResponsibleService responsibleService;
		EditTaskService editTaskService;
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

			//**Colocamos los parametros con los cuales ya cuenta la tarea
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
			//******************************************

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
						Title = "¿Salir?" , Message = "Si sales se perderá la edicion de la tarea"
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
				responsibleService = new ResponsibleService();
				pickerDataModelResponsibles.Items = responsibleService.All();
				actionSheetPicker.Picker.Source = pickerDataModelResponsibles;
				actionSheetPicker.Show();
			};

			this.btnCategoria.TouchUpInside += (sender, e) => {
				categoryService = new CategoryService();
				pickerDataModelCategories.Items = categoryService.All();//llenamos el picker view con la respuesta del servicio de categorias
				actionSheetPicker.Picker.Source = pickerDataModelCategories;
				actionSheetPicker.Show();
			};

			this.btnPrioridad.TouchUpInside += (sender, e) => {
				prioritiesService = new PrioritiesService();
				pickerDataModel.Items = prioritiesService.All();//llenamos el pickerview con la lista de prioridades 
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
				String fecha3 = String.Format("{0:yyyy-MM-dd}",fecha2);
				this.lblFechaCont.Text = fecha3;
			};

			actionSheetDatePicker1.Picker.ValueChanged += (s, e) => {
				DateTime fecha1 = (s as UIDatePicker).Date;
				DateTime fecha2 =fecha1.AddDays(-1);
				String fecha3 = String.Format("{0:yyyy-MM-dd}",fecha2);
				this.lblFechaCompr.Text = fecha3;
			};

			String categoria=task.idCategoria;
			pickerDataModelCategories.ValueChanged += (sender, e) => {
				categoria = pickerDataModelCategories.SelectedItem.idCategoria;
				this.lblCategoria.Text = pickerDataModelCategories.SelectedItem.ToString();
			};

			String prioridad = task.idPrioridad;
			pickerDataModel.ValueChanged += (sender, e) => {
				prioridad = pickerDataModel.SelectedItem.idPrioridad;
				this.lblPrioridad.Text = pickerDataModel.SelectedItem.ToString();
			};

			String responsable = task.idResponsable;
			pickerDataModelResponsibles.ValueChanged += (sender, e) => {
				responsable = pickerDataModelResponsibles.SelectedItem.UserID;
				this.lblResponsable.Text = pickerDataModelResponsibles.SelectedItem.ToString();
			};

			String idPadron =task.idSolicitante;
			pickerDataModelPeople.ValueChanged += (sender, e) => {
				idPadron = pickerDataModelPeople.SelectedItem.idPadron;
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

			//Se crea el boton para enviar la informacion al servidor
			this.btnGuardar.TouchUpInside += (sender, e) => {
				try{
					editTaskService = new EditTaskService();
					String respuesta = editTaskService.SetData(cmpTitulo.Text,task.idTarea,cmpDescripcion.Text,categoria,responsable,prioridad,lblFechaCont.Text,lblFechaCompr.Text,idPadron,MainView.user
					                                          ,cmpTelCasa.Text,cmpTelCel.Text,cmpCorreo.Text);
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
				}catch(System.Net.WebException){
					UIAlertView alert = new UIAlertView(){
						Title = "ERROR", Message = "Error del Servidor, intentelo de nuevo"
					};
					alert.AddButton("Aceptar");
					alert.Show();
				}
			};

			//Se le da un borde al campo de descripcion
			this.cmpDescripcion.Layer.BorderWidth = 1.0f;
			this.cmpDescripcion.Layer.BorderColor = UIColor.Gray.CGColor;
			this.cmpDescripcion.Layer.ShadowColor = UIColor.Black.CGColor;
			this.cmpDescripcion.Layer.CornerRadius = 8;


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

