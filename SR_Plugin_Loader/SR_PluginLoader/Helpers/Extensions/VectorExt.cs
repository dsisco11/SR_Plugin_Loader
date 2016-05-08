
using UnityEngine;

namespace SR_PluginLoader
{
    public static class VectorExt
    {
        /// <summary>
        /// Returns a new vector with the same values as the given one.
        /// </summary>
        public static Vector2 Clone(this Vector2 v) { return new Vector2(v.x, v.y); }
        /// <summary>
        /// Returns a new vector with the same values as the given one.
        /// </summary>
        public static Vector3 Clone(this Vector3 v) { return new Vector3(v.x, v.y, v.z); }

        /// <summary>
        /// Returns TRUE if X or Y is a nonzero value
        /// </summary>
        public static bool NonZero(this Vector2 v) { return (!Util.floatEq(0f, v.x) || !Util.floatEq(0f, v.y)); }
        /// <summary>
        /// Returns TRUE if X or Y is a nonzero value
        /// </summary>
        public static bool NoZero(this Vector2 v) { return (!Util.floatEq(0f, v.x) && !Util.floatEq(0f, v.y)); }

        /// <summary>
        /// Returns TRUE if X, Y, or Z is a nonzero value
        /// </summary>
        public static bool NonZero(this Vector3 v) { return (!Util.floatEq(0f, v.x) || !Util.floatEq(0f, v.y) || !Util.floatEq(0f, v.z)); }
        /// <summary>
        /// Returns TRUE if X, Y, or Z is a nonzero value
        /// </summary>
        public static bool NoZero(this Vector3 v) { return (!Util.floatEq(0f, v.x) && !Util.floatEq(0f, v.y) && !Util.floatEq(0f, v.z)); }
    }
}
