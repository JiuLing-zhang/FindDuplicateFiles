using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using FindDuplicateFiles.Filters;

namespace FindDuplicateFiles.SearchFile
{
    public class SearchDuplicateJob
    {
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool _isStop;

        /// <summary>
        /// 执行任务的消息
        /// </summary>
        public Action<string> EventMessage;

        /// <summary>
        /// 文件查找任务队列
        /// </summary>
        private FileProcessingQueue _fileProcessingQueue;

        public Action<string, SimpleFileInfo> EventDuplicateFound;

        public async void Start(SearchConfigs config)
        {
            _isStop = false;
            await Task.Run(() =>
            {
                _fileProcessingQueue = new FileProcessingQueue
                {
                    EventDuplicateFound = EventDuplicateFound,
                    EventMessage = EventMessage
                };

                _fileProcessingQueue.Start(config.SearchMatch);
                foreach (string folderPath in config.Folders)
                {
                    EachDirectory(folderPath, paths =>
                    {
                        CalcFilesInfo(paths, config.SearchOption);
                    });
                }

                if (_isStop)
                {
                    _fileProcessingQueue.Finished();
                }
            });
        }

        private void EachDirectory(string folderPath, Action<List<string>> callbackFilePaths)
        {
            try
            {
                if (_isStop)
                {
                    return;
                }

                Directory.GetDirectories(folderPath).ToList().ForEach(path =>
                {
                    //继续遍历文件夹内容
                    EachDirectory(path, callbackFilePaths);
                });

                callbackFilePaths.Invoke(Directory.GetFiles(folderPath).ToList());


            }
            catch (UnauthorizedAccessException)
            {
                //todo 没有权限时记录错误
            }
        }

        private void CalcFilesInfo(List<string> paths, SearchOptionEnum searchOption)
        {
            EventMessage?.Invoke($"读取文件：{string.Join(",", paths)}");
            //根据路径加载文件信息
            var files = paths.Select(path => new FileInfo(path)).ToList();

            //条件过滤器
            if ((searchOption & SearchOptionEnum.IgnoreEmptyFile) == SearchOptionEnum.IgnoreEmptyFile)
            {
                IFilePathFilter filter = new EmptyFileFilter();
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.IgnoreHiddenFile) == SearchOptionEnum.IgnoreHiddenFile)
            {
                IFilePathFilter filter = new HiddenFileFilter();
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.IgnoreSmallFile) == SearchOptionEnum.IgnoreSmallFile)
            {
                IFilePathFilter filter = new SmallFileFilter(1024);
                files = filter.FilterByCondition(files);
            }

            files.ForEach(file =>
            {
                //符合条件的文件写入队列
                var simpleInfo = new SimpleFileInfo()
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Size = file.Length
                };
                _fileProcessingQueue.AddOneFileToQueue(simpleInfo);
            });
        }
        public void Stop()
        {
            _isStop = true;
            _fileProcessingQueue.Stop();
        }
    }
}
