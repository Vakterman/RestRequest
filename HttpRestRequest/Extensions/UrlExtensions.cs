using System;
using System.IO;
using System.Text;
using RestCommunication.Entities;

namespace RestCommunication.Extensions
{
	/// <summary>
	/// Методы расширения для строк, передаваемых по http
	/// </summary>
	public static class UtilityExtensions
	{
		/// <summary>
		/// Uses Uri.EscapeDataString() based on recommendations on MSDN
		/// http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
		/// </summary>
		public static string UrlEncode(this string input)
		{
			const int maxLength = 32766;

			if (input == null)
			{
				throw new ArgumentNullException("input");
			}

			if (input.Length <= maxLength)
			{
				return Uri.EscapeDataString(input);
			}

			var sb = new StringBuilder(input.Length * 2);
			var index = 0;

			while (index < input.Length)
			{
				var length = Math.Min(input.Length - index, maxLength);
				var subString = input.Substring(index, length);

				sb.Append(Uri.EscapeDataString(subString));
				index += subString.Length;
			}

			return sb.ToString();
		}

		//есть еще IsWellFormedUriString, но прочитал на StackOverflow что возращает true для файлового пути.
		public static bool IsValidUrlAdrress(this string baseUrl)
		{
			if (string.IsNullOrEmpty("baseUrl"))
				throw new ArgumentNullException("baseUrl");

			Uri uriResult;
			return Uri.TryCreate(baseUrl, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
		}

		/// <summary>
		/// Сериализация в Json
		/// </summary>
		/// <typeparam name="T"> Тип сериализуемого объекта </typeparam>
		/// <param name="obj"> Объект, который затем сериализуется </param>
		/// <param name="action"> Финальное дейыствие  с потоком </param>
		public static void SerializeToStreamUsingJson<T>(this T obj, Action<Stream> action)
		{
			var serializer = new JsonSerialization();
			serializer.Serialize(obj, action);
		}

		/// <summary>
		/// Сериализация в Xml
		/// </summary>
		/// <typeparam name="T"> Тип сериализуемого объекта </typeparam>
		/// <param name="obj"> Объект, который затем сериализуется </param>
		/// <param name="action"> Финальное дейыствие  с потоком </param>
		public static void SerializeToStreamUsingXml<T>(this T obj, Action<Stream> action)
		{
			var serializer = new DotNetXmlSerialization();
			serializer.Serialize(obj, action);
		}
	}
}
