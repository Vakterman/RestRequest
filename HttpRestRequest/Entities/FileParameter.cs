using System;
using System.IO;

namespace RestCommunication.Entities
{
	/// <summary>
	/// Данные,описывающие файл, который должны быть загружен вместе с запросом
	/// </summary>
	public class FileParameter
	{
		/// <summary>
		///  Создать контейнер, содержащий описание параметров файла и действие для записи файлового контента 
		/// </summary>
		/// <param name="name"> Имя параметра используемго в запросе.</param>
		/// <param name="getStream"> Файловый поток, в который передаются данные.</param>
		/// <param name="filename"> Имя файла, используемого в запросе.</param>
		/// <param name="contentType"> Описание типа контента используемог в запросе.</param>
		public FileParameter( string name,  string filename,  string contentType,
			 Func<Stream> getStream)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("name");

			if (string.IsNullOrEmpty(filename))
				throw new ArgumentException("filename");

			if (string.IsNullOrEmpty(contentType))
				throw new ArgumentException("contentType");

			if (getStream == null)
				throw new ArgumentException("stream");

			GetContentStream = getStream;
			FileName = filename;
			ContentType = contentType;
			Name = name;
		}

		/// <summary>
		/// Файловый поток, содержащий данные передаваемого файла
		/// </summary>
		public Func<Stream> GetContentStream { get; private set; }

		/// <summary>
		/// Имя файла для передачи по сети
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// MIME описание передаваемог файла
		/// </summary>
		public string ContentType { get; private set; }

		/// <summary>
		///Имя параметра передаваемого
		/// </summary>
		public string Name { get; private set; }
	}
}
