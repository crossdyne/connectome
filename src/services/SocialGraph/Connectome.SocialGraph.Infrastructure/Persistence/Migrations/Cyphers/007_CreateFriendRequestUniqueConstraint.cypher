CREATE CONSTRAINT friend_request_unique IF NOT EXISTS
FOR ()-[r:FRIEND_REQUEST]-()
REQUIRE (r.projectId, r.fromUserId, r.toUserId) IS UNIQUE;