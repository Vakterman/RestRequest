using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RestCommunication.Interfaces
{
	/// <summary>
	/// Базовый интерфейс для Web-запросов,в которые передаётся stream (POST и PUT запросы) 
	/// </summary>
	public interface IRestStreamRequest : IRequest<IRestStreamRequest>
	{
		/// <summary>
		/// Выполняет запрос с записью ответа в поток
		/// </summary>
		/// <param name="inputStream"> Поток, содержащий данные, которые следует передать в запрос</param>
		/// <returns>Ответ сервера на выполненный запрос </returns>
		Task<WebResponse> ExecuteAsync(Stream inputStream);
	}
}
