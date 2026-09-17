CREATE CONSTRAINT friend_request_id_unique IF NOT EXISTS
FOR (r:FriendRequest)
REQUIRE r.friendRequestId IS UNIQUE;