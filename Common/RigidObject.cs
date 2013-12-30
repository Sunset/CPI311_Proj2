using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class RigidObject : ModelObject
    {
        public static Color[] ColorBank = { Color.Red, Color.Green, Color.Blue, Color.Cyan, Color.Magenta, Color.Yellow, Color.Aqua };

        public Vector3 Velocity { get; set; }
        public float Mass { get; set; }
        public float Friction { get; set; }
        public Vector3 Gravity { get; set; }
        public Vector3 Force { get; set; }

        public Color Color { get; set; }

        public Vector3 AddForce
        {
            set
            {
                Force += value;
            }
        }

        public float Momentum
        {
            get { return Mass * Velocity.Length(); }
        }

        public RigidObject()
            : base()
        {
            Mass = 1;
            Friction = 0;
            Velocity = Vector3.Zero;
            Force = Vector3.Zero;
        }

        public void Update(GameTime gameTime, float gameSpeed)
        {
            
            Velocity += Force / Mass + Gravity / Mass * ((gameTime.ElapsedGameTime.Milliseconds * gameSpeed) / 1000f);
            Force = Vector3.Zero;
            Position += Velocity * gameTime.ElapsedGameTime.Milliseconds*gameSpeed / 1000f;
        }
    }
}
    