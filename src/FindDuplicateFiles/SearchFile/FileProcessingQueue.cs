using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindDuplicateFiles.SearchFile
{
    public class FileProcessingQueue
    {
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool _isStop;
        /// <summary>
        /// 用于处理文件的队列
        /// </summary>
        private readonly ConcurrentQueue<SimpleFileInfo> _fileProcessing = new();

        private static readonly SemaphoreSlim MySemaphoreSlim = new(5, 5);
        public async void Start()
        {
            _isStop = false;
            await Task.Run(() =>
             {
                 while (!_isStop)
                 {
                     if (!_fileProcessing.TryDequeue(out var fileInfo))
                     {
                         continue;
                     }

                     Task.Run(() =>
                     {
                         SearchDuplicate(fileInfo);
                     });
                 }
             });
        }
        /// <summary>
        /// 查找重复文件的具体逻辑
        /// </summary>
        /// <param name="fileInfo"></param>
        private async void SearchDuplicate(SimpleFileInfo fileInfo)
        {
            await MySemaphoreSlim.WaitAsync();

            MySemaphoreSlim.Release();
        }

        public void Add(SimpleFileInfo fileInfo)
        {
            _fileProcessing.Enqueue(fileInfo);
        }

        public void Stop()
        {
            _isStop = true;
        }
    }
}
