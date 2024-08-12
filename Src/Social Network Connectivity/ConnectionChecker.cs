using Common;
using SocialNetworkConnectivity.Dto;

namespace SocialNetworkConnectivity;

internal class ConnectionChecker
{
    public int EarliestTime(int numberOfUsers, HashSet<Connection> connections)
    {
        UnionFind uf = new UnionFind(numberOfUsers);

        foreach (Connection connection in connections)
        {
            uf.Union(connection.User1Id, connection.User2Id);
            if (uf.Count() == 1)
            {
                return connection.Timestamp;
            }
        }

        return -1;
    }
}