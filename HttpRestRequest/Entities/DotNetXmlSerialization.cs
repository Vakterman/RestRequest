using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RestCommunication.Interfaces;

namespace RestCommunication.Entities
{
	/// <summary>
	/// Класс-обёртка для системного класса System.Xml.Serialization.XmlSerializer.
	/// </summary>
	public class DotNetXmlSerialization : ISerializer, IDeserializer
	{
		/// <summary>
		/// Конструктор по умолчанию, создающий объект класса <see cref="DotNetXmlSerialization"/>
		/// </summary>
		public DotNetXmlSerialization()
		{
			this.Encoding = Encoding.UTF8;
		}

		/// <summary>
		/// Указывает namespace для сериализации
		/// </summary>
		/// <param name="namespace">XML namespace</param>
		public DotNetXmlSerialization(string @namespace)
			: this()
		{
			this.Namespace = @namespace;
		}

		/// <summary>
		/// XML namespace to use when serializing
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Кодировка для сериализуемого контента
		/// </summary>
		public Encoding Encoding { get; set; }

		public void Serialize<T>(T obj, Stream stream, Action<Stream> action)
		{
			var ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, this.Namespace);

			var serializer = new XmlSerializer(obj.GetType());

			using (var writer = new StreamWriter(stream, this.Encoding))
			using (var jsonWriter = new XmlTextWriter(writer))

			{
				serializer.Serialize(jsonWriter, obj, ns);
				jsonWriter.Flush();
				stream.Position = 0;

				action(stream);
			}
		}

		/// <summary>
		/// Сериализовать конкретный объект.
		/// </summary>
		/// <param name="obj">Объект для сериализации.</param>
		/// <param name="action"></param>
		public void Serialize<T>(T obj, Action<Stream> action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			using (var memoryStream = new MemoryStream())
			{
				Serialize(obj, memoryStream, action);
			}
		}

		/// <summary>
		/// Сериализовать конкретный объект.
		/// </summary>
		/// <param name="responseStream">Поток, из которого берётся контент для десериализации.</param>
		public T Deserialize<T>(Stream responseStream)
		{
			var serializer = new XmlSerializer(typeof(T));
			using (var reader = new StreamReader(responseStream))
			using (var xmlReader = new XmlTextReader(reader))
			{
				return (T)serializer.Deserialize(xmlReader);
			}
		}
	}
}
