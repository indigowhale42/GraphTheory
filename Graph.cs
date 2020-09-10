using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Graph_V2
{
    class Edge : IComparable<Edge>, IEquatable<Edge>
    {
        public int v1, v2;
        public int weight;

        public Edge(int V1, int V2)
        {
            this.v1 = V1;
            this.v2 = V2;
            this.weight = 1;
        }

        public Edge(int V1, int V2, int weight)
        {
            this.v1 = V1;
            this.v2 = V2;
            this.weight = weight;
        }

        public int CompareTo(Edge other)
        {
            if (other == null)
                return 1;

            return this.weight.CompareTo(other.weight);
            //throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }

        public bool Equals(Edge other)
        {
            return other != null &&
                   v1 == other.v1 &&
                   v2 == other.v2;
        }
    }

    class Graph 
    {
        public int VertexNum { get; set; }
        public int EdgesNum { get; set; }

        public bool IsOriented { get; set; }
        public bool HasNegativeWeight { get; set; }
        public bool IsNetwork { get; set; }

        public int[,] AdjMatrix;
        public List<Edge> Edges;

        public int[,] BandwithMatrix;
        public int maxFlow = 0;

        public int[,] EulerianMatrix;
        public int[,] HamiltonianMatrix;

        public int FloydWarshallIterations = 0;
        public int FloydWarshallIterationsForOneVertex = 0;
        public int BellmanFordIterations = 0;
        public int DijkstraIterations = 0;

        public int PrimaIterations = 0;
        public int KruskalIterations = 0;
        
        /*
       

        public bool IsEulerian()
        {
            if (!IsInitialized)
                throw new Exception("Создайте граф. (Проверка на эйлеров цикл невозможна)");

            if (!IsFilledAdjMatrix)
                FillAdjacencyMatrix();

            int deg = 0;

            for (int i = 0; i < VertexNum; i++)
            {
                for (int j = 0; j < VertexNum; j++)
                {
                    if (AdjMatrix[i, j] != 0)
                    {
                        deg++;
                    }
                }
                if (deg % 2 == 1)
                    return false;
                deg = 0;
            }
            return true;
        }

        public List<string> MakeEulerian()
        {
            if (!IsInitialized)
                throw new Exception("Граф еще не создан! (попытка сделать граф Эйлеровым)");

            if (IsOriented)
                Disorientate();

            List<string> Results = new List<string>
            {
                "Результат преобразования графа в Эйлеров:"
            };

            if (IsEulerian())
            {
                Results.Add("Преобразование не требуется. Эйлеров цикл существует.");
                return Results;
            }

            int[] D = new int[VertexNum];
            foreach (Edge e in Edges)
            {
                D[e.v1]++;
                D[e.v2]++;
            }

            List<int> oddV = new List<int>();
            for (int i = 0; i < VertexNum; i++)
            {
                if (D[i] % 2 == 1)
                    oddV.Add(i);
            }

            int u;
            Random rand = new Random();

            Results.Add("Добавленные ребра:");
            bool added;
            int countOddV = oddV.Count;
            List<int> nokottaV = new List<int>();
            for (int count = 0; count < countOddV; count++)
            {
                u = oddV.First();
                oddV.RemoveAt(0);
                added = false;
                for (int i = 0; i < oddV.Count; i++)
                {
                    if (AdjMatrix[u, oddV[i] ] == 0)
                    {
                        Edge edge = new Edge(u, oddV[i], rand.Next(10));
                        E.Add(edge);
                        Results.Add(String.Format("({0}, {1}) - {2}", edge.v1, edge.v2, edge.weight));
                        oddV.RemoveAt(i);
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    nokottaV.Add(u);
                }
            }


            return Results;
        }
        */
        

        public Graph( int vertexNum )
        {
            VertexNum = 0;
            EdgesNum = 0;

            IsOriented = false;
            HasNegativeWeight = false;
            IsNetwork = false;

            AdjMatrix = null;
            Edges = new List<Edge>();

            BandwithMatrix = null;

            EulerianMatrix = null;

            HamiltonianMatrix = null;

            Generate(vertexNum);
        }


        // создает ориентированный граф
        private void Generate(int vertexNum) // Распределение Биномиальное
        {
            int n = vertexNum - 1;  // типа вершины у нас будут имть степень от 1 до n
            double p = 0.4; //вероятность успеха
            double q = 1 - p;
            double c = p / q;

            int countVertex_d1 = 0;
            int countD = 0;

            int[] V = new int[vertexNum];

            Random rand = new Random();
            for (int i = 0; i < vertexNum; i++)    // для каждой вершины
            {
                p = Math.Pow(q, n);
                int x = 0;
                double r = Convert.ToDouble(rand.Next(10000)) / 10000.0;   // получаем число от 0 до 1 
                bool flag = true;
                while (flag)
                {
                    r = r - p;
                    if (r <= 0)
                    {
                        if (x < n)
                        {
                            V[i] = x;
                            countD += x;
                            if (x == 0)
                                countVertex_d1++;
                            flag = false;
                        }
                        if (flag)
                        {
                            r = Convert.ToDouble(rand.Next(10000)) / 10000.0;
                            p = Math.Pow(q, n);
                        }
                    }
                    else
                    {
                        x = x + 1;
                        if (x == n)
                            x--;
                        p = p * c * (n + 1 - x) / x;
                    }
                }
            }


            while (countVertex_d1 > (countD / 2))
            {
                int j = 0;
                for (; V[j] != 0 && j < vertexNum; j++) ;
                V[j]++;
                countVertex_d1--;
                countD++;
            }


            while (countD < vertexNum)
            {
                if (V[0] == 0)
                    countVertex_d1--;
                countD++;
                V[0]++;
            }




            Array.Sort(V);
            Array.Reverse(V);
            int[] Vertex_d = new int[vertexNum];
            V.CopyTo(Vertex_d, 0);

            for (int i = 0; i < vertexNum; i++)
            {
                if (Vertex_d[i] > (vertexNum - 1 - i))
                {
                    Vertex_d[i] = V[i] = vertexNum - 1 - i;
                    if (i == vertexNum - 1)
                        countVertex_d1++;
                }
            }

            VertexNum = vertexNum; // количество вершин
            // Если естьтолько одна вершина из которой больше ничего не выходит, то граф - сеть
            if (countVertex_d1 == 1)
                IsNetwork = true;

            for (int i = 0; (vertexNum > 2) && (i < vertexNum - 1) && (countVertex_d1 > 0); i++)
            {
                for (int j = i + 1; ((i == 0) ? V[i] > 1 : V[i] > 2) && (j < vertexNum) && (countVertex_d1 > 0); j++)
                {
                    if (V[j] == 0)
                    {
                        Edges.Add(new Edge(i, j, rand.Next(10)));
                        V[i]--;
                        countVertex_d1--;
                    }
                }
            }

            for (int i = 0; i < vertexNum; i++)
            {
                for (int j = i + 1; (V[i] > 0) && (j < vertexNum); j++)
                {
                    Edges.Add(new Edge(i, j, rand.Next(10)));
                    V[i]--;
                }
            }

            EdgesNum = Edges.Count;
            IsOriented = true;

            foreach (Edge e in Edges)
            {
                if (e.weight == 0)
                    e.weight++;
            }

            FillAdjacencyMatrix();

            maxFlow = 0;
            BandwithMatrix = null;
        }

        //заполняет матрицу смежности
        public void FillAdjacencyMatrix()
        {
            if (Edges == null)
                throw new NullReferenceException("Список ребер - null (Ошибка в заполнении матрицы смежности)");

            AdjMatrix = new int[VertexNum, VertexNum];

            for (int i = 0; i < EdgesNum; i++)
            {
                AdjMatrix[Edges[i].v1, Edges[i].v2] = Edges[i].weight;
                if (!IsOriented)
                    AdjMatrix[Edges[i].v2, Edges[i].v1] = Edges[i].weight;
            }
        }

        //заполняет матрицу смежности
        public void FillBandwithMatrix()
        {
            if (Edges == null)
                throw new NullReferenceException("Список ребер - null (Ошибка в заполнении матрицы стоимости)");

            BandwithMatrix = new int[VertexNum, VertexNum];

            for (int i = 0; i < EdgesNum; i++)
            {
                BandwithMatrix[Edges[i].v1, Edges[i].v2] = Edges[i].weight;
                if (!IsOriented)
                    BandwithMatrix[Edges[i].v2, Edges[i].v1] = Edges[i].weight;
            }
        }

        public bool SetNegativeWeight()
        {
            if (HasNegativeWeight)
                return true;

            HasNegativeWeight = true;
            for (int i = 0; i < EdgesNum; i += 2)
            {
                if (Edges[i].weight > 0)
                    Edges[i].weight = -Edges[i].weight;
            }

            FillAdjacencyMatrix();

            maxFlow = 0;
            return HasNegativeWeight;
        }

        public bool SetPositiveWeight()
        {
            if (!HasNegativeWeight)
                return true;

            HasNegativeWeight = false;
            for (int i = 0; i < EdgesNum; i++)
            {
                if (Edges[i].weight < 0)
                    Edges[i].weight = -Edges[i].weight;
            }

            FillAdjacencyMatrix();

            maxFlow = 0;

            return !HasNegativeWeight;
        }
               
        public void Disorientate()
        {
            if (!IsOriented)
                return;

            IsOriented = false;
            IsNetwork = false;

            FillAdjacencyMatrix();
        }

        public void Orientate()
        {
            
            if (IsOriented)
                return;

            IsOriented = true;

            FillAdjacencyMatrix();
        }
  
        // возвращает список добвленных в граф ребер
        public List<Edge> ToNetwork()
        {
            List<Edge> added = new List<Edge>();
            Random rand = new Random();
            if (!IsNetwork)
            {
                for (int i = 0; i < VertexNum - 1; i++)
                    if (PathGaImasuKa(i, VertexNum - 1) == 0)
                    {
                        Edge edge = new Edge(i, VertexNum - 1, rand.Next(10));
                        Edges.Add(edge);
                        added.Add(edge);
                        FillAdjacencyMatrix();
                    }
            }

            IsNetwork = true;
            return added;
        }


        #region Экстремальные пути

        // определение экстремальных путей с заданным количеством ребер
        public int[,] Shimbell(bool isMin, int edgesInPath)
        {
            if (HasNegativeWeight)
                throw new Exception("Не реализуется на графе с \"-\" весами");

            if (edgesInPath <= 0)
                throw new Exception("Количество ребер должно быть больше 0");

            if (edgesInPath > EdgesNum)
                throw new Exception("кол-во ребер пути должно быть <= кол-ва ребер графа");

            if (edgesInPath == 1)
            {
                return AdjMatrix;
            }

            int _p = VertexNum;

            int[,] tmpMatrix = new int[_p, _p];
            int[,] R = new int[_p, _p];
            int[] Wij = new int[_p];

            AdjMatrix.CopyMatrixTo(tmpMatrix, _p, _p);

            for (int n = 2; n <= edgesInPath; n++)
            {
                for (int i = 0; i < _p; i++)
                {
                    for (int j = 0; j < _p; j++)
                    {
                        for (int k = 0; k < _p; k++)
                            Wij[k] = ShimbellMul(tmpMatrix[i, k], AdjMatrix[k, j]);

                        if (isMin)
                        {
                            int k;
                            for (k = 0; (k < _p) && (Wij[k] <= 0); k++) ;
                            if (k < _p)
                                R[i, j] = Wij[k];
                            else
                                R[i, j] = 0;
                            for (k++; k < _p; k++)
                            {
                                if ((Wij[k] < R[i, j]) && (Wij[k] > 0))
                                    R[i, j] = Wij[k];
                            }
                        }
                        else
                            R[i, j] = Wij.Max();
                    }
                }
                if (n < edgesInPath)
                    R.CopyMatrixTo(tmpMatrix, _p, _p);
            }
            return R;
        }


        public int PathGaImasuKa(int v1, int v2)
        {
            if (HasNegativeWeight)
                throw new Exception("Не реализуется на графе с \"-\" весами");

            if ((v1 < 0) || (v2 < 0) || (v1 > VertexNum - 1) || (v2 > VertexNum - 1))
                throw new Exception(String.Format("Номера вершин должны быть от 0 до {0}", VertexNum - 1));


            if (AdjMatrix[v1, v2] != 0)
                return 1;
            else
            {
                int _p = VertexNum;
                int[,] tmpMatrix = new int[_p, _p];
                int[,] R = new int[_p, _p];
                int[] Wij = new int[_p];

                AdjMatrix.CopyMatrixTo(tmpMatrix, _p, _p);

                int n = 2;
                for (; n <= _p && R[v1, v2] == 0; n++)
                {
                    for (int i = 0; i < _p; i++)
                    {
                        for (int j = 0; j < _p; j++)
                        {
                            for (int k = 0; k < _p; k++)
                                Wij[k] = ShimbellMul(tmpMatrix[i, k], AdjMatrix[k, j]);
                            R[i, j] = Wij.Max();
                        }
                    }
                    if (n < _p)
                        R.CopyMatrixTo(tmpMatrix, _p, _p);
                }

                if (R[v1, v2] != 0)
                    return n - 1;
                else
                    return 0;
            }
        }


        private int ShimbellMul(int a, int b)
        {
            return (a == 0 || b == 0) ? 0 : a + b;
        }


        public int[] Dijkstra(int s)
        {
            if (HasNegativeWeight)
                throw new Exception("Не реализуется на графе с \"-\" весами");

            if ((s < 0) || (s > VertexNum - 1))
                throw new Exception(String.Format("Номер вершины должен быть от 0 до {0}", VertexNum - 1));

            int infinite = System.Int32.MaxValue;
            bool[] visited = new bool[VertexNum]; // постоянна ли метка
            int[] d = new int[VertexNum];
            for (int i = 0; i < VertexNum; i++)
            {
                visited[i] = false;
                d[i] = infinite;
            }
            d[s] = 0;
            visited[s] = true;

            int countIterations = 0;

            //int[] path = new int[VertexNum];

            int v = s; // текущая вершина
            int min_u;
            int min_d;
            for (int p = s + 1; p < VertexNum; p++)
            {
                min_u = 0;
                min_d = infinite;
                for (int u = 0; u < VertexNum; u++)
                {
                    countIterations++;
                    if (AdjMatrix[v, u] != 0)
                    {
                        if (visited[u] == false && (d[u] > (d[v] + AdjMatrix[v, u])))
                        {
                            d[u] = d[v] + AdjMatrix[v, u];
                            //path[u] = v;
                        }
                    }
                    if (visited[u] == false && d[u] < min_d) // из всех вершин с временными метками
                    {// выбираем вершину с наим.знач метки.
                        min_u = u;
                        min_d = d[u];
                    }
                }


                v = min_u;  // текущ. вершинра - вершина с мин. меткой
                visited[min_u] = true;  // делаем метку постоянной
            }

            DijkstraIterations = countIterations;

            return d;
        }


        public int?[] BellmanFord(int s)
        {
            if ((s < 0) || (s > VertexNum - 1))
                throw new Exception(String.Format("Номер вершины должен быть от 0 до {0}", VertexNum - 1));

            int? infinite = null;

            int?[] d = new int?[VertexNum];
            for (int i = 0; i < VertexNum; i++)
                d[i] = infinite;
            d[s] = 0;

            int countIterations = 0;

            for (int i = s; i < VertexNum; i++)
            {
                for (int j = 0; j < VertexNum; j++)
                {
                    countIterations++;
                    if (AdjMatrix[i, j] != 0)
                    {
                        if ((d[j] == infinite) || (d[j] > d[i] + AdjMatrix[i, j]))
                            d[j] = d[i] + AdjMatrix[i, j];
                    }
                }
            }

            BellmanFordIterations = countIterations;

            return d;
        }


        public int?[,] FloydWarshall(int s = 0)
        {
            if ((s < 0) || (s > VertexNum - 1))
                throw new Exception(String.Format("Номер вершины должен быть от 0 до {0}", VertexNum - 1));

            int? infinite = null;

            int?[,] T = new int?[VertexNum, VertexNum];
            for (int i = 0; i < VertexNum; i++)
            {
                for (int j = 0; j < VertexNum; j++)
                {
                    if (AdjMatrix[i, j] == 0 && i != j)
                        T[i, j] = infinite;
                    else
                        T[i, j] = AdjMatrix[i, j];
                }
            }

            int countIterations = 0;
            int countIterationsS = 0;

            for (int k = 0; k < VertexNum; k++)
            {
                for (int i = 0; i < VertexNum; i++)
                {
                    for (int j = 0; j < VertexNum; j++)
                    {
                        if (k == s)
                            countIterationsS++;
                        countIterations++;
                        if ((i != k) && (j != k) && (T[i, k] != infinite) && (T[k, j] != infinite) &&
                            ((T[i, j] == infinite) || (T[i, j] > T[i, k] + T[k, j])))
                        {
                            T[i, j] = T[i, k] + T[k, j];
                        }
                    }
                }
            }

            FloydWarshallIterations = countIterations;
            FloydWarshallIterationsForOneVertex = countIterationsS;

            return T;
        }

        #endregion Экстремальные пути


        #region Остовные деревья

        // для неориентированных графов. а если и ориентир., то делаем вид, что нет
        public Tuple<int[,], int> Kirchhoff()
        {
            Disorientate();

            int[,] Kirchhoff = new int[VertexNum, VertexNum];

            for (int i = 0; i < VertexNum; i++)
            {
                for (int j = i + 1; j < VertexNum; j++)
                    if (AdjMatrix[i, j] != 0)
                    {
                        Kirchhoff[i, j] = Kirchhoff[j, i] = -1;
                        Kirchhoff[i, i]++;
                        Kirchhoff[j, j]++;
                    }
            }

            int[,] Minor = Kirchhoff.GetMinor(VertexNum, VertexNum, 0, 0);

            int d = Minor.Det(VertexNum - 1);

            return Tuple.Create(Kirchhoff, d);
        }


        public Tuple<List<Edge>, int> Prima()
        {
            Disorientate();

            int countIterations = 0;

            List<Edge> Ostov = new List<Edge>(); // множество ребер в остове
            List<int> S = new List<int>(); // множество вершин в остове

            List<int> S2 = new List<int>(); // множество еще не добавленных вершин

            //bool[] visited = new bool[G.VertexNum];
            //List<int> was = new List<int>(); // посещенные вершины

            int MinCost = 0;

            int infinity = System.Int32.MaxValue;
            int min = infinity, ne = 1;

            int[,] C = new int[VertexNum, VertexNum]; // матрица длин ребер. Нет ребра - infinity
            for (int i = 0; i < VertexNum; i++)
            {
                for (int j = 0; j < VertexNum; j++)
                {
                    if (AdjMatrix[i, j] != 0)
                        C[i, j] = AdjMatrix[i, j];
                    else
                        C[i, j] = infinity;
                }
            }

            // посетили нулевую вершину.
            //visited[0] = true;
            S.Add(0);

            for (int i = 1; i < VertexNum; i++)
            {
                S2.Add(i);
            }

            int u = 0, v = 0;

            while (ne < VertexNum)
            {
                min = infinity;
                foreach (int i in S)
                {
                    foreach (int j in S2)
                    {
                        if (C[i, j] < min)
                        {
                            min = C[i, j];
                            u = i;
                            v = j;
                        }
                        countIterations++;
                    }
                }

                S.Add(v);
                S2.RemoveAt(S2.FindIndex(x => x == v));

                Ostov.Add(new Edge(u, v, min));
                MinCost += min;
                ne++;

                C[u, v] = C[v, u] = infinity;
            }
            PrimaIterations = countIterations;
            return Tuple.Create(Ostov, MinCost);

           
        }


        public Tuple<List<Edge>, int> Kruskal()
        {
            Disorientate();

            int countIterations = 0;

            List<Edge> Ostov = new List<Edge>(); // множество ребер в остове
            List<int> S = new List<int>(); // множество вершин в остове

            bool[] visited = new bool[VertexNum];

            int MinCost = 0;

            Edges.Sort();

            visited[0] = true;
            for (int i = 0; i < VertexNum; i++)
            {
                foreach (Edge e in Edges)
                {
                    if (visited[e.v1])
                    {
                        if (!visited[e.v2])
                        {
                            visited[e.v2] = true;
                            Ostov.Add(e);
                            MinCost += e.weight;
                            break;
                        }
                    }
                    else if (visited[e.v2])
                    {
                        if (!visited[e.v1])
                        {
                            visited[e.v1] = true;
                            Ostov.Add(new Edge(e.v2, e.v1, e.weight));
                            MinCost += e.weight;
                            break;
                        }
                    }
                    countIterations++;
                }
            }

            KruskalIterations = countIterations;
            return Tuple.Create(Ostov, MinCost);
        }


        public Tuple<List<int>, List<Edge>> Prufer()
        {

            List<Edge> OstovKruskal = Kruskal().Item1;
            List<int> Code = new List<int>();
            int infinity = System.Int32.MaxValue;
            int min = infinity, prev = infinity;

            int[] Deg = new int[VertexNum];
            foreach (Edge e in OstovKruskal)
            {
                Deg[e.v1]++;
                Deg[e.v2]++;
            }

            List<int> Leaves = new List<int>();
            for (int i = 0; i < VertexNum - 1; i++)
            {
                Leaves.Clear();
                for (int j = 0; j < VertexNum; j++)
                {
                    if (Deg[j] == 1)
                        Leaves.Add(j);
                }
                min = infinity;
                foreach (int leaf in Leaves)
                {
                    if (leaf < min)
                        min = leaf;
                }
                Deg[min]--;
                for (int j = 0; j < VertexNum; j++)
                {
                    if (Deg[j] != 0 && (OstovKruskal.Contains(new Edge(min, j))
                                        || OstovKruskal.Contains(new Edge(j, min))))
                    {
                        prev = j;
                        break;
                    }
                }
                Deg[prev]--;
                Code.Add(prev);
            }

            List<int> CodePrufer = new List<int>();
            CodePrufer.AddRange(Code);

            ////////////////////////////////////////////// Decode
            List<Edge> Ostov = new List<Edge>(); // множество ребер в остове
            bool flag = true;

            List<int> B = new List<int>();
            bool[] visited = new bool[VertexNum];

            while (Code.Count() > 0)
            {
                B.Clear();
                for (int i = 0; i < VertexNum; i++)
                {
                    flag = true;
                    for (int j = 0; j < Code.Count(); j++)
                    {
                        if (Code[j] == i)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag && !visited[i])
                    {
                        B.Add(i);
                    }
                }

                min = infinity;
                for (int i = 0; i < B.Count(); i++)
                {
                    if (B[i] < min)
                        min = B[i];
                }
                visited[min] = true;

                Ostov.Add(new Edge(Code[0], min, AdjMatrix[Code[0], min]));
                Code.RemoveAt(0);
            }
            B.Clear();

            return Tuple.Create(CodePrufer, Ostov);
        }




        #endregion Остовные деревья


        #region Потоки в сетях


        public int[,] FordFulkerson(int[,] MatrixC, int n)
        {
             //= BandwithMatrix;
             //= VertexNum;

            List<int> N = new List<int>(); // отметка узла
            List<int> S = new List<int>(); // признак принадлежности вершины мн-ву S
            List<int> Ps = new List<int>(); // "Знак", то есть направление пути
            List<int> Pn = new List<int>(); // предшествующая вершина в аугментальной цепи
            List<int> Pd = new List<int>(); // величина возможного увеличения потока

            int[,] F = new int[n, n]; // матрица максимальногопотока



            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    F[i, j] = 0; // вначале поток нулевой
                N.Add(0);
                S.Add(0);
                Pn.Add(0);
                Ps.Add(0);
                Pd.Add(0);
            }
            int x, de;
            int a = -1;

        M: // итерация увеличения потока
            for (int i = 0; i < n; i++) // инициализация
            {
                N[i] = 0;
                S[i] = 0;
                Pn[i] = 0; // nil
                Ps[i] = 0; // +
                Pd[i] = 0;
            }
            S[0] = 1; // так как источник в S
            Pd[0] = n * 100; // типабесконечность

            do
            {
                a = 0;
                for (int i = 0; i < n; i++)
                {
                    if ((S[i] != 0) && (N[i] == 0))
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (MatrixC[i, j] != 0)
                            {
                                if ((S[j] == 0) && (F[i, j] < MatrixC[i, j]))
                                {
                                    S[j] = 1;
                                    Ps[j] = 0;
                                    Pn[j] = i;
                                    if (Pd[i] < MatrixC[i, j] - F[i, j])
                                        Pd[j] = Pd[i];
                                    else
                                        Pd[j] = MatrixC[i, j] - F[i, j];
                                    a = 1;
                                }
                            }
                        }
                        for (int j = 0; j < n; j++)
                        {
                            if (MatrixC[j, i] != 0)
                            {
                                if ((S[j] == 0) && (F[j, i] > 0))
                                {
                                    S[j] = 1;
                                    Ps[j] = 1;
                                    Pn[j] = i;
                                    if (Pd[i] < F[j, i])
                                        Pd[j] = Pd[i];
                                    else
                                        Pd[j] = F[j, i];
                                    a = 1;
                                }
                            }
                        }
                        N[i] = 1;
                        if (S[n - 1] != 0)
                        {
                            x = n - 1;
                            de = Pd[n - 1];
                            while (x != 0)
                            {
                                if (Ps[x] == 0)
                                {
                                    F[Pn[x], x] = F[Pn[x], x] + de;
                                }
                                else
                                {
                                    F[x, Pn[x]] = F[x, Pn[x]] - de;
                                }
                                x = Pn[x];
                            }
                            goto M;
                        }
                    }
                }

            } while (a != 0);

            return F;
        }


        #endregion Потоки в сетях

        #region Фундаментальные циклы

        // Проверяет является ли граф эйлеровым
        // В случае, если на вход был дан ориентированный граф, то приводит к неориентированному.
        // Идея: каждая вершина имеет четную степень = граф эйлеров
        public bool IsEuler()
        {
            if (IsOriented)
                Disorientate();

            if (VertexNum == 2)
                return true;

            int deg;
            for (int i = 0; i < VertexNum; i++)
            {
                deg = 0;
                for (int j = 0; j < VertexNum; j++)
                {
                    if (AdjMatrix[i, j] != 0)
                        deg++;
                }

                if (deg % 2 != 0)
                    return false;
            }

            return true;
        } // End of "IsEuler" function


        // Достраивает матрицу смежности до принадлежащей эйлерову графу
        // Возвращает новую матрицу.
        // Идея: каждая вершина имеет четную степень = граф эйлеров
        public Tuple<int[,], List<Edge>, List<Edge>> CreateEulerianMatrix()
        {

            if (IsOriented)
                Disorientate();


            List<Edge> added = new List<Edge>();
            List<Edge> removed = new List<Edge>();

            int[,] Euler = new int[VertexNum, VertexNum];

            AdjMatrix.CopyMatrixTo(Euler, VertexNum, VertexNum);

            if (VertexNum == 2)
            {
                return Tuple.Create(Euler, added, removed);
            }

            List<int> OddVertexes = new List<int>(); // вершины с нечетными степенями

            for (int i = 0; i < VertexNum; i++)
            {
                int deg = 0;
                for (int j = 0; j < VertexNum; j++)
                {
                    if (AdjMatrix[i, j] != 0)
                        deg++;
                }
                if (deg % 2 != 0)
                {
                    OddVertexes.Add(i); // добавляем в список вершину с нечетной степенью
                }
            }

            while (OddVertexes.Count() > 0)
            {
                for (int i = 1; i < OddVertexes.Count(); i++)
                {
                    if (Euler[OddVertexes[0], OddVertexes[i]] != 0)
                    {
                        removed.Add(new Edge(OddVertexes[0], OddVertexes[i], Euler[OddVertexes[0], OddVertexes[i]]));
                        Euler[OddVertexes[0], OddVertexes[i]] = 0;
                        Euler[OddVertexes[i], OddVertexes[0]] = 0;                       
                        OddVertexes.RemoveAt(i);
                        OddVertexes.RemoveAt(0);
                    }
                    else
                    {
                        Euler[OddVertexes[0], OddVertexes[i]] = 1;
                        Euler[OddVertexes[i], OddVertexes[0]] = 1;
                        added.Add(new Edge(OddVertexes[0], OddVertexes[i], 1));
                        OddVertexes.RemoveAt(i);
                        OddVertexes.RemoveAt(0);
                    }
                }
            }

            return Tuple.Create(Euler, added, removed);
        } // End of "ToEuler" function


        // Перед использованием Если граф эйлеров, то предавать матрицу смежности, 
        // иначе передавать матрицу, полученную в результате работы функции "CreateEulerianMatrix"
        // Возвращает найденный в графе элеров цикл (содержит все ребра графа)
        public List<int> FindEulerCycle(int[,] Euler)
        {
            if (Euler == null)
                throw new Exception("Сначала проверьте является ли граф эйлеровым!");

            List<int> S = new List<int> { 0 }, EulerCycle = new List<int>();

            while (S.Count() != 0)
            {
                int v = S[S.Count() - 1];
                bool flag = true;
                for (int i = 0; i < VertexNum; i++)
                {
                    if (Euler[i, v] != 0) // Если с данной вершиной есть есть смежные
                        flag = false;
                }
                if (flag)
                {
                    S.RemoveAt(S.Count() - 1);
                    EulerCycle.Add(v);
                }
                else
                {
                    for (int i = 0; i < VertexNum; i++)
                    {
                        if (Euler[i, v] != 0)
                        {
                            S.Add(i);
                            Euler[i, v] = 0;
                            Euler[v, i] = 0;
                            break;
                        }
                    }
                }
            }
            return EulerCycle;
        } // End of "FindEulerCycle" function



        // Проверяет является ли граф гамильтоновым
        // Идея: 
        public bool IsHamiltonian()
        {

            if (IsOriented)
                Disorientate();

            if (VertexNum == 2)
                return true;

            List<bool> used = new List<bool>();

            for (int i = 0; i < VertexNum; i++)
                used.Add(false);

            if (!(Dfs(0, used, AdjMatrix, VertexNum, -1)))
            {
                int mi = 999;
                for (int i = 0; i < VertexNum; i++)
                {
                    int temp = 0;
                    for (int j = 0; j < VertexNum; j++)
                    {
                        temp += (AdjMatrix[i, j] != 0 ? 1 : 0);
                    }
                    if (temp < mi)
                        mi = temp;
                }
                if (mi >= (VertexNum / 2))
                    return true;
                else
                    return false;
            }
            else
                return false;
        } // End of "IsHamiltonian" function


        // Достраивает матрицу смежности до принадлежащей гамильтонову графу
        // Возвращает новую матрицу.
        // Идея: 
        public Tuple<int[,], List<Edge>> CreateHamiltonianMatrix()
        {
            if (IsOriented)
                Disorientate();

            List<Edge> added = new List<Edge>();

            int[,] Hamiltonian = new int[VertexNum, VertexNum];

            AdjMatrix.CopyMatrixTo(Hamiltonian, VertexNum, VertexNum);

            for (int i = VertexNum - 1; i > -1; i--)
            {
                int delta = 0;
                while (delta < (VertexNum / 2))
                {
                    for (int j = 0; j < VertexNum; j++)
                        delta += (Hamiltonian[i, j] != 0 ? 1 : 0);
                    for (int j = VertexNum - 1; j > -1; j--)
                    {
                        if (i != j)
                        {
                            if (Hamiltonian[i, j] == 0)
                            {
                                Hamiltonian[i, j] = 1;
                                Hamiltonian[j, i] = 1;
                                added.Add(new Edge(i, j, 1));
                                break;
                            }
                        }
                    }
                }
            }

            return Tuple.Create(Hamiltonian, added);
        } // End of "ToHamiltonian" function


        private bool Dfs(int x, List<bool> used, int[,] tabl, int n, int p)
        {
            used[x] = true;
            for (int i = 0; i < n; i++)
            {
                if (tabl[x, i] != 0)
                {
                    if (!used[i])
                    {
                        bool d = Dfs(i, used, tabl, n, x);
                        used[i] = true;
                        if (!d)
                            return false;
                    }
                    else
                        if (i != p)
                        return false;
                }
            }
            return true;
        }


        public List<int> KommivoyagerTask( )
        {
            if (HamiltonianMatrix == null)
                throw new Exception("Сначала проверьте является лиграф гамильтоновым!");

            string pathLog = @"E:\Polytech\TGraph\Теория графов CSharp\Graph_V2\RESULTS\result.txt";
            int mi = 999;


            List<int[]> transpositions = GenerateTranspositions(VertexNum);

            int countHamiltonCycles = 0;
            int[] amin = null;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathLog, false))
            {
                foreach (int[] a in transpositions)
                //for (int ii = 0; ii < G.VertexNum; ii++)
                {
                    // Если найденная последовательность вешин является гамильтоновым циклом
                    if (CheckHamiltonCycle(HamiltonianMatrix, a.ToList(), VertexNum))
                    {
                        countHamiltonCycles++;
                        // Считаем вес цикла
                        int temp = 0;
                        for (int i = 0; i < a.Count() - 1; i++)
                            temp += HamiltonianMatrix[a[i], a[i + 1]];
                        temp += HamiltonianMatrix[a[a.Count() - 1], a[0]];
                        // Если найденный вес меньше текущеего минимального
                        if (temp < mi)
                        {
                            mi = temp;
                            amin = a;
                        }

                        // Вывод цикла
                        StringBuilder sb = new StringBuilder();
                        string str = string.Empty;
                        for (int z = 0; z < a.Count(); z++)
                        {
                            str = String.Format("  {0,2}", a[z]);
                            sb.Append(str);
                        }
                        str = String.Format("  Стоимость: {0}", temp);
                        sb.Append(str);
                        file.WriteLine(sb.ToString());
                    }


                }
            }
            return amin.ToList();
        }


        public List<int[]> GenerateTranspositions(int n)
        {
            int[] array = new int[n];

            for (int i = 0; i < n; i++)
                array[i] = i;

            List<int[]> transpositions = new List<int[]>();
            GenerateTranspositionsHelper(array, 0, transpositions);

            return transpositions;

        }

        private void GenerateTranspositionsHelper(int[] array, int lf, List<int[]> transpositions)
        {
            if (lf >= array.Count())
            {                           // перестановки окончены
                //print(ar, dist(ar, dists));                 // выводим перестановку
                int[] b = new int[array.Count()];
                array.CopyTo(b, 0);
                transpositions.Add(b);
                return;
            }

            GenerateTranspositionsHelper(array, lf + 1, transpositions);   // перестановки элементов справа от lf
            for (int i = lf + 1; i < array.Count(); i++)
            {
                // теперь каждый элемент ar[i], i > lf
                int tmp = array[i];
                array[i] = array[lf];
                array[lf] = tmp;

                GenerateTranspositionsHelper(array, lf + 1, transpositions); // и снова переставляем всё справа                       
                // возвращаем элемент ar[i] назад
                tmp = array[i];
                array[i] = array[lf];
                array[lf] = tmp;
            }
        }


        // Проверяет является ли найденная последовательность вершин гамильтоновым циклом
        private bool CheckHamiltonCycle(int[,] Hamiltonian, List<int> a, int n)
        {
            for (int i = 0; i < a.Count() - 1; i++)
            {
                if (Hamiltonian[a[i], a[i + 1]] == 0)
                    return false;
            }
            if (Hamiltonian[a[a.Count() - 1], a[0]] == 0)
                return false;
            return true;
        }

        #endregion Фундаментальные циклы

    } // End of "Graph" class 
} // End of namespace


