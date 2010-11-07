using System.Diagnostics.Contracts;
using System.Text;

namespace Trinity.Encore.Framework.Game
{
    public static class Defines
    {
        public static class Protocol
        {
            public static Encoding Encoding
            {
                get
                {
                    Contract.Ensures(Contract.Result<Encoding>() != null);

                    return Encoding.UTF8;
                }
            }
        }
    }
}
