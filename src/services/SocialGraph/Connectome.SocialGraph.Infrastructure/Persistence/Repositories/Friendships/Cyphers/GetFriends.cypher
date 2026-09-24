MATCH (requester:Person {projectId: $projectId, userId: $userId})-[friendShip:FRIENDSHIP]-(friend:Person {projectId: $projectId})
WHERE friendShip.blockedAt IS NULL
RETURN friend.userId AS userId