using Microsoft.Xna.Framework;

namespace Common
{
    /// <summary>
    /// Class that models Camera parameters. Provides both the View and
    /// Projection matrices in the game.
    /// </summary>
    public class Camera : Object
    {
        /// <summary>
        /// The default forward direction for this camera
        /// </summary>
        public Vector3 LookAt { get; set; }

        /// <summary>
        /// The Up direction for this camera
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// The current forward direction for this camera. Changes as one
        /// rotates the camera
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(LookAt, 
                    Matrix.CreateFromQuaternion(Rotation));
            }
        }

        /// <summary>
        /// Field of View for the projection (in radians)
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Aspect Ratio for the projection
        /// </summary>
        public float AspectRatio { get; set; }

        /// <summary>
        /// The near plane for the view frustum
        /// </summary>
        public float NearPlane { get; set; }

        /// <summary>
        /// The far plane for the view frustum
        /// </summary>
        public float FarPlane { get; set; }

        /// <summary>
        /// Provides the view matrix using the stored parameters
        /// </summary>
        public Matrix View
        {
            get
            {
                Matrix rotation = Matrix.CreateFromQuaternion(Rotation);
                return Matrix.CreateLookAt(Position,
                    Position + Vector3.Transform(LookAt, rotation),
                    Vector3.Transform(Up, rotation));
            }
        }

        /// <summary>
        /// Provides a projection matrix using stored parameters
        /// </summary>
        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                    FieldOfView, AspectRatio,
                    NearPlane, FarPlane);
            }
        }

        /// <summary>
        /// The Default constructor
        /// </summary>
        public Camera()
            : base()
        {
            LookAt = Vector3.UnitZ;
            Up = Vector3.UnitY;

            FieldOfView = MathHelper.PiOver2;
            AspectRatio = 1;
            NearPlane = 1;
            FarPlane = 100;
        }
    }
}
