# Case Surveillance SDK for .NET

The `cs-sdk-dotnet` repository contains software functions for processing case-based surveillance data. All source code is written in C# and targets .NET Standard 2.1. Supported functions include:

- Content validation for case notifications in the following file formats:
    - Json
    - HL7 v2.5.1
- Data format converter:
    - HL7 v2.5.1 to Json
- C# model objects for Message Mapping Guides (MMGs)
- Debatcher for HL7 v2.5.1 batch files

## Usage

### Validation and Conversion
Using the validator and converter in .NET applications is straightforward:

```csharp
// create services for getting MMGs and vocabulary
IVocabularyService vocabService = new InMemoryVocabularyService();
IMmgService mmgService = new InMemoryMmgService();

// an example (fake) HL7v2 message
string hl7v2message = @"MSH|^~\&|SAN^2.16.840.1.1.nnnn^ISO|SF^2.16.840.1.1.nnnn^ISO|PHINCDS^2.16.840.1.1.4.3.2.10^ISO|PHIN^2.16.840.1.1^ISO|20141225120030.1234-0500||ORU^R01^ORU_R01|CONSYPH_V1_0_TM_TC06|T|2.5.1|||||||||NOTF_ORU_v3.0^PHINProfileID^2.16.840.1.1.4.10.3^ISO~Generic_MMG_V2.0^PHINMsgMapID^2.16.840.1.1.4.10.4^ISO~CongenitalSyphilis_MMG_V1.0^PHINMsgMapID^2.16.840.1.1.4.10.4^ISO
PID|1||CONSYPH_TC06^^^SAN&2.16.840.1.1.nnnn&ISO||~^^^^^^S||20130101|M||2054-5^Black or African American^CDCREC|^^^22^71292^^^^22073|||||||||||2186-5^Not Hispanic or Latino^CDCREC
OBR|1||CONSYPH_TC06^SAN^2.16.840.1.1.nnnn^ISO|68991-9^Epidemiologic Information^LN|||20131031170100|||||||||||||||20131031170100|||F||||||10316^Syphilis, Congenital^NND
OBX|1|CWE|78746-5^Country of Birth^LN||USA^United States^ISO3166_1||||||F";

// Convert the HL7v2 message to JSON
IConverter converter = new HL7v2ToJsonConverter(vocabService, mmgService);
ConversionResult conversionResult = converter.Convert(hl7v2message);

// Validate the JSON payload against the MMG
IContentValidator validator = new JsonContentValidator(vocabService, mmgService);
ValidationResult validationResult = validator.ValidateMessage(json);
```

> The `cs-sdk-dotnet` library requires dependency injection for the converter and validator. Doing so enables these functions to vary how they request MMGs and public health vocabulary. It also avoids hard-coding API calls or database access calls in the business logic. Downstream users of this SDK can then provide their own implementations.

### Debatching

The HL7v2 debatcher is somewhat more complex in that it requires a callback function. The callback function is executed each time a message is debatched.

```csharp
IDebatchHandler handler = new StringDebatchHandler(DebatchCallback); // DebatchCallback() is executed for every message in the batch
HL7v2Debatcher debatcher = new HL7v2Debatcher(); // create the debatcher
DebatchResult result = debatcher.Debatch(batchmessage, handler, "1234"); // run the debatching operation
```

The `StringDebatchHandler` might look like this, in simplest form:

```csharp
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
```

The `DebatchCallback()` function might look like this:

```csharp
private static void DebatchCallback(ReadOnlySpan<char> message, MessageDebatchMetadata metadata)
{
    string hl7v2message = message.ToString();
    ConversionResult conversionResult = converter.Convert(hl7v2message, metadata.TransactionId);
    ValidationResult validationResult = validator.Validate(conversionResult.Content, metadata.TransactionId);
}
```

### Example processing pipeline

A public health surveillance use case will likely require these functions to run in a defined sequence. For example, one could author an HL7 v2.5.1 pipeline that runs the functions in the following order:

1. Debatch the HL7 v2.5.1 payload
2. Convert HL7 v2.5.1 to Json (note that basic structural validation of HL7 v2.5.1 occurs by virtue of the conversion)
3. Validate the content of the Json document per the Message Mapping Guide

A basic C# pipeline implementation might look as follows:

```cs
private static void RunPipeline(string batchmessage)
{
    IDebatchHandler handler = new StringDebatchHandler(DebatchCallbackPipeline);
    HL7v2Debatcher debatcher = new HL7v2Debatcher();
    DebatchResult result = debatcher.Debatch(batchmessage, handler, "1234");

    // persist the debatch results in a database
}

private static IConverter converter = new HL7v2ToJsonConverter();
private static IContentValidator validator = new JsonContentValidator();

private static void DebatchCallbackPipeline(ReadOnlySpan<char> message, MessageDebatchMetadata metadata)
{
    string hl7v2message = message.ToString();
    ConversionResult conversionResult = converter.Convert(hl7v2message, metadata.TransactionId);
    ValidationResult validationResult = validator.Validate(conversionResult.Content, metadata.TransactionId);

    // persist the conversion and validation results
    // persist the converted Json
}

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
```

The example pipeline code shown above will debatch 10,000 HL7v2 messages, convert them to Json, and then validate the Json in about 20 seconds. This is single-threaded performance, meaning a scale factor of 1, on 15W TDP Intel CPU.

### HL7v2 to CSV converter

The SDK can optionally convert case surveillance messages in HL7v2 format to CSV format, where the CSV columns are the data elements in the message mapping guide:

```cs
IConverter converter = new HL7v2ToCsvConverter(new MmgCsvTemplateGenerator(), new HL7v2ToJsonConverter(), true);
ConversionResult result = converter.Convert(hl7v2message, "1234");
string csv = result.Content;
```

This is an alternative to the SDK's conversion of HL7v2 messages to Json.

## Disclaimer on Case Notification Data

This repository contains case notification data in various places, including in the [tests/](tests/) folder and in the README.md file. Such data are necessary to both verify the correct functionality of the software as well as provide documentation to users about how to use the software. These data are synthetically generated, representing fictional people and events only. Any similarity to real persons is unintentional.

The primary source of synthetic case notification data used in this repository is the CDC's publicly-available [Message Mapping Guides](https://ndc.services.cdc.gov/message-mapping-guides/) web page. Each MMG listed on that page contains a ZIP file consisting of a Test Message Package. This test message package contains a handful of synthetically generated, raw HL7v2 messages developed by the CDC that demonstrate how to correctly map every data element in the MMG to the HL7v2 format. In some cases, however, the authors of this library created their own synthetic case notifications or modified the CDC-authored test messages to test specific software functionality.

## Public Domain Standard Notice
This repository constitutes a work of the United States Government and is not
subject to domestic copyright protection under 17 USC § 105. This repository is in
the public domain within the United States, and copyright and related rights in
the work worldwide are waived through the [CC0 1.0 Universal public domain dedication](https://creativecommons.org/publicdomain/zero/1.0/).
All contributions to this repository will be released under the CC0 dedication. By
submitting a pull request you are agreeing to comply with this waiver of
copyright interest.

## License Standard Notice
The repository utilizes code licensed under the terms of the Apache Software
License and therefore is licensed under ASL v2 or later.

This source code in this repository is free: you can redistribute it and/or modify it under
the terms of the Apache Software License version 2, or (at your option) any
later version.

This source code in this repository is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the Apache Software License for more details.

You should have received a copy of the Apache Software License along with this
program. If not, see http://www.apache.org/licenses/LICENSE-2.0.html

The source code forked from other open source projects will inherit its license.

## Privacy Standard Notice
This repository contains only non-sensitive, publicly available data and
information. All material and community participation is covered by the
[Disclaimer](https://github.com/CDCgov/template/blob/master/DISCLAIMER.md)
and [Code of Conduct](https://github.com/CDCgov/template/blob/master/code-of-conduct.md).
For more information about CDC's privacy policy, please visit [http://www.cdc.gov/other/privacy.html](https://www.cdc.gov/other/privacy.html).

## Contributing Standard Notice
Anyone is encouraged to contribute to the repository by [forking](https://help.github.com/articles/fork-a-repo)
and submitting a pull request. (If you are new to GitHub, you might start with a
[basic tutorial](https://help.github.com/articles/set-up-git).) By contributing
to this project, you grant a world-wide, royalty-free, perpetual, irrevocable,
non-exclusive, transferable license to all users under the terms of the
[Apache Software License v2](https://www.apache.org/licenses/LICENSE-2.0.html) or
later.

All comments, messages, pull requests, and other submissions received through
CDC including this GitHub page may be subject to applicable federal law, including but not limited to the Federal Records Act, and may be archived. Learn more at [https://www.cdc.gov/other/privacy.html](https://www.cdc.gov/other/privacy.html).

## Records Management Standard Notice
This repository is not a source of government records, but is a copy to increase
collaboration and collaborative potential. All government records will be
published through the [CDC web site](https://www.cdc.gov).

## Additional Standard Notices
Please refer to [CDC's Template Repository](https://github.com/CDCgov/template)
for more information about [contributing to this repository](https://github.com/CDCgov/template/blob/master/CONTRIBUTING.md),
[public domain notices and disclaimers](https://github.com/CDCgov/template/blob/master/DISCLAIMER.md),
and [code of conduct](https://github.com/CDCgov/template/blob/master/code-of-conduct.md).