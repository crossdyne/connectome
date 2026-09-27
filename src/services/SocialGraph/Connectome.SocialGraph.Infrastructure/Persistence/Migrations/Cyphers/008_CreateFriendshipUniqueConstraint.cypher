CREATE CONSTRAINT friendship_unique IF NOT EXISTS
FOR ()-[r:FRIENDSHIP]-()
REQUIRE (r.projectId, r.requesterUserId, r.recipientUserId) IS UNIQUE;