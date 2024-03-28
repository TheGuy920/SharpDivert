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

using System.Collections;
using System.Collections.Generic;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// It is the same as <see cref="WinDivertPacketEnumerator"/> except for the indexes that are added to the result.
    /// </summary>
    /// <remarks>
    /// Since the given packet is pinned, it is safe to dereference the pointer in the parsed result during enumeration.
    /// Do not use the pointer after the enumeration is finished.
    /// </remarks>
    public struct WinDivertIndexedPacketEnumerator : IEnumerator<(int, WinDivertParseResult)>
    {
        private WinDivertPacketEnumerator e;
        private int i;

        public (int, WinDivertParseResult) Current => (i, e.Current);
        object IEnumerator.Current => Current;

        internal WinDivertIndexedPacketEnumerator(WinDivertPacketEnumerator e)
        {
            this.e = e;
            i = -1;
        }

        public bool MoveNext()
        {
            var success = e.MoveNext();
            if (!success) return false;
            i++;
            return true;
        }

        public void Reset()
        {
            e.Reset();
            i = -1;
        }

        /// <summary>
        /// Releases the underlying resources.
        /// The user must call this function. Otherwise, the packet buffer will not be unpinned.
        /// </summary>
        public void Dispose() => e.Dispose();
    }
}
