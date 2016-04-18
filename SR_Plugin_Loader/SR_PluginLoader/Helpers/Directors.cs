using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public static class Directors
    {
        public static LookupDirector lookupDirector { get { return SRSingleton<GameContext>.Instance.LookupDirector; } }
        public static MessageDirector messageDirector { get { return SRSingleton<GameContext>.Instance.MessageDirector; } }
    }
}
