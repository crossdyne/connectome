CREATE INDEX friendship_acceptor_user IF NOT EXISTS
FOR ()-[r:FRIEND_REQUEST]-()
ON (r.projectId, r.acceptorUserId);