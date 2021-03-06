using UnityEngine;
using System.Collections;
namespace Gaminho
{
    public static class Statics
    {

        public enum FacingDirection { Up = 0, Down = 180, Right = 270, Left = 90 }
        public enum TYPE_SHOT { SIMPLE, DOUBLE, TRIPLE };
        public enum TYPE_ENEMY { METEOR, SHIP, PIECES };
        public enum TYPE_DROP { SHOT1, SHOT2, SHOT3, SHIELD,LIFEFULL };
        
        public const string PREFAB_LEVELUP = "LevelUp";
        public const string PREFAB_CONGRATULATION = "Congratulations";
        public const string PREFAB_GAMEOVER = "GameOver";
        public const string PREFAB_LOAD = "Loading";
        public const string PREFAB_HISTORY = "History";
        public const string PLAYERPREF_NEWRECORD = "NewRecord";
        public const string PLAYERPREF_VALUE = "ValueRecord";
        //Which shot is currently in use
        public static int ShootingSelected = 2;
        //Whether it is shielded or not
        public static bool WithShield = false;

        public static int Damage = 0;
        //Level
        public static int CurrentLevel = 2;

        public static Transform Player;

        public static Transform GhostPlayer;
        
        public static int EnemiesDead = 0;

        public static int Points = 0;

        public static float Life;

        public static int Shield;
        //player has ghost upgrade or not
        public static bool withGost;
        //Do a LookAt for 2D
        public static Quaternion FaceObject(Vector2 myPos, Vector2 targetPos, FacingDirection facing)
        {
            Vector2 direction = targetPos - myPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= (float)facing;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Quaternion dodgeObject(Vector2 myPos, Vector2 targetPos, FacingDirection facing)
        {
            Vector2 direction = targetPos - myPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= (float)facing;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
        {
            if (Time.deltaTime < Mathf.Epsilon) return rot;
            // account for double-cover
            var Dot = Quaternion.Dot(rot, target);
            var Multi = Dot > 0f ? 1f : -1f;
            target.x *= Multi;
            target.y *= Multi;
            target.z *= Multi;
            target.w *= Multi;
            // smooth damp (nlerp approx)
            var Result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
            ).normalized;

            // ensure deriv is tangent
            var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
            deriv.x -= derivError.x;
            deriv.y -= derivError.y;
            deriv.z -= derivError.z;
            deriv.w -= derivError.w;

            return new Quaternion(Result.x, Result.y, Result.z, Result.w);
        }
    }

    
}
