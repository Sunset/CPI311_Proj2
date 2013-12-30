using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    /// <summary>
    /// Class for a simple plane
    /// </summary>
    public class Plane : CustomObject
    {
        /// <summary>
        /// Constructor to generate the plane geometry
        /// </summary>
        /// <param name="segments">The number of rows/columns</param>
        public Plane(int segments = 1)
        {
            int rowCount = segments + 1;
            float fSegments = segments;
            // Initialize the arrays
            Indices = new short[6 * segments * segments];
            Vertices = new VertexPositionNormalTexture[(rowCount) * (rowCount)];
            // Populate the vertices
            for(int i = 0; i <= segments; i++)
                for (int j = 0; j <= segments; j++)
                {
                    Vertices[i * rowCount + j] = new VertexPositionNormalTexture(
                        new Vector3(-1 + 2 * j / fSegments, 0, -1 + 2 * i / fSegments), // Position
                        Vector3.Up, // Normal
                        new Vector2(j / fSegments, i / fSegments)); // Texture
                }
            // Populate the indices
            int index = 0;
            for(int i = 0; i < segments; i++)
                for (int j = 0; j < segments; j++)
                {
                    Indices[index++] = (short)(i * rowCount + j);
                    Indices[index++] = (short)(i * rowCount + j + 1);
                    Indices[index++] = (short)((i + 1) * rowCount + j + 1);
                    Indices[index++] = (short)(i * rowCount + j);
                    Indices[index++] = (short)((i + 1) * rowCount + j + 1);
                    Indices[index++] = (short)((i + 1) * rowCount + j);
                }
        }
    }
}
