using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestCommunication.Interfaces;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace RestCommunication.Entities
{
	/// <summary>
	/// Десериализатор клиента для json, обрабатывающий ICollection
	/// </summary>
	public sealed class JsonSerialization : IDeserializer, ISerializer
	{
		private readonly JsonSerializer _serializer;

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="JsonSerialization"/>.
		/// </summary>
		public JsonSerialization()
			: this(
				new JsonSerializerSettings
				{
					ContractResolver = new PrivateJsonContractResolver(),
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					TypeNameHandling = TypeNameHandling.Objects | TypeNameHandling.Arrays
				})
		{
		}

		public JsonSerialization(JsonSerializerSettings settings)
		{
			_serializer = JsonSerializer.Create(settings);

			_serializer.Converters.Add(new StringEnumConverter());

			Culture = CultureInfo.InvariantCulture;
			ContentType = ContentTypes.ApplicationJson;
		}

		/// <summary>
		/// Корневой элемент.
		/// </summary>
		public string RootElement { get; set; }

		/// <summary>
		/// Пространство имён.
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Формат даты.
		/// </summary>
		public string DateFormat { get; set; }

		/// <summary>
		/// Тип контента.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Настройки культуры.
		/// </summary>
		public CultureInfo Culture { get; set; }

		/// <summary>
		/// Сериализовать конкретный объект.
		/// </summary>
		/// <param name="obj">Объект для сериализации.</param>
		/// <param name="stream">Поток, в который записывается сериализованный объект</param>
		/// <param name="action"></param>
		public void Serialize<T>(T obj, Stream stream,  Action<Stream> action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			using (var writer = new StreamWriter(stream))
			using (var jsonWriter = new JsonTextWriter(writer))

			{
				_serializer.Serialize(jsonWriter, obj);
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
		public void Serialize<T>(T obj,  Action<Stream> action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			using (var memoryStream = new MemoryStream())
			{
				Serialize(obj, memoryStream, action);
			}
		}

		/// <summary>
		/// Десериализовать ответ.
		/// </summary>
		/// <typeparam name="T">Требуемый выходной тип.</typeparam>
		/// <param name="streamResponse">Поток, содержащий ответ от клиента.</param>
		/// <returns>Требуемый выходной тип.</returns>
		public T Deserialize<T>(Stream streamResponse)
		{
			T result;

			using (var reader = new StreamReader(streamResponse))
			{
				using (var jsonReader = new JsonTextReader(reader))
				{
					result = _serializer.Deserialize<T>(jsonReader);
				}
			}

			return result;
		}

		/// <summary>
		/// Резолвер, который умеет писать в публичные поля с приватным сеттером.
		/// </summary>
		private class PrivateJsonContractResolver : CamelCasePropertyNamesContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				var prop = base.CreateProperty(member, memberSerialization);

				if (!prop.Writable)
				{
					var property = member as PropertyInfo;
					if (property != null)
					{
						var hasPrivateSetter = property.GetSetMethod(true) != null;
						prop.Writable = hasPrivateSetter;
					}
				}

				return prop;
			}
		}
	}
}
