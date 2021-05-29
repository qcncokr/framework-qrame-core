using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class SocketExtensions
    {
        public static bool IsConnencted(this Socket @this)
        {
            bool result = false;
            try
            {
                result = !(@this.Poll(1, SelectMode.SelectRead) && @this.Available == 0);
            }
            catch
            {
            }

            return result;
        }
    }
}
