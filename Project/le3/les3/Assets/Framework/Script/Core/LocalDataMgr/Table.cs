using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Table : ITable
    {
        private Dictionary<int, IRecord> records = new Dictionary<int, IRecord>();
        public  int Count()
        {
           return records.Count;
        }

        public IRecord Query(int id)
        {
            IRecord record = null;
            if (records.TryGetValue(id, out record))
            {
                return record;
            }
            return null;
        }

        public T Query<T>(int id) where T : IRecord
        {
            return (T)Query(id);
        }

        public T QueryIndex<T>(int idx) where T : IRecord
        {
            if (idx < 0 || idx > records.Count)
            {
                return default(T);
            }

            return (T)records.ElementAt(idx).Value;
        }

        public void AddRecords<T>(T[] addRecords) where T: IRecord
        {
            //records.Add(records)
            foreach(var record  in addRecords)
            {
                records.Add(record.ID(), record);
            }
        }
    }
}
