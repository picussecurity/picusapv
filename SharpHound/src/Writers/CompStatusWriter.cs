using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sharphound.Client;
using SharpHoundCommonLib;

namespace Sharphound.Writers
{
    internal class CompStatusWriter : BaseWriter<CSVComputerStatus>
    {
        private static readonly object LockObj = new();
        private readonly Channel<CSVComputerStatus> _channel;
        private readonly IContext _context;
        private StreamWriter _streamWriter;
        public static string computerStatusCSV;

        private static Random random = new Random();

        public static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public CompStatusWriter(IContext context, Channel<CSVComputerStatus> channel) : base("99_" + RandomString())
        {
            _context = context;
            _channel = channel;
            _channel = channel;
            if (!_context.Flags.DumpComputerStatus) NoOp = true;
        }

        protected override async Task WriteData()
        {
            foreach (var item in Queue) await _streamWriter.WriteLineAsync(item.ToCsv());
        }

        internal async Task StartWriter()
        {
            if (_context.Flags.DumpComputerStatus)
            {
                while (await _channel.Reader.WaitToReadAsync())
                {
                    if (!_channel.Reader.TryRead(out var item)) continue;
                    await AcceptObject(item);
                }

                await FlushWriter();
            }
        }

        internal override async Task FlushWriter()
        {
            await WriteData();
            await _streamWriter.FlushAsync();
            CloseLog();
        }

        protected override void CreateFile()
        {
            var filename = _context.ResolveFileName(DataType, "pcs", true);
            computerStatusCSV = filename;
            var exists = File.Exists(filename);
            _streamWriter = new StreamWriter(
                new FileStream(filename, exists ? FileMode.Truncate : FileMode.Create, FileAccess.ReadWrite),
                new UTF8Encoding(false));
            _streamWriter.WriteLine("ComputerName,Task,Status");
        }

        private void CloseLog()
        {
            try
            {
                lock (LockObj)
                {
                    _streamWriter.Close();
                }
            }
            catch (Exception e)
            {
                _context.Logger.LogError(e, "Error closing task log");
            }
        }
    }
}