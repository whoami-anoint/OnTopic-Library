﻿--------------------------------------------------------------------------------------------------------------------------------
-- GET CHILD TOPIC IDS
--------------------------------------------------------------------------------------------------------------------------------
-- Given a TopicID, returns the TopicID of each child topic, including nested topics.
--------------------------------------------------------------------------------------------------------------------------------

CREATE
FUNCTION [dbo].[GetChildTopicIDs]
(
	@TopicID		INT
)
RETURNS	@Topics		TABLE
(
	TopicID		INT
)
AS

BEGIN

  ------------------------------------------------------------------------------------------------------------------------------
  -- RETRIEVE VALUES
  ------------------------------------------------------------------------------------------------------------------------------
  INSERT
  INTO	@Topics
  SELECT	TopicID
  FROM	Topics
  WHERE	ParentID		= @TopicID

  ------------------------------------------------------------------------------------------------------------------------------
  -- RETURN
  ------------------------------------------------------------------------------------------------------------------------------
  RETURN

END