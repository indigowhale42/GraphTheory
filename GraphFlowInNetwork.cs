using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_V2
{
    static class GraphFlowInNetwork
    {
        public static Tuple<int[,], int> FlowMinCost(this Graph G)
        {
            int n = G.VertexNum;
            int di = 2 * G.maxFlow / 3;

            int[,] F = new int[G.VertexNum, G.VertexNum];
            List<List<int>> Band2 = new List<List<int>>();
            List<List<int>> Cost2 = new List<List<int>>();

            int minCost = 999999;

            for (int i = 0; i < n; i++)
            {
                List<int> temp2 = new List<int>();
                List<int> temp3 = new List<int>();

                for (int j = 0; j < n; j++)
                {
                    F[i, j] = 0;
                    if (j > i)
                    {
                        temp2.Add(G.AdjMatrix[i, j]);
                        temp3.Add(G.BandwithMatrix[i, j]);
                    }
                    else
                    {
                        temp2.Add(0);
                        temp3.Add(0);
                    }
                }
                
                Band2.Add(temp2);
                Cost2.Add(temp3);
            }

            int d = 0;

            while (d < di)
            {
                d = 0;
                List<int> T = new List<int>(), H = new List<int>(), Y = new List<int>();
                G.Deikstra(Cost2, T, H, n, 0, n - 1);
                int w = n - 1;
                Y.Add(w);
                while (w != 0)
                {
                    w = H[w];
                    Y.Add(w);
                }
                for (int i = Y.Count() - 1; i > 0; i--)
                {
                    F[Y[i], Y[i - 1]] = Band2[Y[i]][Y[i - 1]];
                }
                
                F = G.FordFulkerson(F, n);
                for (int i = 0; i < n; i++)
                    d += F[i, n - 1];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        Band2[i][j] -= F[i, j];
                        if (Band2[i][j] == 0)
                            Cost2[i][j] = 0;
                    }
                if (d > di)
                {
                    int dd = d - di;
                    for (int i = Y.Count() - 1; i > 0; i--)
                        F[Y[i], Y[i - 1]] -= dd;
                }
                int cos = 0;
                
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        cos += F[i, j] * G.BandwithMatrix[i,j];

                minCost = cos;
            }
            
            return Tuple.Create(F, minCost);
        }

        
        
        public static bool Deikstra(this Graph G, List<List<int>> C, List<int> T, List<int> H, int n, int s, int t)
        {
            List<int> X = new List<int>();
            for (int i = 0; i < n; i++)
            {
                T.Add(100 * n);
                H.Add(0);
                X.Add(0);
            }
            T[s] = 0;
            H[s] = 0;
            X[s] = 1;
            int v = s;
            while (v != t)
            {
                for (int i = 0; i < n; i++)
                {
                    if ((X[i] == 0) && (T[i] > T[v] + C[v][i]) && (C[v][i] != 0))
                    {
                        T[i] = T[v] + C[v][i];
                        H[i] = v;
                    }
                }
                int m = 100 * n;
                int v_temp = v;
                for (int i = 0; i < n; i++)
                {
                    if ((X[i] == 0) && ((T[i] < m) && (T[i] != 0)))
                    {
                        v = i;
                        m = T[i];
                    }
                }
                if (v == v_temp)
                    return false;
                if (v == s)
                    return false;
                X[v] = 1;
            }
            return true;
        }

        public static List<int> BellmanFordPath(this Graph G, int[,] C, int n, int s, int t, int sw, int[] len)
        {
            int x = s;
            List<int> H = new List<int>();
            List<int> T = new List<int>();
            List<int> Y = new List<int>();
            //vector<int> Y;
            for (int i = 0; i < n; i++)
            {
                T.Add(n * 100);
                H.Add(0);
                Y.Add(0);
            }
            T[s] = 0;
            List<int> Q = new List<int>();
            for (int i = 0; i < n; i++)
            {
                if (C[x, i] != 0)
                {
                    T[i] = C[x, i];
                    Y[i] = x;
                    Q.Add(i);
                    H[i] = 1;
                }
            }
            while (Q.Count() != 0)
            {
                x = Q[0];
                Q.RemoveAt(0);
                for (int i = 0; i < n; i++)
                {
                    if (C[x, i] != 0)
                    {
                        if (C[x, i] + T[x] < T[i])
                        {
                            T[i] = C[x, i] + T[x];
                            Y[i] = x;
                            if (H[i] == 0)
                            {
                                Q.Add(i);
                                H[i] = 1;
                            }
                            else
                            {
                                for (int j = 0; j < Q.Count(); j++)
                                    if (Q[j] == i)
                                    {
                                        for (int k = j; k < Q.Count() - 1; k++)
                                        {
                                            Q[k] = Q[k + 1];
                                            //Q.resize(Q.size() - 1);
                                            Q.RemoveAt(Q.Count - 1);
                                        }
                                    }
                                
                                List<int> tmp = new List<int> { i };
                                tmp.AddRange(Q);
                                Q = tmp;
                            }
                        }
                    }
                }
            }

            len[0] = T[t];
            
            return Y;
        }

    }
}
