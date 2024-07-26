using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDev.Core.Animations
{
    public class AnimationFrame
    {

        public Rectangle sourceRectangle { get; set; }

        public AnimationFrame(Rectangle rectangle)
        {
            sourceRectangle = rectangle;
        }
    }
}
