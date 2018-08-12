using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assets.Scripts.WebSocket.Models
{
    public class ServerMessage
    {
        public static string GetMessageType(string message)
        {
            var index = message.IndexOf(':');
            if (index <= 0)
                return string.Empty;

            return message.Substring(0, index);
        }

        public static string GetMessageContent(string message)
        {
            var index = message.IndexOf(':');
            if (index < 0)
                return message;

            return message.Substring(index + 1);
        }

        public const string PlayerSpawnType = "playerSpawn";

        public const string PlayerPositioningType = "playerRelocate";
    }

    public class SocketMessage
    {
        [JsonProperty("class")]
        public string ClassName
        {
            get { return Data.ClassName; }
        }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public MessageData Data { get; set; }
    }
}
