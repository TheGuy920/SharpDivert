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

#pragma warning disable CS1591

namespace SharpDivert
{
    public partial class WinDivert
    {
        public enum Event
        {
            /// <summary>
            /// A new network packet.
            /// </summary>
            NetworkPacket = 0,

            /// <summary>
            /// A new flow is created.
            /// </summary>
            FlowEstablished = 1,

            /// <summary>
            /// An old flow is deleted.
            /// </summary>
            FlowDeleted = 2,

            /// <summary>
            /// A <c>bind()</c> operation.
            /// </summary>
            SocketBind = 3,

            /// <summary>
            /// A <c>connect()</c> operation.
            /// </summary>
            SocketConnect = 4,

            /// <summary>
            /// A <c> listen()</c> operation.
            /// </summary>
            SocketListen = 5,

            /// <summary>
            /// An <c>accept()</c> operation.
            /// </summary>
            SocketAccept = 6,

            /// <summary>
            /// A socket endpoint is closed.
            /// </summary>
            SocketClose = 7,

            /// <summary>
            /// A new WinDivert handle was opened.
            /// </summary>
            ReflectOpen = 8,

            /// <summary>
            /// An old WinDivert handle was closed.
            /// </summary>
            ReflectClose = 9,
        }
    }
}
