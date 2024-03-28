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
    /// IPv4 address in network byte-order.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NetworkIPv4Addr : IEquatable<NetworkIPv4Addr>
    {
        private uint _raw;
        public IPAddress Ipv4 { get; private set; }
        public uint Raw 
        {
            get => _raw;
            internal set
            {
                _raw = value;
                Ipv4 = new IPAddress(value);
            } 
        }

        /// <summary>
        /// Convert an IPv4 address into a string.
        /// </summary>
        /// <returns>The formatted string.</returns>
        /// <exception cref="WinDivertException">Thrown if formatting fails.</exception>
        public override string ToString() => ((IPv4Addr)this).ToString();

        public static bool operator ==(NetworkIPv4Addr left, NetworkIPv4Addr right) => left.Equals(right);
        public static bool operator !=(NetworkIPv4Addr left, NetworkIPv4Addr right) => !left.Equals(right);

        public bool Equals(NetworkIPv4Addr addr) => Raw == addr.Raw;

        public static implicit operator NetworkIPv4Addr(IPv4Addr addr) => new()
        {
            Raw = NativeMethods.WinDivertHelperHtonl(addr.Raw),
        };

        public static implicit operator IPv4Addr(NetworkIPv4Addr addr) => new()
        {
            Raw = NativeMethods.WinDivertHelperNtohl(addr.Raw),
        };

        public override bool Equals(object? obj)
        {
            if (obj is NetworkIPv4Addr addr) return Equals(addr);
            return base.Equals(obj);
        }

        public override int GetHashCode() => HashCode.Combine(Raw);
    }
}
