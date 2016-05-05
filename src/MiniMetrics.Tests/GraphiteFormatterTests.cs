﻿using System;
using MiniMetrics.Extensions;
using Xunit;

namespace MiniMetrics.Tests
{
    public class GraphiteFormatterTests : IDisposable
    {
        private readonly DefaultFormatter _sut;

        public GraphiteFormatterTests()
        {
            DateTimeExtensions.Now = () => DateTime.Today;
            _sut = new DefaultFormatter();
        }

        [Theory]
        [InlineData(100L)]
        [InlineData(100)]
        public void OnlyIntegerAndLongTypesAreSupported(Object value)
        {
            String expected = $"test {value} { DateTimeExtensions.ToUnixTimestamp() }{Environment.NewLine}";

            var message = _sut.Format("test", value);

            Assert.Equal(expected, message);
        }

        [Theory]
        [InlineData(10D)]
        [InlineData(10F)]
        [InlineData(short.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(ushort.MinValue)]
        [InlineData("test")]
        [InlineData(null)]
        public void NonNumberTypesThrowException(Object value)
        {
            Assert.Throws<NotSupportedException>(() => _sut.Format("test", value));
        }

        public void Dispose()
        {
        }
    }
}