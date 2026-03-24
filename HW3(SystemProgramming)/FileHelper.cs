using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HW3_SystemProgramming_
{
    public static class FileHelper
    {
        public static void WriteJsonSerializer(params Card[] cards)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();

            using (var sw = new StreamWriter("cards.json"))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    serializer.Serialize(jw, cards);
                }
            }
        }
        public static void UpdateCard(Card card)
        {
            var cards = ReadJsonSerializer();
            cards.FirstOrDefault(c => c.Pan == card.Pan).Balance = card.Balance;
            WriteJsonSerializer(cards);
        }
        public static Card[] ReadJsonSerializer()
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var sr = new StreamReader("cards.json"))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    var card = serializer.Deserialize<Card[]>(jr);

                    return card ?? new Card[0];
                }
            }
        }
    }
}
