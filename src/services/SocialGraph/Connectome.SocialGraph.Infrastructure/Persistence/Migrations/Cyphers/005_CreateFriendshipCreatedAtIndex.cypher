CREATE INDEX rel_friendship_created_at IF NOT EXISTS
FOR ()-[r:FRIENDSHIP]-()
ON (r.createdAt)