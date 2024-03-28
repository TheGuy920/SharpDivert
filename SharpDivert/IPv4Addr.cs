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
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// IPv4 address in host byte-order.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IPv4Addr : IEquatable<IPv4Addr>
    {
        internal uint Raw;

        /// <summary>
        /// Parses an IPv4 address stored in <paramref name="addrStr"/>.
        /// </summary>
        /// <param name="addrStr">The address string.</param>
        /// <returns>Output address.</returns>
        /// <exception cref="WinDivertException">Thrown if the parse fails.</exception>
        public static unsafe IPv4Addr Parse(string addrStr)
        {
            var addr = new IPv4Addr();
            var success = NativeMethods.WinDivertHelperParseIPv4Address(addrStr, &addr.Raw);
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertHelperParseIPv4Address));
            return addr;
        }

        /// <summary>
        /// Convert an IPv4 address into a string.
        /// </summary>
        /// <returns>The formatted string.</returns>
        /// <exception cref="WinDivertException">Thrown if formatting fails.</exception>
        public override unsafe string ToString()
        {
            var buffer = (Span<byte>)stackalloc byte[32];
            var success = false;
            fixed (byte* pBuffer = buffer) success = NativeMethods.WinDivertHelperFormatIPv4Address(Raw, pBuffer, (uint)buffer.Length);
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertHelperFormatIPv4Address));

            var strlen = buffer.IndexOf((byte)0);
            return Encoding.ASCII.GetString(buffer[..strlen]);
        }

        public static bool operator ==(IPv4Addr left, IPv4Addr right) => left.Equals(right);
        public static bool operator !=(IPv4Addr left, IPv4Addr right) => !left.Equals(right);

        public bool Equals(IPv4Addr addr) => Raw == addr.Raw;

        public override bool Equals(object? obj)
        {
            if (obj is IPv4Addr ipv4Addr) return Equals(ipv4Addr);
            return base.Equals(obj);
        }

        public override int GetHashCode() => HashCode.Combine(Raw);
    }
}
