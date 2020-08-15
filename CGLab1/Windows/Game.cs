using CGLab1.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CGLab1.Windows
{
    public class Game : GameWindow
    {
        private int ElementBufferObject;
        private int VertexBufferObject;
        private int VertexArrayObject;
        private Shader Shader;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Game created!");
        }

        private float[] GetVertices()
        {
            return new float[] {
                    0.5f,  0.5f, 0.0f,  // top right
                    0.5f, -0.5f, 0.0f,  // bottom right
                    -0.5f, -0.5f, 0.0f,  // bottom left
                    -0.5f,  0.5f, 0.0f   // top left
            };
        }

        private uint[] GetIndices()
        {
            return new uint[] {
                    0, 1, 3,   // first triangle
                    1, 2, 3    // second triangle
            };
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) Exit();

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            var indices = this.GetIndices();
            GL.DrawElements(PrimitiveType.Polygon, indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            Console.WriteLine("Resizing...");

            // Resizing the ViewPort depending on window size, if size of the window changed.
            GL.Viewport(x: 0, y: 0, width: this.Width, height: this.Height);

            base.OnResize(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Console.WriteLine("Game loading...");

            //Set background
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //Generate Vertex Array
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            var indices = this.GetIndices();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            var vertices = this.GetVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var vertexPath = Path.Combine(outPutDirectory, "GLSL_Files\\shader.vert");
            var fragmentPath = Path.Combine(outPutDirectory, "GLSL_Files\\shader.frag");

            Shader = new Shader(vertexPath, fragmentPath);

            GL.VertexAttribPointer(Shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            Shader.Dispose();

            base.OnUnload(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Console.WriteLine("Game closing...");

            base.OnClosing(e);
        }
    }
}
