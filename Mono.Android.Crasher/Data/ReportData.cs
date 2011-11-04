using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mono.Android.Crasher.Data
{
    public class ReportData : IDictionary<ReportField, string>
    {
        private readonly IDictionary<ReportField, string> _innerDictionary;

        public ReportData()
        {
            _innerDictionary = new Dictionary<ReportField, string>();
        }

        public IEnumerator<KeyValuePair<ReportField, string>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var item in this)
            {
                builder.AppendFormat("{0} = {1}", item.Key, item.Value).AppendLine();
            }
            return builder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<ReportField, string> item)
        {
            if (!_innerDictionary.Contains(item))
                _innerDictionary.Add(item);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<ReportField, string> item)
        {
            return _innerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<ReportField, string>[] array, int arrayIndex)
        {
            _innerDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<ReportField, string> item)
        {
            return _innerDictionary.Contains(item) && _innerDictionary.Remove(item);
        }

        public int Count
        {
            get { return _innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _innerDictionary.IsReadOnly; }
        }

        public void Add(ReportField key, string value)
        {
            if (_innerDictionary.ContainsKey(key))
                _innerDictionary[key] = value;
            else
                _innerDictionary.Add(key, value);
        }

        public bool ContainsKey(ReportField key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public bool Remove(ReportField key)
        {
            return _innerDictionary.ContainsKey(key) && _innerDictionary.Remove(key);
        }

        public bool TryGetValue(ReportField key, out string value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public string this[ReportField key]
        {
            get
            {
                string obj;
                _innerDictionary.TryGetValue(key, out obj);
                return obj ?? string.Empty;
            }
            set
            {
                _innerDictionary[key] = value;
            }
        }

        public ICollection<ReportField> Keys
        {
            get { return _innerDictionary.Keys; }
        }

        public ICollection<string> Values
        {
            get { return _innerDictionary.Values; }
        }
    }
}