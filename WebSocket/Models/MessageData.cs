using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.WebSocket.Models
{


    public abstract class MessageData
    {
        [JsonIgnore]
        public virtual string ClassName { get; set; }

        [JsonProperty("playerId")]
        public int PlayerId { get; set; }
    }

    public class RoomMessageData : MessageData
    {
        [JsonIgnore]
        public override string ClassName
        {
            get { return "Playground"; }
        }

        [JsonProperty("roomId")]
        public int RoomId { get; set; }
    }

    public class PositioningMessageData : MessageData
    {
        [JsonIgnore]
        public override string ClassName
        {
            get { return "Playground"; }
        }

        [JsonProperty("roomId")]
        public int RoomId { get; set; }

        [JsonProperty("position")]
        public PositionInfo Position { get; set; }

        public class PositionInfo
        {
            public PositionInfo(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }
    }

    public class MovementMessageData : MessageData
    {
        [JsonIgnore]
        public override string ClassName
        {
            get { return "Playground"; }
        }

        [JsonProperty("roomId")]
        public int RoomId { get; set; }

        [JsonProperty("horizontal")]
        public float Horizontal { get; set; }

        [JsonProperty("vertical")]
        public float Vertical { get; set; }

        [JsonProperty("mousePositionX")]
        public float MousePositionX { get; set; }

        [JsonProperty("mousePositionY")]
        public float MousePositionY { get; set; }

        [JsonProperty("mousePositionZ")]
        public float MousePositionZ { get; set; }
    }

    public class ShootMessageData : MessageData
    {
        [JsonIgnore]
        public override string ClassName
        {
            get { return "Playground"; }
        }

        [JsonProperty("roomId")]
        public int RoomId { get; set; }
    }
}
