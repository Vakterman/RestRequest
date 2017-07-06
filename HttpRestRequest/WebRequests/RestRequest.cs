using RestCommunication.Entities;
using RestCommunication.Interfaces;

namespace RestCommunication.WebRequests
{
	/// <summary>
	/// Базовый класс для Web-Запросов простых web-запросов (это Get и Delete запросы).
	/// </summary>
	internal class RestRequest : RestHttpWebRequest, IRestRequest
	{
		/// <summary>
		/// Создает экземпляр класса <see cref="RestRequest"/>.
		/// </summary>
		/// <param name="method">Web-метод - в данном случае POST или PUT</param>
		/// <param name="url">Базовый URL</param>
		protected RestRequest( string method,  string url)
			: base(method, url)
		{
		}

		/// <summary>
		/// Создаёт GET запрос.
		/// </summary>
		/// <param name="url">базовый url, по которому должен будет производиться запрос</param>
		public static IRestRequest CreateGetRequest(string url)
		{
			return new RestRequest(RequestMethods.GET, url);
		}

		/// <summary>
		/// Создаёт PUT запрос.
		/// </summary>
		/// <param name="url">базовый url, по которому должен будет производиться запрос</param>
		public static IRestRequest CreatePutRequest(string url)
		{
			return new RestRequest(RequestMethods.PUT, url);
		}


		/// <summary>
		/// Создаёт DELETE запрос.
		/// </summary>
		/// <param name="url">базовый url, по которому должен будет производиться запрос</param>
		public static IRestRequest CreateDeleteRequest(string url)
		{
			return new RestRequest(RequestMethods.DELETE, url);
		}

		/// <summary>
		/// Создаёт POST запрос без передачи содержимого.
		/// </summary>
		/// <param name="url">базовый url, по которому должен будет производиться запрос</param>
		public static IRestRequest CreatePostRequest(string url)
		{
			return new RestRequest(RequestMethods.POST, url);
		}

		/// <summary>
		/// Замещает placeholder в пути к ресурсу.
		/// </summary>
		/// <param name="name">Имя placeholder</param>
		/// <param name="value">Значение параметра.</param>
		public IRestRequest AddUrlSegment<TObject>(string name, TObject value)
		{
			base.AddUrlSegment(name, value.ToString());
			return this;
		}

		/// <summary>
		/// Добавить значение параметра в uri.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		public IRestRequest AddUriParameter<TObject>(string name, TObject value)
		{
			base.AddUriParameter(name, value.ToString());
			return this;
		}

		/// <summary>
		/// Добавляет параметр заголовка.
		/// </summary>
		/// <param name="name">Имя параметра заголовка.</param>
		/// <param name="value">Значение параметра заголовка.</param>
		public new IRestRequest AddHeader(string name, string value)
		{
			base.AddHeader(name, value);
			return this;
		}

		/// <summary>
		/// Устанавливает тип формата,в который сериализуется содержимое.
		/// </summary>
		/// <param name="contentType">Тип формата, в который сериализуется содержимое.</param>
		public new IRestRequest SetContentType(string contentType)
		{
			base.SetContentType(contentType);
			return this;
		}
	}
}
