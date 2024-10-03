using MQTTnet.Server;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Client.Receiving;
using MQTTnet.Server.Internal;
using MQTTnet.Protocol;

namespace Sloth.Lib
{
	public class SlothServer : IMqttApplicationMessageReceivedHandler, IMqttServerClientConnectedHandler, IMqttServerClientDisconnectedHandler, IMqttServerClientSubscribedTopicHandler, IMqttServerClientUnsubscribedTopicHandler
	{
		private static IMqttServer? server;
		private static SlothServer? mqttServer;
		SlothLogger _logger = Factory.CreateSlothLogger();
		NextJsClient _nextJsClient = Factory.CreateNextJsClient("http://127.0.0.1:5227/api");
		ConnectedClientsManager _connectedClientsManager = Factory.CreateConnectedClientsManager();
		SceneManager _sceneManager = Factory.CreateSceneManager();
		Statistic _statistic = Factory.CreateStatistic();
		DatabaseHelper _database;
		private static IMqttRetainedMessagesManager _retainedMessagesManager;
		

		// Topics:
		//  - {guid}
		//  - Configuration/{guid}
		//  - Status/{Info-/Error-/WarningTopic}


		#region Singleton necessities
		private SlothServer() 
		{
			_database = Factory.CreateDatabaseHelper();
		}

		public static SlothServer GetSlothMqttServerInstance()
		{
			if (SlothServer.mqttServer == null)
			{
				SlothServer.mqttServer = new SlothServer();
			}

			return SlothServer.mqttServer;
		}
		public static async Task<IMqttServer> GetSlothServerAsync()
		{
			if (SlothServer.server == null)
			{
				_retainedMessagesManager = new MqttRetainedMessagesManager();

				IMqttServerOptions mqttServerOptions = new MqttFactory().CreateServerOptionsBuilder()
				.WithClientCertificate(null, false)
				.WithConnectionBacklog(1000)
				.WithRetainedMessagesManager(_retainedMessagesManager)
				.WithEncryptionSslProtocol(System.Security.Authentication.SslProtocols.None) //!
				.WithClientId("Broker")
				.Build();

				SlothServer.server = new MqttFactory().CreateMqttServer();
				await SlothServer.server.StartAsync(mqttServerOptions);
			}

			return SlothServer.server;
		}
		#endregion

		#region Event Handlers
#pragma warning disable CS1998
		/// <summary>
		/// Handles the incoming messages and filters between status messages and normal messages
		/// </summary>
		/// <param name="eventArgs"></param>
		/// <returns></returns>
		public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
		{
			SlothLogger.ConsoleLogMessageInformation(eventArgs.ApplicationMessage.ConvertPayloadToString());
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Task.Run(() =>
			{
				string topicReceivedOn = eventArgs.ApplicationMessage.Topic;
				string payload = eventArgs.ApplicationMessage.ConvertPayloadToString();
				string idOfClientSending = eventArgs.ClientId;
				string destinationIdOfMessage = payload.Split(';')[0];


				if (destinationIdOfMessage == "Broker")
				{

					if (topicReceivedOn == "Status/Info" || topicReceivedOn == "Status/Warning" || topicReceivedOn == "Status/Error")
					{
						String[] splitPayload = payload.Split(";");
						String[] splitTopic = topicReceivedOn.Split("/");
						StatusMessage statusMessage = Factory.CreateStatusMessage(splitTopic[1], splitPayload[1], splitTopic[1], idOfClientSending, DateTime.Now);
						SlothLogger.ConsoleLogStatusMessage(statusMessage);
						
						_database.AddDataTo_StatusMessage(statusMessage.Id, statusMessage.Topic, statusMessage.Message, statusMessage.MessageType, statusMessage.TimeStamp);
						_nextJsClient.PostDataToNextJs(statusMessage);
					}
					else
					{
						Message message = Translation.CSVMessageToMessageWithEditedId(idOfClientSending, payload);
						SlothLogger.ConsoleLogMessageInformation($"Received from ESP: {message.ToCSV()}");
						_statistic.Add(message); // Newest values and all values
												 //_nextJsClient.PostDataToNextJs(message);
												 //SlothLogger.ConsoleLogMessageInformation($"{message.ToCSV()} published to Web Application.");

						SlothLogger.ConsoleLogMessageInformation($"Testing scenes. 20240313");
						Console.WriteLine(_database.GetScenesFromDatabase()[0].Raw);
						#region Scenes region
						foreach (Scene scene in _database.GetScenesFromDatabase())
						{
							Console.WriteLine(scene.Raw);
							foreach (Condition condition in scene.Conditions)
							{
								Console.WriteLine(condition.Operator);
								if (condition.Id == idOfClientSending)
								{
									switch (condition.Operator)
									{
										case ">":
											if (message.Value >= Convert.ToInt32(condition.Value))
											{
												_sceneManager.RunScene(scene.Title);
												SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene ran", "System", "Broker", DateTime.Now));
											}
											break;
										case "<":
											if (message.Value <= Convert.ToInt32(condition.Value))
											{
												_sceneManager.RunScene(scene.Title);
												SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene ran", "System", "Broker", DateTime.Now));
											}
											break;
										case "=":
											Console.WriteLine(condition.Value);
											Console.WriteLine(message.Value);
											if (message.Value == Convert.ToInt32(condition.Value))
											{
												Console.WriteLine(scene.Title);
												_sceneManager.RunScene(scene.Title);
												SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene ran", "System", "Broker", DateTime.Now));
											}
											else
											{
												SlothLogger.ConsoleLogMessageInformation($"This should be equal");
											}
											break;
										//case "!=":
										//	if (message.Value != Convert.ToInt32(condition.Value))
										//	{
										//		_sceneManager.RunScene(scene.Title);
										//		SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene ran", "System", "Broker", DateTime.Now));
										//	}
										//	break;
										default:
											SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene invalid", "System", "Broker", DateTime.Now));
											break;
									}
								}
							}
						}
						#endregion

					}
				}
			});
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

		}

        #region Connection event handlers

        /// <summary>
        /// Adds the client to the CurrentClientsConnected file and creates a new status message that the client connected, also a ConsoleLog of the status messageis outputted
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
		{
			_connectedClientsManager.HandleClientConnected(eventArgs.ClientId);
		}

		/// <summary>
		/// Removes the client from the CurrentClientsConnected file and creates a new status message that the client disconnected, also a ConsoleLog of the status messageis outputted
		/// </summary>
		/// <param name="eventArgs"></param>
		/// <returns></returns>
		public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
		{
			_connectedClientsManager.HandleClientDisconnected(eventArgs.ClientId);
		}
        #endregion

        #region Subscription event handlers
        /// <summary>
        /// Creates a status message that the client subscribed to the topic and also a ConsoleLog of the status message is outputted
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
		{
			StatusMessage statusMessage = Factory.CreateStatusMessage("System", $"{eventArgs.ClientId} subscribed to {eventArgs.TopicFilter.Topic}", "System", "Broker", DateTime.Now);
			_database.AddDataTo_StatusMessage(statusMessage.Id, statusMessage.Topic, statusMessage.Message, statusMessage.MessageType, statusMessage.TimeStamp);
			SlothLogger.ConsoleLogStatusMessage(statusMessage);

			return Task.CompletedTask;
		}

		/// <summary>
		/// Creates a status message that the client unsubscribed to the topic and also a ConsoleLog of the status message is outputted
		/// </summary>
		/// <param name="eventArgs"></param>
		/// <returns></returns>
		public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
		{
			StatusMessage statusMessage = Factory.CreateStatusMessage("System", $"{eventArgs.ClientId} unsubscribed to {eventArgs.TopicFilter}", "System", "Broker", DateTime.Now);
			_database.AddDataTo_StatusMessage(statusMessage.Id, statusMessage.Topic, statusMessage.Message, statusMessage.MessageType, statusMessage.TimeStamp);
			SlothLogger.ConsoleLogStatusMessage(statusMessage);

			return Task.CompletedTask;
		}
		#endregion
		#endregion


		// Method to run scene, based on its parameter, which will be a list of messages to send, so the event are ran. 
		// Bei der Übergabe einer Szene von den Controllern werden mehrere Json Objekte kommen von der WebApp, die übersetzen wir dann und speichen die Szene in eine Datei ab in folgendem Format:
		// id: [List<String> {"Lamp;1;percent;...", "Temperature;26,celisus, ..."}]
		// Eine MEthode in der zukünftigen Klasse SceneManager wird dafür zuständig sein diese Liste aus der Datei zurückzuliefern und mit einem Parameter in der Datei nachzusuchen.

	}
}
