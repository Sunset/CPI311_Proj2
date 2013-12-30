using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    /// <summary>
    /// Class for a simple plane
    /// </summary>
    public class HeightMap : CustomObject
    {

        public Texture2D TextureMap { get; set; }
        public float ZeroLevel { get; set; }
        public float ElevationScale { get; set; }
        private Color[] HeightMapData { get; set; }
        /// <summary>
        /// Constructor to generate the plane geometry
        /// </summary>
        /// <param name="segments">The number of rows/columns</param>
        public HeightMap(Texture2D heightMap, int segments = 1)
        {
            TextureMap = heightMap;//store the heightap
            HeightMapData=new Color[TextureMap.Width*TextureMap.Height];
            TextureMap.GetData<Color>(HeightMapData);

            int rowCount = segments + 1;
            float fSegments = segments;
            // Initialize the arrays
            Indices = new short[6 * segments * segments];
            Vertices = new VertexPositionNormalTexture[(rowCount) * (rowCount)];
            // Populate the vertices
            for (int i = 0; i <= segments; i++)
                for (int j = 0; j <= segments; j++)
                {
                    Vector2 textureCoord = new Vector2(j / fSegments, i / fSegments);
                    Vertices[i * rowCount + j] = new VertexPositionNormalTexture(
                        new Vector3(-1 + 2 * j / fSegments,GetElevation(textureCoord)/2000f, -1 + 2 * i / fSegments), // Position
                        Vector3.Up, // Normal
                        new Vector2(j / fSegments, i / fSegments)); // Texture
                }
            // Populate the indices
            int index = 0;
            for (int i = 0; i < segments; i++)
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
        public float GetElevation(Vector2 textureCoords)
        {
            int imageRow = (int)(textureCoords.Y * (TextureMap.Height - 1));
            int imageCol = (int)(textureCoords.X * (TextureMap.Width - 1));
            Color color = HeightMapData[imageRow * TextureMap.Width + imageCol];
            return color.R;
        }
    }
}
