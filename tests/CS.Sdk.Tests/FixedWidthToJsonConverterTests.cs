using CS.Sdk.Converters;
using System.Collections.Generic;
using Xunit;

namespace CS.Sdk.Tests
{
    public sealed class FixedWidthToJsonConverterTests
    {
        [Fact]
        public void Netss_Core_Basic_Conversion_M_01()
        {
            string fixedWidthData = @"M00203990605S01531027400001028999999990070142080806519999999";

            FixedWidthToJsonConverter converter = new FixedWidthToJsonConverter();
            ConversionResult result = converter.Convert(fixedWidthData);

            List<Dictionary<string, object>> messages = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(result.Json);

            Assert.Single(messages);

            Dictionary<string, object> message = messages[0];

            Assert.True(message.ContainsKey("transaction_id"));
            Assert.True(message.ContainsKey("source_format"));
            Assert.True(message.ContainsKey("netss_unique_case_id"));

            Assert.True(message.ContainsKey("netss_record_type"));
            Assert.True(message.ContainsKey("netss_year"));
            Assert.True(message.ContainsKey("netss_week"));
            Assert.True(message.ContainsKey("netss_birth_date"));
            Assert.True(message.ContainsKey("netss_record_number_in_payload"));
            Assert.True(message.ContainsKey("netss_record_type"));
            Assert.True(message.ContainsKey("netss_update"));
            Assert.True(message.ContainsKey("netss_state"));
            Assert.True(message.ContainsKey("netss_case_report_id"));
            Assert.True(message.ContainsKey("netss_site_code"));
            Assert.True(message.ContainsKey("netss_event_code"));
            Assert.True(message.ContainsKey("netss_count"));
            Assert.True(message.ContainsKey("netss_county_code"));
            Assert.True(message.ContainsKey("netss_age"));
            Assert.True(message.ContainsKey("netss_age_type"));
            Assert.True(message.ContainsKey("netss_sex"));
            Assert.True(message.ContainsKey("netss_race"));
            Assert.True(message.ContainsKey("netss_ethnicity"));
            Assert.True(message.ContainsKey("netss_event_date"));
            Assert.True(message.ContainsKey("netss_event_date_type"));
            Assert.True(message.ContainsKey("netss_case_status"));
            Assert.True(message.ContainsKey("netss_imported"));
            Assert.True(message.ContainsKey("netss_outbreak"));
            Assert.True(message.ContainsKey("netss_future"));

            Assert.Equal("NETSS", message["source_format"].ToString());
            Assert.Equal("990605_02_S01_03", message["netss_unique_case_id"].ToString());

            Assert.Equal("M", message["netss_record_type"].ToString());
            Assert.Equal("0", message["netss_update"].ToString());
            Assert.Equal("03", message["netss_year"].ToString());
            Assert.Equal("53", message["netss_week"].ToString());
            Assert.Equal("99999999", message["netss_birth_date"].ToString());
            Assert.Equal("1", message["netss_record_number_in_payload"].ToString());
            Assert.Equal("02", message["netss_state"].ToString());
            Assert.Equal("990605", message["netss_case_report_id"].ToString());
            Assert.Equal("S01", message["netss_site_code"].ToString());
            Assert.Equal("10274", message["netss_event_code"].ToString());
            Assert.Equal("00001", message["netss_count"].ToString());
            Assert.Equal("028", message["netss_county_code"].ToString());
            Assert.Equal("007", message["netss_age"].ToString());
            Assert.Equal("0", message["netss_age_type"].ToString());
            Assert.Equal("1", message["netss_sex"].ToString());
            Assert.Equal("4", message["netss_race"].ToString());
            Assert.Equal("2", message["netss_ethnicity"].ToString());
            Assert.Equal("080806", message["netss_event_date"].ToString());
            Assert.Equal("5", message["netss_event_date_type"].ToString());
            Assert.Equal("1", message["netss_case_status"].ToString());
            Assert.Equal("9", message["netss_imported"].ToString());
            Assert.Equal("9", message["netss_outbreak"].ToString());
            Assert.Equal("99999", message["netss_future"].ToString());
        }

        [Fact]
        public void Netss_Core_Basic_Conversion_M_02()
        {
            string netss = @"M00497990605S02531027400001028192204220070142080806519999999";

            FixedWidthToJsonConverter converter = new FixedWidthToJsonConverter();
            ConversionResult result = converter.Convert(netss);

            List<Dictionary<string, object>> messages = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(result.Json);

            Assert.Single(messages);

            Dictionary<string, object> message = messages[0];

            Assert.True(message.ContainsKey("transaction_id"));
            Assert.True(message.ContainsKey("source_format"));
            Assert.True(message.ContainsKey("netss_unique_case_id"));

            Assert.True(message.ContainsKey("netss_record_type"));
            Assert.True(message.ContainsKey("netss_year"));
            Assert.True(message.ContainsKey("netss_week"));
            Assert.True(message.ContainsKey("netss_birth_date"));
            Assert.True(message.ContainsKey("netss_record_number_in_payload"));
            Assert.True(message.ContainsKey("netss_record_type"));
            Assert.True(message.ContainsKey("netss_update"));
            Assert.True(message.ContainsKey("netss_state"));
            Assert.True(message.ContainsKey("netss_case_report_id"));
            Assert.True(message.ContainsKey("netss_site_code"));
            Assert.True(message.ContainsKey("netss_event_code"));
            Assert.True(message.ContainsKey("netss_count"));
            Assert.True(message.ContainsKey("netss_county_code"));
            Assert.True(message.ContainsKey("netss_age"));
            Assert.True(message.ContainsKey("netss_age_type"));
            Assert.True(message.ContainsKey("netss_sex"));
            Assert.True(message.ContainsKey("netss_race"));
            Assert.True(message.ContainsKey("netss_ethnicity"));

            Assert.True(message.ContainsKey("netss_event_date"));
            Assert.True(message.ContainsKey("netss_event_date_type"));
            Assert.True(message.ContainsKey("netss_case_status"));
            Assert.True(message.ContainsKey("netss_imported"));
            Assert.True(message.ContainsKey("netss_outbreak"));
            Assert.True(message.ContainsKey("netss_future"));

            Assert.Equal("NETSS", message["source_format"].ToString());
            Assert.Equal("990605_04_S02_97", message["netss_unique_case_id"].ToString());

            Assert.Equal("M", message["netss_record_type"].ToString());
            Assert.Equal("0", message["netss_update"].ToString());
            Assert.Equal("97", message["netss_year"].ToString());
            Assert.Equal("53", message["netss_week"].ToString());
            Assert.Equal("19220422", message["netss_birth_date"].ToString());
            Assert.Equal("1", message["netss_record_number_in_payload"].ToString());
            Assert.Equal("04", message["netss_state"].ToString());
            Assert.Equal("990605", message["netss_case_report_id"].ToString());
            Assert.Equal("S02", message["netss_site_code"].ToString());
            Assert.Equal("10274", message["netss_event_code"].ToString());
            Assert.Equal("00001", message["netss_count"].ToString());
            Assert.Equal("028", message["netss_county_code"].ToString());
            Assert.Equal("007", message["netss_age"].ToString());
            Assert.Equal("0", message["netss_age_type"].ToString());
            Assert.Equal("1", message["netss_sex"].ToString());
            Assert.Equal("4", message["netss_race"].ToString());
            Assert.Equal("2", message["netss_ethnicity"].ToString());
            Assert.Equal("080806", message["netss_event_date"].ToString());
            Assert.Equal("5", message["netss_event_date_type"].ToString());
            Assert.Equal("1", message["netss_case_status"].ToString());
            Assert.Equal("9", message["netss_imported"].ToString());
            Assert.Equal("9", message["netss_outbreak"].ToString());
            Assert.Equal("99999", message["netss_future"].ToString());
        }

        [Fact]
        public void Netss_Core_Basic_Conversion_With_GenV2_Translation()
        {
            string netss = @"M00497990605S02531027400001028192204220070142080806519999999";

            FixedWidthToJsonConverter converter = new FixedWidthToJsonConverter();
            ConversionResult result = converter.Convert(netss);

            System.Diagnostics.Debug.WriteLine(result.Json);

            List<Dictionary<string, object>> messages = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, object>>>(result.Json);

            Assert.Single(messages);

            Dictionary<string, object> message = messages[0];

            Assert.True(message.ContainsKey("race_category"));
            Assert.True(message.ContainsKey("ethnic_group"));
            Assert.True(message.ContainsKey("mmwr_week"));
            Assert.True(message.ContainsKey("count"));
            Assert.True(message.ContainsKey("birth_date"));
            Assert.True(message.ContainsKey("event_date"));
            Assert.True(message.ContainsKey("age_at_case_investigation"));
            Assert.True(message.ContainsKey("age_unit_at_case_investigation"));
            Assert.True(message.ContainsKey("subjects_sex__code"));

            Assert.Equal("NETSS", message["source_format"].ToString());

            Assert.Equal("M", message["subjects_sex__code"].ToString());
            Assert.Equal(7, int.Parse(message["age_at_case_investigation"].ToString()));
            Assert.Equal(53, int.Parse(message["mmwr_week"].ToString()));
            Assert.Equal(1, int.Parse(message["count"].ToString()));

            Assert.Equal("year [time]", message["age_unit_at_case_investigation"].ToString());
            Assert.Equal("a", message["age_unit_at_case_investigation__code"].ToString());
            Assert.Equal("UCUM", message["age_unit_at_case_investigation__code_system"].ToString());

            Assert.Equal("2008-08-06T00:00:00", message["event_date"].ToString());
            Assert.Equal("1922-04-22T00:00:00", message["birth_date"].ToString());

        }
    }
}