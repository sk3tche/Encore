using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Core.Mathematics;

namespace Trinity.Encore.Framework.Core.Cryptography
{
    // This class was written by Chew Keong TAN.
    [Serializable]
    public sealed class BigInteger : IEquatable<BigInteger>, IComparable<BigInteger>
    {
        /// <summary>
        /// Maximum length of the BigInteger in Int32 (bits).
        /// </summary>
        private const int MaxLength = 512;

        /// <summary>
        /// Holds bytes from the BigInteger.
        /// </summary>
        private readonly uint[] _data;

        public int DataLength { get; private set; }

        public int ByteLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                var numBits = BitCount;
                var numBytes = numBits >> 3;

                if ((numBits & 0x7) != 0)
                    numBytes++;

                Contract.Assume(numBytes > 0);
                return numBytes;
            }
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_data != null);
            Contract.Invariant(_data.Length == MaxLength);
            Contract.Invariant(ByteLength >= 0);
            Contract.Invariant(DataLength >= 0);
        }

        public BigInteger()
        {
            _data = new uint[MaxLength];
            DataLength = 1;
        }

        public BigInteger(long value)
            : this()
        {
            var tempVal = value;
            DataLength = 0;

            while (value != 0 && DataLength < MaxLength)
            {
                _data[DataLength] = (uint)(value & 0xffffffff);
                value >>= 32;
                DataLength++;
            }

            if (tempVal > 0 && (value != 0 || (_data[MaxLength - 1] & 0x80000000) != 0))
                throw new ArithmeticException("Positive overflow.");

            if (tempVal < 0 && (value != -1 || (_data[DataLength - 1] & 0x80000000) == 0))
                throw new ArithmeticException("Negative underflow.");

            if (DataLength == 0)
                DataLength = 1;
        }

        public BigInteger(ulong value)
        {
            DataLength = 0;

            while (value != 0 && DataLength < MaxLength)
            {
                _data[DataLength] = (uint)(value & 0xffffffff);
                value >>= 32;
                DataLength++;
            }

            if (value != 0 || (_data[MaxLength - 1] & 0x80000000) != 0)
                throw new ArithmeticException("Positive overflow.");

            if (DataLength == 0)
                DataLength = 1;
        }

        public BigInteger(BigInteger bi)
            : this()
        {
            Contract.Requires(bi != null);

            DataLength = bi.DataLength;

            for (var i = 0; i < DataLength; i++)
                _data[i] = bi._data[i];
        }

        public BigInteger(string value, int radix = 16)
        {
            Contract.Requires(!string.IsNullOrEmpty(value));
            Contract.Requires(value.Length > 0);

            var multiplier = new BigInteger(1);
            var result = new BigInteger();
            value = value.ToUpper().Trim();
            var limit = 0;

            Contract.Assume(value.Length > 0);
            if (value[0] == '-')
                limit = 1;

            for (var i = value.Length - 1; i >= limit; i--)
            {
                int posVal = value[i];

                if (posVal >= '0' && posVal <= '9')
                    posVal -= '0';
                else if (posVal >= 'A' && posVal <= 'Z')
                    posVal = (posVal - 'A') + 10;
                else
                    posVal = 9999999;

                if (posVal >= radix)
                    throw new ArgumentException("Invalid string.");

                if (value[0] == '-')
                    posVal = -posVal;

                result = result + (multiplier * posVal);

                if ((i - 1) >= limit)
                    multiplier = multiplier * radix;
            }

            if (value[0] == '-' && (result._data[MaxLength - 1] & 0x80000000) == 0)
                throw new ArithmeticException("Negative underflow.");

            if ((result._data[MaxLength - 1] & 0x80000000) != 0)
                throw new ArithmeticException("Positive overflow.");

            _data = new uint[MaxLength];

            for (var i = 0; i < result.DataLength; i++)
                _data[i] = result._data[i];

            DataLength = result.DataLength;
        }

        public BigInteger(byte[] inData)
        {
            Contract.Requires(inData != null);

            inData = (byte[])inData.Clone();
            inData.Reverse();

            var dataLength = inData.Length >> 2;

            var leftOver = inData.Length & 0x3;
            if (leftOver != 0)
                dataLength++;

            if (dataLength > MaxLength)
                throw new ArithmeticException("Byte overflow.");

            DataLength = dataLength;
            _data = new uint[MaxLength];

            for (int i = inData.Length - 1, j = 0; i >= 3; i -= 4, j++)
                _data[j] = (uint)((inData[i - 3] << 24) + (inData[i - 2] << 16) +
                    (inData[i - 1] << 8) + inData[i]);

            switch (leftOver)
            {
                case 1:
                    _data[DataLength - 1] = inData[0];
                    break;
                case 2:
                    _data[DataLength - 1] = (uint)((inData[0] << 8) + inData[1]);
                    break;
                case 3:
                    _data[DataLength - 1] = (uint)((inData[0] << 16) + (inData[1] << 8) + inData[2]);
                    break;
            }

            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }

        public BigInteger(uint[] inData)
        {
            Contract.Requires(inData != null);

            inData = (uint[])inData.Clone();
            DataLength = inData.Length;

            _data = new uint[MaxLength];

            for (int i = DataLength - 1, j = 0; i >= 0; i--, j++)
                _data[j] = inData[i];

            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }

        public BigInteger(FastRandom rand, int bitLength)
        {
            if (rand == null)
                rand = new FastRandom();

            _data = new uint[MaxLength];
            DataLength = 1;

            GenerateRandomBits(bitLength, rand);
        }

        public static explicit operator BigInteger(long value)
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return new BigInteger(value);
        }

        public static explicit operator BigInteger(ulong value)
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return new BigInteger(value);
        }

        public static explicit operator BigInteger(int value)
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return new BigInteger(value);
        }

        public static explicit operator BigInteger(uint value)
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return new BigInteger((ulong)value);
        }

        public static implicit operator BigInteger(byte[] value)
        {
            Contract.Requires(value != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return new BigInteger(value);
        }

        public static BigInteger operator +(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger();
            result.DataLength = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;
            long carry = 0;

            for (var i = 0; i < result.DataLength; i++)
            {
                var sum = (long)bi1._data[i] + bi2._data[i] + carry;
                carry = sum >> 32;
                result._data[i] = (uint)(sum & 0xffffffff);
            }

            if (carry != 0 && result.DataLength < MaxLength)
            {
                result._data[result.DataLength] = (uint)(carry);
                result.DataLength++;
            }

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            const int lastPos = MaxLength - 1;
            if ((bi1._data[lastPos] & 0x80000000) == (bi2._data[lastPos] & 0x80000000) &&
                (result._data[lastPos] & 0x80000000) != (bi1._data[lastPos] & 0x80000000))
                throw new ArithmeticException("Overflow in operator +.");

            return result;
        }

        public static BigInteger operator +(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 + (BigInteger)bi2;
        }

        public static BigInteger operator +(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 + (BigInteger)bi2;
        }

        public static BigInteger operator +(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 + (BigInteger)bi2;
        }

        public static BigInteger operator +(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 + (BigInteger)bi2;
        }

        public static BigInteger operator ++(BigInteger bi1)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger(bi1);
            long carry = 1;
            var index = 0;

            while (carry != 0 && index < MaxLength)
            {
                long val = result._data[index];
                val++;
                result._data[index] = (uint)(val & 0xffffffff);
                carry = val >> 32;
                index++;
            }

            if (index <= result.DataLength)
            {
                while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                    result.DataLength--;
            }
            else
                result.DataLength = index;

            const int lastPos = MaxLength - 1;

            if ((bi1._data[lastPos] & 0x80000000) == 0 && (result._data[lastPos] & 0x80000000) !=
                (bi1._data[lastPos] & 0x80000000))
                throw new ArithmeticException("Overflow in operator ++.");

            return result;
        }

        public static BigInteger operator -(BigInteger bi1)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            if (bi1.DataLength == 1 && bi1._data[0] == 0)
                return new BigInteger();

            var result = new BigInteger(bi1);

            for (var i = 0; i < MaxLength; i++)
                result._data[i] = ~(bi1._data[i]);

            long carry = 1;
            var index = 0;

            while (carry != 0 && index < MaxLength)
            {
                long val = result._data[index];
                val++;
                result._data[index] = (uint)(val & 0xffffffff);
                carry = val >> 32;
                index++;
            }

            if ((bi1._data[MaxLength - 1] & 0x80000000) == (result._data[MaxLength - 1] & 0x80000000))
                throw new ArithmeticException("Overflow in operator -.");

            result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }

        public static BigInteger operator -(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger();
            result.DataLength = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;
            long carryIn = 0;

            for (var i = 0; i < result.DataLength; i++)
            {
                var diff = bi1._data[i] - (long)bi2._data[i] - carryIn;
                result._data[i] = (uint)(diff & 0xffffffff);
                carryIn = diff < 0 ? 1 : 0;
            }

            if (carryIn != 0)
            {
                for (var i = result.DataLength; i < MaxLength; i++)
                    result._data[i] = 0xffffffff;

                result.DataLength = MaxLength;
            }

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            const int lastPos = MaxLength - 1;

            if ((bi1._data[lastPos] & 0x80000000) != (bi2._data[lastPos] & 0x80000000) &&
                (result._data[lastPos] & 0x80000000) != (bi1._data[lastPos] & 0x80000000))
                throw new ArithmeticException("Underflow in operator -.");

            return result;
        }

        public static BigInteger operator -(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 - (BigInteger)bi2;
        }

        public static BigInteger operator -(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 - (BigInteger)bi2;
        }

        public static BigInteger operator -(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 - (BigInteger)bi2;
        }

        public static BigInteger operator -(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 - (BigInteger)bi2;
        }

        public static BigInteger operator --(BigInteger bi1)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger(bi1);
            var carryIn = true;
            var index = 0;

            while (carryIn && index < MaxLength)
            {
                long val = result._data[index];
                val--;
                result._data[index] = (uint)(val & 0xffffffff);

                if (val >= 0)
                    carryIn = false;

                index++;
            }

            if (index > result.DataLength)
                result.DataLength = index;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            const int lastPos = MaxLength - 1;

            if ((bi1._data[lastPos] & 0x80000000) != 0 && (result._data[lastPos] & 0x80000000) !=
                (bi1._data[lastPos] & 0x80000000))
                throw new ArithmeticException("Underflow in operator --.");

            return result;
        }

        public static BigInteger operator *(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            const int lastPos = MaxLength - 1;
            var bi1Neg = false;
            var bi2Neg = false;

            try
            {
                if ((bi1._data[lastPos] & 0x80000000) != 0)
                {
                    bi1Neg = true;
                    bi1 = -bi1;
                }

                if ((bi2._data[lastPos] & 0x80000000) != 0)
                {
                    bi2Neg = true;
                    bi2 = -bi2;
                }
            }
            catch
            {
            }

            var result = new BigInteger();

            try
            {
                for (var i = 0; i < bi1.DataLength; i++)
                {
                    if (bi1._data[i] == 0)
                        continue;

                    ulong mcarry = 0;

                    for (int j = 0, k = i; j < bi2.DataLength; j++, k++)
                    {
                        var val = ((ulong)bi1._data[i] * bi2._data[j]) + result._data[k] + mcarry;

                        result._data[k] = (uint)(val & 0xffffffff);
                        mcarry = (val >> 32);
                    }

                    if (mcarry != 0)
                        result._data[i + bi2.DataLength] = (uint)mcarry;
                }
            }
            catch (Exception)
            {
                throw new ArithmeticException("Multiplication overflow.");
            }

            result.DataLength = bi1.DataLength + bi2.DataLength;

            if (result.DataLength > MaxLength)
                result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            if ((result._data[lastPos] & 0x80000000) != 0)
            {
                if (bi1Neg != bi2Neg && result._data[lastPos] == 0x80000000)
                {
                    if (result.DataLength == 1)
                        return result;

                    var isMaxNeg = true;

                    for (var i = 0; i < result.DataLength - 1 && isMaxNeg; i++)
                        if (result._data[i] != 0)
                            isMaxNeg = false;

                    if (isMaxNeg)
                        return result;
                }

                throw new ArithmeticException("Multiplication overflow.");
            }

            if (bi1Neg != bi2Neg)
                return -result;

            return result;
        }

        public static BigInteger operator *(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 * (BigInteger)bi2;
        }

        public static BigInteger operator *(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 * (BigInteger)bi2;
        }

        public static BigInteger operator *(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 * (BigInteger)bi2;
        }

        public static BigInteger operator *(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 * (BigInteger)bi2;
        }

        private static void MultiByteDivide(BigInteger bi1, BigInteger bi2, ref BigInteger outQuotient,
            ref BigInteger outRemainder)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Requires(outQuotient != null);
            Contract.Requires(outRemainder != null);
            Contract.Ensures(outQuotient != null);
            Contract.Ensures(outRemainder != null);

            var result = new uint[MaxLength];
            var remainderLen = bi1.DataLength + 1;
            var remainder = new uint[remainderLen];

            var mask = 0x80000000;
            var val = bi2._data[bi2.DataLength - 1];
            var shift = 0;
            var resultPos = 0;

            while (mask != 0 && (val & mask) == 0)
            {
                shift++;
                mask >>= 1;
            }

            for (var i = 0; i < bi1.DataLength; i++)
                remainder[i] = bi1._data[i];

            ShiftLeft(remainder, shift);
            bi2 = bi2 << shift;

            var j = remainderLen - bi2.DataLength;
            var pos = remainderLen - 1;

            ulong firstDivisorByte = bi2._data[bi2.DataLength - 1];
            ulong secondDivisorByte = bi2._data[bi2.DataLength - 2];

            var divisorLen = bi2.DataLength + 1;
            var dividendPart = new uint[divisorLen];

            while (j > 0)
            {
                var dividend = ((ulong)remainder[pos] << 32) + remainder[pos - 1];
                Contract.Assert(firstDivisorByte > 0);
                var q_hat = dividend / firstDivisorByte;
                var r_hat = dividend % firstDivisorByte;
                var done = false;

                while (!done)
                {
                    done = true;

                    if (q_hat != 0x100000000 && (q_hat * secondDivisorByte) <=
                        ((r_hat << 32) + remainder[pos - 2]))
                        continue;

                    q_hat--;
                    r_hat += firstDivisorByte;

                    if (r_hat < 0x100000000)
                        done = false;
                }

                for (var h = 0; h < divisorLen; h++)
                    dividendPart[h] = remainder[pos - h];

                var kk = new BigInteger(dividendPart);
                var ss = bi2 * (long)q_hat;

                while (ss > kk)
                {
                    q_hat--;
                    ss -= bi2;
                }

                var yy = kk - ss;

                for (var h = 0; h < divisorLen; h++)
                    remainder[pos - h] = yy._data[bi2.DataLength - h];

                result[resultPos++] = (uint)q_hat;

                pos--;
                j--;
            }

            outQuotient.DataLength = resultPos;
            var y = 0;

            for (var x = outQuotient.DataLength - 1; x >= 0; x--, y++)
                outQuotient._data[y] = result[x];

            for (; y < MaxLength; y++)
                outQuotient._data[y] = 0;

            while (outQuotient.DataLength > 1 && outQuotient._data[outQuotient.DataLength - 1] == 0)
                outQuotient.DataLength--;

            if (outQuotient.DataLength == 0)
                outQuotient.DataLength = 1;

            outRemainder.DataLength = ShiftRight(remainder, shift);

            for (y = 0; y < outRemainder.DataLength; y++)
                outRemainder._data[y] = remainder[y];

            for (; y < MaxLength; y++)
                outRemainder._data[y] = 0;
        }

        private static void SingleByteDivide(BigInteger bi1, BigInteger bi2, ref BigInteger outQuotient,
            ref BigInteger outRemainder)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Requires(outQuotient != null);
            Contract.Requires(outRemainder != null);
            Contract.Ensures(outQuotient != null);
            Contract.Ensures(outRemainder != null);

            var result = new uint[MaxLength];
            var resultPos = 0;

            for (var i = 0; i < MaxLength; i++)
                outRemainder._data[i] = bi1._data[i];

            outRemainder.DataLength = bi1.DataLength;

            while (outRemainder.DataLength > 1 && outRemainder._data[outRemainder.DataLength - 1] == 0)
                outRemainder.DataLength--;

            ulong divisor = bi2._data[0];
            var pos = outRemainder.DataLength - 1;
            ulong dividend = outRemainder._data[pos];

            if (dividend >= divisor)
            {
                Contract.Assume(divisor > 0);
                var quotient = dividend / divisor;
                result[resultPos++] = (uint)quotient;

                outRemainder._data[pos] = (uint)(dividend % divisor);
            }

            pos--;

            while (pos >= 0)
            {
                dividend = ((ulong)outRemainder._data[pos + 1] << 32) + outRemainder._data[pos];
                var quotient = dividend / divisor;
                result[resultPos++] = (uint)quotient;

                outRemainder._data[pos + 1] = 0;
                outRemainder._data[pos--] = (uint)(dividend % divisor);
            }

            outQuotient.DataLength = resultPos;
            var j = 0;

            for (var i = outQuotient.DataLength - 1; i >= 0; i--, j++)
                outQuotient._data[j] = result[i];

            for (; j < MaxLength; j++)
                outQuotient._data[j] = 0;

            while (outQuotient.DataLength > 1 && outQuotient._data[outQuotient.DataLength - 1] == 0)
                outQuotient.DataLength--;

            if (outQuotient.DataLength == 0)
                outQuotient.DataLength = 1;

            while (outRemainder.DataLength > 1 && outRemainder._data[outRemainder.DataLength - 1] == 0)
                outRemainder.DataLength--;
        }

        public static BigInteger operator /(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            if (bi1 == null)
                throw new ArgumentNullException("bi1");

            if (bi2 == null)
                throw new ArgumentNullException("bi2");

            var quotient = new BigInteger();
            var remainder = new BigInteger();

            const int lastPos = MaxLength - 1;
            bool divisorNeg = false, dividendNeg = false;

            if ((bi1._data[lastPos] & 0x80000000) != 0)
            {
                bi1 = -bi1;
                dividendNeg = true;
            }

            if ((bi2._data[lastPos] & 0x80000000) != 0)
            {
                bi2 = -bi2;
                divisorNeg = true;
            }

            if (bi1 < bi2)
                return quotient;

            if (bi2.DataLength == 1)
                SingleByteDivide(bi1, bi2, ref quotient, ref remainder);
            else
                MultiByteDivide(bi1, bi2, ref quotient, ref remainder);

            if (dividendNeg != divisorNeg)
                return -quotient;

            return quotient;
        }

        public static BigInteger operator /(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 / (BigInteger)bi2;
        }

        public static BigInteger operator /(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 / (BigInteger)bi2;
        }

        public static BigInteger operator /(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 / (BigInteger)bi2;
        }

        public static BigInteger operator /(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 / (BigInteger)bi2;
        }

        public static BigInteger operator %(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var quotient = new BigInteger();
            var remainder = new BigInteger(bi1);
            const int lastPos = MaxLength - 1;
            var dividendNeg = false;

            if ((bi1._data[lastPos] & 0x80000000) != 0)
            {
                bi1 = -bi1;
                dividendNeg = true;
            }

            if ((bi2._data[lastPos] & 0x80000000) != 0)
                bi2 = -bi2;

            if (bi1 < bi2)
                return remainder;

            if (bi2.DataLength == 1)
                SingleByteDivide(bi1, bi2, ref quotient, ref remainder);
            else
                MultiByteDivide(bi1, bi2, ref quotient, ref remainder);

            if (dividendNeg)
                return -remainder;

            return remainder;
        }

        public static BigInteger operator %(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 % (BigInteger)bi2;
        }

        public static BigInteger operator %(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 % (BigInteger)bi2;
        }

        public static BigInteger operator %(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 % (BigInteger)bi2;
        }

        public static BigInteger operator %(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return bi1 % (BigInteger)bi2;
        }

        public static BigInteger operator %(long bi1, BigInteger bi2)
        {
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return (BigInteger)bi1 % bi2;
        }

        public static BigInteger operator %(ulong bi1, BigInteger bi2)
        {
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return (BigInteger)bi1 % bi2;
        }

        public static BigInteger operator %(int bi1, BigInteger bi2)
        {
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return (BigInteger)bi1 % bi2;
        }

        public static BigInteger operator %(uint bi1, BigInteger bi2)
        {
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return (BigInteger)bi1 % bi2;
        }

        public static BigInteger operator <<(BigInteger bi1, int shiftVal)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger(bi1);
            Contract.Assume(result._data != null);
            result.DataLength = ShiftLeft(result._data, shiftVal);
            return result;
        }

        private static int ShiftLeft(IList<uint> buffer, int shiftVal)
        {
            Contract.Requires(buffer != null);
            Contract.Ensures(Contract.Result<int>() >= 0);

            var shiftAmount = 32;
            var bufLen = buffer.Count;

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            for (var count = shiftVal; count > 0; )
            {
                if (count < shiftAmount)
                    shiftAmount = count;

                ulong carry = 0;

                for (var i = 0; i < bufLen; i++)
                {
                    var val = ((ulong)buffer[i]) << shiftAmount;
                    val |= carry;
                    buffer[i] = (uint)(val & 0xffffffff);
                    carry = val >> 32;
                }

                if (carry != 0)
                {
                    if (bufLen + 1 <= buffer.Count)
                    {
                        buffer[bufLen] = (uint)carry;
                        bufLen++;
                    }
                }

                count -= shiftAmount;
            }

            return bufLen;
        }

        public static BigInteger operator >>(BigInteger bi1, int shiftVal)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger(bi1);
            Contract.Assume(result._data != null);
            result.DataLength = ShiftRight(result._data, shiftVal);

            if ((bi1._data[MaxLength - 1] & 0x80000000) != 0)
            {
                for (var i = MaxLength - 1; i >= result.DataLength; i--)
                    result._data[i] = 0xffffffff;

                var mask = 0x80000000;
                for (var i = 0; i < 32; i++)
                {
                    if ((result._data[result.DataLength - 1] & mask) != 0)
                        break;

                    result._data[result.DataLength - 1] |= mask;
                    mask >>= 1;
                }

                result.DataLength = MaxLength;
            }

            return result;
        }

        private static int ShiftRight(IList<uint> buffer, int shiftVal)
        {
            Contract.Requires(buffer != null);
            Contract.Ensures(Contract.Result<int>() >= 0);

            var shiftAmount = 32;
            var invShift = 0;
            var bufLen = buffer.Count;

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            for (var count = shiftVal; count > 0; )
            {
                if (count < shiftAmount)
                {
                    shiftAmount = count;
                    invShift = 32 - shiftAmount;
                }

                ulong carry = 0;

                for (var i = bufLen - 1; i >= 0; i--)
                {
                    var val = ((ulong)buffer[i]) >> shiftAmount;
                    val |= carry;
                    carry = ((ulong)buffer[i]) << invShift;
                    buffer[i] = (uint)(val);
                }

                count -= shiftAmount;
            }

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            return bufLen;
        }

        public static BigInteger operator ~(BigInteger bi1)
        {
            Contract.Requires(bi1 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger(bi1);
            for (var i = 0; i < MaxLength; i++)
                result._data[i] = ~(bi1._data[i]);

            result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }

        public static BigInteger operator &(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger();
            var len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;

            for (var i = 0; i < len; i++)
            {
                var sum = bi1._data[i] & bi2._data[i];
                result._data[i] = sum;
            }

            result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }

        public static BigInteger operator |(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger();
            var len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;

            for (var i = 0; i < len; i++)
            {
                var sum = bi1._data[i] | bi2._data[i];
                result._data[i] = sum;
            }

            result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }

        public static BigInteger operator ^(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var result = new BigInteger();
            var len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;

            for (var i = 0; i < len; i++)
            {
                var sum = bi1._data[i] ^ bi2._data[i];
                result._data[i] = sum;
            }

            result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }

        public static bool operator ==(BigInteger bi1, BigInteger bi2)
        {
            var obi1 = bi1 as object;
            var obi2 = bi2 as object;

            if (obi1 == null && obi2 == null)
                return true;

            if (obi1 == null || obi2 == null)
                return false;

            return bi1.Equals(bi2);
        }

        public static bool operator ==(BigInteger bi1, uint bi2)
        {
            return bi1 == (BigInteger)bi2;
        }

        public static bool operator ==(BigInteger bi1, int bi2)
        {
            return bi1 == (BigInteger)bi2;
        }

        public static bool operator ==(BigInteger bi1, long bi2)
        {
            return bi1 == (BigInteger)bi2;
        }

        public static bool operator ==(BigInteger bi1, ulong bi2)
        {
            return bi1 == (BigInteger)bi2;
        }

        public static bool operator !=(BigInteger bi1, BigInteger bi2)
        {
            return !(bi1 == bi2);
        }

        public static bool operator !=(BigInteger bi1, uint bi2)
        {
            return bi1 != (BigInteger)bi2;
        }

        public static bool operator !=(BigInteger bi1, int bi2)
        {
            return bi1 != (BigInteger)bi2;
        }

        public static bool operator !=(BigInteger bi1, long bi2)
        {
            return bi1 != (BigInteger)bi2;
        }

        public static bool operator !=(BigInteger bi1, ulong bi2)
        {
            return bi1 != (BigInteger)bi2;
        }

        public static bool operator >(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);

            var pos = MaxLength - 1;

            if ((bi1._data[pos] & 0x80000000) != 0 && (bi2._data[pos] & 0x80000000) == 0)
                return false;

            if ((bi1._data[pos] & 0x80000000) == 0 && (bi2._data[pos] & 0x80000000) != 0)
                return true;

            var len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;
            for (pos = len - 1; pos >= 0 && bi1._data[pos] == bi2._data[pos]; pos--)
            {
            }

            if (pos >= 0)
                return bi1._data[pos] > bi2._data[pos];

            return false;
        }

        public static bool operator >(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 > (BigInteger)bi2;
        }

        public static bool operator >(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 > (BigInteger)bi2;
        }

        public static bool operator >(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 > (BigInteger)bi2;
        }

        public static bool operator >(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 > (BigInteger)bi2;
        }

        public static bool operator <(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);

            var pos = MaxLength - 1;

            if ((bi1._data[pos] & 0x80000000) != 0 && (bi2._data[pos] & 0x80000000) == 0)
                return true;

            if ((bi1._data[pos] & 0x80000000) == 0 && (bi2._data[pos] & 0x80000000) != 0)
                return false;

            var len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;
            for (pos = len - 1; pos >= 0 && bi1._data[pos] == bi2._data[pos]; pos--)
            {
            }

            if (pos >= 0)
                return bi1._data[pos] < bi2._data[pos];

            return false;
        }

        public static bool operator <(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 < (BigInteger)bi2;
        }

        public static bool operator <(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 < (BigInteger)bi2;
        }

        public static bool operator <(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 < (BigInteger)bi2;
        }

        public static bool operator <(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 < (BigInteger)bi2;
        }

        public static bool operator >=(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);

            return (bi1 == bi2 || bi1 > bi2);
        }

        public static bool operator >=(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 >= (BigInteger)bi2;
        }

        public static bool operator >=(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 >= (BigInteger)bi2;
        }

        public static bool operator >=(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 >= (BigInteger)bi2;
        }

        public static bool operator >=(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 >= (BigInteger)bi2;
        }

        public static bool operator <=(BigInteger bi1, BigInteger bi2)
        {
            Contract.Requires(bi1 != null);
            Contract.Requires(bi2 != null);

            return (bi1 == bi2 || bi1 < bi2);
        }

        public static bool operator <=(BigInteger bi1, long bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 <= (BigInteger)bi2;
        }

        public static bool operator <=(BigInteger bi1, ulong bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 <= (BigInteger)bi2;
        }

        public static bool operator <=(BigInteger bi1, int bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 <= (BigInteger)bi2;
        }

        public static bool operator <=(BigInteger bi1, uint bi2)
        {
            Contract.Requires(bi1 != null);

            return bi1 <= (BigInteger)bi2;
        }

        public BigInteger Max(BigInteger bi)
        {
            Contract.Requires(bi != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return this > bi ? (new BigInteger(this)) : (new BigInteger(bi));
        }

        public BigInteger Min(BigInteger bi)
        {
            Contract.Requires(bi != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return this < bi ? new BigInteger(this) : new BigInteger(bi);
        }

        public BigInteger Abs()
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            return (_data[MaxLength - 1] & 0x80000000) != 0 ? new BigInteger(-this) :
                new BigInteger(this);
        }

        public BigInteger ModPow(BigInteger exp, BigInteger n)
        {
            Contract.Requires(exp != null);
            Contract.Requires(n != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            if ((exp._data[MaxLength - 1] & 0x80000000) != 0)
                throw new ArgumentException();

            var resultNum = (BigInteger)1;
            BigInteger tempNum;
            var thisNegative = false;

            if ((_data[MaxLength - 1] & 0x80000000) != 0)
            {
                tempNum = -this % n;
                thisNegative = true;
            }
            else
                tempNum = this % n;

            if ((n._data[MaxLength - 1] & 0x80000000) != 0)
                n = -n;

            var constant = new BigInteger();
            var i = n.DataLength << 1;
            constant._data[i] = 0x00000001;
            constant.DataLength = i + 1;
            constant = constant / n;
            var totalBits = exp.BitCount;
            var count = 0;

            for (var pos = 0; pos < exp.DataLength; pos++)
            {
                uint mask = 0x01;

                for (var index = 0; index < 32; index++)
                {
                    if ((exp._data[pos] & mask) != 0)
                    {
                        var resultNumTemp = resultNum * tempNum;
                        Contract.Assume(resultNumTemp.DataLength - n.DataLength - 1 >= 0);
                        resultNum = BarrettReduction(resultNumTemp, n, constant);
                    }

                    mask <<= 1;

                    var tempNum2 = tempNum * tempNum;
                    Contract.Assume(tempNum2.DataLength - n.DataLength - 1 >= 0);
                    tempNum = BarrettReduction(tempNum2, n, constant);

                    if (tempNum.DataLength == 1 && tempNum._data[0] == 1)
                    {
                        if (thisNegative && (exp._data[0] & 0x1) != 0)
                            return -resultNum;

                        return resultNum;
                    }

                    count++;

                    if (count == totalBits)
                        break;
                }
            }

            if (thisNegative && (exp._data[0] & 0x1) != 0)
                return -resultNum;

            return resultNum;
        }

        private static BigInteger BarrettReduction(BigInteger x, BigInteger n, BigInteger constant)
        {
            Contract.Requires(x != null);
            Contract.Requires(n != null);
            Contract.Requires(x.DataLength - n.DataLength - 1 >= 0);
            Contract.Requires(constant != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var k = n.DataLength;
            var kPlusOne = k + 1;
            var kMinusOne = k - 1;
            var q1 = new BigInteger();

            for (int i = kMinusOne, j = 0; i < x.DataLength; i++, j++)
                q1._data[j] = x._data[i];

            q1.DataLength = x.DataLength - kMinusOne;

            if (q1.DataLength <= 0)
                q1.DataLength = 1;

            var q2 = q1 * constant;
            var q3 = new BigInteger();

            for (int i = kPlusOne, j = 0; i < q2.DataLength; i++, j++)
                q3._data[j] = q2._data[i];

            Contract.Assume(q2.DataLength - kPlusOne >= 0);
            q3.DataLength = q2.DataLength - kPlusOne;

            if (q3.DataLength <= 0)
                q3.DataLength = 1;

            var r1 = new BigInteger();
            var lengthToCopy = (x.DataLength > kPlusOne) ? kPlusOne : x.DataLength;

            for (var i = 0; i < lengthToCopy; i++)
                r1._data[i] = x._data[i];

            r1.DataLength = lengthToCopy;
            var r2 = new BigInteger();

            for (var i = 0; i < q3.DataLength; i++)
            {
                if (q3._data[i] == 0)
                    continue;

                ulong mcarry = 0;
                var t = i;

                for (var j = 0; j < n.DataLength && t < kPlusOne; j++, t++)
                {
                    var val = (q3._data[i] * (ulong)n._data[j]) + r2._data[t] + mcarry;

                    r2._data[t] = (uint)(val & 0xffffffff);
                    mcarry = (val >> 32);
                }

                if (t < kPlusOne)
                    r2._data[t] = (uint)mcarry;
            }

            r2.DataLength = kPlusOne;

            while (r2.DataLength > 1 && r2._data[r2.DataLength - 1] == 0)
                r2.DataLength--;

            r1 -= r2;

            if ((r1._data[MaxLength - 1] & 0x80000000) != 0)
            {
                var val = new BigInteger();

                val._data[kPlusOne] = 0x00000001;
                val.DataLength = kPlusOne + 1;
                r1 += val;
            }

            while (r1 >= n)
                r1 -= n;

            return r1;
        }

        public BigInteger GreatestCommonDivisor(BigInteger bi)
        {
            Contract.Requires(bi != null);
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            BigInteger x;
            BigInteger y;

            if ((_data[MaxLength - 1] & 0x80000000) != 0)
                x = -this;
            else
                x = this;

            if ((bi._data[MaxLength - 1] & 0x80000000) != 0)
                y = -bi;
            else
                y = bi;

            var g = y;

            while (x.DataLength > 1 || (x.DataLength == 1 && x._data[0] != 0))
            {
                g = x;
                x = y % x;
                y = g;
            }

            return g;
        }

        public void GenerateRandomBits(int bits, FastRandom rand)
        {
            if (rand == null)
                rand = new FastRandom();

            var dwords = bits >> 5;
            var remBits = bits & 0x1f;

            if (remBits != 0)
                dwords++;

            if (dwords > MaxLength)
                throw new ArgumentException("Number of required bits higher than MaxLength.");

            for (var i = 0; i < dwords; i++)
                _data[i] = (uint)(rand.NextDouble() * 0x100000000);

            for (var i = dwords; i < MaxLength; i++)
                _data[i] = 0;

            if (remBits != 0)
            {
                var mask = (uint)(0x01 << (remBits - 1));
                _data[dwords - 1] |= mask;

                mask = 0xffffffff >> (32 - remBits);
                _data[dwords - 1] &= mask;
            }
            else
                _data[dwords - 1] |= 0x80000000;

            DataLength = dwords;

            if (DataLength == 0)
                DataLength = 1;
        }

        public int BitCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                while (DataLength > 1 && _data[DataLength - 1] == 0)
                    DataLength--;

                var value = _data[DataLength - 1];
                var mask = 0x80000000;
                var bits = 32;

                while (bits > 0 && (value & mask) == 0)
                {
                    bits--;
                    mask >>= 1;
                }

                bits += ((DataLength - 1) << 5);

                return bits;
            }
        }

        public byte LeastSignificantByte
        {
            get { return ByteValue; }
        }

        public byte ByteValue
        {
            get { return (byte)_data[0]; }
        }

        public int Int16Value
        {
            get { return (short)_data[0]; }
        }

        public int Int32Value
        {
            get { return (int)_data[0]; }
        }

        public long Int64Value
        {
            get
            {
                long val = _data[0];

                try
                {
                    val |= (long)_data[1] << 32;
                }
                catch (Exception)
                {
                    if ((_data[0] & 0x80000000) != 0)
                        val = (int)_data[0];
                }

                return val;
            }
        }

        public BigInteger ModInverse(BigInteger modulus)
        {
            Contract.Requires(modulus != null);

            var p = new[] { (BigInteger)0, (BigInteger)1 };
            var r = new[] { (BigInteger)0, (BigInteger)0 };
            var q = new BigInteger[2];

            var step = 0;
            var a = modulus;
            var b = this;

            while (b.DataLength > 1 || (b.DataLength == 1 && b._data[0] != 0))
            {
                var quotient = new BigInteger();
                var remainder = new BigInteger();

                if (step > 1)
                {
                    var q0 = q[0];
                    Contract.Assume(q0 != null);

                    var pval = (p[0] - (p[1] * q0)) % modulus;

                    p[0] = p[1];
                    p[1] = pval;
                }

                if (b.DataLength == 1)
                    SingleByteDivide(a, b, ref quotient, ref remainder);
                else
                    MultiByteDivide(a, b, ref quotient, ref remainder);

                q[0] = q[1];
                r[0] = r[1];
                q[1] = quotient;
                r[1] = remainder;

                a = b;
                b = remainder;

                step++;
            }

            if (r[0].DataLength > 1 || (r[0].DataLength == 1 && r[0]._data[0] != 1))
                return null;

            var q02 = q[0];
            Contract.Assume(q02 != null);
            var result = ((p[0] - (p[1] * q02)) % modulus);

            if ((result._data[MaxLength - 1] & 0x80000000) != 0)
                result += modulus;

            return result;
        }

        public byte[] GetBytes()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);

            return GetBytes(ByteLength);
        }

        public byte[] GetBytes(int numBytes)
        {
            Contract.Requires(numBytes <= ByteLength);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var result = new byte[numBytes];
            var numBits = BitCount;
            var realNumBytes = numBits >> 3;

            if ((numBits & 0x7) != 0)
                realNumBytes++;

            for (var i = 0; i < realNumBytes; i++)
            {
                for (var b = 0; b < 4; b++)
                {
                    if (i * 4 + b >= realNumBytes)
                        return result;

                    result[i * 4 + b] = (byte)(_data[i] >> (b * 8) & 0xff);
                }
            }

            return result;
        }

        public void SetBit(uint bitNum)
        {
            Contract.Requires(bitNum >= 0);

            var bytePos = bitNum >> 5;
            var bitPos = (byte)(bitNum & 0x1f);

            var mask = (uint)1 << bitPos;
            _data[bytePos] |= mask;

            if (bytePos >= DataLength)
                DataLength = (int)bytePos + 1;
        }

        public void UnsetBit(uint bitNum)
        {
            Contract.Requires(bitNum >= 0);

            var bytePos = bitNum >> 5;

            if (bytePos >= DataLength)
                return;

            var bitPos = (byte)(bitNum & 0x1f);
            var mask = (uint)1 << bitPos;
            var mask2 = 0xffffffff ^ mask;

            _data[bytePos] &= mask2;

            if (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }

        public BigInteger Sqrt()
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var numBits = (uint)BitCount;

            if ((numBits & 0x1) != 0)
                numBits = (numBits >> 1) + 1;
            else
                numBits = (numBits >> 1);

            var bytePos = numBits >> 5;
            var bitPos = (byte)(numBits & 0x1f);
            uint mask;
            var result = new BigInteger();

            if (bitPos != 0)
            {
                mask = (uint)1 << bitPos;
                bytePos++;
            }
            else
                mask = 0x80000000;

            result.DataLength = (int)bytePos;

            for (var i = (int)bytePos - 1; i >= 0; i--)
            {
                while (mask != 0)
                {
                    result._data[i] ^= mask;

                    if ((result * result) > this)
                        result._data[i] ^= mask;

                    mask >>= 1;
                }

                mask = 0x80000000;
            }

            return result;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object o)
        {
            return Equals(o as BigInteger);
        }

        public bool Equals(BigInteger other)
        {
            if (other == null)
                return false;

            // If the length doesn't equal, we can gain some speed.
            if (DataLength != other.DataLength)
                return false;

            for (var i = 0; i < DataLength; i++)
                if (_data[i] != other._data[i])
                    return false;

            return true;
        }

        public int CompareTo(BigInteger other)
        {
            if (other == null || this > other)
                return 1;

            if (this < other)
                return -1;

            return 0;
        }

        public override string ToString()
        {
            return "0x" + ToString(16);
        }

        public string ToString(int radix)
        {
            Contract.Requires(radix > 0);
            Contract.Ensures(Contract.Result<string>() != null);

            const string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = string.Empty;
            var a = this;
            var negative = false;

            if ((a._data[MaxLength - 1] & 0x80000000) != 0)
            {
                negative = true;

                try
                {
                    a = -a;
                }
                catch
                {
                }
            }

            var quotient = new BigInteger();
            var remainder = new BigInteger();
            var biRadix = new BigInteger(radix);

            if (a.DataLength == 1 && a._data[0] == 0)
                result = "0";
            else
            {
                while (a.DataLength > 1 || (a.DataLength == 1 && a._data[0] != 0))
                {
                    SingleByteDivide(a, biRadix, ref quotient, ref remainder);

                    if (remainder._data[0] >= 10)
                    {
                        var chr = (int)remainder._data[0] - 10;
                        Contract.Assume(chr < charSet.Length);
                        result = charSet[chr] + result;
                    }
                    else
                        result = remainder._data[0] + result;

                    a = quotient;
                }

                if (negative)
                    result = "-" + result;
            }

            return result;
        }
    }
}
