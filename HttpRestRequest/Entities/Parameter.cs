using System;

namespace RestCommunication.Entities
{
	/// <summary>
	/// Описание параметра запроса
	/// </summary>
	public class Parameter
	{
		/// <summary>
		/// Конструктор, инициализирующий Parameter
		/// </summary>
		public Parameter( string name,  string value, ParameterType paramType)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("name");

			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("value");

			Name = name;
			Value = value;
			ParameterType = paramType;
		}

		/// <summary>
		/// Значение добавляемого  параметра.
		/// </summary>
		public object Value { get; private set; }

		/// <summary>
		/// Имя добавляемого  параметра.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Тип добавляемого  параметра.
		/// </summary>
		public ParameterType ParameterType { get; private set; }
	}
}
