using GameDev.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameDev.Core.GameManagers
{
    public class MovementManager
    {

        public void Move(IMovable movable)
        {
            var direction = movable.InputReader.ReadInput();
            var distance = direction * movable.Speed;
            movable.Position += distance;
        }


    }
}
