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
            this.IsMouseVisible = true;

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
            Matrix viewMatrix = Matrix.CreateTranslation(new Vector3(400, 200, 0));

            Vector2 origin = Vector2.Zero;
            Vector2 pointB = new Vector2(origin.X + 200, origin.Y);
            Vector2 pointA = Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(viewMatrix));
            Angle angleBC = new Angle((float)Math.PI / 4);
            Vector2 pointC = pointB.Rotate(angleBC);
            Vector2 pointC2 = pointB.Rotate(-angleBC);

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

            // Draw before transform to view space (so in world space)
            DrawArc(magnitudeB, angleBC, colorBC, Matrix.Identity); // Identity matrix doesn't do any translation
            DrawLines(origin, pointA, pointB, pointC, pointC2, pointAColor, colorC);
            DrawPoints(origin, pointA, pointB, pointC, pointC2);

            origin = Vector2.Transform(origin, viewMatrix);
            pointB = Vector2.Transform(pointB, viewMatrix);
            pointA = Vector2.Transform(pointA, viewMatrix);
            pointC = Vector2.Transform(pointC, viewMatrix);
            pointC2 = Vector2.Transform(pointC2, viewMatrix);

            // Draw after transform to view space (so in view space)
            DrawArc(magnitudeB, angleBC, colorBC, viewMatrix);
            DrawLines(origin, pointA, pointB, pointC, pointC2, pointAColor, colorC);
            DrawPoints(origin, pointA, pointB, pointC, pointC2);

            spriteBatch.DrawString(font, $"angleBA: {angleBA.Degrees}", new Vector2(400, 430), pointAColor);
            spriteBatch.DrawString(font, $"angleBC: {angleBC.Degrees}", new Vector2(400, 450), colorBC);

            spriteBatch.DrawString(font, "Checks if pointA is within the segment BC\n" +
                "using Dot Product", new Vector2(10, 440), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawPoints(Vector2 origin, Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointC2) {
            // Draw points
            Vector2 offset = new Vector2(5, 5);
            spriteBatch.DrawPoint(origin, Color.Red, 10f);
            spriteBatch.DrawString(font, "origin", origin + offset, Color.Black);
            spriteBatch.DrawPoint(pointA, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointA", pointA + offset, Color.Black);
            spriteBatch.DrawPoint(pointB, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointB", pointB + offset, Color.Black);
            spriteBatch.DrawPoint(pointC, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointC", pointC + offset, Color.Black);
            spriteBatch.DrawPoint(pointC2, Color.Red, 10f);
            spriteBatch.DrawString(font, "pointC2", pointC2 + offset, Color.Black);
        }

        private void DrawLines(Vector2 origin, Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointC2, Color pointAColor, Color colorC) {
            // Draw lines
            spriteBatch.DrawLine(origin, pointA, pointAColor, 3f);
            spriteBatch.DrawLine(origin, pointB, Color.Black, 3f);
            spriteBatch.DrawLine(origin, pointC, colorC, 3f);
            spriteBatch.DrawLine(origin, pointC2, colorC, 3f);
        }

        private void DrawArc(float magnitudeB, Angle angleBC, Color colorBC, Matrix viewMatrix) {
            // Draw arc
            for (int i = 0; i < angleBC.Degrees * 2; i += 5) {
                float r = magnitudeB; // Radius
                float _angle = i + angleBC.Degrees; // We need to offset i, because appearently angle 0 is pointing down from origin
                float angle = new Angle(_angle, AngleType.Degree).Radians;
                float x = r * (float)Math.Sin(angle);
                float y = r * (float)Math.Cos(angle);
                Vector2 point = new Vector2(x, y);
                point = Vector2.Transform(point, viewMatrix);
                spriteBatch.DrawPoint(point, colorBC, 5f);
            }
        }
    }
}
