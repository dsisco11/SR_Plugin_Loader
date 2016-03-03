namespace SR_PluginLoader
{
    /// <summary>
    /// FOR INTERNAL USE ONLY, HOOKS SHOULD RETURN AN INSTANCE OF THE "Siscos_Return" CLASS!
    /// </summary>
    public struct _hook_result
    {
        public bool cancel, has_custom_return;
        public object[] args;
        public object return_value;

        public _hook_result(bool _cancel, object[] _args)
        {
            this.cancel = _cancel;
            this.args = _args;
            this.return_value = null;
            this.has_custom_return = false;
        }
    }
}
