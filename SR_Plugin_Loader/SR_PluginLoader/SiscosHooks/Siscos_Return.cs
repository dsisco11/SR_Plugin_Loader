namespace SR_PluginLoader
{
    public class Sisco_Return
    {
        /// <summary>
        ///  should we abort the event? (if TRUE then the calling function, eg: weapon.update(), will return early and not perform the rest of it's logic. In addition no other event handlers registered for it will be called this time.)
        /// </summary>
        public bool abort = false;
        /// <summary>
        /// should we stop calling other event handlers in the list?
        /// </summary>
        public bool handled = false;
        /// <summary>
        /// a value to return from the calling function (make sure it's the correct Type or you might crash the game)
        /// </summary>
        public object return_value = null;
        /// <summary>
        /// Set this to TRUE if you want to make the calling function to return a specific value
        /// </summary>
        public bool has_custom_return = false;



        public Sisco_Return(bool custom_return = false, object return_value = null, bool _abort = false, bool _handled = false)
        {
            this.abort = _abort;
            this.handled = _handled;
        }
    }

}
