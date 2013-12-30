using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    /// <summary>
    /// Class that supports working with custom geometry
    /// </summary>
    public class CustomObject : GameObject
    {
        /// <summary>
        /// The Vertex buffer for the geometry
        /// </summary>
        public VertexPositionNormalTexture[] Vertices { get; set; }

        /// <summary>
        /// The Index buffer for the geometry
        /// </summary>
        public short[] Indices { get; set; }

        /// <summary>
        /// The texture to apply on the geometry
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Draw using a custom Effect
        /// </summary>
        /// <param name="effect">Effect to use</param>
        public override void Draw(Effect effect)
        {
            base.Draw(effect);
            // Get the graphics device in use
            GraphicsDevice graphics = effect.GraphicsDevice;
            // Cycle through every pass
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Enable the pass
                pass.Apply();
                // Draw the geometry
                graphics.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, // Primitive Type
                    Vertices, 0, Vertices.Length, // Vertex location, and number of vertices
                    Indices, 0, Indices.Length / 3); // Indices location, and number of triangles
            }
        }
    }
}
