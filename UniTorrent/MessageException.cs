#if !DISABLE_DHT
using System;
using System.Diagnostics;
using Universal.Torrent.Example;

namespace Universal.Torrent.Dht
{    

    public class MessageException : Exception
    {
        public string MessageString;

        internal MessageException(ErrorCode errorCode, string message) : base(message)
        {
            Debug.WriteLine("[ex] Exception: " + message + ", ErrorCode: "+ errorCode);

            MessageString ="[ex] Exception: " + message + ", ErrorCode: " + errorCode;

            ErrorCode = errorCode;
        }

        internal ErrorCode ErrorCode { get; }
    }
}

#endif