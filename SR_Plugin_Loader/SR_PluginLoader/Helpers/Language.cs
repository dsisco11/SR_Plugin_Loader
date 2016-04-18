using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    /// <summary>
    /// Provides translation methods from the game
    /// </summary>
    public static class Language
    {
        public static MessageBundle pediaBundle { get { return Directors.messageDirector.GetBundle("pedia"); } }
        public static MessageBundle uiBundle { get { return Directors.messageDirector.GetBundle("ui"); } }


        public static string Translate(Identifiable.Id id)
        {
            string name = Enum.GetName(typeof(Identifiable.Id), id).ToLower();
            return pediaBundle.Get("t." + name);
        }
    }
}
