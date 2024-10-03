using Newtonsoft.Json;

namespace Sloth.Lib
{
	/// <summary>
	/// A static class providing methods for translating messages between JSON and CSV formats.
	/// </summary>
	public static class Translation
	{
		#region Message Translations

		/// <summary>
		/// Converts a JSON-formatted message string to a CSV-formatted message string.
		/// </summary>
		/// <param name="jsonString">JSON-formatted message string.</param>
		/// <returns>CSV-formatted message string.</returns>
		public static String JsonMessageToCSVMessage(String jsonString)
		{
			try
			{
				Message msg = JsonConvert.DeserializeObject<Message>(jsonString);
				return msg.ToCSV();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a JSON-formatted message string to a Message object.
		/// </summary>
		/// <param name="jsonString">JSON-formatted message string.</param>
		/// <returns>Message object.</returns>
		public static Message JsonMessageToMessage(String jsonString)
		{
			try
			{
				Message msg = JsonConvert.DeserializeObject<Message>(jsonString);
				return msg;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a CSV-formatted message string to a JSON-formatted message string.
		/// </summary>
		/// <param name="csvString">CSV-formatted message string.</param>
		/// <returns>JSON-formatted message string.</returns>
		public static String CSVMessageToJsonMessage(String csvString)
		{
			try
			{
				String[] splitString = csvString.Split(';');
				String id = splitString[0];
				String room = splitString[1];
                int value = Convert.ToInt32(splitString[2]);
				String unit = splitString[3];
				String type = splitString[4];

				Message msg = Factory.CreateMessage(id, room, value, unit, type, "Communication", DateTime.Now);
				return msg.ToJSON();
			}
			catch (Exception e)
			{
				Console.WriteLine($"The CSV-String could not be read. Potentially invalid format. {e.Message}");
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a CSV-formatted message string to a Message object.
		/// </summary>
		/// <param name="csvString">CSV-formatted message string.</param>
		/// <returns>Message object.</returns>
		public static Message CSVMessageToMessage(String csvString)
		{
			Console.WriteLine(csvString);
			try
			{
				String[] splitString = csvString.Split(';');
				String id = splitString[0];
				String room = splitString[1];
                int value = Convert.ToInt32(splitString[2]);
				String unit = splitString[3];
				String type = splitString[4];

				Message msg = Factory.CreateMessage(id, room, value, unit, type, "Communication", DateTime.Now);
				return msg;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a CSV-formatted message string to a Message object with an edited ID.
		/// </summary>
		/// <param name="idOfDevice">The new ID for the device.</param>
		/// <param name="csvString">CSV-formatted message string.</param>
		/// <returns>Message object.</returns>
		public static Message CSVMessageToMessageWithEditedId(String idOfDevice, String csvString)
		{
			try
			{
				String[] splitString = csvString.Split(';');
				String room = splitString[1];
                int value = Convert.ToInt32(splitString[2]);
				String unit = splitString[3];
				String type = splitString[4];

				Message msg = Factory.CreateMessage(idOfDevice, room, value, unit, type, "Communication", DateTime.Now);
				return msg;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		#endregion

		#region StatusMessage Translations

		/// <summary>
		/// Converts a JSON-formatted status message string to a StatusMessage object.
		/// </summary>
		/// <param name="jsonString">JSON-formatted status message string.</param>
		/// <returns>StatusMessage object.</returns>
		public static StatusMessage JsonStatusMessageToStatusMessage(String jsonString)
		{
			try
			{
				StatusMessage sm = JsonConvert.DeserializeObject<StatusMessage>(jsonString);
				return sm;
			}
			catch (Exception e)
			{
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a JSON-formatted status message string to a CSV-formatted status message string.
		/// </summary>
		/// <param name="jsonString">JSON-formatted status message string.</param>
		/// <returns>CSV-formatted status message string.</returns>
		public static String JsonStatusMessageToCSVStatusMessage(String jsonString)
		{
			try
			{
				return JsonStatusMessageToStatusMessage(jsonString).ToCSV();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a CSV-formatted status message string to a StatusMessage object.
		/// </summary>
		/// <param name="csvString">CSV-formatted status message string.</param>
		/// <param name="id">The ID to be assigned to the StatusMessage.</param>
		/// <returns>StatusMessage object.</returns>
		public static StatusMessage CSVStatusMessageToStatusMessage(String csvString, string id)
		{
			try
			{
				String[] splitString = csvString.Split(';');
				String topic = splitString[0];
				String message = splitString[1];

				StatusMessage sm = Factory.CreateStatusMessage(topic, message, topic, id, DateTime.Now);
				return sm;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		/// <summary>
		/// Converts a CSV-formatted status message string to a JSON-formatted status message string.
		/// </summary>
		/// <param name="csvString">CSV-formatted status message string.</param>
		/// <param name="id">The ID to be assigned to the StatusMessage.</param>
		/// <returns>JSON-formatted status message string.</returns>
		public static String CSVStatusMessageToJsonMessage(String csvString, string id)
		{
			try
			{
				return CSVStatusMessageToStatusMessage(csvString, id).ToJSON();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw e.InnerException;
			}
		}

		#endregion
	}
}
