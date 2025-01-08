using System.Text.Json.Serialization;

namespace FnsChecksApi.Dto.Fns;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "code")]
[JsonDerivedType(typeof(Root), 1)]
[JsonDerivedType(typeof(BadAnswerReceipt), 3)]
public abstract record Receipt(int Code, Request Request);