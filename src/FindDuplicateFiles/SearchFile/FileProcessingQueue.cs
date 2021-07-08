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

        public Action<string, SimpleFileInfo> Duplicate;
        public async void Start(SearchMatchEnum searchMatch)
        {
            _isStop = false;
            _isFinished = false;
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
                //第一个文件
                Duplicate?.Invoke(fileKey, _duplicateFiles[fileKey][0]);

                //本次的文件
                _duplicateFiles[fileKey].Add(fileInfo);
                Duplicate?.Invoke(fileKey, fileInfo);
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
