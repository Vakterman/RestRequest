using System.IO;

namespace RestCommunication.Interfaces
{
	/// <summary>
	/// generic интерфейс, описывающий deserializer данных для преобразования данных в ответ на web запрос.
	/// </summary>
	public interface IDeserializer
	{
		/// <summary>
		/// Десериализовать ответ.
		/// </summary>
		/// <typeparam name="T">Требуемый выходной тип.</typeparam>
		/// <param name="responseStream">Поток, содержащий ответ от клиента.</param>
		/// <returns> Результат десериализации. </returns>
		T Deserialize<T>(Stream responseStream);
	}
}
