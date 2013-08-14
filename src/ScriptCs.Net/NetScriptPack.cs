namespace ScriptCs.Net
{
    using ScriptCs.Contracts;

    public class NetScriptPack : IScriptPack
    {
        public void Initialize(IScriptPackSession session)
        {
            session.AddReference("System.dll");

            session.ImportNamespace("System");
            session.ImportNamespace("ScriptCs.Net");
        }

        public IScriptPackContext GetContext()
        {
            return new Net();
        }

        public void Terminate()
        {
        }
    }
}