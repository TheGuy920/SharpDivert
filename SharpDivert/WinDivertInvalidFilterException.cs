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
    /// <summary>
    /// An exception thrown when a packet filter string is invalid.
    /// </summary>
    public class WinDivertInvalidFilterException : ArgumentException
    {
        /// <summary>
        /// The error description.
        /// </summary>
        public string FilterErrorStr;

        /// <summary>
        /// The error position.
        /// </summary>
        public uint FilterErrorPos;

        /// <summary>
        /// Initializes an new instance of <see cref="WinDivertInvalidFilterException"/> class.
        /// </summary>
        /// <param name="errorStr">The error description.</param>
        /// <param name="errorPos">The error position.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public WinDivertInvalidFilterException(string errorStr, uint errorPos, string? paramName) : base(errorStr, paramName)
        {
            FilterErrorStr = errorStr;
            FilterErrorPos = errorPos;
        }
    }
}
