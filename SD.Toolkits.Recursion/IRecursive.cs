﻿using System.Collections.Generic;

namespace SD.Toolkits.Recursion
{
    /// <summary>
    /// 可递归接口
    /// </summary>
    public interface IRecursive<T>
    {
        #region 是否是根级节点 —— bool IsRoot
        /// <summary>
        /// 是否是根级节点
        /// </summary>
        bool IsRoot { get; }
        #endregion

        #region 是否是叶子级节点 —— bool IsLeaf
        /// <summary>
        /// 是否是叶子级节点
        /// </summary>
        bool IsLeaf { get; }
        #endregion

        #region 父节点 —— IRecursion ParentNode
        /// <summary>
        /// 父节点
        /// </summary>
        T ParentNode { get; }
        #endregion

        #region 子节点集 —— ICollection<T> SubNodes
        /// <summary>
        /// 子节点集
        /// </summary>
        ICollection<T> SubNodes { get; }
        #endregion
    }
}
