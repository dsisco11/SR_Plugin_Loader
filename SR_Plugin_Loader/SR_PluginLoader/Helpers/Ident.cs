using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    public static class Ident
    {
        private static HashSet<Identifiable.Id> _all_idents = null;
        public static HashSet<Identifiable.Id> ALL_IDENTS
        {
            get
            {
                if (_all_idents == null)
                {
                    _all_idents = new HashSet<Identifiable.Id>();
                    foreach(Identifiable.Id id in Enum.GetValues(typeof(Identifiable.Id)))
                    {
                        switch(id)
                        {
                            case Identifiable.Id.NONE:
                                break;
                            default:
                                _all_idents.Add(id);
                                break;
                        }
                    }
                }
                return _all_idents;
            }
        }

        private static HashSet<Identifiable.Id> _all_slimes = null;
        public static HashSet<Identifiable.Id> ALL_SLIMES
        {
            get
            {
                if (_all_slimes == null) _all_slimes = Util.Combine_Ident_Lists(new HashSet<Identifiable.Id>[] { Identifiable.SLIME_CLASS, Identifiable.LARGO_CLASS, Identifiable.GORDO_CLASS });
                return _all_slimes;
            }
        }

        private static HashSet<Identifiable.Id> _all_animals = null;
        public static HashSet<Identifiable.Id> ALL_ANIMALS
        {
            get
            {
                if (_all_animals == null) _all_animals = Util.Combine_Ident_Lists(new HashSet<Identifiable.Id>[] { Identifiable.CHICK_CLASS, Identifiable.MEAT_CLASS });
                return _all_animals;
            }
        }

    }
}
