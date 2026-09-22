MATCH (from:Person {projectId: $projectId, userId: $fromUserId})
MATCH (to:Person   {projectId: $projectId, userId: $toUserId})

OPTIONAL MATCH (from)-[existingFriendship:FRIENDSHIP]-(to) 
WITH from, to, existingFriendship WHERE existingFriendship is NULL

MERGE (from)-[req:FRIEND_REQUEST]->(to)
ON CREATE SET 
    req.createdAt = $createdAt
RETURN req,
    CASE WHEN req.createdAt = $createdAt THEN 'CREATED' ELSE 'ALREADY_EXISTS' END AS action