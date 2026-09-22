CREATE INDEX rel_friend_request_created_at IF NOT EXISTS
FOR ()-[r:FRIEND_REQUEST]-()
ON (r.createdAt)