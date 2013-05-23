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
		PickerDataModel pickerDataModel;
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
			
			this.btnPrioridad.TouchUpInside += (sender, e) => {
				actionSheetPicker.Show();
			};

			//Opciones para la lista de prioridades
			List<String> opciones1 = new List<String>();
			opciones1.Add ("Alta");
			opciones1.Add ("Normal");
			opciones1.Add ("Baja");

			pickerDataModel = new PickerDataModel ();
			pickerDataModel.Items = opciones1;

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

