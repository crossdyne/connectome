MATCH (requester:Person {projectId: $projectId, userId: $requesterUserId})
MATCH (acceptor:Person {projectId: $projectId, userId: $acceptorUserId})
MATCH (requester)-[req:FRIEND_REQUEST]->(acceptor)
OPTIONAL MATCH (acceptor)-[reverseReq:FRIEND_REQUEST]->(requester)
WITH
    requester,
    acceptor,
    req,
    reverseReq,
    CASE
        WHEN toLower(requester.userId) <= toLower(acceptor.userId)
        THEN requester
        ELSE acceptor
    END AS startNode,
    CASE
        WHEN toLower(requester.userId) <= toLower(acceptor.userId)
        THEN acceptor
        ELSE requester
    END AS endNode
MERGE (startNode)-[friendship:FRIENDSHIP]->(endNode)
ON CREATE SET
    friendship.createdAt = $createdAt,
    friendship.requesterUserId = $requesterUserId,
    friendship.acceptorUserId = $acceptorUserId
DELETE req
WITH friendship, reverseReq
FOREACH (r IN CASE WHEN reverseReq IS NULL THEN [] ELSE [reverseReq] END | DELETE r)
RETURN friendship, 'ACCEPTED' AS action