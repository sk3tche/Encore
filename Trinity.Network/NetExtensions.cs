using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;

namespace Trinity.Network
{
    public static class NetExtensions
    {
        #region IP addresses

        /// <summary>
        /// Check if a given IPAddress object is IPv4.
        /// </summary>
        /// <param name="addr">IPAddress object to check.</param>
        [Pure]
        public static bool IsIPv4(this IPAddress addr)
        {
            Contract.Requires(addr != null);

            return addr.AddressFamily == AddressFamily.InterNetwork;
        }

        /// <summary>
        /// Check if a given IPAddress object is IPv6.
        /// </summary>
        /// <param name="addr">IPAddress object to check.</param>
        [Pure]
        public static bool IsIPv6(this IPAddress addr)
        {
            Contract.Requires(addr != null);

            return addr.AddressFamily == AddressFamily.InterNetworkV6;
        }

        /// <summary>
        /// Gets the length, in bytes, of the IPAddress object.
        /// </summary>
        /// <param name="addr">The IPAddress object.</param>
        [Pure]
        public static int GetLength(this IPAddress addr)
        {
            Contract.Requires(addr != null);
            Contract.Ensures(Contract.Result<int>() > 0);
            Contract.Ensures(Contract.Result<int>() == addr.GetAddressBytes().Length);

            var length = addr.GetAddressBytes().Length;
            Contract.Assume(length > 0);
            return length;
        }

        #endregion

        #region End points

        /// <summary>
        /// Converts an EndPoint object into an IPEndPoint object.
        /// </summary>
        /// <param name="endPoint">The EndPoint object to convert.</param>
        [Pure]
        public static IPEndPoint ToIPEndPoint(this EndPoint endPoint)
        {
            return (IPEndPoint)endPoint;
        }

        /// <summary>
        /// Converts an EndPoint object into a DnsEndPoint object.
        /// </summary>
        /// <param name="endPoint">The EndPoint object to convert.</param>
        [Pure]
        public static DnsEndPoint ToDnsEndPoint(this EndPoint endPoint)
        {
            return (DnsEndPoint)endPoint;
        }

        #endregion
    }
}
