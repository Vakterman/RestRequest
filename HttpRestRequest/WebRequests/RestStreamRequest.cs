using System.IO;
using System.Net;
using System.Threading.Tasks;
using RestCommunication.Entities;
using RestCommunication.Interfaces;

namespace RestCommunication.WebRequests
{
	/// <summary>
	/// Web-запрос, в который передаётся поток, содержащий данные для отправки (это Put и Post запросы).
	/// </summary>
	internal class RestStreamRequest : RestHttpWebRequest, IRestStreamRequest
	{
		/// <summary>
		/// Создает экземпляр класса <see cref="RestStreamRequest"/>.
		/// </summary>
		/// <param name="method">Web-метод - в данном случае POST или PUT</param>
		/// <param name="url">Базовый URL</param>
		protected RestStreamRequest( string method,  string url)
			: base(method, url)
		{
		}

		/// <summary>
		/// Создает экземпляр класса <see cref="RestStreamRequest"/> реализующий  WebRequest методом POST.
		/// </summary>
		/// <param name="url">Базовый URL</param>
		/// <returns>Экземпляр класса, реализующий POST request</returns>
		public static IRestStreamRequest CreatePostRequest( string url)
		{
			return new RestStreamRequest(RequestMethods.POST, url);
		}

		/// <summary>
		/// Создает экземпляр класса <see cref="RestStreamRequest"/> реализующий  WebRequest методом PUT.
		/// </summary>
		/// <param name="url">Базовый URL</param>
		/// <returns>Экземпляр класса, реализующий PUT запрос</returns>
		public static IRestStreamRequest CreatePutRequest( string url)
		{
			return new RestStreamRequest(RequestMethods.PUT, url);
		}

		/// <summary>
		///  Выполняет запрос с записью ответа в поток
		/// </summary>
		/// <param name="inputStream"> Поток, содержащий данные, которые следует передать в запрос</param>
		/// <returns>Ответ сервера на выполненный запрос </returns>
		public async Task<WebResponse> ExecuteAsync(Stream inputStream)
		{
			var httpWebRequest = CreateHttpWebRequest();

			try
			{
				var httpWebStream = httpWebRequest.GetRequestStream();
				await inputStream.CopyToAsync(httpWebStream);

				return await ExecuteAsyncInternal(httpWebRequest);
			}
			finally
			{
				inputStream.Close();
			}
		}

		/// <summary>
		/// Замещает placeholder в пути к ресурсу.
		/// </summary>
		/// <param name="name">Имя placeholder</param>
		/// <param name="value">Значение параметра.</param> 
		public IRestStreamRequest AddUrlSegment<TObject>(string name, TObject value)
		{
			base.AddUrlSegment(name, value.ToString());

			return this;
		}

		/// <summary>
		/// Добавить значение параметра в uri.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		public IRestStreamRequest AddUriParameter<TObject>(string name, TObject value)
		{
			base.AddUriParameter(name, value.ToString());

			return this;
		}

		/// <summary>
		/// Добавляет заголовок.
		/// </summary>
		/// <param name="name">Имя параметра заголовка.</param>
		/// <param name="value">Значение параметра заголовка.</param>
		public new IRestStreamRequest AddHeader(string name, string value)
		{
			base.AddHeader(name, value);

			return this;
		}

		/// <summary>
		/// Устанавливает тип передаваемого контента.
		/// </summary>
		/// <param name="contentType"> Тип передаваемого контента.</param>
		public new IRestStreamRequest SetContentType(string contentType)
		{
			base.SetContentType(contentType);
			return this;
		}
	}
}
