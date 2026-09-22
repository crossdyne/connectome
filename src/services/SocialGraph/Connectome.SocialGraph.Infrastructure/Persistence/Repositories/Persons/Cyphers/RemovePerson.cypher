MATCH (p:Person {projectId: $projectId, userId: $userId})
DETACH DELETE p