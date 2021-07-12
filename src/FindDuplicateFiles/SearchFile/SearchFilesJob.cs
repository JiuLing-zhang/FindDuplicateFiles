using FindDuplicateFiles.Common;
using FindDuplicateFiles.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FindDuplicateFiles.SearchFile
{
    public class SearchFilesJob
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
        private CheckDuplicateQueue _checkDuplicateQueue;

        /// <summary>
        /// 发现重复文件
        /// </summary>
        public Action<string, SimpleFileInfo> EventDuplicateFound;
        /// <summary>
        /// 搜索完成
        /// </summary>
        public Action EventSearchFinished;

        public async void Start(SearchConfigs config)
        {
            _isStop = false;
            await Task.Run(() =>
            {
                _checkDuplicateQueue = new CheckDuplicateQueue
                {
                    EventDuplicateFound = EventDuplicateFound,
                    EventMessage = EventMessage,
                    EventSearchFinished = EventSearchFinished
                };

                _checkDuplicateQueue.Start(config.SearchMatch);
                foreach (string folderPath in config.Folders)
                {
                    EachDirectory(folderPath, paths =>
                    {
                        CalcFilesInfo(paths, config.SearchOption);
                    });
                }

                _checkDuplicateQueue.Finished();
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
            if (paths.Any(x => x.IndexOf("JZY.rar") >= 0))
            {
                // return;
            }
            //根据路径加载文件信息
            var files = paths.Select(path => new FileInfo(path)).ToList();

            //条件过滤器
            if ((searchOption & SearchOptionEnum.IgnoreEmptyFile) == SearchOptionEnum.IgnoreEmptyFile)
            {
                IFileSearchFilter filter = new IgnoreEmptyFileFilter();
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.IgnoreHiddenFile) == SearchOptionEnum.IgnoreHiddenFile)
            {
                IFileSearchFilter filter = new IgnoreHiddenFileFilter();
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.IgnoreSmallFile) == SearchOptionEnum.IgnoreSmallFile)
            {
                IFileSearchFilter filter = new IgnoreSmallFileFilter(1024);
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.OnlyDocumentFile) == SearchOptionEnum.OnlyDocumentFile)
            {
                IFileSearchFilter filter = new OnlyExtensionFilter(GlobalArgs.AppConfig.DocumentExtension.Split(';').ToList());
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.OnlyImageFile) == SearchOptionEnum.OnlyImageFile)
            {
                IFileSearchFilter filter = new OnlyExtensionFilter(GlobalArgs.AppConfig.ImageExtension.Split(';').ToList());
                files = filter.FilterByCondition(files);
            }
            if ((searchOption & SearchOptionEnum.OnlyMediaFile) == SearchOptionEnum.OnlyMediaFile)
            {
                IFileSearchFilter filter = new OnlyExtensionFilter(GlobalArgs.AppConfig.MediaExtension.Split(';').ToList());
                files = filter.FilterByCondition(files);
            }

            files.ForEach(file =>
            {
                //符合条件的文件写入队列
                var simpleInfo = new SimpleFileInfo()
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Size = file.Length,
                    LastWriteTime = file.LastWriteTime,
                    Extension = file.Extension.ToLower()
                };
                _checkDuplicateQueue.AddOneFileToQueue(simpleInfo);
            });
        }
        public void Stop()
        {
            _isStop = true;
            _checkDuplicateQueue.Stop();
        }
    }
}
