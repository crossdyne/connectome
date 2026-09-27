MATCH (from:Person {projectId: $projectId, userId: $requesterUserId})
MATCH (to: Person  {projectId: $projectId, userId: $recipientUserId})
MATCH (from)-[req:FRIEND_REQUEST]->(to)
DELETE req 
RETURN true AS deleted