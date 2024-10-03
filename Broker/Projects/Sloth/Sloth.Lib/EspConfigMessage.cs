using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sloth.Lib
{
	public class EspConfigMessage
	{
		public string EspGuid
		{
			get; 
			set;
		}
		public int IntervalOfMeasurementInSeconds
		{
			get; 
			set;
		}

		public EspConfigMessage(string espGuid, int intervalOfMeasurementInSeconds) 
		{
			EspGuid = espGuid;
            IntervalOfMeasurementInSeconds = intervalOfMeasurementInSeconds;
		}


		public string ToCSV()
		{
			return $"{EspGuid};{IntervalOfMeasurementInSeconds}";
		}
	}
}
