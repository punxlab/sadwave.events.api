using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SadWave.Events.Api.Repositories.Events;
using SadWave.Events.Api.Services.Events;

namespace SadWave.Events.Api.Tests.Services
{
	[TestFixture]
	public class EventsUtilsTests
	{
		[Test]
		public void HasNewEventsReturnsTrueIfOldEventsAreNullButNewAreNot()
		{
			List<EventRecord> oldEvents = null;
			var newEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				}
			};

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeTrue();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfOldEventsAreNullButNewAreNull()
		{
			List<EventRecord> oldEvents = null;
			List<EventRecord> newEvents = null;

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfOldEventsAreNullButNewAreEmpty()
		{
			List<EventRecord> oldEvents = null;
			var newEvents = new List<EventRecord>();

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfNewEventsAreNull()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				}
			};
			List<EventRecord> newEvents = null;

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfNewEventsAreEmpty()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				}
			};
			var newEvents = new List<EventRecord>();

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfOldAndNewEventsAreTheSameTest()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = null,
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = null,
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name`",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};
			var newEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = null,
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = null,
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name`",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfOldAndNewEventsAreTheSameButOldContainObsolete()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "ObsoleteEventName",
					Date = new EventDateRecord
					{
						Date = DateTime.MinValue,
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test-old-obs.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name1",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};
			var newEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name1",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsFalseIfOldAndNewEventsAreTheSameButOldAndNewContainObsolete()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "OldObsoleteEventName",
					Date = new EventDateRecord
					{
						Date = DateTime.MinValue,
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test-old-obs.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name1",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};
			var newEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "NewObsoleteEventName",
					Date = new EventDateRecord
					{
						Date = DateTime.MinValue,
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test-new-obs.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name1",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeFalse();
		}

		[Test]
		public void HasNewEventsReturnsTrueIfOldAndNewEventsAreNotTheSame()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				}
			};
			var newEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name1",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeTrue();
		}

		[Test]
		public void HasNewEventsReturnsTrueIfOldAndNewEventsAreNotTheSameButBothContainObsolete()
		{
			var oldEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "OldObsoleteEventName",
					Date = new EventDateRecord
					{
						Date = DateTime.MinValue,
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test-old-obsolete.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				}
			};
			var newEvents = new List<EventRecord>
			{
				new EventRecord
				{
					Name = "NewObsoleteEventName",
					Date = new EventDateRecord
					{
						Date = DateTime.MinValue,
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test-new-obsolete.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test.ru"),
					Address = "Address",
					Description = "Description",
					Overview = "Overview",
					Photo = new Uri("http://testphoto.ru")
				},
				new EventRecord
				{
					Name = "Name1",
					Date = new EventDateRecord
					{
						Date = DateTime.UtcNow.AddDays(1),
						Time = new TimeSpan(12, 0, 0)
					},
					Url = new Uri("http://test1.ru"),
					Address = "Address1",
					Description = "Description1",
					Overview = "Overview1",
					Photo = new Uri("http://testphoto1.ru")
				}
			};

			var actual = EventsUtils.HasNewEvents(oldEvents, newEvents);
			actual.Should().BeTrue();
		}
	}
}
