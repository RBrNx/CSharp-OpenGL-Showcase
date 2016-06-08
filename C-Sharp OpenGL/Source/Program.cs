using System;
using System.IO;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System.Collections.Generic;

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
        static double prevSecs;

        public static void load_Shaders(ref GLuint vertex_shader, ref GLuint frag_shader, ref GLuint program)
        {
            double currentSecs = Glfw.GetTime();
            double elapsedSecs = currentSecs - prevSecs;

            if(program == 0)
            {
                Log.GL_Log("Create Shaders", true);

                vertex_shader = GL.CreateShader(ShaderType.VertexShader);
                string vertexShader = File.ReadAllText("test.vert");
                GL.ShaderSource(vertex_shader, vertexShader);
                GL.CompileShader(vertex_shader);
                Log.GL_Check_Compile((GLint)vertex_shader);

                frag_shader = GL.CreateShader(ShaderType.FragmentShader);
                string fragmentShader = File.ReadAllText("test.frag");
                GL.ShaderSource(frag_shader, fragmentShader);
                GL.CompileShader(frag_shader);
                Log.GL_Check_Compile((GLint)frag_shader);

                program = GL.CreateProgram();
                GL.AttachShader(program, vertex_shader);
                GL.AttachShader(program, frag_shader);
                GL.LinkProgram(program);
            }
            else if (elapsedSecs > 1.5)
            {
                prevSecs = currentSecs;

                //Log.GL_Log("Reload Shaders", true);

                string vertexShader = File.ReadAllText("test.vert");
                GL.ShaderSource(vertex_shader, vertexShader);
                GL.CompileShader(vertex_shader);
                Log.GL_Check_Compile((GLint)vertex_shader);

                string fragmentShader = File.ReadAllText("test.frag");
                GL.ShaderSource(frag_shader, fragmentShader);
                GL.CompileShader(frag_shader);
                Log.GL_Check_Compile((GLint)frag_shader);

                GL.AttachShader(program, vertex_shader);
                GL.AttachShader(program, frag_shader);
                GL.LinkProgram(program);
            }    
        }

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

            GLfloat[] colours =
            {
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f
            };

            Matrix matrix = new Matrix(1.0f, 0.0f, 0.0f, 0.0f,
                                        0.0f, 1.0f, 0.0f, 0.0f,
                                        0.0f, 0.0f, 1.0f, 0.0f,
                                        0.5f, 0.0f, 0.0f, 1.0f);

            GLint points_vbo = 0;
            GL.GenBuffers(1, out points_vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, points_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(points.Length * sizeof(GLfloat)), points, BufferUsageHint.StaticDraw);

            GLint colours_vbo = 0;
            GL.GenBuffers(1, out colours_vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colours_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colours.Length * sizeof(GLfloat)), colours, BufferUsageHint.StaticDraw);

            GLint vao = 0;
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, points_vbo);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colours_vbo);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            GLuint shader_program = 0;
            GLuint vertex_shader = 0;
            GLuint fragment_shader = 0;
            load_Shaders(ref vertex_shader, ref fragment_shader, ref shader_program);

            Log.print_all((GLint)shader_program);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);

            int matrix_location = GL.GetUniformLocation(shader_program, "matrix");
            GL.UseProgram(shader_program);
            GL.UniformMatrix4(matrix_location, false, ref matrix);

            float speed = 1.0f;
            float last_pos = 0.0f;
            double prev_secs = 0;

            while (!Glfw.WindowShouldClose(window)){
                double current_secs = Glfw.GetTime();
                double elapsed_secs = current_secs - prev_secs;
                prev_secs = current_secs;

                if (Math.Abs(last_pos) > 1.0f)
                {
                    speed = -speed;
                }

                matrix.M41 = (float)elapsed_secs * speed + last_pos;
                last_pos = matrix.M41;

                //Wipe the drawing surface clear
                Window.updateFPSCounter(window);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Viewport(0, 0, Window.g_fb_width, Window.g_fb_height);

                GL.UseProgram(shader_program);
                GL.BindVertexArray(vao);

                GL.UniformMatrix4(matrix_location, false, ref matrix);

                //draw points 0-3 from the currently bound VAO with current in-use shader
                GL.DrawArrays(BeginMode.Triangles, 0, 3);

                //update any other events like input handling
                Glfw.PollEvents();

                //Put the stuff we've been drawing onto the display
                Glfw.SwapBuffers(window);

                handle_input(window);

                //load_Shaders(ref vertex_shader, ref fragment_shader, ref shader_program);
            }

            Glfw.Terminate();
        }

        public static void handle_input(GlfwWindowPtr window)
        {

            bool test = Glfw.GetKey(window, Key.Three);

            if (Glfw.GetKey(window, Key.Escape))
            {
                Glfw.SetWindowShouldClose(window, true);
            }

            if(Glfw.GetKey(window, Key.One))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);                
            }

            if (Glfw.GetKey(window, Key.Two))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }

        }

        static void Main(string[] args)
        {
            var Program = new Program();
            Program.run();
        }
    }
}
