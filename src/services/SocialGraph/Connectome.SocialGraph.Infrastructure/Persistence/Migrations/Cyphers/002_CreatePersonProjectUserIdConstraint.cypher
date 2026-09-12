CREATE CONSTRAINT person_project_userId_unique IF NOT EXISTS
FOR (p:Person)
REQUIRE (p.projectId, p.userId) IS UNIQUE;