using System;
using System.Text.Json.Serialization;

namespace EmailSenderLib
{
    internal class EmailData
    {
        [JsonPropertyName("r1")]
        public Int32 R1 { get; set; }
        [JsonPropertyName("sender")]
        public String Sender { get; set; }
        [JsonPropertyName("subject")]
        public String Subject { get; set; }
        [JsonPropertyName("text")]
        public String Text { get; set; }
        [JsonPropertyName("r2")]
        public Int32 R2 { get; set; }
    }
}
