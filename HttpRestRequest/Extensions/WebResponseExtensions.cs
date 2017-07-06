using System;
using System.IO;
using System.Net;
using System.Net.Http;
using RestCommunication.Entities;
using RestCommunication.Interfaces;

namespace RestCommunication
{
	/// <summary>
	/// Методы расширения для обработки Web response'a. 
	/// </summary>
	public static class WebResponseExtensions
	{
		/// <summary>
		/// Записывает содержимое Web Response'a в поток, передаваемый пользователем метода. 
		/// </summary>
		/// <param name="response">response, из которого извлекается поток с ответными данными.</param>
		/// <param name="outputStream">поток, в который пишутся данные из web response'a.</param> 
		public static void WriteToStream( this WebResponse response, Stream outputStream)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			var responseStream = response.GetResponseStream();

			try
			{
				if (responseStream != null)
					responseStream.CopyToAsync(outputStream).Wait();
			}
			finally
			{
				if (responseStream != null)
					responseStream.Close();
			}
		}

		/// <summary>
		/// Записывает Web Response в строку
		/// </summary>
		/// <param name="response">response, из которого извлекается stream</param>
		public static string WebResponseToString( this WebResponse response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			var responseStream = response.GetResponseStream();

			try
			{
				if (responseStream != null)
				{
					using (var sw = new StreamReader(responseStream))
					{
						return sw.ReadToEnd();
					}
				}
			}
			finally
			{
				if (responseStream != null)
					responseStream.Close();
			}

			return null;
		}

		/// <summary>
		/// Записывает HttpResponseMessage в строку
		/// </summary>
		/// <param name="responseMessage">response, из которого извлекается поток, содержащий ответные данные.</param>
		public static string WebResponseToString( this HttpResponseMessage responseMessage)
		{
			if (responseMessage == null)
				throw new ArgumentNullException("responseMessage");

			var responseStream = responseMessage.Content.ReadAsStreamAsync().GetAwaiter().GetResult();

			try
			{
				if (responseStream != null)
				{
					using (var sw = new StreamReader(responseStream))
					{
						return sw.ReadToEnd();
					}
				}
			}
			finally
			{
				if (responseStream != null)
					responseStream.Close();
			}

			return string.Empty;
		}

		public static TResult GetResultFromJson<TResult>( this WebResponse response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			return response.GetDeserializedResult<TResult>(new JsonSerialization());
		}

		public static TResult GetResultFromXml<TResult>( this WebResponse response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			return response.GetDeserializedResult<TResult>(new DotNetXmlSerialization());
		}

		public static TResult GetDeserializedResult<TResult>( this WebResponse response, IDeserializer deserializer)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			if (deserializer == null)
				throw new ArgumentNullException("deserializer");

			return deserializer.Deserialize<TResult>(response.GetResponseStream());
		}

	}
}
