using System;
using System.Buffers;
using System.IO;
using System.Linq;
using CS.Sdk.Batchers;
using Xunit;

namespace CS.Sdk.Tests
{
    public sealed class HL7v2StreamingDebatcherTests
    {
        public class StringDebatchHandler : IDebatchHandler
        {
            private readonly ReadOnlySpanAction<char, MessageDebatchMetadata> _callback;


            public StringDebatchHandler(ReadOnlySpanAction<char, MessageDebatchMetadata> callback)
            {
                _callback = callback;
            }

            public MessageHandleMetadata HandleDebatch(ReadOnlySpan<char> hl7v2message, MessageDebatchMetadata metadata)
            {
                _callback(hl7v2message, metadata);
                return default(MessageHandleMetadata);
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static void Callback_DTS_01(ReadOnlySpan<char> message, MessageDebatchMetadata metadata)
        {
            string payload = message.ToString();

            // callback for every message that's debatched
            if (metadata.MessageBatchPosition == 1)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19370313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            if (metadata.MessageBatchPosition == 2)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName2^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC02^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19380313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            if (metadata.MessageBatchPosition == 3)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName3^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC03^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19390313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            if (metadata.MessageBatchPosition == 4)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName4^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC04^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19400313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            Assert.True(metadata.MessageBatchPosition <= 4);
        }

        [Fact]
        public void Debatcher_Test_Success_01()
        {
            string message = @"FHS|^~\&
BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19370313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
MSH|^~\&|SendAppName2^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC02^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19380313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
MSH|^~\&|SendAppName3^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC03^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19390313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
MSH|^~\&|SendAppName4^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC04^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19400313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
BTS|4|Facility reporting for 2-1-2011
FTS|1";

            IDebatchHandler handler = new StringDebatchHandler(Callback_DTS_01);
            HL7v2StreamingDebatcher debatcher = new HL7v2StreamingDebatcher();
            DebatchResult result = debatcher.DebatchAsync(GenerateStreamFromString(message), handler, "1234").Result;

            Assert.Equal(4, result.ActualMessageCount);
            Assert.Equal(4, result.ReportedMessageCount);
            Assert.Empty(result.ProcessResultMessages);

            Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
            Assert.Equal("ER1^2.16.840.1.113883.19.3.1.1^ISO", result.SenderApplication);
            Assert.Equal("CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO", result.SenderFacility);
            Assert.Equal("SS_APP^2.16.840.1.113883.19.3.2.1^ISO", result.ReceiverApplication);
            Assert.Equal("SPH^2.16.840.1.113883.19.3.2^ISO", result.ReceiverFacility);
        }

        private static void Callback_DTS_02(ReadOnlySpan<char> message, MessageDebatchMetadata metadata)
        {
            string payload = message.ToString();

            // callback for every message that's debatched
            if (metadata.MessageBatchPosition == 1)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130801|F||2054-5^Black or African American^CDCREC|^^^22^71301^^^^22079|||||||||||2186-5^Not Hispanic or Latino^CDCREC
NK1|1||MTH^Mother^HL70063|^^^22^71301^USA^^^22079||||||||||S^Single^HL70002||19940113||||||||||||2186-5^Not Hispanic or Latino^CDCREC|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130825170100|||||||||||||||20130825170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20130801||||||F", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            if (metadata.MessageBatchPosition == 2)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC02|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC02^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130411|F||2054-5^Black or African American^CDCREC|^^^17^60651^^^^17031|||||||||||UNK^Unknown^NULLFL
NK1|1||MTH^Mother^HL70063|^^^17^60651^USA^^^17031||||||||||S^Single^HL70002||19901115||||||||||||UNK^Unknown^NULLFL|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC02^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130525170100|||||||||||||||20130525170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20130411||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20130411||||||F", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            if (metadata.MessageBatchPosition == 3)
            {
                Assert.Equal(@"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC03|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC03^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130917|F||2054-5^Black or African American^CDCREC|^^^17^60643^^^^17031|||||||||||2186-5^Not Hispanic or Latino^CDCREC
NK1|1||MTH^Mother^HL70063|^^^17^60643^USA^^^17031||||||||||S^Single^HL70002||19920827||||||||||||2186-5^Not Hispanic or Latino^CDCREC|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC03^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130922170100|||||||||||||||20130922170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F", payload, ignoreCase: false, ignoreLineEndingDifferences: true);
            }

            Assert.True(metadata.MessageBatchPosition <= 3);
        }

        [Fact]
        public void Debatcher_Test_Success_02()
        {
            string message = @"FHS|^~\&
BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130801|F||2054-5^Black or African American^CDCREC|^^^22^71301^^^^22079|||||||||||2186-5^Not Hispanic or Latino^CDCREC
NK1|1||MTH^Mother^HL70063|^^^22^71301^USA^^^22079||||||||||S^Single^HL70002||19940113||||||||||||2186-5^Not Hispanic or Latino^CDCREC|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130825170100|||||||||||||||20130825170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20130801||||||F
MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC02|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC02^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130411|F||2054-5^Black or African American^CDCREC|^^^17^60651^^^^17031|||||||||||UNK^Unknown^NULLFL
NK1|1||MTH^Mother^HL70063|^^^17^60651^USA^^^17031||||||||||S^Single^HL70002||19901115||||||||||||UNK^Unknown^NULLFL|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC02^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130525170100|||||||||||||||20130525170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20130411||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20130411||||||F
MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC03|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC03^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130917|F||2054-5^Black or African American^CDCREC|^^^17^60643^^^^17031|||||||||||2186-5^Not Hispanic or Latino^CDCREC
NK1|1||MTH^Mother^HL70063|^^^17^60643^USA^^^17031||||||||||S^Single^HL70002||19920827||||||||||||2186-5^Not Hispanic or Latino^CDCREC|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC03^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130922170100|||||||||||||||20130922170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
BTS|3|Facility reporting for 2-1-2011
FTS|1";

            IDebatchHandler handler = new StringDebatchHandler(Callback_DTS_02);
            HL7v2StreamingDebatcher debatcher = new HL7v2StreamingDebatcher();
            DebatchResult result = debatcher.DebatchAsync(GenerateStreamFromString(message), handler, "1234").Result;

            Assert.Equal(3, result.ActualMessageCount);
            Assert.Equal(3, result.ReportedMessageCount);
            Assert.Empty(result.ProcessResultMessages);

            Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
            Assert.Equal("ER1^2.16.840.1.113883.19.3.1.1^ISO", result.SenderApplication);
            Assert.Equal("CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO", result.SenderFacility);
            Assert.Equal("SS_APP^2.16.840.1.113883.19.3.2.1^ISO", result.ReceiverApplication);
            Assert.Equal("SPH^2.16.840.1.113883.19.3.2^ISO", result.ReceiverFacility);
        }

        //         [Fact]
        //         public void Debatcher_Test_Warning_Count_Mismatch()
        //         {
        //             string message = @"FHS|^~\&
        // BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
        // MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19370313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // MSH|^~\&|SendAppName2^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19380313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // MSH|^~\&|SendAppName3^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19390313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // MSH|^~\&|SendAppName4^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19400313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // BTS|3|Facility reporting for 2-1-2011
        // FTS|1";

        //             HL7v2Debatcher debatcher = new HL7v2Debatcher();
        //             DebatchResult result = debatcher.Debatch(message, null, "1234");

        //             Assert.Equal(4, result.ActualMessageCount);
        //             Assert.Equal(3, result.ReportedMessageCount);
        //             Assert.Single(result.ProcessResultMessages);

        //             Assert.Equal(Severity.Warning, result.ProcessResultMessages[0].Severity);
        //             Assert.Equal("0002", result.ProcessResultMessages[0].ErrorCode);

        //             Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
        //             Assert.Equal("ER1^2.16.840.1.113883.19.3.1.1^ISO", result.SenderApplication);
        //             Assert.Equal("CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO", result.SenderFacility);
        //             Assert.Equal("SS_APP^2.16.840.1.113883.19.3.2.1^ISO", result.ReceiverApplication);
        //             Assert.Equal("SPH^2.16.840.1.113883.19.3.2^ISO", result.ReceiverFacility);
        //         }

        //         [Fact]
        //         public void Debatcher_Test_Warning_Count_Invalid()
        //         {
        //             string message = @"FHS|^~\&
        // BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
        // MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19370313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // MSH|^~\&|SendAppName2^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19380313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // MSH|^~\&|SendAppName3^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19390313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // MSH|^~\&|SendAppName4^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19400313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
        // BTS|A|Facility reporting for 2-1-2011
        // FTS|1";

        //             HL7v2Debatcher debatcher = new HL7v2Debatcher();
        //             DebatchResult result = debatcher.Debatch(message, null, "1234");

        //             var warning1 = result.ProcessResultMessages.FirstOrDefault(m => m.ErrorCode == "0001");
        //             var warning2 = result.ProcessResultMessages.FirstOrDefault(m => m.ErrorCode == "0002");

        //             Assert.Equal(4, result.ActualMessageCount);
        //             Assert.Equal(-1, result.ReportedMessageCount);
        //             Assert.Equal(2, result.ProcessResultMessages.Count);


        //             Assert.Equal(Severity.Warning, warning1.Severity);
        //             Assert.Equal("0001", warning1.ErrorCode);

        //             Assert.Equal(Severity.Warning, warning2.Severity);
        //             Assert.Equal("0002", warning2.ErrorCode);

        //             Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
        //             Assert.Equal("ER1^2.16.840.1.113883.19.3.1.1^ISO", result.SenderApplication);
        //             Assert.Equal("CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO", result.SenderFacility);
        //             Assert.Equal("SS_APP^2.16.840.1.113883.19.3.2.1^ISO", result.ReceiverApplication);
        //             Assert.Equal("SPH^2.16.840.1.113883.19.3.2^ISO", result.ReceiverFacility);
        //         }

        //         [Fact]
        //         public void Debatcher_Test_Error_no_Messages()
        //         {
        //             string message = @"FHS|^~\&
        // BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
        // BTS|0|Facility reporting for 2-1-2011
        // FTS|1";

        //             HL7v2Debatcher debatcher = new HL7v2Debatcher();
        //             DebatchResult result = debatcher.Debatch(message, null, "1234");

        //             Assert.Equal(0, result.ActualMessageCount);
        //             Assert.Equal(0, result.ReportedMessageCount);
        //             Assert.Single(result.ProcessResultMessages);

        //             Assert.Equal(Severity.Error, result.ProcessResultMessages[0].Severity);
        //             Assert.Equal("0007", result.ProcessResultMessages[0].ErrorCode);

        //             Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
        //         }

        //         private static void Callback_Debatcher_Test_Error_Missing_FHS(ReadOnlySpan<char> message, MessageDebatchMetadata metadata)
        //         {
        //             string payload = message.ToString();

        //             // callback for every message that's debatched
        //             if (metadata.MessageBatchPosition == 1)
        //             {
        //                 Assert.Equal(@"MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO", payload);
        //             }

        //             Assert.True(metadata.MessageBatchPosition <= 1);
        //         }

        //         [Fact]
        //         public void Debatcher_Test_Error_Missing_FHS()
        //         {
        //             string message = @"BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
        // MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // BTS|1|Facility reporting for 2-1-2011
        // FTS|1";

        //             IDebatchHandler handler = new StringDebatchHandler(Callback_Debatcher_Test_Error_Missing_FHS);
        //             HL7v2Debatcher debatcher = new HL7v2Debatcher();
        //             DebatchResult result = debatcher.Debatch(message, handler, "1234");

        //             Assert.Equal(0, result.ActualMessageCount);
        //             Assert.Equal(0, result.ReportedMessageCount);

        //             Assert.Equal(Severity.Error, result.ProcessResultMessages[0].Severity);
        //             Assert.Equal("0003", result.ProcessResultMessages[0].ErrorCode);
        //         }

        //         [Fact]
        //         public void Debatcher_Test_Error_Missing_BHS()
        //         {
        //             string message = @"FHS|^~\&
        // MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // BTS|1|Facility reporting for 2-1-2011
        // FTS|1";

        //             HL7v2Debatcher debatcher = new HL7v2Debatcher();
        //             DebatchResult result = debatcher.Debatch(message, null, "1234");

        //             Assert.Equal(1, result.ActualMessageCount);
        //             Assert.Equal(1, result.ReportedMessageCount);

        //             Assert.Equal(Severity.Error, result.ProcessResultMessages[0].Severity);
        //             Assert.Equal("0004", result.ProcessResultMessages[0].ErrorCode);

        //             Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
        //         }

        //         [Fact]
        //         public void Debatcher_Test_Error_Missing_BTS()
        //         {
        //             string message = @"FHS|^~\&
        // BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
        // MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        // FTS|1";

        //             HL7v2Debatcher debatcher = new HL7v2Debatcher();
        //             DebatchResult result = debatcher.Debatch(message, null, "1234");

        //             var errorMessage = result.ProcessResultMessages.Where(m => m.Content == "The required BTS segment is missing").FirstOrDefault();

        //             Assert.Equal(1, result.ActualMessageCount);
        //             Assert.Equal(-1, result.ReportedMessageCount); // will be -1 because the BTS segment contains this information and in this test, it's missing

        //             Assert.Equal(Severity.Error, errorMessage.Severity);
        //             Assert.Equal("0006", errorMessage.ErrorCode);
        //         }

        //        [Fact]
        //        public void Debatcher_Test_Error_Missing_FTS()
        //        {
        //            string message = @"FHS|^~\&
        //BHS|^~\&|ER1^2.16.840.1.113883.19.3.1.1^ISO|CITY_GENERAL^2.16.840.1.113883.19.3.1^ISO|SS_APP^2.16.840.1.113883.19.3.2.1^ISO|SPH^2.16.840.1.113883.19.3.2^ISO|20080723123558-0400
        //MSH|^~\&|SendAppName1^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
        //BTS|1|Facility reporting for 2-1-2011";

        //            HL7Debatcher debatcher = new HL7Debatcher();
        //            DebatchResult result = debatcher.Debatch(message, "1234");

        //            var errorMessage = result.ProcessResultMessages.Where(m => m.Content == "The required FTS segment is missing").FirstOrDefault();

        //            Assert.Equal(1, result.ActualMessageCount);
        //            Assert.Equal(1, result.ReportedMessageCount);

        //            Assert.Equal(Severity.Error, errorMessage.Severity);
        //            Assert.Equal("0005", errorMessage.ErrorCode);

        //            Assert.Equal("Facility reporting for 2-1-2011", result.BatchComments);
        //        }
    }
}
