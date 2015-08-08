/*==============================================================================================================================
| Author        Casey Margell, Ignia LLC
| Client        Ignia, LLC
| Project       Topics Library
|
| Purpose       The RenameEventArgs object defines an event argument type specific to rename events
|
\=============================================================================================================================*/
using System;

namespace Ignia.Topics.Providers {

  /*=========================================================================================================================
  | CLASS
  \------------------------------------------------------------------------------------------------------------------------*/
  public class RenameEventArgs : EventArgs {

  /*------------------------------------------------------------------------------------------------------------------------
  | PRIVATE VARIABLES
  \------------------------------------------------------------------------------------------------------------------------*/
    private Topic   _topic = null;

  /*------------------------------------------------------------------------------------------------------------------------
  | CONSTRUCTOR: TAXONOMY RENAME EVENT ARGS
  >-------------------------------------------------------------------------------------------------------------------------
  | Constructor for a rename event args object.
  \------------------------------------------------------------------------------------------------------------------------*/
    public RenameEventArgs() { }

    public RenameEventArgs(Topic topic) {
      _topic = topic;
    }

  /*------------------------------------------------------------------------------------------------------------------------
  | PROPERTY: TOPIC
  >-------------------------------------------------------------------------------------------------------------------------
  | Getter that returns the Topic object associated with the event
  \------------------------------------------------------------------------------------------------------------------------*/
    public Topic Topic {
      get {
        return _topic;
      }
      set {
        _topic = value;
      }
    }

  } //Class

} //Namespace
