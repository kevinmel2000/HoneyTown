﻿using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using HarvestMoon.Entities;
using System.Linq;
using MonoGame.Extended.Screens.Transitions;
using HarvestMoon.Entities.General;
using HarvestMoon.Entities.Town;

namespace HarvestMoon.Screens
{
    public class Mountain : Map
    {
        private HarvestMoon.Arrival _arrival;

        public Mountain(Game game, HarvestMoon.Arrival arrival)
            : base(game)
        {
            _arrival = arrival;
        }

        public override void Initialize()
        {
            // call base initialize func
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Load the compiled map
            _map = Content.Load<TiledMap>("maps/mountain/screen");
            // Create the map renderer
            _mapRenderer = new TiledMapRenderer(GraphicsDevice, _map);

            foreach (var layer in _map.ObjectLayers)
            {
                if (layer.Name == "objects")
                {
                    foreach (var obj in layer.Objects)
                    {
                        if (obj.Type == "player_start")
                        {

                        }

                        if (obj.Type == "npc")
                        {
                            var objectPosition = obj.Position;

                            objectPosition.X = obj.Position.X + obj.Size.Width * 0.5f;
                            objectPosition.Y = obj.Position.Y + obj.Size.Height * 0.5f;

                            var objectSize = obj.Size;

                            string objectMessage = "";

                            foreach (var property in obj.Properties)
                            {
                                if (property.Key.Contains("message"))
                                {
                                    objectMessage = HarvestMoon.Instance.Strings.Get(property.Value);
                                }
                            }

                            _entityManager.AddEntity(new BasicMessage(objectPosition, objectSize, objectMessage));
                        }

                    }
                }
                else if (layer.Name == "doors")
                {
                    foreach (var obj in layer.Objects)
                    {
                        if (obj.Type == "door")
                        {
                            var objectPosition = obj.Position;

                            objectPosition.X = obj.Position.X + obj.Size.Width * 0.5f;
                            objectPosition.Y = obj.Position.Y + obj.Size.Height * 0.5f;

                            var objectSize = obj.Size;

                            var door = new Door(objectPosition, objectSize);
                            _entityManager.AddEntity(door);

                            if (obj.Name == "passage")
                            {
                                door.OnTriggerEnd(() =>
                                {
                                    if (!door.Triggered)
                                    {
                                        door.Triggered = true;
                                        //var screen = new Passage(Game, HarvestMoon.Arrival.Mountain);
                                        //var transition = new FadeTransition(GraphicsDevice, Color.Black, 1.0f);
                                        //ScreenManager.LoadScreen(screen, transition);
                                    }
                                });
                            }
                        }

                    }
                }
                else if (layer.Name == "walls")
                {
                    foreach (var obj in layer.Objects)
                    {
                        if (obj.Type == "wall")
                        {

                            var objectPosition = obj.Position;

                            objectPosition.X = obj.Position.X + obj.Size.Width * 0.5f;
                            objectPosition.Y = obj.Position.Y + obj.Size.Height * 0.5f;

                            var objectSize = obj.Size;

                            _entityManager.AddEntity(new Wall(objectPosition, objectSize));
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO: Add your update logic here
            // Update the map
            // map Should be the `TiledMap`
            _mapRenderer.Update(gameTime);
            _entityManager.Update(gameTime);


            if (_player != null && !_player.IsDestroyed)
            {
                _camera.LookAt(_player.Position);

                var constraints = new Vector2();

                if (_camera.BoundingRectangle.Center.X < 320)
                {
                    constraints.X = 320;
                }

                if (_camera.BoundingRectangle.Center.X > 1408)
                {
                    constraints.X = 1408;
                }

                if (_camera.BoundingRectangle.Center.Y < 240)
                {
                    constraints.Y = 240;
                }

                if (_camera.BoundingRectangle.Center.Y > 1444)
                {
                    constraints.Y = 1444;
                }

                if (constraints.X != 0)
                {
                    _camera.LookAt(new Vector2(constraints.X, _player.Position.Y));
                }

                if (constraints.Y != 0)
                {
                    _camera.LookAt(new Vector2(_player.Position.X, constraints.Y));
                }

                if (constraints.X != 0 && constraints.Y != 0)
                {
                    _camera.LookAt(new Vector2(constraints.X, constraints.Y));
                }
            }

            CheckCollisions();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            float scaleY = HarvestMoon.Instance.Graphics.GraphicsDevice.Viewport.Height / 480.0f;

            var cameraMatrix = _camera.GetViewMatrix();
            cameraMatrix.Translation = new Vector3(cameraMatrix.Translation.X, cameraMatrix.Translation.Y - 32 * scaleY, cameraMatrix.Translation.Z);

            _spriteBatch.Begin(transformMatrix: cameraMatrix, samplerState: SamplerState.PointClamp);


            for (int i = 0; i < _map.Layers.Count; ++i)
            {
                _mapRenderer.Draw(_map.Layers[i], cameraMatrix);

            }
            // End the sprite batch
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            _entityManager.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());

            foreach (var layer in _map.Layers.Where(l => l.Name.Contains("Foreground")))
            {
                _mapRenderer.Draw(layer, cameraMatrix);
            }

            _spriteBatch.End();


            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());

            var interactables = _entityManager.Entities.Where(e => e is Interactable).Cast<Interactable>().ToArray();

            foreach (var interactable in interactables)
            {
                _spriteBatch.DrawRectangle(interactable.BoundingRectangle, Color.Fuchsia);
            }
            _spriteBatch.End();


            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            _spriteBatch.DrawRectangle(_player.BoundingRectangle, Color.Fuchsia);
            _spriteBatch.DrawRectangle(_player.ActionBoundingRectangle, Color.Fuchsia);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
