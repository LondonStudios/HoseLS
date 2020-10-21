using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Server : BaseScript
    {
        public Server()
        {

            EventHandlers["Server:SyncAtOffset"] += new Action<Vector3, Vector3, float, float>((coords, offset, heading, pitch) =>
            {
                TriggerClientEvent("Client:SyncAtOffset", coords, offset, heading, pitch);
            });

            EventHandlers["Server:DripFromHose"] += new Action<Vector3, float>((coords, heading) =>
            {
                TriggerClientEvent("Client:DripFromHose", coords, heading);
            });

            // Types: 1 = Add, 2 = Remove, 3 = Update Pitch
            EventHandlers["Server:SyncEntityLoop"] += new Action<int, float, int>((netId, pitch, type) =>
            {
                TriggerClientEvent("Client:SyncEntityLoop", netId, pitch, type);
            });
        }
    }
}
