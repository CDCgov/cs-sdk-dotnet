using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CS.Sdk.Converters;
using CS.Sdk.Generators;
using CS.Sdk.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace CS.Sdk.Tests
{
    public class HL7v2ToCsvConverterTests
    {
        #region Gen V2 Message 01
        private const string GENV2_MESSAGE_01 = @"MSH|^~\&|SendAppName^2.16.840.1.114222.TBD^ISO|Sending-Facility^2.16.840.1.114222.TBD^ISO|PHINCDS^2.16.840.1.114222.4.3.2.10^ISO|PHIN^2.16.840.1.114222^ISO|20140630120030.1234-0500||ORU^R01^ORU_R01|TM_CN_TC01_GENV2|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.114222.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.114222.4.10.4^ISO
PID|1||GenV2_TC01^^^SendAppName&2.16.840.1.114222.nnnn&ISO||~^^^^^^S||19640502|F||2106-3^White^CDCREC|^^^48^77018^^^^48201|||||||||||2135-2^Hispanic or Latino^CDCREC|||||||20140302
OBR|1||GenV2_TC01^SendAppName^2.16.840.1.114222.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20140227170100|||||||||||||||20140227170100|||F||||||11550^Hemolytic uremic syndrome postdiarrheal^NND
OBX|1|ST|32624-9^Other Race Text^LN||Apache||||||F
OBX|2|CWE|78746-5^Country of Birth^LN||USA^UNITED STATES OF AMERICA^ISO3166_1||||||F
OBX|3|CWE|77983-5^Country of Usual Residence^LN||USA^UNITED STATES OF AMERICA^ISO3166_1||||||F
OBX|4|TS|11368-8^Date of Illness Onset^LN||20140224||||||F
OBX|5|TS|77976-9^Illness End Date^LN||20140302||||||F
OBX|6|SN|77977-7^Illness Duration^LN||^6|d^day [time]^UCUM|||||F
OBX|7|CWE|77996-7^Pregnancy Status^LN||N^No^HL70136||||||F
OBX|8|TS|77975-1^Diagnosis Date^LN||20140225||||||F
OBX|9|CWE|77974-4^Hospitalized^LN||Y^Yes^HL70136||||||F
OBX|10|TS|8656-1^Admission Date^LN||20140226||||||F
OBX|11|TS|8649-6^Discharge Date^LN||20140302||||||F
OBX|12|SN|78033-8^Duration of Hospital Stay in Days^LN||^4||||||F
OBX|13|CWE|77978-5^Subject Died^LN||Y^Yes^HL70136||||||F
OBX|14|ST|77993-4^State Case Identifier^LN||TX4321||||||F
OBX|15|ST|77997-5^Legacy Case Identifier^LN||48432148S012014||||||F
OBX|16|SN|77998-3^Age at Case Investigation^LN||^49|a^year [time]^UCUM|||||F
OBX|17|CWE|77982-7^Case Disease Imported Code^LN||C1512888^International^UML||||||F
OBX|18|CWE|INV153^Imported Country^PHINQUESTION||MEX^MEXICO^ISO3166_1||||||F
OBX|19|CWE|77984-3^Country of Exposure^LN|1|USA^UNITED STATES OF AMERICA^ISO3166_1||||||F
OBX|20|CWE|77985-0^State or Province of Exposure^LN|1|48^Texas^FIPS5_2||||||F
OBX|21|ST|77986-8^City of Exposure^LN|1|Houston||||||F
OBX|22|ST|77987-6^County of Exposure^LN|1|Harris||||||F
OBX|23|CWE|77989-2^Transmission Mode^LN||416086007^Food-borne transmission^SCT||||||F
OBX|24|CWE|77990-0^Case Class Status Code^LN||410605003^Confirmed present^SCT||||||F
OBX|25|CWE|77965-2^Immediate National Notifiable Condition^LN||N^No^HL70136||||||F
OBX|26|CWE|77980-1^Case Outbreak Indicator^LN||Y^Yes^HL70136||||||F
OBX|27|ST|77981-9^Case Outbreak Name^LN||HANSENOUTB1||||||F
OBX|28|ST|77969-4^Jurisdiction Code^LN||S01||||||F
OBX|29|CWE|48766-0^Reporting Source Type Code^LN||1^Hospital^HL70406||||||F
OBX|30|ST|52831-5^Reporting Source ZIP Code^LN||77018||||||F
OBX|31|CWE|77988-4^Binational Reporting Criteria^LN||PHC1140^Exposure to suspected product from Canada or Mexico^CDCPHINVS||||||F
OBX|32|ST|74549-7^Person Reporting to CDC - Name^LN||Smith, John||||||F
OBX|33|ST|74548-9^Person Reporting to CDC - Phone Number^LN||444-321-1234||||||F
OBX|34|ST|74547-1^Person Reporting to CDC - Email^LN||jsmith@txdoh.org||||||F
OBX|35|DT|77979-3^Case Investigation Start Date^LN||20140225||||||F
OBX|36|DT|77995-9^Date Reported^LN||20140225||||||F
OBX|37|TS|77972-8^Earliest Date Reported to County^LN||20140225||||||F
OBX|38|TS|77973-6^Earliest Date Reported to State^LN||20140225||||||F
OBX|39|SN|77991-8^MMWR Week^LN||^9||||||F
OBX|40|DT|77992-6^MMWR Year^LN||2014||||||F
OBX|41|DT|77994-2^Date CDC Was First Verbally Notified of This Case^LN||20140225||||||F
OBX|42|DT|77970-2^Date First Reported to PHD^LN||20140225||||||F
OBX|43|CWE|77966-0^Reporting State^LN||48^Texas^FIPS5_2||||||F
OBX|44|CWE|77967-8^Reporting County^LN||48201^Harris, TX^FIPS6_4||||||F
OBX|45|CWE|77968-6^National Reporting Jurisdiction^LN||48^TX^FIPS5_2||||||F";
        #endregion

        string GENV2_MESSAGE_02_CSV_EXPECTED = @"source_format,datetime_of_message,unique_case_id,message_profile_identifier,message_profile_identifiers[0].message_profile_identifiers,message_profile_identifiers[1].message_profile_identifiers,message_profile_identifiers[2].message_profile_identifiers,local_subject_id,birth_date,subjects_sex,subjects_sex__code,subjects_sex__code_system,race_category[0].race_category,race_category[0].race_category__code,race_category[0].race_category__code_system,race_category[1].race_category,race_category[1].race_category__code,race_category[1].race_category__code_system,race_category[2].race_category,race_category[2].race_category__code,race_category[2].race_category__code_system,other_race_text,ethnic_group,ethnic_group__code,ethnic_group__code_system,country_of_birth,country_of_birth__code,country_of_birth__code_system,other_birth_place,country_of_usual_residence,country_of_usual_residence__code,country_of_usual_residence__code_system,subject_address_county,subject_address_county__code,subject_address_county__code_system,subject_address_state,subject_address_state__code,subject_address_state__code_system,subject_address_zip_code,date_of_illness_onset,illness_end_date,illness_duration,illness_duration_units,illness_duration_units__code,illness_duration_units__code_system,pregnancy_status,pregnancy_status__code,pregnancy_status__code_system,diagnosis_date,hospitalized,hospitalized__code,hospitalized__code_system,admission_date,discharge_date,duration_of_hospital_stay_in_days,subject_died,subject_died__code,subject_died__code_system,deceased_date,condition_code,condition_code__code,condition_code__code_system,local_record_id,state_case_identifier,legacy_case_identifier,age_at_case_investigation,age_unit_at_case_investigation,age_unit_at_case_investigation__code,age_unit_at_case_investigation__code_system,case_disease_imported_code,case_disease_imported_code__code,case_disease_imported_code__code_system,imported_country,imported_country__code,imported_country__code_system,imported_state,imported_state__code,imported_state__code_system,imported_city,imported_city__code,imported_city__code_system,imported_county,imported_county__code,imported_county__code_system,repeating_variables_for_disease_exposure[0].country_of_exposure,repeating_variables_for_disease_exposure[0].country_of_exposure__code,repeating_variables_for_disease_exposure[0].country_of_exposure__code_system,repeating_variables_for_disease_exposure[0].state_or_province_of_exposure,repeating_variables_for_disease_exposure[0].state_or_province_of_exposure__code,repeating_variables_for_disease_exposure[0].state_or_province_of_exposure__code_system,repeating_variables_for_disease_exposure[0].city_of_exposure,repeating_variables_for_disease_exposure[0].county_of_exposure,repeating_variables_for_disease_exposure[1].country_of_exposure,repeating_variables_for_disease_exposure[1].country_of_exposure__code,repeating_variables_for_disease_exposure[1].country_of_exposure__code_system,repeating_variables_for_disease_exposure[1].state_or_province_of_exposure,repeating_variables_for_disease_exposure[1].state_or_province_of_exposure__code,repeating_variables_for_disease_exposure[1].state_or_province_of_exposure__code_system,repeating_variables_for_disease_exposure[1].city_of_exposure,repeating_variables_for_disease_exposure[1].county_of_exposure,repeating_variables_for_disease_exposure[2].country_of_exposure,repeating_variables_for_disease_exposure[2].country_of_exposure__code,repeating_variables_for_disease_exposure[2].country_of_exposure__code_system,repeating_variables_for_disease_exposure[2].state_or_province_of_exposure,repeating_variables_for_disease_exposure[2].state_or_province_of_exposure__code,repeating_variables_for_disease_exposure[2].state_or_province_of_exposure__code_system,repeating_variables_for_disease_exposure[2].city_of_exposure,repeating_variables_for_disease_exposure[2].county_of_exposure,repeating_variables_for_disease_exposure[3].country_of_exposure,repeating_variables_for_disease_exposure[3].country_of_exposure__code,repeating_variables_for_disease_exposure[3].country_of_exposure__code_system,repeating_variables_for_disease_exposure[3].state_or_province_of_exposure,repeating_variables_for_disease_exposure[3].state_or_province_of_exposure__code,repeating_variables_for_disease_exposure[3].state_or_province_of_exposure__code_system,repeating_variables_for_disease_exposure[3].city_of_exposure,repeating_variables_for_disease_exposure[3].county_of_exposure,repeating_variables_for_disease_exposure[4].country_of_exposure,repeating_variables_for_disease_exposure[4].country_of_exposure__code,repeating_variables_for_disease_exposure[4].country_of_exposure__code_system,repeating_variables_for_disease_exposure[4].state_or_province_of_exposure,repeating_variables_for_disease_exposure[4].state_or_province_of_exposure__code,repeating_variables_for_disease_exposure[4].state_or_province_of_exposure__code_system,repeating_variables_for_disease_exposure[4].city_of_exposure,repeating_variables_for_disease_exposure[4].county_of_exposure,transmission_mode,transmission_mode__code,transmission_mode__code_system,case_class_status_code,case_class_status_code__code,case_class_status_code__code_system,immediate_national_notifiable_condition,immediate_national_notifiable_condition__code,immediate_national_notifiable_condition__code_system,case_outbreak_indicator,case_outbreak_indicator__code,case_outbreak_indicator__code_system,case_outbreak_name,notification_result_status,notification_result_status__code,notification_result_status__code_system,jurisdiction_code,reporting_source_type_code,reporting_source_type_code__code,reporting_source_type_code__code_system,reporting_source_zip_code,binational_reporting_criteria[0].binational_reporting_criteria,binational_reporting_criteria[0].binational_reporting_criteria__code,binational_reporting_criteria[0].binational_reporting_criteria__code_system,binational_reporting_criteria[1].binational_reporting_criteria,binational_reporting_criteria[1].binational_reporting_criteria__code,binational_reporting_criteria[1].binational_reporting_criteria__code_system,binational_reporting_criteria[2].binational_reporting_criteria,binational_reporting_criteria[2].binational_reporting_criteria__code,binational_reporting_criteria[2].binational_reporting_criteria__code_system,person_reporting_to_cdc_name,person_reporting_to_cdc_phone_number,person_reporting_to_cdc_email,case_investigation_start_date,date_first_electronically_submitted,date_of_electronic_case_notification_to_cdc,date_reported,earliest_date_reported_to_county,earliest_date_reported_to_state,mmwr_week,mmwr_year,date_cdc_was_first_verbally_notified_of_this_case,date_first_reported_to_phd,reporting_state,reporting_state__code,reporting_state__code_system,reporting_county,reporting_county__code,reporting_county__code_system,national_reporting_jurisdiction,national_reporting_jurisdiction__code,national_reporting_jurisdiction__code_system,comment
HL7v2,6/30/2014 5:00:30 PM,48_GenV2_TC01,Generic_MMG_V2.0,,,,GenV2_TC01,5/2/1964 12:00:00 AM,,F,,White,2106-3,CDCREC,,,,,,,Apache,Hispanic or Latino,2135-2,CDCREC,UNITED STATES OF AMERICA,USA,ISO3166_1,,UNITED STATES OF AMERICA,USA,ISO3166_1,,48201,,,48,,77018,2/24/2014 12:00:00 AM,3/2/2014 12:00:00 AM,6,day [time],d,UCUM,No,N,HL70136,2/25/2014 12:00:00 AM,Yes,Y,HL70136,2/26/2014 12:00:00 AM,3/2/2014 12:00:00 AM,4,Yes,Y,HL70136,3/2/2014 12:00:00 AM,Hemolytic uremic syndrome postdiarrheal,11550,NND,GenV2_TC01,TX4321,48432148S012014,49,year [time],a,UCUM,International,C1512888,UML,MEXICO,MEX,ISO3166_1,,,,,,,,,,UNITED STATES OF AMERICA,USA,ISO3166_1,Texas,48,FIPS5_2,Houston,Harris,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,Food-borne transmission,416086007,SCT,Confirmed present,410605003,SCT,No,N,HL70136,Yes,Y,HL70136,HANSENOUTB1,Final results; Can only be changed with a corrected result.,F,HL7,S01,Hospital,1,HL70406,77018,Exposure to suspected product from Canada or Mexico,PHC1140,CDCPHINVS,,,,,,,""Smith, John"",444-321-1234,jsmith@txdoh.org,2/25/2014 12:00:00 AM,2/27/2014 5:01:00 PM,2/27/2014 5:01:00 PM,2/25/2014 12:00:00 AM,2/25/2014 12:00:00 AM,2/25/2014 12:00:00 AM,9,2014,2/25/2014 12:00:00 AM,2/25/2014 12:00:00 AM,Texas,48,FIPS5_2,""Harris, TX"",48201,FIPS6_4,TX,48,FIPS5_2,
";

        [Fact]
        public void GenV2_Basic_Conversion_01()
        {
            HL7v2ToCsvConverter converter = new HL7v2ToCsvConverter(new MmgCsvTemplateGenerator(), new HL7v2ToJsonConverter(), true);
            ConversionResult result = converter.Convert(GENV2_MESSAGE_01, "1234");

            string csv = result.Content;

            Assert.Equal(GENV2_MESSAGE_02_CSV_EXPECTED, csv);

            //System.IO.File.WriteAllText("test01.csv", csv);

            
        }
    }
}
