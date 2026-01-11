using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace browser
{
    internal struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    internal class LastInputInfo
    {
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInput = new LASTINPUTINFO();
            lastInput.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInput);
            GetLastInputInfo(ref lastInput);

            return ((uint)Environment.TickCount - lastInput.dwTime);
        }

        public static long GetLastInputTime()
        {
            LASTINPUTINFO lastInput = new LASTINPUTINFO();
            lastInput.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInput);
            return lastInput.dwTime;
        }
    }
}
