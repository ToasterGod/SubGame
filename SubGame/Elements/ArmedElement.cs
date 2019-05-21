using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigModel;
using Microsoft.Xna.Framework;

namespace SubGame.Elements
{
    public class ArmedElement : MovingElement
    {
        protected LevelDifficulty myMovementDifficulty;
        protected LevelDifficulty myWeaponDifficulty;

        public ArmedElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager, LevelDifficulty aMovementDifficulty, LevelDifficulty aWeaponDifficulty) 
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
            myMovementDifficulty = aMovementDifficulty;
            myWeaponDifficulty = aWeaponDifficulty;

            if (this is MineElement || this is SinkBombElement)
            {
                switch (aWeaponDifficulty)
                {
                    case LevelDifficulty.Easy:
                        AccessSpeed *= 10.1f;
                        break;
                    case LevelDifficulty.Normal:
                        AccessSpeed *= 1f;
                        break;
                    case LevelDifficulty.Hard:
                        AccessSpeed *= 0.75f;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (aMovementDifficulty)
                {
                    case LevelDifficulty.Easy:
                        AccessSpeed *= 3.5f;
                        break;
                    case LevelDifficulty.Normal:
                        AccessSpeed *= 1f;
                        break;
                    case LevelDifficulty.Hard:
                        AccessSpeed *= 0.5f;
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}
