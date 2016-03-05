using System;
using System.Collections.Generic;

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
        private static Dictionary<HOOK_ID, List<Sisco_Hook_Delegate>> events = new Dictionary<HOOK_ID, List<Sisco_Hook_Delegate>>();
        private static Dictionary<object, List<Sisco_Hook_Ref>> tracker = new Dictionary<object, List<Sisco_Hook_Ref>>();

        
        public static _hook_result call(HOOK_ID hook, object sender, ref object returnValue, params object[] args)
        {
            _hook_result result = new _hook_result(false, args);
            try
            {
                List<Sisco_Hook_Delegate> cb_list;
                bool r = SiscosHooks.events.TryGetValue(hook, out cb_list);
                if (r == false) return result;//no abort

                
                foreach (Sisco_Hook_Delegate act in cb_list)
                {
                    try
                    {
                        Sisco_Return ret = act(ref sender, ref result.args, ref returnValue);
                        if (ret != null)
                        {
                            if (ret.handled == true) break;//cancel all other events
                        }
                    }
                    catch(Exception ex)
                    {
                        Log(ex.Message);
                        return new _hook_result();
                    }
                }
            }
            catch(Exception ex)
            {
                Log(ex.Message);
                return new _hook_result();
            }

            return result;//no abort
        }

        /// <summary>
        /// Register your own function to be called whenever a specified event triggers.
        /// </summary>
        /// <param name="registrar">Unique identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">Id of the event to hook.</param>
        /// <param name="cb">The function to call.</param>
        /// <returns>(BOOL) Whether the event was successfully hooked.</returns>
        public static bool register(object registrar, HOOK_ID hook, Sisco_Hook_Delegate cb)
        {
            if (registrar == null)
            {
                Log("Registrar cannot be NULL!");
                return false;
            }

            try
            {
                // create the callback list for this hook type if it doesn't exist.
                List<Sisco_Hook_Delegate> cb_list;
                if (!SiscosHooks.events.TryGetValue(hook, out cb_list)) SiscosHooks.events[hook] = new List<Sisco_Hook_Delegate>();

                // create this registrar's hooks list if it doesn't exist.
                List<Sisco_Hook_Ref> hooks_list;
                if (!SiscosHooks.tracker.TryGetValue(registrar, out hooks_list)) SiscosHooks.tracker[registrar] = new List<Sisco_Hook_Ref>();

                //add this hook to their list.
                SiscosHooks.tracker[registrar].Add(new Sisco_Hook_Ref(hook, cb));
                SiscosHooks.events[hook].Add(cb);
                return true;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
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
            if (registrar == null)
            {
                Log("Registrar cannot be NULL!");
                return false;
            }

            try
            {
                // create the callback list for this hook type if it doesn't exist.
                List<Sisco_Hook_Delegate> cb_list;
                if (!SiscosHooks.events.TryGetValue(hook, out cb_list)) SiscosHooks.events[hook] = new List<Sisco_Hook_Delegate>();

                // create this registrar's hooks list if it doesn't exist.
                List<Sisco_Hook_Ref> hooks_list;
                if (!SiscosHooks.tracker.TryGetValue(registrar, out hooks_list)) SiscosHooks.tracker[registrar] = new List<Sisco_Hook_Ref>();

                //add this hook to their list.
                bool tr_success = SiscosHooks.tracker[registrar].Remove(new Sisco_Hook_Ref(hook, cb));
                if (!tr_success)
                {
                    Log("Failed to remove hook from tracker. Sender({0})", registrar);
                    return false;
                }

                bool hk_success = SiscosHooks.events[hook].Remove(cb);
                if (!hk_success)
                {
                    Log("Failed to remove hook from hooks list. Sender({0})", registrar);
                    return false;
                }

                return (tr_success && hk_success);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return false;
        }


        /// <summary>
        /// Unhook ALL of the previous hooks you installed with a specified registrar object.
        /// </summary>
        /// <param name="registrar">Unique identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">Id of the event to unhook. Leave this blank to remove ALL hooked events.</param>
        /// <returns>(BOOL) Whether the events was successfully unhooked.</returns>
        public static bool unregister_all(object registrar, HOOK_ID hook=HOOK_ID.NONE)
        {
            if (registrar == null)
            {
                Log("Registrar cannot be NULL!");
                return false;
            }

            try
            {
                // create the callback list for this hook type if it doesn't exist.
                List<Sisco_Hook_Delegate> cb_list;
                if (!SiscosHooks.events.TryGetValue(hook, out cb_list)) SiscosHooks.events[hook] = new List<Sisco_Hook_Delegate>();

                // create this registrar's hooks list if it doesn't exist.
                List<Sisco_Hook_Ref> hooks_list;
                if (!SiscosHooks.tracker.TryGetValue(registrar, out hooks_list)) SiscosHooks.tracker[registrar] = new List<Sisco_Hook_Ref>();
                
                foreach(var o in hooks_list)
                {
                    if (hook != HOOK_ID.NONE && o.evt != hook) continue;
                    bool b = unregister(registrar, o.evt, o.callback);
                    if(!b)
                    {
                        Log("");
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return false;
        }

        private static void Log(string format, params object[] args)
        {
            string tag = "[SiscosHooks]";
            DebugHud.Log(String.Format("{0} {1}", tag, format), args);
        }

        private static void Log(Exception ex)
        {
            string tag = "[SiscosHooks]";
            string str = DebugHud.Format_Log(ex, 1);
            DebugHud.Log("{0} {1}", tag, str);
        }
    }
}
