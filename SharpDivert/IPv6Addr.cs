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
using System.Text;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// IPv6 address in host byte-order.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct IPv6Addr : IEquatable<IPv6Addr>
    {
        internal fixed uint Raw[4];

        /// <summary>
        /// Parses an IPv6 address stored in <paramref name="addrStr"/>.
        /// </summary>
        /// <param name="addrStr">The address string.</param>
        /// <returns>Output address.</returns>
        /// <exception cref="WinDivertException">Thrown if the parse fails.</exception>
        public static IPv6Addr Parse(string addrStr)
        {
            var addr = new IPv6Addr();
            var success = NativeMethods.WinDivertHelperParseIPv6Address(addrStr, addr.Raw);
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertHelperParseIPv6Address));
            return addr;
        }

        /// <summary>
        /// Convert an IPv6 address into a string.
        /// </summary>
        /// <returns>The formatted string.</returns>
        /// <exception cref="WinDivertException">Thrown if formatting fails.</exception>
        public override string ToString()
        {
            var buffer = (Span<byte>)stackalloc byte[64];
            var success = false;
            fixed (uint* addr = Raw) fixed (byte* pBuffer = buffer)
            {
                success = NativeMethods.WinDivertHelperFormatIPv6Address(addr, pBuffer, (uint)buffer.Length);
            }
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertHelperFormatIPv6Address));

            var strlen = buffer.IndexOf((byte)0);
            return Encoding.ASCII.GetString(buffer[..strlen]);
        }

        public static bool operator ==(IPv6Addr left, IPv6Addr right) => left.Equals(right);
        public static bool operator !=(IPv6Addr left, IPv6Addr right) => !left.Equals(right);

        public bool Equals(IPv6Addr addr)
        {
            return Raw[0] == addr.Raw[0]
                && Raw[1] == addr.Raw[1]
                && Raw[2] == addr.Raw[2]
                && Raw[3] == addr.Raw[3];
        }

        public override bool Equals(object? obj)
        {
            if (obj is IPv6Addr ipv6Addr) return Equals(ipv6Addr);
            return base.Equals(obj);
        }

        public override unsafe int GetHashCode() => HashCode.Combine(Raw[0], Raw[1], Raw[2], Raw[3]);
    }
}
