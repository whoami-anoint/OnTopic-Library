﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System.Data;

namespace OnTopic.Data.Sql.Models {

  /*============================================================================================================================
  | CLASS: TOPIC LIST (DATA TABLE)
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Extends <see cref="DataTable"/> to model the schema for the <c>TopicList</c> user-defined table type.
  /// </summary>
  internal class TopicListDataTable: DataTable {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a new <see cref="DataTable"/> with the appropriate schema for the <c>TopicList</c> user-defined
    ///   table type.
    /// </summary>
    internal TopicListDataTable() {

      /*------------------------------------------------------------------------------------------------------------------------
      | COLUMN: Topic ID
      \-----------------------------------------------------------------------------------------------------------------------*/
      Columns.Add(
        new DataColumn("TopicID", typeof(int))
      );

    }

    /*==========================================================================================================================
    | ADD ROW
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a convenience method for adding a new <see cref="DataRow"/> based on the expected column values.
    /// </summary>
    internal DataRow AddRow(int topicId) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Define record
      \-----------------------------------------------------------------------------------------------------------------------*/
      var record                = NewRow();
      record["TopicID"]         = topicId;

      /*------------------------------------------------------------------------------------------------------------------------
      | Add record
      \-----------------------------------------------------------------------------------------------------------------------*/
      Rows.Add(record);

      /*------------------------------------------------------------------------------------------------------------------------
      | Return record
      \-----------------------------------------------------------------------------------------------------------------------*/
      return record;

    }

  } //Class
} //Namespaces