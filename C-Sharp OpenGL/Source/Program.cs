using System;
using System.IO;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;

using GLbitfield = System.UInt32;
using GLboolean = System.Boolean;
using GLbyte = System.SByte;
using GLclampf = System.Single;
using GLdouble = System.Double;
using GLenum = System.UInt32;
using GLfloat = System.Single;
using GLint = System.Int32;
using GLshort = System.Int16;
using GLsizei = System.Int32;
using GLubyte = System.Byte;
using GLuint = System.UInt32;
using GLushort = System.UInt16;
using GLvoid = System.IntPtr;

namespace C_Sharp_OpenGL
{
    class Program
    {
        void run()
        {
            Log.restart_GL_Log();
            Log.GL_Log("Starting GLFW" + Environment.NewLine + Glfw.GetVersionString() + Environment.NewLine);
            Glfw.SetErrorCallback(Log.glfw_error_callback);

            if (!Glfw.Init()){
                Console.WriteLine("ERROR: Could not start GLFW3");
                Console.WriteLine("Press Any Key to Continue...");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            Window.initWindowHints();
            GlfwWindowPtr window = Window.initWindow("Extended Hello Triangle");

            Log.log_gl_params();

            Glfw.SetWindowSizeCallback(window, Window.glfw_window_size_callback);
            Glfw.SetFramebufferSizeCallback(window, Window.glfw_framebuffer_resize_callback);

            //Tell GL to only draw onto a pixel if the shape is closer to the viewer
            GL.Enable(EnableCap.DepthTest); //Enable Depth Testing
            GL.DepthFunc(DepthFunction.Less); //Depth Testing interprets a smallet value as "closer"

            /* OTHER STUFF GOES HERE */
            GLfloat[] points =
            {
                0.0f, 0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                -0.6f, -0.5f, 0.0f
            };

            GLint vbo = 0;
            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(points.Length * sizeof(GLfloat)), points, BufferUsageHint.StaticDraw);

            GLint vao = 0;
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GLuint vs = GL.CreateShader(ShaderType.VertexShader);
            string vertexShader = File.ReadAllText("test.vert");
            GL.ShaderSource(vs, vertexShader);
            GL.CompileShader(vs);
            GLuint fs = GL.CreateShader(ShaderType.FragmentShader);
            string fragmentShader = File.ReadAllText("test.frag");
            GL.ShaderSource(fs, fragmentShader);
            GL.CompileShader(fs);

            GLuint shader_program = GL.CreateProgram();
            GL.AttachShader(shader_program, vs);
            GL.AttachShader(shader_program, fs);
            GL.LinkProgram(shader_program);

            while (!Glfw.WindowShouldClose(window)){
                //Wipe the drawing surface clear
                Window.updateFPSCounter(window);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Viewport(0, 0, Window.g_fb_width, Window.g_fb_height);

                GL.UseProgram(shader_program);
                GL.BindVertexArray(vao);

                //draw points 0-3 from the currently bound VAO with current in-use shader
                GL.DrawArrays(BeginMode.Triangles, 0, 3);

                //update any other events like input handling
                Glfw.PollEvents();

                //Put the stuff we've been drawing onto the display
                Glfw.SwapBuffers(window);

                if(Glfw.GetKey(window, Key.Escape))
                {
                    Glfw.SetWindowShouldClose(window, true);
                }
            }

            Glfw.Terminate();
        }

        static void Main(string[] args)
        {
            var Program = new Program();
            Program.run();
        }
    }
}
