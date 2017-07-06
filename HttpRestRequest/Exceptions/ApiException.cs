using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using RestCommunication.Entities;

namespace RestCommunication.Exceptions
{
	/// <summary>
	/// Ошибка некорректного запроса к Web API
	/// </summary>
	[Serializable]
	public class ApiException : Exception
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ApiException"/>.
		/// </summary>
		/// <param name="uri"> Uri при запросе к которому произошло исключение. </param>
		/// <param name="response"> Ответ http, при котором произошло исключение. </param>
		/// <param name="httpError"> Ошибки. </param>
		public ApiException( Uri uri,  HttpResponseMessage response,  HttpError httpError)
			: this(uri, response.ReasonPhrase)
		{
			Response = response ?? throw new ArgumentNullException("response");
			StatusCode = response.StatusCode;
			HttpError = httpError;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ApiException"/>.
		/// </summary>
		/// <param name="uri"> Uri при запросе к которому произошло исключение. </param>
		/// <param name="message"> Сообщение. </param>
		/// <param name="exception"> Исключение. </param>
		public ApiException( Uri uri,  string message, Exception exception = null)
			: base(GetMessage(message, uri), exception)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			Uri = uri;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ApiException"/>.
		/// </summary>
		/// <param name="info"><see cref="T:System.Runtime.Serialization.SerializationInfo" /> инфо для сериализации.</param>
		/// <param name="context"><see cref="T:System.Runtime.Serialization.StreamingContext" /> контекстная информация.</param>
		public ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ApiException"/>.
		/// </summary>
		/// <param name="uri"> Uri при запросе к которому произошло исключение. </param>
		/// <param name="statusCode"> Код http, при котором произошло исключение. </param>
		public ApiException( Uri uri, HttpStatusCode statusCode)
			: this(uri, GetHttpStatusMessage(statusCode))
		{
			StatusCode = statusCode;
		}

		/// <summary>
		/// Код http, при котором произошло исключение
		/// </summary>
		public HttpStatusCode StatusCode { get; private set; }

		/// <summary>
		/// Ответ http, при котором произошло исключение
		/// </summary>
		
		public HttpResponseMessage Response { get; private set; }

		/// <summary>
		/// Ошибки.
		/// </summary>
		
		public HttpError HttpError { get; private set; }

		/// <summary>
		/// Uri при запросе к которому произошло исключение
		/// </summary>
		
		public Uri Uri { get; private set; }

		private static string GetHttpStatusMessage(HttpStatusCode statusCode)
		{
			switch (statusCode)
			{
				case HttpStatusCode.BadRequest:
					return "Bad request in api client.";
				case HttpStatusCode.InternalServerError:
					return "Internal source error occured.";
			}
			return "Unknown error in api client.";
		}

		private static string GetMessage(string message, Uri uri)
		{
			return string.Format("{0}{1}Request URI: '{2}'", message, Environment.NewLine, uri.AbsoluteUri);
		}
	}
}