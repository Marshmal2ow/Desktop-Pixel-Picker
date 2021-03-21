using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace PixelPicker
{
    public class MouseHook
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        //Virtual key codes
        //found at http://msdn.microsoft.com/en-us/library/dd375731(v=VS.85).aspx
        private const UInt16 VK_LBUTTON = 0x01;//left mouse button
        private const UInt16 VK_RBUTTON = 0x02;//right mouse button
        private const UInt16 VK_MBUTTON = 0x04;//middle mouse button


        ///<summary>
        /// Returns negative when the button is DOWN and 0 when the button is UP
        ///</summary>
        ///<returns></returns>
        public static short GetMiddleButtonState()
        {
            return GetAsyncKeyState(VK_MBUTTON);
        }

        ///<summary>
        /// Returns negative when the button is DOWN and 0 when the button is UP
        ///</summary>
        ///<returns></returns>

        public static short GetRightButtonState()
        {
            return GetAsyncKeyState(VK_RBUTTON);
        }

        ///<summary>
        /// Returns negative when the button is DOWN and 0 when the button is UP
        ///</summary>
        ///<returns></returns>
        public static short GetLeftButtonState()
        {
            return GetAsyncKeyState(VK_LBUTTON);
        }



        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            if (!GetCursorPos(out lpPoint))
            {
                throw new IOException();
            }
            return lpPoint;
        }
    }
}
