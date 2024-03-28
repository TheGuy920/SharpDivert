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
using System.Threading;

#pragma warning disable CS1591

namespace SharpDivert
{
#pragma warning disable CA2101
    internal static class NativeMethods
    {
        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe IntPtr WinDivertOpen(byte* filter, WinDivert.Layer layer, short priority, WinDivert.Flag flags);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertRecvEx(IntPtr handle, void* packet, uint packetLen, uint* recvLen, ulong flags, WinDivertAddress* addr, uint* addrLen, NativeOverlapped* overlapped);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertSendEx(IntPtr handle, void* packet, uint packetLen, uint* sendLen, ulong flags, WinDivertAddress* addr, uint addrLen, NativeOverlapped* overlapped);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern bool WinDivertSetParam(IntPtr handle, WinDivert.Param param, ulong value);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern bool WinDivertGetParam(IntPtr handle, WinDivert.Param param, out ulong value);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern bool WinDivertShutdown(IntPtr handle, WinDivert.ShutdownHow how);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern bool WinDivertClose(IntPtr handle);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern unsafe bool WinDivertHelperParsePacket(void* packet, uint packetLen, WinDivertIPv4Hdr** ipv4Hdr, WinDivertIPv6Hdr** ipv6Hdr, byte* protocol, WinDivertICMPv4Hdr** icmpv4Hdr, WinDivertICMPv6Hdr** icmpv6Hdr, WinDivertTCPHdr** tcpHdr, WinDivertUDPHdr** udpHdr, void** data, uint* dataLen, void** next, uint* nextLen);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertHelperParseIPv4Address(string addrStr, uint* addr);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertHelperParseIPv6Address(string addrStr, uint* addr);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertHelperFormatIPv4Address(uint addr, byte* buffer, uint buflen);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertHelperFormatIPv6Address(uint* addr, byte* buffer, uint buflen);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern unsafe bool WinDivertHelperCalcChecksums(void* packet, uint packetLen, WinDivertAddress* addr, WinDivert.ChecksumFlag flags);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern unsafe bool WinDivertHelperCompileFilter(string filter, WinDivert.Layer layer, byte* fobj, uint fobjLen, byte** errorStr, uint* errorPos);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = true)]
        public static extern unsafe bool WinDivertHelperFormatFilter(byte* filter, WinDivert.Layer layer, byte* buffer, uint bufLen);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern ushort WinDivertHelperNtohs(ushort x);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern ushort WinDivertHelperHtons(ushort x);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern uint WinDivertHelperNtohl(uint x);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern uint WinDivertHelperHtonl(uint x);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern unsafe void WinDivertHelperNtohIPv6Address(uint* inAddr, uint* outAddr);

        [DllImport("WinDivert.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, PreserveSig = true, SetLastError = false)]
        public static extern unsafe void WinDivertHelperHtonIPv6Address(uint* inAddr, uint* outAddr);
    }
}
