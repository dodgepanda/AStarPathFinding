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

namespace AStarPathFinding
{
    //
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const float BOARD_OFFSET_X = 0, BOARD_OFFSET_Y = 0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport vp;
        Texture2D squareSprite;
        Texture2D circleSprite;
        Texture2D starSprite;
        bool mouseDown = false;
        Grid grid;
        GridTerrain selectedTerrain = GridTerrain.NONE;
        GridSquare start, end;
        PathFinder pf;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 600;
            grid = new Grid(12, 12);

            //TESTING AREA
            start = grid.GetSquares()[0, 0];
            end = grid.GetSquares()[11, 11];
            //END TESTING AREA
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
            vp = graphics.GraphicsDevice.Viewport;
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

            squareSprite = Content.Load<Texture2D>(@"Sprites\square");
            circleSprite = Content.Load<Texture2D>(@"Sprites\circle");
            starSprite = Content.Load<Texture2D>(@"Sprites\star");
            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D1)) { selectedTerrain = GridTerrain.GRASS; }
            if (Keyboard.GetState().IsKeyDown(Keys.D2)) { selectedTerrain = GridTerrain.WATER; }
            if (Keyboard.GetState().IsKeyDown(Keys.D3)) { selectedTerrain = GridTerrain.MOUNTAIN; }
            if (Keyboard.GetState().IsKeyDown(Keys.D0)) { selectedTerrain = GridTerrain.NONE; }
            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde)) { grid.Clear(); }
            int mouseX = 0;
            int mouseY = 0;
            bool selectStart = false;
            bool selectEnd = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) { selectStart = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.W)) { selectEnd = true; }
            // TODO: Add your update logic here
            if (Mouse.GetState().LeftButton == ButtonState.Pressed )
            {
                mouseDown = true;
                mouseX = Mouse.GetState().X;
                mouseY = Mouse.GetState().Y;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                mouseDown = false;

            if (mouseDown)
            {
                Rectangle mouseRec = new Rectangle(mouseX, mouseY, 0, 0);
                for (int i = 0; i < grid.GetWidth(); i++)
                {
                    for (int j = 0; j < grid.GetHeight(); j++)
                    {
                        Rectangle gridRec = new Rectangle(i * squareSprite.Width, j * squareSprite.Height, squareSprite.Width, squareSprite.Height);
                        if(mouseRec.Intersects(gridRec))
                        {
                            if (selectEnd || selectStart)
                            {
                                if (selectStart)
                                    start = grid.GetSquares()[i, j];
                                else
                                    end = grid.GetSquares()[i, j];
                            }
                            else
                                grid.GetSquares()[i, j].Terrain = selectedTerrain;                            
                        }
                    }
                }
            }

            // TODO: Add your update logic here            
            pf = new PathFinder(grid, start, end);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();  //Use spriteBatch to draw offscreen
            // TODO: Add your drawing code here
            for (int i = 0; i < grid.GetWidth(); i++)
            {
                for (int j = 0; j < grid.GetHeight(); j++)
                {
                    spriteBatch.Draw(squareSprite, new Vector2(i*squareSprite.Width, j*squareSprite.Height), grid.GetSquares()[i,j].GetColor());
                }
            }
            foreach (GridSquare cs in pf.GetClosedSet())
            {
                spriteBatch.Draw(circleSprite, new Vector2(cs.GetX() * squareSprite.Width, cs.GetY() * squareSprite.Height), Color.DarkBlue);
            }
            foreach (GridSquare os in pf.GetOpenSet())
            {
                spriteBatch.Draw(circleSprite, new Vector2(os.GetX() * squareSprite.Width, os.GetY() * squareSprite.Height), Color.LightBlue);
            }
            spriteBatch.Draw(circleSprite, new Vector2(start.GetX() * squareSprite.Width, start.GetY() * squareSprite.Height), Color.Green);
            spriteBatch.Draw(circleSprite, new Vector2(end.GetX() * squareSprite.Width, end.GetY() * squareSprite.Height), Color.Red);

            foreach (GridSquare path in pf.GetPath())
            {
                spriteBatch.Draw(circleSprite, new Vector2(path.GetX() * squareSprite.Width, path.GetY() * squareSprite.Height), Color.Yellow);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
