using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
	public struct Timer
	{
		public long Id { get; set; }
		public long Time { get; set; }
		public TaskCompletionSource<bool> tcs;
	}
    public class TimerService
    {
        private readonly Dictionary<long, Timer> timers = new Dictionary<long, Timer>();
        private readonly Queue<long> timeOutTimerIds = new Queue<long>();
        private long minTime;

		public void Update()
		{
			if (this.timers.Count == 0)
			{
				return;
			}

			long timeNow = TimeHelper.Now();

			if (timeNow < this.minTime)
			{
				return;
			}

			long newMinTime = -1;
			foreach (var kv in timers)
			{
				if (kv.Value.Time <= timeNow)
				{
					timeOutTimerIds.Enqueue(kv.Key);
				}
				else
				{
					if (newMinTime == -1)
					{
						newMinTime = kv.Value.Time;
					}
					else
					{
						if (kv.Value.Time < newMinTime)
						{
							newMinTime = kv.Value.Time;
						}
					}
				}
			}

			minTime = newMinTime;

			while (timeOutTimerIds.Count>0)
			{
				long timerId = this.timeOutTimerIds.Dequeue();
				Timer timer;
				if (!this.timers.TryGetValue(timerId, out timer))
				{
					continue;
				}
				this.timers.Remove(timerId);
				timer.tcs.SetResult(true);
			}
		}

		public void Remove(long id)
		{
			Timer timer;
			if (!this.timers.TryGetValue(id, out timer))
			{
				return;
			}
			this.timers.Remove(id);
		}
		
		public Task Delay(long time)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			Timer timer = new Timer { Id = UniqueIdHelper.CreateId(), Time = TimeHelper.Now() + time, tcs = tcs };
			if (timers.ContainsKey(timer.Id))
			{
				tcs.SetResult(true);
				return tcs.Task;
			}
			this.timers[timer.Id] = timer;
			
			
			if (timer.Time < this.minTime)
			{
				this.minTime = timer.Time;
			}
			return tcs.Task;
		}
    }
}