using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Gestion2013iOS
{
	public	class PrioritiesService
	{
		public string Prioridad {get;set;}
		public string idPrioridad {get;set;}

		string PriorityURL =  "http://198.58.107.204:5810/prioridades.json";

		public PrioritiesService()
		{
		}

		public List<PrioritiesService> All()
		{
			return GetPriorities();
		}		

		public List <PrioritiesService> GetPriorities()
		{

			WebClient client = new WebClient();
			Stream stream = client.OpenRead(PriorityURL);
			StreamReader reader = new StreamReader(stream);		
			JArray prioritiesJSON = JArray.Parse(reader.ReadLine());
			List <PrioritiesService> priorities = new List<PrioritiesService>();

			foreach (JObject jobject in prioritiesJSON)
			{
				PrioritiesService priority = PrioritiesService.FromJObject(jobject);
				priorities.Add(priority);
			}

			return priorities;
		}

		internal static PrioritiesService FromJObject(JObject jObject)
		{
			PrioritiesService priority = new PrioritiesService();
			priority.Prioridad = jObject["Prioridad"].ToString();
			priority.idPrioridad = jObject["idPrioridad"].ToString();
			return priority;
		}

		public override string ToString ()
		{
			if(Prioridad != null)
				return Prioridad;
			else
				return base.ToString();
		}
	}
}


