﻿--------------------------------------------------------------------------------------------------------------------------------
-- UPDATE RELATIONSHIPS
--------------------------------------------------------------------------------------------------------------------------------
-- Saves the n:n mappings for related topics.
--------------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UpdateRelationships]
	@TopicID		INT,
	@RelationshipKey	VARCHAR(255),
	@RelatedTopics		TopicList	READONLY,
	@DeleteUnmatched	BIT	= 0
AS

--------------------------------------------------------------------------------------------------------------------------------
-- INSERT NOVEL VALUES
--------------------------------------------------------------------------------------------------------------------------------
INSERT
INTO	Relationships (
	  Source_TopicID,
	  RelationshipKey,
	  Target_TopicID
	)
SELECT	@TopicID,
	@RelationshipKey,
	TopicID
FROM	@RelatedTopics		Target
LEFT JOIN	Relationships		Existing
  ON	Target_TopicID		= TopicID
  AND	Source_TopicID		= @TopicID
  AND	RelationshipKey		= @RelationshipKey
WHERE	Target_TopicID		IS NULL

--------------------------------------------------------------------------------------------------------------------------------
-- DELETE UNMATCHED VALUES
--------------------------------------------------------------------------------------------------------------------------------
IF @DeleteUnmatched = 1
  BEGIN
    DELETE	Existing
    FROM	@RelatedTopics		Relationships
    RIGHT JOIN	Relationships		Existing
      ON	Target_TopicID		= TopicID
    WHERE	Source_TopicID		= @TopicID
      AND	ISNULL(TopicID, '')	= ''
      AND	RelationshipKey		= @RelationshipKey
  END

--------------------------------------------------------------------------------------------------------------------------------
-- RETURN TOPIC ID
--------------------------------------------------------------------------------------------------------------------------------
RETURN	@TopicID;