using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace Sloth.Lib
{
    public class DatabaseHelper
    {
        //private static string connectionString = @"Data Source=C:\Users\pacyg\source\repos\Sloth\Sloth.Lib\SlothLib.db;";
        //private static string connectionString = @"Data Source=C:\Users\marti\source\repos\Sloth\Sloth.Lib\SlothLib.db;";
        private static string connectionString = @"Data Source=/home/pi/SlothLogs/SlothLib.db;";

        private static DatabaseHelper? instance = null;

        private DatabaseHelper()
        {
            DatabaseHelper.InitializeDatabase();
        }
        public static DatabaseHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseHelper();
                    SlothLogger.ConsoleLogDatabaseCreated("Datenbank verbunden");
                }
                return instance;
            }
        }
		/// <summary>
		/// Creates an Instance of the database
		/// </summary>
		private static void InitializeDatabase()
		{
			if (!File.Exists(@".db"))
			{
				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();

					string createMessagesTable = @"
                   CREATE TABLE IF NOT EXISTS Messages (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        identifier VARCHAR(255), 
                        room VARCHAR(255),
                        value INTEGER,
                        unit VARCHAR(50),
                        type VARCHAR(50),
                        timeStamp DATETIME,
                        messageType VARCHAR(255)
                    );";

					string createScenesTable = @"
                   CREATE TABLE IF NOT EXISTS Scenes (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        raw VARCHAR(255),
                        name VARCHAR(255)
                    );";

					string createMessageScenesTable = @"
                    CREATE TABLE IF NOT EXISTS MessagesScenes (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        identifier VARCHAR(255),
                        room VARCHAR(255),
                        value DOUBLE,
                        unit VARCHAR(50),
                        type VARCHAR(50),
                        messageType VARCHAR(50),
                        sceneName VARCHAR(255)
                    );";

					string createConditionsTable = @"
                    CREATE TABLE IF NOT EXISTS Conditions (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        value INTEGER,
                        operator VARCHAR(10),
                        guid VARCHAR(255),
                        sceneName VARCHAR(255)
                    );";

					string createStatusMessagesTable = @"
                    CREATE TABLE IF NOT EXISTS StatusMessages (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        guid VARCHAR(255),
                        topic VARCHAR(255),
                        message VARCHAR(255),
                        messageType VARCHAR(50),
                        timeStamp DATETIME
                    );";

					string createConnectedClientsTable = @"
                    CREATE TABLE IF NOT EXISTS ConnectedClients (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name VARCHAR(255)
                    );";

					// Create Tables
					using (var command = new SqliteCommand())
					{
						command.Connection = connection;

						command.CommandText = createMessagesTable;
						command.ExecuteNonQuery();

						command.CommandText = createScenesTable;
						command.ExecuteNonQuery();

						command.CommandText = createMessageScenesTable;
						command.ExecuteNonQuery();

						command.CommandText = createConditionsTable;
						command.ExecuteNonQuery();

						command.CommandText = createStatusMessagesTable;
						command.ExecuteNonQuery();

						command.CommandText = createConnectedClientsTable;
						command.ExecuteNonQuery();
					}

				}
			}
		}

        public string ReturnPath()
        {
            return connectionString;
        }

		#region Add Data
		/// <summary>
		/// Adds an entry into Message
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="room"></param>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="type"></param>
		public void AddDataTo_Message(string identifier, string room, double value, string unit, string type, string messageType, DateTime? timeStamp)
		{
			using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				using (SqliteCommand command = new SqliteCommand())
				{
					command.Connection = connection;
					command.CommandText = @"INSERT INTO Messages (identifier, room, value, unit, type, timeStamp, messageType)
                                    VALUES (@identifier, @room, @value, @unit, @type, @timeStamp, @messageType);";

                    
					command.Parameters.AddWithValue("@identifier", identifier);
					command.Parameters.AddWithValue("@room", room);
					command.Parameters.AddWithValue("@value", value);
					command.Parameters.AddWithValue("@unit", unit);
					command.Parameters.AddWithValue("@type", type);
                    if (timeStamp == null)
                    {
                        timeStamp = DateTime.MinValue;
                    }
					command.Parameters.AddWithValue("@timeStamp", timeStamp);
                    command.Parameters.AddWithValue("@messageType", messageType);

					// Execute the command
					command.ExecuteNonQuery();
                    command.Parameters.Clear();
					SlothLogger.ConsoleLogDatabaseAdd("Data added to Message");
				}
			}
		}


		/// <summary>
		/// Adds an entry into Scenes
		/// </summary>
		/// <param name="name"></param>
		/// <param name="raw"></param>
		public void AddDataTo_Scenes(string name, string raw)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"INSERT INTO Scenes (name, raw)
                                            VALUES (@name, @raw);";

                    command.Parameters.AddWithValue(@"name", name);
                    command.Parameters.AddWithValue(@"raw", raw);
                    
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    SlothLogger.ConsoleLogDatabaseAdd("Data added to Scenes");

                }
            }
        }

        /// <summary>
        /// Adds an entry into MessageScenees
        /// </summary>
        /// <param name="room"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="type"></param>
        /// <param name="messageType"></param>
        /// <param name="sceneName"></param>
        public void AddDataTo_MessagesScenes(string identifier, string room, double value, string unit, string type, string messageType, string sceneName)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"INSERT INTO MessagesScenes (identifier, room, value, unit, type, messageType, sceneName)
                                            VALUES (@identifier, @room, @value, @unit, @type, @messageType, @sceneName);";

                    command.Parameters.AddWithValue(@"identifier", identifier);
                    command.Parameters.AddWithValue(@"room", room);
                    command.Parameters.AddWithValue(@"value", value);
                    command.Parameters.AddWithValue(@"unit", unit);
                    command.Parameters.AddWithValue(@"type", type);
                    command.Parameters.AddWithValue(@"messageType", messageType);
                    command.Parameters.AddWithValue(@"sceneName", sceneName);

					command.ExecuteNonQuery();

					command.Parameters.Clear();

                    SlothLogger.ConsoleLogDatabaseAdd("Data added to MessageScenes");

                }
            }
        }

        /// <summary>
        /// Adds an entry into Condition
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="operator"></param>
        /// <param name="value"></param>
        /// <param name="sceneName"></param>
        public void AddDataTo_Condition(int value, string @operator, string guid, string sceneName)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"INSERT INTO Conditions (value, operator, guid ,sceneName)
                                            VALUES (@value, @operator, @guid ,@sceneName);";
                    

                    command.Parameters.AddWithValue(@"value", value);
                    command.Parameters.AddWithValue(@"operator", @operator);
                    command.Parameters.AddWithValue(@"guid", guid);
                    command.Parameters.AddWithValue(@"sceneName", sceneName);

					command.ExecuteNonQuery();

                    command.Parameters.Clear();

					SlothLogger.ConsoleLogDatabaseAdd("Data added to Condition");
                }
            }
        }

        /// <summary>
        /// Adds an entry into StatusMessage
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        public void AddDataTo_StatusMessage(string guid, string topic, string message, string messageType, DateTime? timeStamp)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"INSERT INTO StatusMessages (guid, topic, message, messageType, timeStamp)
                                            VALUES (@guid, @topic, @message, @messageType, @timeStamp);";

                    command.Parameters.AddWithValue(@"guid", guid);
					command.Parameters.AddWithValue(@"topic", topic);
                    command.Parameters.AddWithValue(@"message", message);
                    command.Parameters.AddWithValue(@"messageType", messageType);
					command.Parameters.AddWithValue(@"timeStamp", timeStamp);

					command.ExecuteNonQuery();

					command.Parameters.Clear();

					SlothLogger.ConsoleLogDatabaseAdd("Data added to StatusMessage");
                }
            }
        }

        /// <summary>
        /// Adds an entry into ConnectedCLients
        /// </summary>
        /// <param name="client"></param>
        public void AddDataTo_ConnectedClients(string client)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"INSERT INTO ConnectedClients (name)
                                            VALUES (@client);";

                    command.Parameters.AddWithValue(@"client", client);

					command.ExecuteNonQuery();

					command.Parameters.Clear();

					SlothLogger.ConsoleLogDatabaseAdd("Data added to ConnectedClients");
                }
            }
        }
        #endregion

        #region Delete Data
        /// <summary>
        /// Deletes an entry from the Message table
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="room"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="type"></param>
        public void DeleteDataFrom_Message(string identifier, string room, int value, string unit, string type, string messageType)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = $@"DELETE FROM Messages
                                            WHERE identifier = @identifier AND room = @room AND value = @value AND unit = @unit AND type = @type AND messageType = @messageType;";

                    command.Parameters.AddWithValue(@"identifier", identifier);
                    command.Parameters.AddWithValue(@"room", room);
                    command.Parameters.AddWithValue(@"value", value);
                    command.Parameters.AddWithValue(@"unit", unit);
                    command.Parameters.AddWithValue(@"type", type);
                    command.Parameters.AddWithValue(@"messageType", messageType);

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();

                    SlothLogger.ConsoleLogDatabaseDelete("Data deleted from Message");
                }
            }
        }

		/// <summary>
		/// Deletes an entry from the Scene table
		/// </summary>
		/// <param name="name"></param>
		public void DeleteDataFrom_Scenes(string name)
		{
			using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				using (SqliteCommand command = new SqliteCommand())
				{
					command.Connection = connection;
					command.CommandText = @"DELETE FROM Scenes
                                    WHERE name = @name";

					command.Parameters.AddWithValue("@name", name);

					// Execute the command
					command.ExecuteNonQuery();

					SlothLogger.ConsoleLogDatabaseDelete("Data deleted from Scenes");

					// Clear parameters before exiting the using block
					command.Parameters.Clear();
				}
			}
            DeleteDataFrom_Condition(name);
            DeleteDataFrom_MessageScenes(name);
		}


		/// <summary>
		/// Deleetes an entry from the Scene table
		/// </summary>
		/// <param name="sceneName"></param>
		public void DeleteDataFrom_MessageScenes(string sceneName)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"DELETE FROM MessagesScenes
                                            WHERE sceneName = @sceneName";

                    command.Parameters.AddWithValue(@"sceneName", sceneName);

					command.ExecuteNonQuery();

					command.Parameters.Clear();

                    SlothLogger.ConsoleLogDatabaseDelete("Data deleted from MessageScene");
                }
            }
        }

        /// <summary>
        /// Deletes an entry from the Condition table
        /// </summary>
        /// <param name="sceneName"></param>
        public void DeleteDataFrom_Condition(string sceneName)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"DELETE FROM Conditions
                                            WHERE sceneName = @sceneName";
                    command.Parameters.AddWithValue(@"sceneName", sceneName);

					command.ExecuteNonQuery();

					command.Parameters.Clear();

                    SlothLogger.ConsoleLogDatabaseDelete("Data deleted from Condition");
                }
            }
        }
        /// <summary>
        /// Deletes an entry from the StatusMessage table
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        public void DeleteDataFrom_StatusMessage(string topic, string message, string messageType)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"DELETE FROM StatusMessages
                                            WHERE topic = @topic AND message = @message AND @messageType = messageType";

                    command.Parameters.AddWithValue(@"topic", topic);
                    command.Parameters.AddWithValue(@"message", message);
                    command.Parameters.AddWithValue(@"messageType", messageType);

					command.ExecuteNonQuery();

					command.Parameters.Clear();
					SlothLogger.ConsoleLogDatabaseDelete("Data deleted from StatusMessage");
                }
            }
        }
        /// <summary>
        /// Removes the client from the Table
        /// </summary>
        /// <param name="name"></param>
		public void RemoveConnectedClientFromDatabase(string name)
		{
			using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				using (SqliteCommand command = new SqliteCommand())
				{
					command.Connection = connection;
					command.CommandText = @"DELETE FROM ConnectedClients
                                            WHERE name = @name";

					command.Parameters.AddWithValue("@name", name);

					// Execute the command
					command.ExecuteNonQuery();

					SlothLogger.ConsoleLogDatabaseDelete("Data deleted from ConnectedClients");

					// Clear parameters before exiting the using block
					command.Parameters.Clear();
				}
			}
		}
        #endregion

        #region Controller Queries

        /// <summary>
        /// Returns a list of objects each object has an identifier, value and datetime
        /// </summary>
        /// <returns></returns>
        public List<object> GetMessageObjectsFromDatabase()
        {
            List<object> values = new List<object>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                int i = 0;

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"SELECT * FROM Messages WHERE id=@id DESC";
                    command.Parameters.AddWithValue(@"id", i);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            int value = reader.GetInt32(reader.GetOrdinal("value"));
                            string dateTime = reader.GetDateTime(reader.GetOrdinal("timeStamp")).ToString("yyyy-MM-dd_HH-mm-ss");

                            values.Add(new { Id = identifier, Value = value, DateTime = dateTime });
                            i++;
                            command.Parameters.AddWithValue(@"id", i);
                            
                            command.ExecuteNonQuery();

                            command.Parameters.Clear();

                        }
                    }
                    SlothLogger.ConsoleLogDatabaseAdd("Returned Messages from Database");
                }
            }
            return values;
        }


        /// <summary>
        /// Returns a list of objects each object has an guid, messageType, message and dateTimem
        /// </summary>
        /// <returns></returns>
        public List<object> GetStatusMessagesObjectsFromDatabaseForControllers()
		{
			List<object> values = new List<object>();

			using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				using (SqliteCommand command = new SqliteCommand())
				{
					command.Connection = connection;
					command.CommandText = @"SELECT * FROM StatusMessages";

					using (SqliteDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							string guid = reader.GetString(reader.GetOrdinal("guid"));
							string message = reader.GetString(reader.GetOrdinal("message"));
							string messageType = reader.GetString(reader.GetOrdinal("messageType"));
							string dateTime = reader.GetDateTime(reader.GetOrdinal("timeStamp")).ToString("yyyy-MM-dd_HH-mm-ss");

							values.Add(new { Guid = guid, MessageType = messageType, Message = message, DateTime = dateTime });
						}
					}
					SlothLogger.ConsoleLogDatabaseAdd("Returned status messages from database");
				}
			}
			return values;
		}

		/// <summary>
		/// Returns the last Message for a specific guid
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public List<object> GetLastMessageObjectsForSpecificGUIDFromDatabase(string guid)
        {
            List<object> values = new List<object>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT identifier, room, value, unit, type, messageType
                                    FROM Messages 
                                    WHERE (identifier, timeStamp) IN 
                                    (SELECT identifier, MAX(timeStamp) as timeStamp
                                     FROM Messages 
                                     WHERE identifier = @identifier
                                     GROUP BY identifier);";

                    command.Parameters.AddWithValue("@identifier", guid);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            string room = reader.GetString(reader.GetOrdinal("room"));
                            double value = reader.GetInt32(reader.GetOrdinal("value"));
                            string unit = reader.GetString(reader.GetOrdinal("unit"));
                            string type = reader.GetString(reader.GetOrdinal("type"));
                            string messageType = reader.GetString(reader.GetOrdinal("messageType"));


                            values.Add(new { Id = identifier, Room = room, Value = value, Unit = unit, Type = type, MessageType = messageType});
							Console.WriteLine($"Component: {identifier};{room};{value};{unit};{type};{messageType}");
						}
					}
                }
            }
            
            SlothLogger.ConsoleLogDatabaseAdd("Returned Messages from Database");

            return values;
        }

        /// <summary>
        /// Returns the last Message for each guid
        /// </summary>
        /// <returns></returns>
        public List<object> GetLastMessageObjectDataForAllGUIDsFromDatabase()
        {
            List<object> values = new List<object>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    // Corrected SQL query to retrieve the latest message for each identifier
                    command.CommandText = @"SELECT identifier, room, value, unit, type, messageType, timeStamp
                                    FROM Messages 
                                    WHERE (identifier, timeStamp) IN 
                                    (SELECT identifier, MAX(timeStamp) as timeStamp
                                     FROM Messages 
                                     GROUP BY identifier);";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            string room = reader.GetString(reader.GetOrdinal("room"));
                            int value = reader.GetInt32(reader.GetOrdinal("value"));
                            string unit = reader.GetString(reader.GetOrdinal("unit"));
                            string type = reader.GetString(reader.GetOrdinal("type"));
							string timeStamp = reader.GetDateTime(reader.GetOrdinal("timeStamp")).ToString("yyyy-MM-dd_HH-mm-ss");
							string messageType = reader.GetString(reader.GetOrdinal("messageType"));

							values.Add(new { Id = identifier, Room = room, Value = value, Unit = unit, Type = type, TimeStamp = timeStamp, MessageType = messageType });
                            Console.WriteLine($"All: {identifier};{room};{value};{unit};{type};{timeStamp};{messageType}");
                        }
                    }
                }
            }

            SlothLogger.ConsoleLogDatabaseAdd("Returned Messages from Database");

            return values;
        }

        /// <summary>
        /// Returns the history of a specific guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public List<object> GetMessageObjectDataForCertainGUIDFromDatabase(string guid)
        {
            List<object> values = new List<object>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    // Retrieve all entries for a certain GUID
                    command.CommandText = @"SELECT identifier, room, value, unit, type, messageType, timeStamp
                                    FROM Messages 
                                    WHERE identifier = @identifier;";

                    command.Parameters.AddWithValue("@identifier", guid);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            string room = reader.GetString(reader.GetOrdinal("room"));
                            int value = reader.GetInt32(reader.GetOrdinal("value"));
                            string unit = reader.GetString(reader.GetOrdinal("unit"));
                            string type = reader.GetString(reader.GetOrdinal("type"));
                            string timeStamp = reader.GetDateTime(reader.GetOrdinal("timeStamp")).ToString("yyyy-MM-dd_HH-mm-ss");
                            string messageType = reader.GetString(reader.GetOrdinal("messageType"));

							values.Add(new { Id = identifier, Room = room, Value = value, Unit = unit, Type = type, TimeStamp = timeStamp, MessageType = messageType });
                        }
                    }
                }
            }

            SlothLogger.ConsoleLogDatabaseAdd("Returned Messages from Database");

            return values;
        }
        #endregion

        #region Get Data
         /// <summary>
         /// Returns a list of the current scenes in the database
         /// </summary>
         /// <returns></returns>
        public List<Scene> GetScenesFromDatabase()
        {
            List<Scene> scenes = new List<Scene>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                int i =0;


				using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"SELECT * FROM Scenes";
					using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string raw = reader.GetString(reader.GetOrdinal("raw"));
                            Console.WriteLine(raw);
                            scenes.Add(Factory.CreateScene(raw));
							i++;

						}
					}
                    SlothLogger.ConsoleLogDatabaseAdd("Returned Scenes from Database");
                }
            }
            return scenes;
        }

        /// <summary>
        /// Returns a list of the current messages in the database
        /// </summary>
        /// <returns></returns>
        public List<Message> GetMessagesFromDatabase()
        {
            List<Message> messages = new List<Message>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;
					command.CommandText = @"SELECT * FROM Messages";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            string room = reader.GetString(reader.GetOrdinal("room"));
                            int value = reader.GetInt32(reader.GetOrdinal("value"));
                            string unit = reader.GetString(reader.GetOrdinal("unit"));
                            string type = reader.GetString(reader.GetOrdinal("type"));
                            string messageType = reader.GetString(reader.GetOrdinal("messageType"));

                            messages.Add(Factory.CreateMessage(identifier, room, value, unit, type, messageType, null));
                        }
                    }
                    SlothLogger.ConsoleLogDatabaseAdd("Returned Messages from Database");
                }
            }
            return messages;
        }

        /// <summary>
        /// Returns a list of reply messages for the specific sceneName
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public List<Message> GetMessagesForSceneFromDatabase(string sceneName)
        {
            List<Message> messages = new List<Message>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                int i = 0;

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT * FROM MessagesScenes ms JOIN Scenes s ON s.name = ms.sceneName WHERE ms.id=@id AND ms.sceneName=@sceneName";
                    command.Parameters.AddWithValue(@"id", i);
                    command.Parameters.AddWithValue(@"sceneName", sceneName);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            command.Parameters.AddWithValue(@"sceneName", sceneName);
                            i++;
                            command.Parameters.AddWithValue(@"id", i);

                            string identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            string room = reader.GetString(reader.GetOrdinal("room"));
                            int value = reader.GetInt32(reader.GetOrdinal("value"));
                            string unit = reader.GetString(reader.GetOrdinal("unit"));
                            string type = reader.GetString(reader.GetOrdinal("type"));
                            string messageType = reader.GetString(reader.GetOrdinal("messageType"));

                            messages.Add(Factory.CreateMessage(identifier, room, value, unit, type, messageType, null));
							command.Parameters.Clear();
						}
					}
                    SlothLogger.ConsoleLogDatabaseAdd("Returned Messages from Database");
                }
            }
            return messages;
        }

        /// <summary>
        /// Returns a list of conditions for the specific sceneName
        /// </summary>
        /// <param name="sceneNameForDB"></param>
        /// <returns></returns>
        public List<Condition> GetConditionForSceneFromDatabase(string sceneNameForDB)
        {
            List<Condition> messages = new List<Condition>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                int i = 0;

                using (SqliteCommand command = new SqliteCommand())
                {
					command.Connection = connection;

					command.CommandText = @"SELECT * FROM Conditions c JOIN Scenes s ON c.sceneName=s.name WHERE c.id=@id AND c.sceneName=@sceneName";
                    command.Parameters.AddWithValue(@"id", i);
                    command.Parameters.AddWithValue(@"sceneName", sceneNameForDB);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            command.Parameters.AddWithValue(@"sceneName", sceneNameForDB);
                            i++;
                            command.Parameters.AddWithValue(@"id", i);

                            string guid = reader.GetString(reader.GetOrdinal("guid"));
                            string @operator = reader.GetString(reader.GetOrdinal("operator"));
                            int value = reader.GetInt32(reader.GetOrdinal("value"));
                            string sceneName = reader.GetString(reader.GetOrdinal("sceneName"));

                            string raw = "";
							command.Parameters.Clear();

							messages.Add(Factory.CreateCondition(raw));
                        }
                    }
                    SlothLogger.ConsoleLogDatabaseAdd("Returned Conditions from Database");
                }
            }
            return messages;
        }

        /// <summary>
        /// Returns a list of the current connected clients
        /// </summary>
        /// <returns></returns>
		internal List<string> GetConnectedClientsFromDatabase()
		{
			List<string> guids = new List<string>();

			using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
				connection.Open();

				using (SqliteCommand command = new SqliteCommand())
				{
					command.CommandText = @"SELECT * FROM ConnectedClients";
					command.Connection = connection;

					using (SqliteDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							string id = reader.GetString(reader.GetOrdinal("id"));
							string name = reader.GetString(reader.GetOrdinal("name"));

							guids.Add(name);
						}
					}
					SlothLogger.ConsoleLogDatabaseAdd("Returned connected GUIDs from Database");
				}
			}
			return guids;
		}


        /// <summary>
        /// Returns a list of the current stored StatusMessages
        /// </summary>
        /// <returns></returns>
        public List<StatusMessage> GetStatusMessages()
        {
            List<StatusMessage> statusMessages = new List<StatusMessage>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand())
                {
                    command.CommandText = @"SELECT * FROM StatusMessages";
                    command.Connection = connection;

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader.GetString(reader.GetOrdinal("id"));
                            string guid = reader.GetString(reader.GetOrdinal("guid"));
                            string topic = reader.GetString(reader.GetOrdinal("topic"));
                            string message = reader.GetString(reader.GetOrdinal("message"));
                            string messageType = reader.GetString(reader.GetOrdinal("messageType"));
                            string dateTime = reader.GetString(reader.GetOrdinal("timeStamp"));

                            DateTime res;

                            DateTime.TryParse(dateTime, out res);

                            DateTime dateValue;
                            CultureInfo culture = CultureInfo.CreateSpecificCulture("de-AT");

                            dateValue = DateTime.Parse(dateTime, culture);

                            statusMessages.Add(Factory.CreateStatusMessage(topic, message, messageType, id, res));
                        }
                    }
                    SlothLogger.ConsoleLogDatabaseAdd("Returned connected GUIDs from Database");
                }
            }
            return statusMessages;
        }

		#endregion
	}
}
