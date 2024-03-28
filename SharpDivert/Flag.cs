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
    public partial class WinDivert
    {
        [Flags]
        public enum Flag : ulong
        {
            /// <summary>
            /// This flag opens the WinDivert handle in packet sniffing mode.
            /// In packet sniffing mode the original packet is not dropped-and-diverted (the default) but copied-and-diverted.
            /// </summary>
            Sniff = 0x0001,

            /// <summary>
            /// This flag indicates that the user application does not intend to read matching packets with <see cref="RecvEx"/>, instead the packets should be silently dropped.
            /// </summary>
            Drop = 0x0002,

            /// <summary>
            /// This flags forces the handle into receive only mode which effectively disables <see cref="SendEx"/>.
            /// This means that it is possible to block/capture packets or events but not inject them.
            /// </summary>
            RecvOnly = 0x0004,

            /// <summary>
            /// An alias for <see cref="RecvOnly"/>.
            /// </summary>
            ReadOnly = RecvOnly,

            /// <summary>
            /// This flags forces the handle into send only mode which effectively disables <see cref="RecvEx"/>.
            /// This means that it is possible to inject packets or events, but not block/capture them.
            /// </summary>
            SendOnly = 0x0008,

            /// <summary>
            /// An alias for <see cref="SendOnly"/>.
            /// </summary>
            WriteOnly = SendOnly,

            /// <summary>
            /// This flags causes the constructor to fail with <c>ERROR_SERVICE_DOES_NOT_EXIST</c> if the WinDivert driver is not already installed.
            /// </summary>
            NoInstall = 0x0010,

            /// <summary>
            /// If set, the handle will capture inbound IP fragments, but not inbound reassembled IP packets.
            /// Otherwise, if not set (the default), the handle will capture inbound reassembled IP packets, but not inbound IP fragments.
            /// This flag only affects inbound packets at the <see cref="Layer.Network"/> layer, else the flag is ignored.
            /// </summary>
            Fragments = 0x0020,
        }
    }
}
