MATCH (from:Person {projectId: $projectId, userId: $requesterUserId})
MATCH (to:Person   {projectId: $projectId, userId: $recipientUserId})

OPTIONAL MATCH (from)-[existingFriendship:FRIENDSHIP]-(to)
OPTIONAL MATCH (from)-[existingReq:FRIEND_REQUEST]->(to)
OPTIONAL MATCH (to)-[reverseReq:FRIEND_REQUEST]->(from)

WITH from, to, existingFriendship, existingReq, reverseReq
WHERE existingFriendship IS NULL 
  AND existingReq IS NULL 
  AND reverseReq IS NULL

MERGE (from)-[req:FRIEND_REQUEST]->(to)
ON CREATE SET 
    req.createdAt = $createdAt,
    req.requesterUserId = $requesterUserId,
    req.recipientUserId = $recipientUserId,
    req.projectId = $projectId

RETURN req.createdAt AS createdAt, 'CREATED' AS action