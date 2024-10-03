using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sloth.Lib
{
	/// <summary>
	/// Represents a class for statistical operations related to sensor messages.
	/// </summary>
	public class Statistic
    {
        List<Message> _values = new List<Message>();
        SlothLogger _logger = Factory.CreateSlothLogger();
		static DatabaseHelper _databaseHelper;
        private static Statistic? instance = null;
        private static readonly object padlock = new object();

        private Statistic()
        {
            _databaseHelper = Factory.CreateDatabaseHelper();
        }
		/// <summary>
		/// Gets the singleton instance of the Statistic class.
		/// </summary>
		public static Statistic Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Statistic();
						instance._values = _databaseHelper.GetMessagesFromDatabase();
                    }
					return instance;
				}
			}
		}
		/// <summary>
		/// Adds a message to the statistic and logs the message to database.
		/// </summary>
		/// <param name="message">The message to be added to the statistic.</param>
		public void Add(Message message)
        {
            _values.Add(message);
			_databaseHelper.AddDataTo_Message(message.Id, message.Room, message.Value, message.Unit, message.Type, message.MessageType, message.TimeStamp);
		}

		#region Message list methods
		/// <summary>
		/// Gets the last values for all GUIDs from the file.
		/// </summary>
		/// <returns>List of last values for all GUIDs.</returns>
		//public static List<Message>? GetLastValuesForAllGUIDs()
  //      {
  //          List<Message> data = new List<Message>();
  //          lock (Statistic.Instance)
  //          {
		//		try
		//		{
		//			if (!File.Exists(FilePaths.PathOfNewestValuesFile))
		//			{
		//				return null;
		//			}

		//			using (StreamReader sr = new StreamReader(FilePaths.PathOfNewestValuesFile))
		//			{
		//				while (!sr.EndOfStream)
		//				{
		//					string[] line = sr.ReadLine().Split(";");
		//					data.Add(new Message(line[0], line[1], Convert.ToInt32(line[2]), line[3], line[4]));
		//				}
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			Console.WriteLine(ex.Message);
		//			return null;
		//		}

  //              return data;
  //          }
  //      }
		
		#endregion

		#region Object list methods for controllers
		/// <summary>
		/// Returns the history from a specific GUID.
		/// </summary>
		/// <param name="guid">The GUID for which to get the history.</param>
		/// <returns>History from a Sensor.</returns>
		//public static List<object> GetObjectValuesFromFile(string guid)
  //      {
			

  //          List<object> values = new List<object>();
  //          StreamReader sr = null;
  //          lock (Statistic.Instance)
  //          {
  //              try
  //              {
  //                  if (!File.Exists(FilePaths.PathOfAllValuesFile))
  //                  {
  //                      return null;
  //                  }
  //                  sr = new StreamReader(FilePaths.PathOfAllValuesFile);

  //                  // guid, value, date
  //                  while (!sr.EndOfStream)
  //                  {
  //                      string[] line = sr.ReadLine().Split(";");
  //                      if (line[1].Trim() == guid)
  //                      {
  //                          values.Add(new { Id = line[1].Trim(), Value = line[3], TimeStamp = line[0] });
  //                      }
  //                  }

  //              }
  //              catch
  //              {
  //                  return null;
  //              }
  //              finally { if (sr != null) sr.Close(); }

  //              return values;
  //          }
  //      }

        // For controllers.
		/// <summary>
		/// Returns the ID, Value, and the Timestamp for each GUID.
		/// </summary>
		/// <returns>ID, Value, and Timestamp.</returns>
		//public static List<object> GetLastObjectValuesForEveryGUIDShort()
  //      {
  //          List<object> values = new List<object>();
  //          StreamReader sr = null;

  //          lock (Statistic.Instance)
  //          {
  //              try
  //              {
  //                  if (!File.Exists(FilePaths.PathOfAllValuesFile))
  //                  {
  //                      return null;
  //                  }
  //                  sr = new StreamReader(FilePaths.PathOfAllValuesFile);

  //                  // guid, value, date
  //                  while (!sr.EndOfStream)
  //                  {
  //                      string[] line = sr.ReadLine().Split(";");
  //                      if (!values.Contains(line[1].ToString().Trim()))
  //                      {
  //                          Console.WriteLine(line[1]);
  //                          values.Add(new { Id = line[1].Trim(), Value = line[3], Timestamp = line[0] });
  //                      }
  //                  }

  //              }
  //              catch
  //              {
  //                  return null;
  //              }
  //              finally { if (sr != null) sr.Close(); }

  //              return values;
  //          }
  //      }

		/// <summary>
		/// Returns the last object for a specific GUID.
		/// </summary>
		/// <param name="guid">The GUID for which to get the last object.</param>
		/// <returns>Last object for GUID.</returns>
		//public static object GetLastObjectDataForSpecificGUID(string guid)
		//{
		//	lock (Statistic.Instance)
		//	{
		//		if (!File.Exists(FilePaths.PathOfNewestValuesFile))
		//		{
		//			return null;
		//		}

		//		using (StreamReader sr = new StreamReader(FilePaths.PathOfNewestValuesFile))
		//		{
		//			while (!sr.EndOfStream)
		//			{
		//				string[] line = sr.ReadLine().Split(";");
		//				if (guid.Trim() == line[0].Trim())
		//				{
		//					return new { Id = line[0], Room = line[1], Value = line[2], Unit = line[3], Type = line[4] };
		//				}
		//			}
		//		}

		//		return null;
		//	}
		//}

		/// <summary>
		/// Gets the last object data for all GUIDs. This is to help the webapp initialize all sensors.
		/// </summary>
		/// <returns>List of objects for each Sensor.</returns>
		//public static List<object> GetLastObjectDataForAllGUIDs()
		//{
		//	List<object> data = new List<object>();

		//	lock (Statistic.Instance)
		//	{
		//		try
		//		{
		//			if (!File.Exists(FilePaths.PathOfNewestValuesFile))
		//			{
		//				return null;
		//			}

		//			using (StreamReader sr = new StreamReader(FilePaths.PathOfNewestValuesFile))
		//			{
		//				while (!sr.EndOfStream)
		//				{
		//					string[] line = sr.ReadLine().Split(";");
		//					data.Add(new { Id = line[0].Trim(), Room = line[1], Value = line[2], Unit = line[3], Type = line[4] });
		//				}
		//			}
		//		}
		//		catch
		//		{
		//			return null;
		//		}

		//		return data;
		//	}
		//}

		#endregion


	}
}