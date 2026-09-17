MATCH (p:Person {projectId: $projectId, userId: $userId})

MATCH (req:FriendRequest) 
WHERE req.projectId = $projectId AND (req.fromUserId = $userId OR req.toUserId = $userId)

DETACH DELETE req
WITH p
DETACH DELETE p