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

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// The result of packet parsing.
    /// </summary>
    public unsafe struct WinDivertParseResult
    {
        /// <summary>
        /// The entirety of the packet.
        /// </summary>
        public Memory<byte> Packet;

        /// <summary>
        /// Points to the IPv4 header of the packet.
        /// If the packet does not contain any IPv4 header, it will be null.
        /// </summary>
        public WinDivertIPv4Hdr* IPv4Hdr;

        /// <summary>
        /// Points to the IPv6 header of the packet.
        /// If the packet does not contain any IPv6 header, it will be null.
        /// </summary>
        public WinDivertIPv6Hdr* IPv6Hdr;

        /// <summary>
        /// Transport protocol.
        /// </summary>
        public byte Protocol;

        /// <summary>
        /// Points to the ICMPv4 header of the packet.
        /// If the packet does not contain any ICMPv4 header, it will be null.
        /// </summary>
        public WinDivertICMPv4Hdr* ICMPv4Hdr;

        /// <summary>
        /// Points to the ICMPv6 header of the packet.
        /// If the packet does not contain any ICMPv6 header, it will be null.
        /// </summary>
        public WinDivertICMPv6Hdr* ICMPv6Hdr;

        /// <summary>
        /// Points to the TCP header of the packet.
        /// If the packet does not contain any TCP header, it will be null.
        /// </summary>
        public WinDivertTCPHdr* TCPHdr;

        /// <summary>
        /// Points to the UDP header of the packet.
        /// If the packet does not contain any UDP header, it will be null.
        /// </summary>
        public WinDivertUDPHdr* UDPHdr;

        /// <summary>
        /// The packet's data/payload.
        /// If the packet does not contain any data/payload, it will be empty.
        /// </summary>
        public Memory<byte> Data;
    }
}
