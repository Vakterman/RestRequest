using System;
using System.Net.Http;
using Newtonsoft.Json;
using RestCommunication.Entities;

namespace RestCommunication.Exceptions
{
	public static class ApiExceptionExtensions
	{
		
		public static HttpError TryGetHttpError( this ApiException exception)
		{
			if (exception == null)
				throw new ArgumentNullException("exception");

			if (exception.Response == null
				|| exception.Response.Content == null)
				return null;

			HttpError httpError;
			//TODO: в разработке, еще не продумал
			//if (exception.Response.TryGetContentValue(out httpError))
			//	return httpError;

			return DeserializeHttpError(exception.Response.Content);
		}

		
		private static HttpError DeserializeHttpError( HttpContent content)
		{
			if (content.Headers.ContentType == null || content.Headers.ContentType.MediaType != "application/json")
				return null;

			HttpError result = null;
			try
			{
				result = JsonConvert.DeserializeObject<HttpError>(content.ReadAsStringAsync().Result);
			}
			catch (JsonException)
			{
			}

			return result;
		}
	}
}