using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Graph_V2
{
    static class Matrix
    {
        public static int Det(this int [,] M, int n)
        {
            int d = 0;

            int k = 1;
            if (n == 1)
            {
                d = M[0, 0];
                return d;
            }
            else if (n == 2)
            {
                d = M[0, 0] * M[1, 1] - M[0, 1] * M[1, 0];
                return d;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    int[,] Minor = M.GetMinor(n, n, 0, i);
                    d += k * M[0, i] * Minor.Det(n - 1);
                    k = -k;
                }
            }
            return d;
        }

        public static int[,] GetMinor(this int[,] M, int n, int m, int i, int j)
        {
            if (i >= n || j >= m)
                return null;

            int[,] Minor = new int[n - 1, m - 1];

            int mi = 0, mj = 0;
            for (int ii = 0; ii < n; ii++ )
            {
                if (ii != i)
                {
                    mj = 0;
                    for (int jj = 0; jj < m; jj++)
                    {
                        if (jj != j)
                        {
                            Minor[mi, mj] = M[ii, jj];
                            mj++;
                        }
                    }
                    mi++;
                }   
            }

            return Minor;
        }

        public static List<string> GetMatrixStrings(this int[,] M, int n, int m)
        {
           // if (M == null)
             //   throw new NullReferenceException();

            List<string> MatrixStrings = new List<string>();

            StringBuilder sb = new StringBuilder();
            StringBuilder sb_ = new StringBuilder();

            sb.Append(String.Format(" V |"));
            for (int i = 0; i < m; i++)
            {
                sb.Append(String.Format("_{0, 2}", i));
            }
            sb.Append("_");
            MatrixStrings.Add(sb.ToString());
            sb.Clear();

            for (int i = 0; i < n; i++)
            {
                string str = String.Format("{0:d2} |", i);
                sb.Append(str);
                for (int j = 0; j < m; j++)
                {
                    str = String.Format("  {0,2}", M[i, j]);
                    sb.Append(str);
                }
                MatrixStrings.Add(sb.ToString());
                sb.Clear();
            }

            return MatrixStrings;
        }



        public static List<string> GetMatrixStrings(this int?[,] M, int n, int m)
        {
            List<string> MatrixStrings = new List<string>();

            StringBuilder sb = new StringBuilder();
            StringBuilder sb_ = new StringBuilder();

            sb.Append(String.Format(" V |"));
            for (int i = 0; i < m; i++)
            {
                sb.Append(String.Format("_{0, 2}", i));
            }
            sb.Append("_");
            MatrixStrings.Add(sb.ToString());
            sb.Clear();

            for (int i = 0; i < n; i++)
            {
                string str = String.Format("{0:d2} |", i);
                sb.Append(str);
                for (int j = 0; j < m; j++)
                {
                    str = ( M[i,j] == null ? "  --" :String.Format("  {0,2}", M[i, j]) );
                    sb.Append(str);
                }
                MatrixStrings.Add(sb.ToString());
                sb.Clear();
            }

            return MatrixStrings;
        }
        

        public static void CopyMatrixTo(this int[,] Src, int[,] Dst, int n, int m)
        {
            if (Dst == null )
                throw new ArgumentNullException();

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    Dst[i, j] = Src[i, j];
        }

        public static void CopyMatrixTo(this int?[,] Src, int?[,] Dst, int n, int m)
        {
            if (Dst == null /*|| Src == null*/)
                throw new ArgumentNullException();

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    Dst[i, j] = Src[i, j];
        }

        public static int[,] Substract(this int[,] A, int[,] B, int n, int m)
        {
            int[,] Res = new int[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Res[i, j] = A[i, j] - B[i, j];
                }
            }

            return Res;
        }

       
    }
}
