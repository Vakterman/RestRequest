using System;
using System.IO;
using System.Net;
using System.Net.Http;
using RestCommunication.Entities;
using RestCommunication.Interfaces;

namespace RestCommunication.Extensions
{
	/// <summary>
	/// Методы расширения для выполнения Web request'a
	/// </summary>
	public static class WebRequestExtensions
	{
		/// <summary>
		/// Синхронно выполняет Web запрос и получает результат
		/// <returns>Результат web запроса</returns>
		/// </summary>
		public static WebResponse Execute(this IRestRequest restRequest)
		{
			if(restRequest == null)
				throw new ArgumentNullException("restRequest");

			return restRequest.ExecuteAsync().GetAwaiter().GetResult();
		}

		/// <summary>
		/// Синхронно выполняет Web запрос c передачей потока и получает результат
		/// <returns>Результат web запроса</returns>
		/// </summary>
		public static WebResponse Execute(this IRestStreamRequest restRequest, Stream stream)
		{
			if (restRequest == null)
				throw new ArgumentNullException("restRequest");

			if (stream == null)
				throw new ArgumentNullException("stream");

			return restRequest.ExecuteAsync(stream).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Синхронно выполняет Web запрос и получает результат, преобразовывая его к указанному типу
		/// </summary>
		public static TResult Execute<TResult>(this IRestRequest request)
		{
			if(request == null)
				throw new ArgumentNullException("request");

			return request.Execute().GetResultFromJson<TResult>();
		}

		/// <summary>
		/// Синхронно выполняет Web запрос передающий multipart содержимое
		/// </summary>
		public static HttpResponseMessage Execute( this IRestMultiPartRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			return request.ExecuteAsync().GetAwaiter().GetResult();
		}

		/// <summary>
		/// Синхронно выполняет Web запрос и получает результат в виде строки
		/// <returns> Результат в виде строки</returns>
		/// </summary>
		public static string ExecuteString( this IRestRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			return request.Execute().WebResponseToString();
		}

		/// <summary>
		/// Выполнить запрос синхронно, передав в теле потока сериализованный объект в Xml
		/// </summary>
		/// <typeparam name="T">Тип передаваемого в запросе объекта</typeparam>
		/// <param name="request">Web запрос передающий Stream</param>
		/// <param name="obj"> Передаваемый объект </param>
		/// <returns> Результат web запроса </returns>
		public static WebResponse Execute<T>( this IRestStreamRequest request, T obj)
		{
			if(request == null)
				throw new ArgumentNullException("request");

			WebResponse response = null;
			obj.SerializeToStreamUsingJson(streamForRequest => { response = request.Execute(stream: streamForRequest); });

			return response;
		}

		/// <summary>
		/// Выполнить запрос синхронно, передав в теле потока сериализованный объект и получить десериализованный результат
		/// </summary>
		/// <typeparam name="TResult">Тип передаваемого в запросе объекта</typeparam>
		/// <param name="request">Web запрос передающий Stream</param>
		/// <param name="stream"> Передаваемый объект </param>
		/// <returns> Результат web запроса </returns>
		public static TResult Execute<TResult>( this IRestStreamRequest request, Stream stream)
		{
			if(request == null)
				throw new ArgumentNullException("request");

			return request.Execute(stream).GetResultFromJson<TResult>();
		}

		/// <summary>
		/// Выполнить запрос синхронно, передав в теле потока объект, который будет сериализоваться в Json
		/// </summary>
		/// <typeparam name="T">Тип передаваемого в запросе объекта</typeparam>
		/// <typeparam name="TResult">Тип возращаемого значения</typeparam>
		/// <param name="request">Web запрос, передающий объект</param>
		/// <param name="obj"> Передаваемый объект </param>
		/// <returns> Результат web запроса сериализованный в указанный тип</returns>
		public static TResult Execute<T, TResult>( this IRestStreamRequest request, T obj)
		{
			if(request == null)
				throw new ArgumentNullException("request");

			WebResponse response = null;
			obj.SerializeToStreamUsingJson(streamForRequest => { response = request.Execute(stream: streamForRequest); });

			return response.GetResultFromJson<TResult>();
		}

		/// <summary>
		/// Выполнить запрос синхронно, передав в теле потока объект, который будет сериализоваться в XML
		/// </summary>
		/// <typeparam name="T">Тип передаваемого в запросе объекта</typeparam>
		/// <typeparam name="TResult">Тип возращаемого значения</typeparam>
		/// <param name="request">Web запрос, передающий объект</param>
		/// <param name="obj"> Передаваемый объект </param>
		/// <returns> Результат web запроса сериализованный в указанный тип</returns>
		public static TResult ExecuteXml<T, TResult>( this IRestStreamRequest request, T obj)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			request.SetContentType(ContentTypes.ApplicationXml);
			WebResponse response = null;
			obj.SerializeToStreamUsingXml(streamForRequest => { response = request.Execute(stream: streamForRequest); });

			return response.GetResultFromXml<TResult>();
		}

		public static WebResponse ExecuteXml<T>( this IRestStreamRequest request, T obj)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			request.SetContentType(ContentTypes.ApplicationXml);
			WebResponse response = null;
			obj.SerializeToStreamUsingXml(streamForRequest => { response = request.Execute(stream: streamForRequest); });

			return response;
		}
	}
}
