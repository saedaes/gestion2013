using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Gestion2013iOS
{
	public	class TasksService
	{
		public string Descripcion {get;set;}
		public string Encargado{get;set;}
		public string Titulo {get;set;}
		public string coloniaSolicitante {get;set;}
		public string correoSolicitante {get;set;}
		public string direccionSolicitante {get;set;}
		public string fechaAlta {get;set;}
		public string fechaAsignacion {get;set;}
		public string fechaCompromiso {get;set;}
		public string fechaContacto {get;set;}
		public string fechaEdicion {get;set;}
		public string fechaTermino {get;set;}
		//public string finalizado {get;set;}
		public string idCategoria {get;set;}
		public string idPrioridad {get;set;}
		public string idEstatus {get;set;}
		public string idResponsable {get;set;}
		//public string idSeccion {get;set;}
		public string CveElector {get;set;}
		public string idTarea {get;set;}
		public string nombreSolicitante {get;set;}
		public string idSolicitante {get;set;}
		public string telCasaSolicitante {get;set;}
		public string telCelularSolicitante {get;set;}
		public string usuarioAlta {get;set;}
		//public string usuarioEdicion {get;set;}

		string TasksURL = "";

		public TasksService()
		{
		}

		public void setUser(String appuser){
			this.TasksURL = "http://198.58.107.204:5810/tareas.json?user="+appuser;
		}

		public void setUserandStatus(String appuser, String status){
			this.TasksURL = "http://198.58.107.204:5810/tareas.json?user="+ appuser+"&status="+status;
		}

		public List<TasksService> All()
		{
			return GetTasks();
		}		

		public List <TasksService> GetTasks()
		{

			WebClient client= new WebClient();
			Stream stream= client.OpenRead(TasksURL);
			StreamReader reader= new StreamReader(stream);		
			JArray tasksJSON = JArray.Parse(reader.ReadLine());
			List <TasksService> tasks = new List<TasksService>();

			foreach (JObject jobject in tasksJSON)
			{
				TasksService task = TasksService.FromJObject(jobject);

				tasks.Add(task);

			}

			return tasks;
		}

		internal static TasksService FromJObject(JObject jObject)
		{
			TasksService task = new TasksService();
			task.Descripcion = jObject["Descripcion"].ToString();
			task.Encargado =jObject["Encargado"].ToString();
			task.Titulo = jObject["Titulo"].ToString();
			task.coloniaSolicitante = jObject["coloniaSolicitante"].ToString();
			task.correoSolicitante = jObject["correoSolicitante"].ToString();
			task.direccionSolicitante= jObject["direccionSolicitante"].ToString();
			task.fechaAlta = jObject["fechaAlta"].ToString();
			task.fechaAsignacion = jObject["fechaAsignacion"].ToString();
			task.fechaCompromiso = jObject["fechaCompromiso"].ToString();
			task.fechaContacto = jObject["fechaContacto"].ToString();
			task.fechaEdicion = jObject["fechaEdicion"].ToString();
			task.fechaTermino = jObject["fechaTermino"].ToString();
			//task.finalizado = jObject["finalizado"].ToString();
			task.idCategoria = jObject["idCategoria"].ToString();
			task.idPrioridad = jObject["idPrioridad"].ToString();
			task.idEstatus = jObject["idEstatus"].ToString();
			task.idResponsable = jObject["idResponsable"].ToString();
			//task.idSeccion = jObject["idSeccion"].ToString();
			task.CveElector = jObject["CveElector"].ToString();
			task.idTarea = jObject["idTarea"].ToString();
			task.nombreSolicitante = jObject["nombreSolicitante"].ToString();
			task.idSolicitante = jObject["idSolicitante"].ToString();
			task.telCasaSolicitante = jObject["telCasaSolicitante"].ToString();
			task.telCelularSolicitante = jObject["telCelularSolicitante"].ToString();
			task.usuarioAlta = jObject["usuarioAlta"].ToString();
			//task.usuarioEdicion = jObject["usuarioEdicion"].ToString();

			return task;
		}

		public override string ToString ()
		{
			if(Titulo != null)
				return Titulo;
			else
				return base.ToString();
		}
	}
}

