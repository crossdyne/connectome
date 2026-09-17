CREATE INDEX friend_request_created_at IF NOT EXISTS
FOR (r:FriendRequest)
ON (r.createdAt);