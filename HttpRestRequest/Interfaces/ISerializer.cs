using System;
using System.IO;

namespace RestCommunication.Interfaces
{
	/// <summary>
	/// generic интерфейс, описывающий serializer данных для преобразования при отправке.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Метод, сериализующий контент в поток для передачи данных
		/// <param name="obj">Объект сериализуюемый в поток</param>
		/// <param name="stream">Поток, в который сериализуется контент</param>
		/// <param name="action">Действие с переданным потоком</param>
		/// </summary>
		void Serialize<T>(T obj, Stream stream, Action<Stream> action);
	}
}
