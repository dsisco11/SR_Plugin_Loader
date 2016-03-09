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
        /// This is a table that tracks the number of hooks each event id has, this allows us the most efficient way to determine if an early abort on event firing is possible by limiting instructions at the had of the 'Call' function.
        /// </summary>
        private static int[] EventCounter = null;
        private static Dictionary<HOOK_ID, List<Sisco_Hook_Delegate>> events = new Dictionary<HOOK_ID, List<Sisco_Hook_Delegate>>();
        private static Dictionary<object, List<Sisco_Hook_Ref>> tracker = new Dictionary<object, List<Sisco_Hook_Ref>>();

        public static void init()
        {
            int max = 0;
            foreach(var hook in HOOKS.HooksList)
            {
                max = Math.Max(max, hook.id);
            }

            EventCounter = new int[max+1];
            for(int i=0; i<EventCounter.Length; i++)
            {
                EventCounter[i] = 0;
            }
        }
        
        public static _hook_result call(HOOK_ID hook, object sender, ref object returnValue, object[] args)
        {
            try
            {
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
                            if (ret.handled == true) break;//cancel all other events
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
                /*
                if (result.args != null)
                {
                    Log(hook, "Result.args.Length: {0}", result.args.Length);
                    Log(hook, "Result.args: {0}", Get_Arg_String(args));
                }
                */
                
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

        #region HOOK REGISTRATION LOGIC
        /// <summary>
        /// Register your own function to be called whenever a specified event triggers.
        /// </summary>
        /// <param name="registrar">Identifier used for grouping many hooks into a category for efficient removal later.</param>
        /// <param name="hook">The event to hook.</param>
        /// <param name="cb">The function to call.</param>
        /// <returns>(BOOL) Whether the event was successfully hooked.</returns>
        public static bool register(object registrar, HOOK_ID hook, Sisco_Hook_Delegate cb)
        {
            if (registrar == null)
            {
                Log("Registrar cannot be NULL!");
                return false;
            }

            if(hook == null)
            {
                Log("Attempted to register for NULL event!");
                return false;
            }
            
            try
            {
                // create the callback list for this hook type if it doesn't exist.
                List<Sisco_Hook_Delegate> cb_list;
                if (!SiscosHooks.events.TryGetValue(hook, out cb_list)) SiscosHooks.events[hook] = new List<Sisco_Hook_Delegate>();
                EventCounter[(int)hook] = SiscosHooks.events[hook].Count;

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
                EventCounter[(int)hook] = SiscosHooks.events[hook].Count;
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
        public static bool unregister_all(object registrar)
        {
            if (registrar == null)
            {
                Log("Registrar cannot be NULL!");
                return false;
            }

            try
            {
                // create this registrar's hooks list if it doesn't exist.
                List<Sisco_Hook_Ref> hooks_list;
                if (!SiscosHooks.tracker.TryGetValue(registrar, out hooks_list)) SiscosHooks.tracker[registrar] = new List<Sisco_Hook_Ref>();
                
                foreach(var o in hooks_list)
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
        public static bool unregister_all(object registrar, HOOK_ID hook)
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

                foreach (var o in hooks_list)
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

        public static string Get_Arg_String(object[] args)
        {
            if (args != null)
            {
                string argStr = "";
                foreach (object arg in args)
                {
                    if(arg == null) argStr = String.Format("{0}, null", argStr);
                    else argStr = String.Format("{0}, {1}", argStr, arg.ToString());
                }
                return argStr.TrimEnd(new char[] { ',', ' ' });
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

        #region LOGGING
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
            string str = DebugHud.Format_Log(ex, 1);
            DebugHud.Log("{0} {1}", LOG_TAG, str);
        }

        #endregion

        public static void Example(GameObject gameObj, ref int slimesInVac, ref List<LiquidSource> currLiquids)
        {
            object num = 0;
            _hook_result hook_result = SiscosHooks.call(HOOK_ID.VacPak_Consume, null, ref num, new object[]
            {
                (object) gameObj,
		        (object) slimesInVac,
                (object) currLiquids
            });

            gameObj = (GameObject)hook_result.args[0];
            slimesInVac = (int)hook_result.args[1];
            currLiquids = (List<LiquidSource>)hook_result.args[2];
        }
    }
}
