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
using System.Text;

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

        public static void GL_Check_Compile(GLint shader)
        {
            int param = -1;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out param);
            if(param != 1)
            {
                GL_Log("ERROR: GL Shader index " + shader + " did not complile");
                Print_Shader_Info_Log(shader);
            }
        }

        
        public static void GL_Check_Link(GLint program)
        {
            int param = -1;
            GL.GetProgram(program, ProgramParameter.LinkStatus, out param);
            if (param != 1)
            {
                GL_Log("ERROR: Could not link shader program GL Index " + program);
                Print_Program_Info_Log(program);
            }
        }

        public static void Print_Shader_Info_Log(GLint shader)
        {
            string error = GL.GetShaderInfoLog(shader);
            GL_Log("Shader info log for GL index " + shader + ":");
            GL_Log(error, true);
        }

        public static void Print_Program_Info_Log(GLint program)
        {
            string error = GL.GetProgramInfoLog(program);
            GL_Log("Program info log for GL index " + program + ":", true);
            GL_Log(error, true);
        }

        public static void print_all(GLint program)
        {
            GL_Log("--------------------");
            GL_Log("Shader Program " + program + " info:");
            int param = -1;
            GL.GetProgram(program, ProgramParameter.LinkStatus, out param);
            GL_Log("GL_LINK_STATUS = " + param, true);

            GL.GetProgram(program, ProgramParameter.AttachedShaders, out param);
            GL_Log("GL_ATTACHED_SHADERS = " + param, true);

            GL.GetProgram(program, ProgramParameter.ActiveAttributes, out param);
            GL_Log("GL_ACTIVE_ATTRIBUTES = " + param, true);
            for(int i = 0; i < param; i++)
            {
                ActiveAttribType type = 0;
                int size = 0;
                StringBuilder name = new StringBuilder();
                int length = 0;
                GL.GetActiveAttrib(program, i, 64, out length, out size, out type, name);
                if(size > 1)
                {
                    for(int j = 0; j < size; j++)
                    {
                        string long_name = name + "[" + j + "]";
                        int location = GL.GetAttribLocation(program, long_name);
                        GL_Log(i + ") Type: " + GL_type_to_string(type) + "   Name: " + long_name + "   Location: " + location, true);
                    }
                    
                }
                else
                {
                    string new_name = name + "";
                    int location = GL.GetAttribLocation(program, new_name);
                    GL_Log(i + ") Type: " + GL_type_to_string(type) + "   Name: " + new_name + "   Location: " + location, true);
                }
            }

            GL.GetProgram(program, ProgramParameter.ActiveUniforms, out param);
            GL_Log("GL_ACTIVE_UNIFORMS = " + param, true);
            for(int i = 0; i < param; i++)
            {
                ActiveUniformType type = 0;
                int size = 0;
                StringBuilder name = new StringBuilder();
                int length = 0;
                GL.GetActiveUniform(program, i, 64, out length, out size, out type, name);
                if(size > 1)
                {
                    for(int j = 0; j < size; j++)
                    {
                        string long_name = name + "[" + j + "]";
                        int location = GL.GetUniformLocation(program, long_name);
                        GL_Log(i + ") Type: " + GL_type_to_string(type) + "   Name: " + long_name + "   Location: " + location, true);
                    }
                }
                else
                {
                    string new_name = name + "";
                    int location = GL.GetUniformLocation(program, new_name);
                    GL_Log(i + ") Type: " + GL_type_to_string(type) + "   Name: " + new_name + "   Location: " + location, true);
                }
            }

            Print_Program_Info_Log(program);
        }

        public static string GL_type_to_string(ActiveUniformType type)
        {
            switch (type)
            {
                case ActiveUniformType.Bool: return "bool";
                case ActiveUniformType.Int: return "int";
                case ActiveUniformType.Float: return "float";
                case ActiveUniformType.FloatVec2: return "vec2";
                case ActiveUniformType.FloatVec3: return "vec3";
                case ActiveUniformType.FloatVec4: return "vec4";
                case ActiveUniformType.FloatMat2: return "mat2";
                case ActiveUniformType.FloatMat3: return "mat3";
                case ActiveUniformType.FloatMat4: return "mat4";
                case ActiveUniformType.Sampler2D: return "sampler2D";
                case ActiveUniformType.Sampler3D: return "sampler3D";
                case ActiveUniformType.SamplerCube: return "samplerCube";
                case ActiveUniformType.Sampler2DShadow: return "sampler2DShadow";
                default: break;
            }
            return "other";
        }

        public static string GL_type_to_string(ActiveAttribType type)
        {
            switch (type)
            {
                case ActiveAttribType.Int: return "int";
                case ActiveAttribType.Float: return "float";
                case ActiveAttribType.FloatVec2: return "vec2";
                case ActiveAttribType.FloatVec3: return "vec3";
                case ActiveAttribType.FloatVec4: return "vec4";
                case ActiveAttribType.FloatMat2: return "mat2";
                case ActiveAttribType.FloatMat3: return "mat3";
                case ActiveAttribType.FloatMat4: return "mat4";
                default: break;
            }
            return "other";
        }
    }
}
