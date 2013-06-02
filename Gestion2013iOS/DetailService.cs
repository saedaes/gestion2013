using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Gestion2013iOS
{
	public	class DetailService
	{
		public string Descripcion {get;set;}
		public string fechaAlta {get;set;}
		public string fechaEdicion {get;set;}
		public string idTarea {get;set;}
		public string idTareaDetalle {get;set;}
		public string usuarioAlta {get;set;}
		public string usuarioEdicion {get;set;}

		string DetailTasksURL =  "http://198.58.107.204:5810/detalle_tarea.json?idTarea="+ TasksView.tareaId;

		public DetailService()
		{
		}

		public List<DetailService> All()
		{
			return GetDetails();
		}		

		public List <DetailService> GetDetails()
		{

			WebClient client= new WebClient();
			Stream stream= client.OpenRead(DetailTasksURL);
			StreamReader reader= new StreamReader(stream);		
			JArray detailsJSON = JArray.Parse(reader.ReadLine());
			List <DetailService> details = new List<DetailService>();

			foreach (JObject jobject in detailsJSON)
			{
				DetailService detail = DetailService.FromJObject(jobject);

				details.Add(detail);

			}

			return details;
		}

		internal static DetailService FromJObject(JObject jObject)
		{
			DetailService detail = new DetailService();
			detail.Descripcion = jObject["Descripcion"].ToString();
			detail.fechaAlta =jObject["fechaAlta"].ToString();
			detail.fechaEdicion = jObject["fechaEdicion"].ToString();
			detail.idTarea = jObject["idTarea"].ToString();
			detail.idTareaDetalle = jObject["idTareaDetalle"].ToString();
			detail.usuarioAlta = jObject["usuarioAlta"].ToString();
			detail.usuarioEdicion = jObject["usuarioEdicion"].ToString();

			return detail;
		}

		public override string ToString ()
		{
			if(Descripcion != null)
				return Descripcion;
			else
				return base.ToString();
		}
	}
}

