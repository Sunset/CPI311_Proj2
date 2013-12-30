using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    /// <summary>
    /// A GameObject thas has an associated Model
    /// </summary>
    public class ModelObject : GameObject
    {
        /// <summary>
        /// Model associated with this object
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Texture associated with this object
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Use the defualt draw method of the model
        /// </summary>
        /// <param name="view">The View Matrix to use</param>
        /// <param name="projection">The Projection Matrix to use</param>
        public override void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
            Model.Draw(World, view, projection);
        }

        /// <summary>
        /// Draw using a custom Effect
        /// </summary>
        /// <param name="effect">The Effect to use</param>
        public override void Draw(Effect effect)
        {
            base.Draw(effect);
            // Get the graphics device to use
            GraphicsDevice graphics = effect.GraphicsDevice;
            // Cycle through every pass in the current technique
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Enable this pass. An effect may have multiple passes
                pass.Apply();
                // Cycle through every mesh
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    // Cycle through every mesh part
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // Set the "Vertex Buffer" and "Index Buffer"
                        graphics.SetVertexBuffer(part.VertexBuffer);
                        graphics.Indices = part.IndexBuffer;
                        // Draw the primitives
                        graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, // Type of primitives
                            part.VertexOffset, 0, // Vertex indices for this part begins here
                            part.NumVertices, // Number of vertices in the Vertex Buffer
                            part.StartIndex, // Indices for this part starts here
                            part.PrimitiveCount); // Number of primitives to draw
                    }
                }
            }
        }
    }
}
