﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Action<string> ExecutedMessage;

        /// <summary>
        /// 文件查找任务队列
        /// </summary>
        private FileProcessingQueue _fileProcessingQueue;

        public Action<string, SimpleFileInfo> Duplicate;

        public async void Start(SearchConfigs config)
        {
            ExecutedMessage?.Invoke("准备查找....");
            _isStop = false;
            await Task.Run(() =>
            {
                _fileProcessingQueue = new FileProcessingQueue
                {
                    Duplicate = Duplicate
                };

                _fileProcessingQueue.Start(config.SearchMatch);
                foreach (string folderPath in config.Folders)
                {
                    EachDirectory(folderPath, fileName =>
                    {
                        ExecutedMessage?.Invoke(fileName);
                        CalcFileInfo(fileName, config.SearchOption);
                    });
                }

                if (_isStop != false)
                {
                    _fileProcessingQueue.Finished();
                }

                // for (int i = 0; i < config.Folders.Count; i++)
                // {
                //     var index = i;
                //     Task.Run(() =>
                //     {
                //         EachDirectory(config.Folders[index], fileName =>
                //         {
                //             CalcFileInfo(fileName, config.SearchOption);
                //         });
                //     });
                // }
            });
        }

        private void EachDirectory(string folderPath, Action<string> calcFileInfo)
        {
            try
            {
                if (_isStop)
                {
                    return;
                }
                Directory.GetFiles(folderPath).ToList().ForEach(calcFileInfo.Invoke);

                Directory.GetDirectories(folderPath).ToList().ForEach(path =>
                {
                    //继续遍历文件夹内容
                    EachDirectory(path, calcFileInfo);
                });

            }
            catch (UnauthorizedAccessException ex)
            {
                //todo 没有权限时记录错误
            }
        }

        private void CalcFileInfo(string fileName, SearchOptionEnum searchOption)
        {
            var fi = new FileInfo(fileName);
            decimal fileSize = fi.Length;
            if ((searchOption & SearchOptionEnum.IgnoreSizeZero) == SearchOptionEnum.IgnoreSizeZero && fileSize == 0)
            {
                return;
            }
            if ((searchOption & SearchOptionEnum.IgnoreHiddenFile) == SearchOptionEnum.IgnoreHiddenFile && fi.Attributes == FileAttributes.Hidden)
            {
                return;
            }

            fileSize = fileSize / 1024;
            if ((searchOption & SearchOptionEnum.IgnoreSmallFile) == SearchOptionEnum.IgnoreSmallFile && fileSize < 1024)
            {
                return;
            }

            var simpleInfo = new SimpleFileInfo()
            {
                Name = fi.Name,
                Path = fi.FullName,
                Size = fileSize
            };
            _fileProcessingQueue.Add(simpleInfo);
        }
        public void Stop()
        {
            _isStop = true;
            ExecutedMessage?.Invoke("");
            _fileProcessingQueue.Stop();
        }
    }
}
