using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Graph_V2
{
    
    public partial class Form1 : Form
    {
        bool CheckBoxShimbellMin = false;

        List<string> ResultsBox = new List<string>();

        Graph G = null;

        public Form1()
        {
            InitializeComponent();
        }


        private void checkBoxShimbellMin_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxShimbellMin = !CheckBoxShimbellMin;
        }

        private void ButtonOrientation_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    OutputResults("Сначала создайте граф!");
                else
                {
                    if (G.IsOriented)
                    {
                        G.Disorientate();
                        OutputResults("Теперь граф неориентированный. См. соседнее окно.");
                    }
                    else
                    {
                        G.Orientate();
                        OutputResults("Теперь граф ориентированный. См. соседнее окно.");
                    }
                    OutputGraph(G);
                }
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }

        private void ButtonNegativeWeight_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    OutputResults("Сначала создайте граф!");
                else
                {
                    if (G.HasNegativeWeight)
                    {
                        G.SetPositiveWeight();
                        OutputResults("Отрицательные веса убраны. См. соседнее окно.");
                    }
                    else
                    {
                        G.SetNegativeWeight();
                        OutputResults("Отрицательные веса добавлены. См. соседнее окно.");
                    }
                    OutputGraph(G);
                }
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }

        }

        private void OutputGraph(Graph G)
        {
            if (G == null)
            {
                return;
            }

            List<string> GraphStrings = new List<string>
            {
                String.Format((G.IsOriented ? "Ориентированный Граф. К-во дуг" : "Неориентированный Граф. К-во ребер: ") +
                                                                        " : {0}", G.EdgesNum),
                "",
                String.Format((G.IsOriented ? "Дуги: (v1, v2) вес" : "Рёбра: (v1, v2) вес")),
                ""
            };

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < G.Edges.Count; i++)
            {
                sb.Append(String.Format("({0}, {1}) {2}; ", G.Edges[i].v1, G.Edges[i].v2, G.Edges[i].weight));
                if (i != 0 && i % 5 == 0)
                {
                    GraphStrings.Add(sb.ToString());
                    sb.Clear();
                }
            }
            if (sb.Length > 0)
                GraphStrings.Add(sb.ToString());

            GraphStrings.Add("");
            GraphStrings.Add("Матрица смежности(стоимости):");
            GraphStrings.AddRange(G.AdjMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));

            if (G.BandwithMatrix != null)
            {
                GraphStrings.Add("");
                GraphStrings.Add("Матрица пропускных способностей:");
                GraphStrings.AddRange(G.BandwithMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));
            }

            if (G.EulerianMatrix != null)
            {
                GraphStrings.Add("");
                GraphStrings.Add("Эйлеров вариант графа:");
                GraphStrings.AddRange(G.EulerianMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));
            }

            if (G.HamiltonianMatrix != null)
            {
                GraphStrings.Add("");
                GraphStrings.Add("Гамильтонов вариант графа:");
                GraphStrings.AddRange(G.HamiltonianMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));
            }


            TextBoxGraph.Text = String.Empty;
            foreach (string line in GraphStrings)
            {
                TextBoxGraph.Text += line + Environment.NewLine;
            }
            TextBoxGraph.SelectionStart = TextBoxGraph.TextLength;
            TextBoxGraph.ScrollToCaret();
        }

        private void OutputResults(List<string> Results)
        {
            foreach (string line in Results)
            {
                TextBoxResults.Text += line + Environment.NewLine;
            }
            TextBoxResults.SelectionStart = TextBoxResults.TextLength;
            TextBoxResults.ScrollToCaret();
        }

        private void OutputResults(string Results)
        {
            TextBoxResults.Text += Results + Environment.NewLine;
            TextBoxResults.SelectionStart = TextBoxResults.TextLength;
            TextBoxResults.ScrollToCaret();
        }

        private void ButtonGraphCreate_Click(object sender, EventArgs e)
        {
            G = null;

            try
            {
                string number = TextBoxVertexNum.Text;
                int numberInt = Convert.ToInt32(number);
                if (numberInt > 15 || numberInt < 1)
                {
                    throw new Exception("Кол-во вершин должно быть от 1 до 15!");
                }

                G = new Graph(numberInt);
                if (G.AdjMatrix != null)
                {
                    OutputResults("Граф Создан! См. соседнее окно.");
                    OutputResults(G.AdjMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));
                    OutputGraph(G);
                }
                else
                {
                    G = null;
                    OutputResults("Не удалось создать граф");
                }
            }
            catch (Exception ex)
            {
                if ((ex as FormatException) != null)
                    OutputResults("Не корректный ввод ЧИСЛА вершин!");
                OutputResults(ex.Message);
                G = null;
                OutputGraph(G);
            }
        }


        #region Лабораторная 1. Метод Шимбелла

        private void ButtonShimbellDo_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                int numberInt = Convert.ToInt32(TextBoxShimbell.Text);

                int[,] shimbell = G.Shimbell(CheckBoxShimbellMin, numberInt);

                OutputResults(String.Format("М. Шимбелла для {0} ребер", numberInt));
                OutputResults(String.Format(CheckBoxShimbellMin ? "Минимальные пути" : "Максимальные пути"));              
                OutputResults(shimbell.GetMatrixStrings(G.VertexNum, G.VertexNum));

                OutputGraph(G);
            }
            catch (Exception ex)
            {
                if (ex as FormatException != null)
                    OutputResults("Неправильный ввод ЧИСЛА ребер в Методе Шимбелла.");
                else
                    OutputResults(ex.Message);
                OutputGraph(G);
            }
        }

        private void ButtonShimbellPath_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                int v1 = Convert.ToInt32(TextBoxShimbellV1.Text);
                int v2 = Convert.ToInt32(TextBoxShimbellV2.Text);
                int n = G.PathGaImasuKa(v1, v2);
                OutputResults(String.Format(n == 0 ? "Пути не существует" : String.Format("Количество ребер в пути: {0}", n)));
            }
            catch (Exception ex)
            {
                if (ex as FormatException != null)
                    OutputResults("Неправильные номера вершин для пути.");
                else
                    OutputResults(ex.Message);
            }
        }

        #endregion


        // Дописать здесь вывод самих путей
        #region Лабораторная 2. Дейкстра, Беллман-Форд, Флойд-Уоршелл 

        private void ButtonDijkstra_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                int v = Convert.ToInt32(TextBoxExtrPathV.Text);

                int[] dijkstra = G.Dijkstra(v);

                OutputResults("Алгоритм Дейкстры:");
                OutputResults("Вектор длин кратчайших путей:");

                StringBuilder sb = new StringBuilder();
                sb.Append(String.Format(" V |"));
                for (int i = 0; i < G.VertexNum; i++)
                {
                    sb.Append(String.Format("_{0, 2}", i));
                }
                sb.Append("_");
                OutputResults(sb.ToString());
                sb.Clear();

                sb.Append("      ");
                for (int i = 0; i < G.VertexNum; i++)
                {
                    string str = (dijkstra[i] == System.Int32.MaxValue ? "   --" : String.Format("  {0,2}", dijkstra[i]));
                    sb.Append(str);
                }
                OutputResults(sb.ToString());
                sb.Clear();

                OutputResults(String.Format("Количество итераций: {0}", G.DijkstraIterations));
            }
            catch (Exception ex)
            {
                if (ex as FormatException != null)
                    OutputResults("Неправильный ввод НОМЕРА вершины.");
                else
                    OutputResults(ex.Message);
            }
        }

        private void ButtonBellmanFord_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                int v = Convert.ToInt32(TextBoxExtrPathV.Text);
                int?[] BellmanFord = G.BellmanFord(v); 

                OutputResults("Алгоритм Беллмана-Форда:");
                OutputResults("Вектор длин кратчайших путей:");

                StringBuilder sb = new StringBuilder();
                sb.Append(String.Format(" V |"));
                for (int i = 0; i < G.VertexNum; i++)
                {
                    sb.Append(String.Format("_{0, 2}", i));
                }
                sb.Append("_");
                OutputResults(sb.ToString());
                sb.Clear();
                sb.Append("    ");
                for (int i = 0; i < G.VertexNum; i++)
                {
                    string str = (BellmanFord[i] == null ? "  -- " : String.Format("  {0,2}", BellmanFord[i]));
                    sb.Append(str);
                }
                OutputResults(sb.ToString());
                sb.Clear();

                OutputResults(String.Format("Количество итераций: {0}", G.BellmanFordIterations));

            }
            catch (Exception ex)
            {
                if (ex as FormatException != null)
                    OutputResults("Неправильный ввод НОМЕРА вершины.");
                else
                    OutputResults(ex.Message);
            }
        }

        private void ButtonFloyd_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                int v = 0;
                if (TextBoxExtrPathV.Text != String.Empty)
                    v = Convert.ToInt32(TextBoxExtrPathV.Text);

                int?[,] FloydWarshall = G.FloydWarshall(v);

                OutputResults("Алгоритм Флойда-Уоршелла:");
                OutputResults("Матрица длин кратчайших путей:");
                OutputResults(FloydWarshall.GetMatrixStrings(G.VertexNum, G.VertexNum));
                OutputResults(String.Format("Количество итераций: {0}", G.FloydWarshallIterations));
                OutputResults(String.Format("Количество итераций для вершины {0}: {1}",  v, G.FloydWarshallIterationsForOneVertex));
            }
            catch (Exception ex)
            {
                if (ex as FormatException != null)
                    OutputResults("Неправильный ввод НОМЕРА вершины.");
                else
                    OutputResults(ex.Message);
            }
        }

        private void ButtonPathMatrix_Click(object sender, EventArgs e)
        {

            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                int v = Convert.ToInt32(TextBoxExtrPathV.Text);

                OutputResults(String.Format("кратчайшие пути от вершины {0}", v));

                for (int i = 0; i < G.VertexNum; i++)
                {
                    if (i != v)
                    {
                        int[] len = new int[1];
                        List<int> path = G.BellmanFordPath(G.AdjMatrix, G.VertexNum, v, i, 0, len);
                        StringBuilder sb = new StringBuilder();

                        if (path.Count() != 0)
                        {
                            sb.Append(String.Format("{0} : ", i));
                            for (int j = 0; j < path.Count - 1; j++)
                                sb.Append(String.Format(" {0} ", path[j]));
                            sb.Append(String.Format("Длина кратчайшего пути = {0}", len[0]));
                            OutputResults(sb.ToString());

                            sb.Clear();
                        }
                        else
                        {
                            OutputResults(String.Format("Между вершинами {0} и {1} пути нет", v, i));
                        }
                    }
                }

                /*

                
                int[] T = new int[G.VertexNum], H = new int[G.VertexNum];

                List<List<int>> AM = new List<List<int>>();
                
                for (int i = 0; i < G.VertexNum; i++)
                {
                    List<int> tmp = new List<int>();
                    for (int j = 0; j < G.VertexNum; j++)
                        tmp.Add(G.AdjMatrix[i, j]);
                    AM.Add(tmp);
                }
                StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < G.VertexNum; i++)
                {
                    int i = 5;
                    if(G.Deikstra(AM, T.ToList(), H.ToList(), G.VertexNum, v, i))
                    {
                        List<int> Y = new List<int>();
                        int w = i;
                        Y.Add(w);
                        while (w != v)
                        {
                            w = H[w];
                            Y.Add(w);
                        }

                        sb.Append(String.Format("{0} : ", i));
                        for (int j = Y.Count() - 1; j >= 0; j--)
                            sb.Append(String.Format(" {0} ", Y[i]));
                        sb.Append(String.Format("Длина кратчайшего пути = {0}", T[i]));
                        OutputResults(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        OutputResults(String.Format("Между вершинами {0} и {1} пути нет", v, i));
                    }
                }
                */

                OutputResults("");
                OutputGraph(G);
            }
            catch (Exception ex)
            {
                if (ex as FormatException != null)
                    OutputResults("Неправильный ввод НОМЕРА вершины.");
                else
                    OutputResults(ex.Message);
                OutputResults("");
                OutputGraph(G);
            }

        }


        #endregion Лабораторная 2. Дейкстра, Беллман-Форд, Флойд-Уоршелл 


        #region Лабораторная 3. Прима, Краскала, Остовы, Прюфер

        private void ButtonPrima_Click(object sender, EventArgs e)
        {
            try
            {
                var result = G.Prima();
                List<Edge> Ostov = result.Item1;
                int minCost = result.Item2;


                OutputResults("Алгоритм Прима:");
                OutputResults(String.Format("Количество итераций: {0}", G.PrimaIterations));
                OutputResults(String.Format("Минимальная стоимость: {0}", minCost));
                OutputResults("Минимальный остов:");

                StringBuilder sb = new StringBuilder();
                foreach (Edge edge in Ostov)
                {
                    sb.Append(String.Format(" ({0}, {1}) - {2} ", edge.v1, edge.v2, edge.weight));
                }
                OutputResults(sb.ToString());
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }

        private void ButtonKruskal_Click(object sender, EventArgs e)
        {
            try
            {
                var result = G.Kruskal();
                List<Edge> Ostov = result.Item1;
                int minCost = result.Item2;

                OutputResults("Алгоритм Краскала:");
                OutputResults(String.Format("Количество итераций: {0}", G.KruskalIterations));
                OutputResults(String.Format("Минимальная стоимость: {0}", minCost));
                OutputResults("Минимальный остов:");

                StringBuilder sb = new StringBuilder();
                foreach (Edge edge in Ostov)
                {
                    sb.Append(String.Format(" ({0}, {1}) - {2} ", edge.v1, edge.v2, edge.weight));
                }
                OutputResults(sb.ToString());
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }

        private void ButtonKirchhoff_Click(object sender, EventArgs e)
        {
            try
            {
                var results = G.Kirchhoff();
                int[,] KirchhoffMatrix = results.Item1;
                int det = results.Item2;
                OutputResults("Матрица Кирхгофа:");
                OutputResults(KirchhoffMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));
                OutputResults(String.Format("Количество остовных деревьев по матричной теореме Кирхгофа {0}", det));

            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }

        private void ButtonPrufer_Click(object sender, EventArgs e)
        {
            try
            {
                var result = G.Prufer();
                List<int> Code = result.Item1;
                List<Edge> Ostov = result.Item2;

                OutputResults("Код Прюфера:");
                StringBuilder sb = new StringBuilder();
                foreach (int i in Code)
                {
                    sb.Append(i.ToString() + " ");
                }

                OutputResults(sb.ToString());
                sb.Clear();

                OutputResults("Декодирование:");
                foreach (Edge edge in Ostov)
                {
                    sb.Append(String.Format("({0}, {1}) - {2} ", edge.v1, edge.v2, edge.weight));
                }
                OutputResults(sb.ToString());

            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }


        #endregion Лабораторная 3. Прима, Краскала, Остовы, Прюфер


        #region Лабораторная 4. Потоки в сетях, Форд-Фалкерсон

        private void ButtonNetwork_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                if (G.IsNetwork)
                {
                    OutputResults("Граф является сетью.");
                }
                else
                {
                    OutputResults("Так как граф не является сетью, то был достроен.");
                    List<Edge> added = G.ToNetwork();

                    if (added.Count() != 0)
                    {
                        OutputResults("Добавленные ребра:");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < added.Count; i++)
                        {
                            sb.Append(String.Format("({0}, {1}) {2}; ", added[i].v1, added[i].v2, added[i].weight));
                            if (i != 0 && i % 5 == 0)
                            {
                                OutputResults(sb.ToString());
                                sb.Clear();
                            }
                        }
                        if (sb.Length > 0)
                            OutputResults(sb.ToString());
                    }
                }

                if (G.BandwithMatrix == null)
                {
                    G.FillBandwithMatrix();
                    OutputResults("Создана матрица пропускных способностей:");
                    OutputResults(G.BandwithMatrix.GetMatrixStrings(G.VertexNum, G.VertexNum));
                }

                OutputResults("");
                OutputGraph(G);
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
                OutputResults("");
                OutputGraph(G);
            }
        }


        private void ButtonFulkerson_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                if (!G.IsNetwork)
                    throw new Exception("Сначала преобразуйте граф в сеть!");

                if (G.BandwithMatrix == null)
                    throw new Exception("Нужнаматрица пропускных способностей: нажмите \"Сделать сеть\"");

                int[,] MatrixMaxCost = G.FordFulkerson(G.BandwithMatrix, G.VertexNum);
                OutputResults("Матрица максимального потока:");
                OutputResults(MatrixMaxCost.GetMatrixStrings(G.VertexNum, G.VertexNum));

                int maxflow = 0;
                for (int i = 0; i < G.VertexNum; i++)
                    maxflow += MatrixMaxCost[i, G.VertexNum - 1];
                OutputResults(String.Format("Максимальный поток = {0}", maxflow));

                G.maxFlow = maxflow;

                OutputResults("");
                OutputGraph(G);
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
                OutputResults("");
                OutputGraph(G);
            }
        }

        private void ButtonMinCost_Click(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                if (!G.IsNetwork)
                    throw new Exception("Сначала преобразуйте граф в сеть!");

                if (G.BandwithMatrix == null)
                    throw new Exception("Нужнаматрица пропускных способностей: нажмите \"Сделать сеть\"");

                if (G.maxFlow == 0)
                    throw new Exception("Сначала посчитайте максисмальный поток!");

                var result = G.FlowMinCost();

                int[,] MatrixGivenCost = result.Item1;
                int minCost = result.Item2;

                OutputResults("Матрица заданного потока:");
                OutputResults(MatrixGivenCost.GetMatrixStrings(G.VertexNum, G.VertexNum));
                OutputResults(String.Format("Заданный поток {0} = 2/3 от максимального", 2 * G.maxFlow / 3));
                OutputResults(String.Format("Минимальная стоимость этого потока  = {0}", minCost));

                OutputResults("");
                OutputGraph(G);

            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
                OutputResults("");
                OutputGraph(G);
            }


        }

        #endregion Лабораторная 4. Потоки в сетях, Форд-Фалкерсон


        #region Лабораторная 5. Эйлероф и Гамильтонов граф. Задача комивояжера.


        private void ButtonIsEuler_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                if (!G.IsEuler())
                {
                    OutputResults("Граф не является эйлеровым!");
                    var result = G.CreateEulerianMatrix();
                    G.EulerianMatrix = result.Item1;
                    List<Edge> added = result.Item2;
                    List<Edge> removed = result.Item3;
                    OutputResults("Достроенный до эйлерова вариант графа см. в соседнем окне!");
                    if (added.Count() != 0)
                    {
                        OutputResults("Добавленные ребра:");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < added.Count; i++)
                        {
                            sb.Append(String.Format("({0}, {1}) {2}; ", added[i].v1, added[i].v2, added[i].weight));
                            if (i != 0 && i % 5 == 0)
                            {
                                OutputResults(sb.ToString());
                                sb.Clear();
                            }
                        }
                        if (sb.Length > 0)
                            OutputResults(sb.ToString());
                    }
                    if (removed.Count() != 0)
                    {
                        OutputResults("Удаленные ребра:");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < removed.Count; i++)
                        {
                            sb.Append(String.Format("({0}, {1}) {2}; ", removed[i].v1, removed[i].v2, removed[i].weight));
                            if (i != 0 && i % 5 == 0)
                            {
                                OutputResults(sb.ToString());
                                sb.Clear();
                            }
                        }
                        if (sb.Length > 0)
                            OutputResults(sb.ToString());
                    }
                }
                else
                {
                    OutputResults("Граф эйлеров!");
                    G.EulerianMatrix = new int[G.VertexNum, G.VertexNum];
                    G.AdjMatrix.CopyMatrixTo(G.EulerianMatrix, G.VertexNum, G.VertexNum);
                    OutputResults("эйлеров вариант графа - сам граф!");
                }
                OutputResults("");
                OutputGraph(G);
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
                OutputResults("");
            }
        }

        private void ButtonHamilton_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                if (!G.IsHamiltonian())
                {
                    OutputResults("Граф не является гамельтоновым!");
                    var result = G.CreateHamiltonianMatrix();
                    G.HamiltonianMatrix = result.Item1;
                    List<Edge> added = result.Item2;
                    OutputResults("Достроенный до гамельтонова вариант графа см. в соседнем окне!");

                    if (added.Count() != 0)
                    {
                        OutputResults("Добавленные ребра:");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < added.Count; i++)
                        {
                            sb.Append(String.Format("({0}, {1}) {2}; ", added[i].v1, added[i].v2, added[i].weight));
                            if (i != 0 && i % 5 == 0)
                            {
                                OutputResults(sb.ToString());
                                sb.Clear();
                            }
                        }
                        if (sb.Length > 0)
                            OutputResults(sb.ToString());
                    }
                }
                else
                {
                    OutputResults("Граф гамильтонов!");
                    G.HamiltonianMatrix = new int[G.VertexNum, G.VertexNum];
                    G.AdjMatrix.CopyMatrixTo(G.HamiltonianMatrix, G.VertexNum, G.VertexNum);
                    OutputResults("Гамильтонов вариант графа - сам граф!");
                }
                OutputResults("");
                OutputGraph(G);
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
                OutputResults("");
            }
        }

        private void ButtonEulerCycle_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (G == null)
                    throw new Exception("Сначала создайте граф!");

                
                if (G.EulerianMatrix == null)
                {
                    if (!G.IsEuler())
                    {
                        OutputResults("Так как раф не является эйлеровым, он был достроен до эйлерова");
                        var result = G.CreateEulerianMatrix();
                        G.EulerianMatrix = result.Item1;
                        List<Edge> added = result.Item2;
                        List<Edge> removed = result.Item3;
                        OutputResults("Достроенный до эйлерова вариант графа см. в соседнем окне!");
                        if (added.Count() != 0)
                        {
                            OutputResults("Добавленные ребра:");
                            StringBuilder sb1 = new StringBuilder();
                            for (int i = 0; i < added.Count; i++)
                            {
                                sb1.Append(String.Format("({0}, {1}) {2}; ", added[i].v1, added[i].v2, added[i].weight));
                                if (i != 0 && i % 5 == 0)
                                {
                                    OutputResults(sb1.ToString());
                                    sb1.Clear();
                                }
                            }
                            if (sb1.Length > 0)
                                OutputResults(sb1.ToString());
                        }
                        if (removed.Count() != 0)
                        {
                            OutputResults("Удаленные ребра:");
                            StringBuilder sb1 = new StringBuilder();
                            for (int i = 0; i < removed.Count; i++)
                            {
                                sb1.Append(String.Format("({0}, {1}) {2}; ", removed[i].v1, removed[i].v2, removed[i].weight));
                                if (i != 0 && i % 5 == 0)
                                {
                                    OutputResults(sb1.ToString());
                                    sb1.Clear();
                                }
                            }
                            if (sb1.Length > 0)
                                OutputResults(sb1.ToString());
                        }
                    }
                    else
                    {
                        OutputResults("Так как граф эйлеров, то его не нужно достраивать");
                        G.EulerianMatrix = new int[G.VertexNum, G.VertexNum];
                        G.AdjMatrix.CopyMatrixTo(G.EulerianMatrix, G.VertexNum, G.VertexNum);
                    }

                    OutputGraph(G);
                }

                List<int> EulerCycle = G.FindEulerCycle(G.EulerianMatrix);
                OutputResults("Эйлеров цикл:");
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < EulerCycle.Count(); i++)
                    sb.Append(String.Format(" {0} ", EulerCycle[i]));
                OutputResults(sb.ToString());

            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }

        private void ButtonTSP_Click_1(object sender, EventArgs e)
        {
            try
            {                
                if (G == null) // проверка, что граф был создан
                    throw new Exception("Сначала создайте граф!");

                // Проверка, что у нас есть матрица для работы
                if (G.HamiltonianMatrix == null)
                {
                    if (G.IsHamiltonian())
                    {
                        G.HamiltonianMatrix = new int[G.VertexNum, G.VertexNum];
                        G.AdjMatrix.CopyMatrixTo(G.HamiltonianMatrix, G.VertexNum, G.VertexNum);
                    }
                    else
                    {
                        var result = G.CreateHamiltonianMatrix();
                        G.HamiltonianMatrix = result.Item1;
                        List<Edge> added = result.Item2;
                        OutputResults("Так как граф не является гамельтоновым, то он был достроен.");
                        OutputResults("Достроенный до гамельтонова вариант графа см. в соседнем окне!");
                        StringBuilder sb1 = new StringBuilder();
                        if (added.Count() != 0)
                        {
                            OutputResults("Добавленные ребра:");
                            sb1.Clear();
                            for (int i = 0; i < added.Count; i++)
                            {
                                sb1.Append(String.Format("({0}, {1}) {2}; ", added[i].v1, added[i].v2, added[i].weight));
                                if (i != 0 && i % 5 == 0)
                                {
                                    OutputResults(sb1.ToString());
                                    sb1.Clear();
                                }
                            }
                            if (sb1.Length > 0)
                                OutputResults(sb1.ToString());
                        }
                    }
                }

                OutputResults("Если граф был ориентированным, то произошел переход к неориентированному");

                List<int> minPath = G.KommivoyagerTask();
                OutputResults("Гамильтонов цикл минимального веса:");
                StringBuilder sb = new StringBuilder();
                int weight = 0;
                for (int i = 0; i < minPath.Count(); i++)
                {
                    sb.Append(String.Format("  {0,2}", minPath[i]));

                    if (i <= (minPath.Count() - 2))
                        weight += G.HamiltonianMatrix[minPath[i], minPath[i + 1]];
                    else
                        weight += G.HamiltonianMatrix[minPath[i], minPath[0]];
                }
                //weight += G.HamiltonianMatrix[minPath[minPath.Count() - 1], minPath[0]];
                sb.Append(String.Format("  {0,2}", minPath[0]));
                sb.Append(String.Format("  Вес: {0}", weight));
                OutputResults(sb.ToString());
                OutputResults("Все остальные циклы выведены в файл RESULTS\\results.txt");

                // всегда обновляем окно с графом!
                OutputGraph(G);
            }
            catch (Exception ex)
            {
                OutputResults(ex.Message);
            }
        }


        #endregion Лабораторная 5. Эйлероф и Гамильтонов граф. Задача комивояжера.


        #region Я случайно!

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        #endregion Я случайно!

        
    }
}

