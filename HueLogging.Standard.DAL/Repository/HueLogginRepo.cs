using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HueLogging.Standard.DAL.Repository
{
	public class HueLogginRepo : IHueLoggingRepo, IDisposable
	{
		private HueLoggingContext _context;
		private ILogger<HueLogginRepo> logger;
		private bool disposed = false;

		public HueLogginRepo(HueLoggingContext context, ILogger<HueLogginRepo> logger)
		{
			_context = context;
			this.logger = logger;
		}

		public HueConfigStates GetRecentConfig()
		{
			logger.LogInformation("Getting Recent Configurations");
			var r = _context.HueConfigStates.OrderByDescending(x => x.Id).FirstOrDefault();
			logger.LogInformation($"Recent Config Result: {r?.Id}");
			return r;
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

		public IEnumerable<LightEvent> GetRecentEvents(string lightId, TimeSpan durationBack)
		{
			return (from l in _context.LightEvent.Include(x => x.Light).Include(x => x.State)
					where DateTime.UtcNow.Subtract(durationBack) < l.AddDate && l.Light.Id == lightId
					orderby l.AddDate descending
					select l);
		
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
					where s.EndDate.HasValue
					orderby s.StartDate ascending
					group s by s.Light into g
					select new HueSessionSummary
					{
						Light = g.Key,
						TotalDuration = new TimeSpan(g.Sum(x => (x.EndDate.Value - x.StartDate).Ticks))
					});
		}

		public Light GetLightByName(string lightName)
		{
			return (from x in _context.LightEvent.Include(y => y.Light)
					where x.Light.Name == lightName
					orderby x.Id descending
					select x).FirstOrDefault()?.Light;
		}
	}
}
