MATCH (from:Person {projectId: $projectId})-[req:FRIEND_REQUEST]->(me:Person {projectId: $projectId, userId: $userId})
WITH from.userId AS userId, max(req.createdAt) AS latestCreatedAt
ORDER BY latestCreatedAt DESC
RETURN userId