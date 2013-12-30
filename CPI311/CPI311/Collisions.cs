using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Common;

namespace CPI311
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Collisions : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;        // Drawing 2D stuff
        SpriteFont textFont;            // Object for a font
        BasicEffect effect;
        Effect SimpleShading;
        Model sphereModel;

        float gameSpeed = 1;
        int collisionCount = 0;
        int prevCollisionCount = 0;
        int timeElapsed = 0;
        int ballCount = 0;
        Common.HeightMap plane;
        Camera camera;

        RigidObject[] lights = new RigidObject[3];

        //RigidObject[] spheres;
        List<RigidObject> spheres;
        Random random;
        KeyboardState prevState = Keyboard.GetState();
        // Simulate two Triangles
        Vector3[] vertices = { new Vector3(-10, -5, -10), new Vector3(10, -5, -10), new Vector3(10, -5, 10), new Vector3(-10, -5, 10),
                             new Vector3(-10, 15, -10), new Vector3(10, 15, -10), new Vector3(10, 15, 10), new Vector3(-10, 15, 10)};
        /*int[] indices = { 0, 2, 1, 
                          0, 3, 1, 
                          4, 6, 7, 
                          4, 5, 6, 
                          0, 7, 3, 
                          0, 4, 7, 
                          2, 5, 1, 
                          2, 6, 5};
        */
        int[] indices = { 0, 2, 1,   0, 3, 2, 
                            1, 6, 5,   1, 2, 6, 
                            5, 7, 4,   5, 6, 7, 
                            4, 3, 0,   4, 7, 3, 
                            4, 1, 5,   4, 0, 1, 
                            3, 6, 2, 3, 7, 6 };
        public Collisions()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            random = new Random();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textFont = Content.Load<SpriteFont>("Fonts/SegoeUI");    // Load the font
            sphereModel = Content.Load<Model>("Models/Sphere");

            spheres = new List<RigidObject>();
            spheres.Add(new RigidObject());
            spheres[0].Model = sphereModel;
            spheres[0].Texture = Content.Load<Texture2D>("Textures/Stripes");
            spheres[0].Position = new Vector3(-2, -3, 0) ;
            spheres[0].Velocity = Vector3.Zero;
            spheres[0].Mass = 10;

            spheres.Add(new RigidObject());
            spheres[1].Model = spheres[0].Model;
            spheres[1].Texture = spheres[0].Texture;
            spheres[1].Position = new Vector3(2, 3, 0);
            spheres[1].Velocity = new Vector3(-6, -3, 0);
            spheres[1].Mass = 1;
            ballCount = spheres.Count<RigidObject>();
            AddBall();
            ChoorseRandomLights();
            // TODO: use this.Content to load your game content here
            camera = new Camera();
            camera.Position = new Vector3(0, 0, -20);
            camera.AspectRatio = GraphicsDevice.Viewport.AspectRatio;

            plane = new Common.HeightMap(Content.Load<Texture2D>("Textures/asdf"),99);
            plane.Texture = Content.Load<Texture2D>("Textures/asdf");
            plane.Scale *= 50;
            plane.Position = new Vector3(0, -5, 0);
            
            new HeightMap(plane.Texture, 2);

            effect = new BasicEffect(GraphicsDevice);
            SimpleShading = Content.Load<Effect>("Effects/SimpleShading");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // Update both the spheres
            foreach (RigidObject sphere in spheres)
                sphere.Update(gameTime, gameSpeed);

            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.W))
                camera.Position += camera.Forward * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (keyboardState.IsKeyDown(Keys.S))
                camera.Position -= camera.Forward * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (keyboardState.IsKeyDown(Keys.A))
                camera.RotateY = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (keyboardState.IsKeyDown(Keys.D))
                camera.RotateY  = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (keyboardState.IsKeyDown(Keys.Space)&&prevState.IsKeyUp(Keys.Space))
                AddBall();

            Vector3 position = new Vector3(MathHelper.Clamp(camera.Position.X, -50, 50), 0, MathHelper.Clamp(camera.Position.Z, -50, 50));
            Vector2 tex = new Vector2((position.X + 50) / 100f, (position.Z + 50) / 100f);
            position.Y = plane.GetElevation(tex) / 1000f * plane.Scale.Y + 1;
            camera.Position = position;

            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            if (keyboardState.IsKeyDown(Keys.Multiply))//speed animation
                gameSpeed += 1.0f;
            if (keyboardState.IsKeyDown(Keys.Divide))//slow animation
                gameSpeed -= 1.0f;

            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (timeElapsed > 999)
            {
                timeElapsed -= 1000;
                prevCollisionCount = collisionCount;
                collisionCount = 0;
            }

            if (keyboardState.IsKeyDown(Keys.Tab) && prevState.IsKeyUp(Keys.Tab))
            {
                foreach (RigidObject sphere in spheres)
                    sphere.Color = RigidObject.ColorBank[random.Next(0, 7)];   
            }

            gameSpeed = MathHelper.Clamp(gameSpeed, 0, 3);//limitation on max min speed
            // Do a worst case (all pairwise) collision detection
            for (int i = 0; i < spheres.Count; i++)
            {
                for (int j = 0; j < indices.Length / 3; j++)
                {
                    TestCollision(spheres[i], vertices[indices[j * 3]], vertices[indices[j * 3 + 1]], vertices[indices[j * 3 + 2]]);
                    
                }
                for (int j = i + 1; j < spheres.Count; j++)
                {
                    TestCollision(spheres[i], spheres[j]);
                    
                }
            }
            //every 15 collisions change the color of ball.
            if (collisionCount == 50)
            {
                spheres.Clear();
                ballCount = 0;
            }

            prevState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Goldenrod);
            // Set the Depth Stencil State to default for 3D rendering
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            // Setup some colors for lighting
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
            effect.DiffuseColor = new Vector3(0.5f, 0.0f, 0.0f);
            effect.SpecularColor = new Vector3(0.0f, 0.5f, 0.0f);
            effect.SpecularPower = 10;
            effect.LightingEnabled = true; // Enable lighting!
            effect.TextureEnabled = true; // Enable texturing
            // Some parameters for the Directional lighting
            effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
            effect.DirectionalLight0.SpecularColor = Vector3.One;
            // Provide the different matrices
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            //--for simpleshading
            SimpleShading.Parameters["View"].SetValue(camera.View);
            SimpleShading.Parameters["Projection"].SetValue(camera.Projection);
            //SimpleShading.Parameters["CameraPosition"].SetValue(camera.Position);
            SimpleShading.Parameters["RedPosition"].SetValue(lights[0].Position);
            SimpleShading.Parameters["GreenPosition"].SetValue(lights[1].Position);
            SimpleShading.Parameters["BluePosition"].SetValue(lights[2].Position);
            // Cycle through all objects in the array
            foreach (RigidObject sphere in spheres)
            {
                SimpleShading.Parameters["World"].SetValue(sphere.World);
                SimpleShading.Parameters["BallColor"].SetValue(sphere.Color.ToVector4());
                effect.World = sphere.World;
                effect.Texture = sphere.Texture;
                effect.DiffuseColor = new Vector3(0, 1, 0);    
                sphere.Draw(SimpleShading);
            }
            //SimpleShading.Parameters["World"].SetValue(plane.World);
            effect.World = plane.World;
            effect.Texture = plane.Texture;
        //    plane.Draw(effect);
         //   plane1.Draw(effect);

            spriteBatch.Begin();    // First, start the sprite batch
            spriteBatch.DrawString(textFont, "Collisions detected in 1 second: " + prevCollisionCount + "\nCurrent collisions detected:" 
                + collisionCount + "\nBall count: " + ballCount, new Vector2(50, 50), Color.Black);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        /// <summary>
        /// Sphere / Sphere Collision
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void TestCollision(RigidObject first, RigidObject second)
        {
            // Simple sphere / sphere collision
            Vector3 direction = first.Position - second.Position;
            if (direction.Length() < first.Scale.X + second.Scale.X)
            {

                collisionCount++;
                float diff = direction.Length() - first.Scale.X - second.Scale.X;
                // First, normalize the direction vector
                direction.Normalize();
                // If the spheres are overlapping, push them out a little so they just touch
                first.Position -= diff / 2 * direction;
                second.Position += diff / 2 * direction;
                // Next, find the relative velocity between the two bodies
                Vector3 relativeVelocity = first.Velocity - second.Velocity;
                // Find the velocity in the direction of contact - use dot product
                float relativeVelocityPerp = -Vector3.Dot(relativeVelocity, direction);
                // Esoteric: Finally, the impulse is determined using this relationship
                float impulse = relativeVelocityPerp  / (1 / first.Mass + 1 / second.Mass);
                // Add the impulses in the correct direction to both bodies
                first.AddForce = 2* impulse * direction;
                second.AddForce = -2 * impulse * direction;
            }
        }

        /// <summary>
        /// Sphere / Triangle Collision
        /// </summary>
        /// <param name="first"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        public void TestCollision(RigidObject first, Vector3 A, Vector3 B, Vector3 C)
        {
            // First, make the sphere center our origin
            A = A - first.Position;
            B = B - first.Position;
            C = C - first.Position;
            // Find the surface normal for the triangle (using a cross product)
            Vector3 normal = Vector3.Cross(B - A, C - A);
            normal.Normalize();
            // Find how far away the sphere is from the triangle
            // We use a dot prouct to "project" Vector A on the normal
            float separation = (float)Math.Abs(Vector3.Dot(A, normal));
            // If the sphere is too far away, then quit
            if (separation > first.Scale.X)
                return;
            // Good, so now it is in range! Find the contact point on the plane of the triangle
            Vector3 P = separation * normal;
            // Esoteric: Determine if the point is actually in the triangle
            if (Vector3.Dot(Vector3.Cross(B - A, P - A), Vector3.Cross(C - A, P - A)) > 0 ||
                Vector3.Dot(Vector3.Cross(B - C, P - C), Vector3.Cross(A - C, P - C)) > 0)
                return;
            // If the sphere is actually moving away, then don't bother (dot product here
            if (Vector3.Dot(first.Velocity, normal) > 0)
                return;
            // Finally, add the impulse force to the sphere
            first.AddForce = -2 * Vector3.Dot(first.Velocity, normal) * normal * first.Mass;
            collisionCount++;
        }

        public void AddBall()
        {
            ballCount++;
            RigidObject sphere = new RigidObject();
            sphere.Scale *= (float)(random.NextDouble()+0.5f);
            sphere.Velocity = new Vector3((float)random.Next(10), (float)random.Next(10), (float)random.Next(10));
            sphere.Position = new Vector3((float)random.Next(10), (float)random.Next(10), (float)random.Next(10));
            sphere.Model = sphereModel;
            spheres.Add(sphere);
        }

        private void ChoorseRandomLights()
        {
            lights[0] = spheres[random.Next(0, spheres.Count)];
            lights[1] = spheres[random.Next(0, spheres.Count)];
            lights[2] = spheres[random.Next(0, spheres.Count)];
        }
    }
}
