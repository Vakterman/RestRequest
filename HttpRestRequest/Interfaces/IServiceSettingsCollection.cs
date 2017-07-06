namespace RestCommunication.Interfaces
{
	/// <summary>
	/// generic интерфейс, описывающий коллекцию настроек сервисов .
	/// </summary>
	public interface IServiceSettingsCollection
	{
		/// <summary>
		/// Получает строковый Uri по имени сервиса
		/// </summary>
		/// <returns></returns>
		bool TryGetServiceUrlByName(string serviceName, out string serviceUrl);
	}
}
