using Newtonsoft.Json;
using System;

namespace Sloth.Lib
{
	/// <summary>
	/// Represents a message with specific properties and serialization methods.
	/// </summary>
	public class Message
	{
		#region Instance Variables
		private string _id;
		private string _room;
		private int _value;
		private string _unit;
		private string _type;
		private string _messageType;
		private DateTime? _timeStamp = null;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the identifier of the message. This is the Guid of the ESPs or the Broker's identifier.
		/// </summary>
		public string Id
		{
			get => _id;
			set => _id = value;
		}

		/// <summary>
		/// Gets or sets the room associated with the message.
		/// </summary>
		public string Room
		{
			get => _room;
			set => _room = value;
		}

		/// <summary>
		/// Gets or sets the value of the message.
		/// </summary>
		public int Value
		{
			get => _value;
			set => _value = value;
		}

		/// <summary>
		/// Gets or sets the unit of measurement for the message value.
		/// </summary>
		public string Unit
		{
			get => _unit;
			set => _unit = value;
		}

		/// <summary>
		/// Gets or sets the type of the message.
		/// </summary>
		public string Type
		{
			get => _type;
			set => _type = value;
		}

		/// <summary>
		/// Gets or sets the message type, always set to "Communication". You are not allowed to change this from "Communication" to something else.
		/// </summary>
		public string MessageType
		{
			get => _messageType;
			set => _messageType = value;
		}

        /// <summary>
        /// Gets or sets the timestamp of the message.
        /// </summary>
        public DateTime? TimeStamp
		{
			get
			{
				if (_timeStamp != null)
				{
					return _timeStamp;
				}
				else
				{
					return null;
				}
			}
			set => _timeStamp = value;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Message class with specified parameters.
		/// </summary>
		/// <param name="identifier">The identifier of the message.</param>
		/// <param name="room">The room associated with the message.</param>
		/// <param name="value">The value associated with the message.</param>
		/// <param name="unit">The unit of measurement for the message value.</param>
		/// <param name="type">The type of the message.</param>
		public Message(string identifier, string room, int value, string unit, string type, DateTime? timeStamp, string messageType = "Communication")
		{
			_id = identifier;
			_room = room;
			_value = value;
			_unit = unit;
			_type = type;
			_messageType = messageType;
			if (timeStamp != null)
			{
				_timeStamp = timeStamp;
			}
		}

		/// <summary>
		/// Parameterless constructor for JSON deserialization.
		/// </summary>
		public Message() { }
		#endregion

		#region Methods
		/// <summary>
		/// Converts the message to a CSV-formatted string.
		/// </summary>
		/// <returns>A CSV-formatted string representing the message.</returns>
		public string ToCSV()
		{
			return $"{Id};{Room};{Value};{Unit};{Type}";
		}

		/// <summary>
		/// Converts the message to a JSON-formatted string.
		/// </summary>
		/// <returns>A JSON-formatted string representing the message.</returns>
		public string ToJSON()
		{
			return JsonConvert.SerializeObject(this);
		}
		#endregion
	}
}
