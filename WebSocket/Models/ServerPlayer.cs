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

        public ServerMovement Movement { get; set; }

        public int PlayerId { get; set; }

        public bool IsShooting { get; set; }
    }
    
    public class ServerMovement
    {
        public float Horizontal { get; set; }

        public float Vertical { get; set; }

        public float MousePositionX { get; set; }

        public float MousePositionY { get; set; }

        public float MousePositionZ { get; set; }
    }
}
