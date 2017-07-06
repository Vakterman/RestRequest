
namespace RestCommunication.Interfaces
{
	/// <summary>
	/// generic интерфейс HTTP запроса для добавления типизации к запросам, поддерживающий fluent api.
	/// </summary>
	public interface IRequest<out TRequest> where TRequest : IRequest<TRequest>
	{
		/// <summary>
		/// Замещает placeholder в пути к ресурсу.
		/// </summary>
		/// <param name="name">Имя placeholder</param>
		/// <param name="value">Значение параметра.</param>
		TRequest AddUrlSegment<TObject>(string name, TObject value);

		/// <summary>
		/// Добавить значение параметра в uri.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		TRequest AddUriParameter<TObject>(string name, TObject value);

		/// <summary>
		/// Добавляет заголовок.
		/// </summary>
		/// <param name="name">Имя параметра заголовка.</param>
		/// <param name="value">Значение параметра заголовка.</param>
		TRequest AddHeader(string name, string value);

		/// <summary>
		/// Устанавливает тип формата,в который сериализуется содержимое.
		/// </summary>
		/// <param name="contentType">Тип формата, в который сериализуется содержимое.</param>
		TRequest SetContentType(string contentType);
	}
}
