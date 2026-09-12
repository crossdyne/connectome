CREATE CONSTRAINT person_project_personId_unique IF NOT EXISTS
FOR (p:Person)
REQUIRE (p.projectId, p.personId) IS UNIQUE;