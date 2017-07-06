using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RestCommunication.Entities;
using RestCommunication.Interfaces;

namespace RestCommunication.WebRequests
{
	/// <summary>
	/// Web-запрос для передачи multipart содержимого. 
	/// </summary>
	internal class RestMultipartRequest : WebRequest<HttpResponseMessage>, IRestMultiPartRequest
	{
		private ICollection<FileParameter> _fileCollection;

		/// <summary>
		/// Создаёт новый Web-запрос для передачи multipart-данных.
		/// <param name="url"> Базовый url, по которому происходит запрос</param>
		/// </summary>
		protected RestMultipartRequest( string url) : 
			base(RequestMethods.POST, url)
		{
			if(string.IsNullOrEmpty(url))
				throw new ArgumentException("url");

			_fileCollection = new List<FileParameter>();
		}

		/// <summary>
		/// Создает экземпляр класса <see cref="RestMultipartRequest"/>, реализующий  передачу multipar данных методом POST.
		/// </summary>
		/// <param name="url">Базовый URL</param>
		/// <returns> Экземпляр класса, реализующий POST request передавая multipart data</returns>
		public static IRestMultiPartRequest CreateRestMultipartRequest(string url)
		{
			return new RestMultipartRequest(url);
		}

		/// <summary>
		///Выполняет Web-запрос асинхронно.
		/// </summary>
		public override async Task<HttpResponseMessage> ExecuteAsync()
		{
			using (var client = new HttpClient())
			using (var multipartMetadata = new MultipartFormDataContent())
			{
				var uri = BuildUri();

				var headerParams = _requestParameters.Where(_ => _.ParameterType == ParameterType.HttpHeader);

				foreach (var param in headerParams)
				{
					client.DefaultRequestHeaders.Add(param.Name, param.Value.ToString());
				}

				var streamsCollection = new List<Stream>();

				try
				{
					foreach (var fileData in _fileCollection)
					{
						var stream = fileData.GetContentStream();
						streamsCollection.Add(stream);
						multipartMetadata.Add(new StreamContent(stream), fileData.Name, fileData.FileName);
					}

					return await client.PostAsync(uri, multipartMetadata);
				}
				finally
				{
					foreach (var stream in streamsCollection)
					{
						stream.Close();
					}
				}
			}
		}

		/// <summary>
		/// Замещает placeholder в пути к ресурсу.
		/// </summary>
		/// <param name="name"> Имя placeholder</param>
		/// <param name="value"> Значение параметра.</param>
		public IRestMultiPartRequest AddUrlSegment<TObject>(string name, TObject value)
		{
			base.AddUrlSegment(name, value.ToString());
			return this;
		}

		/// <summary>
		/// Добавить значение параметра в uri.
		/// </summary>
		/// <param name="name"> Имя параметра.</param>
		/// <param name="value"> Значение параметра.</param>
		public IRestMultiPartRequest AddUriParameter<TObject>(string name, TObject value)
		{
			base.AddUriParameter(name, value.ToString());
			return this;
		}

		/// <summary>
		/// Добавляет заголовок.
		/// </summary>
		/// <param name="name">Имя параметра заголовка.</param>
		/// <param name="value">Значение параметра заголовка.</param>
		public new IRestMultiPartRequest AddHeader(string name, string value)
		{
			base.AddHeader(name, value);
			return this;
		}

		public new IRestMultiPartRequest SetContentType(string contentType)
		{
			base.SetContentType(contentType);
			return this;
		}

		/// <summary>
		/// Добавляет файл, описываемый через контейнер FileParameter.
		/// <param name="fileParameter"> Описание передаваемого в запросе файла.</param>
		/// </summary>
		public IRestMultiPartRequest AddFile( FileParameter fileParameter)
		{
			if (fileParameter == null)
				throw new ArgumentException("fileParameter");

			_fileCollection.Add(fileParameter);
			return this;
		}
	}
}
