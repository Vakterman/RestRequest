using System.Net;
using System.Threading.Tasks;

namespace RestCommunication.Interfaces
{
	/// <summary>
	/// Интерфейс HTTP запроса для добавления типизации к запросам, поддерживающий fluent api.
	/// </summary>
	public interface IRestRequest : IRequest<IRestRequest>
	{
		/// <summary>
		/// Выполнить запрос асинхронно.
		/// </summary>
		Task<WebResponse> ExecuteAsync();
	}
}
