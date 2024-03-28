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

using System.Runtime.InteropServices;

#pragma warning disable CS1591

namespace SharpDivert
{
#pragma warning restore CA2101

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct WinDivertAddress
    {
        /// <summary>
        /// A timestamp indicating when event occurred.
        /// </summary>
        [FieldOffset(0)]
        public long Timestamp;

        [FieldOffset(8)] private byte byteLayer;
        [FieldOffset(9)] private byte byteEvent;
        [FieldOffset(10)] private byte flags;

        /// <summary>
        /// Data specific to the network layer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Layer"/> is not <see cref="WinDivert.Layer.Network"/>, do not access this field.
        /// This field is in a <c>union</c> and shares memory space with other fields.
        /// </remarks>
        [FieldOffset(16)]
        public WinDivertDataNetwork Network;

        /// <summary>
        /// Data specific to the flow layer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Layer"/> is not <see cref="WinDivert.Layer.Flow"/>, do not access this field.
        /// This field is in a <c>union</c> and shares memory space with other fields.
        /// </remarks>
        [FieldOffset(16)]
        public WinDivertDataFlow Flow;

        /// <summary>
        /// Data specific to the socket layer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Layer"/> is not <see cref="WinDivert.Layer.Socket"/>, do not access this field.
        /// This field is in a <c>union</c> and shares memory space with other fields.
        /// </remarks>
        [FieldOffset(16)]
        public WinDivertDataSocket Socket;

        /// <summary>
        /// Data specific to the reflect layer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Layer"/> is not <see cref="WinDivert.Layer.Reflect"/>, do not access this field.
        /// This field is in a <c>union</c> and shares memory space with other fields.
        /// </remarks>
        [FieldOffset(16)]
        public WinDivertDataReflect Reflect;

        [FieldOffset(16)] private fixed byte reserved[64];

        /// <summary>
        /// The handle's layer.
        /// </summary>
        public WinDivert.Layer Layer
        {
            get => (WinDivert.Layer)byteLayer;
            set => byteLayer = (byte)value;
        }

        /// <summary>
        /// The captured event.
        /// </summary>
        public WinDivert.Event Event
        {
            get => (WinDivert.Event)byteEvent;
            set => byteEvent = (byte)value;
        }

        /// <summary>
        /// Set to <c>true</c> if the event was sniffed (i.e., not blocked), <c>false</c> otherwise.
        /// </summary>
        public bool Sniffed
        {
            get => GetFlag(1 << 0);
            set => SetFlag(1 << 0, value);
        }

        /// <summary>
        /// Set to <c>true</c> for outbound packets/event, <c>false</c> for inbound or otherwise.
        /// </summary>
        public bool Outbound
        {
            get => GetFlag(1 << 1);
            set => SetFlag(1 << 1, value);
        }

        /// <summary>
        /// Set to <c>true</c> for loopback packets, <c>false</c> otherwise.
        /// </summary>
        public bool Loopback
        {
            get => GetFlag(1 << 2);
            set => SetFlag(1 << 2, value);
        }

        /// <summary>
        /// Set to <c>true</c> for impostor packets, <c>false</c> otherwise.
        /// </summary>
        public bool Impostor
        {
            get => GetFlag(1 << 3);
            set => SetFlag(1 << 3, value);
        }

        /// <summary>
        /// Set to <c>true</c> for IPv6 packets/events, <c>false</c> otherwise.
        /// </summary>
        public bool IPv6
        {
            get => GetFlag(1 << 4);
            set => SetFlag(1 << 4, value);
        }

        /// <summary>
        /// Set to <c>true</c> if the IPv4 checksum is valid, <c>false</c> otherwise.
        /// </summary>
        public bool IPChecksum
        {
            get => GetFlag(1 << 5);
            set => SetFlag(1 << 5, value);
        }

        /// <summary>
        /// Set to <c>true</c> if the TCP checksum is valid, <c>false</c> otherwise.
        /// </summary>
        public bool TCPChecksum
        {
            get => GetFlag(1 << 6);
            set => SetFlag(1 << 6, value);
        }

        /// <summary>
        /// Set to <c>true</c> if the UDP checksum is valid, <c>false</c> otherwise.
        /// </summary>
        public bool UDPChecksum
        {
            get => GetFlag(1 << 7);
            set => SetFlag(1 << 7, value);
        }

        private bool GetFlag(byte bit) => (flags & bit) != 0;

        private void SetFlag(byte bit, bool val)
        {
            if (val) flags = (byte)(flags | bit);
            else flags = (byte)((flags | bit) ^ bit);
        }
    }
}
