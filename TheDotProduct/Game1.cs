using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace TheDotProduct {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            // Own implemention of projecting a vector onto a line
            // TODO: Make our own coordinates system, so we can move the origin (0,0)(top-left of screen) to anywhere we want
            // resource to do this (maybe): https://www.khanacademy.org/math/linear-algebra/alternate-bases/change-of-basis/v/linear-algebra-coordinates-with-respect-to-a-basis
            Vector2 origin = Vector2.Zero;
            //Vector2 origin = new Vector2(100, 300); // Origin;
            Vector2 pointB = new Vector2(origin.X + 700, origin.Y);
            Vector2 pointA = Mouse.GetState().Position.ToVector2();

            // Magnitude of line A
            float magnitudeA = (float)Math.Sqrt(Math.Pow(pointA.X, 2) + Math.Pow(pointA.Y, 2));

            // Magnitude of line B
            float magnitudeB = (float)Math.Sqrt(Math.Pow(pointB.X, 2) + Math.Pow(pointB.Y, 2)); ;

            // Dot product
            float dotProduct = (pointA.X * pointB.X) + (pointA.Y * pointB.Y);

            // Angle between line A and B
            Angle angleBetweenAB = new Angle((float)Math.Acos(dotProduct / (magnitudeA * magnitudeB)));
            //Angle angleBetweenAB = new Angle((float)Math.Atan2(pointACorrected.Y, pointACorrected.X)); // Another way of getting the angle

            // Vector Projection (without knowing the angle)
            Vector2 pointBNorm = new Vector2(pointB.X / magnitudeB, pointB.Y / magnitudeB); // Normalization
            Console.WriteLine($"pointBNorm: {pointBNorm}");
            dotProduct = (pointA.X * pointBNorm.X) + (pointA.Y * pointBNorm.Y);
            Vector2 vectorProjection = pointBNorm * dotProduct;
            float vectorProjectionLength = (float)Math.Sqrt(Math.Pow(vectorProjection.X, 2) + Math.Pow(vectorProjection.Y, 2));

            // Another way of getting the vector projection (if you know the angle)
            //float scalarProjectionLength = magnitudeA * (float)Math.Cos(angleBetweenAB.Radians);
            //Vector2 scalarProjection = new Vector2(scalarProjectionLength, origin.Y);

            // Compare with the Monogame.Extended library projection
            //Vector2 scalarProjection = pointA.ProjectOnto(pointB);
            //float scalarProjectionLength = Vector2.Distance(origin, scalarProjection)

            spriteBatch.DrawLine(origin, pointB, Color.Black, 4f);
            spriteBatch.DrawLine(origin, pointA, Color.LightGreen, 4f);
            spriteBatch.DrawLine(pointA, vectorProjection, Color.Red, 4f);

            spriteBatch.DrawString(font, $"Angle: {angleBetweenAB.Degrees}", new Vector2(300, 400), Color.Indigo);
            spriteBatch.DrawString(font, $"Magnitude A: {magnitudeA}", new Vector2(300, 420), Color.LightGreen);
            spriteBatch.DrawString(font, $"Magnitude B: {magnitudeB}", new Vector2(300, 440), Color.Black);
            spriteBatch.DrawString(font, $"Vector Projecten length: {vectorProjectionLength}", new Vector2(300, 460), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
