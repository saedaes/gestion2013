using System;
using System.Net;
using System.IO;
using System.Text;

namespace Gestion2013iOS
{
	public class NewDetailService
	{
		public NewDetailService ()
		{
		}

		public String SetData(String idTarea,String descripcion,String usuario){
			string loginURL = "http://148.229.75.81:3000/detalle_nuevo.json?tarea_id="+ idTarea+"&descrip="+descripcion+"&user="+usuario;
			WebRequest request = WebRequest.Create(loginURL);
			request.Method = "POST";

			string postData = "Esta es la peticion al servicio de borrado de tarea desde la app movil";
			byte[] byteArray = Encoding.UTF8.GetBytes (postData);
			// Set the ContentType property of the WebRequest.
			request.ContentType = "application/x-www-form-urlencoded";
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = byteArray.Length;
			// Get the request stream.
			Stream dataStream = request.GetRequestStream ();
			// Write the data to the request stream.
			dataStream.Write (byteArray, 0, byteArray.Length);
			// Close the Stream object.
			dataStream.Close ();
			// Get the response.
			WebResponse response = request.GetResponse ();
			// Display the status.
			//Console.WriteLine (((HttpWebResponse)response).StatusDescription);
			// Get the stream containing content returned by the server.
			dataStream = response.GetResponseStream ();
			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader (dataStream);
			// Read the content.
			string responseFromServer = reader.ReadToEnd ();
			// Display the content.
			Console.WriteLine (responseFromServer);
			// Clean up the streams.

			return responseFromServer;
			reader.Close ();
			dataStream.Close ();
			response.Close ();
		}
	}
}
