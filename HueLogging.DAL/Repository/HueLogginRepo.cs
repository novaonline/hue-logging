using HueLogging.Models;
using HueLogging.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HueLogging.DAL.Repository
{
	public class HueLogginRepo : IHueLoggingRepo, IDisposable
	{
		private HueLoggingContext _context;
		private bool disposed = false;

		public HueLogginRepo(HueLoggingContext context)
		{
			_context = context;
		}

		public HueConfigStates GetRecentConfig()
		{
			return _context.HueConfigStates.OrderByDescending(x => x.Id).FirstOrDefault();
		}

		public LightEvent GetEvent(int id)
		{
			return _context.LightEvent
				.Include(x => x.Light)
				.Include(x => x.State).FirstOrDefault(x => x.Id == id);
		}

		public LightEvent GetLastEvent()
		{
			return (from l in _context.LightEvent
					orderby l.AddDate descending
					select l)
					.Include(x => x.Light)
					.Include(x => x.State)
					.FirstOrDefault();
		}

		public LightEvent GetLastEvent(string lightId)
		{
			return (from l in _context.LightEvent
					where l.Light.Id == lightId
					orderby l.AddDate descending
					select l)
					.Include(x => x.Light)
					.Include(x => x.State)
					.FirstOrDefault();
		}

		public IEnumerable<LightEvent> GetRecentEvents(TimeSpan durationBack)
		{
			return (from l in _context.LightEvent
					where DateTime.UtcNow.Subtract(durationBack) < l.AddDate
					orderby l.AddDate descending
					select l)
					.Include(x => x.Light)
					.Include(x => x.State);
		}

		public IEnumerable<LightEvent> GetLastNumberOfEvents()
		{
			return (from l in _context.LightEvent
					orderby l.AddDate descending
					select l).Take(10)
					.Include(x => x.Light)
					.Include(x => x.State);
		}

		public void Save(LightEvent lightEvent)
		{
			_context.LightEvent.Add(lightEvent);
			_context.SaveChanges();
		}

		public void Save(HueConfigStates configStates)
		{
			_context.HueConfigStates.Add(configStates);
			_context.SaveChanges();
		}

		public void Save(IEnumerable<LightEvent> lightEvents)
		{
			_context.LightEvent.AddRange(lightEvents);
			_context.SaveChanges();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public HueSession GetLastIncompleteSession(string lightId)
		{
			return (from s in _context.HueSessions
					where s.LightId == lightId && s.EndDate == null
					orderby s.Id descending
					select s).FirstOrDefault();
		}

		public IEnumerable<HueSession> GetHueSessions(DateTime startDate, DateTime endDate)
		{
			return (from s in _context.HueSessions
					where startDate <= s.StartDate && s.StartDate <= endDate
					&& s.EndDate.HasValue
					select s)
					.Include(x=>x.Light);
		}

		public void Save(HueSession hueSession)
		{
			if (hueSession.Id == 0)
			{
				_context.Add(hueSession);
			}
			else
			{
				_context.Update(hueSession);
			}
			_context.SaveChanges();
		}

		public IEnumerable<HueSessionSummary> GetHueSessionSummary(DateTime startDate, DateTime endDate)
		{
			return (from s in GetHueSessions(startDate, endDate)
					orderby s.StartDate ascending
					group s by s.Light into g
					select new HueSessionSummary
					{
						Light = g.Key,
						TotalDuration = new TimeSpan(g.Sum(x => (x.EndDate.Value - x.StartDate).Ticks))
					});
		}
	}
}
