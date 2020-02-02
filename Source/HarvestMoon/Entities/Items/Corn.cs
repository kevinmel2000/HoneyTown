﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestMoon.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace HarvestMoon.Entities.Items
{
    public class Corn : Item
    {
        private readonly AnimatedSprite _sprite;

        public Corn(ContentManager content, Vector2 initialPosition)
            : base(initialPosition)
        {
            var cropItems = AnimationLoader.LoadAnimatedSprite(content,
                                                                "animations/iconSet",
                                                                "animations/cropItemsMap",
                                                                "cornItem",
                                                                1.0f / 7.5f,
                                                                false);



            _sprite = cropItems;


            _sprite.Play("corn_normal");

            X = initialPosition.X;
            Y = initialPosition.Y;


            SellPrice = 120;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Vector2(X, Y), 0.0f, new Vector2(1, 1));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _sprite.Update(gameTime);

            if (_dropped)
            {
                _sprite.Play("corn_broken");
            }

        }
    }
}
