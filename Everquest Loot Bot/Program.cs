using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

public static class Input
{
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT point);

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    public static POINT GetMousePosition()
    {
        POINT pos;
        GetCursorPos(out pos);
        return pos;
    }
    public static Color GetPixelColor(int x, int y)
    {
        IntPtr desktopPtr = GetDC(IntPtr.Zero);
        uint color = GetPixel(desktopPtr, x, y);
        ReleaseDC(IntPtr.Zero, desktopPtr);

        // Extract the RGB components from the color value
        byte red = (byte)(color & 0xFF);
        byte green = (byte)((color >> 8) & 0xFF);
        byte blue = (byte)((color >> 16) & 0xFF);

        return Color.FromArgb(red, green, blue);
    }
    public static string ColorToHex(Color color)
    {
        return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
    }
}

class Program
{
    [DllImport("user32.dll")]
    public static extern bool GetAsyncKeyState(int button);

    public static bool IsMouseButtonPressed(MouseButton button)
    {
        return GetAsyncKeyState((int)button);
    }

    public enum MouseButton
    {
        LeftMouseButton = 0x01,
        RightMouseButton = 0x02,
        MiddleMouseButton = 0x04
    }

    static void Main()
    {
        Console.WriteLine("Listening for left mouse clicks. Press any key to exit.");

        string folderPath = @"C:\temp\EQTestFiles";
        string filePath = Path.Combine(folderPath, "mouse_positions2.csv");

        Directory.CreateDirectory(folderPath);

        int previousX = -1;
        int previousY = -1;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("x,y,R,G,B,Hex");
            while (!Console.KeyAvailable)
            {
                if (IsMouseButtonPressed(MouseButton.LeftMouseButton))
                {
                    Input.POINT mousePos = Input.GetMousePosition();
                    if (mousePos.x != previousX || mousePos.y != previousY)
                    {

                        Color pixelColor = Input.GetPixelColor(mousePos.x, mousePos.y);
                        string hexColor = Input.ColorToHex(pixelColor);
                        writer.WriteLine($"{mousePos.x},{mousePos.y},{pixelColor.R},{pixelColor.G},{pixelColor.B},{hexColor}");
                        Console.WriteLine($"Mouse position: X = {mousePos.x}, Y = {mousePos.y}");
                        Console.WriteLine($"Pixel color: R = {pixelColor.R}, G = {pixelColor.G}, B = {pixelColor.B} Hex color: {hexColor}");
                        previousX = mousePos.x;
                        previousY = mousePos.y;



                    }
                }
                Thread.Sleep(50);
            }
            writer.Flush();
        }
        
    }
}
