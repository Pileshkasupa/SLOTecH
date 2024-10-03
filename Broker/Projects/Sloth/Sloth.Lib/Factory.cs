using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Server;

namespace Sloth.Lib
{
    public class Factory
    {
        public static Condition CreateCondition(string raw)
        {
            return new Condition(raw);
        }
        public static ConnectedClientsManager CreateConnectedClientsManager()
        {
            return ConnectedClientsManager.Instance;
        }
        public static DatabaseHelper CreateDatabaseHelper()
        {
            return DatabaseHelper.Instance;
        }
        public static EspConfigMessage CreateEspConfigMessage(string espGuid, int intervalOfMeasurementInMilliseconds)
        {
            return new EspConfigMessage(espGuid, intervalOfMeasurementInMilliseconds);
        }
		public static Message CreateMessage(string identifier, string room, int value, string unit, string type, string messageType = "Communication", DateTime? timeStamp = null)
		{
			return new Message(identifier, room, value, unit, type, timeStamp , messageType);
		}
		public static NextJsClient CreateNextJsClient(string nextJsUrl)
        {
            return new NextJsClient(nextJsUrl);
        }
        public static Scene CreateScene(string raw)
        {
            return new Scene(raw);
        }
        public static SceneManager CreateSceneManager()
        {
            return SceneManager.Instance;
        }
        public static SlothLogger CreateSlothLogger()
        {
            return new SlothLogger();
        }
        public static SlothServer CreaterSlothServer()
        {
            return SlothServer.GetSlothMqttServerInstance();
        }
        public static Statistic CreateStatistic()
        {
            return Statistic.Instance;
        }
        public static StatusMessage CreateStatusMessage(string topic, string message, string messageType, string id, DateTime timeStamp)
        {
            return new StatusMessage(topic, message, messageType, id, timeStamp);
        }
    }
}
