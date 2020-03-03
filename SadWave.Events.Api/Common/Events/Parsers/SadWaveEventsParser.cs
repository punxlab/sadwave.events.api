using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using SadWave.Events.Api.Common.Dates;
using SadWave.Events.Api.Common.Html;

namespace SadWave.Events.Api.Common.Events.Parsers
{
	public class SadWaveEventsParser
	{
		private readonly HtmlProvider _htmlProvider;

		public SadWaveEventsParser(HtmlProvider htmlProvider)
		{
			_htmlProvider = htmlProvider ?? throw new ArgumentNullException(nameof(htmlProvider));
		}

		public Task<List<SadWaveEvent>> ParseAsync(Uri eventUrl)
		{
			if (eventUrl == null)
				throw new ArgumentNullException(nameof(eventUrl));

			return ParseByUrlAsync(eventUrl);
		}

		private async Task<List<SadWaveEvent>> ParseByUrlAsync(Uri eventUrl)
		{
			var eventsList = new List<SadWaveEvent>();
			const string entryContentClass = ".entry-content";

			var document = await _htmlProvider.GetHtmlAsync(eventUrl);

			var content = document.QuerySelector(entryContentClass);

			int? firstMonth = null;

			foreach (var element in content.ChildNodes)
			{
				if (!DateTimeUtils.TryParse(element.TextContent, out var date))
					continue;

				// In some cases Sadwave reviewers set <p> instead of <ul>.
				if (element.NextSibling is IHtmlParagraphElement)
				{
					var eventItem = CreateEvent(date, element.NextSibling, ref firstMonth);
					eventsList.Add(eventItem);
					continue;
				}

				if (!(element.NextSibling is IHtmlUnorderedListElement))
					continue;

				foreach (var child in element.NextSibling.ChildNodes)
				{
					if (!(child is IHtmlListItemElement))
						continue;

					var eventItem = CreateEvent(date, child, ref firstMonth);
					eventsList.Add(eventItem);
				}
			}

			return eventsList;
		}

		private SadWaveEvent CreateEvent(DateTime date, INode contentNode, ref int? firstMonth)
		{
			var text = contentNode.TextContent;
			var urlNode = FindUrlNode(contentNode);

			Uri url = null;
			if (urlNode != null)
			{
				var anchorNode = (IHtmlAnchorElement)urlNode;
				url = new Uri(anchorNode.Href);
			}

			var year = DateTime.UtcNow.Year;

			// Hot fix. If it is the first record and difference between
			// current month and event month more than 10 probably
			// it is previous year
			if (DateTime.UtcNow.Month - date.Month <= -10)
			{
				year = year - 1;
			}
			else if (firstMonth > DateTime.UtcNow.Month)
			{
				year = year + 1;
			}

			return new SadWaveEvent
			{
				Date = new DateTime(year, date.Month, date.Day),
				Text = text,
				Url = url
			};
		}

		private INode FindUrlNode(INode node)
		{
			if (!node.HasChildNodes)
				return null;

			var urlNode = GetEventUrl(node);

			if(urlNode == null)
			{
				foreach (var childNode in node.ChildNodes)
				{
					urlNode = FindUrlNode(childNode);
					if (urlNode != null)
						break;
				}
			}

			return urlNode;
		}

		private INode GetEventUrl(INode node)
		{
			var urlNodes = node
				.ChildNodes
				.Where(child => child is IHtmlAnchorElement)
				.Cast<IHtmlAnchorElement>()
				.ToList();

			var vkNode = urlNodes.FirstOrDefault(actor => actor.Href.Contains("vk.com"));
			if (vkNode != null)
				return vkNode;

			var fbNode = urlNodes.FirstOrDefault(actor => actor.Href.Contains("fb") || actor.Href.Contains("facebook"));
			if (fbNode != null)
				return fbNode;

			return urlNodes.FirstOrDefault();
		}
	}
}