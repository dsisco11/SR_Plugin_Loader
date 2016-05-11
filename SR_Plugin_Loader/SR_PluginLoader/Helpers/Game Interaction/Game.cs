﻿using System;
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
        /// <summary>
        /// Returns the name of the currently loaded save file or NULL if no file is loaded.
        /// </summary>
        public static string SaveFileName { get { if (atMainMenu) { return null; }  if (Directors.autosaveDirector.current==null) { return null; } return Directors.autosaveDirector.current.gameName; } }
    }
}
