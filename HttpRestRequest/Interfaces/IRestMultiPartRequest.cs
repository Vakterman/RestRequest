using System.Net.Http;
using System.Threading.Tasks;
using RestCommunication.Entities;

namespace RestCommunication.Interfaces
{
	/// <summary>
	/// Интерфейс HTTP запроса с multipart содержимым.
	/// </summary>
	public interface IRestMultiPartRequest : IRequest<IRestMultiPartRequest>
	{
		/// <summary>
		/// Асинхронное выполнение запроса, передающего multipart содержимое
		/// </summary>
		Task<HttpResponseMessage> ExecuteAsync();

		/// <summary>
		/// Добавляет файл, описываемый через контейнер FileParameter.
		/// <param name="fileParameter"> Описание передаваемого в запросе файла.</param>
		/// </summary>
		IRestMultiPartRequest AddFile(FileParameter fileParameter);
	}
}
