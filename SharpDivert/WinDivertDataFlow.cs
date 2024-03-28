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
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertDataFlow
    {
        /// <summary>
        /// The endpoint ID of the flow.
        /// </summary>
        public ulong EndpointId;

        /// <summary>
        /// The parent endpoint ID of the flow.
        /// </summary>
        public ulong ParentEndpointId;

        /// <summary>
        /// The ID of the process associated with the flow.
        /// </summary>
        public uint ProcessId;

        /// <summary>
        /// The local address associated with the flow.
        /// </summary>
        public IPv6Addr LocalAddr;

        /// <summary>
        /// The remote address associated with the flow.
        /// </summary>
        public IPv6Addr RemoteAddr;

        /// <summary>
        /// The local port associated with the flow.
        /// </summary>
        public ushort LocalPort;

        /// <summary>
        /// The remote port associated with the flow.
        /// </summary>
        public ushort RemotePort;

        /// <summary>
        /// The protocol associated with the flow.
        /// </summary>
        public byte Protocol;
    }
}
