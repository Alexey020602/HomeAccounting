using System.Text.Json.Serialization;
using Receipts.Contracts;
using Shared.Model;

namespace Reports.Core;

// public sealed class Report
// {
//     public required DateRange DateRange { get; init; }
//     public required IReadOnlyList<Category> Categories { get; init; }
//     [JsonIgnore]
//     public int PennySum => Categories.Sum(c => c.PennySum);
//     public Sum Sum => new Sum(PennySum);
// }