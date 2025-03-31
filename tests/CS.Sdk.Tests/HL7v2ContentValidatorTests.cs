using CS.Sdk.Services;
using CS.Sdk.Validators;
using Xunit;

namespace CS.Sdk.Tests
{
        public class HL7v2ContentValidatorTests
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
OBX|78|CWE|INV290^Test Type^PHINQUESTION|4|9584-4^IFA – IgG^LN||||||F
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
                //         private static string STD_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|STD_V1_CN_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~STD_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
                // PID|1||STD_TC01^^^SendAppName&2.16.840.1.114222.GENv2&ISO||~^^^^^^S||19640502|F||2106-3^White^CDCREC|^^^48^77018^^^^48201^312800|||||||||||2135-2^Hispanic or Latino^CDCREC
                // OBR|1||STD_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20140227170100|||||||||||||||20140227170100|||F||||||10311^Syphilis, primary^NND
                // OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
                // OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
                // OBX|3|TS|11368-8^Date of Illness Onset^LN||20140224||||||F
                // OBX|4|TS|77976-9^Illness End Date^LN||20140302||||||F
                // OBX|5|SN|77977-7^Illness Duration^LN||^6|d^day^UCUM|||||F
                // OBX|6|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
                // OBX|7|TS|77975-1^Diagnosis Date^LN||20140225||||||F
                // OBX|8|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
                // OBX|9|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
                // OBX|10|ST|77993-4^State Case Identifier^LN||2014LEV100000001||||||F
                // OBX|11|ST|77997-5^Legacy Case Identifier^LN||34022748S012014||||||F
                // OBX|12|SN|77998-3^Age at Case Investigation^LN||^49|a^year^UCUM|||||F
                // OBX|13|CWE|77982-7^Case Disease Imported Code^LN||PHC244^Indigenous, within jurisdiction^CDCPHINVS||||||F
                // OBX|14|CWE|77984-3^Country of Exposure^LN|1|USA^United States^ISO3166_1||||||F
                // OBX|15|CWE|77985-0^State or Province of Exposure^LN|1|48^Texas^FIPS5_2||||||F
                // OBX|16|ST|77986-8^City of Exposure^LN|1|Houston||||||F
                // OBX|17|ST|77987-6^County of Exposure^LN|1|Harris||||||F
                // OBX|18|CWE|77989-2^Transmission Mode^LN||417564009^Sexual transmission^SCT||||||F
                // OBX|19|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
                // OBX|20|CWE|77965-2^Immediate National Notifiable Condition^LN||N^No^HL70136||||||F
                // OBX|21|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F
                // OBX|22|ST|77969-4^Jurisdiction Code^LN||S01||||||F
                // OBX|23|CWE|48766-0^Reporting Source Type Code^LN||PHC1220^HIV Counseling and Testing Site^CDCPHINVS||||||F
                // OBX|24|ST|52831-5^Reporting Source Zip Code^LN||77009||||||F
                // OBX|25|ST|74549-7^Person Reporting to CDC-Name^LN||Smith, John ||||||F
                // OBX|26|ST|74548-9^Person Reporting to CDC-Phone Number^LN||(734)677-7777||||||F
                // OBX|27|ST|74547-1^Person Reporting to CDC-Email^LN||Smith@you.com||||||F
                // OBX|28|DT|77979-3^Case Investigation Start Date^LN||20140225||||||F
                // OBX|29|DT|77995-9^Date Reported^LN||20140225||||||F
                // OBX|30|TS|77972-8^Earliest Date Reported to County^LN||20140225||||||F
                // OBX|31|TS|77973-6^Earliest Date Reported to State^LN||20140225||||||F
                // OBX|32|SN|77991-8^MMWR Week^LN||^9||||||F
                // OBX|33|DT|77992-6^MMWR Year^LN||2014||||||F
                // OBX|34|DT|77994-2^Date CDC Was First Verbally Notified of This Case^LN||20140225||||||F
                // OBX|35|DT|77970-2^Date First Reported to PHD^LN||20140225||||||F
                // OBX|36|CWE|77966-0^Reporting State^LN||48^Texas^FIPS5_2||||||F
                // OBX|37|CWE|77967-8^Reporting County^LN||48201^Harris^FIPS6_4||||||F
                // OBX|38|CWE|77968-6^National Reporting Jurisdiction^LN||48^Texas^FIPS5_2||||||F
                // OBX|39|TX|77999-1^Comment^LN||<use any comment>||||||F
                // OBX|40|CWE|76691-5^Gender Identity^LN||PHC1490^Cisgender/Not transgender (finding)^CDCPHINVS||||||F
                // OBX|41|CWE|76690-7^Sexual Orientation^LN||20430005^Heterosexual^SCT||||||F
                // OBX|42|DT|STD099^Date of Initial Health Exam Associated with Case Report ""Health Event""^PHINQUESTION||20140225||||||F
                // OBX|43|CWE|STD102^Neurologic Manifestations^PHINQUESTION||PHC1475^Yes, Verified^CDCPHINVS||||||F
                // OBX|44|CWE|410478005^Ocular Manifestations^SCT||PHC1476^Yes, Verified^CDCPHINVS||||||F
                // OBX|45|CWE|PHC1472^Otic Manifestations^CDCPHINVS||N^No^HL70136||||||F
                // OBX|46|CWE|72083004^Late Clinical Manifestations^SCT||N^No^HL70136||||||F
                // OBX|47|DT|STD105^Treatment Date^PHINQUESTION||20140225||||||F
                // OBX|48|CWE|55277-8^HIV Status^LN||165816005^HIV positive^SCT||||||F
                // OBX|49|CWE|STD107^Had Sex with a Male within the Past 12 Months^PHINQUESTION||Y^Yes^HL70136||||||F
                // OBX|50|CWE|STD108^Had Sex with a Female within the Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|51|CWE|STD109^Had Sex with an Anonymous Partner within the Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|52|CWE|STD110^Had Sex with a Person Known to Him/Her to be an ""Intravenous Drug User (IDU)"" within the Past 12 months^PHINQUESTION||NASK^Did Not Ask^NULLFL||||||F
                // OBX|53|CWE|STD111^Had Sex while Intoxicated and/or High on Drugs within the Past 12 months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|54|CWE|STD112^Exchanged Drugs/Money for Sex within the Past 12 Months^PHINQUESTION||Y^Yes^HL70136||||||F
                // OBX|55|CWE|STD113^Had Sex with a Person Who is Known to Her to Be an MSM within Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|56|CWE|STD114^Engaged in Injection Drug use Within Past 12 Months^PHINQUESTION||Y^Yes^HL70136||||||F
                // OBX|57|CWE|INV159^Detection Method^PHINQUESTION||20135006^Screening^SCT||||||F
                // OBX|58|CWE|STD115^Drugs Used^PHINQUESTION|2|2653^Cocaine^RXNORM||||||F
                // OBX|59|CWE|STD116^Drugs Used Indicator^PHINQUESTION|2|Y^Yes^HL70136||||||F
                // OBX|60|CWE|STD115^Drugs Used^PHINQUESTION|3|PHC1308^Other Drugs Used^CDCPHINVS||||||F
                // OBX|61|CWE|STD116^Drugs Used Indicator^PHINQUESTION|3|Y^Yes^HL70136||||||F
                // OBX|62|CWE|STD115^Drugs Used^PHINQUESTION|4|PHC1160^Erectile dysfunction medications^CDCPHINVS||||||F
                // OBX|63|CWE|STD116^Drugs Used Indicator^PHINQUESTION|4|Y^Yes^HL70136||||||F
                // OBX|64|CWE|STD117^Previous STD History^PHINQUESTION||N^No^HL70136||||||F
                // OBX|65|CWE|STD118^Been Incarcerated within the Past 12 Months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|66|CWE|STD119^Have You Met Sex Partners through the Internet in the Last 12 Months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|67|SN|STD120^Total Number of Sex Partners in the Last 12 months^PHINQUESTION||^1||||||F
                // OBX|68|CWE|STD121^Clinician-Observed Lesion(s) Indicative of Syphilis^PHINQUESTION||76784001^Vagina^SCT||||||F
                // OBX|69|CWE|STD999^Unknown Number of Sex Partners in Last 12 Months^PHINQUESTION||N^No^HL70136||||||F
                // OBX|70|CWE|INV892^HIV Status Documented Through eHARS Record Search^PHINQUESTION||UNK^Unknown^NULLFL||||||F
                // OBX|71|ST|INV893^eHARS State no(i.e.State Number)^PHINQUESTION||58968||||||F
                // OBX|72|CWE|INV894^Transmission Category(eHARS)^PHINQUESTION||PHC1143^Adult heterosexual contact^CDCPHINVS||||||F
                // OBX|73|CWE|INV895^Case Sampled for Enhanced Investigation^PHINQUESTION||N^No^HL70136||||||F
                // OBX|74|CWE|INV290^Test Type^PHINQUESTION|5|24110-9^T pallidum Ab Ser QI IA^LN||||||F
                // OBX|75|CWE|INV291^Test Result^PHINQUESTION|5|10828004^Positive(qualifier value)^SCT||||||F
                // OBX|76|DT|82772-5^Date of lab result^LN|5|20140225||||||F
                // OBX|77|CWE|31208-2^Specimen source^LN|5|PHC1151^Blood/Serum^CDCPHINVS||||||F
                // OBX|78|DT|33882-2^Specimen Collection Date^LN|5|20140225||||||F
                // OBR|2||SYP9700123-1^Lab^2.16.840.1.113883.19.3.1.6^ISO|71793-4^Treponema pallidum AB[Titer} in Serum or Plasma by Agglutination^LN|||20140225151000||||||||||||||||||F
                // OBX|1|SN|71793-4^Treponema pallidum AB [Titer} in Serum or Plasma by Agglutination^LN|1|^1^:^64|{titer}^titer^UCUM|<1:8|H|||F|||20140225|||0014^Agglutination^OBSMETHOD||20140225
                // SPM|1|||119364003^Serum specimen^SCT||||368149001^Right Elbow^SCT|||P^Patient^HL70369|2^mL&milliliter&UCUM||Serum Sample|||20140225|20140225
                // ";
                #endregion

                #region CS Message 01
                //                 private static string CS_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.nnnn^ISO|Sending-Facility^2.16.840.1.114222.nnnn^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC01|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
                // PID|1||CONSYPH_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||20130801|F||2054-5^Black or African American^CDCREC|^^^22^71301^^^^22079|||||||||||2186-5^Not Hispanic or Latino^CDCREC
                // NK1|1||MTH^Mother^HL70063|^^^22^71301^USA^^^22079||||||||||S^Single^HL70002||19940113||||||||||||2186-5^Not Hispanic or Latino^CDCREC|||||||2054-5^Black or African American^CDCREC
                // OBR|1||CONSYPH_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20130825170100|||||||||||||||20130825170100|||F||||||10316^Syphilis, Congenital^NND
                // OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F
                // OBX|2|CWE|77983-5^Country of Usual Residence^LN||USA^United States^ISO3166_1||||||F
                // OBX|3|TS|11368-8^Date of Illness Onset^LN||20130801||||||F
                // OBX|4|TS|77975-1^Diagnosis Date^LN||20130801||||||F
                // OBX|5|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
                // OBX|6|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
                // OBX|7|ST|77993-4^State Case Identifier^LN||<use state-assigned value>||||||F
                // OBX|8|ST|77997-5^Legacy Case Identifier^LN||61528936181002006||||||F
                // OBX|9|SN|77998-3^Age at Case Investigation^LN||^1|d^day^UCUM|||||F
                // OBX|10|CWE|77982-7^Case Disease Imported Code^LN||PHC244^Indigenous, within jurisdiction^CDCPHINVS||||||F
                // OBX|11|CWE|77984-3^Country of Exposure^LN|1|USA^United States^ISO3166_1||||||F
                // OBX|12|CWE|77985-0^State or Province of Exposure^LN|1|22^Louisiana^FIPS5_2||||||F
                // OBX|13|ST|77986-8^City of Exposure^LN|1|Alexandria||||||F
                // OBX|14|ST|77987-6^County of Exposure^LN|1|Rapides||||||F
                // OBX|15|CWE|77989-2^Transmission Mode^LN||417409004^Transplacental transmission^SCT||||||F
                // OBX|16|CWE|77990-0^Case Class Status Code^LN||2931005^Probable diagnosis^SCT||||||F
                // OBX|17|CWE|77965-2^Immediate National Notifiable Condition^LN||N^No^HL70136||||||F
                // OBX|18|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F
                // OBX|19|ST|77969-4^Jurisdiction Code^LN||S01||||||F
                // OBX|20|CWE|48766-0^Reporting Source Type Code^LN||1^Hospital^HL70406||||||F
                // OBX|21|ST|52831-5^Reporting Source Zip Code^LN||71301||||||F
                // OBX|22|ST|74549-7^Person Reporting to CDC-Name^LN||Smith, John||||||F
                // OBX|23|ST|74548-9^Person Reporting to CDC-Phone Number^LN||(734)677-7777||||||F
                // OBX|24|DT|77979-3^Case Investigation Start Date^LN||20130801||||||F
                // OBX|25|DT|77995-9^Date Reported^LN||20130801||||||F
                // OBX|26|TS|77972-8^Earliest Date Reported to County^LN||20130802||||||F
                // OBX|27|TS|77973-6^Earliest Date reported to State^LN||20130806||||||F
                // OBX|28|SN|77991-8^MMWR Week^LN||^31||||||F
                // OBX|29|DT|77992-6^MMWR Year^LN||2013||||||F
                // OBX|30|DT|77970-2^Date First Reported to PHD^LN||20130802||||||F
                // OBX|31|CWE|77966-0^Reporting State^LN||22^Louisiana^FIPS5_2||||||F
                // OBX|32|CWE|77967-8^Reporting County^LN||22079^Rapides^FIPS6_4||||||F
                // OBX|33|CWE|77968-6^National Reporting Jurisdiction^LN||22^Louisiana^FIPS5_2||||||F
                // OBX|34|NM|75201-4^Number of Pregnancies^LN||1||||||F
                // OBX|35|NM|75202-2^Number of Total Live Births^LN||1||||||F
                // OBX|36|DT|75203-0^Last Menstrual Period^LN||99999999||||||F
                // OBX|37|DT|75200-6^Date of First Prenatal Visit^LN||20130315||||||F
                // OBX|38|CWE|75204-8^Prenatal Visit Indicator^LN||Y^Yes^HL70136||||||F
                // OBX|39|CWE|75163-6^Trimester of First Prenatal Visit^LN||255247007^Second trimester^SCT||||||F
                // OBX|40|CWE|75179-2^Mother's HIV Status During Pregnancy^LN||165815009^HIV negative^SCT||||||F
                // OBX|41|CWE|75180-0^Mother's Clinical Stage of Syphilis During Pregnancy^LN||186867005^early latent^SCT||||||F
                // OBX|42|CWE|75181-8^Mother's Surveillance Stage of Syphilis During Pregnancy^LN||PHC1506^Syphilis, early non-primary, non-secondary^CDCPHINVS||||||F
                // OBX|43|DT|75182-6^Date When Mother Received Her First Dose of Benzathine Penicillin^LN||20130226||||||F
                // OBX|44|CWE|75183-4^Trimester in Which Mother Received Her First Dose of Benzathine Penicillin^LN||255246003^First Trimester^SCT||||||F
                // OBX|45|CWE|75184-2^Mother's Treatment^LN||PHC1278^2.4 M units benzathine penicillin^CDCPHINVS||||||F
                // OBX|46|CWE|75185-9^Appropriate Serologic Reponse^LN||PHC1282^No, inappropriate response: evidence of treatment failure or reinfection^CDCPHINVS||||||F
                // OBX|47|CWE|75164-4^Non-treponemal Test or Treponemal Test at First Prenatal Visit^LN||Y^Yes^HL70136||||||F
                // OBX|48|CWE|75165-1^Non-treponemal Test or Treponemal Test at 28-32 Weeks Gestation^LN||Y^Yes^HL70136||||||F
                // OBX|49|CWE|75166-9^Non-treponemal Test or Treponemal Test at Delivery^LN||Y^Yes^HL70136||||||F
                // OBX|50|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|2|STD150^Mother's First Non-treponemal Test Finding/Information^PHINQUESTION||||||F
                // OBX|51|CWE|INV290^Test Type^PHINQUESTION|2|5292-8^VDRL (Serum)^LN||||||F
                // OBX|52|CWE|INV291^Test Result^PHINQUESTION|2|11214006^Reactive^SCT||||||F
                // OBX|53|CWE|STD123^Nontreponemal Serologic syphilis test result^PHINQUESTION|2|STD8^1:8^CDCPHINVS||||||F
                // OBX|54|DT|82772-5^Date of lab result^LN|2|20130204||||||F
                // OBX|55|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|3|STD151^Mother's Most Recent Non-treponemal Test Finding/Information^PHINQUESTION||||||F
                // OBX|56|CWE|INV290^Test Type^PHINQUESTION|3|20507-0^RPR^LN||||||F
                // OBX|57|CWE|INV291^Test Result^PHINQUESTION|3|11214006^Reactive^SCT||||||F
                // OBX|58|CWE|STD123^Nontreponemal Serologic syphilis test result^PHINQUESTION|3|STD8^1:8^CDCPHINVS||||||F
                // OBX|59|DT|82772-5^Date of lab result^LN|3|20130808||||||F
                // OBX|60|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|4|STD152^Mother's First Treponemal Test Finding^PHINQUESTION||||||F
                // OBX|61|CWE|INV290^Test Type^PHINQUESTION|4|24110-9^Treponema Pallidum - Enzyme immunoassay (EIA)^LN||||||F
                // OBX|62|CWE|INV291^Test Result^PHINQUESTION|4|11214006^Reactive^SCT||||||F
                // OBX|63|DT|82772-5^Date of lab result^LN|4|20130204||||||F
                // OBX|64|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|5|STD153^Mother's Most Recent Treponemal Test Finding^PHINQUESTION||||||F
                // OBX|65|CWE|INV290^Test Type^PHINQUESTION|5|PHC1306^Other Treponemal Test Type^CDCPHINVS||||||F
                // OBX|66|CWE|INV291^Test Result^PHINQUESTION|5|11214006^Reactive^SCT||||||F
                // OBX|67|DT|82772-5^Date of lab result^LN|5|20130801||||||F
                // OBX|68|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|6|STD154^Infant's Non-treponemal Test Finding^PHINQUESTION||||||F
                // OBX|69|CWE|INV290^Test Type^PHINQUESTION|6|20507-0^RPR^LN||||||F
                // OBX|70|CWE|INV291^Test Result^PHINQUESTION|6|11214006^Reactive^SCT||||||F
                // OBX|71|CWE|STD123^Nontreponemal Serologic syphilis test result^PHINQUESTION|6|STD1^1:1^CDCPHINVS||||||F
                // OBX|72|DT|82772-5^Date of lab result^LN|6|20130801||||||F
                // OBX|73|CWE|LAB588^Lab Test Performed Modifier^PHINQUESTION|7|STD155^Infant's Treponemal Test Finding^PHINQUESTION||||||F
                // OBX|74|CWE|INV290^Test Type^PHINQUESTION|7|24110-9^Treponema Pallidum - Enzyme immunoassay (EIA)^LN||||||F
                // OBX|75|CWE|INV291^Test Result^PHINQUESTION|7|11214006^Reactive^SCT||||||F
                // OBX|76|DT|82772-5^Date of lab result^LN|7|20130801||||||F
                // OBX|77|CWE|75186-7^Vital Status^LN||438949009^Alive^SCT||||||F
                // OBX|78|SN|56056-5^Birth Weight^LN||^2623|g^gram^UCUM|||||F
                // OBX|79|SN|57714-8^Gestational Age^LN||^36|wk^week^UCUM|||||F
                // OBX|80|CWE|75193-3^Clinical Signs of Congenital Syphilis^SCT||84387000^no signs/asymptomatic^SCT||||||F
                // OBX|81|CWE|75194-1^Long Bone X-rays for Infant^LN||PHC1288^Yes, no signs of CS^CDCPHINVS||||||F
                // OBX|82|CWE|75192-5^Darkfield Exam, DFA, or Special Stain Test Findings^LN||373121007^Test Not done^SCT||||||F
                // OBX|83|CWE|LP48341-9^CSF WBC Count^LN||373121007^Test Not done^SCT||||||F
                // OBX|84|CWE|LP69956-8^CSF Protein Level^LN||373121007^Test Not done^SCT||||||F
                // OBX|85|CWE|75195-8^CSF VDRL Test Finding^LN||373121007^Test Not done^SCT||||||F
                // OBX|86|CWE|75197-4^Infant Treated^LN||PHC1296^Yes,with other treatment^CDCPHINVS||||||F
                // OBX|87|CWE|75207-1^Stillbirth Indicator^LN||N^No^HL70136||||||F
                // OBR|2|CSYP23456-1^EHR^2.16.840.1.113883.19.3.2.3^ISO|CSYP9700123-1^Lab^2.16.840.1.113883.19.3.1.6^ISO|31147-2^Reagin Ab [Titer] in Serum by RPR^LN|||20130801000000|||||||||121121121^^^^^^^^NPI&2.16.840.1.113883.4.6&ISO^L^^^NPI^NPI_Facility&2.16.840.1.113883.3.72.5.26&ISO^^^^^^^MD||||||20130801000000|||F||||||A53.9^Syphilis, unspecified^I10C
                // OBX|1|SN|31147-2^Reagin Ab [Titer] in Serum by RPR^LN||^1^:^4|{titer}^titer^UCUM|<1:1|H|||F|||20130801|||0021^Rapid Agglutination^OBSMETHOD||20130801||||GHH Lab^L^^^^CLIA&2.16.840.1.113883.19.4.6&ISO^XX^^^1236||9876543^^^^^^^^NPI&1.3.6.1.4.1.562.2.4.1.43&ISO^^^^NPI
                // NTE|1|L|Response to therapy is indicated by a > or =4-fold decrease in titer between pre and post treatment samples. However, a significant decrease in RPR titers may not occur for months to years following treatment, some patients may show persistent, low-level titers (e.g. serofast) despite adequate therapy.
                // SPM|1|CSYP234561-1&EHR&2.16.840.1.113883.19.3.2.3&ISO^CSYP97001222-1&Lab&2.16.840.1.113883.19.3.1.6&ISO||119364003^Serum specimen^SCT||||368149001^Right Elbow^SCT|||P^Patient^HL70369||||||20130801|20130801";
                #endregion

                //[Fact]
                //public void Babesiosis_Validation_01()
                //{
                //    IContentValidator validator = new HL7v2ContentValidator(new InMemoryVocabularyService(), new InMemoryMmgService());

                //    string transactionId = "1234";

                //    ValidationResult result = validator.Validate(BABESIOSIS_MESSAGE_01, transactionId);

                //    Assert.Equal(0, result.Errors);
                //    Assert.Equal(0, result.Warnings);
                //    Assert.True(result.IsSuccess);
                //}

                //[Fact]
                //public void STD_Validation_01()
                //{
                //    IContentValidator validator = new HL7v2ContentValidator(new InMemoryVocabularyService(), new InMemoryMmgService());

                //    string transactionId = "1234";

                //    ValidationResult result = validator.Validate(STD_MESSAGE_01, transactionId);

                //    Assert.True(result.ValidationMessages.TrueForAll(m => m.ErrorCode == "90001"));
                //    Assert.Equal(0, result.Errors);
                //    Assert.Equal(7, result.Warnings);
                //    Assert.True(result.IsSuccess); // even with 7 warnings, its valid and we pass it on
                //}

                //[Fact]
                //public void CS_Validation_01()
                //{
                //    IContentValidator validator = new HL7v2ContentValidator(new InMemoryVocabularyService(), new InMemoryMmgService());

                //    string transactionId = "1234";

                //    ValidationResult result = validator.Validate(CS_MESSAGE_01, transactionId);

                //    Assert.True(result.ValidationMessages.TrueForAll(m => m.ErrorCode.StartsWith("9000")));
                //    Assert.Equal(0, result.Errors);
                //    Assert.Equal(3, result.Warnings);
                //    Assert.True(result.IsSuccess);
                //}

                [Theory]
                [InlineData(
        @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|2|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|3|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||N^No^HL70136||||||F
OBX|6|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|7|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|8|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|9|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|10|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|11|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|12|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F")]
                public void Obx_Vocabulary_Validation_Success(string message)
                {
                        IContentValidator validator = new HL7v2ContentValidator(new InMemoryVocabularyService(), new InMemoryMmgService());
                        string transactionId = "1234";

                        ValidationResult result = validator.Validate(message, transactionId);
                        Assert.Equal(0, result.Errors);
                        Assert.Equal(0, result.Warnings);
                        Assert.True(result.IsSuccess);
                }

                [Theory]
                [InlineData(
        @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|2|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|3|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||No^No^HL70136||||||F
OBX|6|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|7|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|8|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|9|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|10|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|11|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|12|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F")]
                [InlineData(
        @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|2|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|3|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||U^Unknown^NULLFL||||||F
OBX|6|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|7|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|8|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|9|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|10|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|11|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|12|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F")]
                public void Obx_Vocabulary_Validation_Warnings(string message)
                {
                        // This tests for invalid vocabulary values, like 'No' instead of 'N' for the concept code, which should always be warnings

                        IContentValidator validator = new HL7v2ContentValidator(new InMemoryVocabularyService(), new InMemoryMmgService());
                        string transactionId = "1234";

                        ValidationResult result = validator.Validate(message, transactionId);
                        Assert.Equal(0, result.Errors);
                        Assert.Equal(1, result.Warnings);
                        Assert.True(result.IsSuccess);
                }

                [Theory]
                [InlineData(
        @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|2|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|3|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||N^No||||||F
OBX|6|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|7|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|8|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|9|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|10|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|11|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|12|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F")]
                [InlineData(
        @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|2|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|3|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||N||||||F
OBX|6|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|7|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|8|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|9|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|10|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|11|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|12|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F")]
                [InlineData(
        @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|MESSAGE CONTROL ID|D|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO~TB_MMG_V3.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||TB_TC_01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19780301|M||2028-9^Asian^CDCREC~2106-3^White^CDCREC|^^Atlanta^13^30313^^^^13089^13121009101|||||||||||2186-5^Not Hispanic or Latino^CDCREC||||||||||||||||
OBR|1||TB_TC_01^SendAppName^2.16.840.1.114222.TBD^ISO|68991-9^Epidemiologic Information^LN|||20180510111111|||||||||||||||20180510111111|||F||||||10220^Tuberculosis^NND
OBX|1|CWE|77978-5^Subject Died^LN||N^No^HL70136||||||F
OBX|2|SN|77998-3^Age at Case Investigation^LN||^40|a^year [time]^UCUM|||||F
OBX|3|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|4|TS|77975-1^Diagnosis Date^LN||20180202||||||F
OBX|5|CWE|77974-4^Hospitalized^LN||No||||||F
OBX|6|SN|77991-8^MMWR Week^LN||^17||||||F
OBX|7|DT|77992-6^MMWR Year^LN||2018||||||F
OBX|8|CWE|77966-0^Reporting State^LN||13^Georgia^FIPS5_2||||||F
OBX|9|CWE|77967-8^Reporting County^LN||13089^DeKalb, GA^FIPS6_4||||||F
OBX|10|CWE|77968-6^National Reporting Jurisdiction^LN||13^GA^FIPS5_2||||||F
OBX|11|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|12|CWE|77980-1^Case Outbreak Indicator^LN||N^No^HL70136||||||F")]
                public void Obx_Vocabulary_Validation_Errors(string message)
                {
                        // This tests for invalid vocabulary structure, like the whole OBX-3 is just 'NO' in which case the structure of the message is techincally incorrect and we throw an error

                        IContentValidator validator = new HL7v2ContentValidator(new InMemoryVocabularyService(), new InMemoryMmgService());
                        string transactionId = "1234";

                        ValidationResult result = validator.Validate(message, transactionId);
                        Assert.Equal(1, result.Errors);
                        Assert.Equal(0, result.Warnings);
                        Assert.False(result.IsSuccess);
                }
        }
}
