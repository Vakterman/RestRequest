using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using RestCommunication.Entities;
using RestCommunication.Exceptions;
using ApiException = RestCommunication.Entities.ApiException;
using SystemWebRequest = System.Net.WebRequest;

namespace RestCommunication.WebRequests
{
	/// <summary>
	/// Базовый класс для Web-запросов, использующий HttpWebRequest
	/// </summary>
	internal abstract class RestHttpWebRequest : WebRequest<WebResponse>
	{
		private const int MaxAttemptCount = 3;

		/// <summary>
		/// Создаёт новый Web-запрос, использующий HttpWebRequest.
		/// <param name="method">Определение Http метода</param>
		/// <param name="url">Базовый url, по которому происходит запрос</param>
		/// </summary>
		protected RestHttpWebRequest( string method,  string url) :
			base(method, url)
		{
		}

		/// <summary>
		///Выполняет Web-запрос асинхронно.
		/// </summary>
		public override Task<WebResponse> ExecuteAsync()
		{
			return ExecuteAsyncInternal(CreateHttpWebRequest());
		}

		protected virtual async Task<WebResponse> ExecuteAsyncInternal(HttpWebRequest httpWebRequest)
		{
			AddHeaders(httpWebRequest);

			var taskResult = TryExecuteRequest(httpWebRequest);

			return await taskResult.Task;
		}

		private CommunicationApiException ConvertToCommunicationException(Exception exception, Uri requestUri)
		{
			SocketException socketException;
			if (TryFindSocketException(exception, out socketException))
				return new CommunicationApiException(requestUri, socketException);

			return null;
		}

		private TaskCompletionSource<WebResponse> TryExecuteRequest(HttpWebRequest httpWebRequest, int tryCount = 0)
		{
			var taskResult = new TaskCompletionSource<WebResponse>();
			try
			{
				var result = httpWebRequest.GetResponseAsync().GetAwaiter().GetResult();
				taskResult.SetResult(result);
			}
			catch (Exception exception)
			{
				var communicationException = ConvertToCommunicationException(exception, httpWebRequest.RequestUri);
				if (communicationException != null)
				{
					taskResult.SetException(communicationException);
				}
				else if (IsRetryNeeded(exception, httpWebRequest.Method) && tryCount < MaxAttemptCount)
				{
					return TryExecuteRequest(httpWebRequest, ++tryCount);
				}
				else
				{
					taskResult.SetException(new ApiException(httpWebRequest.RequestUri, exception.Message, exception));
				}
			}

			return taskResult;
		}

		private bool IsRetryNeeded(Exception exception, string method)
		{
			var webException = exception as WebException;
			if (webException != null && webException.Status == WebExceptionStatus.ServerProtocolViolation
				&& method == RequestMethods.GET)
			{
				return true;
			}

			return false;
		}

		private static bool TryFindSocketException(Exception exception, out SocketException socketException)
		{
			var currentException = exception;

			while (currentException != null)
			{
				socketException = currentException as SocketException;

				if (socketException != null)
					return true;

				currentException = currentException.InnerException;
			}

			socketException = null;
			return false;
		}

		protected virtual HttpWebRequest CreateHttpWebRequest()
		{
			var request = SystemWebRequest.Create(BuildUri());
			request.Method = _method;

			return (HttpWebRequest)request;
		}

		protected void AddHeaders(HttpWebRequest httpWebRequest)
		{
			var headers = _requestParameters.Where(param => param.ParameterType == ParameterType.HttpHeader);

			foreach (var header in headers)
			{
				httpWebRequest.Headers.Add(header.Name, header.Value.ToString());
			}

			httpWebRequest.ContentType = _contentType;
		}
	}
}
