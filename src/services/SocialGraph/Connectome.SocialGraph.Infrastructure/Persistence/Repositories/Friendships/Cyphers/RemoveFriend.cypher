MATCH (requester:Person {projectId: $projectId, userId: $requesterUserId})-[friendship:FRIENDSHIP]-(removable:Person {projectId: $projectId, userId: $removableUserId})
DELETE friendship