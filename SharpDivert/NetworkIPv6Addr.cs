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
using System.Net;
using System.Runtime.InteropServices;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// IPv6 address in network byte-order.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NetworkIPv6Addr : IEquatable<NetworkIPv6Addr>
    {
        internal fixed uint Raw[4];

        public IPAddress ToIPAddress()
        {
            var addressBytes = new byte[16]; // IPv6 length in bytes

            for (var i = 0; i < 4; i++)
            {
                var temp = BitConverter.GetBytes(Raw[i]);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(temp); // Convert to big-endian (network byte order)
                }
                Array.Copy(temp, 0, addressBytes, i * 4, 4);
            }

            return new IPAddress(addressBytes);
        }

        /// <summary>
        /// Convert an IPv6 address into a string.
        /// </summary>
        /// <returns>The formatted string.</returns>
        /// <exception cref="WinDivertException">Thrown if formatting fails.</exception>
        public override string ToString() => ((IPv6Addr)this).ToString();

        public static bool operator ==(NetworkIPv6Addr left, NetworkIPv6Addr right) => left.Equals(right);
        public static bool operator !=(NetworkIPv6Addr left, NetworkIPv6Addr right) => !left.Equals(right);

        public bool Equals(NetworkIPv6Addr addr)
        {
            return Raw[0] == addr.Raw[0]
                && Raw[1] == addr.Raw[1]
                && Raw[2] == addr.Raw[2]
                && Raw[3] == addr.Raw[3];
        }

        public static implicit operator NetworkIPv6Addr(IPv6Addr addr)
        {
            var naddr = new NetworkIPv6Addr();
            NativeMethods.WinDivertHelperHtonIPv6Address(addr.Raw, naddr.Raw);
            return naddr;
        }

        public static implicit operator IPv6Addr(NetworkIPv6Addr addr)
        {
            var haddr = new IPv6Addr();
            NativeMethods.WinDivertHelperNtohIPv6Address(addr.Raw, haddr.Raw);
            return haddr;
        }

        public override bool Equals(object? obj)
        {
            if (obj is NetworkIPv6Addr addr) return Equals(addr);
            return base.Equals(addr);
        }

        public override unsafe int GetHashCode() => HashCode.Combine(Raw[0], Raw[1], Raw[2], Raw[3]);
    }
}
