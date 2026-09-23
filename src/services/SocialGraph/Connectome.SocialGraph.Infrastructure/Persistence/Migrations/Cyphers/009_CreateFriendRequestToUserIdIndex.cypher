CREATE INDEX friend_request_to_user IF NOT EXISTS
FOR ()-[r:FRIEND_REQUEST]-()
ON (r.projectId, r.toUserId);