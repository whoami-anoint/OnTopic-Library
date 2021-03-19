﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnTopic.Attributes;
using OnTopic.Internal.Reflection;
using OnTopic.Metadata;
using OnTopic.Tests.BindingModels;
using OnTopic.Tests.ViewModels;
using OnTopic.ViewModels;

namespace OnTopic.Tests {

  /*============================================================================================================================
  | CLASS: TYPE COLLECTION TESTS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides unit tests for the <see cref="MemberDispatcher"/> and <see cref="MemberInfoCollection"/> classes.
  /// </summary>
  /// <remarks>
  ///   These are internal collections and not accessible publicly.
  /// </remarks>
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class MemberDispatcherTest {

    /*==========================================================================================================================
    | TEST: CONSTRUCTOR: VALID TYPE: IDENTIFIES PROPERTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberInfoCollection"/> based on a type, and confirms that the property collection returns
    ///   properties of the type.
    /// </summary>
    [TestMethod]
    public void Constructor_ValidType_IdentifiesProperty() {

      var properties = new MemberInfoCollection<PropertyInfo>(typeof(ContentTypeDescriptor));

      Assert.IsTrue(properties.Contains("AttributeDescriptors")); //First class collection property
      Assert.IsFalse(properties.Contains("InvalidPropertyName")); //Invalid property

    }

    /*==========================================================================================================================
    | TEST: CONSTRUCTOR: VALID TYPE: IDENTIFIES DERIVED PROPERTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberInfoCollection"/> based on a type, and confirms that the property collection returns
    ///   properties derived from the base class.
    /// </summary>
    [TestMethod]
    public void Constructor_ValidType_IdentifiesDerivedProperty() {

      var properties = new MemberInfoCollection<PropertyInfo>(typeof(ContentTypeDescriptor));

      Assert.IsTrue(properties.Contains("Key")); //Inherited string property

    }

    /*==========================================================================================================================
    | TEST: CONSTRUCTOR: VALID TYPE: IDENTIFIES METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberInfoCollection"/> based on a type, and confirms that the property collection returns
    ///   methods of the type.
    /// </summary>
    [TestMethod]
    public void Constructor_ValidType_IdentifiesMethod() {

      var properties = new MemberInfoCollection<PropertyInfo>(typeof(ContentTypeDescriptor));

      Assert.IsFalse(properties.Contains("IsTypeOf")); //This is a method, not a property

    }

    /*==========================================================================================================================
    | TEST: ADD: DUPLICATE KEY: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="TypeMemberInfoCollection"/> and confirms that <see cref="TypeMemberInfoCollection.InsertItem(
    ///   Int32, MemberInfoCollection)"/> throws an exception if a <see cref="MemberInfoCollection"/> with a duplicate <see cref
    ///   ="MemberInfoCollection{T}.Type"/> already exists.
    ///   functions.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Add_DuplicateKey_ThrowsException() =>
      new TypeMemberInfoCollection {
        new(typeof(EmptyViewModel)),
        new(typeof(EmptyViewModel))
      };

    /*==========================================================================================================================
    | TEST: HAS MEMBER: PROPERTY INFO: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="TypeMemberInfoCollection"/> and confirms that <see cref="TypeMemberInfoCollection.HasMember{T
    ///   }(Type, String)"/> returns <c>true</c> when appropriate.
    ///   functions.
    /// </summary>
    [TestMethod]
    public void HasMember_PropertyInfo_ReturnsExpected() {

      var typeCollection        = new TypeMemberInfoCollection();

      Assert.IsTrue(typeCollection.HasMember(typeof(ContentTypeDescriptorTopicBindingModel), "ContentTypes"));
      Assert.IsFalse(typeCollection.HasMember(typeof(ContentTypeDescriptorTopicBindingModel), "MissingProperty"));
      Assert.IsFalse(typeCollection.HasMember<MethodInfo>(typeof(ContentTypeDescriptorTopicBindingModel), "Attributes"));

    }

    /*==========================================================================================================================
    | TEST: HAS GETTABLE PROPERTY: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.HasGettableProperty(Type,
    ///   String, Type)"/> returns the expected value.
    ///   functions.
    /// </summary>
    [TestMethod]
    public void HasGettableProperty_ReturnsExpected() {

      var dispatcher            = new MemberDispatcher();

      Assert.IsTrue(dispatcher.HasGettableProperty(typeof(ContentTypeDescriptorTopicBindingModel), "Key"));
      Assert.IsFalse(dispatcher.HasGettableProperty(typeof(ContentTypeDescriptorTopicBindingModel), "ContentTypes"));
      Assert.IsFalse(dispatcher.HasGettableProperty(typeof(ContentTypeDescriptorTopicBindingModel), "MissingProperty"));
      Assert.IsTrue(dispatcher.HasGettableProperty(typeof(TopicReferenceTopicViewModel), "TopicReference", typeof(TopicViewModel)));

    }

    /*==========================================================================================================================
    | TEST: HAS GETTABLE PROPERTY: WITH ATTRIBUTE: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> with a required <see cref="Attribute"/> constraint and confirms that <see
    ///   cref="MemberDispatcher.HasGettableProperty(Type, String, Type)"/> returns the expected value.
    ///   functions.
    /// </summary>
    [TestMethod]
    public void HasGettableProperty_WithAttribute_ReturnsExpected() {

      var dispatcher            = new MemberDispatcher(typeof(AttributeSetterAttribute));

      Assert.IsFalse(dispatcher.HasGettableProperty(typeof(Topic), nameof(Topic.Key)));
      Assert.IsTrue(dispatcher.HasGettableProperty(typeof(Topic), nameof(Topic.View)));

    }

    /*==========================================================================================================================
    | TEST: HAS SETTABLE PROPERTY: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.HasSettableProperty(Type,
    ///   String, Type)"/> returns the expected value.
    ///   functions.
    /// </summary>
    [TestMethod]
    public void HasSettableProperty_ReturnsExpected() {

      var dispatcher            = new MemberDispatcher();

      Assert.IsTrue(dispatcher.HasSettableProperty(typeof(ContentTypeDescriptorTopicBindingModel), "Key"));
      Assert.IsFalse(dispatcher.HasSettableProperty(typeof(ContentTypeDescriptorTopicBindingModel), "ContentTypes"));
      Assert.IsFalse(dispatcher.HasSettableProperty(typeof(ContentTypeDescriptorTopicBindingModel), "MissingProperty"));
      Assert.IsTrue(dispatcher.HasSettableProperty(typeof(TopicReferenceTopicViewModel), "TopicReference", typeof(TopicViewModel)));

    }

    /*==========================================================================================================================
    | TEST: HAS GETTABLE METHOD: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.HasGettableMethod(Type,
    ///   String, Type)"/> returns the expected value.
    ///   functions.
    /// </summary>
    [TestMethod]
    public void HasGettableMethod_ReturnsExpected() {

      var dispatcher            = new MemberDispatcher();

      Assert.IsTrue(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), "GetMethod"));
      Assert.IsFalse(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), "SetMethod"));
      Assert.IsFalse(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), "MissingMethod"));
      Assert.IsFalse(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), "GetComplexMethod"));
      Assert.IsTrue(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), "GetComplexMethod", typeof(TopicViewModel)));

    }

    /*==========================================================================================================================
    | TEST: HAS GETTABLE METHOD: WITH ATTRIBUTE: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> with a required <see cref="Attribute"/> constraint and confirms that <see
    ///   cref="MemberDispatcher.HasGettableMethod(Type, String, Type)"/> returns the expected value.
    /// </summary>
    /// <remarks>
    ///   In practice, we haven't encountered a need for this yet and, thus, don't have any semantically relevant attributes to
    ///   use in this situation. As a result, this example is a bit contrived.
    /// </remarks>
    [TestMethod]
    public void HasGettableMethod_WithAttribute_ReturnsExpected() {

      var dispatcher            = new MemberDispatcher(typeof(DisplayNameAttribute));

      Assert.IsTrue(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), nameof(MethodBasedViewModel.GetAnnotatedMethod)));
      Assert.IsFalse(dispatcher.HasGettableMethod(typeof(MethodBasedViewModel), nameof(MethodBasedViewModel.GetMethod)));

    }

    /*==========================================================================================================================
    | TEST: HAS SETTABLE METHOD: RETURNS EXPECTED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.HasSettableMethod(Type,
    ///   String, Type)"/> returns the expected value.
    ///   functions.
    /// </summary>
    [TestMethod]
    public void HasSettableMethod_ReturnsExpected() {

      var dispatcher            = new MemberDispatcher();

      Assert.IsTrue(dispatcher.HasSettableMethod(typeof(MethodBasedViewModel), nameof(MethodBasedViewModel.SetMethod)));
      Assert.IsFalse(dispatcher.HasSettableMethod(typeof(MethodBasedViewModel), nameof(MethodBasedViewModel.GetMethod)));
      Assert.IsFalse(dispatcher.HasSettableMethod(typeof(MethodBasedViewModel), nameof(MethodBasedViewModel.SetComplexMethod)));
      Assert.IsFalse(dispatcher.HasSettableMethod(typeof(MethodBasedViewModel), nameof(MethodBasedViewModel.SetParametersMethod)));
      Assert.IsFalse(dispatcher.HasSettableMethod(typeof(MethodBasedViewModel), "MissingMethod"));

    }

    /*==========================================================================================================================
    | TEST: GET MEMBERS: PROPERTY INFO: RETURNS PROPERTIES
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.GetMembers{T}(Type)"/>
    ///   functions.
    /// </summary>
    [TestMethod]
    public void GetMembers_PropertyInfo_ReturnsProperties() {

      var types = new MemberDispatcher();

      var properties = types.GetMembers<PropertyInfo>(typeof(ContentTypeDescriptor));

      Assert.IsTrue(properties.Contains("Key"));
      Assert.IsTrue(properties.Contains("AttributeDescriptors"));
      Assert.IsFalse(properties.Contains("IsTypeOf"));
      Assert.IsFalse(properties.Contains("InvalidPropertyName"));

    }

    /*==========================================================================================================================
    | TEST: GET MEMBER: PROPERTY INFO BY KEY: RETURNS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.GetMember{T}(Type, String)"
    ///   /> correctly returns the expected properties.
    /// </summary>
    [TestMethod]
    public void GetMember_PropertyInfoByKey_ReturnsValue() {

      var types = new MemberDispatcher();

      Assert.IsNotNull(types.GetMember<PropertyInfo>(typeof(ContentTypeDescriptor), "Key"));
      Assert.IsNotNull(types.GetMember<PropertyInfo>(typeof(ContentTypeDescriptor), "AttributeDescriptors"));
      Assert.IsNull(types.GetMember<PropertyInfo>(typeof(ContentTypeDescriptor), "InvalidPropertyName"));

    }

    /*==========================================================================================================================
    | TEST: GET MEMBER: METHOD INFO BY KEY: RETURNS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see
    ///   cref="MemberDispatcher.GetMember{T}(Type, String)"/> correctly returns the expected methods.
    /// </summary>
    [TestMethod]
    public void GetMember_MethodInfoByKey_ReturnsValue() {

      var types = new MemberDispatcher();

      Assert.IsNotNull(types.GetMember<MethodInfo>(typeof(ContentTypeDescriptor), "GetWebPath"));
      Assert.IsNull(types.GetMember<MethodInfo>(typeof(ContentTypeDescriptor), "AttributeDescriptors"));

    }

    /*==========================================================================================================================
    | TEST: GET MEMBER: GENERIC TYPE MISMATCH: RETURNS NULL
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that <see cref="MemberDispatcher.GetMember{T}(Type, String)
    ///   "/> does not return values if the types mismatch.
    /// </summary>
    [TestMethod]
    public void GetMember_GenericTypeMismatch_ReturnsNull() {

      var types = new MemberDispatcher();

      Assert.IsNull(types.GetMember<PropertyInfo>(typeof(ContentTypeDescriptor), "IsTypeOf"));
      Assert.IsNull(types.GetMember<MethodInfo>(typeof(ContentTypeDescriptor), "AttributeDescriptors"));

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: KEY: SETS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a key value can be properly set using the <see cref="
    ///   MemberDispatcher.SetPropertyValue(Object, String, Object?)"/> method.
    /// </summary>
    [TestMethod]
    public void SetPropertyValue_Key_SetsValue() {

      var types                 = new MemberDispatcher();
      var topic                 = TopicFactory.Create("Test", "ContentType");

      types.SetPropertyValue(topic, "Key", "NewKey");

      var key                   = types.GetPropertyValue(topic, "Key", typeof(string))?.ToString();

      Assert.AreEqual<string>("NewKey", topic.Key);
      Assert.AreEqual<string?>("NewKey", key);

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: NULL VALUE: SETS TO NULL
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that the <see cref="MemberDispatcher.SetPropertyValue(
    ///   Object, String, Object?)"/> method sets the property to <c>null</c>.
    /// </summary>
    [TestMethod]
    public void SetPropertyValue_NullValue_SetsToNull() {

      var types                 = new MemberDispatcher();
      var model                 = new NullablePropertyTopicViewModel() {
        NullableInteger         = 5
      };

      types.SetPropertyValue(model, "NullableInteger", null);

      Assert.IsNull(model.NullableInteger);

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: EMPTY VALUE: SETS TO NULL
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that the <see cref="MemberDispatcher.SetPropertyValue(
    ///   Object, String, Object?)"/> sets the target property value to <c>null</c> if the value is set to <see cref="String.
    ///   Empty"/>.
    /// </summary>
    [TestMethod]
    public void SetPropertyValue_EmptyValue_SetsToNull() {

      var types                 = new MemberDispatcher();
      var model                 = new NullablePropertyTopicViewModel() {
        NullableInteger         = 5
      };

      types.SetPropertyValue(model, "NullableInteger", "");

      Assert.IsNull(model.NullableInteger);

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: EMPTY VALUE: SETS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that the <see cref="MemberDispatcher.SetPropertyValue(
    ///   Object, String, Object?)"/> sets the value to its default if the value is set to <see cref="String.Empty"/> and the
    ///   target property type is not nullable.
    /// </summary>
    [TestMethod]
    public void SetPropertyValue_EmptyValue_ThrowsException() {

      var types                 = new MemberDispatcher();
      var model                 = new NonNullablePropertyTopicViewModel();

      types.SetPropertyValue(model, "NonNullableInteger", "ABC");

      Assert.IsNotNull(model.NonNullableInteger);

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: BOOLEAN: SETS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a boolean value can be properly set using the <see cref
    ///   ="MemberDispatcher.SetPropertyValue(Object, String, Object?)"/> method.
    /// </summary>
    [TestMethod]
    public void SetPropertyValue_Boolean_SetsValue() {

      var types                 = new MemberDispatcher();
      var topic                 = TopicFactory.Create("Test", "ContentType");

      types.SetPropertyValue(topic, "IsHidden", "1");

      Assert.IsTrue(topic.IsHidden);

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: DATE/TIME: SETS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a date/time value can be properly set using the <see
    ///   cref="MemberDispatcher.SetPropertyValue(Object, String, Object?)"/> method.
    /// </summary>
    [TestMethod]
    public void SetPropertyValue_DateTime_SetsValue() {

      var types                 = new MemberDispatcher();
      var topic                 = TopicFactory.Create("Test", "ContentType");

      types.SetPropertyValue(topic, "LastModified", "June 3, 2008");
      Assert.AreEqual<DateTime>(new(2008, 6, 3), topic.LastModified);

      types.SetPropertyValue(topic, "LastModified", "2008-06-03");
      Assert.AreEqual<DateTime>(new(2008, 6, 3), topic.LastModified);

      types.SetPropertyValue(topic, "LastModified", "06/03/2008");
      Assert.AreEqual<DateTime>(new(2008, 6, 3), topic.LastModified);

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: INVALID PROPERTY: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that an invalid property being set via the <see cref="
    ///   MemberDispatcher.SetPropertyValue(Object, String, Object?)"/> method throws an <see cref="InvalidOperationException"
    ///   />.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetPropertyValue_InvalidProperty_ReturnsFalse() {

      var types                 = new MemberDispatcher();
      var topic                 = TopicFactory.Create("Test", "ContentType");

      types.SetPropertyValue(topic, "InvalidProperty", "Invalid");

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: VALID VALUE: SETS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a value can be properly set using the <see cref="
    ///   MemberDispatcher.SetMethodValue(Object, String, Object?)"/> method.
    /// </summary>
    [TestMethod]
    public void SetMethodValue_ValidValue_SetsValue() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedViewModel();

      types.SetMethodValue(source, "SetMethod", "123");

      Assert.AreEqual<int>(123, source.GetMethod());

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: INVALID VALUE: DOESN'T SET VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a value set with an invalid value using the <see cref="
    ///   MemberDispatcher.SetMethodValue(Object, String, Object?)"/> method returns <c>false</c>.
    /// </summary>
    [TestMethod]
    public void SetMethodValue_InvalidValue_DoesNotSetValue() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedViewModel();

      types.SetMethodValue(source, "SetMethod", "ABC");

      Assert.AreEqual<int>(0, source.GetMethod());

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: INVALID MEMBER: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that setting an invalid method name using the <see cref="
    ///   MemberDispatcher.SetMethodValue(Object, String, Object?)"/> method throws an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetMethodValue_InvalidMember_ThrowsException() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedViewModel();

      types.SetMethodValue(source, "BogusMethod", "123");

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: VALID REFENCE VALUE: SETS VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a reference value can be properly set using the <see
    ///   cref="MemberDispatcher.SetMethodValue(Object, String, Object)"/> method.
    /// </summary>
    [TestMethod]
    public void SetMethodValue_ValidReferenceValue_SetsValue() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedReferenceViewModel();
      var reference             = new TopicViewModel();

      types.SetMethodValue(source, "SetMethod", reference);

      Assert.AreEqual<TopicViewModel?>(reference, source.GetMethod());

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: INVALID REFERENCE VALUE: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a value set with an invalid value using the <see cref="
    ///   MemberDispatcher.SetMethodValue(Object, String, Object)"/> method throws an <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SetMethodValue_InvalidReferenceValue_ThrowsException() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedReferenceViewModel();
      var reference             = new EmptyViewModel();

      types.SetMethodValue(source, "SetMethod", reference);

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: INVALID REFERENCE MEMBER: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that setting an invalid method name using the <see cref="
    ///   MemberDispatcher.SetMethodValue(Object, String, Object?)"/> method returns <c>false</c>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SetMethodValue_InvalidReferenceMember_ThrowsException() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedViewModel();

      types.SetMethodValue(source, "BogusMethod", new object());

    }

    /*==========================================================================================================================
    | TEST: SET METHOD VALUE: NULL REFERENCE VALUE: DOESN'T SET VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a <see cref="MemberDispatcher"/> and confirms that a value set with an null value using the <see cref="
    ///   MemberDispatcher.SetMethodValue(Object, String, Object)"/> method returns <c>false</c>.
    /// </summary>
    [TestMethod]
    public void SetMethodValue_NullReferenceValue_DoesNotSetValue() {

      var types                 = new MemberDispatcher();
      var source                = new MethodBasedReferenceViewModel();

      types.SetMethodValue(source, "SetMethod", (object?)null);

      Assert.IsNull(source.GetMethod());

    }

    /*==========================================================================================================================
    | TEST: SET PROPERTY VALUE: REFLECTION PERFORMANCE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets properties via reflection n number of times; quick way of evaluating the relative performance impact of changes.
    /// </summary>
    /// <remarks>
    ///   Unit tests should run quickly, so this isn't an optimal place to test performance. As such, the counter should be set
    ///   to a small number when not being actively tested. Nevertheless, this provides a convenient test harness for quickly
    ///   evaluating the performance impact of changes or optimizations without setting up a fully performance test. To adjust
    ///   the number of iterations, simply increment the "totalIterations" variable.
    /// </remarks>
    [TestMethod]
    public void SetPropertyValue_ReflectionPerformance() {

      var totalIterations = 1;
      var types = new MemberDispatcher();
      var topic = TopicFactory.Create("Test", "ContentType");

      int i;
      for (i = 0; i < totalIterations; i++) {
        types.SetPropertyValue(topic, "Key", "Key" + i);
      }

      Assert.AreEqual<string>("Key" + (i-1), topic.Key);

    }

  } //Class
} //Namespace