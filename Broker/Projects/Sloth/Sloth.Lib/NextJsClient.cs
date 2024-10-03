using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sloth.Lib
{
	/// <summary>
	/// Client for interacting with a Next.js server, providing methods to post data asynchronously.
	/// </summary>
	public class NextJsClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _nextJsUrl = "http://192.168.10.221:3000/next/api"; // Server URL

		/// <summary>
		/// Initializes a new instance of the NextJsClient class with the specified Next.js server URL.
		/// </summary>
		/// <param name="nextJsUrl">The URL of the Next.js server.</param>
		public NextJsClient(string nextJsUrl)
		{
			_httpClient = new HttpClient();
			_nextJsUrl = nextJsUrl;
		}

		/// <summary>
		/// Posts a message data object to the specified Next.js endpoint asynchronously.
		/// </summary>
		/// <param name="data">The message data to be posted.</param>
		/// <returns>A string representing the response from the server.</returns>
		public String PostDataToNextJs(Message data)
		{
			try
			{
				var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
				var response = Task.Run(() => _httpClient.PostAsync($"{_nextJsUrl}/update", content));
				Task.WaitAll(response);
				var result = response.Result;

				if (result.IsSuccessStatusCode)
				{
					var responseContent = result.Content.ReadAsStringAsync();
					Task.WaitAll(responseContent);
					return responseContent.Result;
				}
				else
				{
					return "Request failed";
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		/// <summary>
		/// Posts a status message data object to the specified Next.js endpoint asynchronously.
		/// </summary>
		/// <param name="data">The status message data to be posted.</param>
		/// <returns>A string representing the response from the server.</returns>
		public String PostDataToNextJs(StatusMessage data)
		{
			try
			{
				var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
				var response = Task.Run(() => _httpClient.PostAsync($"{_nextJsUrl}/update", content));
				Task.WaitAll(response);
				var result = response.Result;

				if (result.IsSuccessStatusCode)
				{
					var responseContent = result.Content.ReadAsStringAsync();
					Task.WaitAll(responseContent);
					return responseContent.Result;
				}
				else
				{
					return "Request failed";
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
