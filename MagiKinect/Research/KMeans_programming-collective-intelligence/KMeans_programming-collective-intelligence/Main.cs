/**
 * Quick comment block to explain the point of this file.
 * 
 * This is a simple implementation of the k-means clustering algorithm. Along with this algorithm, I will be doing
 * Sample implementation of other machine learning algorithms to find the best one. This will include: a Bayesian Classifier, a Decision Tree, 
 * and at least one k-Nearest neighbor algorithm. I'm trying to find the one for the job. I am familiar with k-means, so I will begin with
 * this one.
 * 
 * Keep in mind, this is research. So, I will not be concerned with method scoping, seperate objects in seperate files, or any of that.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeans_programming_collective_intelligence
{

    class Point
    {
        public int x = 0;
        public int y = 0;
    }

    // in essence, our cluster is our centroid
    class Cluster : Point
    {
        public List<Point> children = new List<Point>();
        public int id;
        public Cluster(int _x, int _y, int _id)
        {
            x = _x;
            y = _y;
            id = _id;
        }

        public void GetCentroid()
        {
            Console.WriteLine("x {0:N} y {1:N}", x, y);
        }
    }

    class KMeans
    {
        public static KMeans instance;
        Random rand;
       
        public void apply(List<Point> points, int k, int maxX, int minX, int maxY, int minY)
        {
            // init local variables
            List<Cluster> clusters = new List<Cluster>(k);
            List<Cluster> lastmatches = new List<Cluster>(k);

            // create k randomly placed centroids
            for (int i = 0; i < k; i++)
            {
                clusters.Add(
                    new Cluster(rand.Next(minX, maxX), rand.Next(minY, maxY), i)
                );
                clusters[i].GetCentroid();
            }

            for (int t = 0; t < 100; t++)
            {
                Console.WriteLine("Iteration {0:N}", t);
                var bestmatches = new List<Cluster>(k);

                // find the closest cluster to each point
                foreach(Point p in points)
                {
                    Cluster closestToP = this.GetBestMatch(clusters, p);
                    closestToP.children.Add(p);
                    bestmatches.Add(closestToP);
                }

                // break condition: if the closest points are the same
                // then we can assume we are done.
                if (this.ScrambledEquals(bestmatches, lastmatches))
                {
                    break;
                }

                lastmatches = bestmatches;

                // move step of kmeans. For each clustoid, move it's current location to the mean of
                // it's members locations, clean, and continue.
                for (int i = 0; i < k; i++)
                {
                    Cluster current = clusters[i];
                    double x_av = 0, y_av = 0;
                    foreach (Point child in current.children)
                    {
                        x_av += child.x;
                        y_av += child.y;
                    }
                    current.x = (int)Math.Round(x_av / current.children.Count);
                    current.y = (int)Math.Round(y_av / current.children.Count);
                    current.children = new List<Point>();
                }
            }

            int totalUsedClusters = clusters.Where(x => x.children.Count > 0).Select(x => x).Count();
            foreach (var q in clusters.Where(x => x.children.Count > 0).Select(x => x))
            {
                Console.WriteLine("Cluster {0:N} has {1:N} points", q.id, q.children.Count);
            }
            Console.WriteLine("Total clusters used {0:N}", totalUsedClusters);


            string[] lines = new string[clusters.Count], lines2 = new string[points.Count];
            StringBuilder s = new StringBuilder(), s2 = new StringBuilder();

            int j = 0;
            foreach (Cluster c in clusters)
            {
                lines[j] = (String.Format("({0:N}, {1:N})", c.x, c.y));
                j++;
            }
            for (int i = 0; i < points.Count; i++)
            {
                lines2[i] = (String.Format("({0:N}, {1:N}),", points[i].x, points[i].y));
            }
            System.IO.File.WriteAllLines(@"Points.txt", lines2);

            System.IO.File.WriteAllLines(@"Clusters.txt", lines);
        }


        public Cluster GetBestMatch(List<Cluster> clusters, Point point)
        {
            double min = Double.MaxValue;
            Cluster best = clusters[0];

            foreach (Cluster cluster in clusters)
            {
                // get distance between our clusters, and the point.
                double x = Math.Pow(cluster.x - point.x, 2);
                double y = Math.Pow(cluster.y - point.y, 2);
                double distance = Math.Sqrt(x + y);

                if (distance < min)
                {
                    best = cluster;
                    min = distance;
                }
            }

            return best;
        }

        public bool ScrambledEquals(List<Cluster> listOne, List<Cluster> listTwo)
        {
            Dictionary<Cluster, int> dic = new Dictionary<Cluster, int>();
            foreach (Cluster c in listOne)
            {
                if (dic.ContainsKey(c))
                {
                    dic[c]++;
                }
                else
                {
                    dic.Add(c, 1);
                }
            }

            foreach (Cluster c in listTwo)
            {
                if (dic.ContainsKey(c))
                {
                    dic[c]--;
                }
                else
                {
                    return false;
                }
            }

            return dic.Values.All(i => i == 0);
        }

        public static void Main()
        {
            // init member variables
            KMeans.instance = new KMeans();
            KMeans.instance.rand = new Random();

            // init local variables
            Random xRand = new Random(7);
            Random yRand = new Random(2);
            int maxX = 10, maxY = 10, minX = 0, minY = 0;

            // gather test data
            List<Point> rows = new List<Point>();
            int pcount = KMeans.instance.rand.Next(1, 50);
            for (int i = 0; i < pcount; i++)
            {
                Point np = new Point();
                np.x = xRand.Next(minX, maxX);
                np.y = yRand.Next(minY, maxY);
                rows.Add(np);
            }

            // apply algorithm
            KMeans.instance.apply(rows, 10, maxX, minX, maxY, minY);
            Console.Read();
        }
    }
}
