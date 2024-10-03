using Newtonsoft.Json.Linq;
using Sloth.Lib;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

public class SlothLogger
{
    private readonly object fileWritelock = new object();
    DatabaseHelper _databaseHelper = Factory.CreateDatabaseHelper();
    public SlothLogger()
    {

    }

    #region Console loggers
    /// <summary>
    /// Logs a string on the console with the Foregroundcolor: Cyan
    /// </summary>
    /// <param name="message"></param>
    public static void ConsoleLogMessageInformation(string message)
    {
        lock (Console.Out)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string logEntry = $"{DateTime.Now}: {message}";
            Console.WriteLine(logEntry);
        }
    }
    /// <summary>
    /// Logs status message on the console Info=Green, Warning=Yellow, Error=Red, System=DarkMagenta
    /// </summary>
    /// <param name="statusMessage"></param>
    public static void ConsoleLogStatusMessage(StatusMessage statusMessage)
    {
        string logEntry;

        switch (statusMessage.Topic)
        {
            case "Info":
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case "Warning":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "Error":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case "System":
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }

        lock (Console.Out)
        {
            logEntry = $"{DateTime.Now}: {statusMessage.Message}";

            Console.WriteLine(logEntry);
        }
	    Console.ResetColor();
    }

    public static void ConsoleLogDatabaseCreated(string s)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(s);
	    Console.ResetColor();
    }

    public static void ConsoleLogDatabaseAdd(string s)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(s);
	    Console.ResetColor();
    }
    public static void ConsoleLogDatabaseDelete(string s)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(s);
	    Console.ResetColor();
    }
    #endregion

    //   #region File loggers
    //   /// <summary>
    //   /// Saves the message to /home/pi/SlothLogs/slothAllValues.txt
    //   /// </summary>
    //   /// <param name="message"></param>
    //   public void SaveMessageToAllValuesFile(Message message)
    //   {
    //       string logEntry = $"{DateTime.Now};{message.Id};{message.Room};{message.Value};{message.Unit};{message.Type}";

    //       lock (fileWritelock)
    //       {
    //           ShortenFile(FilePaths.PathOfAllValuesFile);

    //           List<string> fileLines = new List<string>(File.ReadAllLines(FilePaths.PathOfAllValuesFile));

    //           fileLines.Insert(0, logEntry);

    //           File.WriteAllLines(FilePaths.PathOfAllValuesFile, fileLines);
    //       }
    //   }

    //   /// <summary>
    //   /// Saves the message to /home/pi/SlothLogs/slothNewestValues.txt
    //   /// </summary>
    //   /// <param name="message"></param>
    //   public void SaveMessageToNewestValuesFile(Message message)
    //   {
    //       lock (fileWritelock)
    //       {
    //           string[] lines = File.ReadAllLines(FilePaths.PathOfNewestValuesFile);
    //           int lineIndex = Array.FindIndex(lines, line => line.Split(";")[0] == message.Id);

    //           if (lineIndex != -1)
    //           {
    //               lines[lineIndex] = message.ToCSV();
    //           }
    //           else
    //           {
    //               lines = lines.Concat(new string[] { message.ToCSV() }).ToArray();
    //           }

    //           File.WriteAllLines(FilePaths.PathOfNewestValuesFile, lines);
    //       }
    //   }

    //   /// <summary>
    //   /// Saves the incoming status message into: /home/pi/SlothLogs/slothLogs.txt
    //   /// </summary>
    //   /// <param name="statusMessage"></param>
    //   public void SaveStatusMessageToFile(StatusMessage statusMessage)
    //   {
    //       lock (fileWritelock)
    //       {
    //           ShortenFile(FilePaths.PathOfLogFile);

    //           List<string> fileLines = new List<string>(File.ReadAllLines(FilePaths.PathOfLogFile));

    //           fileLines.Insert(0, $"{DateTime.Now}: {statusMessage.ToCSV()}");

    //           File.WriteAllLines(FilePaths.PathOfLogFile, fileLines);
    //       }
    //   }

    ///// <summary>
    ///// Saves the current Connected Clients into: /home/pi/SlothLogs/slothGUIDs.txt
    ///// </summary>
    ///// <param name="connectedClients"></param>
    //public void SaveConnectedClientsToFile(List<string> connectedClients)
    //{
    //	try
    //	{
    //		using (StreamWriter writer = new StreamWriter(FilePaths.PathOfGUIDFile, true))
    //		{
    //               Task.Run(() =>
    //               {
    //                   foreach (string client in connectedClients)
    //                   {
    //                       writer.WriteLineAsync($"{client}");
    //                   }
    //               });

    //		}
    //	}
    //	catch (Exception ex)
    //	{
    //		Console.WriteLine($"Error writing connected clients to file: {ex.Message}");
    //	}
    //}

    ///// <summary>
    ///// Returns the Scenes stored in ScenesFile
    ///// </summary>
    ///// <returns>List of Scenes</returns>
    //internal List<Scene> GetScenesFromFile()
    //   {
    //       lock (fileWritelock)
    //       {
    //           List<Scene> scenes = new List<Scene>();
    //           if (File.Exists(FilePaths.PathOfScenesFile))
    //           {
    //               scenes.Clear();
    //               string[] lines = File.ReadAllLines(FilePaths.PathOfScenesFile);
    //               foreach (string line in lines)
    //               {
    //                   scenes.Add(new Scene(line.ToString()));
    //               }
    //           }
    //           return scenes;
    //       }
    //   }

    //   /// <summary>
    //   /// Saves the scenes in SceneFile
    //   /// </summary>
    //   /// <param name="scenes"></param>
    //internal void WriteScenesToFile(List<Scene> scenes)
    //{
    //    lock (fileWritelock)
    //    {
    //        foreach (Scene scene in scenes)
    //        {
    //            string logEntry = $"{scene.Raw}";
    //            string[] lines = File.ReadAllLines(FilePaths.PathOfScenesFile);
    //            int lineIndex = Array.FindIndex(lines, line => line.Split(";")[0] == scene.Title);

    //            if (lineIndex != -1)
    //            {
    //                lines[lineIndex] = scene.ToString();
    //            }
    //            else
    //            {
    //                lines = lines.Concat(new string[] { scene.ToString() }).ToArray();
    //            }

    //            File.WriteAllLines(FilePaths.PathOfScenesFile, lines);
    //        }
    //    }
    //}

    //   /// <summary>
    //   /// Saves the Scene in SceneFile
    //   /// </summary>
    //   /// <param name="scene"></param>
    //internal void WriteSceneToFile(Scene scene)
    //{
    //    string logEntry = $"{scene.Raw}";

    //    lock (fileWritelock)
    //    {
    //        string[] lines = File.ReadAllLines(FilePaths.PathOfScenesFile);
    //        int lineIndex = Array.FindIndex(lines, line => line.Split(";")[0] == scene.Title);

    //        if (lineIndex != -1)
    //        {
    //            lines[lineIndex] = scene.ToString();
    //        }
    //        else
    //        {
    //            lines = lines.Concat(new string[] { scene.ToString() }).ToArray();
    //        }

    //        File.WriteAllLines(FilePaths.PathOfScenesFile, lines);
    //    }
    //}

    //   /// <summary>
    //   /// Removes the Scene from the File and rewrites the File
    //   /// </summary>
    //   /// <param name="scene"></param>
    //internal void RemoveSceneFromFile(Scene scene)
    //{
    //    //List<Scene> list = GetScenesFromFile();
    //    List<Scene> list = _databaseHelper.GetScenesFromDatabase();

    //    foreach (Scene item in list.ToList())
    //    {
    //        if (item.Title == scene.Title)
    //        {
    //            list.Remove(item);
    //        }
    //    }
    //    using (FileStream fs = File.Open(FilePaths.PathOfScenesFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
    //    {
    //        lock (fs)
    //        {
    //            fs.SetLength(0);
    //            fs.Close();

    //            foreach (var item in list)
    //            {
    //                _databaseHelper.AddDataTo_Scenes(item.Title, item.Raw);
    //            }

    //            WriteScenesToFile(list);
    //        }
    //    }
    //}
    //   #endregion

    #region misc
    /// <summary>
    /// Shortens the file if the lines exceed the maximum amount of 150 lines
    /// </summary>
    /// <param name="filePath"></param>
    public void ShortenFile(string filePath)
    {
        int length;

        lock (fileWritelock)
        {
            length = File.ReadAllLines(filePath).Length;
        }

        string tempFilePath = Path.GetTempFileName();

        if (length >= LogConfig.MAX_LINES)
        {
            lock (fileWritelock)
            {
                int half = length - LogConfig.REMOVE_LINES_COUNT;
                using (StreamReader sr = new StreamReader(filePath))
                using (StreamWriter sw = new StreamWriter(tempFilePath))
                {
                    for (int i = 0; i < half; i++)
                    {
                        sw.WriteLine(sr.ReadLine());
                    }
                }
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }
        }
    }

    #endregion
}
