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
using System.Collections;
using System.Collections.Generic;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// It is the same as <see cref="WinDivertPacketParser"/> except for the indexes that are added to the result.
    /// </summary>
    public struct WinDivertIndexedPacketParser : IEnumerable<(int, WinDivertParseResult)>
    {
        private readonly WinDivertPacketParser e;

        /// <summary>
        /// Initializes an instance of <see cref="WinDivertIndexedPacketParser"/> class with given packets.
        /// </summary>
        /// <param name="packet">Packets to be parsed.</param>
        public WinDivertIndexedPacketParser(Memory<byte> packet) => e = new WinDivertPacketParser(packet);

        /// <summary>
        /// Initializes an instance of the <see cref="WinDivertIndexedPacketParser"/> class that wraps the given <see cref="WinDivertPacketParser"/>.
        /// </summary>
        /// <param name="e"><see cref="WinDivertPacketParser"/> to be wrapped.</param>
        public WinDivertIndexedPacketParser(WinDivertPacketParser e) => this.e = e;

        /// <summary>
        /// Returns an enumerator that iterates over the results of packet parsing.
        /// Since this function returns the struct as is, no boxing occurs and heap allocation can be avoided.
        /// </summary>
        /// <returns>An enumerator that iterates the result of packet parsing.</returns>
        public WinDivertIndexedPacketEnumerator GetEnumerator() => new(e.GetEnumerator());

        IEnumerator<(int, WinDivertParseResult)> IEnumerable<(int, WinDivertParseResult)>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
