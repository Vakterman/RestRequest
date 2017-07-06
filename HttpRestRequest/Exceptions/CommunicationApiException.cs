using System;
using System.Runtime.Serialization;

namespace RestCommunication.Exceptions
{
	/// <summary>
	/// Ошибка недоступной точки доступа
	/// </summary>
	[Serializable]
	public class CommunicationApiException : ApiException
	{
		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="CommunicationApiException"/>.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="exception"></param>
		public CommunicationApiException( Uri uri,  Exception exception = null)
			: base(uri, "Endpoint is unavailable", exception)
		{
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="CommunicationApiException"/>.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public CommunicationApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
