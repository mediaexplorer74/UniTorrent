#if !DISABLE_DHT
//
// NodeId.cs
//
// Authors:
//   J?r?mie Laval <jeremie.laval@gmail.com>
//   Alan McGovern <alan.mcgovern@gmail.com>
//
// Copyright (C) 2008 J?r?mie Laval, Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using Universal.Torrent.Bencoding;
using Universal.Torrent.Common;

namespace Universal.Torrent.Dht.Nodes
{
    internal class NodeId : IEquatable<NodeId>, IComparable<NodeId>, IComparable
    {
        private static readonly Random Random = new Random();

        private BigInteger _value;

        internal NodeId(byte[] value)
            : this(new BigInteger(value))
        {
            Bytes = value;
        }

        internal NodeId(InfoHash infoHash)
            : this(infoHash.ToArray())
        {
        }

        private NodeId(BigInteger value)
        {
            _value = value;
        }

        internal NodeId(BEncodedString value)
            : this(new BigInteger(value.TextBytes))
        {
            Bytes = value.TextBytes;
        }

        internal byte[] Bytes { get; }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as NodeId);
        }

        public int CompareTo(NodeId other)
        {
            if ((object) other == null)
                return 1;

            var s = _value.Compare(other._value);
            if (s == BigInteger.Sign.Zero)
                return 0;
            if (s == BigInteger.Sign.Positive)
                return 1;
            return -1;
        }

        public bool Equals(NodeId other)
        {
            if ((object) other == null)
                return false;

            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NodeId);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        internal NodeId Xor(NodeId right)
        {
            return new NodeId(_value.Xor(right._value));
        }

        public static implicit operator NodeId(int value)
        {
            return new NodeId(new BigInteger((uint) value));
        }

        public static NodeId operator -(NodeId first)
        {
            CheckArguments(first);
            return new NodeId(first._value);
        }

        public static NodeId operator -(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return new NodeId(first._value - second._value);
        }

        public static bool operator >(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return first._value > second._value;
        }

        public static bool operator <(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return first._value < second._value;
        }

        public static bool operator <=(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return first < second || first == second;
        }

        public static bool operator >=(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return first > second || first == second;
        }

        public static NodeId operator +(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return new NodeId(first._value + second._value);
        }

        public static NodeId operator *(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return new NodeId(first._value*second._value);
        }

        public static NodeId operator /(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return new NodeId(first._value/second._value);
        }

        public static NodeId operator %(NodeId first, NodeId second)
        {
            CheckArguments(first, second);
            return new NodeId(first._value%second._value);
        }

        private static void CheckArguments(NodeId first)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
        }

        private static void CheckArguments(NodeId first, NodeId second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
        }

        public static bool operator ==(NodeId first, NodeId second)
        {
            if ((object) first == null)
                return (object) second == null;
            if ((object) second == null)
                return false;
            return first._value == second._value;
        }

        public static bool operator !=(NodeId first, NodeId second)
        {
            return first._value != second._value;
        }

        internal BEncodedString BencodedString()
        {
            return new BEncodedString(_value.GetBytes());
        }

        internal NodeId Pow(uint p)
        {
            _value = BigInteger.Pow(_value, p);
            return this;
        }

        public static NodeId Create()
        {
            var b = new byte[20];
            lock (Random)
                Random.NextBytes(b);
            return new NodeId(b);
        }
    }
}

#endif