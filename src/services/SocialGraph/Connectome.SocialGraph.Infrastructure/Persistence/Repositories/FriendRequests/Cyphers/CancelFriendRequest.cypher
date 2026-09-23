MATCH (from:Person {projectId: $projectId, userId: $fromUserId})
MATCH (to: Person  {projectId: $projectId, userId: $toUserId})
MATCH (from)-[req:FRIEND_REQUEST]->(to)
DELETE req 
RETURN true AS deleted