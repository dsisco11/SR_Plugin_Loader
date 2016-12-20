using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader_Installer
{
    public static class Variable_Alterations
    {
        public static VariableChange[] vars = new VariableChange[]
        {
            new VariableChange() { name="Ammo.numSlots", act= VarChange.ADD_GETTER_SETTER, new_name="slotCount" },
            new VariableChange() { name="GardenCatcher.plantableDict", act= VarChange.ADD_GETTER_SETTER, new_name="plantPrefabs" },
            new VariableChange() { name="LandPlot.attached", act= VarChange.ADD_GET_ACCESSOR, function_name="Get_Attached" },
            new VariableChange() { name="WeaponVacuum.joints", act= VarChange.ADD_GET_ACCESSOR, function_name="Get_Joints" },
            new VariableChange() { name="SpawnResource.landPlot", act= VarChange.ADD_GET_ACCESSOR, function_name="Get_LandPlot" },
            new VariableChange() { name="LandPlotUI.activator", act= VarChange.ADD_GET_ACCESSOR, function_name="Get_LandPlot" },
            new VariableChange() { name="SiloCatcher.nextEject", act= VarChange.PRIVATE_TO_PUBLIC },
            new VariableChange() { name="SiloCatcher.storage", act= VarChange.PRIVATE_TO_PUBLIC },
            new VariableChange() { name="PlayerState.maxAmmo", act= VarChange.PRIVATE_TO_PUBLIC },
            new VariableChange() { name="PlayerState.maxHealth", act= VarChange.PRIVATE_TO_PUBLIC },
            new VariableChange() { name="PlayerState.maxEnergy", act= VarChange.PRIVATE_TO_PUBLIC },
            //new VariableChange() { name="AutoSaveDirector.current", act= VarChange.PRIVATE_TO_PUBLIC },
        };

        public static VariableChange[] types = new VariableChange[]
        {
            //new VariableChange() { name="EconomyDirector.CurrValueEntry", act= VarChange.PRIVATE_TO_PUBLIC },
        };
    }

    public enum VarChange
    {
        NONE = 0,
        PRIVATE_TO_PUBLIC,
        ADD_GETTER_SETTER,// requires 'new_name' so it can create a new variable with this getter and setter combo.
        ADD_GETTER,// requires 'new_name'.
        ADD_SETTER,// requires 'new_name'.
        ADD_GET_ACCESSOR,// requires 'function_name'.
        ADD_SET_ACCESSOR,// requires 'function_name'.
    }

    public class VariableChange
    {
        /// <summary>
        /// Class.VarName
        /// </summary>
        public string name = null;
        /// <summary>
        /// Just the VarName no Class.
        /// </summary>
        public string new_name = null;
        /// <summary>
        /// Certain actions will create a new function, this is the name the function will be given.
        /// </summary>
        public string function_name = null;
        /// <summary>
        /// What action to perform on the variable.
        /// </summary>
        public VarChange act = VarChange.NONE;

        public VariableChange()
        {
        }
    }
}
