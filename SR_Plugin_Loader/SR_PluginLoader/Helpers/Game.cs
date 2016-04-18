using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    /// <summary>
    /// Provides information about the current state of the game.
    /// </summary>
    public static class Game
    {
        public static bool atMainMenu { get { return Levels.isSpecial(); } }

    }
}
