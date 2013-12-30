using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    /// <summary>
    /// A GameObject is a entity that will partake in the events of the game. Specifically,
    /// they are visual and have the additional property of Scale. Provides and World matrix
    /// </summary>
    public class GameObject : Object
    {
        /// <summary>
        /// Scale for this entity
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// World matrix for the object. Uses the position, rotation, and scale parameters
        /// in usual order
        /// </summary>
        public virtual Matrix World
        {
            get
            {
                return Matrix.CreateScale(Scale) *
                    Matrix.CreateFromQuaternion(Rotation) *
                    Matrix.CreateTranslation(Position);
            }
        }

        /// <summary>
        /// Draw the object. Note: World matrix is implicit
        /// </summary>
        /// <param name="view">The View (Camera) matrix to use</param>
        /// <param name="projection"> The Projection matrix to use</param>
        public virtual void Draw(Matrix view, Matrix projection) { }

        /// <summary>
        /// Draw the object using an Effect. The World matrix must be
        /// set before calling this method.
        /// </summary>
        /// <param name="effect">The effect to use</param>
        public virtual void Draw(Effect effect) { }

        /// <summary>
        /// The Default constructor.
        /// </summary>
        public GameObject()
            : base()
        {
            Scale = Vector3.One;
        }
    }
}
