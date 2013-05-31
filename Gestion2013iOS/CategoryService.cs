using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Gestion2013iOS
{
	public	class CategoryService
	{
		public string Categoria {get;set;}
		public string idCategoria {get;set;}

		string CategoryURL =  "http://148.229.75.81:3000/categorias.json";

		public CategoryService()
		{
		}

		public List<CategoryService> All()
		{
			return GetCategories();
		}		

		public List <CategoryService> GetCategories()
		{

			WebClient client = new WebClient();
			Stream stream = client.OpenRead(CategoryURL);
			StreamReader reader = new StreamReader(stream);		
			JArray categoriesJSON = JArray.Parse(reader.ReadLine());
			List <CategoryService> categories = new List<CategoryService>();

			foreach (JObject jobject in categoriesJSON)
			{
				CategoryService category = CategoryService.FromJObject(jobject);
				categories.Add(category);
			}

			return categories;
		}

		internal static CategoryService FromJObject(JObject jObject)
		{
			CategoryService category = new CategoryService();
			category.Categoria = jObject["Categoria"].ToString();
			category.idCategoria = jObject["idCategoria"].ToString();
			return category;
		}

		public override string ToString ()
		{
			if(Categoria != null)
				return Categoria;
			else
				return base.ToString();
		}
	}
}


