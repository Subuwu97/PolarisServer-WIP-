﻿using System.IO;
using System.Linq;

namespace PolarisServer.Packets.Handlers
{
    [PacketHandlerAttr(0x07, 0x00)]
    public class ChatHandler : PacketHandler
    {
        #region implemented abstract members of PacketHandler

        public override void HandlePacket(Client context, byte flags, byte[] data, uint position, uint size)
        {
            if (context.Character == null)
                return;

            var reader = new PacketReader(data, position, size);
            reader.BaseStream.Seek(0xC, SeekOrigin.Begin);
            var channel = reader.ReadUInt32();
            var message = reader.ReadUtf16(0x9D3F, 0x44);

            if (message.StartsWith(PolarisApp.Config.CommandPrefix))
            {
                var valid = false;

                // Iterate commands
                foreach (var command in PolarisApp.ConsoleSystem.Commands)
                {
                    var full = message.Substring(1); // Strip the command chars
                    var args = full.Split(' ');

                    if (command.Names.Any(name => args[0].ToLower() == name.ToLower()))
                    {
                        command.Run(args, args.Length, full, context);
                        valid = true;
                        Logger.WriteCommand(null, "[CMD] {0} issued command {1}", context.User.Username, full);
                    }

                    if (valid)
                        break;
                }

                if (!valid)
                    Logger.WriteClient(context, "[CMD] {0} - Command not found", message.Split(' ')[0].Trim('\r'));
            }
            else
            {
                Logger.Write("[CHT] <{0}> <{1}>", context.Character.Name, message);

                var writer = new PacketWriter();
                writer.WritePlayerHeader((uint) context.User.PlayerId);
                writer.Write(channel);
                writer.WriteUtf16(message, 0x9D3F, 0x44);

                data = writer.ToArray();

                foreach (var c in Server.Instance.Clients)
                {
                    if (c.Character == null || c.CurrentZone != context.CurrentZone)
                        continue;

                    c.SendPacket(0x07, 0x00, 0x44, data);
                }
            }
        }

        #endregion
    }
}