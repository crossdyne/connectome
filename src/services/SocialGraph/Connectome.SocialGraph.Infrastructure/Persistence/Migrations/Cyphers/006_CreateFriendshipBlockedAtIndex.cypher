CREATE INDEX rel_friendship_blocked_at IF NOT EXISTS
FOR ()-[r:FRIENDSHIP]-()
ON (r.blockedAt)