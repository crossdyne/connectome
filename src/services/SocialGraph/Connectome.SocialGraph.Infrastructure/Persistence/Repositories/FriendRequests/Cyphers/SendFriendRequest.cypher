MATCH (from:Person {userId: $fromUserId, projectId: $projectId})
MATCH (to:Person {userId: $toUserId, projectId: $projectId})

MERGE (req:FriendRequest {projectId: $projectId, fromUserId: $fromUserId, toUserId: $toUserId})

ON CREATE SET 
    req.friendRequestId = $friendRequestId,
    req.createdAt = $createdAt,
    req.status = $status

MERGE (from)-[:SENT_REQUEST]->(req)
MERGE (req)-[:RECEIVED_REQUEST]->(to)

RETURN req,
    CASE WHEN req.createdAt = $createdAt THEN 'CREATED' ELSE 'ALREADY_EXISTS' END AS action