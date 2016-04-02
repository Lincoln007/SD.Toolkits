﻿using System;
using System.Collections.Generic;
using Quartz;
using SD.Toolkits.TaskScheduler.ITask;

namespace SD.Toolkits.TaskSchedulerTests.StubTasks
{
    /// <summary>
    /// 报时任务
    /// </summary>
    public class StubShowTimeTask : BaseTask<StubShowTimeTask>
    {
        /// <summary>
        /// 参照时间集
        /// </summary>
        public readonly static ICollection<DateTime> ReferenceTimes = new HashSet<DateTime>();

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context">执行上下文</param>
        public override void Execute(IJobExecutionContext context)
        {
            ReferenceTimes.Add(DateTime.Now);
        }
    }
}
