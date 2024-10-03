using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sloth.Lib;
using System;
using System.Net.NetworkInformation;

namespace Sloth.WebAPI.Controller
{

	/// <summary>
	/// Receives an action from the WebApp and sents the received action to the ESP
	/// </summary>
	[Route("api/actors")]
	[ApiController]
	public class ActorsController : ControllerBase
	{
		IMqttServer _slothServer = SlothServer.GetSlothServerAsync().Result;
		
		[HttpPost]
		public ActionResult<string> Post(Message controlMessage)
		{
			if (controlMessage != null)
			{
				try
				{
					String csvControlMessage = controlMessage.ToCSV();
					var message = new MqttApplicationMessageBuilder()
						.WithTopic(controlMessage.Id.Trim())
						.WithPayload(csvControlMessage)
						.WithAtLeastOnceQoS()
						.WithRetainFlag(true)
						.Build();
					_slothServer.PublishAsync(message);
					SlothLogger.ConsoleLogMessageInformation("Control Action received from WebApp and forwarded");

					return "Ok";
				}
				catch (Exception e)
				{
					SlothLogger.ConsoleLogMessageInformation(e.Message);
					throw e.InnerException;
				}
			}
			return "Invalid";
		}
	}
    /// <summary>
    /// Returns the history of a specific GUID to the WebApp
    /// </summary>
    [Route("api/history")]
	[ApiController]
	public class HistoryController : ControllerBase
	{
		[HttpGet]
		public ActionResult<String> Get(string guid)
		{
			DatabaseHelper databaseHelper = Factory.CreateDatabaseHelper();

			List<object> lastValues = databaseHelper.GetMessageObjectDataForCertainGUIDFromDatabase(guid);
            SlothLogger.ConsoleLogMessageInformation($"Last Values for {guid} sent");
            return JsonConvert.SerializeObject(lastValues);
		}
	}

    //[Route("api/status")]
    //[ApiController]
    //public class StatusController : ControllerBase
    //{
    //	[HttpGet]
    //	public ActionResult<String> Get()
    //	{
    //		List<object> values = Statistic.GetLastObjectValuesForEveryGUIDShort();
    //           SlothLogger.ConsoleLogMessageInformation("Last Data for every GUID sent");
    //           return JsonConvert.SerializeObject(values);
    //	}
    //}


    /// <summary>
    /// Returns the Last Data of a specific guid to the WebApp
    /// </summary>
    [Route("api/component")]
	[ApiController]
	public class ComponentController : ControllerBase
	{
		[HttpGet]
		public ActionResult<String> Get(string guid)
		{
			DatabaseHelper databaseHelper = Factory.CreateDatabaseHelper();

			object value = databaseHelper.GetLastMessageObjectsForSpecificGUIDFromDatabase(guid);
            SlothLogger.ConsoleLogMessageInformation($"Last Data for {guid} sent");
            return JsonConvert.SerializeObject(value);
		}
	}

	/// <summary>
	/// Returns the last Data of every GUID to the WebApp
	/// </summary>
	[Route("api/allComponents")]
	[ApiController]
	public class AllComponentsController : ControllerBase
	{
		[HttpGet]
		public ActionResult<String> Get()
		{
			DatabaseHelper databaseHelper = Factory.CreateDatabaseHelper();

			List<object> value = databaseHelper.GetLastMessageObjectDataForAllGUIDsFromDatabase();

            SlothLogger.ConsoleLogMessageInformation("Last Data for all GUIDs sent");
			return JsonConvert.SerializeObject(value);
		}
	}

	[Route("api/setEspConfiguration")]
	[ApiController]
	public class SetEspConfigurationController : ControllerBase
	{
		IMqttServer _slothServer = SlothServer.GetSlothServerAsync().Result;

		[HttpPost]
		public ActionResult<string> Post(EspConfigMessage espConfigMessage)
		{
			if (espConfigMessage != null)
			{
				try
				{
					String csvControlMessage = espConfigMessage.ToCSV();
					_slothServer.PublishAsync($"Configuration/{espConfigMessage.EspGuid.Trim()}", csvControlMessage);
					SlothLogger.ConsoleLogMessageInformation("Esp configuration message received from WebApp and forwarded");

					return "Ok";
				}
				catch (Exception e)
				{
					SlothLogger.ConsoleLogMessageInformation(e.Message);
					return $"Unable to send message. Exception: {e.InnerException.Message}";
				}
			}
			return "Invalid message";
		}
	}

	[Route("api/createScene")]
	[ApiController]
	public class CreateSceneController : ControllerBase
	{
		SceneManager sceneManager = Factory.CreateSceneManager();

		[HttpPost]
		public ActionResult<string> Post(string scene)
		{
			string realSceneRaw;
			if(scene != null)
			{
				realSceneRaw = scene.Replace("%3C", "<");

				realSceneRaw = scene.Replace("%3E", ">");

				realSceneRaw = scene.Replace("%3D", "=");

				Console.WriteLine($"From WebAapp: {scene}");
				Console.WriteLine($"Changed: {realSceneRaw}");


				sceneManager.AddScene(realSceneRaw);
				
				return "Ok";
			}
			return "Failed";
		}
	}

	[Route("api/getScenes")]
	[ApiController]
	public class GetScenesController : ControllerBase
	{
		[HttpGet]
		public ActionResult<String> Get()
		{
			DatabaseHelper databaseHelper = Factory.CreateDatabaseHelper();
			List<object> objScenes = new List<object>();
			List<Scene> scenes = databaseHelper.GetScenesFromDatabase();
			foreach(Scene scene in scenes)
			{
				objScenes.Add(scene);
				Console.WriteLine(scene.Raw);
			}
			SlothLogger.ConsoleLogMessageInformation("All scenes sent");
			return JsonConvert.SerializeObject(scenes);
		}
	}


	[Route("api/espConfigMessage")]
    [ApiController]
    public class EspConfigMessageController : ControllerBase
    {
        IMqttServer _slothServer = SlothServer.GetSlothServerAsync().Result;

        [HttpPost]
        public ActionResult<string> Post(string espGuid, int intervalOfMeasurementInMiliseconds)
        {
			try
			{
                EspConfigMessage configMsg = Factory.CreateEspConfigMessage(espGuid, intervalOfMeasurementInMiliseconds);
                _slothServer.PublishAsync($"Configuration/{configMsg.EspGuid}", Convert.ToString(configMsg.IntervalOfMeasurementInSeconds));

                return "Config message sent";
            }
			catch (Exception)
			{
				return "Sending the config message failed";
				throw;
			}
        }
    }

    [Route("api/deleteScene")]
	[ApiController]
	public class DeleteSceneController : ControllerBase
	{
		SceneManager sceneManager = Factory.CreateSceneManager();

		[HttpDelete]
		public ActionResult<string> Delete(string title)
		{
			if (title != null)
			{
				sceneManager.RemoveScene(title);
				SlothLogger.ConsoleLogMessageInformation("Scene removed");
				return "Ok";
			}

			return "Invalid";
		}
	}

	[Route("api/test")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpPost]
		public ActionResult<string> Post(string message)
		{
			Console.WriteLine(message);
			return "Test success";
		}
	}


	[Route("api/statusMessages")]
	[ApiController]
	public class StatusMessagesController : ControllerBase
	{
		[HttpGet]
		public ActionResult<String> Get()
		{
			DatabaseHelper databaseHelper = Factory.CreateDatabaseHelper();

			List<object> values = databaseHelper.GetStatusMessagesObjectsFromDatabaseForControllers();
			SlothLogger.ConsoleLogMessageInformation($"Status messages sent");
			return JsonConvert.SerializeObject(values);

		}
	}


}
