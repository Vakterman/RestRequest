using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestCommunication.Entities;
using RestCommunication.Extensions;

namespace RestCommunication.WebRequests
{
	/// <summary>
	/// Базовый абстрактный класс для Web-Запросов
	/// </summary>
	internal abstract class WebRequest<TResponse>
	{
		protected readonly List<Parameter> _requestParameters;
		protected readonly string _method;

		protected string _baseUrl { get; set; }

		public Encoding Encoding { get; set; }

		protected  string _contentType  { get; set; }

		/// <summary>
		/// Создает экземпляр класса <see cref="WebRequest"/>.
		/// </summary>
		/// <param name="method">web-метод</param>
		/// <param name="url">Базовый URL, на основе которого формируется запрос</param>
		protected WebRequest( string method,  string url)
		{
			if (string.IsNullOrEmpty(method))
				throw new ArgumentException("method");

			if (string.IsNullOrEmpty(url))
				throw new ArgumentException("url");

			_baseUrl = url;
			_method = method;
			_requestParameters = new List<Parameter>();

			Encoding = Encoding.UTF8;

			_contentType = ContentTypes.ApplicationJson;
		}

	    /// <summary>
	    ///Выполняет Web-запрос асинхронно.
	    /// </summary>
	    public abstract  Task<TResponse> ExecuteAsync();

		/// <summary>
		/// Добавляет заголовок.
		/// </summary>
		/// <param name="name">Имя параметра заголовка.</param>
		/// <param name="value">Значение параметра заголовка.</param>
		public virtual void AddHeader( string name,  string value)
		{
			if(string.IsNullOrEmpty(name))
				throw new ArgumentException("name");

			if(string.IsNullOrEmpty(value))
				throw new ArgumentException("value");

			_requestParameters.Add(new Parameter(name, value, ParameterType.HttpHeader));
		}

		/// <summary>
		/// Замещает placeholder в пути к ресурсу.
		/// </summary>
		/// <param name="name">Имя placeholder</param>
		/// <param name="value">Значение параметра.</param>
		public virtual void AddUrlSegment( string name,  string value)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("name");

			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("value");

			_requestParameters.Add(new Parameter(name, value, ParameterType.UrlSegment));
		}

		/// <summary>
		/// Добавить значение параметра в uri.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		public virtual void AddUriParameter( string name,  string value)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("name");

			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("value");

			_requestParameters.Add(new Parameter(name, value, ParameterType.QueryString));
		}

		/// <summary>
		/// Устанавливает тип передаваемого контента.
		/// </summary>
		/// <param name="contentType">Тип передаваемого контента.</param>
		public virtual void SetContentType( string contentType)
		{
			_contentType = contentType;
		}

		protected virtual Uri BuildUri()
		{
			AddUrlSegments();

			AddUrlParameters();

			return new Uri(_baseUrl);
		}

		private void AddUrlSegments()
		{
			var urlSegments = _requestParameters.Where(urlParameter => urlParameter.ParameterType == ParameterType.UrlSegment);

			foreach (var urlSegment in urlSegments)
			{
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					_baseUrl = _baseUrl.Replace("{" + urlSegment.Name + "}", urlSegment.Value.ToString().UrlEncode());
				}
			}
		}

		private void AddUrlParameters()
		{
			var urlParameters =
				_requestParameters.Where(urlParameter => urlParameter.ParameterType == ParameterType.QueryString).ToList();

			if (urlParameters.Any())
			{
				var data = EncodeParameters(urlParameters);
				var separator = _baseUrl.Contains("?") ? "&" : "?";
				_baseUrl = string.Concat(_baseUrl, separator, data);
			}
		}

		private static string EncodeParameters(IEnumerable<Parameter> parameters)
		{
			return string.Join("&", parameters.Select(EncodeParameter)
				.ToArray());
		}

		private static string EncodeParameter(Parameter parameter)
		{
			return parameter.Value == null
				? string.Concat(parameter.Name.UrlEncode(), "=")
				: string.Concat(parameter.Name.UrlEncode(), "=", parameter.Value.ToString().UrlEncode());
		}
	}
}
