using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Gestion2013iOS
{
	public	class ResponsibleService
	{
		public string Username {get;set;}
		public string UserID {get;set;}

		string ResponsibleURL =  "http://148.229.75.81:3000/resp.json";

		public ResponsibleService()
		{
		}

		public List<ResponsibleService> All()
		{
			return GetResponsibles();
		}		

		public List <ResponsibleService> GetResponsibles()
		{

			WebClient client = new WebClient();
			Stream stream = client.OpenRead(ResponsibleURL);
			StreamReader reader = new StreamReader(stream);		
			JArray responsiblesJSON = JArray.Parse(reader.ReadLine());
			List <ResponsibleService> responsibles = new List<ResponsibleService>();

			foreach (JObject jobject in responsiblesJSON)
			{
				ResponsibleService responsible = ResponsibleService.FromJObject(jobject);
				responsibles.Add(responsible);
			}

			return responsibles;
		}

		internal static ResponsibleService FromJObject(JObject jObject)
		{
			ResponsibleService responsible = new ResponsibleService();
			responsible.Username = jObject["Username"].ToString();
			responsible.UserID = jObject["UserID"].ToString();
			return responsible;
		}

		public override string ToString ()
		{
			if(Username != null)
				return Username;
			else
				return base.ToString();
		}
	}
}


