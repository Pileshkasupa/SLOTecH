using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace Sloth.Lib
{
    public class SceneManager
    {
        SlothLogger _logger = Factory.CreateSlothLogger();
        IMqttServer _slothServer = SlothServer.GetSlothServerAsync().Result;
        private static SceneManager? _instance;
        private List<Scene> _scenes;
        DatabaseHelper _databaseHelper = Factory.CreateDatabaseHelper();
        private SceneManager()
        {
            GetScenesFromDatabase();
        }

        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneManager();
                }
                return _instance;
            }
        }
        internal List<Scene> Scenes { get { return _scenes; } }

        /// <summary>
        /// Returns true if scene exists
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        private bool CheckIfSceneExists(Scene scene)
        {
            foreach (var item in _scenes)
            {
                if (item.Title == scene.Title)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds the scene into the list and stores the new scene in a file
        /// </summary>
        /// <param name="scene"></param>
        public void AddScene(Scene scene)
		{
			if (!CheckIfSceneExists(scene))
			{
                _scenes.Add(scene);

                _databaseHelper.AddDataTo_Scenes(scene.Title, scene.Raw);

                foreach (Message item in scene.Messages)
                {
                    _databaseHelper.AddDataTo_MessagesScenes(item.Id, item.Room, item.Value, item.Unit, item.Type, item.MessageType, scene.Title);
                }
                foreach (Condition item in scene.Conditions)
                {
                    _databaseHelper.AddDataTo_Condition(item.Value, item.Operator, item.Id, scene.Title);
                }

                SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene added", "System", "Broker", DateTime.Now));
                return;
            }
            SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene already exists", "System", "Broker", DateTime.Now));
        }

        /// <summary>
        /// Adds the scene into the list and stores the new scene in a file
        /// </summary>
        /// <param name="raw"></param>
        public void AddScene(string raw)
        {
            Scene scene = new Scene(raw);
            if (!CheckIfSceneExists(scene))
            {
                _scenes.Add(scene);

                _databaseHelper.AddDataTo_Scenes(scene.Title, scene.Raw);

                foreach (Message item in scene.Messages)
                {
                    _databaseHelper.AddDataTo_MessagesScenes(item.Id, item.Room, item.Value, item.Unit, item.Type, item.MessageType, scene.Title);
                }
                foreach (Condition item in scene.Conditions)
                {
                    _databaseHelper.AddDataTo_Condition(item.Value, item.Operator, item.Id, scene.Title);
                }
                SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene added", "System", "Broker", DateTime.Now));
                return;
            }
            SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene already exists", "System", "Broker", DateTime.Now));

        }

        /// <summary>
        /// Removes the scene from the list and deletes the scene from the file
        /// </summary>
        /// <param name="scene"></param>
        internal void RemoveScene(Scene scene)
        {
            if (CheckIfSceneExists(scene))
            {
                _scenes.Remove(scene);
                SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene was removed", "System", "Broker", DateTime.Now));
                _databaseHelper.DeleteDataFrom_Scenes(scene.Title);
                //_logger.RemoveSceneFromFile(scene);
                return;
            }
            SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene doesn't exist", "System", "Broker" , DateTime.Now));
        }

        /// <summary>
        /// Removes the scene from the list and deletes the scene from the file
        /// </summary>
        /// <param name="scene"></param>
        public void RemoveScene(string name)
        {
            foreach (Scene item in _scenes)
            {
                if (item.Title.Trim() == name.Trim())
                {
                    _scenes.Remove(item);
                    SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene was removed", "System", "Broker", DateTime.Now));
                    _databaseHelper.DeleteDataFrom_Scenes(item.Title);
                    //_logger.RemoveSceneFromFile(item);
                    return;
                }
            }
            SlothLogger.ConsoleLogStatusMessage(Factory.CreateStatusMessage("System", "Scene doesn't exists", "System", "Broker", DateTime.Now));
        }

        /// <summary>
        /// Runs the scene
        /// </summary>
        /// <param name="nameOfScene"></param>
        public void RunScene(string nameOfScene)
        {
            foreach (Scene item in _scenes)
            {
                if (item.Title.Trim() == nameOfScene.Trim())
                {
                    foreach (Message message in item.Messages)
                    {
                        Console.WriteLine(message.ToCSV());
                        var mqttMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(message.Id.Trim())
                        .WithPayload(message.ToCSV())
                        .WithAtLeastOnceQoS()
						.WithRetainFlag(true)
						.Build();

						Task.Run(() => _slothServer.PublishAsync(mqttMessage));
                    }
                }
            }
        }

        /// <summary>
        /// Fills the list with scenes from the database
        /// </summary>
        public void GetScenesFromDatabase()
        {
            _scenes = _databaseHelper.GetScenesFromDatabase();

            foreach (var item in _scenes)
            {
                item.Messages = _databaseHelper.GetMessagesForSceneFromDatabase(item.Title);
                item.Conditions = _databaseHelper.GetConditionForSceneFromDatabase(item.Title);
            }
            //_scenes = _logger.GetScenesFromFile();
        }

        /// <summary>
        /// Writes the scenes that are in the list into the database
        /// </summary>
        public void WriteScenesToDatabase()
        {
            List<Message> msg;
            List<Condition> conditions;

            foreach (var item in _scenes)
            {
                _databaseHelper.AddDataTo_Scenes(item.Title, item.Raw);
                msg = item.Messages;
                foreach (var message in msg)
                {
                    _databaseHelper.AddDataTo_MessagesScenes(message.Id, message.Room, message.Value, message.Unit, message.Type, message.MessageType, item.Title);
                }
                conditions = item.Conditions;
                foreach (var condition in conditions)
                {
                    _databaseHelper.AddDataTo_Condition(condition.Value, condition.Operator, condition.Id, item.Title);
                }
            }

            //_logger.WriteScenesToFile(_scenes);
        }

    }
}
