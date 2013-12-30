using Microsoft.Xna.Framework;

namespace Common
{
    /// <summary>
    /// An entity used in the Game Engine. Base class for many classes. Contains a position
    /// and rotation (orientation)
    /// </summary>
    public class Object
    {
        /// <summary>
        /// Position (translation) of the object
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Rotation (orientation) of the object
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Rotate the object by the specified quaternion (not public)
        /// </summary>
        protected Quaternion RotateQ
        {
            set { Rotation = value * Rotation; }
        }

        /// <summary>
        /// Rotate the object on the X-axis
        /// </summary>
        public float RotateX
        {
            set
            {
                RotateQ = Quaternion.CreateFromAxisAngle(Vector3.UnitX, value);
            }
        }

        /// <summary>
        /// Rotate the object on the Y-axis
        /// </summary>
        public float RotateY
        {
            set
            {
                RotateQ = Quaternion.CreateFromAxisAngle(Vector3.UnitY, value);
            }
        }

        /// <summary>
        /// Rotate the object on the Z-Axis
        /// </summary>
        public float RotateZ
        {
            set
            {
                RotateQ = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, value);
            }
        }

        /// <summary>
        /// Default constructor. Sets the initial values for the parameters
        /// </summary>
        public Object()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
        }
    }
}
