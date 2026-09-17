CREATE CONSTRAINT friend_request_from_to_unique IF NOT EXISTS
FOR (r:FriendRequest)
REQUIRE (r.fromUserId, r.toUserId) IS UNIQUE;