using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;

namespace SR_PluginLoader
{
    /// <summary>
    /// Hook functions may return either NULL or an instance of 'Sisco_Return'.
    /// </summary>
    /// <param name="sender">the triggering functions 'this' instance.</param>
    /// <param name="args">reference to the triggering functions args list.</param>
    /// <param name="return_value">reference to the value currently set to be returned by the function that fired this event.</param>
    /// <returns></returns>
    public delegate Sisco_Return Sisco_Hook_Delegate(ref object sender, ref object[] args, ref object return_value);

    /// <summary>
    /// yeah I named it after myself, wanna fight about it? Tough guy?!?
    /// </summary>
    public static class SiscosHooks
    {
        /// <summary>
        /// This is a table that tracks the number of hooks each event id has, this allows us the most efficient way to determine if an early abort on event firing is possible by limiting instructions at the head of the 'Call' function.
        /// </summary>
        private static int[] EventCounter = null;

        /// <summary>
        /// (FOR DEBUG PURPOSES)
        /// This is a list of hooks which when fired, we want to print a log messge for so we can verify they are working.
        /// </summary>
        public static HashSet<HOOK_ID> HOOKS_TO_ANNOUNCE = new HashSet<HOOK_ID>();
        private static Dictionary<HOOK_ID, List<Sisco_Hook_Delegate>> events = new Dictionary<HOOK_ID, List<Sisco_Hook_Delegate>>();
        private static Dictionary<object, List<Sisco_Hook_Ref>> tracker = new Dictionary<object, List<Sisco_Hook_Ref>>();
        private static Dictionary<string, object> assembly_registrars = new Dictionary<string, object>();

        internal static void Setup()
        {
            int max = HOOK_ID.Count;
            // foreach(var hook in HOOKS.HooksList) { max = Math.Max(max, hook.id); }
            
            EventCounter = new int[max];
            for(int i=0; i<EventCounter.Length; i++)
            {
                EventCounter[i] = 0;
            }

            #region Setup Event Extension Proxys
            register(null, HOOK_ID.Ext_Game_Saved, HookProxys.Ext_Game_Saved);
            register(null, HOOK_ID.Ext_Pre_Game_Loaded, HookProxys.Ext_Pre_Game_Loaded);
            register(null, HOOK_ID.Ext_Post_Game_Loaded, HookProxys.Ext_Post_Game_Loaded);
            register(null, HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, HookProxys.Ext_Spawn_Plot_Upgrades_UI);
            #endregion

            #region Hook Prefab Instantiation Events
            Util.Inject_Into_Prefabs<Entity_Pref_Spawn_Hook>(Ident.ALL_IDENTS);
            Util.Inject_Into_Prefabs<Plot_Pref_Spawn_Hook>(Ident.ALL_PLOTS);
            Util.Inject_Into_Prefabs<Resource_Pref_Spawn_Hook>(Ident.ALL_GARDEN_PATCHES);
            #endregion
            
        }

        public static _hook_result call(HOOK_ID hook, object sender, ref object returnValue, object[] args)
        {
            try
            {
#if DEBUG
                if (HOOKS_TO_ANNOUNCE.Contains(hook)) DebugHud.Log("[SiscosHooks] {0}({1})", hook, Get_Arg_String(args));
#endif

                _hook_result result = new _hook_result(args);
                List<Sisco_Hook_Delegate> cb_list;
                bool r = SiscosHooks.events.TryGetValue((HOOK_ID)hook, out cb_list);
                if (r == false) return new _hook_result();//no abort

                foreach (Sisco_Hook_Delegate act in cb_list)
                {
                    try
                    {
                        if (act == null) continue;
                        Sisco_Return ret = act(ref sender, ref result.args, ref returnValue);                        
                        if (ret != null)
                        {
                            if (ret.early_return) result.abort = true;
                            if (ret.handled == true)
                            {
                                result.handled = true;
                                break;//cancel all other events
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Log(hook, ex.Message);
                        Log(ex.StackTrace);
                    }
                }

                if(result == null)
                {
                    Log(hook, "Result became NULL somehow!");
                    return new _hook_result();// we MUST return something other then NULL or the whole game can screw up!
                }

                if(args != null && args.Length != result.args.Length)
                {
                    Log(hook, "The size of Result.args does not match the number of arguments recieved from the function!");
                }
                
                return result;
            }
            catch(Exception ex)
            {
                Log(hook, ex.Message);
                Log(ex.StackTrace);
                return new _hook_result();
            }

            return new _hook_result();//no abort
        }


        #region Internal
        internal static object Get_Assembly_Registrar(Assembly asy)
        {
            string key = asy.FullName;
            object value;
            if (!assembly_registrars.TryGetValue(key, out value))
            {
                value = new object();
                assembly_registrars.Add(key, value);
            }

            if (!SiscosHooks.tracker.ContainsKey(value)) SiscosHooks.tracker.Add(value, new List<Sisco_Hook_Ref>());

            return value;
        }

        #endregion

        #region REGISTRATION LOGIC
        /// <summary>
        /// Register your own function to be called whenever a specified event triggers.
        /// </summary>
        /// <param name="registrar">Identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">The event to hook.</param>
        /// <param name="cb">The function to call.</param>
        /// <returns>(BOOL) Whether the event was successfully hooked.</returns>
        public static bool register(object registrar, HOOK_ID hook, Sisco_Hook_Delegate cb)
        {
            if (registrar == null) registrar = Get_Assembly_Registrar( Assembly.GetCallingAssembly() );
            if(hook == null)
            {
                Log("Attempted to register for NULL event!");
                return false;
            }
            
            try
            {
                // create the callback list for this hook type if it doesn't exist.
                if (!SiscosHooks.events.ContainsKey(hook)) SiscosHooks.events[hook] = new List<Sisco_Hook_Delegate>();
                SiscosHooks.events[hook].Add(cb);
                EventCounter[(int)hook] = SiscosHooks.events[hook].Count;

                if (registrar != null)
                {
                    // create this registrar's hooks list if it doesn't exist.
                    if (!SiscosHooks.tracker.ContainsKey(registrar)) return false;
                    //add this hook to their list.
                    SiscosHooks.tracker[registrar].Add(new Sisco_Hook_Ref(hook, cb));
                }
                return true;
            }
            catch (Exception ex)
            {
                Log(ex);
            }

            return false;
        }


        /// <summary>
        /// Unhook a previous hook you installed.
        /// </summary>
        /// <param name="registrar">Unique identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">Id of the event to unhook.</param>
        /// <param name="cb">The function to call.</param>
        /// <returns>(BOOL) Whether the event was successfully unhooked.</returns>
        public static bool unregister(object registrar, HOOK_ID hook, Sisco_Hook_Delegate cb)
        {
            if (registrar == null) registrar = Get_Assembly_Registrar(Assembly.GetCallingAssembly());

            try
            {
                bool hk_success = false;
                if (SiscosHooks.events.ContainsKey(hook))
                {
                    hk_success = SiscosHooks.events[hook].Remove(cb);
                    if (!hk_success)
                    {
                        Log("Failed to remove hook from hooks list. Sender({0})", registrar);
                        return false;
                    }
                    else EventCounter[(int)hook] = SiscosHooks.events[hook].Count;
                }

                bool tr_success = false;
                if (SiscosHooks.tracker.ContainsKey(registrar))
                {
                    //add this hook to their list.
                    tr_success = SiscosHooks.tracker[registrar].Remove(new Sisco_Hook_Ref(hook, cb));
                    if (!tr_success)
                    {
                        Log("Failed to remove hook from tracker. Sender({0})", registrar);
                    }
                }

                return (tr_success && hk_success);
            }
            catch (Exception ex)
            {
                Log(ex);
            }

            return false;
        }


        /// <summary>
        /// Unhook ALL of the previous hooks you installed with a specified registrar object.
        /// </summary>
        /// <param name="registrar">Unique identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">Id of the event to unhook. Leave this blank to remove ALL hooked events.</param>
        /// <returns>(BOOL) Whether the events was successfully unhooked.</returns>
        public static bool unregister_all(object registrar)
        {
            if (registrar == null) registrar = Get_Assembly_Registrar(Assembly.GetCallingAssembly());

            try
            {
                // create this registrar's hooks list if it doesn't exist.
                List<Sisco_Hook_Ref> hooks_list;
                if (!SiscosHooks.tracker.ContainsKey(registrar)) return false;
                SiscosHooks.tracker.TryGetValue(registrar, out hooks_list);

                List<Sisco_Hook_Ref> trash = new List<Sisco_Hook_Ref>(hooks_list);
                foreach (var o in trash)
                {
                    bool b = unregister(registrar, o.evt, o.callback);
                    if(!b)
                    {
                        Log("Failed to unregister hook<{0}>", o.evt);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }

            return false;
        }


        /// <summary>
        /// Unhook ALL of the previous hooks you installed with a specified registrar object.
        /// </summary>
        /// <param name="registrar">Unique identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">Id of the event to unhook. Leave this blank to remove ALL hooked events.</param>
        /// <returns>(BOOL) Whether the events was successfully unhooked.</returns>
        public static bool unregister_all(object registrar, HOOK_ID hook)
        {
            if (registrar == null) registrar = Get_Assembly_Registrar(Assembly.GetCallingAssembly());

            try
            {
                // create the callback list for this hook type if it doesn't exist.
                if (!SiscosHooks.events.ContainsKey(hook)) SiscosHooks.events[hook] = new List<Sisco_Hook_Delegate>();

                // create this registrar's hooks list if it doesn't exist.
                List<Sisco_Hook_Ref> hooks_list;
                if (!SiscosHooks.tracker.ContainsKey(registrar)) return false;
                SiscosHooks.tracker.TryGetValue(registrar, out hooks_list);

                List<Sisco_Hook_Ref> trash = new List<Sisco_Hook_Ref>(hooks_list);
                foreach (var o in trash)
                {
                    if (hook != HOOK_ID.NONE && o.evt != hook) continue;
                    bool b = unregister(registrar, o.evt, o.callback);
                    if (!b)
                    {
                        Log("Failed to unregister hook<{0}>", o.evt);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return false;
        }

        #endregion

        #region HELPERS
        public static string Get_Arg_String(object[] args)
        {
            if (args != null)
            {
                List<string> arglist = new List<string>();
                
                foreach (object arg in args)
                {
                    if(arg == null) arglist.Add( "null" );
                    else arglist.Add( arg.ToString() );
                }

                return String.Join(", ", arglist.ToArray());
            }

            return "NULL";
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            if (toSerialize == null) return "null";
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        #endregion

        #region LOGGING
        //These are just a bunch of functions to wrap around the logging system calls so the Event Hooks system can print customized log messages

        private const string LOG_TAG = "<b>SiscosHooks</b>  ";
        private static void Log(HOOK_ID hook, string format, params object[] args)
        {
            DebugHud.Log(String.Format("{0}<{1}> {2}", LOG_TAG, hook.ToString(), format), args);
        }

        private static void Log(string format, params object[] args)
        {
            DebugHud.Log(String.Format("{0} {1}", LOG_TAG, format), args);
        }

        private static void Log(Exception ex)
        {
            string str = DebugHud.Format_Exception_Log(ex, 1);
            DebugHud.Log("{0}(Exception) {1}", LOG_TAG, str);
        }

#endregion

    }

    /// <summary>
    /// Here is where we keep any event hook extension proxys (Too keep things tidy!)
    /// An event hook extension proxy is a proxy function that extends or builds upon the information provided by a default hook coming from the generic hook system.
    /// This allows us to provide more intelligent and useful hooks to plugin makers!
    /// How does it work? Well to be honest I don't know, but I suspect magic...
    /// </summary>
    internal static class HookProxys
    {

        internal static Sisco_Return Ext_Spawn_Plot_Upgrades_UI(ref object sender, ref object[] args, ref object return_value)
        {
            LandPlot.Id kind = LandPlot.Id.NONE;
            switch (sender.GetType().Name)
            {
                case nameof(GardenUI):
                    kind = LandPlot.Id.GARDEN;
                    break;
                case nameof(CoopUI):
                    kind = LandPlot.Id.COOP;
                    break;
                case nameof(CorralUI):
                    kind = LandPlot.Id.CORRAL;
                    break;
                case nameof(PondUI):
                    kind = LandPlot.Id.POND;
                    break;
                case nameof(SiloUI):
                    kind = LandPlot.Id.SILO;
                    break;
                case nameof(IncineratorUI):
                    kind = LandPlot.Id.INCINERATOR;
                    break;
            }
            return new Sisco_Return(SiscosHooks.call(HOOK_ID.Spawn_Plot_Upgrades_UI, sender, ref return_value, new object[] { kind }));
        }

        internal static Sisco_Return Ext_Pre_Game_Loaded(ref object sender, ref object[] args, ref object return_value)
        {
            string saveFile = GameData.ToPath(args[0] as string);
            return new Sisco_Return(SiscosHooks.call(HOOK_ID.Pre_Game_Loaded, sender, ref return_value, new object[] { saveFile }));
        }

        internal static Sisco_Return Ext_Post_Game_Loaded(ref object sender, ref object[] args, ref object return_value)
        {
            string saveFile = GameData.ToPath(args[0] as string);
            return new Sisco_Return(SiscosHooks.call(HOOK_ID.Post_Game_Loaded, sender, ref return_value, new object[] { saveFile }));
        }

        internal static Sisco_Return Ext_Game_Saved(ref object sender, ref object[] args, ref object return_value)
        {
            string saveFile = GameData.ToPath((sender as GameData).gameName);
            return new Sisco_Return(SiscosHooks.call(HOOK_ID.Game_Saved, sender, ref return_value, new object[] { saveFile }));
        }

        
    }
}
