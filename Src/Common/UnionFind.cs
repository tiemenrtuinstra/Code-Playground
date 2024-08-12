namespace Common;

public class UnionFind
{
    private int[] parent;
    private int[] rank;
    private int count;

    public UnionFind(int size)
    {
        parent = new int[size];
        rank = new int[size];
        count = size;

        for (int i = 0; i < size; i++)
        {
            parent[i] = i;
            rank[i] = 0;
        }
    }

    public int Find(int x)
    {
        if (parent[x] != x)
        {
            parent[x] = Find(parent[x]);
        }
        return parent[x];
    }

    public bool Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX == rootY)
        {
            return false;
        }

        if (rank[rootX] > rank[rootY])
        {
            parent[rootY] = rootX;
        }
        else if (rank[rootX] < rank[rootY])
        {
            parent[rootX] = rootY;
        }
        else
        {
            parent[rootY] = rootX;
            rank[rootX]++;
        }

        count--;
        return true;
    }

    public int Count() => count;
}
