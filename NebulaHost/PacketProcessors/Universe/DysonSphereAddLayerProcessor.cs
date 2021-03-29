﻿using NebulaModel.Attributes;
using NebulaModel.Networking;
using NebulaModel.Packets.Universe;
using NebulaModel.Logger;
using NebulaModel.DataStructures;
using NebulaModel.Packets.Processors;
using NebulaWorld.Universe;

namespace NebulaHost.PacketProcessors.Universe
{
    [RegisterPacketProcessor]
    public class DysonSphereAddLayerProcessor : IPacketProcessor<DysonSphereAddLayerPacket>
    {
        private PlayerManager playerManager;

        public DysonSphereAddLayerProcessor()
        {
            playerManager = MultiplayerHostSession.Instance.PlayerManager;
        }

        public void ProcessPacket(DysonSphereAddLayerPacket packet, NebulaConnection conn)
        {
            Log.Info($"Processing DysonSphere Add Layer notification for system {GameMain.data.galaxy.stars[packet.StarIndex].name} (Index: {GameMain.data.galaxy.stars[packet.StarIndex].index})");
            Player player = playerManager.GetPlayer(conn);
            if (player != null)
            {
                playerManager.SendPacketToOtherPlayers(packet, player);
                DysonSphere_Manager.IncomingDysonSpherePacket = true;
                GameMain.data.dysonSpheres[packet.StarIndex]?.AddLayer(packet.OrbitRadius, DataStructureExtensions.ToUnity(packet.OrbitRotation), packet.OrbitAngularSpeed);
                DysonSphere_Manager.IncomingDysonSpherePacket = false;
            }
        }
    }
}