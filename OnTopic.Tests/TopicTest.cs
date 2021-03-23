﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Diagnostics.CodeAnalysis;
using OnTopic.Attributes;
using OnTopic.Collections;
using OnTopic.Metadata;
using OnTopic.Repositories;
using Xunit;

namespace OnTopic.Tests {

  /*============================================================================================================================
  | CLASS: TOPIC TEST
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides unit tests for the <see cref="Topic"/> class.
  /// </summary>
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class TopicTest {

    /*==========================================================================================================================
    | TEST: CREATE: RETURNS TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a topic using the factory method, and ensures it's correctly returned.
    /// </summary>
    [Fact]
    public void Create_ReturnsTopic() {
      var topic = TopicFactory.Create("Test", "Page");
      Assert.IsNotNull(topic);
      Assert.AreEqual<string>(topic.Key, "Test");
      Assert.AreEqual<string>(topic.ContentType, "Page");
    }

    /*==========================================================================================================================
    | TEST: CREATE: CONTENT TYPE: RETURNS DERIVED TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a topic of a content type which maps to a class derived from <see cref="Topic"/>, and ensures the derived
    ///   version of the <see cref="Topic"/> class is returned.
    /// </summary>
    [Fact]
    public void Create_ContentType_ReturnsDerivedTopic() {
      var topic = TopicFactory.Create("Test", "ContentTypeDescriptor");
      Assert.IsNotNull(topic);
      Assert.IsInstanceOfType(topic, typeof(ContentTypeDescriptor));
    }

    /*==========================================================================================================================
    | TEST: CREATE: ATTRIBUTE DESCRIPTOR: RETURNS FALLBACK
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a topic with a <see cref="Topic.ContentType"/> ending with <c>AttributeDescriptor</c> and ensures that, by
    ///   convention, a <see cref="AttributeDescriptor"/> is returned.
    /// </summary>
    /// <remarks>
    ///   This is a special use case to address the fact that we expect concrete types of <see cref="AttributeDescriptor"/> to
    ///   be in external plugin libraries, but the <see cref="ITopicRepository"/> only needs to know that they're an <see cref="
    ///   AttributeDescriptor"/>. This is similar to how other types will fallback to <see cref="Topic"/> if no matching type
    ///   can be found in the <see cref="TopicFactory.TypeLookupService"/>.
    /// </remarks>
    [Fact]
    public void Create_AttributeDescriptor_ReturnsFallback() {
      var topic = TopicFactory.Create("Test", "ArbitraryAttributeDescriptor");
      Assert.IsNotNull(topic);
      Assert.IsInstanceOfType(topic, typeof(AttributeDescriptor));
    }

    /*==========================================================================================================================
    | TEST: ID: CHANGE VALUE: THROWS ARGUMENT EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a topic using the factory method, and ensures that the ID cannot be modified.
    /// </summary>
    [Fact]
    [ExpectedException(typeof(InvalidOperationException), "Topic permitted the ID to be reset; this should never happen.")]
    public void Id_ChangeValue_ThrowsArgumentException() {

      var topic                 = TopicFactory.Create("Test", "ContentTypeDescriptor", 123);
      topic.Id                  = 124;

    }

    /*==========================================================================================================================
    | TEST: KEY: CHANGE VALUE: UPDATES PARENT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Changes a <see cref="Topic.Key"/>, and confirms that the <see cref="Topic.Parent"/>'s <see cref="Topic.Children"/>
    ///   collection is updated to reflect the new <see cref="Topic.Key"/>.
    /// </summary>
    /// <remarks>
    ///   By default, <see cref="KeyedTopicCollection{T}"/> won't automatically update its key if the underlying <see cref="
    ///   Topic.Key"/> changed. We have code that will handle that, however.
    /// </remarks>
    [Fact]
    public void Key_ChangeValue_UpdatesParent() {

      var parent                = TopicFactory.Create("Test", "ContentTypeDescriptor", 1);
      var topic                 = TopicFactory.Create("Original", "ContentTypeDescriptor", parent, 2);

      topic.Key                 = "New";

      Assert.AreEqual<string>("New", topic.Key);
      Assert.IsTrue(topic.IsDirty("Key"));
      Assert.IsTrue(parent.Children.Contains("New"));
      Assert.IsFalse(parent.Children.Contains("Original"));

    }

    /*==========================================================================================================================
    | TEST: PARENT: SET VALUE: UPDATES PARENT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets the parent of a topic and ensures it is correctly reflected in the object model.
    /// </summary>
    [Fact]
    public void Parent_SetValue_UpdatesParent() {

      var parentTopic           = TopicFactory.Create("Parent", "ContentTypeDescriptor");
      var childTopic            = TopicFactory.Create("Child", "ContentTypeDescriptor");

      parentTopic.Id            = 5;
      childTopic.Parent         = parentTopic;

      Assert.AreEqual<Topic?>(parentTopic.Children["Child"], childTopic);
      Assert.AreEqual<int>(5, childTopic.Parent.Id);

    }

    /*==========================================================================================================================
    | TEST: PARENT: SET TO DESCENDANT: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets the <see cref="Topic.Parent"/> to a <see cref="Topic"/> that is a descendant, and ensure it throws an <see cref="
    ///   ArgumentOutOfRangeException"/>.
    /// </summary>
    [Fact]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Parent_SetToDescendant_ThrowsException() {

      var parentTopic           = TopicFactory.Create("Parent", "ContentTypeDescriptor");
      var childTopic            = TopicFactory.Create("Child", "ContentTypeDescriptor", parentTopic);

      parentTopic.Parent        = childTopic;

    }

    /*==========================================================================================================================
    | TEST: PARENT: DUPLICATE KEY: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets the <see cref="Topic.Parent"/> to a <see cref="Topic"/> whose <see cref="Topic.Key"/> already exists in the new
    ///   <see cref="Topic.Parent"/> and ensures that an <see cref="InvalidKeyException"/> is thrown.
    /// </summary>
    [Fact]
    [ExpectedException(typeof(InvalidKeyException))]
    public void Parent_DuplicateKey_ThrowsException() {

      var parentTopic           = new Topic("Parent", "ContentTypeDescriptor");
      _                         = new Topic("Child", "ContentTypeDescriptor", parentTopic);
      _                         = new Topic("Child", "ContentTypeDescriptor", parentTopic);

    }

    /*==========================================================================================================================
    | TEST: PARENT: CHANGE VALUE: UPDATES PARENT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Changes the parent of a topic and ensures it is correctly reflected in the object model.
    /// </summary>
    [Fact]
    public void Parent_ChangeValue_UpdatesParent() {

      var sourceParent          = TopicFactory.Create("SourceParent", "ContentTypeDescriptor", 5);
      var targetParent          = TopicFactory.Create("TargetParent", "ContentTypeDescriptor", 10);
      var childTopic            = TopicFactory.Create("ChildTopic", "ContentTypeDescriptor", sourceParent);

      childTopic.Parent         = targetParent;

      Assert.AreEqual<Topic?>(targetParent.Children["ChildTopic"], childTopic);
      Assert.IsTrue(targetParent.Children.Contains("ChildTopic"));
      Assert.IsFalse(sourceParent.Children.Contains("ChildTopic"));
      Assert.AreEqual<int>(10, childTopic.Parent.Id);

    }

    /*==========================================================================================================================
    | TEST: UNIQUE KEY: RETURNS UNIQUE KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures the Unique Key is correct for a deeply nested child.
    /// </summary>
    [Fact]
    public void UniqueKey_ReturnsUniqueKey() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page");
      var childTopic            = TopicFactory.Create("ChildTopic", "Page");
      var grandChildTopic       = TopicFactory.Create("GrandChildTopic", "Page");

      childTopic.Parent         = parentTopic;
      grandChildTopic.Parent    = childTopic;

      Assert.AreEqual<string>("ParentTopic:ChildTopic:GrandChildTopic", grandChildTopic.GetUniqueKey());
      Assert.AreEqual<string>("/ParentTopic/ChildTopic/GrandChildTopic/", grandChildTopic.GetWebPath());

    }

    /*==========================================================================================================================
    | TEST: IS VISIBLE: RETURNS EXPECTED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that <see cref="Topic.IsVisible(Boolean)"/> returns expected values based on <see cref="Topic.IsHidden"/> and
    ///   <see cref="Topic.IsDisabled"/>.
    /// </summary>
    [Fact]
    public void IsVisible_ReturnsExpectedValue() {

      var hiddenTopic           = TopicFactory.Create("HiddenTopic", "Page");
      var disabledTopic         = TopicFactory.Create("DisabledTopic", "Page");
      var visibleTopic          = TopicFactory.Create("VisibleTopic", "Page");

      hiddenTopic.IsHidden      = true;
      disabledTopic.IsDisabled  = true;

      Assert.IsFalse(hiddenTopic.IsVisible());
      Assert.IsFalse(hiddenTopic.IsVisible(true));
      Assert.IsFalse(disabledTopic.IsVisible());
      Assert.IsTrue(disabledTopic.IsVisible(true));
      Assert.IsTrue(visibleTopic.IsVisible());
      Assert.IsTrue(visibleTopic.IsVisible(true));

    }

    /*==========================================================================================================================
    | TEST: TITLE: NULL VALUE: RETURNS KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that the title falls back appropriately.
    /// </summary>
    [Fact]
    public void Title_NullValue_ReturnsKey() {

      var untitledTopic         = TopicFactory.Create("UntitledTopic", "Page");
      var titledTopic           = TopicFactory.Create("TitledTopic", "Page");

      titledTopic.Title         = "Titled Topic";

      Assert.AreEqual<string>(untitledTopic.Title, "UntitledTopic");
      Assert.AreEqual<string>(titledTopic.Title, "Titled Topic");

    }

    /*==========================================================================================================================
    | TEST: LAST MODIFIED: UPDATE VALUE: RETURNS EXPECTED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Returns the last modified date via <see cref="Topic.LastModified"/>, and ensures it's returned correctly.
    /// </summary>
    [Fact]
    public void LastModified_UpdateLastModified_ReturnsExpectedValue() {

      var topic                 = TopicFactory.Create("Topic1", "Page");
      var lastModified          = new DateTime(1976, 10, 15);

      topic.LastModified        = lastModified;

      Assert.AreEqual<DateTime>(lastModified, topic.LastModified);

    }

    /*==========================================================================================================================
    | TEST: LAST MODIFIED: UPDATE VALUE: RETURNS EXPECTED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Returns the last modified date via <see cref="Topic.VersionHistory"/>, and ensures it's returned correctly.
    /// </summary>
    [Fact]
    public void LastModified_UpdateVersionHistory_ReturnsExpectedValue() {

      var topic                 = TopicFactory.Create("Topic2", "Page");

      var lastModified          = new DateTime(1976, 10, 15);

      topic.VersionHistory.Add(lastModified);

      Assert.AreEqual<DateTime>(lastModified, topic.LastModified);

    }

    /*==========================================================================================================================
    | TEST: LAST MODIFIED: UPDATE ATTRIBUTE: RETURNS EXPECTED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Returns the last modified date via <see cref="AttributeCollection"/>, and ensures it's returned correctly.
    /// </summary>
    [Fact]
    public void LastModified_UpdateValue_ReturnsExpectedValue() {

      var topic                 = TopicFactory.Create("Topic3", "Page");

      var lastModified          = new DateTime(1976, 10, 15);

      topic.Attributes.SetValue("LastModified", lastModified.ToShortDateString());

      Assert.AreEqual<DateTime>(lastModified, topic.LastModified);

    }

    /*==========================================================================================================================
    | TEST: BASE TOPIC: UPDATE VALUE: RETURNS EXPECTED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets a base topic to a topic entity, then replaces the references with a new topic entity. Ensures that both the
    ///   base topic as well as the underlying <see cref="AttributeRecord"/> correctly reference the new value.
    /// </summary>
    [Fact]
    public void BaseTopic_UpdateValue_ReturnsExpectedValue() {

      var topic                 = TopicFactory.Create("Topic", "Page");
      var firstBaseTopic        = TopicFactory.Create("BaseTopic", "Page");
      var secondBaseTopic       = TopicFactory.Create("BaseTopic", "Page", 1);
      var finalBaseTopic        = TopicFactory.Create("BaseTopic", "Page", 2);

      topic.BaseTopic           = firstBaseTopic;
      topic.BaseTopic           = secondBaseTopic;
      topic.BaseTopic           = finalBaseTopic;

      Assert.AreEqual<Topic?>(topic.BaseTopic, finalBaseTopic);
      Assert.AreEqual<int?>(2, topic.References.GetValue("BaseTopic")?.Id);

    }

    /*==========================================================================================================================
    | TEST: BASE TOPIC: RESAVED VALUE: RETURNS EXPECTED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets a base topic to an unsaved topic entity, then saves the entity and reestablishes the reference. Ensures that the
    ///   base topic is correctly set as a <see cref="Topic.References"/> entry.
    /// </summary>
    [Fact]
    public void BaseTopic_ResavedValue_ReturnsExpectedValue() {

      var topic                 = TopicFactory.Create("Topic", "Page");
      var baseTopic             = TopicFactory.Create("BaseTopic", "Page");

      topic.BaseTopic           = baseTopic;
      baseTopic.Id              = 5;
      topic.BaseTopic           = baseTopic;

      Assert.AreEqual<Topic?>(topic.BaseTopic, baseTopic);
      Assert.AreEqual<int?>(5, topic.References.GetValue("BaseTopic")?.Id);

    }

    /*==========================================================================================================================
    | TEST: BASE TOPIC: SET TO NULL: REMOVES VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets a base topic to a topic entity, then updates it to a null value. Ensures that the base topic is correctly
    ///   removed.
    /// </summary>
    [Fact]
    public void BaseTopic_SetToNull_RemovesValue() {

      var topic                 = TopicFactory.Create("Topic", "Page");
      var baseTopic             = TopicFactory.Create("BaseTopic", "Page");

      topic.BaseTopic           = baseTopic;
      topic.BaseTopic           = null;

      Assert.IsNull(topic.BaseTopic);

    }

    /*==========================================================================================================================
    | IS DIRTY: NEW TOPIC: RETURNS TRUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a new topic, and confirms that <see cref="Topic.IsDirty(Boolean, Boolean)"/> returns <c>true</c>.
    /// </summary>
    [Fact]
    public void IsDirty_NewTopic_ReturnsTrue() {

      var topic                 = TopicFactory.Create("Topic", "Page");

      Assert.IsTrue(topic.IsDirty());

    }

    /*==========================================================================================================================
    | IS DIRTY: EXISTING TOPIC: RETURNS FALSE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates an existing topic, and confirms that <see cref="Topic.IsDirty(Boolean, Boolean)"/> returns <c>false</c>.
    /// </summary>
    [Fact]
    public void IsDirty_ExistingTopic_ReturnsFalse() {

      var topic                 = TopicFactory.Create("Topic", "Page", 1);

      Assert.IsFalse(topic.IsDirty());

    }

    /*==========================================================================================================================
    | IS DIRTY: CHANGE KEY: RETURNS TRUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates an existing topic, changes the <see cref="Topic.Key"/>, and confirms that <see cref="Topic.IsDirty(Boolean,
    ///   Boolean)"/> returns <c>true</c>.
    /// </summary>
    [Fact]
    public void IsDirty_ChangeKey_ReturnsTrue() {

      var topic                 = TopicFactory.Create("Topic", "Page", 1);

      topic.Key                 = "NewTopic";

      Assert.IsTrue(topic.IsDirty());

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: EXISTING VALUES: REMAINS CLEAN
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates an existing topic, and updates the <see cref="Topic.Key"/>, <see cref="Topic.ContentType"/>, and <see cref="
    ///   Topic.Parent"/> to their existing values. Ensures that <see cref="Topic.IsDirty(String)"/> remains <c>false</c>.
    /// </summary>
    [Fact]
    public void IsDirty_ExistingValue_RemainsClean() {

      var parent                = TopicFactory.Create("Parent", "Page", 1);
      var topic                 = TopicFactory.Create("Topic", "Page", parent, 2);

      topic.Key                 = topic.Key;
      topic.ContentType         = topic.ContentType;
      topic.Parent              = parent;

      Assert.IsFalse(topic.IsDirty());

    }

    /*==========================================================================================================================
    | IS DIRTY: CHANGE COLLECTIONS: RETURNS TRUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates an existing topic, changes the <see cref="Topic.Attributes"/>, <see cref="Topic.References"/>, and <see cref=
    ///   "Topic.Relationships"/> collections, and confirms that <see cref="Topic.IsDirty(Boolean, Boolean)"/> returns
    ///   <c>true</c>.
    /// </summary>
    [Fact]
    public void IsDirty_ChangeCollections_ReturnsTrue() {

      var topic                 = TopicFactory.Create("Topic", "Page", 1);
      var related               = TopicFactory.Create("Related", "Page", 2);

      topic.Attributes.SetValue("Related", related.Key);
      topic.References.SetValue("Related", related);
      topic.Relationships.SetValue("Related", related);

      Assert.IsTrue(topic.IsDirty(true));
      Assert.IsTrue(topic.IsDirty("Related", true));

    }

    /*==========================================================================================================================
    | MARK CLEAN: CHANGE COLLECTIONS: RESETS IS DIRTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates an existing topic, changes the <see cref="Topic.Attributes"/>, <see cref="Topic.References"/>, and <see cref=
    ///   "Topic.Relationships"/> collections, and confirms that <see cref="Topic.MarkClean(Boolean, DateTime?)"/> resets the
    ///   value of <see cref="Topic.IsDirty(Boolean, Boolean)"/>.
    /// </summary>
    [Fact]
    public void MarkClean_ChangeCollections_ResetIsDirty() {

      var topic                 = TopicFactory.Create("Topic", "Page", 1);
      var related               = TopicFactory.Create("Related", "Page", 2);

      topic.Attributes.SetValue("Related", related.Key);
      topic.References.SetValue("Related", related);
      topic.Relationships.SetValue("Related", related);

      topic.MarkClean(true);

      Assert.IsFalse(topic.IsDirty(true));

    }

    /*==========================================================================================================================
    | MARK CLEAN: INCLUDE COLLECTIONS: RESETS IS DIRTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates an existing topic, changes the <see cref="Topic.Attributes"/>, <see cref="Topic.References"/>, and <see cref=
    ///   "Topic.Relationships"/> collections, and confirms that <see cref="Topic.MarkClean(String, Boolean)"/> resets the value
    ///   of <see cref="Topic.IsDirty(Boolean, Boolean)"/>.
    /// </summary>
    [Fact]
    public void MarkClean_IncludeCollections_ResetsIsDirty() {

      var topic                 = TopicFactory.Create("Topic", "Page", 1);
      var related               = TopicFactory.Create("Related", "Page", 2);

      topic.Attributes.SetValue("Related", related.Key);
      topic.References.SetValue("Related", related);
      topic.Relationships.SetValue("Related", related);

      topic.MarkClean("Related", true);

      Assert.IsFalse(topic.IsDirty("Related", true));
      Assert.IsFalse(topic.IsDirty(true));

    }


    /*==========================================================================================================================
    | MARK CLEAN: NEW TOPIC: REMAINS DIRTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a new <see cref="Topic"/> and confirms that <see cref="Topic.MarkClean()"/> does <i>not</i> reset the value of
    ///   <see cref="Topic.IsDirty(Boolean, Boolean)"/>. Topics that are marked as <see cref="Topic.IsNew"/> cannot be clean.
    /// </summary>
    [Fact]
    public void MarkClean_NewTopic_RemainsDirty() {

      var topic                 = TopicFactory.Create("Topic", "Page");

      topic.Attributes.SetValue("Attribute", "Test");

      topic.MarkClean("Attribute", true);
      topic.MarkClean(true);

      Assert.IsTrue(topic.IsDirty());
      Assert.IsTrue(topic.IsDirty("Attribute", true));

    }

  } //Class
} //Namespace