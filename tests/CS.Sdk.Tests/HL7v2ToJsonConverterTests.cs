using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CS.Sdk.Converters;
using CS.Sdk.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace CS.Sdk.Tests
{
    public class HL7v2ToJsonConverterTests
    {
        #region TB Message 01
        private const string TB_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|78746-5^Country of Birth^LN||CHN^CHINA^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^UNITED STATES^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20180115||||||F
OBX|4|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|5|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|6|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|7|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|8|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
OBX|9|ST|74549-7^Person Reporting to CDC - Name^LN||Solo, Han||||||F
OBX|10|ST|74547-1^Person Reporting to CDC - Email^LN||hsolo@ga.dph.gov||||||F
OBX|11|CWE|77989-2^Transmission Mode^LN||416380006^Airborne transmission^SCT||||||F
OBX|12|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|13|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|14|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|15|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|16|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|17|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|18|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F
OBX|19|CWE|48766-0^Reporting Source Type Code^LN||PHC1221^TB Clinic^CDCPHINVS||||||F
OBX|20|ST|INV1107^TB State Case Number^PHINQUESTION||2018GA987436179||||||F
OBX|21|CWE|DEM153^Detailed Race^PHINQUESTION||2034-7^Chinese^CDCREC||||||F
OBX|22|DT|DEM2005^Date Arrived in US^PHINQUESTION||20171107||||||F
OBX|23|CWE|DEM2003^US Born^PHINQUESTION||N^No^HL70136||||||F
OBX|24|CWE|INV1114^Remain in US After Report^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|25|CWE|INV1116^Initial Reason for Evaluation^PHINQUESTION||PHC681^Contact investigation^CDCPHINVS||||||F
OBX|26|ST|INV1108^City or County Case Number^PHINQUESTION||2018GAFULT04583||||||F
OBX|27|CWE|76689-9^Birth Sex^LN||M^Male^HL70001||||||F
OBX|28|CWE|INV1109^Previously Counted Case^PHINQUESTION||PHC659^Verified Case: Counted by another US area^CDCPHINVS||||||F
OBX|29|ST|INV1110^Previously Reported State Case Number^PHINQUESTION||2018AL987436179||||||F
OBX|30|CWE|INV1112^Inside City Limits^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|31|CWE|INV1276^Occupation and Industry Category^PHINQUESTION||UNK^Unknown^NULLFL||||||F
OBX|32|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|1|PHC2098^Diabetic at Diagnostic Evaluation^CDCPHINVS||||||F
OBX|33|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|1|N^No^HL70136||||||F
OBX|34|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|2|46177005^End-Stage Renal Disease^SCT||||||F
OBX|35|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|2|N^No^HL70136||||||F
OBX|36|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|3|86933000^Heavy Alcohol Use in the Past 12 Months^SCT||||||F
OBX|37|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|3|Y^Yes^HL70136||||||F
OBX|38|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|4|161663000^Post-Organ Transplantation^SCT||||||F
OBX|39|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|4|N^No^HL70136||||||F
OBX|40|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|5|32911000^Homeless Ever^SCT||||||F
OBX|41|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|5|Y^Yes^HL70136||||||F
OBX|42|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|6|PHC1876^Homeless in the Past 12 Months^CDCPHINVS||||||F
OBX|43|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|6|Y^Yes^HL70136||||||F
OBX|44|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|7|226034001^Injecting Drug Use in the Past 12 Months^SCT||||||F
OBX|45|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|7|N^No^HL70136||||||F
OBX|46|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|8|PHC1877^Noninjecting Drug Use in the Past 12 Months^CDCPHINVS||||||F
OBX|47|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|8|Y^Yes^HL70136||||||F
OBX|48|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|9|42665001^Resident of Long-Term Care Facility at Diagnositic Evaluation^SCT||||||F
OBX|49|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|9|UNK^Unknown^NULLFL||||||F
OBX|50|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|10|OTH^Other (specify)^NULLFL^^^^^^cancer||||||F
OBX|51|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|10|Y^Yes^HL70136||||||F
OBX|52|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|11|PHC1878^Other Immunocompromise (not HIV/AIDS)^CDCPHINVS||||||F
OBX|53|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|11|N^No^HL70136||||||F
OBX|54|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|12|257656006^Resident of Correctional Facility at Diagnostic Evaluation^SCT||||||F
OBX|55|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|12|N^No^HL70136||||||F
OBX|56|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|13|PHC2099^Resident of Correctional Facility Ever^CDCPHINVS||||||F
OBX|57|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|13|N^No^HL70136||||||F
OBX|58|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|14|PHC690^TNF Antagonist Therapy^CDCPHINVS||||||F
OBX|59|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|14|N^No^HL70136||||||F
OBX|60|CWE|INV1117^Patient Epidemiological Risk Factors^PHINQUESTION|15|3738000^Viral hepatitis (any type)^SCT||||||F
OBX|61|CWE|INV1118^Patient Epidemiological Risk Factors Indicator^PHINQUESTION|15|UNK^Unknown^NULLFL||||||F
OBX|62|CWE|72166-2^Smoking Status^LN||449868002^Current every day smoker^SCT||||||F
OBX|63|CWE|INV1121^Patient lived outside of US for more than 2 months^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|64|CWE|INV290^Test Type^PHINQUESTION|16|20431-3^Smear^LN||||||F
OBX|65|CWE|INV291^Test Result^PHINQUESTION|16|10828004^Positive^SCT||||||F
OBX|66|TS|82773-3^Date/Time of Lab Result^LN|16|20180205||||||F
OBX|67|CWE|31208-2^Specimen Source Site^LN|16|119334006^sputum^SCT||||||F
OBX|68|TS|68963-8^Specimen Collection Date/Time^LN|16|20180202||||||F
OBX|69|CWE|INV290^Test Type^PHINQUESTION|17|50941-4^Culture^LN||||||F
OBX|70|CWE|INV291^Test Result^PHINQUESTION|17|260385009^Negative^SCT||||||F
OBX|71|TS|82773-3^Date/Time of Lab Result^LN|17|20180314||||||F
OBX|72|CWE|31208-2^Specimen Source Site^LN|17|119334006^sputum^SCT||||||F
OBX|73|TS|68963-8^Specimen Collection Date/Time^LN|17|20180202||||||F
OBX|74|CWE|INV290^Test Type^PHINQUESTION|18|LAB673^NAA^PHINQUESTION||||||F
OBX|75|CWE|INV291^Test Result^PHINQUESTION|18|385660001^Not Done^SCT||||||F
OBX|76|CWE|INV290^Test Type^PHINQUESTION|19|LAB671^IGRA-QFT^PHINQUESTION||||||F
OBX|77|CWE|INV291^Test Result^PHINQUESTION|19|10828004^Positive^SCT||||||F
OBX|78|TS|82773-3^Date/Time of Lab Result^LN|19|20180203||||||F
OBX|79|TS|68963-8^Specimen Collection Date/Time^LN|19|20180202||||||F
OBX|80|CWE|INV290^Test Type^PHINQUESTION|20|55277-8^HIV Status^LN||||||F
OBX|81|CWE|INV291^Test Result^PHINQUESTION|20|10828004^Positive^SCT||||||F
OBX|82|TS|82773-3^Date/Time of Lab Result^LN|20|20180210||||||F
OBX|83|CWE|31208-2^Specimen Source Site^LN|20|87612001^Blood^SCT||||||F
OBX|84|TS|68963-8^Specimen Collection Date/Time^LN|20|20180204||||||F
OBX|85|CWE|INV290^Test Type^PHINQUESTION|21|TB119^Tuberculin Skin Test^PHINQUESTION||||||F
OBX|86|CWE|INV291^Test Result^PHINQUESTION|21|385660001^Not Done^SCT||||||F
OBX|87|CWE|INV290^Test Type^PHINQUESTION|22|76629-5^Fasting blood glucose^LN||||||F
OBX|88|CWE|INV291^Test Result^PHINQUESTION|22|385660001^Not Done^SCT||||||F
OBX|89|CWE|INV290^Test Type^PHINQUESTION|23|50595-8^Pathology^LN||||||F
OBX|90|CWE|INV291^Test Result^PHINQUESTION|23|385660001^Not Done^SCT||||||F
OBX|91|CWE|LAB677^Type of Chest Study^PHINQUESTION|24|399208008^Plain Chest X-Ray^SCT||||||F
OBX|92|CWE|LAB678^Result of Chest Study^PHINQUESTION|24|PHC1873^Consistent with TB^CDCPHINVS||||||F
OBX|93|CWE|LAB679^Evidence of Cavity^PHINQUESTION|24|N^No^HL70136||||||F
OBX|94|CWE|LAB680^Evidence of Miliary TB^PHINQUESTION|24|Y^Yes^HL70136||||||F
OBX|95|TS|LAB681^Date of Chest Study^PHINQUESTION|24|20180201||||||F
OBX|96|CWE|LAB677^Type of Chest Study^PHINQUESTION|25|169069000^CT Scan^SCT||||||F
OBX|97|CWE|LAB678^Result of Chest Study^PHINQUESTION|25|385660001^Not Done^SCT||||||F
OBX|98|CWE|INV1274^Case Meets Binational Reporting Criteria^PHINQUESTION||N^No^HL70136||||||F
OBX|99|CWE|INV1122^Identified During Contact Investigation^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|100|CWE|INV1123^Evaluation During Contact Investigation^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|101|ST|INV1124^Linked Case Number^PHINQUESTION||2018GA287163402||||||F
OBX|102|DT|86948-7^Date Treatment or Therapy Started^PHINQUESTION||20180210||||||F
OBX|103|CWE|55753-8^Treatment Administration Type^LN||435891000124101^DOT (Directly observed therapy, in person)^SCT~PHC1881^EDOT (Electronic DOT, via video call or other electronic method)^CDCPHINVS||||||F
OBX|104|CWE|INV1115^Case Verification Category^PHINQUESTION||PHC654^3 - Clinical Case Definition^CDCPHINVS||||||F
OBX|105|CWE|TB101^Status at Diagnosis of TB^PHINQUESTION||438949009^Alive^SCT||||||F
OBX|106|CWE|INV1133^Site of Disease^PHINQUESTION||39607008^Lung structure^SCT||||||F
OBX|107|CWE|INV1134^Contact Investigation^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|108|CWE|161413004^History of Previous Illness^SCT||Y^Yes^HL70136||||||F
OBX|109|CWE|INV1135^Diagnosis Type^PHINQUESTION|26|11999007^LTBI^SCT||||||F
OBX|110|DT|82758-4^Date of Previous Illness^LN|26|201504||||||F
OBX|111|CWE|INV1137^Completed Treatment for Previous Diagnosis^PHINQUESTION|26|Y^Yes^HL70136||||||F
OBX|112|CWE|INV1139^Reason Not Treated with RIPE^PHINQUESTION||PHC1909^Suspected Drug Resistance^CDCPHINVS||||||F
OBX|113|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|27|641^Amikacin^RXNORM||||||F
OBX|114|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|27|N^No^HL70136||||||F
OBX|115|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|28|7833^Para-Aminesalicylicacid^RXNORM||||||F
OBX|116|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|28|N^No^HL70136||||||F
OBX|117|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|29|1364504^Bedaquiline^RXNORM||||||F
OBX|118|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|29|Y^Yes^HL70136||||||F
OBX|119|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|30|78903^Capreomycin^RXNORM||||||F
OBX|120|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|30|N^No^HL70136||||||F
OBX|121|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|31|2551^Ciprofloxacin^RXNORM||||||F
OBX|122|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|31|Y^Yes^HL70136||||||F
OBX|123|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|32|2592^Clofazimine^RXNORM||||||F
OBX|124|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|32|N^No^HL70136||||||F
OBX|125|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|33|3007^Cycloserine^RXNORM||||||F
OBX|126|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|33|N^No^HL70136||||||F
OBX|127|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|34|PHC1889^Delamanid^CDCPHINVS||||||F
OBX|128|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|34|N^No^HL70136||||||F
OBX|129|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|35|4110^Ethambutol^RXNORM||||||F
OBX|130|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|35|Y^Yes^HL70136||||||F
OBX|131|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|36|4127^Ethionamide^RXNORM||||||F
OBX|132|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|36|N^No^HL70136||||||F
OBX|133|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|37|6038^Isoniazid^RXNORM||||||F
OBX|134|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|37|Y^Yes^HL70136||||||F
OBX|135|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|38|6099^Kanamycin^RXNORM||||||F
OBX|136|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|38|UNK^Unknown^NULLFL||||||F
OBX|137|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|39|82122^Levofloxacin^RXNORM||||||F
OBX|138|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|39|Y^Yes^HL70136||||||F
OBX|139|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|40|190376^Linezolid^RXNORM||||||F
OBX|140|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|40|UNK^Unknown^NULLFL||||||F
OBX|141|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|41|139462^Moxifloxacin^RXNORM||||||F
OBX|142|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|41|N^No^HL70136||||||F
OBX|143|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|42|7623^Ofloxacin^RXNORM||||||F
OBX|144|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|42|N^No^HL70136||||||F
OBX|145|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|43|OTH^Other (specify)^NULLFL||||||F
OBX|146|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|43|N^No^HL70136||||||F
OBX|147|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|44|PHC1888^Other Quinolones^CDCPHINVS||||||F
OBX|148|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|44|N^No^HL70136||||||F
OBX|149|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|45|8987^Pyrazinamide^RXNORM||||||F
OBX|150|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|45|Y^Yes^HL70136||||||F
OBX|151|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|46|55672^Rifabutin^RXNORM||||||F
OBX|152|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|46|N^No^HL70136||||||F
OBX|153|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|47|9384^Rifampin^RXNORM||||||F
OBX|154|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|47|N^No^HL70136||||||F
OBX|155|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|48|35617^Rifapentine^RXNORM||||||F
OBX|156|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|48|N^No^HL70136||||||F
OBX|157|CWE|INV1143^Initial Drug Regimen^PHINQUESTION|49|10109^Streptomycin^RXNORM||||||F
OBX|158|CWE|INV1144^Initial Drug Regimen Indicator^PHINQUESTION|49|N^No^HL70136||||||F
OBX|159|CWE|INV1145^Isolate Submitted for Genotyping^PHINQUESTION||N^No^HL70136||||||F
OBX|160|CWE|INV1147^Phenotypic Drug Susceptibility Completed^PHINQUESTION||N^No^HL70136||||||F
OBX|161|CWE|INV1148^Molecular Drug Susceptibility Completed^PHINQUESTION||N^No^HL70136||||||F
OBX|162|CWE|INV1275^Patient Treated as MDR Case^PHINQUESTION||N^No^HL70136||||||F
OBX|163|CWE|TB279^Patient Move During TB Therapy^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|164|CWE|INV1152^Moved to Where^PHINQUESTION||PHC246^Out of State^CDCPHINVS||||||F
OBX|165|CWE|INV1153^Out of State Move^PHINQUESTION||01^Alabama^FIPS5_2||||||F";
        #endregion

        #region Babesiosis Message 01
        private const string BABESIOSIS_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Babesiosis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||BABESIOSIS_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19370313|M||2106-3^White^CDCREC|^^^09^6320^^^^09011|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^UNITED STATES^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^UNITED STATES^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20150515||||||F
OBX|4|CWE|77978-5^Subject Died^LN||UNK^Unknown^NULLFL||||||F
OBX|5|ST|77993-4^State Case Identifier^LN||340227||||||F
OBX|6|ST|77997-5^Legacy Case Identifier^LN||34022748S012014||||||F
OBX|7|SN|77998-3^Age at Case Investigation^LN||^78|a^year [time]^UCUM|||||F
OBX|8|CWE|77982-7^Case Disease Imported Code^LN||PHC244^Indigenous^CDCPHINVS||||||F
OBX|9|TS|77976-9^Illness End Date^LN||20150530||||||F
OBX|10|SN|77977-7^Illness Duration^LN||^15|d^day [time]^UCUM|||||F
OBX|11|TS|77975-1^Diagnosis Date^LN||20150515||||||F
OBX|12|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
OBX|13|CWE|77984-3^Country of Exposure^LN|0|USA^UNITED STATES^ISO3166_1||||||F
OBX|14|CWE|77985-0^State or Province of Exposure^LN|0|09^Connecticut^FIPS5_2||||||F
OBX|15|ST|77986-8^City of Exposure^LN|0|New London, CT||||||F
OBX|16|ST|77987-6^County of Exposure^LN|0|New London, CT||||||F
OBX|17|ST|52831-5^Reporting Source ZIP Code^LN||06320||||||F
OBX|18|ST|74549-7^Person Reporting to CDC - Name^LN||Smith, John||||||F
OBX|19|ST|74548-9^Person Reporting to CDC - Phone Number^LN||(722) 277-4477||||||F
OBX|20|ST|74547-1^Person Reporting to CDC - Email^LN||Smith@you.com||||||F
OBX|21|DT|77979-3^Case Investigation Start Date^LN||20150515||||||F
OBX|22|DT|77995-9^Date Reported^LN||20150801||||||F
OBX|23|TS|77972-8^Earliest Date Reported to County^LN||20150801||||||F
OBX|24|TS|77973-6^Earliest Date Reported to State^LN||20150808||||||F
OBX|25|SN|77991-8^MMWR Week^LN||^32||||||F
OBX|26|DT|77992-6^MMWR Year^LN||2015||||||F
OBX|27|DT|77970-2^Date First Reported to PHD^LN||20150808||||||F
OBX|28|CWE|77966-0^Reporting State^LN||09^Connecticut^FIPS5_2||||||F
OBX|29|CWE|77968-6^National Reporting Jurisdiction^LN||09^CT^FIPS5_2||||||F
OBX|30|TX|77999-1^Comment^LN||a comment can be added here||||||F
OBX|31|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|32|CWE|77965-2^Immediate National Notifiable Condition^LN||N^No^HL70136||||||F
OBX|33|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F
OBX|34|ST|77969-4^Jurisdiction Code^LN||S01||||||F
OBX|35|CWE|48766-0^Reporting Source Type Code^LN||PHC251^Public Health Clinic^CDCPHINVS||||||F
OBX|36|ST|52526-1^Physician Name^LN|0|Smith, Jim||||||F
OBX|37|ST|68340-9^Physician Phone^LN|0|404.555.0123||||||F
OBX|38|CWE|300564004^Asplenic^SCT||N^No^HL70136||||||F
OBX|39|CWE|161413004^History of Previous Illness^SCT||Y^Yes^HL70136||||||F
OBX|40|CWE|264931009^Symptomatic^SCT||Y^Yes^HL70136||||||F
OBX|41|DT|82758-4^Date of Previous Illness^LN|0|2009||||||F
OBX|42|CWE|INV929^Clinical Manifestation^PHINQUESTION|0|386661006^Fever^SCT||||||F
OBX|43|CWE|INV930^Clinical Manifestation Indicator^PHINQUESTION|0|Y^Yes^HL70136||||||F
OBX|44|CWE|INV929^Clinical Manifestation^PHINQUESTION|1|271737000^Anemia^SCT||||||F
OBX|45|CWE|INV930^Clinical Manifestation Indicator^PHINQUESTION|1|Y^Yes^HL70136||||||F
OBX|46|CWE|INV929^Clinical Manifestation^PHINQUESTION|3|25064002^Headache^SCT||||||F
OBX|47|CWE|INV930^Clinical Manifestation Indicator^PHINQUESTION|3|N^No^HL70136||||||F
OBX|48|CWE|INV929^Clinical Manifestation^PHINQUESTION|4|68962001^Muscle pain^SCT||||||F
OBX|49|CWE|INV930^Clinical Manifestation Indicator^PHINQUESTION|4|Y^Yes^HL70136||||||F
OBX|50|SN|81265-1^Highest Measured Temperature^LN||^101|[degF]^degree Fahrenheit [temperature]^UCUM|||||F
OBX|51|CWE|255633001^Treatment Drug Indicator^SCT||Y^Yes^HL70136||||||F
OBX|52|CWE|55753-8^Treatment Drug(s)^LN||9071^Quinine^RXNORM~2582^Clindamycin^RXNORM||||||F
OBX|53|CWE|82312-0^Blood Transfusion^LN||N^No^HL70136||||||F
OBX|54|CWE|418912005^Transfusion Associated^SCT||N^No^HL70136||||||F
OBX|55|CWE|105470007^Blood Donor^SCT||N^No^HL70136||||||F
OBX|56|CWE|82762-6^Engage In Outdoor Activities^LN||Y^Yes^HL70136||||||F
OBX|57|CWE|82763-4^Outdoor Activities^LN||451261000124107^Hiking (qualifier value)^SCT~10496000^Camping^SCT||||||F
OBX|58|CWE|272500005^Wooded or Brushy Areas^SCT||Y^Yes^HL70136||||||F
OBX|59|CWE|95898004^Tick Bite^SCT||N^No^HL70136||||||F
OBX|60|CWE|420008001^Travel^SCT||Y^Yes^HL70136||||||F
OBX|61|CWE|82764-2^International Destination(s) of Recent Travel^LN|3|BRA^BRAZIL^ISO3166_1||||||F
OBX|62|DT|TRAVEL06^Date of Arrival to Travel Destination^PHINQUESTION|3|20150410||||||F
OBX|63|DT|TRAVEL07^Date of Departure from Travel Destination^PHINQUESTION|3|20150411||||||F
OBX|64|CWE|82764-2^International Destination(s) of Recent Travel^LN|4|HND^HONDURAS^ISO3166_1||||||F
OBX|65|DT|TRAVEL06^Date of Arrival to Travel Destination^PHINQUESTION|4|20150411||||||F
OBX|66|DT|TRAVEL07^Date of Departure from Travel Destination^PHINQUESTION|4|20150413||||||F
OBX|67|CWE|82764-2^International Destination(s) of Recent Travel^LN|5|MEX^MEXICO^ISO3166_1||||||F
OBX|68|DT|TRAVEL06^Date of Arrival to Travel Destination^PHINQUESTION|5|20150413||||||F
OBX|69|DT|TRAVEL07^Date of Departure from Travel Destination^PHINQUESTION|5|20150414||||||F
OBX|70|ST|85658-3^Current Occupation^LN|4|Retired||||||F
OBX|71|CE|85659-1^Current Occupation Standardized^LN|4|9060^Retired - NIOSH/NCHS [9060]^CDCOCCUPATION2010||||||F
OBX|72|ST|85078-4^Current Industry^LN|4|Retired||||||F
OBX|73|CE|85657-5^Current Industry Standardized^LN|4|9880^Retired [9880]^CDCINDUSTRY2010||||||F
OBX|74|ST|85658-3^Current Occupation^LN|5|Volunteer||||||F
OBX|75|CE|85659-1^Current Occupation Standardized^LN|5|9020^Volunteer - NIOSH/NCHS [9020]^CDCOCCUPATION2010||||||F
OBX|76|ST|85078-4^Current Industry^LN|5|Game Keeper||||||F
OBX|77|CE|85657-5^Current Industry Standardized^LN|5|0280^Fishing, hunting, and trapping [0280]^CDCINDUSTRY2010||||||F
OBX|78|CWE|INV290^Test Type^PHINQUESTION|4|9584-4^IFA � IgG^LN||||||F
OBX|79|CWE|INV291^Test Result^PHINQUESTION|4|10828004^Positive^SCT||||||F
OBX|80|CWE|LAB278^Organism Name^PHINQUESTION|4|76828008^Babesia microti^SCT||||||F
OBX|81|ST|LAB628^Test Result Quantitative^PHINQUESTION|4|1:256||||||F
OBX|82|TS|68963-8^Specimen Collection Date/Time^LN|4|20150515||||||F
OBX|83|CWE|INV290^Test Type^PHINQUESTION|5|5909-7^Blood smear^LN||||||F
OBX|84|CWE|INV291^Test Result^PHINQUESTION|5|10828004^Positive^SCT||||||F
OBX|85|CWE|LAB278^Organism Name^PHINQUESTION|5|372376003^Babesia species^SCT||||||F
OBX|86|TS|68963-8^Specimen Collection Date/Time^LN|5|20150515||||||F
OBR|2|BAB23456-1^EHR^2.16.840.1.113883.19.3.2.3^ISO|BAB9700123-1^Lab^2.16.840.1.113883.19.3.1.6^ISO|67866-4^Babesia identification panel^LN|||20150913150000|||||||||121121121^^^^^^^^NPI&2.16.840.1.113883.4.6&ISO^L^^^NPI^NPI_Facility&2.16.840.1.113883.3.72.5.26&ISO^^^^^^^MD||||||20150913150000|||F||||||A68.1^Tick-borne relapsing fever^I10C|||||||||||||||||||
OBX|1|CWE|21089-8^Babesia microti DNA [Presence] in Blood by Probe and target amplification method^SCT||10828004^Positive^SCT||Negative|POS|||F|||20150515|||0132^Polymerase Chain Reaction (PCR)^OBSMETHOD||20150516||||GHH Lab||9876543
SPM|1|BAB1234-1&EHR&2.16.840.1.113883.19.3.2.3&ISO^BAB9700122-1&Lab&2.16.840.1.113883.19.3.1.6&ISO||119364003^Serum specimen^SCT||||244001006^Antecubital fossa^SCT|||P^Patient^HL70369|2^mL&milliliter&UCUM||Serum Sample|||20150515|20150515101500
OBR|3|BAB1234-2^EHR^2.16.840.1.113883.19.3.2.3^ISO|BAB9700123-1^Lab^2.16.840.1.113883.19.3.1.6^ISO|67866-4^Babesia identification panel^LN|||20150913150000|||||||||121121121^^^^^^^^NPI&2.16.840.1.113883.4.6&ISO^L^^^NPI^NPI_Facility&2.16.840.1.113883.3.72.5.26&ISO^^^^^^^MD||||||20150913150000|||F||||||A68.1^Tick-borne relapsing fever^I10C|||||||||||||||||||
OBX|1|CWE|662-7^Microscopic observation [Identifier] in Unspecified specimen by Giemsa stain^LN||10828004^Positive^SCT||Negative|POS|||F|||20150515|||0204^May-Grunwald Giemsa stain^OBSMETHOD||20150516||||GHH Lab||9876543
SPM|1|BAB1234-2&EHR&2.16.840.1.113883.19.3.2.3&ISO^BAB9700122-2&Lab&2.16.840.1.113883.19.3.1.6&ISO||119364003^Serum specimen^SCT||||244001006^Antecubital fossa^SCT|||P^Patient^HL70369|2^mL&milliliter&UCUM||Serum sample|||20150515|20150515101500";
        #endregion

        #region STD Message 01
        private static string STD_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|STD_V1_CN_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~STD_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||STD_TC01^^^SendAppName&2.16.840.1.114222.GENv2&ISO||~^^^^^^S||19640502|F||2106-3^White^CDCREC|^^^48^77018^^^^48201^312800|||||||||||2135-2^Hispanic or Latino^CDCREC
OBR|1||STD_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20140227170100|||||||||||||||20140227170100|||F||||||10311^Syphilis, primary^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20140224||||||F
OBX|4|TS|77976-9^Illness End Date^LN||20140302||||||F
OBX|5|SN|77977-7^Illness Duration^LN||^6|d^day^UCUM|||||F
OBX|6|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|7|TS|77975-1^Diagnosis Date^LN||20140225||||||F
OBX|8|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
OBX|9|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|10|ST|77993-4^State Case Identifier^LN||2014LEV100000001||||||F
OBX|11|ST|77997-5^Legacy Case Identifier^LN||34022748S012014||||||F
OBX|12|SN|77998-3^Age at Case Investigation^LN||^49|a^year^UCUM|||||F
OBX|13|CWE|77982-7^Case Disease Imported Code^LN||PHC244^Indigenous, within jurisdiction^CDCPHINVS||||||F
OBX|14|CWE|77984-3^Country of Exposure^LN|1|USA^United States^ISO3166_1||||||F
OBX|15|CWE|77985-0^State or Province of Exposure^LN|1|48^Texas^FIPS5_2||||||F
OBX|16|ST|77986-8^City of Exposure^LN|1|Houston||||||F
OBX|17|ST|77987-6^County of Exposure^LN|1|Harris||||||F
OBX|18|CWE|77989-2^Transmission Mode^LN||417564009^Sexual transmission^SCT||||||F
OBX|19|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|20|CWE|77965-2^Immediate National Notifiable Condition^LN||N^No^HL70136||||||F
OBX|21|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F
OBX|22|ST|77969-4^Jurisdiction Code^LN||S01||||||F
OBX|23|CWE|48766-0^Reporting Source Type Code^LN||PHC1220^HIV Counseling and Testing Site^CDCPHINVS||||||F
OBX|24|ST|52831-5^Reporting Source Zip Code^LN||77009||||||F
OBX|25|ST|74549-7^Person Reporting to CDC-Name^LN||Smith, John ||||||F
OBX|26|ST|74548-9^Person Reporting to CDC-Phone Number^LN||(734)677-7777||||||F
OBX|27|ST|74547-1^Person Reporting to CDC-Email^LN||Smith@you.com||||||F
OBX|28|DT|77979-3^Case Investigation Start Date^LN||20140225||||||F
OBX|29|DT|77995-9^Date Reported^LN||20140225||||||F
OBX|30|TS|77972-8^Earliest Date Reported to County^LN||20140225||||||F
OBX|31|TS|77973-6^Earliest Date Reported to State^LN||20140225||||||F
OBX|32|SN|77991-8^MMWR Week^LN||^9||||||F
OBX|33|DT|77992-6^MMWR Year^LN||2014||||||F
OBX|34|DT|77994-2^Date CDC Was First Verbally Notified of This Case^LN||20140225||||||F
OBX|35|DT|77970-2^Date First Reported to PHD^LN||20140225||||||F
OBX|36|CWE|77966-0^Reporting State^LN||48^Texas^FIPS5_2||||||F
OBX|37|CWE|77967-8^Reporting County^LN||48201^Harris^FIPS6_4||||||F
OBX|38|CWE|77968-6^National Reporting Jurisdiction^LN||48^Texas^FIPS5_2||||||F
OBX|39|TX|77999-1^Comment^LN||<use any comment>||||||F
OBX|40|CWE|76691-5^Gender Identity^LN||PHC1490^Cisgender/Not transgender (finding)^CDCPHINVS||||||F
OBX|41|CWE|76690-7^Sexual Orientation^LN||20430005^Heterosexual^SCT||||||F
OBX|42|DT|STD099^Date of Initial Health Exam Associated with Case Report ""Health Event""^PHINQUESTION||20140225||||||F
OBX|43|CWE|STD102^Neurologic Manifestations^PHINQUESTION||PHC1475^Yes, Verified^CDCPHINVS||||||F
OBX|44|CWE|410478005^Ocular Manifestations^SCT||PHC1476^Yes, Verified^CDCPHINVS||||||F
OBX|45|CWE|PHC1472^Otic Manifestations^CDCPHINVS||N^No^HL70136||||||F
OBX|46|CWE|72083004^Late Clinical Manifestations^SCT||N^No^HL70136||||||F
OBX|47|DT|STD105^Treatment Date^PHINQUESTION||20140225||||||F
OBX|48|CWE|55277-8^HIV Status^LN||165816005^HIV positive^SCT||||||F
OBX|49|CWE|STD107^Had Sex with a Male within the Past 12 Months^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|50|CWE|STD108^Had Sex with a Female within the Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
OBX|51|CWE|STD109^Had Sex with an Anonymous Partner within the Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
OBX|52|CWE|STD110^Had Sex with a Person Known to Him/Her to be an ""Intravenous Drug User (IDU)"" within the Past 12 months^PHINQUESTION||NASK^Did Not Ask^NULLFL||||||F
OBX|53|CWE|STD111^Had Sex while Intoxicated and/or High on Drugs within the Past 12 months^PHINQUESTION||N^No^HL70136||||||F
OBX|54|CWE|STD112^Exchanged Drugs/Money for Sex within the Past 12 Months^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|55|CWE|STD113^Had Sex with a Person Who is Known to Her to Be an MSM within Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
OBX|56|CWE|STD114^Engaged in Injection Drug use Within Past 12 Months^PHINQUESTION||Y^Yes^HL70136||||||F
OBX|57|CWE|INV159^Detection Method^PHINQUESTION||20135006^Screening^SCT||||||F
OBX|58|CWE|STD115^Drugs Used^PHINQUESTION|2|2653^Cocaine^RXNORM||||||F
OBX|59|CWE|STD116^Drugs Used Indicator^PHINQUESTION|2|Y^Yes^HL70136||||||F
OBX|60|CWE|STD115^Drugs Used^PHINQUESTION|3|PHC1308^Other Drugs Used^CDCPHINVS||||||F
OBX|61|CWE|STD116^Drugs Used Indicator^PHINQUESTION|3|Y^Yes^HL70136||||||F
OBX|62|CWE|STD115^Drugs Used^PHINQUESTION|4|PHC1160^Erectile dysfunction medications^CDCPHINVS||||||F
OBX|63|CWE|STD116^Drugs Used Indicator^PHINQUESTION|4|Y^Yes^HL70136||||||F
OBX|64|CWE|STD117^Previous STD History^PHINQUESTION||N^No^HL70136||||||F
OBX|65|CWE|STD118^Been Incarcerated within the Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
OBX|66|CWE|STD119^Have You Met Sex Partners through the Internet in the Last 12 Months^PHINQUESTION||N^No^HL70136||||||F
OBX|67|SN|STD120^Total Number of Sex Partners in the Last 12 months^PHINQUESTION||^1||||||F
OBX|68|CWE|STD121^Clinician-Observed Lesion(s) Indicative of Syphilis^PHINQUESTION||76784001^Vagina^SCT||||||F
OBX|69|CWE|STD999^Unknown Number of Sex Partners in Last 12 Months^PHINQUESTION||N^No^HL70136||||||F
OBX|70|CWE|INV892^HIV Status Documented Through eHARS Record Search^PHINQUESTION||UNK^Unknown^NULLFL||||||F
OBX|71|ST|INV893^eHARS State no(i.e.State Number)^PHINQUESTION||58968||||||F
OBX|72|CWE|INV894^Transmission Category(eHARS)^PHINQUESTION||PHC1143^Adult heterosexual contact^CDCPHINVS||||||F
OBX|73|CWE|INV895^Case Sampled for Enhanced Investigation^PHINQUESTION||N^No^HL70136||||||F
OBX|74|CWE|INV290^Test Type^PHINQUESTION|5|24110-9^T pallidum Ab Ser QI IA^LN||||||F
OBX|75|CWE|INV291^Test Result^PHINQUESTION|5|10828004^Positive(qualifier value)^SCT||||||F
OBX|76|DT|82772-5^Date of lab result^LN|5|20140225||||||F
OBX|77|CWE|31208-2^Specimen source^LN|5|PHC1151^Blood/Serum^CDCPHINVS||||||F
OBX|78|DT|33882-2^Specimen Collection Date^LN|5|20140225||||||F
OBR|2||SYP9700123-1^Lab^2.16.840.1.113883.19.3.1.6^ISO|71793-4^Treponema pallidum AB[Titer} in Serum or Plasma by Agglutination^LN|||20140225151000||||||||||||||||||F
OBX|1|SN|71793-4^Treponema pallidum AB [Titer} in Serum or Plasma by Agglutination^LN|1|^1^:^64|{titer}^titer^UCUM|<1:8|H|||F|||20140225|||0014^Agglutination^OBSMETHOD||20140225
SPM|1|||119364003^Serum specimen^SCT||||368149001^Right Elbow^SCT|||P^Patient^HL70369|2^mL&milliliter&UCUM||Serum Sample|||20140225|20140225
";
        #endregion 

        #region CS Message 01
        private static string CS_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||CONSYPH_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130801|F||2054-5^Black or African American^CDCREC|^^^22^71301^^^^22079|||||||||||2186-5^Not Hispanic or Latino^CDCREC
NK1|1||MTH^Mother^HL70063|^^^22^71301^USA^^^22079||||||||||S^Single^HL70002||19940113||||||||||||2186-5^Not Hispanic or Latino^CDCREC|||||||2054-5^Black or African American^CDCREC
OBR|1||CONSYPH_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130825170100|||||||||||||||20130825170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
OBX|3|TS|11368-8^Date of Illness Onset^LN||20130801||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20130801||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
OBX|6|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|7|ST|77993-4^State Case Identifier^LN||<use state-assigned value>||||||F
OBX|8|ST|77997-5^Legacy Case Identifier^LN||61528936181002006||||||F
OBX|9|SN|77998-3^Age at Case Investigation^LN||^1|d^day^UCUM|||||F
OBX|10|CWE|77982-7^Case Disease Imported Code^LN||PHC244^Indigenous, within jurisdiction^CDCPHINVS||||||F
OBX|11|CWE|77984-3^Country of Exposure^LN|1|USA^United States^ISO3166_1||||||F
OBX|12|CWE|77985-0^State or Province of Exposure^LN|1|22^Louisiana^FIPS5_2||||||F
OBX|13|ST|77986-8^City of Exposure^LN|1|Alexandria||||||F
OBX|14|ST|77987-6^County of Exposure^LN|1|Rapides||||||F
OBX|15|CWE|77989-2^Transmission Mode^LN||417409004^Transplacental transmission^SCT||||||F
OBX|16|CWE|77990-0^Case Class Status Code^LN||2931005^Probable diagnosis^SCT||||||F
OBX|17|CWE|77965-2^Immediate National Notifiable Condition^LN||N^No^HL70136||||||F
OBX|18|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F
OBX|19|ST|77969-4^Jurisdiction Code^LN||S01||||||F
OBX|20|CWE|48766-0^Reporting Source Type Code^LN||1^Hospital^HL70406||||||F
OBX|21|ST|52831-5^Reporting Source Zip Code^LN||71301||||||F
OBX|22|ST|74549-7^Person Reporting to CDC-Name^LN||Smith, John||||||F
OBX|23|ST|74548-9^Person Reporting to CDC-Phone Number^LN||(734)677-7777||||||F
OBX|24|DT|77979-3^Case Investigation Start Date^LN||20130801||||||F
OBX|25|DT|77995-9^Date Reported^LN||20130801||||||F
OBX|26|TS|77972-8^Earliest Date Reported to County^LN||20130802||||||F
OBX|27|TS|77973-6^Earliest Date reported to State^LN||20130806||||||F
OBX|28|SN|77991-8^MMWR Week^LN||^31||||||F
OBX|29|DT|77992-6^MMWR Year^LN||2013||||||F
OBX|30|DT|77970-2^Date First Reported to PHD^LN||20130802||||||F
OBX|31|CWE|77966-0^Reporting State^LN||22^Louisiana^FIPS5_2||||||F
OBX|32|CWE|77967-8^Reporting County^LN||22079^Rapides^FIPS6_4||||||F
OBX|33|CWE|77968-6^National Reporting Jurisdiction^LN||22^Louisiana^FIPS5_2||||||F
OBX|34|NM|75201-4^Number of Pregnancies^LN||1||||||F
OBX|35|NM|75202-2^Number of Total Live Births^LN||1||||||F
OBX|36|DT|75203-0^Last Menstrual Period^LN||99999999||||||F
OBX|37|DT|75200-6^Date of First Prenatal Visit^LN||20130315||||||F
OBX|38|CWE|75204-8^Prenatal Visit Indicator^LN||Y^Yes^HL70136||||||F
OBX|39|CWE|75163-6^Trimester of First Prenatal Visit^LN||255247007^Second trimester^SCT||||||F
OBX|40|CWE|75179-2^Mother's HIV Status During Pregnancy^LN||165815009^HIV negative^SCT||||||F
OBX|41|CWE|75180-0^Mother's Clinical Stage of Syphilis During Pregnancy^LN||186867005^early latent^SCT||||||F
OBX|42|CWE|75181-8^Mother's Surveillance Stage of Syphilis During Pregnancy^LN||PHC1506^Syphilis, early non-primary, non-secondary^CDCPHINVS||||||F
OBX|43|DT|75182-6^Date When Mother Received Her First Dose of Benzathine Penicillin^LN||20130226||||||F
OBX|44|CWE|75183-4^Trimester in Which Mother Received Her First Dose of Benzathine Penicillin^LN||255246003^First Trimester^SCT||||||F
OBX|45|CWE|75184-2^Mother's Treatment^LN||PHC1278^2.4 M units benzathine penicillin^CDCPHINVS||||||F
OBX|46|CWE|75185-9^Appropriate Serologic Reponse^LN||PHC1282^No, inappropriate response: evidence of treatment failure or reinfection^CDCPHINVS||||||F
OBX|47|CWE|75164-4^Non-treponemal Test or Treponemal Test at First Prenatal Visit^LN||Y^Yes^HL70136||||||F
OBX|48|CWE|75165-1^Non-treponemal Test or Treponemal Test at 28-32 Weeks Gestation^LN||Y^Yes^HL70136||||||F
OBX|49|CWE|75166-9^Non-treponemal Test or Treponemal Test at Delivery^LN||Y^Yes^HL70136||||||F
OBX|50|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|2|STD150^Mother's First Non-treponemal Test Finding/Information^PHINQUESTION||||||F
OBX|51|CWE|INV290^Test Type^PHINQUESTION|2|5292-8^VDRL (Serum)^LN||||||F
OBX|52|CWE|INV291^Test Result^PHINQUESTION|2|11214006^Reactive^SCT||||||F
OBX|53|CWE|STD123^Nontreponemal Serologic syphilis test result^PHINQUESTION|2|STD8^1:8^CDCPHINVS||||||F
OBX|54|DT|82772-5^Date of lab result^LN|2|20130204||||||F
OBX|55|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|3|STD151^Mother's Most Recent Non-treponemal Test Finding/Information^PHINQUESTION||||||F
OBX|56|CWE|INV290^Test Type^PHINQUESTION|3|20507-0^RPR^LN||||||F
OBX|57|CWE|INV291^Test Result^PHINQUESTION|3|11214006^Reactive^SCT||||||F
OBX|58|CWE|STD123^Nontreponemal Serologic syphilis test result^PHINQUESTION|3|STD8^1:8^CDCPHINVS||||||F
OBX|59|DT|82772-5^Date of lab result^LN|3|20130808||||||F
OBX|60|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|4|STD152^Mother's First Treponemal Test Finding^PHINQUESTION||||||F
OBX|61|CWE|INV290^Test Type^PHINQUESTION|4|24110-9^Treponema Pallidum - Enzyme immunoassay (EIA)^LN||||||F
OBX|62|CWE|INV291^Test Result^PHINQUESTION|4|11214006^Reactive^SCT||||||F
OBX|63|DT|82772-5^Date of lab result^LN|4|20130204||||||F
OBX|64|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|5|STD153^Mother's Most Recent Treponemal Test Finding^PHINQUESTION||||||F
OBX|65|CWE|INV290^Test Type^PHINQUESTION|5|PHC1306^Other Treponemal Test Type^CDCPHINVS||||||F
OBX|66|CWE|INV291^Test Result^PHINQUESTION|5|11214006^Reactive^SCT||||||F
OBX|67|DT|82772-5^Date of lab result^LN|5|20130801||||||F
OBX|68|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|6|STD154^Infant's Non-treponemal Test Finding^PHINQUESTION||||||F
OBX|69|CWE|INV290^Test Type^PHINQUESTION|6|20507-0^RPR^LN||||||F
OBX|70|CWE|INV291^Test Result^PHINQUESTION|6|11214006^Reactive^SCT||||||F
OBX|71|CWE|STD123^Nontreponemal Serologic syphilis test result^PHINQUESTION|6|STD1^1:1^CDCPHINVS||||||F
OBX|72|DT|82772-5^Date of lab result^LN|6|20130801||||||F
OBX|73|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|7|STD155^Infant's Treponemal Test Finding^PHINQUESTION||||||F
OBX|74|CWE|INV290^Test Type^PHINQUESTION|7|24110-9^Treponema Pallidum - Enzyme immunoassay (EIA)^LN||||||F
OBX|75|CWE|INV291^Test Result^PHINQUESTION|7|11214006^Reactive^SCT||||||F
OBX|76|DT|82772-5^Date of lab result^LN|7|20130801||||||F
OBX|77|CWE|75186-7^Vital Status^LN||438949009^Alive^SCT||||||F
OBX|78|SN|56056-5^Birth Weight^LN||^2623|g^gram^UCUM|||||F
OBX|79|SN|57714-8^Gestational Age^LN||^36|wk^week^UCUM|||||F
OBX|80|CWE|75193-3^Clinical Signs of Congenital Syphilis^SCT||84387000^no signs/asymptomatic^SCT||||||F
OBX|81|CWE|75194-1^Long Bone X-rays for Infant^LN||PHC1288^Yes, no signs of CS^CDCPHINVS||||||F
OBX|82|CWE|75192-5^Darkfield Exam, DFA, or Special Stain Test Findings^LN||373121007^Test Not done^SCT||||||F
OBX|83|CWE|LP48341-9^CSF WBC Count^LN||373121007^Test Not done^SCT||||||F
OBX|84|CWE|LP69956-8^CSF Protein Level^LN||373121007^Test Not done^SCT||||||F
OBX|85|CWE|75195-8^CSF VDRL Test Finding^LN||373121007^Test Not done^SCT||||||F
OBX|86|CWE|75197-4^Infant Treated^LN||PHC1296^Yes,with other treatment^CDCPHINVS||||||F
OBX|87|CWE|75207-1^Stillbirth Indicator^LN||N^No^HL70136||||||F
OBR|2|CSYP23456-1^EHR^2.16.840.1.113883.19.3.2.3^ISO|CSYP9700123-1^Lab^2.16.840.1.113883.19.3.1.6^ISO|31147-2^Reagin Ab [Titer] in Serum by RPR^LN|||20130801000000|||||||||121121121^^^^^^^^NPI&2.16.840.1.113883.4.6&ISO^L^^^NPI^NPI_Facility&2.16.840.1.113883.3.72.5.26&ISO^^^^^^^MD||||||20130801000000|||F||||||A53.9^Syphilis, unspecified^I10C
OBX|1|SN|31147-2^Reagin Ab [Titer] in Serum by RPR^LN||^1^:^4|{titer}^titer^UCUM|<1:1|H|||F|||20130801|||0021^Rapid Agglutination^OBSMETHOD||20130801||||GHH Lab^L^^^^CLIA&2.16.840.1.113883.19.4.6&ISO^XX^^^1236||9876543^^^^^^^^NPI&1.3.6.1.4.1.562.2.4.1.43&ISO^^^^NPI
NTE|1|L|Response to therapy is indicated by a > or =4-fold decrease in titer between pre and post treatment samples. However, a significant decrease in RPR titers may not occur for months to years following treatment, some patients may show persistent, low-level titers (e.g. serofast) despite adequate therapy.
SPM|1|CSYP234561-1&EHR&2.16.840.1.113883.19.3.2.3&ISO^CSYP97001222-1&Lab&2.16.840.1.113883.19.3.1.6&ISO||119364003^Serum specimen^SCT||||368149001^Right Elbow^SCT|||P^Patient^HL70369||||||20130801|20130801";
        #endregion 

        [Fact]
        public void Pertussis_Basic_Conversion_01()
        {
            string message = @"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20150630120030.1234-0500||ORU^R01^ORU_R01|PERT_V1_TM_TC06|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Pertussis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||Pertussis_TC06^^^CDCNBS01&2.16.840.1.114222.GENv2&ISO||~^^^^^^S||20100213|F||2054-5^Black or African American^CDCREC|^^Albuquerque^35^87121^^^^35001|||||||||||2135-2^Hispanic or Latino^CDCREC
OBR|1||Pertussis_TC06^SendAppName^2.16.840.1.114222^ISO|68991-9^Epidemiologic Information^LN|||20150302170100|||||||||||||||20150302170100|||F||||||10190^Pertussis^NND
OBX|1|ST|32624-9^Other Race Text^LN||Multiracial||||||F";

            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());
            ConversionResult result = converter.Convert(message, "1234");

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);
            Assert.Equal("Pertussis_MMG_V1.0", result.Profile);
            Assert.Equal("Pertussis", result.Condition);
            Assert.Equal("10190", result.ConditionCode);

            Assert.True(attributes.ContainsKey("other_race_text"));
            Assert.Equal("Multiracial", attributes["other_race_text"]);

            Assert.True(attributes.ContainsKey("date_first_electronically_submitted"));

            Assert.Equal("Pertussis_TC06", attributes["local_subject_id"]);
            Assert.Equal("Pertussis_TC06", attributes["local_record_id"]);
            //Assert.Equal("20100213", attributes["birth_date"]);
            Assert.Equal("Pertussis", attributes["condition_code"]);
            Assert.Equal("10190", attributes["condition_code__code"]);
            Assert.Equal("87121", attributes["subject_address_zip_code"]);
            Assert.Equal("35", attributes["subject_address_state__code"]);
            Assert.Equal("35001", attributes["subject_address_county__code"]);
            Assert.Equal("F", attributes["subjects_sex__code"]);
            Assert.Equal("F", attributes["notification_result_status__code"]);

            Assert.False(attributes.ContainsKey("date_reported"));
            Assert.False(attributes.ContainsKey("mmwr_week"));
            Assert.False(attributes.ContainsKey("mmwr_year"));
            Assert.False(attributes.ContainsKey("national_reporting_jurisdiction"));
        }

        [Fact]
        public void EscapeSequences_Conversion_01()
        {
            string message = @"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20150630120030.1234-0500||ORU^R01^ORU_R01|PERT_V1_TM_TC06|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~Pertussis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||Pertussis_TC32^^^CDCNBS01&2.16.840.1.114222.GENv2&ISO||~^^^^^^S||20100213|F||2054-5^Black or African American^CDCREC|^^Albuquerque^35^87121^^^^35001|||||||||||2135-2^Hispanic or Latino^CDCREC
OBR|1||Pertussis_TC32^SendAppName^2.16.840.1.114222^ISO|68991-9^Epidemiologic Information^LN|||20150302170100|||||||||||||||20150302170100|||F||||||10190^Pertussis^NND
OBX|1|ST|32624-9^Other Race Text^LN||Black \T\ White||||||F";

            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());
            ConversionResult result = converter.Convert(message, "1234");

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);

            Assert.True(attributes.ContainsKey("other_race_text"));
            Assert.Equal("Black & White", attributes["other_race_text"]);
        }

        [Fact]
        public void TB_Basic_Conversion_01()
        {
            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());
            
            string transactionId = "1234";

            ConversionResult result = converter.Convert(TB_MESSAGE_01, transactionId);

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);

            Assert.True(attributes.ContainsKey("date_first_electronically_submitted"));

            Assert.True(attributes.ContainsKey("patient_address_city"));
            Assert.True(attributes.ContainsKey("census_tract_of_case_patient_residence"));
            
            Assert.True(attributes.ContainsKey("transaction_id"));
            Assert.Equal(transactionId, attributes["transaction_id"]);

            Assert.Equal("Atlanta", attributes["patient_address_city"]);
            Assert.Equal("13121009101", attributes["census_tract_of_case_patient_residence"]);

            Assert.Equal("TB_TC_01", attributes["local_subject_id"]);
            Assert.Equal("TB_TC_01", attributes["local_record_id"]);
            Assert.Equal("1978-03-01T00:00:00", ((DateTime)attributes["birth_date"]).ToString("s"));
            Assert.Equal("Tuberculosis", attributes["condition_code"]);
            Assert.Equal("10220", attributes["condition_code__code"]);
            Assert.Equal("30313", attributes["subject_address_zip_code"]);
            Assert.Equal("13", attributes["subject_address_state__code"]);
            Assert.Equal("13089", attributes["subject_address_county__code"]);
            Assert.Equal("M", attributes["subjects_sex__code"]);
            Assert.Equal("F", attributes["notification_result_status__code"]);

            Assert.Equal("No", attributes["case_meets_binational_reporting_criteria"]);
            Assert.Equal("N", attributes["case_meets_binational_reporting_criteria__code"]);

            Assert.Equal("CHN", attributes["country_of_birth__code"]);
            Assert.Equal("CHINA", attributes["country_of_birth"]);
            Assert.Equal("ISO3166_1", attributes["country_of_birth__code_system"]);

            Assert.Equal("USA", attributes["country_of_usual_residence__code"]);
            Assert.Equal("UNITED STATES", attributes["country_of_usual_residence"]);
            Assert.Equal("ISO3166_1", attributes["country_of_usual_residence__code_system"]);

            Assert.Equal("N", attributes["subject_died__code"]);
            Assert.Equal("No", attributes["subject_died"]);
            Assert.Equal("HL70136", attributes["subject_died__code_system"]);
            
            Assert.Equal("40", attributes["age_at_case_investigation"].ToString());

            Assert.Equal("a", attributes["age_unit_at_case_investigation__code"]);
            Assert.Equal("year [time]", attributes["age_unit_at_case_investigation"]);
            Assert.Equal("UCUM", attributes["age_unit_at_case_investigation__code_system"]);

            Assert.Equal("N", attributes["pregnancy_status__code"]);
            Assert.Equal("No", attributes["pregnancy_status"]);

            Assert.Equal("2018-02-02T00:00:00", ((DateTime)attributes["diagnosis_date"]).ToString("s"));

            Assert.Equal("N", attributes["hospitalized__code"]);
            Assert.Equal("No", attributes["hospitalized"]);

            Assert.Equal("Solo, Han", attributes["person_reporting_to_cdc_name"]);

            Assert.Equal("hsolo@ga.dph.gov", attributes["person_reporting_to_cdc_email"]);

            Assert.Equal("416380006", attributes["transmission_mode__code"]);
            Assert.Equal("Airborne transmission", attributes["transmission_mode"]);
            Assert.Equal("SCT", attributes["transmission_mode__code_system"]);

            Assert.Equal("13", attributes["reporting_state__code"]);
            Assert.Equal("Georgia", attributes["reporting_state"]);

            Assert.Equal("410605003", attributes["case_class_status_code__code"]);
            Assert.Equal("Confirmed present", attributes["case_class_status_code"]);

            // TODO: Fix this... repeating
            //Assert.Equal("2034-7", attributes["detailed_race__code"]);
            //Assert.Equal("Chinese", attributes["detailed_race"]);

            /*OBX|25|CWE|INV1116^Initial Reason for Evaluation^PHINQUESTION||PHC681^Contact investigation^CDCPHINVS||||||F
OBX|26|ST|INV1108^City or County Case Number^PHINQUESTION||2018GAFULT04583||||||F
OBX|27|CWE|76689-9^Birth Sex^LN||M^Male^HL70001||||||F
OBX|28|CWE|INV1109^Previously Counted Case^PHINQUESTION||PHC659^Verified Case: Counted by another US area^CDCPHINVS||||||F
OBX|29|ST|INV1110^Previously Reported State Case Number^PHINQUESTION||2018AL987436179||||||F
OBX|30|CWE|INV1112^Inside City Limits^PHINQUESTION||Y^Yes^HL70136||||||F
             */
            //Assert.Equal("REPLACEME", attributes["REPLACEME"]);
            //Assert.Equal("REPLACEME", attributes["REPLACEME"]);

            //Assert.Equal("REPLACEME", attributes["REPLACEME"]);
            //Assert.Equal("REPLACEME", attributes["REPLACEME"]);

        }

        [Fact]
        public void TB_Basic_Conversion_01_RepeatingBlocks()
        {
            // Special test method dedicated to checking repeating blocks

            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());
            ConversionResult result = converter.Convert(TB_MESSAGE_01, "1234");

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            object rvctRiskFactorBlockObject = attributes["rvct_and_tbliss_risk_factor_repeating_group"];
            object rvctInitialDrugRegimenBlockObject = attributes["rvct_initial_drug_regimen_repeating_block"];

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);

            
            Assert.IsType<JArray>(rvctRiskFactorBlockObject);
            JArray rvctRiskFactorBlock = (JArray)rvctRiskFactorBlockObject;
            Assert.Equal(15, rvctRiskFactorBlock.Count);

            Assert.IsType<JArray>(rvctInitialDrugRegimenBlockObject);
            JArray rvctInitialDrugRegimenBlock = (JArray)rvctInitialDrugRegimenBlockObject;
            Assert.Equal(23, rvctInitialDrugRegimenBlock.Count);

            // First item in block
            JObject rvctRiskFactorBlockFirstItem = (JObject)rvctRiskFactorBlock[0];

            Assert.Equal("Diabetic at Diagnostic Evaluation", rvctRiskFactorBlockFirstItem["patient_epidemiological_risk_factors"]);
            Assert.Equal("PHC2098", rvctRiskFactorBlockFirstItem["patient_epidemiological_risk_factors__code"]);

            Assert.Equal("No", rvctRiskFactorBlockFirstItem["patient_epidemiological_risk_factors_indicator"]);
            Assert.Equal("N", rvctRiskFactorBlockFirstItem["patient_epidemiological_risk_factors_indicator__code"]);

            // Second item in block
            JObject rvctRiskFactorBlockSecondItem = (JObject)rvctRiskFactorBlock[1];

            Assert.Equal("End-Stage Renal Disease", rvctRiskFactorBlockSecondItem["patient_epidemiological_risk_factors"]);
            Assert.Equal("46177005", rvctRiskFactorBlockSecondItem["patient_epidemiological_risk_factors__code"]);

            Assert.Equal("No", rvctRiskFactorBlockSecondItem["patient_epidemiological_risk_factors_indicator"]);
            Assert.Equal("N", rvctRiskFactorBlockSecondItem["patient_epidemiological_risk_factors_indicator__code"]);

            // Third item in block
            JObject rvctRiskFactorBlockThirdItem = (JObject)rvctRiskFactorBlock[2];

            Assert.Equal("Heavy Alcohol Use in the Past 12 Months", rvctRiskFactorBlockThirdItem["patient_epidemiological_risk_factors"]);
            Assert.Equal("86933000", rvctRiskFactorBlockThirdItem["patient_epidemiological_risk_factors__code"]);

            Assert.Equal("Yes", rvctRiskFactorBlockThirdItem["patient_epidemiological_risk_factors_indicator"]);
            Assert.Equal("Y", rvctRiskFactorBlockThirdItem["patient_epidemiological_risk_factors_indicator__code"]);

            // Last item in block
            JObject rvctRiskFactorBlockLastItem = (JObject)rvctRiskFactorBlock[14];

            Assert.Equal("Viral hepatitis (any type)", rvctRiskFactorBlockLastItem["patient_epidemiological_risk_factors"]);
            Assert.Equal("3738000", rvctRiskFactorBlockLastItem["patient_epidemiological_risk_factors__code"]);

            Assert.Equal("Unknown", rvctRiskFactorBlockLastItem["patient_epidemiological_risk_factors_indicator"]);
            Assert.Equal("UNK", rvctRiskFactorBlockLastItem["patient_epidemiological_risk_factors_indicator__code"]);
        }

        [Fact]
        public void Babesiosis_Basic_Conversion_01()
        {
            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());

            string transactionId = "1234";

            ConversionResult result = converter.Convert(BABESIOSIS_MESSAGE_01, transactionId);

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);

            Assert.True(attributes.ContainsKey("date_first_electronically_submitted"));

            Assert.False(attributes.ContainsKey("patient_address_city"));
            Assert.False(attributes.ContainsKey("census_tract_of_case_patient_residence"));

            Assert.True(attributes.ContainsKey("transaction_id"));
            Assert.Equal(transactionId, attributes["transaction_id"]);

            Assert.Equal("BABESIOSIS_TC01", attributes["local_subject_id"]);
            Assert.Equal("BABESIOSIS_TC01", attributes["local_record_id"]);
            Assert.Equal("1937-03-13T00:00:00", ((DateTime)attributes["birth_date"]).ToString("s"));
            Assert.Equal("Babesiosis", attributes["condition_code"]);
            Assert.Equal("12010", attributes["condition_code__code"]);
            Assert.Equal("6320", attributes["subject_address_zip_code"]);
            Assert.Equal("09", attributes["subject_address_state__code"]);
            Assert.Equal("09011", attributes["subject_address_county__code"]);
            Assert.Equal("M", attributes["subjects_sex__code"]);
            Assert.Equal("F", attributes["notification_result_status__code"]);

            Assert.Equal("USA", attributes["country_of_birth__code"]);
            Assert.Equal("UNITED STATES", attributes["country_of_birth"]);
            Assert.Equal("ISO3166_1", attributes["country_of_birth__code_system"]);

            Assert.Equal("USA", attributes["country_of_usual_residence__code"]);
            Assert.Equal("UNITED STATES", attributes["country_of_usual_residence"]);
            Assert.Equal("ISO3166_1", attributes["country_of_usual_residence__code_system"]);

            Assert.Equal("UNK", attributes["subject_died__code"]);
            Assert.Equal("Unknown", attributes["subject_died"]);
            Assert.Equal("NULLFL", attributes["subject_died__code_system"]);

            Assert.Equal("78", attributes["age_at_case_investigation"].ToString());

            Assert.Equal("a", attributes["age_unit_at_case_investigation__code"]);
            Assert.Equal("year [time]", attributes["age_unit_at_case_investigation"]);
            Assert.Equal("UCUM", attributes["age_unit_at_case_investigation__code_system"]);

            Assert.False(attributes.ContainsKey("pregnancy_status"));
            Assert.False(attributes.ContainsKey("pregnancy_status__code"));

            Assert.Equal("2015-05-15T00:00:00", ((DateTime)attributes["diagnosis_date"]).ToString("s"));

            Assert.Equal("N", attributes["hospitalized__code"]);
            Assert.Equal("No", attributes["hospitalized"]);

            Assert.Equal("Smith, John", attributes["person_reporting_to_cdc_name"]);

            Assert.Equal("Smith@you.com", attributes["person_reporting_to_cdc_email"]);

            Assert.Equal("09", attributes["reporting_state__code"]);
            Assert.Equal("Connecticut", attributes["reporting_state"]);
            Assert.Equal("FIPS5_2", attributes["reporting_state__code_system"]);
        }

        [Fact]
        public void CS_Basic_Conversion_01()
        {
            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());

            string transactionId = "1234";

            ConversionResult result = converter.Convert(CS_MESSAGE_01, transactionId);

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);

            #region Generic HL7v2 and Generic MMG fields
            Assert.True(attributes.ContainsKey("date_first_electronically_submitted"));

            Assert.False(attributes.ContainsKey("patient_address_city"));
            Assert.False(attributes.ContainsKey("census_tract_of_case_patient_residence"));

            Assert.True(attributes.ContainsKey("transaction_id"));
            Assert.Equal(transactionId, attributes["transaction_id"]);

            Assert.Equal("CONSYPH_TC01", attributes["local_subject_id"]);
            Assert.Equal("CONSYPH_TC01", attributes["local_record_id"]);
            Assert.Equal("2013-08-01T00:00:00", ((DateTime)attributes["birth_date"]).ToString("s"));

            Assert.Equal("2013-08-01T00:00:00", ((DateTime)attributes["diagnosis_date"]).ToString("s"));

            Assert.Equal("2013-08-01T00:00:00", ((DateTime)attributes["date_of_illness_onset"]).ToString("s"));

            Assert.Equal("N", attributes["hospitalized__code"]);
            Assert.Equal("No", attributes["hospitalized"]);

            Assert.Equal("N", attributes["subject_died__code"]);
            Assert.Equal("No", attributes["subject_died"]);

            Assert.Equal("Smith, John", attributes["person_reporting_to_cdc_name"]);

            //Assert.Equal("61528936181002006", attributes["legacy_case_identifer"]);
            
            Assert.Equal("22", attributes["reporting_state__code"]);
            Assert.Equal("Louisiana", attributes["reporting_state"]);
            Assert.Equal("FIPS5_2", attributes["reporting_state__code_system"]);
            #endregion

            // CS-specific fields start here

            Assert.Equal(1L, attributes["number_of_pregnancies"]);
            Assert.Equal(1L, attributes["number_of_total_live_births"]);
            Assert.Equal("99999999", attributes["last_menstrual_period"]);

            Assert.Equal("2013-03-15T00:00:00", ((DateTime)attributes["date_of_first_prenatal_visit"]).ToString("s"));

            Assert.Equal("Y", attributes["prenatal_visit_indicator__code"]);
            Assert.Equal("Yes", attributes["prenatal_visit_indicator"]);
            Assert.Equal("HL70136", attributes["prenatal_visit_indicator__code_system"]);

            Assert.Equal("255247007", attributes["trimester_of_first_prenatal_visit__code"]);
            Assert.Equal("Second trimester", attributes["trimester_of_first_prenatal_visit"]);
            Assert.Equal("SCT", attributes["trimester_of_first_prenatal_visit__code_system"]);

            Assert.Equal("165815009", attributes["mothers_hiv_status_during_pregnancy__code"]);
            Assert.Equal("HIV negative", attributes["mothers_hiv_status_during_pregnancy"]);
            Assert.Equal("SCT", attributes["mothers_hiv_status_during_pregnancy__code_system"]);

            Assert.Equal("186867005", attributes["mothers_clinical_stage_of_syphilis_during_pregnancy__code"]);
            Assert.Equal("early latent", attributes["mothers_clinical_stage_of_syphilis_during_pregnancy"]);
            Assert.Equal("SCT", attributes["mothers_clinical_stage_of_syphilis_during_pregnancy__code_system"]);

            Assert.Equal("PHC1506", attributes["mothers_surveillance_stage_of_syphilis_during_pregnancy__code"]);
            Assert.Equal("Syphilis, early non-primary, non-secondary", attributes["mothers_surveillance_stage_of_syphilis_during_pregnancy"]);
            Assert.Equal("CDCPHINVS", attributes["mothers_surveillance_stage_of_syphilis_during_pregnancy__code_system"]);

            Assert.Equal("2013-02-26T00:00:00", ((DateTime)attributes["date_when_mother_received_her_first_dose_of_benzathine_penicillin"]).ToString("s"));

            Assert.Equal("255246003", attributes["trimester_in_which_mother_received_her_first_dose_of_benzathine_penicillin__code"]);
            Assert.Equal("First Trimester", attributes["trimester_in_which_mother_received_her_first_dose_of_benzathine_penicillin"]);
            Assert.Equal("SCT", attributes["trimester_in_which_mother_received_her_first_dose_of_benzathine_penicillin__code_system"]);

            Assert.Equal("PHC1278", attributes["mothers_treatment__code"]);
            Assert.Equal("2.4 M units benzathine penicillin", attributes["mothers_treatment"]);
            Assert.Equal("CDCPHINVS", attributes["mothers_treatment__code_system"]);

            Assert.Equal("PHC1282", attributes["appropriate_serologic_reponse__code"]);
            Assert.Equal("No, inappropriate response: evidence of treatment failure or reinfection", attributes["appropriate_serologic_reponse"]);
            Assert.Equal("CDCPHINVS", attributes["appropriate_serologic_reponse__code_system"]);

            Assert.Equal("Y", attributes["non_treponemal_test_or_treponemal_test_at_first_prenatal_visit__code"]);
            Assert.Equal("Yes", attributes["non_treponemal_test_or_treponemal_test_at_first_prenatal_visit"]);
            Assert.Equal("HL70136", attributes["non_treponemal_test_or_treponemal_test_at_first_prenatal_visit__code_system"]);

            Assert.Equal("Y", attributes["non_treponemal_test_or_treponemal_test_at_28_32_weeks_gestation__code"]);
            Assert.Equal("Yes", attributes["non_treponemal_test_or_treponemal_test_at_28_32_weeks_gestation"]);
            Assert.Equal("HL70136", attributes["non_treponemal_test_or_treponemal_test_at_28_32_weeks_gestation__code_system"]);

            Assert.Equal("Y", attributes["non_treponemal_test_or_treponemal_test_at_delivery__code"]);
            Assert.Equal("Yes", attributes["non_treponemal_test_or_treponemal_test_at_delivery"]);
            Assert.Equal("HL70136", attributes["non_treponemal_test_or_treponemal_test_at_delivery__code_system"]);

            // repeating blocks skipped - see adv test

            Assert.Equal("438949009", attributes["vital_status__code"]);
            Assert.Equal("Alive", attributes["vital_status"]);
            Assert.Equal("SCT", attributes["vital_status__code_system"]);

            Assert.Equal(2623L, attributes["birth_weight"]);

            Assert.Equal(36L, attributes["gestational_age"]);

            // TODO: Fix - this is repeating
            //Assert.Equal("84387000", attributes["clinical_signs_of_congenital_syphilis__code"]); 
            //Assert.Equal("no signs/asymptomatic", attributes["clinical_signs_of_congenital_syphilis"]);
            //Assert.Equal("SCT", attributes["clinical_signs_of_congenital_syphilis__code_system"]);

            Assert.Equal("PHC1288", attributes["long_bone_x_rays_for_infant__code"]);
            Assert.Equal("Yes, no signs of CS", attributes["long_bone_x_rays_for_infant"]);
            Assert.Equal("CDCPHINVS", attributes["long_bone_x_rays_for_infant__code_system"]);
        }

        [Fact]
        public void STD_v1_0_Basic_Conversion_01()
        {
            HL7v2ToJsonConverter converter = new HL7v2ToJsonConverter(new InMemoryMmgService());

            string transactionId = "1234";

            ConversionResult result = converter.Convert(STD_MESSAGE_01, transactionId);

            string json = result.Json;

            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.Equal(0, result.Errors);
            Assert.Equal(0, result.Warnings);
            Assert.True(result.IsSuccess);

            #region Generic HL7v2 and Generic MMG fields
            Assert.True(attributes.ContainsKey("date_first_electronically_submitted"));

            Assert.False(attributes.ContainsKey("patient_address_city"));
            Assert.True(attributes.ContainsKey("census_tract_of_case_patient_residence"));

            Assert.True(attributes.ContainsKey("transaction_id"));
            Assert.Equal(transactionId, attributes["transaction_id"]);

            Assert.Equal("STD_TC01", attributes["local_subject_id"]);
            Assert.Equal("STD_TC01", attributes["local_record_id"]);
            Assert.Equal("1964-05-02T00:00:00", ((DateTime)attributes["birth_date"]).ToString("s"));

            Assert.Equal("2014-02-25T00:00:00", ((DateTime)attributes["diagnosis_date"]).ToString("s"));

            Assert.Equal("2014-02-24T00:00:00", ((DateTime)attributes["date_of_illness_onset"]).ToString("s"));

            Assert.Equal("N", attributes["hospitalized__code"]);
            Assert.Equal("No", attributes["hospitalized"]);

            Assert.Equal("N", attributes["subject_died__code"]);
            Assert.Equal("No", attributes["subject_died"]);

            Assert.Equal("Smith, John ", attributes["person_reporting_to_cdc_name"]);

            //Assert.Equal("61528936181002006", attributes["legacy_case_identifer"]);

            Assert.Equal("48", attributes["reporting_state__code"]);
            Assert.Equal("Texas", attributes["reporting_state"]);
            Assert.Equal("FIPS5_2", attributes["reporting_state__code_system"]);
            #endregion

            // STD-specific fields start here

            //Assert.Equal("2013-03-15T00:00:00", ((DateTime)attributes["date_of_first_prenatal_visit"]).ToString("s"));

            Assert.Equal("PHC1490", attributes["gender_identity__code"]);
            Assert.Equal("Cisgender/Not transgender (finding)", attributes["gender_identity"]);
            Assert.Equal("CDCPHINVS", attributes["gender_identity__code_system"]);

            Assert.Equal("20430005", attributes["sexual_orientation__code"]);
            Assert.Equal("Heterosexual", attributes["sexual_orientation"]);
            Assert.Equal("SCT", attributes["sexual_orientation__code_system"]);

            
        }

        [Fact]
        public void Repeating_Group_Test_01()
        {
        }

        [Fact]
        public void Repeating_Group_Test_02()
        {
        }

        [Theory]
        //[InlineData("2014-12-25T17:00:30", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        //[InlineData("2015-01-01T04:00:30", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20141231230030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        //[InlineData("2014-12-31T23:00:30", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20150101040030.1234+0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        //[InlineData("2010-11-04T00:00:00", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20101104000000-0000||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        [InlineData("2010-11-04T00:00:00", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20101104-0000||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        //[InlineData("2010-11-04T00:00:00", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20101104+0000||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        //[InlineData("2010-11-04T00:00:00", @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20101104||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO" + "\r\n" + @"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND")]
        public void Date_Conversion_Test(string expected, string message)
        {
            ConversionResult result = new HL7v2ToJsonConverter(new InMemoryMmgService()).Convert(message, "1234");
            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Json);
            Assert.Equal(expected, ((DateTime)attributes["datetime_of_message"]).ToString("s"));
        }

        [Fact]
        public void Date_AllNines_Test()
        {
            // what do we do when a date field has all 9's? 

            // CONSTRAINT: Per the PHIN Spec: For date data types outlined in the PHIN Messaging Specification 
            // for Case Notification Version 3.0, the literal string of �99999999� (eight nines) MAY be sent in 
            // place of a valid date value to signify a date is unknown. A sender MAY send the literal string of
            // �99999999� for any date data elements of interest outlined in the Message Mapping Guides or 
            // supported segments in the PHIN Messaging Specification, except for those that are considered 
            // required (CDC Priority of �R� or HL7 Usage �R�). Required date data elements must be populated 
            // with a valid date value in the format outlined for the PHIN Messaging Specification. 
            string message = @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20100101||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO"
+ "\r\n" +
@"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND"
+ "\r\n" +
@"OBX|1|DT|77979-3^Case Investigation Start Date^LN||99999999||||||F";
            ConversionResult result = new HL7v2ToJsonConverter(new InMemoryMmgService()).Convert(message, "1234");
            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Json);
            Assert.Equal("99999999", attributes["case_investigation_start_date"]);
        }

        [Fact]
        public void Date_Only_has_Year_Test()
        {
            // HL7 dates can just have a year and that's it. does our code handle this acceptably?
            string message = @"MSH|^~\&|SAN^2.16.840.1.114222.nnnn^ISO|SF^2.16.840.1.114222.nnnn^ISO|PH^2.16.840.1.114222.4.3.2.10^ISO|PH^2.16.840.1.114222^ISO|20100101||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO"
+ "\r\n" + 
@"OBR|1||BABESIOSIS_TC01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20150805150000|||||||||||||||20150805150000|||F||||||12010^Babesiosis^NND"
+ "\r\n" +
@"OBX|1|DT|77979-3^Case Investigation Start Date^LN||2014||||||F";
            ConversionResult result = new HL7v2ToJsonConverter(new InMemoryMmgService()).Convert(message, "1234");
            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(result.Json);
            Assert.Equal("2014", attributes["case_investigation_start_date"]);

            // REQUIREMENT - if the date is valid HL7v2, we just get a string and not a Json date. 
            // ASSUMPTION - the Json converter is not here to validate the HL7v2 message. We'd assume a three-digit value here, or something like 'AAA' which is an obviously invalid date, would have been caught earlier in the processing pipeline.
        }

        [Fact]
        public void Numeric_Element_Test()
        {
        }

        [Fact]
        public void Coded_Element_OBX5_Basic_Test()
        {
            // does the code handle coded data elements properly?
        }

        [Fact]
        public void Coded_Element_OBX5_AltValues_Test()
        {
        }

        [Fact]
        public void Coded_Element_OBX5_ThirdTriplet_Test()
        {
        }

        [Fact]
        public void Coded_Element_OBX6_Test()
        {
        }
    }
}
