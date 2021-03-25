﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Diagnostics.CodeAnalysis;
using OnTopic.Attributes;
using Xunit;

namespace OnTopic.Tests {

  /*============================================================================================================================
  | CLASS: ATTRIBUTE VALUE CONVERTER TEST
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides unit tests for the <see cref="AttributeValueConverter"/> class.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class AttributeValueConverterTest {

    /*==========================================================================================================================
    | TEST: IS CONVERTIBLE? TYPE: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Passes a <paramref name="type"/> into the <see cref="AttributeValueConverter.IsConvertible(Type)"/> method and
    ///   confirms that result matches the <paramref name="result"/>.
    ///   <c>true</c>
    /// </summary>
    [Theory]
    [InlineData(                typeof(string),                 true)]
    [InlineData(                typeof(int),                    true)]
    [InlineData(                typeof(bool),                   true)]
    [InlineData(                typeof(DateTime),               true)]
    [InlineData(                typeof(Uri),                    true)]
    [InlineData(                typeof(int?),                   true)]
    [InlineData(                typeof(bool?),                  true)]
    [InlineData(                typeof(DateTime?),              true)]
    [InlineData(                typeof(object),                 false)]
    public void IsConvertible_Type_ReturnsExpected(Type type, bool result) =>
      Assert.True(AttributeValueConverter.IsConvertible(type) == result);

    /*==========================================================================================================================
    | TEST: CONVERT: FROM STRING VALUE: SUCCEEDS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Passes a valid <paramref name="input"/> into <see cref="AttributeValueConverter.Convert(String?, Type)"/> based on a
    ///   <paramref name="type"/> and confirms that the result matches the <paramref name="expected"/>.
    /// </summary>
    [Theory]
    [InlineData(                typeof(string),                 "String",                       "String")]
    [InlineData(                typeof(int),                    "1",                            1)]
    [InlineData(                typeof(bool),                   "1",                            true)]
    [InlineData(                typeof(bool),                   "0",                            false)]
    [InlineData(                typeof(bool),                   "true",                         true)]
    [InlineData(                typeof(bool),                   "false",                        false)]
    [InlineData(                typeof(string),                 "",                             "")]
    [InlineData(                typeof(string),                 null,                           null)]
    [InlineData(                typeof(int?),                   "",                             null)]
    [InlineData(                typeof(bool?),                  "",                             null)]
    [InlineData(                typeof(DateTime?),              "",                             null)]
    [InlineData(                typeof(object),                 "Value",                        null)]
    public void Convert_FromStringValue_Succeeds(Type type, string? input, object? expected) =>
      Assert.Equal(expected, AttributeValueConverter.Convert(input, type));

    /*==========================================================================================================================
    | TEST: CONVERT: TO DATE/TIME: SUCCEEDS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Passes a valid valid <paramref name="input"/> string into <see cref="AttributeValueConverter.Convert(String?, Type)"/>
    ///   and confirms that the result matches the expected <see cref="DateTime"/> value.
    /// </summary>
    [Theory]
    [InlineData(                "1976-10-15 01:02:03")]
    [InlineData(                "October 15, 1976 01:02:03 AM")]
    [InlineData(                "15 Oct 1976 01:02:03")]
    public void Convert_ToDateTime_Succeeds(string? input) =>
      Assert.Equal(new DateTime(1976, 10, 15, 1, 2, 3), AttributeValueConverter.Convert(input, typeof(DateTime)));

    /*==========================================================================================================================
    | TEST: CONVERT: TO URI: SUCCEEDS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Passes a valid input string into <see cref="AttributeValueConverter.Convert(String?, Type)"/> and confirms that the
    ///   result matches the expected <see cref="Uri"/> value.
    /// </summary>
    [Fact]
    public void Convert_ToUri_Succeeds() {
      Assert.Equal("/OnTopicCMS/", ((Uri?)AttributeValueConverter.Convert("https://www.github.com/OnTopicCMS/", typeof(Uri)))?.LocalPath);
      Assert.False(((Uri?)AttributeValueConverter.Convert("/OnTopicCMS/", typeof(Uri)))?.IsAbsoluteUri);
    }

    /*==========================================================================================================================
    | TEST: CONVERT: FROM STRING VALUE: FAILS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Passes an invalid value into <see cref="AttributeValueConverter.Convert(String?, Type)"/> based on a <paramref name="
    ///   type"/> and confirms that the result is <c>null</c>.
    /// </summary>
    [Theory]
    [InlineData(                typeof(int))]
    [InlineData(                typeof(bool))]
    [InlineData(                typeof(DateTime))]
    [InlineData(                typeof(int?))]
    [InlineData(                typeof(bool?))]
    [InlineData(                typeof(DateTime?))]
    [InlineData(                typeof(object))]
    public void Convert_FromString_Fails(Type type) =>
      Assert.Null(AttributeValueConverter.Convert("ABC", type));

  } //Class
} //Namespace