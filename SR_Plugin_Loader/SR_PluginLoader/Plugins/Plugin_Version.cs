using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public class Plugin_Version
    {
        public int major;
        public int minor;
        public int patch;

        public Plugin_Version(int _major, int _minor = 0)
        {
            this.major = _major;
            this.minor = _minor;
            this.patch = 0;
        }

        public Plugin_Version(int _major, int _minor, int _patch)
        {
            this.major = _major;
            this.minor = _minor;
            this.patch = _patch;
        }

        public override string ToString()
        {
            return String.Format("v{0}.{1}.{2}", this.major, this.minor, this.patch);
        }

        public static bool operator ==(Plugin_Version v1, Plugin_Version v2)
        {
            return v1.Compare(v2) == 0;
        }

        public static bool operator !=(Plugin_Version v1, Plugin_Version v2)
        {
            return v1.Compare(v2) != 0;
        }

        public static bool operator <(Plugin_Version v1, Plugin_Version v2)
        {
            return v1.Compare(v2) < 0;
        }

        public static bool operator >(Plugin_Version v1, Plugin_Version v2)
        {
            return v1.Compare(v2) > 0;
        }

        public static bool operator <=(Plugin_Version v1, Plugin_Version v2)
        {
            return v1.Compare(v2) <= 0;
        }

        public static bool operator >=(Plugin_Version v1, Plugin_Version v2)
        {
            return v1.Compare(v2) >= 0;
        }

        protected long toInt()
        {
            return ((this.major << 32) + (this.minor << 16) + this.patch);
        }

        public int Compare(Plugin_Version other)
        {
            return (int)Math.Max(-1, Math.Min(1, this.toInt() - other.toInt()));
        }

        public override int GetHashCode()
        {
            return (int)toInt();
        }

        public override bool Equals(object obj)
        {
            Plugin_Version vers = obj as Plugin_Version;
            return Compare(vers)==0;
        }
    }
}
