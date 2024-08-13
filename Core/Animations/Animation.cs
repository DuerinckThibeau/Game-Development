using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDev.Core.Animations
{
    public class Animation
    {
        public AnimationFrame currentFrame { get; set; }
        private List<AnimationFrame> frames;
        private int counter;
        private double secondCounter = 0;

        public Animation()
        {
            frames = new List<AnimationFrame>();
        }

        public void Update(GameTime gameTime) 
        {
            currentFrame = frames[counter];
            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            int fps = 10;

            if(secondCounter >= 1d / fps)
            {
                counter++;
                secondCounter = 0;
            }
            
            if(counter >= frames.Count)
            {
                counter = 0;
            }
        }

        public void AddAnimation(int sprites, int width, int height)
        {
            for(int x = 0; x < sprites * width; x += width)
            {
                frames.Add(new AnimationFrame(new Rectangle(x, 0, width, height)));
            }
        }
        
    }
}
