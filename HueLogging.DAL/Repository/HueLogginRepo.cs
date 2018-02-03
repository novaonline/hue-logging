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
			return _context.HueConfigStates.OrderByDescending(x => x.Id).LastOrDefault();
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
	}
}
