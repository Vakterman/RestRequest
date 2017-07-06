namespace RestCommunication.Entities
{
	///<summary>
	/// Тип параметров, добавляемых к запросу.
	///</summary>
	public enum ParameterType
	{
		///<summary>
		/// Тип параметра, добавляемого как url сегмент
		///</summary>
		UrlSegment,

		///<summary>
		/// Тип параметра, добавляемого как параметр запроса
		///</summary>
		QueryString,

		///<summary>
		/// Тип параметра, добавляемого как часть заголовка запроса
		///</summary>
		HttpHeader
	}
}
