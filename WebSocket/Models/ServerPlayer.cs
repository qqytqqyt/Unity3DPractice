using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.WebSocket.Models
{
    public class ServerPlayer
    {
        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public float RotationY { get; set; }

        public int PlayerId { get; set; }
    }
}
