using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        /// <summary>
        /// 任务是否添加完成
        /// </summary>
        private bool _isFinished;
        /// <summary>
        /// 匹配方式
        /// </summary>
        private SearchMatchEnum _searchMatch;

        private static readonly SemaphoreSlim MySemaphoreSlim = new(5, 5);
        private readonly ConcurrentDictionary<string, List<SimpleFileInfo>> _duplicateFiles = new();

        /// <summary>
        /// 执行任务的消息
        /// </summary>
        public Action<string> EventMessage;
        public Action<string, SimpleFileInfo> EventDuplicateFound;
        public async void Start(SearchMatchEnum searchMatch)
        {

            _isStop = false;
            _isFinished = false;
            EventMessage?.Invoke("准备查找....");
            await Task.Run(() =>
            {
                _searchMatch = searchMatch;
                while (!_isStop)
                {
                    if (!_fileProcessing.TryDequeue(out var fileInfo))
                    {
                        if (_isFinished)
                        {
                            return;
                        }
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

            EventMessage?.Invoke($"重复校验：{fileInfo.Path}");
            string fileKey = "";
            if ((_searchMatch & SearchMatchEnum.FileName) == SearchMatchEnum.FileName)
            {
                fileKey = fileInfo.Name;
            }
            if ((_searchMatch & SearchMatchEnum.FileSize) == SearchMatchEnum.FileSize)
            {
                fileKey = $"{fileKey}${fileInfo.Size}";
            }

            if (!_duplicateFiles.ContainsKey(fileKey))
            {
                _duplicateFiles[fileKey] = new List<SimpleFileInfo>()
                {
                    fileInfo
                };
            }
            else
            {

                if (_duplicateFiles[fileKey].Count == 1)
                {
                    //如果是第一次发现重复，则需要连同之前一次的文件信息一并通知
                    EventDuplicateFound?.Invoke(fileKey, _duplicateFiles[fileKey][0]);
                }

                //本次的文件
                _duplicateFiles[fileKey].Add(fileInfo);
                EventDuplicateFound?.Invoke(fileKey, fileInfo);
            }

            MySemaphoreSlim.Release();
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        public void Finished()
        {
            _isFinished = true;
        }

        public void AddOneFileToQueue(SimpleFileInfo file)
        {
            _fileProcessing.Enqueue(file);
        }

        public void AddMultipleFilesToQueue(List<SimpleFileInfo> files)
        {
            foreach (var file in files)
            {
                _fileProcessing.Enqueue(file);
            }
        }

        public void Stop()
        {
            _isStop = true;
            EventMessage?.Invoke("任务被终止");
        }
    }
}
