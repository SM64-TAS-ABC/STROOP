using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class Kernal32NativeMethods
    {
        [Flags]
        private enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        #region DLL Import
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess,
            IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, IntPtr dwSize, ref int lpNumberOfBytesWritten);
        #endregion

        public static IntPtr ProcessGetHandleFromId(int dwDesiredAccess, bool bInheritHandle, int dwProcessId)
        {
            return OpenProcess(dwDesiredAccess, bInheritHandle, dwProcessId);
        }

        public static bool CloseProcess(IntPtr processHandle)
        {
            return CloseHandle(processHandle);
        }

        public static bool ProcessReadMemory(IntPtr hProcess,
            IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr dwSize, ref int lpNumberOfBytesRead)
        {
            return Kernal32NativeMethods.ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, dwSize, ref lpNumberOfBytesRead);
        }

        public static bool ProcessWriteMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, IntPtr dwSize, ref int lpNumberOfBytesWritten)
        {
            return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, dwSize, ref lpNumberOfBytesWritten);
        }

        public static void ResumeProcess(Process process)
        {
            // Resume all threads
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = Kernal32NativeMethods.OpenThread(Kernal32NativeMethods.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                    continue;

                int suspendCount = 0;
                do
                {
                    suspendCount = Kernal32NativeMethods.ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                Kernal32NativeMethods.CloseHandle(pOpenThread);
            }
        }

        public static void SuspendProcess(Process process)
        {
            // Pause all threads
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = Kernal32NativeMethods.OpenThread(Kernal32NativeMethods.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                    continue;

                Kernal32NativeMethods.SuspendThread(pOpenThread);
                Kernal32NativeMethods.CloseHandle(pOpenThread);
            }
        }
    }
}
