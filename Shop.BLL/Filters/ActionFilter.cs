using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shop.BLL.Filters
{
	public class ActionFilter : Attribute, IActionFilter
	{
		IUnitOfWork Database { get; set; }

		public ActionFilter(IUnitOfWork uow)
		{
			Database = uow;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			//LogToDB(context);
			LogToFile(context);
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			//LogToDB(context);
			LogToFile(context);
		}

		public void LogToFile(ActionExecutingContext context)
		{
			StringBuilder builder = new StringBuilder();
			builder
				.AppendLine("----------")
				.AppendFormat("Request Time:\t{0}", DateTime.Now.ToString())
				.AppendLine()
				.AppendFormat("Request ULI:\t{0}", context.HttpContext.Request.Path)
				.AppendLine()
				.AppendFormat("Headers:\t{0}", context.HttpContext.Request.Headers.ToString())
				.AppendLine()
				.AppendFormat("UserName:\t{0}", context.HttpContext.User.Identity.Name)
				.AppendLine()
				.AppendFormat("Query String:\t{0}", context.HttpContext.Request.QueryString)
				.AppendLine()
				.AppendFormat("Http Verb:\t{0}", context.HttpContext.Request.Method)
				.AppendLine();

			string filePath = "log.log";

			//using (StreamWriter writer = System.IO.File.AppendText(filePath))
			//{
			//	writer.Write(builder.ToString());
			//	writer.Flush();
			//	writer.Close();
			//}
		}

		public void LogToFile(ActionExecutedContext context)
		{
			StringBuilder builder = new StringBuilder();
			builder
				.AppendLine("----------")
				.AppendFormat("Response Time:\t{0}", DateTime.Now.ToString())
				.AppendLine()
				.AppendFormat("Request ULI:\t{0}", context.HttpContext.Request.Path)
				.AppendLine()
				.AppendFormat("Headers:\t{0}", context.HttpContext.Response.Headers.Count)
				.AppendLine()
				.AppendFormat("UserName:\t{0}", context.HttpContext.User.Identity.Name)
				.AppendLine()
				.AppendFormat("Status Code:\t{0}", context.HttpContext.Response.StatusCode)
				.AppendLine();

			string filePath = "log.log";

			//using (StreamWriter writer = System.IO.File.AppendText(filePath))
			//{
			//	writer.Write(builder.ToString());
			//	writer.Flush();
			//}
		}

		//public void LogToDB(ActionExecutingContext context)
		//{
		//	LogDetail logDetail = new LogDetail()
		//	{
		//		RequestTime = DateTime.Now.ToString(),
		//		RequestUrl = context.HttpContext.Request.Path,
		//		UserName = context.HttpContext.User.Identity.Name,
		//		Headers = context.HttpContext.Request.Headers.ToString(),
		//		QueryString = context.HttpContext.Request.QueryString.ToString(),
		//		HttpVerb = context.HttpContext.Request.Method
		//	};

		//	Database.LogDetails.Create(logDetail);
		//	Database.Save();
		//}

		//public void LogToDB(ActionExecutedContext context)
		//{
		//	LogDetail logDetail = new LogDetail()
		//	{
		//		ResponseTime = DateTime.Now.ToString(),
		//		RequestUrl = context.HttpContext.Request.Path,
		//		UserName = context.HttpContext.User.Identity.Name,
		//		Headers = context.HttpContext.Request.Headers.ToString(),
		//		HttpVerb = context.HttpContext.Request.Method,
		//		StatusCode = context.HttpContext.Response.StatusCode.ToString()
		//	};

		//	Database.LogDetails.Create(logDetail);
		//	Database.Save();
		//}
	}
}
