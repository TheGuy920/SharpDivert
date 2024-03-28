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
    /// Retrieves a raw handle from a <see cref="SafeHandle"/>.
    /// </summary>
    public struct SafeHandleReference : IDisposable
    {
        /// <summary>
        /// Handle taken from <see cref="SafeHandle"/>.
        /// If the handle has already been closed, it will be set to an invalid handle value.
        /// </summary>
        public IntPtr RawHandle { get; private set; }

        private readonly SafeHandle? handle;
        private readonly IntPtr invalid;
        private bool reference;

        /// <summary>
        /// Initializes an instance of <see cref="SafeHandleReference"/> class.
        /// </summary>
        /// <param name="handle">The target <see cref="SafeHandle"/>.</param>
        /// <param name="invalid">Invalid value for handle. The value to be used instead of the actual handle if <paramref name="handle"/> is already closed.</param>
        public SafeHandleReference(SafeHandle? handle, IntPtr invalid)
        {
            RawHandle = invalid;
            this.handle = handle;
            this.invalid = invalid;
            reference = false;
            if (handle is null || handle.IsInvalid || handle.IsClosed) return;
            handle.DangerousAddRef(ref reference);
            RawHandle = handle.DangerousGetHandle();
        }

        /// <summary>
        /// Releases the underlying <see cref="SafeHandle"/>.
        /// The user must call this function. Otherwise, the handle will leak.
        /// </summary>
        public void Dispose()
        {
            RawHandle = invalid;
            if (reference)
            {
                handle?.DangerousRelease();
                reference = false;
            }
        }
    }
}
