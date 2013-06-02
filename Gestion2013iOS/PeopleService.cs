using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Gestion2013iOS
{
	public	class PeopleService
	{
		public string idPadron {get;set;}
		public string nombre {get;set;}
		public string apellidoPaterno {get;set;}
		public string apellidoMaterno {get;set;}
		public string cveElectoral {get;set;}
		string PeopleURL = "";

		public PeopleService(){
		}

		public void FindPeople(String nombre, String apaterno, String amaterno)
		{
			this.PeopleURL = "http://198.58.107.204:5810/padron.json?nombre="+nombre+"&apaterno="+apaterno+"&amaterno="+amaterno;
		}

		public List<PeopleService> All()
		{
			return GetPeople();
		}		

		public List <PeopleService> GetPeople()
		{

			WebClient client= new WebClient();
			Stream stream= client.OpenRead(PeopleURL);
			StreamReader reader= new StreamReader(stream);		
			JArray peopleJSON = JArray.Parse(reader.ReadLine());
			List <PeopleService> people = new List<PeopleService>();

			foreach (JObject jobject in peopleJSON)
			{
				PeopleService task = PeopleService.FromJObject(jobject);

				people.Add(task);

			}

			return people;
		}

		internal static PeopleService FromJObject(JObject jObject)
		{
			PeopleService person = new PeopleService();
			person.idPadron = jObject["idPadron"].ToString();
			person.nombre =jObject["nombre"].ToString();
			person.apellidoPaterno = jObject["apellidoPaterno"].ToString();
			person.apellidoMaterno = jObject["apellidoMaterno"].ToString();
			person.cveElectoral = jObject["cveElectoral"].ToString();


			return person;
		}

		public override string ToString ()
		{
			if(nombre != null)
				return nombre +" "+ apellidoPaterno+" "+apellidoMaterno+" - "+cveElectoral;
			else
				return base.ToString();
		}
	}
}


