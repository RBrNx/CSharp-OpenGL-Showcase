using System;
using System.IO;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;

namespace C_Sharp_OpenGL
{
    class Log
    {
        public static void restart_GL_Log()
        {
            string dateTime = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
            File.WriteAllText("GL_LOG.txt", "GL_LOG_FILE " + dateTime + Environment.NewLine);
        }

        public static void GL_Log(string message, bool timestamp = false)
        {
            string time = DateTime.Now.ToString("hh:mm:ss");
            if (timestamp)
            {
                File.AppendAllText("GL_LOG.txt", time + "    " + message + Environment.NewLine);
            }
            else
            {
                File.AppendAllText("GL_LOG.txt", message + Environment.NewLine);
            }
            
        }

        public static void glfw_error_callback(GlfwError code, string desc)
        {
            GL_Log("GLFW ERROR: Code: " + code + "    Message: " + desc);
        }

        public static void log_gl_params()
        {
            GetPName[] parameters =
            {
                GetPName.MaxCombinedTextureImageUnits,
                GetPName.MaxCubeMapTextureSize,
                GetPName.DrawBuffer,
                GetPName.MaxFragmentUniformComponents,
                GetPName.MaxTextureImageUnits,
                GetPName.MaxTextureSize,
                GetPName.MaxVaryingFloats,
                GetPName.MaxVertexAttribs,
                GetPName.MaxVertexTextureImageUnits,
                GetPName.MaxVertexUniformComponents,
                GetPName.MaxViewportDims,
                GetPName.Stereo,
            };

            string[] names =
            {
                "GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS",
                "GL_MAX_CUBE_MAP_TEXTURE_SIZE",
                "GL_MAX_DRAW_BUFFERS",
                "GL_MAX_FRAGMENT_UNIFORM_COMPONENTS",
                "GL_MAX_TEXTURE_IMAGE_UNITS",
                "GL_MAX_TEXTURE_SIZE",
                "GL_MAX_VARYING_FLOATS",
                "GL_MAX_VERTEX_ATTRIBS",
                "GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS",
                "GL_MAX_VERTEX_UNIFORM_COMPONENTS",
                "GL_MAX_VIEWPORT_DIMS",
                "GL_STEREO",
            };

            GL_Log("GL Context Params");
            for (int i = 0; i < 10; i++)
            {
                int j = 0;
                GL.GetInteger(parameters[i], out j);
                GL_Log(names[i] + ": " + j);
            }

            //Others
            int[] v = { 0, 0 };
            GL.GetInteger(parameters[10], v);
            GL_Log(names[10] + ": " + v[0] + " " + v[1]);
            bool s = false;
            GL.GetBoolean(parameters[11], out s);
            GL_Log(names[11] + ": " + s);
            GL_Log("-----------------------------------");
        }
    }
}
