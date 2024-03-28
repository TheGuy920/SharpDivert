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
using System.Text;

#pragma warning disable CS1591

namespace SharpDivert
{
    /// <summary>
    /// Allows user-mode applications to capture/modify/drop network packets sent to/from the Windows network stack.
    /// </summary>
    public partial class WinDivert : IDisposable
    {
        private readonly SafeWinDivertHandle handle;

        public const short PriorityHighest = 30000;
        public const short PriorityLowest = -PriorityHighest;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinDivert"/> class.
        /// </summary>
        /// <param name="filter">A packet filter string specified in the WinDivert filter language.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="priority">The priority of the handle.</param>
        /// <param name="flags">Additional flags.</param>
        /// <exception cref="WinDivertInvalidFilterException">Thrown when the <paramref name="filter"/> is invalid.</exception>
        /// <exception cref="WinDivertException">Thrown when a WinDivert handle fails to open.</exception>
        public WinDivert(string filter, Layer layer, short priority, Flag flags)
        {
            var fobj = CompileFilter(filter, layer);
            handle = Open(fobj.Span, layer, priority, flags);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinDivert"/> class with a compiled filter object.
        /// </summary>
        /// <param name="filter">A filter object compiled by <see cref="CompileFilter"/>. Passing non-null-terminated data may cause out-of-bounds access.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="priority">The priority of the handle.</param>
        /// <param name="flags">Additional flags.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="filter"/> is empty.</exception>
        /// <exception cref="WinDivertException">Thrown when a WinDivert handle fails to open.</exception>
        public WinDivert(ReadOnlySpan<byte> filter, Layer layer, short priority, Flag flags) => handle = Open(filter, layer, priority, flags);

        private static unsafe SafeWinDivertHandle Open(ReadOnlySpan<byte> filter, Layer layer, short priority, Flag flags)
        {
            if (filter.IsEmpty) throw new ArgumentException($"{nameof(filter)} is empty.", nameof(filter));

            IntPtr hraw = -1;
            fixed (byte* pFilter = filter) hraw = NativeMethods.WinDivertOpen(pFilter, layer, priority, flags);
            
            if (hraw == -1)
                throw new WinDivertException(nameof(NativeMethods.WinDivertOpen));
            
            return new SafeWinDivertHandle(hraw, true);
        }

        public const int BatchMax = 0xFF;
        public const int MTUMax = 40 + 0xFFFF;

        /// <summary>
        /// Receives captured packets/events matching the filter passed to the constructor.
        /// </summary>
        /// <param name="packet">An buffer for the captured packet. Can be empty if packets are not needed.</param>
        /// <param name="abuf">An buffer for the address of the captured packet/event. Can be empty if addresses are not needed.</param>
        /// <returns><c>recvLen</c> is the total number of bytes written to <paramref name="packet"/>. <c>addrLen</c> is the total number of addresses written to <paramref name="abuf"/>.</returns>
        /// <exception cref="WinDivertException">Thrown when a packet fails to be received.</exception>
        public unsafe (uint recvLen, uint addrLen) RecvEx(Span<byte> packet, Span<WinDivertAddress> abuf)
        {
            var recvLen = (uint)0;
            var addrLen = (uint)0;
            var pAddrLen = (uint*)null;
            if (!abuf.IsEmpty)
            {
                addrLen = (uint)(abuf.Length * sizeof(WinDivertAddress));
                pAddrLen = &addrLen;
            }

            using (var href = new SafeHandleReference(handle, (IntPtr)(-1)))
            {
                var success = false;
                fixed (byte* pPacket = packet) fixed (WinDivertAddress* pAbuf = abuf)
                {
                    success = NativeMethods.WinDivertRecvEx(href.RawHandle, pPacket, (uint)packet.Length, &recvLen, 0, pAbuf, pAddrLen, null);
                }
                if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertRecvEx));
            }

            addrLen /= (uint)sizeof(WinDivertAddress);
            return (recvLen, addrLen);
        }

        /// <summary>
        /// Injects packets into the network stack.
        /// </summary>
        /// <param name="packet">A buffer containing a packet to be injected.</param>
        /// <param name="addr">The address of the injected packet.</param>
        /// <returns>The total number of bytes injected.</returns>
        /// <exception cref="WinDivertException">Throws when a packet fails to be injected.</exception>
        public unsafe uint SendEx(ReadOnlySpan<byte> packet, ReadOnlySpan<WinDivertAddress> addr)
        {
            using var href = new SafeHandleReference(handle, (IntPtr)(-1));
            var sendLen = (uint)0;
            var success = false;
            fixed (byte* pPacket = packet) fixed (WinDivertAddress* pAddr = addr)
            {
                success = NativeMethods.WinDivertSendEx(href.RawHandle, pPacket, (uint)packet.Length, &sendLen, 0, pAddr, (uint)(addr.Length * sizeof(WinDivertAddress)), null);
            }
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertSendEx));
            return sendLen;
        }

        public const ulong QueueLengthDefault = 4096;
        public const ulong QueueLengthMin = 32;
        public const ulong QueueLengthMax = 16384;

        /// <summary>
        /// The maximum length of the packet queue for <see cref="RecvEx"/>.
        /// </summary>
        /// <remarks>
        /// Setting an out-of-range value will cause a <see cref="WinDivertException"/>.
        /// </remarks>
        public ulong QueueLength
        {
            get => GetParam(Param.QueueLength);
            set => SetParam(Param.QueueLength, value);
        }

        public const ulong QueueTimeDefault = 2000;
        public const ulong QueueTimeMin = 100;
        public const ulong QueueTimeMax = 16000;

        /// <summary>
        /// The minimum time, in milliseconds, a packet can be queued before it is automatically dropped.
        /// </summary>
        /// <remarks>
        /// Setting an out-of-range value will cause a <see cref="WinDivertException"/>.
        /// </remarks>
        public ulong QueueTime
        {
            get => GetParam(Param.QueueTime);
            set => SetParam(Param.QueueTime, value);
        }

        public const ulong QueueSizeDefault = 4194304;
        public const ulong QueueSizeMin = 65535;
        public const ulong QueueSizeMax = 33554432;

        /// <summary>
        /// The maximum number of bytes that can be stored in the packet queue for <see cref="RecvEx"/>.
        /// </summary>
        /// <remarks>
        /// Setting an out-of-range value will cause a <see cref="WinDivertException"/>.
        /// </remarks>
        public ulong QueueSize
        {
            get => GetParam(Param.QueueSize);
            set => SetParam(Param.QueueSize, value);
        }

        /// <summary>
        /// The major version of the WinDivert driver.
        /// </summary>
        public ulong VersionMajor => GetParam(Param.VersionMajor);

        /// <summary>
        /// The minor version of the WinDivert driver.
        /// </summary>
        public ulong VersionMinor => GetParam(Param.VersionMinor);

        private ulong GetParam(Param param)
        {
            using var href = new SafeHandleReference(handle, (IntPtr)(-1));
            var success = NativeMethods.WinDivertGetParam(href.RawHandle, param, out var value);
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertGetParam));
            return value;
        }

        private void SetParam(Param param, ulong value)
        {
            using var href = new SafeHandleReference(handle, (IntPtr)(-1));
            var success = NativeMethods.WinDivertSetParam(href.RawHandle, param, value);
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertSetParam));
        }

        /// <summary>
        /// Stop new packets being queued for <see cref="RecvEx"/>
        /// </summary>
        /// <exception cref="WinDivertException">Thrown when an error occurs.</exception>
        public void ShutdownRecv() => Shutdown(ShutdownHow.Recv);

        /// <summary>
        /// Stop new packets being injected via <see cref="SendEx"/>
        /// </summary>
        /// <exception cref="WinDivertException">Thrown when an error occurs.</exception>
        public void ShutdownSend() => Shutdown(ShutdownHow.Send);

        /// <summary>
        /// Causes all of a WinDivert handle to be shut down.
        /// </summary>
        /// <exception cref="WinDivertException">Thrown when an error occurs.</exception>
        public void Shutdown() => Shutdown(ShutdownHow.Both);

        private void Shutdown(ShutdownHow how)
        {
            using var href = new SafeHandleReference(handle, (IntPtr)(-1));
            var success = NativeMethods.WinDivertShutdown(href.RawHandle, how);
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertShutdown));
        }

        /// <summary>
        /// Closes a WinDivert handle.
        /// </summary>
#pragma warning disable CA1816
        public void Dispose() => handle.Dispose();
#pragma warning restore CA1816

        /// <summary>
        /// (Re)calculates the checksum for any IPv4/ICMP/ICMPv6/TCP/UDP checksum present in the given packet.
        /// </summary>
        /// <param name="packet">The packet to be modified.</param>
        /// <param name="addr">The address.</param>
        /// <param name="flags">Additional flags.</param>
        /// <exception cref="ArgumentException"></exception>
        public static unsafe void CalcChecksums(Span<byte> packet, ref WinDivertAddress addr, ChecksumFlag flags)
        {
            var success = false;
            fixed (void* pPacket = packet) fixed (WinDivertAddress* pAddr = &addr)
            {
                success = NativeMethods.WinDivertHelperCalcChecksums(pPacket, (uint)packet.Length, pAddr, flags);
            }
            if (!success) throw new ArgumentException("An error occurred while calculating the checksum of the packet.");
        }

        /// <summary>
        /// Compiles the given packet filter string into a compact object representation.
        /// </summary>
        /// <param name="filter">The packet filter string to be checked.</param>
        /// <param name="layer">The layer.</param>
        /// <returns>The compiled filter object.</returns>
        /// <exception cref="WinDivertInvalidFilterException">Thrown when the <paramref name="filter"/> is invalid.</exception>
        public static unsafe ReadOnlyMemory<byte> CompileFilter(string filter, Layer layer)
        {
            var buffer = (Span<byte>)stackalloc byte[256 * 24];
            var pErrorStr = (byte*)null;
            var errorPos = (uint)0;
            var success = false;

            fixed (byte* pBuffer = buffer) success = NativeMethods.WinDivertHelperCompileFilter(filter, layer, pBuffer, (uint)buffer.Length, &pErrorStr, &errorPos);
            if (!success)
            {
                var errorLen = 0;
                while (*(pErrorStr + errorLen) != 0) errorLen++;
                var errorStr = Encoding.ASCII.GetString(pErrorStr, errorLen);
                throw new WinDivertInvalidFilterException(errorStr, errorPos, nameof(filter));
            }

            var fobjLen = buffer.IndexOf((byte)0) + 1;
            var fobj = new Memory<byte>(new byte[fobjLen]);
            buffer[..fobjLen].CopyTo(fobj.Span);
            return fobj;
        }

        /// <summary>
        /// Formats the given filter string or object.
        /// </summary>
        /// <param name="filter">The packet filter string to be evaluated. Passing non-null-terminated data may cause out-of-bounds access.</param>
        /// <param name="layer">The layer.</param>
        /// <returns>The formatted filter.</returns>
        /// <exception cref="WinDivertException">Thrown when the <paramref name="filter"/> is invalid.</exception>
        public static unsafe string FormatFilter(ReadOnlySpan<byte> filter, Layer layer)
        {
            var buffer = (Span<byte>)stackalloc byte[30000];
            var success = false;

            fixed (byte* pFilter = filter) fixed (byte* pBuffer = buffer)
            {
                success = NativeMethods.WinDivertHelperFormatFilter(pFilter, layer, pBuffer, (uint)buffer.Length);
            }
            if (!success) throw new WinDivertException(nameof(NativeMethods.WinDivertHelperFormatFilter));

            var strlen = buffer.IndexOf((byte)0);
            return Encoding.ASCII.GetString(buffer[..strlen]);
        }
    }
}
