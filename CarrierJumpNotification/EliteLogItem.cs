using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace CarrierJumpNotification
{
    public class CarrierJumpData
    {
        public DateTime JumpSequenceStart { get; set; }
        public string CarrierID { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }
        public string CurrentSystem { get; set; }
        public string TargetSystem { get; set; }
    }

    public class EliteLogParser
    {
        public static CarrierJumpData PullFromLog(string logPath)
        {
            CarrierJumpData jumpInfo = new CarrierJumpData();

            if (!File.Exists(logPath))
                return null;

            List<EliteLogItem> logData = new List<EliteLogItem>();


            var filestream = new System.IO.FileStream(logPath,
                                         System.IO.FileMode.Open,
                                         System.IO.FileAccess.Read,
                                         System.IO.FileShare.ReadWrite);
            var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 1024);
            string logItem;
            while ((logItem = file.ReadLine()) != null)
            {
                EliteLogItem logObject;
                try
                {
                    logObject = JsonConvert.DeserializeObject<EliteLogItem>(logItem);
                }
                catch { continue; }

                if (logObject != null && logObject.isImportant)
                {
                    logData.Add(logObject);
                }
            }

            logData.Sort((x, y) => DateTime.Compare(y.timestamp, x.timestamp));

            EliteLogItem tmp;

            tmp = logData.Find(x => x.isJumpRequest);

            if (tmp == null)
                return null;
#if !DEBUG
            TimeSpan howOldJump = DateTime.UtcNow.Subtract(tmp.timestamp);

            if (howOldJump.TotalMinutes > 16)
                return null;
#endif

            jumpInfo.JumpSequenceStart = tmp.timestamp;
            jumpInfo.CarrierID = tmp.CarrierID;
            jumpInfo.TargetSystem = tmp.SystemName;

            tmp = logData.Find(x => x.isCarrierData && x.CarrierID == jumpInfo.CarrierID);
            if (tmp != null)
            {
                jumpInfo.Name = tmp.Name;
                jumpInfo.Callsign = tmp.Callsign;
            }

            tmp = logData.Find(x => x.isCarrierDocked && x.StationName == jumpInfo.Callsign);
            if (tmp != null)
            {
                jumpInfo.CurrentSystem = tmp.StarSystem;
            }

            return jumpInfo;
        }

        public class EliteLogItem
        {
            public DateTime timestamp { get; set; }
            [JsonProperty("event")]
            public string eventType { get; set; }
            public string CarrierID { get; set; }
            public string Callsign { get; set; }
            public string Name { get; set; }
            public string SystemName { get; set; }
            public string StationName { get; set; }
            public string StationType { get; set; }
            public string StarSystem { get; set; }

            public bool isCarrierDocked { get { return (eventType == "Docked" && StationType == "FleetCarrier"); } }
            public bool isJumpRequest { get { return (eventType == "CarrierJumpRequest"); } }
            public bool isCarrierData { get { return (eventType == "CarrierStats"); } }

            public bool isImportant { get { return (isJumpRequest || isCarrierDocked || isCarrierData); } }
        }
    }
}
