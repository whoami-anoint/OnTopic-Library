/*==============================================================================================================================
| Author        Katherine Trunkey, Ignia LLC
| Client        Ignia
| Project       Topics Library
\=============================================================================================================================*/
using System;

namespace Ignia.Topics {

  /*============================================================================================================================
  | CLASS: ATTRIBUTE VALUE
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Represents an instance of an Attribute value.
  /// </summary>
  /// <remarks>
  ///   Provides values and metadata specific to individual attribute value instances, such as state (e.g., IsDirty signifies 
  ///   that the attribute value has changed) and last modified date. State (IsDirty) is evaluated as part of the setter for 
  ///   Value; i.e., when the value changes, IsDirty is automatically set to true, if it wasn't previously.
  /// </remarks>
  public class AttributeValue {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    private     string  _key            = null;
    private     string  _value          = null;
    private     bool    _isDirty        = false;

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///  Initializes a new instance of the <see cref="AttributeValue"/> class.
    /// </summary>
    /// <remarks>
    ///   Optional overloads allow object to be constructed based on specified key/value pairs or for the
    ///   IsDirty (has been changed) property to be set.
    /// </remarks>
    public AttributeValue() { }

    /// <param name="key">
    ///   The string identifier for the <see cref="AttributeValue"/> collection item key/value pair.
    /// </param>
    /// <param name="value">
    ///   The string value text for the <see cref="AttributeValue"/> collection item key/value pair.
    /// </param>
    /// <param name="isDirty">
    ///   The boolean indicator noting whether the <see cref="AttributeValue"/> collection item has been changed.
    /// </param>
    public AttributeValue(string key, string value) {
      this.Key          = key;
      this.Value        = value;
    }

    /// <param name="key">
    ///   The string identifier for the <see cref="AttributeValue"/> collection item key/value pair.
    /// </param>
    /// <param name="value">
    ///   The string value text for the <see cref="AttributeValue"/> collection item key/value pair.
    /// </param>
    /// <param name="isDirty">
    ///   The boolean indicator noting whether the <see cref="AttributeValue"/> collection item has been changed.
    /// </param>
    public AttributeValue(string key, string value, bool isDirty) {
      this.Key          = key;
      this.Value        = value;
      this.IsDirty      = isDirty;
    }

    /*==========================================================================================================================
    | PROPERTY: KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///  Gets or sets the key of the attribute.
    /// </summary>
    public string Key {
      get {
        return _key;
      }
      set {
        _key = value;
      }
    }

    /*==========================================================================================================================
    | PROPERTY: VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the current value of the attribute. Automatically sets IsDirty based on whether the value has changed.
    /// </summary>
    public string Value {
      get {
        return _value;
      }
      set {
        _isDirty        = !value.Equals(_value) || _isDirty;
        _value          = value;
      }
    }

    /*==========================================================================================================================
    | PROPERTY: IS DIRTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///  Boolean setting which is set automatically when an attribute's Value is set to a new value.
    /// </summary>
    /// <remarks>
    ///   The IsDirty attribute is used by the <see cref="Topics.Providers.TopicDataProviderBase"/> to determine whether or not 
    ///   the value has been persisted to the database. If it is set to true, the attribute's value is sent to the database 
    ///   when <see cref="Topics.Providers.TopicDataProviderBase.Save(Topic, bool, bool)"/> is called. Otherwise, it is ignored, 
    ///   thus preventing the need to update attributes (or create new versions of attributes) whose values haven't changed.
    /// </remarks>
    public bool IsDirty {
      get {
        return _isDirty;
      }
      set {
        _isDirty        = value;
      }
    }

    /*=========================================================================================================================
    | PROPERTY: LAST MODIFIED
    \------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Read-only reference to last DateTime the <see cref="AttributeValue"/> instance was updated.
    /// </summary>
    public readonly DateTime LastModified = DateTime.Now;

  } //Class

} //Namepace
