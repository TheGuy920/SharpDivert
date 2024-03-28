/*
 * SharpDivert.cs
 * Copyright gcrtnst
 *
 * This file is part of SharpDivert.
 *
 * SharpDivert is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * SharpDivert is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with SharpDivert.  If not, see <http://www.gnu.org/licenses/>.
 *
 * SharpDivert is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option)
 * any later version.
 * 
 * SharpDivert is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * for more details.
 * 
 * You should have received a copy of the GNU General Public License along
 * with SharpDivert; if not, write to the Free Software Foundation, Inc., 51
 * Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// An enumerator that parses the given packet.
    /// </summary>
    /// <remarks>
    /// Since the given packet is pinned, it is safe to dereference the pointer in the parsed result during enumeration.
    /// Do not use the pointer after the enumeration is finished.
    /// </remarks>
    public unsafe struct WinDivertPacketEnumerator : IEnumerator<WinDivertParseResult>
    {
        private readonly MemoryHandle hmem;
        private readonly Memory<byte> packet;
        private readonly byte* pPacket0;
        private byte* pPacket;
        private uint packetLen;

        private WinDivertParseResult current;
        public WinDivertParseResult Current => current;
        object IEnumerator.Current => current;

        internal WinDivertPacketEnumerator(Memory<byte> packet)
        {
            hmem = packet.Pin();
            this.packet = packet;
            pPacket0 = (byte*)hmem.Pointer;
            pPacket = pPacket0;
            packetLen = (uint)packet.Length;
            current = new WinDivertParseResult();
        }

        public bool MoveNext()
        {
            var ipv4Hdr = (WinDivertIPv4Hdr*)null;
            var ipv6Hdr = (WinDivertIPv6Hdr*)null;
            var protocol = (byte)0;
            var icmpv4Hdr = (WinDivertICMPv4Hdr*)null;
            var icmpv6Hdr = (WinDivertICMPv6Hdr*)null;
            var tcpHdr = (WinDivertTCPHdr*)null;
            var udpHdr = (WinDivertUDPHdr*)null;
            var pData = (byte*)null;
            var dataLen = (uint)0;
            var pNext = (byte*)null;
            var nextLen = (uint)0;

            var success = NativeMethods.WinDivertHelperParsePacket(pPacket, packetLen, &ipv4Hdr, &ipv6Hdr, &protocol, &icmpv4Hdr, &icmpv6Hdr, &tcpHdr, &udpHdr, (void**)&pData, &dataLen, (void**)&pNext, &nextLen);
            if (!success) return false;

            current.Packet = pNext != null
                ? packet[(int)(pPacket - pPacket0)..(int)(pNext - pPacket0)]
                : packet[(int)(pPacket - pPacket0)..(int)(pPacket + packetLen - pPacket0)];
            current.IPv4Hdr = ipv4Hdr;
            current.IPv6Hdr = ipv6Hdr;
            current.Protocol = protocol;
            current.ICMPv4Hdr = icmpv4Hdr;
            current.ICMPv6Hdr = icmpv6Hdr;
            current.TCPHdr = tcpHdr;
            current.UDPHdr = udpHdr;
            current.Data = pData != null && dataLen > 0
                ? packet[(int)(pData - pPacket0)..(int)(pData + dataLen - pPacket0)]
                : Memory<byte>.Empty;

            pPacket = pNext;
            packetLen = nextLen;
            return true;
        }

        public void Reset()
        {
            pPacket = pPacket0;
            packetLen = (uint)packet.Length;
            current = new WinDivertParseResult();
        }

        /// <summary>
        /// Releases the underlying resources.
        /// The user must call this function. Otherwise, the packet buffer will not be unpinned.
        /// </summary>
        public void Dispose() => hmem.Dispose();
    }
}
