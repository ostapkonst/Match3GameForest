using System.Collections.Generic;
using System.Linq;
using Match3GameForest.Entities;

namespace Match3GameForest.Entities
{
    public class FieldSeries : IEnumerable<IEnemy>
    {
        public IEnumerator<IEnemy> GetEnumerator()
        {
            return Line.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Line.GetEnumerator();
        }

        public bool IsEmpty => Line.Count() == 0;
        public int Score => Line.Sum(x => x.Prize);

        public IList<IEnemy> Line { get; private set; }
        public IList<IList<IEnemy>> Series { get; private set; }

        public FieldSeries()
        {
            Line = new List<IEnemy>();
            Series = new List<IList<IEnemy>>();
            return;
        }

        public FieldSeries(IList<IList<IEnemy>> data)
        {
            var set = new HashSet<IEnemy>();

            foreach (var dt in data) {
                foreach (var enemy in dt) {
                    set.Add(enemy);
                }
            }

            Line = set.ToList();
            Series = data;
        }

        public FieldSeries(IList<IEnemy> data)
        {
            var set = new HashSet<IEnemy>();

            foreach (var dt in data) {
                set.Add(dt);
            }

            Line = set.ToList();
            Series = new List<IList<IEnemy>>() { data };
        }
    }
}
