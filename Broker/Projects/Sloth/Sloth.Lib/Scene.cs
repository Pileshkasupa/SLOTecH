using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sloth.Lib
{
	public class Scene
	{
		public string Raw {  get; set; }
		public string Title {  get; set; }
		public List<Condition> Conditions { get; set; }
		public List<Message> Messages { get; set; }

		public Scene(string raw) 
		{
			Console.WriteLine(raw);
			Conditions = new List<Condition>();
			Messages = new List<Message>();
			Raw = raw;
			string[] columnSplitRaw = raw.Split(':');
			Title = columnSplitRaw[0];
			string[] conditions = columnSplitRaw[1].Split('?');
			string conditionsString = conditions[0];
			string messagesString = conditions[1];

			string[] andSignSplitStringConditions = conditionsString.Split('&');
			string[] andSignSplitStringMessages = messagesString.Split('&');

			Console.WriteLine("Title:" + Title);
			Console.WriteLine("Conditions:" +  conditionsString);
			Console.WriteLine("Messages:" + messagesString);

			foreach (string item in andSignSplitStringConditions)
			{
				Conditions.Add(Factory.CreateCondition(item));
			}

			foreach (string item in andSignSplitStringMessages)
			{
				Messages.Add(Translation.CSVMessageToMessage(item));
			}
		}

		public Scene() { } // For JSON Serialisation

		public new string ToString()
		{
			return Raw;
		}
	}
}
