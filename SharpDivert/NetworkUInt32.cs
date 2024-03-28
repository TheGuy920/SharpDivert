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

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// <see cref="uint"/> in network byte-order.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NetworkUInt32 : IEquatable<NetworkUInt32>
    {
        private readonly uint raw;

        private NetworkUInt32(uint raw) => this.raw = raw;
        public static implicit operator NetworkUInt32(uint x) => new(NativeMethods.WinDivertHelperHtonl(x));
        public static implicit operator uint(NetworkUInt32 x) => NativeMethods.WinDivertHelperNtohl(x.raw);
        public static bool operator ==(NetworkUInt32 left, NetworkUInt32 right) => left.Equals(right);
        public static bool operator !=(NetworkUInt32 left, NetworkUInt32 right) => !left.Equals(right);
        public bool Equals(NetworkUInt32 x) => raw == x.raw;

        public override bool Equals(object? obj)
        {
            if (obj is NetworkUInt32 x) return Equals(x);
            return base.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => ((uint)this).ToString();
    }
}
