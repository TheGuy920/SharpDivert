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
        public enum Layer
        {
            /// <summary>
            /// Network packets to/from the local machine.
            /// </summary>
            Network = 0,

            /// <summary>
            /// Network packets passing through the local machine.
            /// </summary>
            NetworkForward = 1,

            /// <summary>
            /// Network flow established/deleted events.
            /// </summary>
            Flow = 2,

            /// <summary>
            /// Socket operation events.
            /// </summary>
            Socket = 3,

            /// <summary>
            /// WinDivert handle events.
            /// </summary>
            Reflect = 4,
        }
    }
}
