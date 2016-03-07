﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public class Plugin_Version
    {
        public int major;
        public int minor;

        public Plugin_Version(int maj, int min=0)
        {
            this.major = maj;
            this.minor = min;
        }

        public override string ToString()
        {
            return String.Format("v{0}.{1}", this.major, this.minor);
        }

        protected long toInt()
        {
            return ((this.major << 8) + this.minor);
        }

        public int Compare(Plugin_Version other)
        {
            return (int)Math.Max(-1, Math.Min(1, this.toInt() - other.toInt()));
        }
    }
}
