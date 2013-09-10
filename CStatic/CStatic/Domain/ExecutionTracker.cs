using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public class ExecutionTracker
    {
        private Dictionary<string, ProcessResult> _Results;

        public ExecutionTracker()
        {
            _Results = new Dictionary<string, ProcessResult>();
        }

        public void AddResult(ProcessRequest req, ProcessResult res)
        {
            _Results[req.SourceFileName] = res;
        }

        public ProcessResult GetResult(string sourceFileName)
        {
            if (_Results.ContainsKey(sourceFileName))
                return _Results[sourceFileName];
            return null;
        }
    }
}
