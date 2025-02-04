﻿using PolarisServer.Database;
using PolarisServer.Packets.PSOPackets;
using PolarisServer.Party;

namespace PolarisServer.Packets.Handlers
{
    [PacketHandlerAttr(0x11, 0x4)]
    public class StartGame : PacketHandler
    {
        #region implemented abstract members of PacketHandler

        public override void HandlePacket(Client context, byte flags, byte[] data, uint position, uint size)
        {
            var reader = new PacketReader(data, position, size);
            var charId = reader.ReadUInt32();

            if (context.User == null)
                return;

            if (context.Character == null) // On character create, this is already set.
            {
                using (var db = new PolarisEf())
                {
                    var character = db.Characters.Find((int)charId);

                    if (character == null || character.Player.PlayerId != context.User.PlayerId)
                        return;

                    context.Character = character;
                }

            }

            // Initialize you in an empty party
            PartyManager.Instance.CreateNewParty(context);

            // Transition to the loading screen
            context.SendPacket(new NoPayloadPacket(0x3, 0x4));

            // TODO Set area, Set character, possibly more. See PolarisLegacy for more.
        }

        #endregion
    }

    [PacketHandlerAttr(0x03, 0x03)]
    public class InitialLoad : PacketHandler
    {
        // Ninji note: 3-3 may not be the correct place to do this
        // Once we have better state tracking, we should make sure that
        // 3-3 only does anything at the points where the client is supposed
        // to be sending it, etc etc

        // This seems to only ever be called once after logging in, yet is also handled by 11-3E in other places
        // Moved the actual handling into 11-3E until I can actually confirm this
        // Just insantiate a new CharacterSpawn and push it through until then
        // - Kyle

        #region implemented abstract members of PacketHandler

        public override void HandlePacket(Client context, byte flags, byte[] data, uint position, uint size)
        {
            // Set Player ID
            var setPlayerId = new PacketWriter();
            setPlayerId.WritePlayerHeader((uint)context.User.PlayerId);
            context.SendPacket(6, 0, 0, setPlayerId.ToArray());

            // Spawn Player
            new CharacterSpawn().HandlePacket(context, flags, data, position, size);
        }

        #endregion
    }

    [PacketHandlerAttr(0x03, 0x10)]
    public class DoItMaybe : PacketHandler
    {
        #region implemented abstract members of PacketHandler

        public override void HandlePacket(Client context, byte flags, byte[] data, uint position, uint size)
        {
            if (context.User == null || context.Character == null)
                return;

            context.SendPacket(new NoPayloadPacket(0x03, 0x23));
        }

        #endregion
    }
}