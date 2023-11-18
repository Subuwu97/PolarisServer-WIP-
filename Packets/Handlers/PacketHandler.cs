﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PolarisServer.Packets.Handlers
{
    public class PacketHandlerAttr : Attribute
    {
        public uint Type, Subtype;

        public PacketHandlerAttr(uint type, uint subtype)
        {
            Type = type;
            Subtype = subtype;
        }
    }

    public abstract class PacketHandler
    {
        public abstract void HandlePacket(Client context, byte flags, byte[] data, uint position, uint size);
    }

    public static class PacketHandlers
    {
        private static readonly Dictionary<ushort, PacketHandler> Handlers = new Dictionary<ushort, PacketHandler>();

        public static void LoadPacketHandlers()
        {
            var classes = from t in Assembly.GetExecutingAssembly().GetTypes()
                where
                    t.IsClass && t.Namespace == "PolarisServer.Packets.Handlers" &&
                    t.IsSubclassOf(typeof (PacketHandler))
                select t;

            foreach (var t in classes.ToList())
            {
                var attrs = (Attribute[]) t.GetCustomAttributes(typeof (PacketHandlerAttr), false);

                if (attrs.Length > 0)
                {
                    var attr = (PacketHandlerAttr) attrs[0];
                    Logger.WriteInternal("[PKT] Loaded PacketHandler {0} for packet {1:X}-{2:X}.", t.Name, attr.Type,
                        attr.Subtype);
                    if (!Handlers.ContainsKey(Helper.PacketTypeToUShort(attr.Type, attr.Subtype)))
                        Handlers.Add(Helper.PacketTypeToUShort(attr.Type, attr.Subtype),
                            (PacketHandler) Activator.CreateInstance(t));
                }
            }
        }

        /// <summary>
        ///     Gets and creates a PacketHandler for a given packet type and subtype.
        /// </summary>
        /// <returns>An instance of a PacketHandler or null</returns>
        /// <param name="type">Type a.</param>
        /// <param name="subtype">Type b.</param>
        public static PacketHandler GetHandlerFor(uint type, uint subtype)
        {
            var packetCode = Helper.PacketTypeToUShort(type, subtype);
            PacketHandler handler = null;

            if (Handlers.ContainsKey(packetCode))
                Handlers.TryGetValue(packetCode, out handler);

            return handler;
        }

        public static PacketHandler[] GetLoadedHandlers()
        {
            return Handlers.Values.ToArray();
        }
    }
}