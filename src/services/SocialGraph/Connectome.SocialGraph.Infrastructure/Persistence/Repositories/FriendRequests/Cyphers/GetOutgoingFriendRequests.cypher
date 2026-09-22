MATCH (me:Person {projectId: $projectId, userId: $userId})-[req:FRIEND_REQUEST]->(to:Person {projectId: $projectId})
WITH to.userId AS userId, max(req.createdAt) AS latestCreatedAt
ORDER BY latestCreatedAt DESC
RETURN userId