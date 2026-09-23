CREATE INDEX friendship_requester_user IF NOT EXISTS
FOR ()-[r:FRIENDSHIP]-()
ON (r.projectId, r.requesterUserId);