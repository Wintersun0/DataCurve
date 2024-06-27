using System;
using System.Runtime.InteropServices;

namespace UIResource
{
    class DataCurveFrame
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreatePen(int style, int width, int color);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeleteObject(HandleRef obj);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetStockObject(int index);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Rectangle(HandleRef dc, int left, int top, int right, int bottom);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SelectObject(HandleRef dc, HandleRef obj);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetBkColor(HandleRef dc, int color);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetROP2(HandleRef dc, int drawMode);
    }


    /// <summary>
    /// The possible pen styles for creating pens.
    /// </summary>
    enum PenStyle : int
    {
        Solid = 0,

        Dash = 1, /* -------  */

        Dot = 2, /* .......  */

        DashDot = 3, /* _._._._  */

        DashDotDot = 4, /* _.._.._  */

        Invisible = 5,

        InsideFrame = 6,
    }

    /// <summary>
    /// The available raster operation types.
    /// </summary>
    enum RasterOperation
    {
        Black = 1, /*  0       */

        NOTMERGEPEN = 2, /* DPon     */

        MASKNOTPEN = 3, /* DPna     */

        NOTCOPYPEN = 4, /* PN       */

        MASKPENNOT = 5, /* PDna     */

        NOT = 6, /* Dn       */

        XORPEN = 7, /* DPx      */

        NOTMASKPEN = 8, /* DPan     */

        MASKPEN = 9, /* DPa      */

        NOTXORPEN = 10, /* DPxn     */

        NOP = 11, /* D        */

        MERGENOTPEN = 12, /* DPno     */

        COPYPEN = 13, /* P        */

        MERGEPENNOT = 14, /* PDno     */

        MERGEPEN = 15, /* DPo      */

        WHITE = 16, /*  1       */

        LAST = 16,
    }

    enum StockObject : int
    {
        WhiteBrush = 0,

        LightGrayBrush = 1,

        GrayBrush = 2,

        DarkGrayBrush = 3,

        BlackBrush = 4,

        NullBrush = 5,

        WhitePen = 6,

        BlackPen = 7,

        NullPen = 8,

        OemFixedFont = 10,

        AnsiFixedFont = 11,

        AnsiVaribleFont = 12,

        SystemFont = 13,

        DeviceDefaultFont = 14,

        DefaultPalette = 15,

        SystemFixedFont = 16,
    }
}
