using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Threading.Tasks;

namespace Sloth.Lib
{
	public sealed class ConnectedClientsManager
	{
		//private const string _filePath = FilePaths.PathOfGUIDFile;
		private static ConnectedClientsManager? _instance;
		private readonly List<string> _connectedClients = new List<string>();
		private SlothLogger _logger = Factory.CreateSlothLogger();
		private object _lock = new object();
		DatabaseHelper _database;
		private ConnectedClientsManager()
		{
            _database = Factory.CreateDatabaseHelper();
			_connectedClients = _database.GetConnectedClientsFromDatabase();
		}

		public static ConnectedClientsManager Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				else
				{
					_instance = new ConnectedClientsManager();
					return _instance;
				}
			}
		}
		public void HandleClientConnected(string clientGuid)
		{
			lock (_lock)
			{
				if (!_connectedClients.Contains(clientGuid))
				{
					_connectedClients.Add(clientGuid);
					Task.Run(() => _database.AddDataTo_ConnectedClients(clientGuid));

					StatusMessage statusMessage = Factory.CreateStatusMessage("System", $"{clientGuid} connected.", "System", "Broker", DateTime.Now);
					_database.AddDataTo_StatusMessage(statusMessage.Id, statusMessage.Topic, statusMessage.Message, statusMessage.MessageType, statusMessage.TimeStamp);
					SlothLogger.ConsoleLogStatusMessage(statusMessage);
				}
				else return;
				
			}
		}

		public void HandleClientDisconnected(string clientGuid)
		{

			lock (_lock)
			{
				if (_connectedClients.Contains(clientGuid.Trim()))
				{
					_connectedClients.Remove(clientGuid.Trim());
					_database.RemoveConnectedClientFromDatabase(clientGuid);

					StatusMessage statusMessage = Factory.CreateStatusMessage("System", $"{clientGuid} disconnected.", "System", "Broker", DateTime.Now);
					_database.AddDataTo_StatusMessage(statusMessage.Id, statusMessage.Topic, statusMessage.Message, statusMessage.MessageType, statusMessage.TimeStamp);
					SlothLogger.ConsoleLogStatusMessage(statusMessage);
				}
				else
				{
					return;
				}
			}
		}
		private List<string> GetConnectedClients()
		{
			lock (_lock)
			{
				return new List<string>(_connectedClients);
			}
		}

	}
}
