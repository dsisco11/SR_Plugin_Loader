using SR_PluginLoader;
using System.Collections.Generic;
using UnityEngine;

namespace EventHooks
{
    public class ExamplePlugin : MonoBehaviour
    {

        private void Awake()
        {
            // Note: we can technically use absolutely any value as the 'registrar' when we register a hook. The registrar is just used to track WHO the hook belongs to. So we could actually create hook groups by passing each 'group' a different registrar item.
            // seperating hooks into groups would allow us to unregister an entire group of hooks with a single function call (If we needed to)!
            SiscosHooks.register(this.gameObject, HOOK_ID.Player_Damaged, this.onPlayer_Damaged);
            SiscosHooks.register(this.gameObject, HOOK_ID.Get_Available_Saves, this.onGet_Available_Saves);
            SiscosHooks.register(this.gameObject, HOOK_ID.Vac_Can_Capture, this.onVac_Can_Capture);
        }
        
        // Make the player invinceable
        private Sisco_Return onPlayer_Damaged(ref object sender, ref object[] args, ref object return_value)
        {
            PlayerState player = (PlayerState)sender;// The sender of this event is a PlayerState instance, it contains some useful things for manipulating the player.
            player.AddCurrency(1);// Get 1 sheckle for every time the player takes a hit, ITS LIKE MARIO IN REVERSE!

            args[0] = (object)0;// Here we are changing the ammount of damage the hooked function was told the player took when it got called.
            return_value = (object)false;// If the ingame function returns false then it means that the player is not dead, no matter what their health is.
            return null;
        }

        // Change the list of available save files
        private Sisco_Return onGet_Available_Saves(ref object sender, ref object[] args, ref object return_value)
        {
            // This is the entire list of save names that will appear in the load game menu screen.
            // Here we replace the list of save files, this means if any other plugins wanted to do something with them.
            // YOU JUST SCREWED THEM ALL OVER!
            // SO DONT DO THINGS LIKE THIS
            List<string> lst = new List<string>() { "foo", "bar" };
            return_value = (object)lst;
            // DO THIS INSTEAD
            List<string> saves = (List<string>)return_value;
            saves.Add("foo");
            saves.Add("bar");
            // Now assuming any other plugins using this event did the same you can all work together peacefully
            // Just make sure you give unique names to each of your entries to avoid name collisions
            // And you'll need to handle the onload event to determine when the player chose your save-file-name also of course
            // But this way of doing things would be a bit suspect overall, a better idea is to just make your own menu entry that allows players to see a custom load-thingy screen for your plugin.


            // Since this function isnt a POST hook in order for our value to actually be returned we need to tell the hooked function to return early.
            return new Sisco_Return() { early_return = true };
        }

        // Make it so slimes cant be sucked up if you're holding the ALT key.
        private Sisco_Return onVac_Can_Capture(ref object sender, ref object[] args, ref object return_value)
        {
            if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) return null;
            Vacuumable vac = (Vacuumable)sender;
            SlimeFaceAnimator anim = vac.GetComponent<SlimeFaceAnimator>();
            if (anim != null)
            {
                // Here we are telling the hook to make the root function which fired it, to return FALSE
                // in this case returning false from said function tells the game that this object cannot be vacuumed up!
                return_value = (object)false;
                return new Sisco_Return() { early_return = true };
            }

            return null;
        }

    }
}
