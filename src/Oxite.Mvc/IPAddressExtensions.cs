using System;
using System.Net;
using System.Web;

namespace Oxite.Mvc
{
    public static class IPAddressExtensions
    {
        public static IPAddress GetUserIPAddress(this HttpRequestBase request)
        {
            IPAddress address;

            if (!IPAddress.TryParse(request.UserHostAddress, out address))
                address = null;

            return address;
        }
    }
}
