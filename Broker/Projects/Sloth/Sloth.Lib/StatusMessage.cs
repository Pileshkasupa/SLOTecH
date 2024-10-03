using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sloth.Lib
{
	/// <summary>
	/// Represents a status message implementing the IMessage interface.
	/// </summary>
	public class StatusMessage
	{
		private string _id;
		private string _topic;
		private string _message;
		private string _messageType;
		private DateTime _timeStamp;

		/// <summary>
		/// Gets the message ID.
		/// </summary>
		public String Id { get { return _id; } }

		/// <summary>
		/// Gets or sets the message topic with internal setter.
		/// </summary>
		public String Topic
		{
			get => _topic; internal set => _topic = value;
		}

		/// <summary>
		/// Gets or sets the message content with internal setter.
		/// </summary>
		public String Message
		{
			get => _message; internal set => _message = value;
		}

		/// <summary>
		/// Gets or sets the message type with internal setter.
		/// </summary>
		public String MessageType
		{
			get => _messageType; internal set => _messageType = value;
		}

		public DateTime TimeStamp
		{
			get => _timeStamp;
			set => _timeStamp = value;
		}

		/// <summary>
		/// Initializes a new instance of the StatusMessage class.
		/// </summary>
		/// <param name="topic">The message topic.</param>
		/// <param name="message">The message content.</param>
		/// <param name="messageType">The message type.</param>
		/// <param name="id">The message ID.</param>
		/// <param name="timeStamp">The time when the status message has been created.</param>
		public StatusMessage(string topic, string message, string messageType, string id, DateTime timeStamp)
		{
			_id = id;
			_topic = topic;
			_message = message;
			_messageType = messageType;
			_timeStamp = timeStamp;
		}

		/// <summary>
		/// Parameterless constructor for JSON deserialization.
		/// </summary>
		public StatusMessage() { }

		/// <summary>
		/// Converts the message to a CSV-formatted string.
		/// </summary>
		/// <returns>CSV-formatted string.</returns>
		public String ToCSV()
		{
			return $"{TimeStamp};{Topic};{Id};{Message}";
		}

		/// <summary>
		/// Converts the message to a JSON-formatted string.
		/// </summary>
		/// <returns>JSON-formatted string.</returns>
		public String ToJSON()
		{
			return JsonConvert.SerializeObject(this);
		}

		/// <summary>
		/// Overrides the base ToString method to provide a custom string representation of the message.
		/// </summary>
		/// <returns>String representation of the message.</returns>
		public override String ToString()
		{
			return $"{TimeStamp}_{Topic}: {Id}-{Message}";
		}
	}
}
