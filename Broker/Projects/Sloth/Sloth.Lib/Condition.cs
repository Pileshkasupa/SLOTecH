using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sloth.Lib
{
	public class Condition
	{
		public string Id {  get; set; }
		public string Operator { get; set; }
		public int Value { get; set; }

		public Condition(string raw) 
		{
			Console.WriteLine(raw);
			if (raw.Contains(">"))
			{
				string[] split = raw.Split(">");
				Operator = ">";
				Id = split[0];
				Value = Convert.ToInt32(split[1]);
			}
			else if (raw.Contains("<"))
			{
				string[] split = raw.Split("<");
				Operator = "<";
				Id = split[0];
				Value = Convert.ToInt32(split[1]);

			}
			else if (raw.Contains("="))
			{
				string[] split = raw.Split("=");
				Operator = "=";
				Id = split[0];
				Value = Convert.ToInt32(split[1]);

			}
			//else if (raw.Contains("!="))
			//{
			//	string[] split = raw.Split("!=");
			//	Operator = "!=";
			//	Id = split[0];
			//	Value = Convert.ToInt32(split[1]);

			//}
		}

	}
}
