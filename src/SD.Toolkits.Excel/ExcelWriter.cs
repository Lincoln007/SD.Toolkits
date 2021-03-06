﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace SD.Toolkits.Excel
{
    /// <summary>
    /// Excel写入器
    /// </summary>
    public static class ExcelWriter
    {
        //Public

        #region # 将集合写入Excel文件 —— static void WriteFile<T>(IEnumerable<T> enumerable...
        /// <summary>
        /// 将集合写入Excel文件
        /// </summary>
        /// <param name="enumerable">集合对象</param>
        /// <param name="path">写入路径</param>
        /// <param name="titles">标题集</param>
        public static void WriteFile<T>(IEnumerable<T> enumerable, string path, string[] titles = null)
        {
            T[] array = enumerable == null ? new T[0] : enumerable.ToArray();

            #region # 验证参数

            if (!array.Any())
            {
                throw new ArgumentNullException("enumerable", string.Format(@"源{0}类型集合不可为空！", typeof(T).Name));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path", @"写入路径不可为空！");
            }
            if (Path.GetExtension(path) != ".xls")
            {
                throw new ArgumentOutOfRangeException("path", @"文件格式不正确！");
            }

            #endregion

            //创建工作簿
            IWorkbook workbook = CreateWorkbook(array, titles);

            //写入文件
            using (FileStream stream = File.OpenWrite(path))
            {
                workbook.Write(stream);
            }
        }
        #endregion

        #region # 将集合写入Excel字节数组 —— static byte[] WriteStream<T>(IEnumerable<T>...
        /// <summary>
        /// 将集合写入Excel字节数组
        /// </summary>
        /// <param name="enumerable">集合对象</param>
        /// <param name="titles">标题集</param>
        /// <returns>字节数组</returns>
        public static byte[] WriteStream<T>(IEnumerable<T> enumerable, string[] titles = null)
        {
            T[] array = enumerable == null ? new T[0] : enumerable.ToArray();

            #region # 验证参数

            if (!array.Any())
            {
                throw new ArgumentNullException("enumerable", string.Format(@"源{0}类型集合不可为空！", typeof(T).Name));
            }

            #endregion

            IWorkbook workbook = CreateWorkbook(array, titles);

            //写入内存流
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                return stream.ToArray();
            }
        }
        #endregion


        //Private

        #region # 创建工作簿 —— static IWorkbook CreateWorkbook<T>(T[] array...
        /// <summary>
        /// 创建工作簿
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="titles">标题集</param>
        /// <returns>工作簿</returns>
        private static IWorkbook CreateWorkbook<T>(T[] array, string[] titles = null)
        {
            //01.创建工作簿
            IWorkbook workbook = new HSSFWorkbook();

            //02.创建工作表
            ISheet sheet = workbook.CreateSheet(typeof(T).Name);

            #region //03.创建标题行

            IRow rowTitle = sheet.CreateRow(0);

            string[] defaultTitles = typeof(T).GetProperties().Select(x => x.Name).ToArray();

            if (titles == null)
            {
                CreateTitleRow(defaultTitles, rowTitle);
            }
            else
            {
                if (titles.Length != defaultTitles.Length)
                {
                    throw new InvalidDataException("标题列数与数据列数不一致！");
                }

                CreateTitleRow(titles, rowTitle);
            }

            #endregion

            //04.创建数据行
            CreateDataRows(array, sheet);
            return workbook;
        }
        #endregion

        #region # 创建标题行 —— static void CreateTitleRow(string[] titles, IRow rowTitle)
        /// <summary>
        /// 创建标题行
        /// </summary>
        /// <param name="titles">标题数组</param>
        /// <param name="rowTitle">标题行</param>
        private static void CreateTitleRow(string[] titles, IRow rowTitle)
        {
            for (int i = 0; i < titles.Length; i++)
            {
                rowTitle.CreateCell(i).SetCellValue(titles[i]);
            }
        }
        #endregion

        #region # 创建数据行 —— static void CreateDataRows<T>(T[] array, ISheet sheet)
        /// <summary>
        /// 创建数据行
        /// </summary>
        /// <param name="array">集合对象</param>
        /// <param name="sheet">工作表对象</param>
        private static void CreateDataRows<T>(T[] array, ISheet sheet)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T item = array[i];
                PropertyInfo[] properties = typeof(T).GetProperties();
                IRow row = sheet.CreateRow(i + 1);

                for (int j = 0; j < properties.Length; j++)
                {
                    //在行中创建单元格
                    if (properties[j].PropertyType == typeof(double))
                    {
                        row.CreateCell(j).SetCellValue((double)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(float))
                    {
                        row.CreateCell(j).SetCellValue((float)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(decimal))
                    {
                        row.CreateCell(j).SetCellValue(Convert.ToDouble(properties[j].GetValue(item)));
                    }
                    else if (properties[j].PropertyType == typeof(byte))
                    {
                        row.CreateCell(j).SetCellValue((byte)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(short))
                    {
                        row.CreateCell(j).SetCellValue((short)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(int))
                    {
                        row.CreateCell(j).SetCellValue((int)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(long))
                    {
                        row.CreateCell(j).SetCellValue((long)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(bool))
                    {
                        row.CreateCell(j).SetCellValue((bool)properties[j].GetValue(item));
                    }
                    else if (properties[j].PropertyType == typeof(DateTime))
                    {
                        row.CreateCell(j).SetCellValue(((DateTime)properties[j].GetValue(item)).ToString("yyyy/MM/dd HH:mm:ss"));
                    }
                    else
                    {
                        row.CreateCell(j).SetCellValue(properties[j].GetValue(item) == null ? string.Empty : properties[j].GetValue(item).ToString());
                    }
                }
            }
        }
        #endregion
    }
}
