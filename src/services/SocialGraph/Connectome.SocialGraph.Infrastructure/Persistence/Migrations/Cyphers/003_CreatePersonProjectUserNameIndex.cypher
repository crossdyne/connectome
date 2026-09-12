CREATE INDEX person_project_userName IF NOT EXISTS
FOR (p:Person)
ON (p.projectId, p.userName);