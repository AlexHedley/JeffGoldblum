using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using StreamDeckLib;
using StreamDeckLib.Messages;

using Jeffsum;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace JeffGoldblumPlugin
{
    [ActionUuid(Uuid = "com.alexhedley.jeffGoldblum.receivethejeff")]
    public class JeffGoldblumPluginAction : BaseStreamDeckActionWithSettingsModel<Models.SettingsModel>
    {
        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            //var title = $"{SettingsModel.Count.ToString()} {SettingsModel.JeffsumType.ToString("g")}";
            var title = $"{SettingsModel.Count.ToString()} {SettingsModel.JeffsumType.ToString()}";
            await Manager.SetTitleAsync(args.context, title);

            var count = SettingsModel.Count;
            var jeffsumType = SettingsModel.JeffsumType;

            //this.Logger.LogInformation($"Count: {count} | jeffsumType: {jeffsumType}");

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "JeffSum");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, "quote.txt");

            var items = new List<string>();

            switch (jeffsumType)
            {
                case "Words": // JeffsumType.Words:
                    var words = Goldblum.ReceiveTheJeff(count, JeffsumType.Words);
                    items = words.ToList();
                    break;
                case "Quotes": // JeffsumType.Quotes:
                    var quotes = Goldblum.ReceiveTheJeff(count, JeffsumType.Quotes);
                    items = quotes.ToList();
                    break;
                case "Paragraphs": // JeffsumType.Paragraphs:
                default:
                    var paragraphs = Goldblum.ReceiveTheJeff(count, JeffsumType.Paragraphs); //default
                    items = paragraphs.ToList();
                    break;
            }

            // TODO: Separator configurable?
            var jeffsum = string.Join(Environment.NewLine, items);
            File.WriteAllText(path, jeffsum);

            //update settings
            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
            await Manager.SetTitleAsync(args.context, SettingsModel.JeffsumType.ToString());
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await base.OnWillAppear(args);
            await Manager.SetTitleAsync(args.context, SettingsModel.JeffsumType.ToString());
        }

    }
}
