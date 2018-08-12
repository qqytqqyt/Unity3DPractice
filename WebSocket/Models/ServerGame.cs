using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.WebSocket.Models
{
    public class ServerGame
    {
        public ServerPlayer Player1 { get; set; }

        public ServerPlayer Player2 { get; set; }

        public int RoomId { get; set; }

        public int ControllerPlayerId { get; set; }

        public ServerPlayer GetControllerPlayer()
        {
            return Player1.PlayerId == GameStartUiManager.PlayerId ? Player1 : Player2;
        }
        public ServerPlayer GetOtherPlayer()
        {
            return Player1.PlayerId == GameStartUiManager.PlayerId ? Player2 : Player1;
        }
    }
}
