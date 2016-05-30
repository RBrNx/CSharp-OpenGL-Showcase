using System;
using System.IO;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;

namespace C_Sharp_OpenGL
{
    class Window
    {

        static int g_width = 640;
        static int g_height = 480;

        //reported window size may be good to know for a few things
        static int g_win_width = g_width;
        static int g_win_height = g_height;

        //keep track of framebuffer size for things like the viewport and the mouse cursor
        public static int g_fb_width = g_width;
        public static int g_fb_height = g_height;

        public static void glfw_window_size_callback(GlfwWindowPtr window, int width, int height)
        {
            g_win_width = width;
            g_win_height = height;
            Log.GL_Log("GLFW Window Resize. Width: " + width + "     Height: " + height);
        }

        public static void glfw_framebuffer_resize_callback(GlfwWindowPtr window, int width, int height)
        {
            g_fb_width = width;
            g_fb_height = height;
            Log.GL_Log("GLFW Framebuffer Resize. Width: " + width + "     Height: " + height);

            /*later update any perspective matrices here*/
        }

        public static void initWindowHints()
        {
            Glfw.WindowHint(WindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 2);
            Glfw.WindowHint(WindowHint.OpenGLForwardCompat, 1);
            Glfw.WindowHint(WindowHint.Samples, 4);
        }

        static string WindowTitle;

        public static GlfwWindowPtr initWindow(string windowTitle)
        {
            GlfwMonitorPtr mon = Glfw.GetPrimaryMonitor();
            GlfwVidMode mode = Glfw.GetVideoMode(mon);
            GlfwWindowPtr window = Glfw.CreateWindow(g_width, g_height, windowTitle, GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
            Glfw.MakeContextCurrent(window);
            WindowTitle = windowTitle;

            //Get Version Info
            string renderer = GL.GetString(StringName.Renderer);
            string version = GL.GetString(StringName.Version);

            Log.GL_Log("Renderer: " + renderer);
            Log.GL_Log("Version: " + version);
            Log.GL_Log("-----------------------------------");

            return window;
        }

        static double prevSecs;
        static int frameCount;

        public static void updateFPSCounter(GlfwWindowPtr window)
        {
            double currentSecs = Glfw.GetTime();
            double elapsedSecs = currentSecs - prevSecs;
            
            //Limit text updates to 4 per second
            if(elapsedSecs > 0.25)
            {
                prevSecs = currentSecs;
                double fps = frameCount / elapsedSecs;
                Glfw.SetWindowTitle(window, WindowTitle + " - " + fps);
                frameCount = 0;
            }

            frameCount++;
        }
    }
}
