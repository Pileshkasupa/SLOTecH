using Sloth; using MQTTnet.Server;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.PacketDispatcher;
using MQTTnet.Protocol;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using Sloth.Lib;
using Sloth.WebAPI;

namespace Sloth.WebAPI
{

	class Program
	{
		static void Main(string[] args)
		{
			#region Webservice
			WebService webService = new WebService();
			Task.Run(() => webService.Run());
			Task.WaitAll();
			#endregion

			#region Database
			DatabaseHelper _databaseHelper = Factory.CreateDatabaseHelper();


			//string room = "Wohnzimmer";
			//int value = 10;
			//string unit = "Grad";
			//string type = "type";
			//string messageType = "message";
			//string sceneName = "name";
			//string topic = "topic";
			//string message = "message";
			//string identifier = "id";
			//string name = "name";
			//string raw = "raw";
			//string guid = "guid";
			//char @operator = '+';
			//string client = "client";


			//_databaseHelper.AddDataTo_StatusMessage(guid, topic, message, messageType, DateTime.Now);
			//_databaseHelper.AddDataTo_Scenes(name, raw);
			//_databaseHelper.AddDataTo_Message(identifier, room, value, unit, type, messageType, DateTime.Now);
			//_databaseHelper.AddDataTo_Condition(value, @operator + "", guid, sceneName);
			//_databaseHelper.AddDataTo_ConnectedClients(client);
			//_databaseHelper.AddDataTo_MessagesScenes(identifier, room, value, unit, type, messageType, sceneName);

			//_databaseHelper.DeleteDataFrom_Scenes(name);
			//_databaseHelper.DeleteDataFrom_Message(identifier, room, value, unit, type, messageType);
			//_databaseHelper.DeleteDataFrom_MessageScenes(sceneName);
			//_databaseHelper.DeleteDataFrom_Condition(sceneName);
			//_databaseHelper.DeleteDataFrom_StatusMessage(topic, message, messageType);
			//_databaseHelper.RemoveConnectedClientFromDatabase(client);

			/*
			_databaseHelper.GetMessagesForSceneFromDatabase("hallo");
			_databaseHelper.GetConditionForSceneFromDatabase("hallo");
			*/

			#endregion

			SceneManager manager = Factory.CreateSceneManager();

			//manager.AddScene(Factory.CreateScene("TestScene:47a5a7c5-09af-44bd-1c84-9c25bc737293==1000%3f6d4a76-7d13-43b3-e983-85d358f38748;kitchen;1000;boolean;lamp"));
			//manager.AddScene(Factory.CreateScene("TestScene2:47a5a7c5-09af-44bd-1c84-9c25bc737293==0%3f6d4a76-7d13-43b3-e983-85d358f38748;kitchen;0;boolean;lamp"));


            IMqttServer server = SlothServer.GetSlothServerAsync().Result;
			SlothServer mqttServer = Factory.CreaterSlothServer();

			// server, when this happens, use that method(method as parameter)
			server.UseApplicationMessageReceivedHandler(mqttServer.HandleApplicationMessageReceivedAsync);
			server.UseClientConnectedHandler(mqttServer.HandleClientConnectedAsync);
			server.UseClientDisconnectedHandler(mqttServer.HandleClientDisconnectedAsync);
			server.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(mqttServer.HandleClientUnsubscribedTopicAsync);
			server.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(mqttServer.HandleClientSubscribedTopicAsync);
			//server.StartedHandler = new MqttServerStartedHandlerDelegate(mqttServer.HandleServerStartedAsync);
			//server.StoppedHandler = new MqttServerStoppedHandlerDelegate(mqttServer.HandleServerStoppedAsync);
			while(true){
				Console.WriteLine("Slept.");
				Thread.Sleep(10000);
			}
		}

	}
}
