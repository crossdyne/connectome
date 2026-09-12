MERGE (p:Person {projectId: $projectId, personId: $personId})
ON CREATE SET 
    p.userId = $userId,
    p.userName = $userName
RETURN p