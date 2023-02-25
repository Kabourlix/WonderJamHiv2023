using System;
using UnityEngine;

namespace Game.Scripts.Utility
{
    public static class KUtils
    {
        public static System.Random Rnd = new System.Random();

        public static Vector2 xz(this Vector3 v) => v.x * Vector2.right + v.z * Vector2.up;
    }
}