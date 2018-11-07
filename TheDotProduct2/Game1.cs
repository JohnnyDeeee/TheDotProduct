using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace TheDotProduct2 {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            this.IsMouseVisible = false;

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
        }


        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            // Visualise: check if pointA is within segment BC
            Vector2 origin = Vector2.Zero;
            //Vector2 origin = new Vector2(300, 250);
            Vector2 pointB = new Vector2(500, origin.Y);
            Vector2 pointA = Mouse.GetState().Position.ToVector2();
            Angle angleBC = new Angle((float)Math.PI / 4);
            Vector2 pointC = pointB.Rotate(angleBC);

            float magnitudeB = (float)Math.Sqrt(Math.Pow(pointB.X, 2) + Math.Pow(pointB.Y, 2));
            float magnitudeA = (float)Math.Sqrt(Math.Pow(pointA.X, 2) + Math.Pow(pointA.Y, 2));
            float dotProduct = (pointB.X * pointA.X) + (pointB.Y * pointA.Y);
            Angle angleBA = new Angle((float)Math.Acos(dotProduct / (magnitudeB * magnitudeA)));

            // Define some colors
            Color colorC = new Color(9, 132, 227);
            Color colorBC = new Color(116, 185, 255);
            Color pointAColor = Color.Red;
            if (angleBA < angleBC && magnitudeA < magnitudeB)
                pointAColor = Color.Green; // pointA is inbetween B and C

            // Draw arc
            for (int i = 0; i < angleBC.Degrees; i++) {
                float r = magnitudeB; // Radius
                float _angle = i + angleBC.Degrees; // We need to offset i, because appearently angle 0 is pointing down from origin
                float angle = new Angle(_angle, AngleType.Degree).Radians;
                float x = r * (float)Math.Sin(angle);
                float y = r * (float)Math.Cos(angle);
                Vector2 point = new Vector2(x, y);
                spriteBatch.DrawPoint(point, colorBC, 5f);
            }

            spriteBatch.DrawLine(origin, pointA, pointAColor, 3f);
            spriteBatch.DrawLine(origin, pointB, Color.Black, 3f);
            spriteBatch.DrawLine(origin, pointC, colorC, 3f);

            Vector2 offset = new Vector2(5, 5);
            spriteBatch.DrawPoint(origin, Color.Red, 10f);
            spriteBatch.DrawString(font, "origin", origin + offset, Color.Black);
            spriteBatch.DrawPoint(pointA, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointA", pointA + offset, Color.Black);
            spriteBatch.DrawPoint(pointB, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointB", pointB + offset, Color.Black);
            spriteBatch.DrawPoint(pointC, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointC", pointC + offset, Color.Black);

            spriteBatch.DrawString(font, $"angleBA: {angleBA.Degrees}", new Vector2(400, 430), pointAColor);
            spriteBatch.DrawString(font, $"angleBC: {angleBC.Degrees}", new Vector2(400, 450), colorBC);

            spriteBatch.DrawString(font, "Checks if pointA is within the segment BC\n" +
                "using Dot Product", new Vector2(10, 440), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
